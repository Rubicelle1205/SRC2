using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.Model;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.主資源維護)]
    public class BorrowMainResourceMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        BorrowMainResourceMangDataAccess dbAccess = new BorrowMainResourceMangDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public BorrowMainResourceMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlEnable = dbAccess.GetddlEnable();


            BorrowMainResourceMangViewModel vm = new BorrowMainResourceMangViewModel();
            vm.ConditionModel = new BorrowMainResourceMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlSecondClass = dbAccess.GetddlSecondClass();
            ViewBag.ddlEnable = dbAccess.GetddlEnable();
            ViewBag.ddlBorrowMultType = dbAccess.GetddlBorrowMultType();

            BorrowMainResourceMangViewModel vm = new BorrowMainResourceMangViewModel();
            vm.CreateModel = new BorrowMainResourceMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, BorrowMainResourceMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlSecondClass = dbAccess.GetddlSecondClass();
            ViewBag.ddlEnable = dbAccess.GetddlEnable();
            ViewBag.ddlBorrowMultType = dbAccess.GetddlBorrowMultType();

            //BorrowMainResourceMangViewModel vm = new BorrowMainResourceMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }

        [Log(LogActionChineseName.盤點)]
        public IActionResult InventoryIndex(string submitBtn, BorrowMainResourceMangViewModel vm)
        {
            vm.InventoryRecordModel = dbAccess.GetInventoryRecord(submitBtn);
            vm.InventoryRecordModel.RunType = vm.InventoryRecordModel.InventoryStatus == "02" ? "1" : "0";


            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(BorrowMainResourceMangViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetInventorySearchResult(BorrowMainResourceMangViewModel vm)
        {
            //RunType: null / 0:查看SecondResource ；1:盤點中；2:盤點結束

            DataTable dtt = dbAccess.GetMainResourceInventoryStatus(vm.InventoryRecordModel.MainResourceID);
            
            string RunType = dtt.QueryFieldByDT("InventoryStatus");

            if (vm.InventoryRecordModel.RunType != "2")
            {
                if (RunType == "02") { vm.InventoryRecordModel.RunType = "1"; }
            }
            

            switch (vm.InventoryRecordModel.RunType)
            {
                case null:
                    vm.InventoryDetailModel = dbAccess.GetSecondResource(vm.InventoryRecordModel.MainResourceID).ToList();
                break;
                case "0":
                    vm.InventoryDetailModel = dbAccess.GetSecondResource(vm.InventoryRecordModel.MainResourceID).ToList();
                    break;
                case "1":

                    try
                    {                    
                        
                        dbAccess.DbaInitialTransaction();

                        //先找一下有沒有盤點單，沒有的話，新增一張Record單
                        DataTable dt = new DataTable();
                        string RecodeOrder = "";

                        dt = dbAccess.SearchInventoryRecord(vm.InventoryRecordModel.MainResourceID);
                        RecodeOrder = dt.QueryFieldByDT("ID");

                        if (string.IsNullOrEmpty(RecodeOrder))
                        {
                            //更新Flag
                            var dbResult = dbAccess.UpdMainResourceToInventory(vm.InventoryRecordModel.MainResourceID, "02");

                            if (!dbResult.isSuccess)
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = "失敗";
                                return Json(vmRtn);
                            }

                            dbResult = dbAccess.UpdSecondResourceToInventory(vm.InventoryRecordModel.MainResourceID);

                            if (!dbResult.isSuccess)
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = "失敗";
                                return Json(vmRtn);
                            }

                            dbResult = dbAccess.CreateInventoryRecord(vm.InventoryRecordModel, LoginUser, out dt);

                            if (!dbResult.isSuccess)
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = "失敗";
                                return Json(vmRtn);
                            }

                            RecodeOrder = dt.QueryFieldByDT("ID");


                            //取得SecondResource資料
                            vm.InventoryDetailModel = dbAccess.GetSecondResource(vm.InventoryRecordModel.MainResourceID).ToList();

                            //回寫進Detail
                            dbAccess.InserInventoryDetailData(vm.InventoryDetailModel, RecodeOrder, LoginUser);

                            dbAccess.DbaCommit();
                        }

                        //撈取Detail資料到前台
                        vm.InventoryDetailModel = dbAccess.GetInventoryDetail(RecodeOrder).ToList();
                        vm.InventoryRecordModel.InventoryRecordID = RecodeOrder;
                    }
                    catch (Exception ex)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "刪除失敗" + ex.Message;
                        return Json(vmRtn);
                    }

                    break;
                case "2":

                    try
                    {
                        dbAccess.DbaInitialTransaction();


                        //更新Flag 為非盤點
                        var dbResult = dbAccess.UpdMainResourceToInventory(vm.InventoryRecordModel.MainResourceID, "01");


                        //撈取Detail資料
                        vm.InventoryDetailModel = dbAccess.GetInventoryDetail(vm.InventoryRecordModel.InventoryRecordID).ToList();

                        //更新上下架狀態與借用狀態
                        dbAccess.updSecondResourceStatus(vm.InventoryDetailModel, vm.InventoryRecordModel.InventoryRecordID, LoginUser);

                        //更新總盤點數到Record
                        DataTable dt2 = dbAccess.GetInventoryAmt(vm.InventoryRecordModel.InventoryRecordID);
                        string AmtInventory = dt2.QueryFieldByDT("Amt");
                        dbAccess.updInventoryAmtToRecord(vm.InventoryRecordModel.InventoryRecordID, AmtInventory, LoginUser);

                        //顯示SecondResource到前台
                        vm.InventoryDetailModel = dbAccess.GetSecondResource(vm.InventoryRecordModel.MainResourceID).ToList();

                        dbAccess.DbaCommit();
                    }
                    catch (Exception ex)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "刪除失敗" + ex.Message;
                        return Json(vmRtn);
                    }

                    break;
            }
            
            return PartialView("_InventorySearchResultPartial", vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> UpdSingleInventory(string InventoryRecordID, string DeviceID)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdInventoryDetailData(InventoryRecordID, DeviceID, LoginUser);

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

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> UpdMultInventory(string InventoryRecordID, string ReturnAmt)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdInventoryDetailMultData(InventoryRecordID, ReturnAmt, LoginUser);

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

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> updInventoryDetailAsync(string InventoryRecordID, string DeviceID)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdInventoryDetailData(InventoryRecordID, DeviceID, LoginUser);

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



        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> SaveNewDataAsync(BorrowMainResourceMangViewModel vm)
        {
            try
            {

                DataTable dt = dbAccess.GetMainResourceID(vm.CreateModel.MainResourceID);
                string ResourceID = dt.QueryFieldByDT("MainResourceID");

                if (!string.IsNullOrEmpty(ResourceID)) 
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = $@"新增失敗， 資源代碼 {ResourceID} 已存在";
                    return Json(vmRtn);
                }


                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("ResourceImg1"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.ResourceImg1");

                            string strFilePath = await upload.UploadFileAsync("ResourceImg", file);

                            vm.CreateModel.ResourceImg1 = strFilePath;
                        }
                        if (Request.Form.Files[i].Name.Contains("ResourceImg2"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.ResourceImg2");

                            string strFilePath = await upload.UploadFileAsync("ResourceImg", file);

                            vm.CreateModel.ResourceImg2 = strFilePath;
                        }
                        if (Request.Form.Files[i].Name.Contains("ResourceImg3"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.ResourceImg3");

                            string strFilePath = await upload.UploadFileAsync("ResourceImg", file);

                            vm.CreateModel.ResourceImg3 = strFilePath;
                        }
                        if (Request.Form.Files[i].Name.Contains("ResourceImg4"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.ResourceImg4");

                            string strFilePath = await upload.UploadFileAsync("ResourceImg", file);

                            vm.CreateModel.ResourceImg4 = strFilePath;
                        }
                    }
                }
                
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
        public async Task<IActionResult> EditOldDataAsync(BorrowMainResourceMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("ResourceImg1"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.ResourceImg1");

                            string strFilePath = await upload.UploadFileAsync("ResourceImg", file);

                            vm.EditModel.ResourceImg1 = strFilePath;
                        }
                        if (Request.Form.Files[i].Name.Contains("ResourceImg2"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.ResourceImg2");

                            string strFilePath = await upload.UploadFileAsync("ResourceImg", file);

                            vm.EditModel.ResourceImg2 = strFilePath;
                        }
                        if (Request.Form.Files[i].Name.Contains("ResourceImg3"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.ResourceImg3");

                            string strFilePath = await upload.UploadFileAsync("ResourceImg", file);

                            vm.EditModel.ResourceImg3 = strFilePath;
                        }
                        if (Request.Form.Files[i].Name.Contains("ResourceImg4"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.ResourceImg4");

                            string strFilePath = await upload.UploadFileAsync("ResourceImg", file);

                            vm.EditModel.ResourceImg4 = strFilePath;
                        }
                    }
                }

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
    }
}
