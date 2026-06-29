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
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.性平事件管理)]
    public class EventGenderMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        EventGenderMangDataAccess dbAccess = new EventGenderMangDataAccess();
        MailUtil mail = new MailUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public EventGenderMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlGenderMainClass = dbAccess.GetddlGenderMainClass();
            ViewBag.ddlGenderSecondClass = dbAccess.GetddlGenderSecondClass();
            ViewBag.ddlAcceptStatus = dbAccess.GetddlAcceptStatus();
            ViewBag.ddlCaseFinish = dbAccess.GetddlCaseFinish();

            EventGenderMangViewModel vm = new EventGenderMangViewModel();
            vm.ConditionModel = new EventGenderMangConditionModel();
            vm.ConditionModel.LstColumnDataModel = dbAccess.GetDefaultColumnData().ToList();

            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, EventGenderMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlGenderMainClass = dbAccess.GetddlGenderMainClass();
            ViewBag.ddlGenderSecondClass = dbAccess.GetddlGenderSecondClass();
            ViewBag.ddlAcceptStatus = dbAccess.GetddlAcceptStatus();
            ViewBag.ddlCaseFinish = dbAccess.GetddlCaseFinish();
            ViewBag.ddlGenderEventStatus = dbAccess.GetddlGenderEventStatus();

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.GenderEventDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            if (vm.EditModel != null)
            {
                vm.EditModel.LstVictim = dbAccess.GetLstVictimData(vm.EditModel.CaseID);
                vm.EditModel.LstEventData = dbAccess.GetEventData(vm.EditModel.CaseID);

                ViewBag.ddlGenderSecondClass = dbAccess.GetddlSecondClass(vm.EditModel.GenderMainClass);
            }
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(EventGenderMangViewModel vm)
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

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldData(EventGenderMangViewModel vm)
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

                //結案發送Mail
                if (vm.EditModel.CaseStatus == "01")
                {
                    string MailBody = GenMailBody(vm, LoginUser);
                    string CaseID = vm.EditModel.CaseID;
                    DataTable dtTeacher = dbAccess.GetSystemMember("03");

                    foreach (DataRow dr in dtTeacher.Rows)
                    {
                        mail.ExecuteSendMail(dr["EMail"].ToString(), string.Format("案件結案通知-{0}", CaseID), 
                            MailBody, System.Net.Mail.MailPriority.High, null);
                    }
                }

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


        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldEventData(EventGenderMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateEventData(vm, LoginUser, vm.EditModel.CaseID);

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

        [Log(LogActionChineseName.匯入)]
        public IActionResult Upload()
        {
            EventGenderMangViewModel vm = new EventGenderMangViewModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadTemplate()
        {
            string FileName = "性平事件管理_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public async Task<IActionResult> ImportExcel(EventGenderMangViewModel vm)
        {
            if (vm.File != null && vm.File.Length > 0)
            {
                string fileExtension = Path.GetExtension(vm.File.FileName);

                if (fileExtension != ".xlsx")
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案格式錯誤";
                    return Json(vmRtn);
                }

                if (!vm.File.FileName.Contains("性平事件管理_template"))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List<EventGenderMangImportModel> LstExcel = new List<EventGenderMangImportModel>();

                using (Stream stream = vm.File.OpenReadStream())
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);
                    List<string> LstSNo = new List<string>();

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        row.GetCell(0).SetCellType(CellType.String);
                        bool CanGo = true;

                        for (int j = 0; j <= row.Count() - 1; j++)
                        {
                            if (j != 7 && j != 9)
                            {
                                string Celldata = row.GetCell(j)?.ToString();
                                if (string.IsNullOrEmpty(Celldata))
                                    CanGo = false;
                            }
                        }

                        if (row != null && CanGo)
                        {
                            string CaseID = "";
                            string MainClass = "";
                            string SecondClass = "";
                            string AcceptStatus = "";
                            string CaseFinishClass = "";

                            //檢查一下
                            for (i = 0; i <= row.Count() - 1; i++)
                            {
                                if (i != 7 && i != 9)
                                {
                                    string str = row.GetCell(i)?.ToString();

                                    if (string.IsNullOrEmpty(str))
                                    {
                                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                        vmRtn.ErrorMsg = string.Format("檢核資料失敗:必填資料未填寫");
                                        return Json(vmRtn);
                                    }
                                }
                            }

                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlCaseID = dbAccess.GetddlCaseID();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlGenderCaseID = dbAccess.GetddlGenderCaseID(row.GetCell(0)?.ToString().TrimStartAndEnd());
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlMainClass = dbAccess.GetddlMainClass();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlSecondClass = dbAccess.GetddlSecondClass();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlAcceptStatus = dbAccess.GetddlAcceptStatus();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlCaseFinishClass = dbAccess.GetddlCaseFinishClass();


                            if (!LstddlCaseID.Any(m => m.Text == row.GetCell(0)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:校安事件編號{0}不存在", row.GetCell(0)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (LstddlGenderCaseID.Any(m => m.Text == row.GetCell(1)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:校安事件編號{0}內已經存在性平號{1}", row.GetCell(0)?.ToString().TrimStartAndEnd(), row.GetCell(1)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!LstddlMainClass.Any(m => m.Text == row.GetCell(4)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗: 查無性平事件主類別 {0}", row.GetCell(4)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                MainClass = LstddlMainClass.Where(m => m.Text == row.GetCell(4)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!LstddlSecondClass.Any(m => m.Text == row.GetCell(5)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗: 查無性平事件次類別 {0}", row.GetCell(5)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                SecondClass = LstddlSecondClass.Where(m => m.Text == row.GetCell(5)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!LstddlAcceptStatus.Any(m => m.Text == row.GetCell(6)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗: 查無受理狀態 {0}", row.GetCell(6)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                AcceptStatus = LstddlAcceptStatus.Where(m => m.Text == row.GetCell(6)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!LstddlCaseFinishClass.Any(m => m.Text == row.GetCell(8)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗: 查無是否結案 {0}", row.GetCell(8)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                CaseFinishClass = LstddlCaseFinishClass.Where(m => m.Text == row.GetCell(8)?.ToString()).FirstOrDefault().Value;
                            }

                            try
                            {
                                EventGenderMangImportModel excel = new EventGenderMangImportModel
                                {
                                    CaseID = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                    SubCaseID = row.GetCell(1)?.StringCellValue.TrimStartAndEnd(),
                                    OccurTime = DateTime.Parse(row.GetCell(2)?.StringCellValue),
                                    KnowTime = DateTime.Parse(row.GetCell(3).StringCellValue),
                                    GenderMainClass = MainClass,
                                    GenderSecondClass = SecondClass,
                                    AcceptStatus = AcceptStatus,
                                    AcceptTime = string.IsNullOrEmpty(row.GetCell(7)?.StringCellValue) ? (DateTime?)null : DateTime.Parse(row.GetCell(7).StringCellValue),
                                    CaseStatus = CaseFinishClass,
                                    CaseFinishDateTime = string.IsNullOrEmpty(row.GetCell(9)?.StringCellValue) ? (DateTime?)null : DateTime.Parse(row.GetCell(9).StringCellValue),
                                    Created = DateTime.Now
                                };

                                LstExcel.Add(excel);
                            }
                            catch (Exception ex)
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = "上傳失敗，" + ex.Message;
                                return Json(vmRtn);
                            }
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();

                if (LstExcel.Count > 0)
                {
                    var dbResult = dbAccess.ImportData(LstExcel, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "上傳失敗";
                    }
                }

                dbAccess.DbaCommit();
            }
            else
            {
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "請選擇檔案上傳";
            }

            return Json(vmRtn);
        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(EventGenderMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.性平事件管理, DateTime.Now.ToString("yyyyMMdd"));
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
                var itemType = typeof(EventGenderMangResultModel); // 你的資料模型類別

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

        [ValidateInput(false)]

        public IActionResult GetSecond(string MainClass, string Source, string CaseID)
        {
            EventGenderMangViewModel vm = new EventGenderMangViewModel();
            vm.EditModel = dbAccess.GetEditData(CaseID);

            ViewBag.ddlGenderSecondClass = dbAccess.GetddlSecondClass(MainClass);

            return PartialView("_SecondClassPartial", vm);
        }


        private string GenMailBody(EventGenderMangViewModel vm, UserInfo loginUser)
        {
            string str = string.Empty;

            str = $@"<p>案件結案通知-{vm.EditModel.CaseID}</p>
                    <p>性平號：{vm.EditModel.SubCaseID}已結案，結案人：{loginUser.UserName}</p>";

            return str;
        }
    }
}
