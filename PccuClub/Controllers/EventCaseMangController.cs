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
    [LogAttribute(LogActionChineseName.校安事件管理)]
    public class EventCaseMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        EventCaseMangDataAccess dbAccess = new EventCaseMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public EventCaseMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        #region 校安事件


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlSecondClass = dbAccess.GetddlSecondClass();
            ViewBag.ddlCaseFinish = dbAccess.GetddlCaseFinishClass();

            EventCaseMangViewModel vm = new EventCaseMangViewModel();
            vm.ConditionModel = new EventCaseMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlSecondClass = dbAccess.GetddlSecondClass();
            ViewBag.ddlCaseFinish = dbAccess.GetddlCaseFinishClass();
            ViewBag.ddlReferUnit = dbAccess.GetddlReferUnit();
            ViewBag.ddlYesOrNo = dbAccess.GetddlYesOrNo();
            ViewBag.ddlEventStatus = dbAccess.GetddlEventStatus();

            ViewBag.ddlSex = dbAccess.GetddlSex();
            ViewBag.ddlVictimTitle = dbAccess.GetddlVictimTitle();
            ViewBag.ddlVictimUnit = dbAccess.GetddlVictimUnit();
            ViewBag.ddlVictimLocation = dbAccess.GetddlVictimLocation();
            ViewBag.ddlVictimRole = dbAccess.GetddlVictimRole();
            ViewBag.ddlBirth = dbAccess.GetddlBirth();
            ViewBag.ddlVictimStatus = dbAccess.GetddlVictimStatus();

            EventCaseMangViewModel vm = new EventCaseMangViewModel();
            vm.CreateModel = new EventCaseMangCreateModel();
            vm.CreateModel.EventDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, EventCaseMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlSecondClass = dbAccess.GetddlSecondClass();
            ViewBag.ddlCaseFinish = dbAccess.GetddlCaseFinishClass();
            ViewBag.ddlReferUnit = dbAccess.GetddlReferUnit();
            ViewBag.ddlYesOrNo = dbAccess.GetddlYesOrNo();
            ViewBag.ddlEventStatus = dbAccess.GetddlEventStatus();

            ViewBag.ddlSex = dbAccess.GetddlSex();
            ViewBag.ddlVictimTitle = dbAccess.GetddlVictimTitle();
            ViewBag.ddlVictimUnit = dbAccess.GetddlVictimUnit();
            ViewBag.ddlVictimLocation = dbAccess.GetddlVictimLocation();
            ViewBag.ddlVictimRole = dbAccess.GetddlVictimRole();
            ViewBag.ddlBirth = dbAccess.GetddlBirth();
            ViewBag.ddlVictimStatus = dbAccess.GetddlVictimStatus();

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.EventDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (vm.EditModel != null)
            {
                vm.EditModel.LstVictim = dbAccess.GetLstVictimData(vm.EditModel.CaseID);
                vm.EditModel.LstEventData = dbAccess.GetEventData(vm.EditModel.CaseID);
            }

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(EventCaseMangViewModel vm)
        {
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
        public IActionResult SaveNewData(EventCaseMangViewModel vm)
        {
            try
            {
                if (!string.IsNullOrEmpty(vm.CreateModel.strLstVictim))
                    vm.CreateModel.LstVictim = TransToLstVictim(vm.CreateModel.strLstVictim);

                vm.CreateModel.LstEventData = TransToLstEventData(vm);

                dbAccess.DbaInitialTransaction();
                DataTable dt = new DataTable();

                var dbResult = dbAccess.InsertData(vm, LoginUser, out dt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                string CaseID = dt.QueryFieldByDT("CaseID");
                dbResult = dbAccess.InsertVictim(vm.CreateModel.LstVictim, LoginUser, CaseID);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.InsertEventData(vm.CreateModel.LstEventData, LoginUser, CaseID);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                string SecondClass = vm.CreateModel.SecondClass;
                List<string> LstSubSystem = dbAccess.GetNeedWriteSecond(SecondClass);
                
                dbResult = dbAccess.InsertSubSystem(LstSubSystem, vm.CreateModel.CaseID, LoginUser);

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
        public IActionResult EditOldData(EventCaseMangViewModel vm)
        {
            try
            {
                if (!string.IsNullOrEmpty(vm.EditModel.strLstVictim))
                    vm.EditModel.LstVictim = TransToLstVictim(vm.EditModel.strLstVictim);

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.InsertVictim(vm.EditModel.LstVictim, LoginUser, vm.EditModel.CaseID);

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
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldEventData(EventCaseMangViewModel vm)
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
            EventCaseMangViewModel vm = new EventCaseMangViewModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadTemplate()
        {
            string FileName = "校安事件管理_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public async Task<IActionResult> ImportExcel(EventCaseMangViewModel vm)
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

                if (!vm.File.FileName.Contains("校安事件管理_template"))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List<EventCaseImportMangResultModel> LstExcel = new List<EventCaseImportMangResultModel>();
                List<EventCaseMangChkSeconodModel> LstChkSecond = new List<EventCaseMangChkSeconodModel>();

                using (Stream stream = vm.File.OpenReadStream())
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);
                    List<string> LstSNo = new List<string>();

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        bool CanGo = true;

                        for (int j = 0; j <= row.Count() - 1; j++)
                        {
                            row.GetCell(j)?.SetCellType(CellType.String);
                            string Celldata = row.GetCell(j)?.ToString();
                            if (string.IsNullOrEmpty(Celldata))
                                CanGo = false;
                        }

                        if (row != null && CanGo)
                        {
                            string CaseID = "";
                            string MainClass = "";
                            string SecondClass = "";
                            string CaseFinishClass = "";

                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlCaseID = dbAccess.GetddlCaseID();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlMainClass = dbAccess.GetddlMainClass();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlSecondClass = dbAccess.GetddlSecondClass();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlCaseFinishClass = dbAccess.GetddlCaseFinishClass();

                            if (LstddlCaseID.Any(m => m.Text == row.GetCell(0)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:校安事件編號{0}已存在", row.GetCell(0)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!LstddlMainClass.Any(m => m.Text == row.GetCell(1)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(1)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                MainClass = LstddlMainClass.Where(m => m.Text == row.GetCell(1)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!LstddlSecondClass.Any(m => m.Text == row.GetCell(2)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(2)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                SecondClass = LstddlSecondClass.Where(m => m.Text == row.GetCell(2)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!LstddlCaseFinishClass.Any(m => m.Text == row.GetCell(5)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(5)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                CaseFinishClass = LstddlCaseFinishClass.Where(m => m.Text == row.GetCell(5)?.ToString()).FirstOrDefault().Value;
                            }
                            EventCaseImportMangResultModel excel = new EventCaseImportMangResultModel
                            {
                                CaseID = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                MainClass = MainClass,
                                SecondClass = SecondClass,
                                OccurTime = DateTime.Parse(row.GetCell(3)?.StringCellValue),
                                KnowTime = DateTime.Parse(row.GetCell(4).StringCellValue),
                                CaseStatus = CaseFinishClass,
                                CaseFinishDateTime = DateTime.Parse(row.GetCell(6)?.StringCellValue),
                                Created = DateTime.Now
                            };

                            LstExcel.Add(excel);

                            EventCaseMangChkSeconodModel chkSecond = new EventCaseMangChkSeconodModel();
                            chkSecond.CaseID = excel.CaseID;
                            chkSecond.SecondCode = excel.SecondClass;

                            LstChkSecond.Add(chkSecond);
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

                    foreach (EventCaseMangChkSeconodModel item in LstChkSecond)
                    {
                        string SecondClass = item.SecondCode;
                        List<string> LstSubSystem = dbAccess.GetNeedWriteSecond(SecondClass);

                        dbResult = dbAccess.InsertSubSystem(LstSubSystem, item.CaseID, LoginUser);

                        if (!dbResult.isSuccess)
                        {
                            dbAccess.DbaRollBack();
                            vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                            vmRtn.ErrorMsg = "新增失敗";
                            return Json(vmRtn);
                        }

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
        public IActionResult ExportSearchResult(EventCaseMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.校安事件管理, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 80, 80, 40, 40, 20, 40, 40 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(EventCaseMangExcelHeaderModel).GetProperties();

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
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].MainClassText);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].SecondClassText);
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].OccurTime != null ? vm.ResultModel[i].OccurTime.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(4).SetCellValue(vm.ResultModel[i].KnowTime != null ? vm.ResultModel[i].KnowTime.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(5).SetCellValue(vm.ResultModel[i].CaseStatusText);
                    dataRow.CreateCell(6).SetCellValue(vm.ResultModel[i].CaseFinishDateTime != null ? vm.ResultModel[i].CaseFinishDateTime.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(7).SetCellValue(vm.ResultModel[i].Created != null ? vm.ResultModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm") : "");

                    foreach (ICell cell in dataRow.Cells)
                        cell.CellStyle = contentStyle;
                }

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms, true);
                ms.Flush();
                ms.Position = 0;

                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            }

            return View("Index", vm);

        }

        private List<Victim> TransToLstVictim(string strLstVictim)
        {
            List<Victim> LstVictim = new List<Victim>();

            if (!string.IsNullOrEmpty(strLstVictim))
            {
                string[] arrRow = strLstVictim.Split("|");

                for (int i = 0; i <= arrRow.Length - 1; i++)
                { 
                    string[] arrdtRowData = arrRow[i].Split(":");
                    string[] arrRowData = arrdtRowData[1].Split(",");

                    Victim vic = new Victim();

                    vic.Sex = arrRowData[0].ToString();
                    vic.Name = arrRowData[1].ToString();
                    vic.Status = arrRowData[2].ToString();
                    vic.Title = arrRowData[3].ToString();
                    vic.Unit = arrRowData[4].ToString();
                    vic.SNO = arrRowData[5].ToString();
                    vic.BirthYear = arrRowData[6].ToString();
                    vic.Location = arrRowData[7].ToString();
                    vic.Role = arrRowData[8].ToString();

                    LstVictim.Add(vic);
                }
            }

            return LstVictim;
        }

        private List<EventData> TransToLstEventData(EventCaseMangViewModel vm)
        {
            List<EventData> LstEventData = new List<EventData>();

            string EventID = vm.CreateModel.EventID;
            string EventDateTime = vm.CreateModel.EventDateTime;
            string EventText = vm.CreateModel.EventText;

            EventData ed = new EventData();
            ed.EventID = EventID;
            ed.EventDateTime = DateTime.Parse(EventDateTime);
            ed.Text = EventText;

            LstEventData.Add(ed);

            return LstEventData;
        }

        #endregion

        #region 轉介

        [Log(LogActionChineseName.首頁)]
        public IActionResult ReferDataIndex(string submitBtn)
        {
            ViewBag.ddlRefer = dbAccess.GetddlReferUnit();

            EventCaseMangViewModel vm = new EventCaseMangViewModel();
            vm.ReferDataConditionModel = new EventCaseReferDataMangConditionModel();

            if (!string.IsNullOrEmpty(submitBtn))
                vm.ReferDataConditionModel.CaseID = submitBtn;

            return View(vm);
        }

        [Log(LogActionChineseName.匯入)]
        public IActionResult ReferDataUpload()
        {
            EventCaseMangViewModel vm = new EventCaseMangViewModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetReferDataSearchResult(EventCaseMangViewModel vm)
        {
            vm.ReferDataResultModel = dbAccess.GetReferDataSearchResult(vm.ReferDataConditionModel).ToList();

            #region 分頁
            vm.ReferDataConditionModel.TotalCount = vm.ReferDataResultModel.Count();
            int StartRow = vm.ReferDataConditionModel.Page * vm.ReferDataConditionModel.PageSize;
            vm.ReferDataResultModel = vm.ReferDataResultModel.Skip(StartRow).Take(vm.ReferDataConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchReferDataResultPartial", vm);
        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadReferDataTemplate()
        {
            string FileName = "轉介內容歷程_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public async Task<IActionResult> ReferDataImportExcel(EventCaseMangViewModel vm)
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

                if (!vm.File.FileName.Contains("轉介內容歷程_template"))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List< EventCaseReferDataImportMangResultModel > LstExcel = new List<EventCaseReferDataImportMangResultModel>();

                using (Stream stream = vm.File.OpenReadStream())
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);
                    List<string> LstSNo = new List<string>();

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        bool CanGo = true;

                        for (int j = 0; j <= row.Count() - 1; j++)
                        {
                            row.GetCell(j)?.SetCellType(CellType.String);
                            string Celldata = row.GetCell(j)?.ToString();
                            if (string.IsNullOrEmpty(Celldata))
                                CanGo = false;
                        }

                        if (row != null && CanGo)
                        {
                            string ReferID = "";
                            string CaseID = "";

                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstReferUnit = dbAccess.GetddlReferUnit();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlCaseID = dbAccess.GetddlCaseID();

                            if (!LstddlCaseID.Any(m => m.Text == row.GetCell(0)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:查無校安事件編號{0}", row.GetCell(0)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!LstReferUnit.Any(m => m.Text == row.GetCell(1)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(1)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                ReferID = LstReferUnit.Where(m => m.Text == row.GetCell(1)?.ToString()).FirstOrDefault().Value;
                            }

                            EventCaseReferDataImportMangResultModel excel = new EventCaseReferDataImportMangResultModel
                            {
                                CaseID = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                ReferID = ReferID,
                                HandleTime = DateTime.Parse(row.GetCell(2)?.StringCellValue),
                                HandleEvent = row.GetCell(3)?.StringCellValue.TrimStartAndEnd(),
                                HandleMan = row.GetCell(4)?.StringCellValue.TrimStartAndEnd(),
                                Leader = row.GetCell(5)?.StringCellValue.TrimStartAndEnd(),
                                Director = row.GetCell(6)?.StringCellValue.TrimStartAndEnd(),
                                Created = DateTime.Now
                            };

                            LstExcel.Add(excel);
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();
                if (LstExcel.Count > 0)
                {
                    var dbResult = dbAccess.ReferDataImportData(LstExcel, LoginUser);

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
        public IActionResult ExportReferDataSearchResult(EventCaseMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.轉介內容歷程, DateTime.Now.ToString("yyyyMMdd"));
            vm.ReferDataResultModel = dbAccess.GetReferDataSearchResult(vm.ReferDataConditionModel);

            if (vm.ReferDataResultModel != null && vm.ReferDataResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 40, 40, 40, 40, 40, 40, 40, 40 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(EventCaseReferDataExcelHeaderMangResultModel).GetProperties();

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
                for (int i = 0; i <= vm.ReferDataResultModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellValue(vm.ReferDataResultModel[i].CaseID);
                    dataRow.CreateCell(1).SetCellValue(vm.ReferDataResultModel[i].ReferIDText);
                    dataRow.CreateCell(2).SetCellValue(vm.ReferDataResultModel[i].HandleTime != null ? vm.ReferDataResultModel[i].HandleTime.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(3).SetCellValue(vm.ReferDataResultModel[i].HandleEvent);
                    dataRow.CreateCell(4).SetCellValue(vm.ReferDataResultModel[i].HandleMan);
                    dataRow.CreateCell(5).SetCellValue(vm.ReferDataResultModel[i].Leader);
                    dataRow.CreateCell(6).SetCellValue(vm.ReferDataResultModel[i].Director);
                    dataRow.CreateCell(7).SetCellValue(vm.ReferDataResultModel[i].Created != null ? vm.ReferDataResultModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm") : "");

                    foreach (ICell cell in dataRow.Cells)
                        cell.CellStyle = contentStyle;
                }

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms, true);
                ms.Flush();
                ms.Position = 0;

                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            }

            return View("Index", vm);

        }

        #endregion
    }
}
