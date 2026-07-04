using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PccuClub.WebAuth;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebAuth.Entity;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.社團基本資料)]
    public class ClubMangController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubMangDataAccess dbAccess = new ClubMangDataAccess();
        AuthManager auth = new AuthManager();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();
            ViewBag.ddlClubClass = dbAccess.GetAllClubClass();

            ClubMangViewModel vm = new ClubMangViewModel();
            vm.ConditionModel = new ClubMangConditionModel();
            vm.ConditionModel.LstColumnDataModel = dbAccess.GetDefaultColumnData().ToList();

            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            //ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();
            ViewBag.ddlClubClass = dbAccess.GetAllClubClass();
            ViewBag.ddlRoleClass = dbAccess.GetAllRoleClass();
            ViewBag.ddlFrontShow = dbAccess.GetFrontShow();

            ClubMangViewModel vm = new ClubMangViewModel();
            vm.CreateModel = new ClubMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ClubMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            //ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();
            ViewBag.ddlClubClass = dbAccess.GetAllClubClass();
            ViewBag.ddlRoleClass = dbAccess.GetAllRoleClass();
            ViewBag.ddlFrontShow = dbAccess.GetFrontShow();

            vm.EditModel = dbAccess.GetEditData(submitBtn);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubMangViewModel vm)
        {
            var allLegalColumns = dbAccess.GetDefaultColumnData().ToList();
            vm.ConditionModel.LstColumnDataModel = allLegalColumns;

            if (!string.IsNullOrEmpty(vm.ConditionModel.SelectedColumns))
            {
                var rawSelected = vm.ConditionModel.SelectedColumns.Split(',');

                // 只保留「確實存在於資料庫定義中」的欄位名稱 (白名單比對)
                var activeColumns = allLegalColumns.Where(x => rawSelected.Contains(x.ColumnValue)).ToList();

                // 3. 【產生安全字串】用於 SQL 查詢
                // 加上 [] 可以防止欄位名稱與 SQL 關鍵字衝突
                var safeFieldsForSql = string.Join(", ", activeColumns.Select(x => $"[{x.ColumnValue}]"));

                var orderedColumns = rawSelected.Select(val => allLegalColumns
                    .FirstOrDefault(x => x.ColumnValue == val))
                    .Where(x => x != null)
                    .ToList();

                // 將過濾後的合法清單傳給 View 渲染標頭
                ViewBag.ActiveColumns = orderedColumns;

                // 將安全字串存入 ConditionModel，供 dbAccess 內部組 SQL 使用
                vm.ConditionModel.SafeSqlColumns = safeFieldsForSql;
            }
            else
            {
                // 如果完全沒選，可以給予預設必選欄位
                ViewBag.ActiveColumns = allLegalColumns.Where(x => x.IsDefault).ToList();
            }

            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> SaveNewData(ClubMangViewModel vm)
        {
            try
            {

                dbAccess.DbaInitialTransaction();

                List<ClubMangResultModel> ResultModel = dbAccess.ChkClubExist(vm);

                if (ResultModel.Count > 0)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = $"新增失敗，社團編號:{vm.CreateModel.ClubId} 已存在";
                    return Json(vmRtn);
                }


                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("LogoPath"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.LogoPath");

                            string strFilePath = await upload.UploadFileAsync("LogoPath", file);

                            vm.CreateModel.LogoPath = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("ActImgPath"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.ActImgPath");

                            string strFilePath = await upload.UploadFileAsync("ActImgPath", file);

                            vm.CreateModel.ActImgPath = strFilePath;
                        }
                    }
                }

                string EncryptPw = String.Empty;

                if (!string.IsNullOrEmpty(vm.CreateModel.Pwd))
                    EncryptPw = auth.EncryptionText(vm.CreateModel.Pwd);

                var dbResult = dbAccess.InsertData(EncryptPw, vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.UpdateRole(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "新增失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> EditOldData(ClubMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("LogoPath"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.LogoPath");

                            string strFilePath = await upload.UploadFileAsync("LogoPath", file);

                            vm.EditModel.LogoPath = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("ActImgPath"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.ActImgPath");

                            string strFilePath = await upload.UploadFileAsync("ActImgPath", file);

                            vm.EditModel.ActImgPath = strFilePath;
                        }
                    }
                }

                string EncryptPw = String.Empty;

                if (!string.IsNullOrEmpty(vm.EditModel.Pwd))
                    EncryptPw = auth.EncryptionText(vm.EditModel.Pwd);

                var dbResult = dbAccess.UpdateData(EncryptPw, vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.UpdateRole(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [Log(LogActionChineseName.刪除)]
        [ValidateInput(false)]
        public IActionResult Delete(string Ser)
        {
            try
            {

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.DeletetData(Ser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "刪除失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(ClubMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.社團基本資料, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                // 1. 取得欄位定義白名單 (確保能從 ColumnValue 對應到中文名稱)
                var allLegalColumns = dbAccess.GetDefaultColumnData().ToList();

                if (string.IsNullOrEmpty(vm.ConditionModel.SelectedColumns))
                {
                    // 將 IsDefault 的欄位用逗號串接回字串
                    var defaultCols = allLegalColumns
                                        .Where(x => x.IsDefault)
                                        .Select(x => x.ColumnValue);

                    vm.ConditionModel.SelectedColumns = string.Join(",", defaultCols);
                }


                var rawSelected = (vm.ConditionModel.SelectedColumns ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var activeColumns = rawSelected
                    .Select(val => allLegalColumns.FirstOrDefault(x => x.ColumnValue == val))
                    .Where(x => x != null)
                    .ToList();

                IWorkbook workbook = new XSSFWorkbook();

                // 動態設定寬度 (這裡可以根據 activeColumns 的數量給予預設值)
                List<int> LstWidth = activeColumns.Select(x => 25).ToList();
                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                // --- 設定標頭 (Header Row) ---
                IRow headerRow = sheet.CreateRow(0);
                XSSFCellStyle headStyle = ExcelUtil.GetDefaultHeaderStyle(workbook);

                for (int i = 0; i < activeColumns.Count; i++)
                {
                    // 直接使用白名單定義的 ColumnName
                    headerRow.CreateCell(i).SetCellValue(activeColumns[i].ColumnName);
                    headerRow.GetCell(i).CellStyle = headStyle;
                }

                // --- 設定資料 (Content Rows) ---
                XSSFCellStyle contentStyle = ExcelUtil.GetDefaultContentStyle(workbook);

                for (int i = 0; i < vm.ResultModel.Count; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);
                    var rowData = vm.ResultModel[i];
                    if (rowData == null) continue; // 防呆

                    // 💡 優化重點 1：直接動態獲取這筆資料的實際型別
                    var actualType = rowData.GetType();

                    for (int j = 0; j < activeColumns.Count; j++)
                    {
                        var colValue = activeColumns[j].ColumnValue;
                        var cell = dataRow.CreateCell(j);

                        // 💡 優化重點 2：使用實際型別來尋找屬性，並加上大小寫忽略的防呆
                        var prop = actualType.GetProperty(colValue, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                        object val = prop?.GetValue(rowData, null);

                        // 針對特定欄位或型別做處理 (例如日期)
                        if (val is DateTime dateVal)
                        {
                            cell.SetCellValue(dateVal.ToString("yyyy/MM/dd HH:mm:ss"));
                        }
                        else
                        {
                            cell.SetCellValue(val?.ToString() ?? "");
                        }

                        cell.CellStyle = contentStyle;
                    }
                }

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms, true);
                ms.Flush();
                ms.Position = 0;

                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            }

            AlertMsg.Add("無資料已供匯出");
            return Redirect("Index");

        }



        #region Method

        #endregion
    }
}
