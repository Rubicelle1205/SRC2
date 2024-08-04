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
    [LogAttribute(LogActionChineseName.借用紀錄管理)]
    public class BorrowRecordMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        BorrowRecordMangDataAccess dbAccess = new BorrowRecordMangDataAccess();
        UploadUtil upload = new UploadUtil();
        MailUtil mail = new MailUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public BorrowRecordMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            
            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlApplyUnitType = dbAccess.GetddlApplyUnitType();
            ViewBag.ddlBorrowActVerify = dbAccess.GetddlBorrowActVerify();

            BorrowRecordMangViewModel vm = new BorrowRecordMangViewModel();
            vm.ConditionModel = new BorrowRecordMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlApplyUnitType = dbAccess.GetddlApplyUnitType();
            ViewBag.ddlBorrowActVerify = dbAccess.GetddlBorrowActVerify();
            ViewBag.ddlSecondResurce = dbAccess.GetddlSecondResurce();

            BorrowRecordMangViewModel vm = new BorrowRecordMangViewModel();
            vm.CreateModel = new BorrowRecordMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, BorrowRecordMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlApplyUnitType = dbAccess.GetddlApplyUnitType();
            ViewBag.ddlBorrowActVerify = dbAccess.GetddlBorrowActVerify();

            //BorrowRecordMangViewModel vm = new BorrowRecordMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.LstFile = dbAccess.GetFileData(submitBtn);
            vm.EditModel.LstDevice = dbAccess.GetDeviceData(submitBtn);
            vm.EditModel.LstEventData = dbAccess.GetEventData(submitBtn);
            vm.EditModel.EventDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(BorrowRecordMangViewModel vm)
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
        public async Task<IActionResult> SaveNewDataAsync(BorrowRecordMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("File"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("BorrowRecord", file);

                            BorrowRecordMangFileModel model = new BorrowRecordMangFileModel();
                            model.FileName = file.FileName;
                            model.FilePath = strFilePath;

                            vm.CreateModel.LstFile.Add(model);
                        }
                    }
                }

                List<string> LstMainClassID = new List<string>();
                List<string> LstSavedMainResourceID = new List<string>();
                string[] arr = vm.CreateModel.strDeviceData.Split("|");

                for (int i = 0; i <= arr.Length - 1; i++)
                {
                    string [] arrData = arr[i].Split(",");

                    string Device = arrData[0];
                    string Amt = arrData[1];

                    DataTable dt = dbAccess.GetMainResourceID(Device);

                    string MainClass = dt.QueryFieldByDT("MainClass");
                    string BorrowType = dt.QueryFieldByDT("BorrowType");

                    LstMainClassID.Add(MainClass);

                    BorrowRecordMangDeviceModel DeviceModel = new BorrowRecordMangDeviceModel();
                    
                    //非大量借用
                    if (BorrowType == "02")
                    {
                        int TotAmt = Int32.Parse(Amt);

                        for (int j = 1; j <= TotAmt; j++)
                        {
                            DeviceModel.MainClassID = MainClass;
                            DeviceModel.MainResourceID = Device;
                            DeviceModel.BorrowAmt = "1";
                            DeviceModel.BorrowStatus = "01";
                            vm.CreateModel.LstDevice.Add(DeviceModel);
                        }
                    }
                    else
                    {
                        DeviceModel.MainClassID = MainClass;
                        DeviceModel.MainResourceID = Device;
                        DeviceModel.BorrowAmt = Amt;
                        DeviceModel.BorrowStatus = "01";
                        vm.CreateModel.LstDevice.Add(DeviceModel);
                    }
                }

                for (int i = 0; i <= LstMainClassID.Count - 1; i++)
                {
                    DataTable dt = new DataTable();

                    vm.CreateModel.MainClassID = LstMainClassID[i];
                    var dbResult = dbAccess.InsertMainData(vm, LoginUser, out dt);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "新增失敗";
                        return Json(vmRtn);
                    }

                    string BorrowMainID = dt.QueryFieldByDT("BorrowMainID");


                    List<BorrowRecordMangDeviceModel> datalist = vm.CreateModel.LstDevice.Where(x => x.MainClassID == LstMainClassID[i]).ToList();
                    dbResult = dbAccess.InsertDeviceData(datalist, LoginUser, BorrowMainID);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "新增失敗";
                        return Json(vmRtn);
                    }

                    if (vm.CreateModel.LstFile.Count > 0)
                    {
                        if (!LstSavedMainResourceID.Any(x => x == BorrowMainID))
                        {
                            dbResult = dbAccess.InsertFileData(vm.CreateModel.LstFile, LoginUser, BorrowMainID);

                            if (!dbResult.isSuccess)
                            {
                                dbAccess.DbaRollBack();
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = "新增失敗";
                                return Json(vmRtn);
                            }

                            LstSavedMainResourceID.Add(BorrowMainID);
                        }
                    }
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
        public async Task<IActionResult> EditOldDataAsync(BorrowRecordMangViewModel vm)
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
        public IActionResult EditOldEventData(BorrowRecordMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateEventData(vm, LoginUser, vm.EditModel.BorrowMainID);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();

                if (vm.EditModel.EventID == "02" || vm.EditModel.EventID == "03")
                {
                    DataTable dtReceiver = dbAccess.GetMailReceiver(vm.EditModel.BorrowMainID);

                    string Receiver = dtReceiver.QueryFieldByDT("ApplyEmail");
                    string ApplyMan = dtReceiver.QueryFieldByDT("ApplyMan");

                    string MailBody = GenMailBody(ApplyMan, vm.EditModel.EventID);

                    if (!string.IsNullOrEmpty(Receiver))
                    {
                        mail.ExecuteSendMail(Receiver, "借用單更新通知", MailBody, System.Net.Mail.MailPriority.High, null);
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

        //單筆借出
        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult UpdSecondResource(string DeviceID, string selectedSecondResourceID)
        {

            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdDeviceBorrowSecondResource(DeviceID, selectedSecondResourceID, LoginUser);

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
        public IActionResult UpdMultSecondResource(string DeviceID, string BorrowSecondResourceID, string BorrowAmt)
        {

            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdMultDeviceBorrowSecondResource(DeviceID, BorrowSecondResourceID, BorrowAmt, LoginUser);

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
        public IActionResult UpdSingleReturn(string BorrowMainID, string ReturnSecondResource)
        {

            try
            {
                //先確認本次歸還的資產編號是否存在
                List<BorrowRecordMangDeviceModel> LstDevice = new List<BorrowRecordMangDeviceModel>();
                LstDevice = dbAccess.GetDeviceDataBySecondResourceNo(BorrowMainID, ReturnSecondResource);

                if (LstDevice.Count == 0)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "本次借用中，查無此資產編號";
                    return Json(vmRtn);
                }

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdDeviceReturnSecondResource(BorrowMainID, ReturnSecondResource, LoginUser);

                if (!dbResult.isSuccess && dbResult.AffectRowCount != 1)
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
        private string GenMailBody(string ApplyMan, string EventID)
        {
            string str = string.Empty;
            string Event = EventID == "02" ? "審核通過" : "審核失敗";

            str = $@"<p>{ApplyMan} 您好:</p>
                    <p>借用單{Event}，可至系統查看</p>";

            return str;
        }

        [Log(LogActionChineseName.取得上架數量)]
        [ValidateInput(false)]
        public IActionResult InitBorrowAmt(string MainResourceID)
        {
            if (!string.IsNullOrEmpty(MainResourceID))
            {
                ViewBag.ddlSecondResurce = dbAccess.GetddlSecondResurce();
            }

            DataTable dt = new DataTable();

            dbAccess.GetBorrowAmt(MainResourceID, out dt);

            string AmtShelves = dt.QueryFieldByDT("AmtShelves");
            string AmtOnce = dt.QueryFieldByDT("AmtOnce");

            BorrowRecordMangViewModel vm = new BorrowRecordMangViewModel();
            vm.CreateModel = new BorrowRecordMangCreateModel();
            vm.CreateModel.MainResourceID = MainResourceID;
            vm.CreateModel.AmtShelves = AmtShelves;
            vm.CreateModel.AmtOnce = AmtOnce;


            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstItem = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

            if (Int32.Parse(AmtOnce) == 0)
            {
                for (int i = 1; i <= Int32.Parse(AmtShelves); i++)
                {
                        LstItem.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = i.ToString(), Text = string.Format("{0}", i) });
                }
            }
            else
            {
                for (int i = 1; i <= Int32.Parse(AmtShelves); i++)
                {
                    if (i > Int32.Parse(AmtOnce))
                    {
                        LstItem.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = i.ToString(), Text = string.Format("{0}", i), Disabled = true });
                    }
                    else
                    {
                        LstItem.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = i.ToString(), Text = string.Format("{0}", i) });
                    }

                }
            }

            ViewBag.ddlSecondAmt = LstItem;

            return PartialView("_BorrowAmtPartial", vm);
        }

        [Log(LogActionChineseName.取得上架數量)]
        [ValidateInput(false)]
        public IActionResult InitBorrowSecondsResourceID(string MainResourceID, string RowID)
        {
            BorrowRecordMangViewModel vm = new BorrowRecordMangViewModel();
            vm.ddlModel = new BorrowRecordMangddlModel();


            ViewBag.ddlSecondResource = dbAccess.GetddlSecondResource(MainResourceID);
            ViewBag.BorrowType = "0";
            ViewBag.RowID = RowID;

            return PartialView("_BorrowSecondResourcePartial");
        }

        [Log(LogActionChineseName.取得上架數量)]
        [ValidateInput(false)]
        public IActionResult InitMultBorrowResource(string MainResourceID, string RowID, string BorrowMainID)
        {
            //大量借用
            BorrowRecordMangViewModel vm = new BorrowRecordMangViewModel();
            vm.ddlModel = new BorrowRecordMangddlModel();

            DataTable dtBorrowAmt = dbAccess.GetOrderBorrowAmt(BorrowMainID, MainResourceID);

            ViewBag.BorrowType = "1";
            ViewBag.MaxAmt = dtBorrowAmt.QueryFieldByDT("BorrowAmt");

            return PartialView("_BorrowSecondResourcePartial");
        }

        [Log(LogActionChineseName.取得上架數量)]
        [ValidateInput(false)]
        public IActionResult UpdSingoReturn()
        {
            //大量歸還
            BorrowRecordMangViewModel vm = new BorrowRecordMangViewModel();
            vm.ddlModel = new BorrowRecordMangddlModel();

            ViewBag.BorrowType = "4";

            return PartialView("_BorrowSecondResourcePartial");
        }


        [Log(LogActionChineseName.取得上架數量)]
        [ValidateInput(false)]
        public IActionResult InitMultReturnResource()
        {
            //大量歸還
            BorrowRecordMangViewModel vm = new BorrowRecordMangViewModel();
            vm.ddlModel = new BorrowRecordMangddlModel();

            ViewBag.BorrowType = "3";

            return PartialView("_BorrowSecondResourcePartial");
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult UpdMultRetrunResource(string DeviceID, string BorrowSecondResourceID, string ReturnAmt)
        {

            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdMultDeviceRetrunSecondResource(DeviceID, BorrowSecondResourceID, ReturnAmt, LoginUser);

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

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(BorrowRecordMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.借用紀錄管理, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 20, 20, 20, 20, 20, 20, 20 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(BorrowRecordMangExcelHeaderModel).GetProperties();

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

                    dataRow.CreateCell(0).SetCellValue(vm.ResultModel[i].BorrowMainID);
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].ApplyUnitName);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].ApplyMan);
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].ApplyTitle);
                    dataRow.CreateCell(4).SetCellValue(vm.ResultModel[i].TakeSDate != null ? vm.ResultModel[i].TakeSDate.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(5).SetCellValue(vm.ResultModel[i].TakeEDate != null ? vm.ResultModel[i].TakeEDate.Value.ToString("yyyy/MM/dd HH:mm") : "");
                    dataRow.CreateCell(6).SetCellValue(vm.ResultModel[i].MainClassIDText);
                    dataRow.CreateCell(7).SetCellValue(vm.ResultModel[i].ActVerifyText);
                    dataRow.CreateCell(8).SetCellValue(vm.ResultModel[i].Created != null ? vm.ResultModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm") : "");

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

        [Log(LogActionChineseName.刪除)]
        [ValidateInput(false)]
        public IActionResult Delete(string Ser)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.DeletetMainData(Ser);

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

        [Log(LogActionChineseName.刪除)]
        [ValidateInput(false)]
        public IActionResult DeleteDevice(string Ser)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.DeletetDeviceData(Ser);

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
    }
}
