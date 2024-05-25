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
using static WebPccuClub.Models.FBorrowRecordViewModel;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.我的借用紀錄)]
    public class FBorrowRecordController : FBaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        FBorrowRecordDataAccess dbAccess = new FBorrowRecordDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public FBorrowRecordController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            FBorrowRecordViewModel vm = new FBorrowRecordViewModel();
            vm.ConditionModel = new FBorrowRecordConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlApplyUnitType = dbAccess.GetddlApplyUnitType();
            ViewBag.ddlBorrowActVerify = dbAccess.GetddlBorrowActVerify();
            ViewBag.ddlSecondResurce = dbAccess.GetddlSecondResurce();

            FBorrowRecordViewModel vm = new FBorrowRecordViewModel();
            vm.CreateModel = new FBorrowRecordCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Detail(string id)
        {

            FBorrowRecordViewModel vm = new FBorrowRecordViewModel();
            vm.DetailModel = dbAccess.GetEditData(id);
            vm.DetailModel.LstFile = dbAccess.GetFileData(id);
            vm.DetailModel.LstDevice = dbAccess.GetDeviceData(id);
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(FBorrowRecordViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(LoginUser).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> SaveNewData(FBorrowRecordViewModel vm)
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

                            FBorrowRecordFileModel model = new FBorrowRecordFileModel();
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
                    string[] arrData = arr[i].Split(",");

                    string Device = arrData[0];
                    string Amt = arrData[1];

                    DataTable dt = dbAccess.GetMainResourceID(Device);

                    string MainClass = dt.QueryFieldByDT("MainClass");
                    string BorrowType = dt.QueryFieldByDT("BorrowType");

                    LstMainClassID.Add(MainClass);

                    FBorrowRecordDeviceModel DeviceModel = new FBorrowRecordDeviceModel();

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


                    List<FBorrowRecordDeviceModel> datalist = vm.CreateModel.LstDevice.Where(x => x.MainClassID == LstMainClassID[i]).ToList();
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

            FBorrowRecordViewModel vm = new FBorrowRecordViewModel();
            vm.CreateModel = new FBorrowRecordCreateModel();
            vm.CreateModel.MainResourceID = MainResourceID;
            vm.CreateModel.AmtShelves = AmtShelves;
            vm.CreateModel.AmtOnce = AmtOnce;


            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstItem = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

            for (int i = 1; i <= Int32.Parse(AmtShelves); i++)
            {
                if (i > Int32.Parse(AmtOnce))
                {
                    LstItem.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = i.ToString(), Text = string.Format("{0}", i), Disabled = true });
                }
                else
                {
                    LstItem.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = i.ToString(), Text = string.Format("{0}", i)});
                }
                
            }

            ViewBag.ddlSecondAmt = LstItem;

            return PartialView("_BorrowAmtPartial", vm);
        }

    }
}
