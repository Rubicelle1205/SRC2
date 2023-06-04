using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
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

            ActListMangViewModel vm = new ActListMangViewModel();
            vm.CreateModel = new ActListMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ActListMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            //ActListMangViewModel vm = new ActListMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }

        [Log(LogActionChineseName.匯入)]
        public IActionResult Upload()
        {
            ActListMangViewModel vm = new ActListMangViewModel();
            vm.ExcelModel = new ActListMangExcelResultModel();
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
            string FileName = string.Format("{0}_{1}", LogActionChineseName.活動性質項目, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 130 };

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

                    //dataRow.CreateCell(0).SetCellValue(vm.ResultModel[i].ActTypeName);
                    //dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].Memo);

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

                vm.CreateModel.LstFile = new List<ActListMangFileModel>();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("File"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass01", file);

                            ActListMangFileModel model = new ActListMangFileModel();
                            model.FilePath = strFilePath;

                            vm.CreateModel.LstFile.Add(model);
                        }
                        else if(Request.Form.Files[i].Name.Contains("CreateModel.ActProposal"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("ActProposal", file);
                            
                            vm.CreateModel.ActProposal = strFilePath;
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();

                //新增Main
                var dbResult = dbAccess.InsertData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                //新增Detail
                dbResult = dbAccess.InsertData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                //FileOutSide
                dbResult = dbAccess.InsertData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                //Files
                dbResult = dbAccess.InsertData(vm, LoginUser);

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
        public IActionResult EditOldData(ActListMangViewModel vm)
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






        #region Jquery用

        [ValidateInput(false)]
        public IActionResult ChkRundown(ActListMangViewModel vm)
        {
            bool CanUse = true;

            //先判斷是否在開放時間  PlaceSource (01:校內, 02:校內其他, 03:校外
            if (vm.RundownModel.PlaceSource == "01")
            {
                CanUse = dbAccess.ChkPlaceSchoolCanUse(vm);

                if (!CanUse)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "該場地目前不可使用";
                    return Json(vmRtn);
                }
            }

            //再查看選擇的時間是否已有活動
            if (vm.RundownModel.PlaceSource == "01")
            {
                CanUse = dbAccess.ChkHasAct(vm);

                if (!CanUse)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "該場地目前已被使用";
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


                CanUse = LstRundown.Where(m => m.STime == vm.RundownModel.STime && m.ETime == vm.RundownModel.ETime).ToList().Count > 0;

                if (!CanUse)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "場地重複增加";
                    return Json(vmRtn);
                }
            }

            return Json(vmRtn);
        }

        [ValidateInput(false)]
        public IActionResult GetUsedByDate(string SelectedDate)
        {
            ActListMangViewModel vm = new ActListMangViewModel();
            vm.LstPlaceUsedModel = dbAccess.GetPlaceUsedData(SelectedDate);

            return PartialView("_PlaceUsedPartial", vm);
        }

        public IActionResult GetTodayAct(string PlaceSource, string SelectedDate)
        {
            ActListMangViewModel vm = new ActListMangViewModel();

            if (string.IsNullOrEmpty(PlaceSource) || string.IsNullOrEmpty(SelectedDate))
            {
                return PartialView("_PlaceTodayActPartial", vm);
            }

            vm.LstTodayActModel = dbAccess.GetTodayAct(PlaceSource, SelectedDate);


            return PartialView("_PlaceTodayActPartial", vm);
        }

        [Log(LogActionChineseName.取得樓館選單)]
        [ValidateInput(false)]
        public IActionResult InitBuildSelect(string PlaceSource)
        {
            if (PlaceSource != "03")
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
