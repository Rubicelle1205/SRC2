using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
    [LogAttribute(LogActionChineseName.霸凌事件管理)]
    public class EventBullyingMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        EventBullyingMangDataAccess dbAccess = new EventBullyingMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public EventBullyingMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlBullyingMainClass = dbAccess.GetddlBullyingMainClass();
            ViewBag.ddlBullyingSecondClass = dbAccess.GetddlBullyingSecondClass();
            ViewBag.ddlAcceptStatus = dbAccess.GetddlAcceptStatus();
            ViewBag.ddlCaseFinish = dbAccess.GetddlCaseFinish();

            EventBullyingMangViewModel vm = new EventBullyingMangViewModel();
            vm.ConditionModel = new EventBullyingMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, EventBullyingMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlBullyingMainClass = dbAccess.GetddlBullyingMainClass();
            ViewBag.ddlBullyingSecondClass = dbAccess.GetddlBullyingSecondClass();
            ViewBag.ddlAcceptStatus = dbAccess.GetddlAcceptStatus();
            ViewBag.ddlCaseFinish = dbAccess.GetddlCaseFinish();
            ViewBag.ddlBullyingEventStatus = dbAccess.GetddlBullyingEventStatus();

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.BullyingEventDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (vm.EditModel != null)
            {
                vm.EditModel.LstVictim = dbAccess.GetLstVictimData(vm.EditModel.CaseID);
                vm.EditModel.LstEventData = dbAccess.GetEventData(vm.EditModel.CaseID);

                ViewBag.ddlBullyingSecondClass = dbAccess.GetddlSecondClass(vm.EditModel.BullyingMainClass);

            }
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(EventBullyingMangViewModel vm)
        {
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
        public IActionResult EditOldData(EventBullyingMangViewModel vm)
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

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldEventData(EventBullyingMangViewModel vm)
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

        [Log(LogActionChineseName.匯入)]
        public IActionResult Upload()
        {
            EventBullyingMangViewModel vm = new EventBullyingMangViewModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadTemplate()
        {
            string FileName = "霸凌事件管理_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public async Task<IActionResult> ImportExcel(EventBullyingMangViewModel vm)
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

                if (!vm.File.FileName.Contains("霸凌事件管理"))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List<EventBullyingMangImportModel> LstExcel = new List<EventBullyingMangImportModel>();

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

                            if (!CanGo)
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:必填資料未填寫");
                                return Json(vmRtn);
                            }
                        }

                        if (row != null && CanGo)
                        {
                            string CaseID = "";
                            string MainClass = "";
                            string SecondClass = "";
                            string AcceptStatus = "";
                            string CaseFinishClass = "";

                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlCaseID = dbAccess.GetddlCaseID();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlBullyCaseID = dbAccess.GetddlBullyCaseID(row.GetCell(0)?.ToString().TrimStartAndEnd());
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

                            if (LstddlBullyCaseID.Any(m => m.Text == row.GetCell(1)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:校安事件編號{0}內已經存在霸凌號{1}", row.GetCell(0)?.ToString().TrimStartAndEnd(), row.GetCell(1)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!LstddlMainClass.Any(m => m.Text == row.GetCell(4)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗: 查無霸稜事件主類別 {0}", row.GetCell(4)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                MainClass = LstddlMainClass.Where(m => m.Text == row.GetCell(4)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!LstddlSecondClass.Any(m => m.Text == row.GetCell(5)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗: 查無霸稜事件次類別 {0}", row.GetCell(5)?.ToString().TrimStartAndEnd());
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
                                EventBullyingMangImportModel excel = new EventBullyingMangImportModel
                                {
                                    CaseID = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                    SubCaseID = row.GetCell(1)?.StringCellValue.TrimStartAndEnd(),
                                    OccurTime = DateTime.Parse(row.GetCell(2)?.StringCellValue),
                                    KnowTime = DateTime.Parse(row.GetCell(3).StringCellValue),
                                    BullyingMainClass = MainClass,
                                    BullyingSecondClass = SecondClass,
                                    AcceptStatus = AcceptStatus,
                                    AcceptTime = DateTime.Parse(row.GetCell(7).StringCellValue),
                                    CaseStatus = CaseFinishClass,
                                    CaseFinishDateTime = DateTime.Parse(row.GetCell(9)?.StringCellValue),
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
                        else
                        {
                            vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                            vmRtn.ErrorMsg = "上傳失敗";
                            return Json(vmRtn);
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
        public IActionResult ExportSearchResult(EventBullyingMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.霸凌事件管理, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(EventBullyingMangExcelHeaderModel).GetProperties();

                //設定欄位
                IRow headerRow = sheet.CreateRow(0);

                XSSFCellStyle headStyle = ExcelUtil.GetDefaultHeaderStyle(workbook);

                for (int i = 0; i <= properties.Length - 1; i++)
                {
                    var displayAttribute = (DisplayNameAttribute)properties[i].GetCustomAttribute(typeof(DisplayNameAttribute));
                    var displayName = displayAttribute?.DisplayName ?? properties[i].Name;

                    headerRow.CreateCell(i).SetCellValue(displayName);

                    foreach (ICell cell in headerRow.Cells)
                        cell.CellStyle = headStyle;

                }

                XSSFCellStyle contentStyle = ExcelUtil.GetDefaultContentStyle(workbook);

                //設定資料
                for (int i = 0; i <= vm.ResultModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellValue(vm.ResultModel[i].CaseID);
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].SubCaseID);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].OccurTime != null ? vm.ResultModel[i].OccurTime.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].KnowTime != null ? vm.ResultModel[i].KnowTime.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(4).SetCellValue(vm.ResultModel[i].BullyingMainClassText);
                    dataRow.CreateCell(5).SetCellValue(vm.ResultModel[i].BullyingSecondClassText);
                    dataRow.CreateCell(6).SetCellValue(vm.ResultModel[i].AcceptStatusText);
                    dataRow.CreateCell(7).SetCellValue(vm.ResultModel[i].AcceptTime != null ? vm.ResultModel[i].AcceptTime.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(8).SetCellValue(vm.ResultModel[i].CaseStatusText);
                    dataRow.CreateCell(9).SetCellValue(vm.ResultModel[i].CaseFinishDateTime != null ? vm.ResultModel[i].CaseFinishDateTime.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(10).SetCellValue(vm.ResultModel[i].Created != null ? vm.ResultModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm") : "");

                    foreach (ICell cell in dataRow.Cells)
                        cell.CellStyle = contentStyle;
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
            EventBullyingMangViewModel vm = new EventBullyingMangViewModel();
            vm.EditModel = dbAccess.GetEditData(CaseID);

            ViewBag.ddlBullyingSecondClass = dbAccess.GetddlSecondClass(MainClass);

            return PartialView("_SecondClassPartial", vm);
        }
    }
}
