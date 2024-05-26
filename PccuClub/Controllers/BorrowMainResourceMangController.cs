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
            vm.InventoryDetailModel = dbAccess.GetInventoryDetailTemplete(vm.InventoryRecordModel.MainResourceID).ToList();


            return PartialView("_InventorySearchResultPartial", vm);
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
