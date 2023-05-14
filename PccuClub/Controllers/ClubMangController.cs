using DataAccess;
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
using WebAuth.Entity;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.社團基本資料)]
    public class ClubMangController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubMangDataAccess dbAccess = new ClubMangDataAccess();
        AuthManager auth = new AuthManager();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();
            ViewBag.ddlClubClass = dbAccess.GetAllClubClass();

            ClubMangViewModel vm = new ClubMangViewModel();
            vm.ConditionModel = new ClubMangConditionModel();

            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();
            ViewBag.ddlClubClass = dbAccess.GetAllClubClass();
            ViewBag.ddlRoleClass = dbAccess.GetAllRoleClass();

            ClubMangViewModel vm = new ClubMangViewModel();
            vm.CreateModel = new ClubMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ClubMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();
            ViewBag.ddlClubClass = dbAccess.GetAllClubClass();
            ViewBag.ddlRoleClass = dbAccess.GetAllRoleClass();

            vm.EditModel = dbAccess.GetEditData(submitBtn);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubMangViewModel vm)
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
        public async Task<IActionResult> SaveNewData(ClubMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("LogoPath"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.LogoPath");

                            string strFilePath = await upload.UploadFileAsync("LogoPath", file);

                            vm.CreateModel.LogoPath = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("ActImgPath"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.ActImgPath");

                            string strFilePath = await upload.UploadFileAsync("ActImgPath", file);

                            vm.CreateModel.ActImgPath = strFilePath;
                        }
                    }
                }

                string EncryptPw = String.Empty;

                if (!string.IsNullOrEmpty(vm.CreateModel.Password))
                    EncryptPw = auth.EncryptionText(vm.CreateModel.Password);

                var dbResult = dbAccess.InsertData(EncryptPw, vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.InsertRole(vm, LoginUser);

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
        public async Task<IActionResult> EditOldData(ClubMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("LogoPath"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.LogoPath");
                            
                            string strFilePath = await upload.UploadFileAsync("LogoPath", file);

                            vm.EditModel.LogoPath = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("ActImgPath"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.ActImgPath");

                            string strFilePath = await upload.UploadFileAsync("ActImgPath", file);

                            vm.EditModel.ActImgPath = strFilePath;
                        }
                    }
                }

                string EncryptPw = String.Empty;

                if (!string.IsNullOrEmpty(vm.EditModel.Password))
                    EncryptPw = auth.EncryptionText(vm.EditModel.Password);

                var dbResult = dbAccess.UpdateData(EncryptPw, vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.UpdateRole(vm, LoginUser);

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

                var dbResult = dbAccess.DeleteFunData(Ser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode =  (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.DeletetUserRole(Ser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.DeletetData(Ser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
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





        #region Method

        #endregion
    }
}
