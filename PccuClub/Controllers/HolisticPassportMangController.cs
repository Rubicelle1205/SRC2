using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NuGet.Protocol;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.全人學習護照管理)]
    public class HolisticPassportMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        HolisticPassportMangDataAccess dbAccess = new HolisticPassportMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public HolisticPassportMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlAllSchoolYear = dbAccess.GetSchoolYear(2);
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");

            HolisticPassportMangViewModel vm = new HolisticPassportMangViewModel();
            vm.ConditionModel = new HolisticPassportMangConditionModel();
            vm.ConditionModel.LstColumnDataModel = dbAccess.GetHolisticPassportResultColumnData().ToList();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlAllClub = dbAccess.GetAllClubID();
            ViewBag.ddlAllActName = dbAccess.GetAllActName();
            ViewBag.ddlHolisticMainClass = dbAccess.GetddlHolisticMainClass();
            ViewBag.ddlHolisticSecondClass = dbAccess.GetddlHolisticSecondClass();
            ViewBag.ddlHolisticThridClass = dbAccess.GetddlHolisticThirdClass();
            ViewBag.ddlActInOrOut = dbAccess.GetddlActInOrOut();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");

            HolisticPassportMangViewModel vm = new HolisticPassportMangViewModel();
            vm.CreateModel = new HolisticPassportMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, HolisticPassportMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAllClub = dbAccess.GetAllClubID();
            ViewBag.ddlAllActName = dbAccess.GetAllActName();
            ViewBag.ddlHolisticMainClass = dbAccess.GetddlHolisticMainClass();
            ViewBag.ddlHolisticSecondClass = dbAccess.GetddlHolisticSecondClass();
            ViewBag.ddlHolisticThridClass = dbAccess.GetddlHolisticThirdClass();
            ViewBag.ddlActInOrOut = dbAccess.GetddlActInOrOut();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");
            
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(HolisticPassportMangViewModel vm)
        {
            var allLegalColumns = dbAccess.GetHolisticPassportResultColumnData().ToList();
            vm.ConditionModel.LstColumnDataModel = allLegalColumns;

            if (!string.IsNullOrEmpty(vm.ConditionModel.SelectedColumns))
            {
                var rawSelected = vm.ConditionModel.SelectedColumns.Split(',');

                // 只保留「確實存在於資料庫定義中」的欄位名稱 (白名單比對)
                var activeColumns = allLegalColumns.Where(x => rawSelected.Contains(x.ColumnValue)).ToList();

                // 3. 【產生安全字串】用於 SQL 查詢
                // 加上 [] 可以防止欄位名稱與 SQL 關鍵字衝突
                var safeFieldsForSql = string.Join(", ", activeColumns.Select(x => $"[{x.ColumnValue}]"));

                var orderedColumns = rawSelected
    .Select(val => allLegalColumns.FirstOrDefault(x => x.ColumnValue == val))
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
        public IActionResult SaveNewData(HolisticPassportMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.InsertData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
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
        public IActionResult EditOldData(HolisticPassportMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateData(vm, LoginUser);

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
                    vmRtn.ErrorCode =  (int)DBActionChineseName.失敗;
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
        public IActionResult ExportSearchResult(HolisticPassportMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.全人學習護照管理, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                // 1. 取得欄位定義白名單 (確保能從 ColumnValue 對應到中文名稱)
                var allLegalColumns = dbAccess.GetHolisticPassportResultColumnData().ToList();

                if (string.IsNullOrEmpty(vm.ConditionModel.SelectedColumns))
                {
                    // 將 IsDefault 的欄位用逗號串接回字串
                    var defaultCols = allLegalColumns
                                        .Where(x => x.IsDefault)
                                        .Select(x => x.ColumnValue);

                    vm.ConditionModel.SelectedColumns = string.Join(",", defaultCols);
                }

                // 2. 解析順序字串 (由前端傳回的 "ActName,ClubID...")
                var rawSelected = (vm.ConditionModel.SelectedColumns ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // 3. 依照順序重組欄位物件清單
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
                var itemType = typeof(HolisticPassportMangResultModel); // 你的資料模型類別

                for (int i = 0; i < vm.ResultModel.Count; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);
                    var rowData = vm.ResultModel[i];

                    for (int j = 0; j < activeColumns.Count; j++)
                    {
                        var colValue = activeColumns[j].ColumnValue;
                        var cell = dataRow.CreateCell(j);

                        if (colValue == "ActVerify") 
                            colValue = "ActVerifyText";

                        // 使用 Reflection (反射) 依照 ColumnValue 動態抓取屬性值
                        var prop = itemType.GetProperty(colValue);
                        object val = prop?.GetValue(rowData, null);

                        // 針對特定欄位或型別做處理 (例如日期)
                        if (val is DateTime dateVal)
                        {
                            cell.SetCellValue(dateVal.ToString("yyyy/MM/dd HH:mm"));
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



        [Log(LogActionChineseName.取得樓館選單)]
        [ValidateInput(false)]
        public IActionResult InitBuildSelect(string PlaceSource)
        {
            if (PlaceSource == "01")
            {
                ViewBag.ddlBuild = dbAccess.GetBuild();
            }

            HolisticPassportMangViewModel vm = new HolisticPassportMangViewModel();
            vm.CreateModel = new HolisticPassportMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;

            return PartialView("_PlaceDataPartial", vm);
        }

        [Log(LogActionChineseName.取得場地選單)]
        [ValidateInput(false)]
        public IActionResult InitPlaceSelect(string PlaceSource, string Buildid)
        {
            ViewBag.ddlBuild = dbAccess.GetBuild();
            ViewBag.ddlPlace = dbAccess.GetPlace(PlaceSource, Buildid);


            HolisticPassportMangViewModel vm = new HolisticPassportMangViewModel();
            vm.CreateModel = new HolisticPassportMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;
            vm.CreateModel.BuildID = Buildid;


            return PartialView("_PlaceDataPartial", vm);
        }


    }
}
