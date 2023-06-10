using DataAccess;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.已報備活動)]
    public class ActListMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ActListMangDataAccess dbAccess = new ActListMangDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ActListMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();
            ViewBag.ddlActVerify = dbAccess.GetAllActVerify();


            ActListMangViewModel vm = new ActListMangViewModel();
            vm.ConditionModel = new ActListMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlStaticOrDynamic = dbAccess.GetStaticOrDynamic();
            ViewBag.ddlActInOrOut = dbAccess.GetActInOrOut();
            ViewBag.ddlActType = dbAccess.GetActType();
            ViewBag.ddlUseITEquip = dbAccess.GetUseITEquip();
            ViewBag.ddlSDGs = dbAccess.GetSDGs();
            ViewBag.ddlPassport = dbAccess.GetPassport();
            ViewBag.ddlPlaceSource = dbAccess.GetPlaceSource();
            ViewBag.ddlHour = dbAccess.GetAllHour();
            ViewBag.ddlActVerify = dbAccess.GetAllActVerify();
            ViewBag.ddlAllClub = dbAccess.GetAllClub();

            ActListMangViewModel vm = new ActListMangViewModel();
            vm.CreateModel = new ActListMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ActListMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");


            ViewBag.ddlSDGs = dbAccess.GetSDGs();
            ViewBag.ddlRundownStatus = dbAccess.GetAllRundownStatus();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify();

            vm.EditModel = dbAccess.GetEditData(submitBtn);

            vm.EditModel.LstActRundown = dbAccess.GetEditRundownData(submitBtn);
            vm.EditModel.LstProposal = dbAccess.GetEditProposalData(submitBtn);
            vm.EditModel.LstOutSideFile = dbAccess.GetEditOutSideFileData(submitBtn);

            return View(vm);
        }



        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ActListMangViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(ActListMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.已報備活動, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 50, 50, 30, 30, 30, 50 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ActListMangExcelResultModel).GetProperties();

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

                    dataRow.CreateCell(0).SetCellValue(vm.ResultModel[i].ActId);
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].SchoolYear);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].ClubName);
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].ActName);
                    dataRow.CreateCell(4).SetCellValue(vm.ResultModel[i].SDate.Value.ToString("yyyy/MM/dd"));
                    dataRow.CreateCell(5).SetCellValue(vm.ResultModel[i].EDate.Value.ToString("yyyy/MM/dd"));
                    dataRow.CreateCell(6).SetCellValue(vm.ResultModel[i].ActVerifyText);
                    dataRow.CreateCell(7).SetCellValue(vm.ResultModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm:ss"));

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



        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> SaveNewData(ActListMangViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("File"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("ActOutSide", file);

                            ActListFilesModel model = new ActListFilesModel();
                            model.FileName = file.FileName;
                            model.FilePath = strFilePath;

                            vm.CreateModel.LstOutSideFile.Add(model);
                        }
                        else if(Request.Form.Files[i].Name.Contains("Proposal"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("ActProposal", file);

                            ActListFilesModel model = new ActListFilesModel();
                            model.FileName = file.FileName;
                            model.FilePath = strFilePath;

                            vm.CreateModel.LstProposal.Add(model);
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();

                DataTable dt = new DataTable();

                var dbResult = dbAccess.InsertActMainData(vm, LoginUser, out dt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                string ActId = dt.QueryFieldByDT("ActID");
                dt.Dispose();

                dbResult = dbAccess.InsertActDetailData(vm, ActId, LoginUser, out dt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                string ActDetailId = dt.QueryFieldByDT("ActDetailId");
                dt.Dispose();

                #region 整理一下..
                List<ActListMangRundownModel> LstRundown = new List<ActListMangRundownModel>();
                string[] arr = vm.CreateModel.strRundown.Split("|");

                for (int i = 0; i <= arr.Length - 1; i++)
                { 
                    string[] arr2 = arr[i].Split(",");

                    string PlaceSource = arr2[0];
                    string Date = arr2[1];
                    string STime = arr2[2];
                    string ETime = arr2[3];
                    string PlaceID = arr2[4];
                    string PlaceText = arr2[5];

                    if (LstRundown.Where(x => x.Date == Date && x.PlaceSource == PlaceSource).Count() > 0)
                    {
                        for (int j = 0; j <= LstRundown.Count - 1; j++)
                        {
                            for (int k = int.Parse(STime); k <= int.Parse(ETime) - 1; k++)
                            {
                                LstRundown[j].LstStime.Add(j);
                            }
                        }
                    }
                    else
                    {
                        ActListMangRundownModel model = new ActListMangRundownModel();
                        model.PlaceSource = PlaceSource;
                        model.Date = Date;
                        model.STime = STime;
                        model.ETime = ETime;
                        model.PlaceID = PlaceID;
                        model.PlaceText = PlaceText;

                        for (int j = int.Parse(model.STime); j <= int.Parse(model.ETime) - 1; j++)
                        {
                            model.LstStime.Add(j);
                        }

                        LstRundown.Add(model);
                    }
                }
                #endregion
                List<string> WritedDate = new List<string>();

                for (int i = 0; i <= LstRundown.Count - 1; i++)
                {

                    if (!WritedDate.Contains(LstRundown[i].Date))
                    {
                        dbResult = dbAccess.InsertActSectionData(vm, ActId, ActDetailId, DateTime.Parse(LstRundown[i].Date), LoginUser, out dt);

                        if (!dbResult.isSuccess)
                        {
                            dbAccess.DbaRollBack();
                            vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                            vmRtn.ErrorMsg = "新增失敗";
                            return Json(vmRtn);
                        }

                        WritedDate.Add(LstRundown[i].Date);
                    }

                    string ActSectionId = dt.QueryFieldByDT("ActSectionId");

                    dbResult = dbAccess.InsertActRundownData(vm, ActId, ActDetailId, ActSectionId, LstRundown[i], LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "新增失敗";
                        return Json(vmRtn);
                    }
                }

                dbResult = dbAccess.InsertActProposalData(vm, ActId, ActDetailId, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.InsertOutSideData(vm, ActId, ActDetailId, LoginUser);

                if (!dbResult.isSuccess && dbResult.ErrorCode != dbErrorCode._EC_NotAffect)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.InsertOutSideFileData(vm, ActId, ActDetailId, LoginUser);

                if (!dbResult.isSuccess && dbResult.ErrorCode != dbErrorCode._EC_NotAffect)
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
        public IActionResult EditOldData(ActListMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateActMainData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.UpdateActDetailData(vm, LoginUser);

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


        #region Jquery用

        [ValidateInput(false)]
        public IActionResult ChkRundown(ActListMangViewModel vm)
        {
            bool CanUse = true;
            List<int> hours = new List<int>();

            if (vm.RundownModel.PlaceSource == "01")
            {
                //確認場地開放狀態
                CanUse = dbAccess.ChkPlaceSchoolCanUse(vm);

                if (!CanUse)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "該場地目前不可使用";
                    return Json(vmRtn);
                }

                for (int j = int.Parse(vm.RundownModel.STime); j <= int.Parse(vm.RundownModel.ETime) - 1; j++)
                {
                    hours.Add(j);
                }

                //確認場地使用狀態
                string msg = string.Empty;
                CanUse = ChkCanBatchData(vm, DateTime.Parse(vm.RundownModel.Date), hours, out msg);

                if (!CanUse)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = msg;
                    return Json(vmRtn);
                }
            }

            //判斷是否重複增加，先拿出舊的資料
            if (!string.IsNullOrEmpty(vm.RundownModel.strRundown))
            {
                List<ActListMangRundownModel> LstRundown = new List<ActListMangRundownModel>();

                string[] arr = vm.RundownModel.strRundown.Split("|");

                for (int i = 0; i <= arr.Length - 1; i++)
                {
                    string[] arr2 = vm.RundownModel.strRundown.Split(",");

                    ActListMangRundownModel model = new ActListMangRundownModel();
                    model.PlaceSource = arr2[0];
                    model.Date = arr2[1];
                    model.STime = arr2[2];
                    model.ETime = arr2[3];
                    model.PlaceID = arr2[4];
                    model.PlaceText = arr2[5];

                    LstRundown.Add(model);
                }


                CanUse = LstRundown.Where(m => m.PlaceSource == vm.RundownModel.PlaceSource && m.Date == vm.RundownModel.Date && m.STime == vm.RundownModel.STime && m.ETime == vm.RundownModel.ETime && 
                                               m.PlaceID == vm.RundownModel.PlaceID && m.PlaceText == vm.RundownModel.PlaceText).ToList().Count > 0;

                if (CanUse)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "場地重複增加";
                    return Json(vmRtn);
                }
            }

            return Json(vmRtn);
        }

        //檢核
        private bool ChkCanBatchData(ActListMangViewModel vm, DateTime date, List<int> hours, out string msg)
        {
            bool ok = true;
            msg = string.Empty;

            string PlaceID = vm.RundownModel.PlaceID;

            if (string.IsNullOrEmpty(PlaceID)) { msg = "查找場地失敗"; return false; }


            DataTable dt = dbAccess.GetRundown(PlaceID, date);

            foreach (DataRow dr in dt.Rows)
            {
                int STime = int.Parse(dr["STime"].ToString());

                for (int i = 0; i <= hours.Count - 1; i++)
                {
                    if (STime == hours[i])
                    {
                        ok = false;
                        msg = string.Format("以下時段已有其他活動。<br/> 日期:{0} 時間:{1}:00 ", date.ToString("yyyy/MM/dd"), hours[i]);
                    }
                }
            }


            return ok;
        }

        [ValidateInput(false)]
        public IActionResult GetUsedByDate(string SelectedDate)
        {
            ActListMangViewModel vm = new ActListMangViewModel();
            //vm.LstPlaceUsedModel = dbAccess.GetPlaceUsedData(SelectedDate);

            //先抓取DB資料
            List<ActListMangPlaceUsedModel> LstNewPlaceUsed = dbAccess.GetPlaceUsedData(SelectedDate);
            List<ActListMangPlaceUsedModel> LstNewPlaceUsed2 = new List<ActListMangPlaceUsedModel>();

            foreach (ActListMangPlaceUsedModel item in LstNewPlaceUsed)
            {
                ActListMangPlaceUsedModel model = new ActListMangPlaceUsedModel();
                model.PlaceName = item.PlaceName;
                model.STime = item.STime;
                model.ETime = item.ETime;

                var SelectedItem = LstNewPlaceUsed2.Where(m => m.PlaceName == item.PlaceName).FirstOrDefault();

                if (SelectedItem != null)
                {
                    if (int.Parse(model.STime) < int.Parse(SelectedItem.STime))
                    {
                        SelectedItem.STime = model.STime;
                    }

                    if (int.Parse(model.ETime) > int.Parse(SelectedItem.ETime))
                    {
                        SelectedItem.ETime = model.ETime;
                    }
                }
                else
                {
                    LstNewPlaceUsed2.Add(model);
                }
            }


            vm.LstPlaceUsedModel = LstNewPlaceUsed2;


            return PartialView("_PlaceUsedPartial", vm);
        }

        public IActionResult GetTodayAct(string PlaceId, string SelectedDate)
        {
            ActListMangViewModel vm = new ActListMangViewModel();


            vm.LstTodayActModel = dbAccess.GetTodayAct(PlaceId, SelectedDate);


            return PartialView("_PlaceTodayActPartial", vm);
        }

        [Log(LogActionChineseName.取得樓館選單)]
        [ValidateInput(false)]
        public IActionResult InitBuildSelect(string PlaceSource)
        {
            if (PlaceSource == "01")
            {
                ViewBag.ddlBuild = dbAccess.GetBuild();
            }

            ActListMangViewModel vm = new ActListMangViewModel();
            vm.CreateModel = new ActListMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;

            return PartialView("_PlaceDataPartial", vm);
        }

        [Log(LogActionChineseName.取得場地選單)]
        [ValidateInput(false)]
        public IActionResult InitPlaceSelect(string PlaceSource, string Buildid)
        {
            ViewBag.ddlBuild = dbAccess.GetBuild();
            ViewBag.ddlPlace = dbAccess.GetPlace(PlaceSource, Buildid);


            ActListMangViewModel vm = new ActListMangViewModel();
            vm.CreateModel = new ActListMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;
            vm.CreateModel.Buildid = Buildid;


            return PartialView("_PlaceDataPartial", vm);
        }

        [Log(LogActionChineseName.取得場地資料)]
        [ValidateInput(false)]
        public IActionResult InitPlaceData(string PlaceSource, string Buildid, string PlaceId)
        {
            ViewBag.ddlBuild = dbAccess.GetBuild();
            ViewBag.ddlPlace = dbAccess.GetPlace(PlaceSource, Buildid);


            ActListMangViewModel vm = new ActListMangViewModel();
            vm.CreateModel = new ActListMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;
            vm.CreateModel.Buildid = Buildid;
            vm.CreateModel.PlaceId = PlaceId;

            vm.PlaceDataModel = dbAccess.GetPlaceData(PlaceSource, PlaceId);



            return PartialView("_PlaceDataPartial", vm);
        }

        #endregion

    }
}
