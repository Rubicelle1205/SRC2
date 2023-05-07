using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.POIFS.Crypt;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PccuClub.WebAuth;
using System.ComponentModel;
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
    [LogAttribute(LogActionChineseName.使用者維護)]
    public class UserMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        UserMangDataAccess dbAccess = new UserMangDataAccess();
        AuthManager auth = new AuthManager();

        private readonly IHostingEnvironment hostingEnvironment;

        public UserMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlRole = dbAccess.GetAllRole();
            ViewBag.ddlIsEnable = dbAccess.GetIsEnable();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();

            UserMangViewModel vm = new UserMangViewModel();
            vm.ConditionModel = new UserMangConditionModel();

            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlRole = dbAccess.GetAllRole();
            ViewBag.ddlIsEnable = dbAccess.GetIsEnable();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();

            UserMangViewModel vm = new UserMangViewModel();
            vm.CreateModel = new UserMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, UserMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlRole = dbAccess.GetAllRole();
            ViewBag.ddlIsEnable = dbAccess.GetIsEnable();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();

            vm.EditModel = dbAccess.GetEditData(submitBtn);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(UserMangViewModel vm)
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
        public IActionResult SaveNewData(UserMangViewModel vm)
        {
            try
            {
                vm.EditModel = dbAccess.GetEditData(vm.CreateModel.LoginId);

                if (vm.EditModel != null)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = String.Format("管理者帳號:{0}已存在", vm.CreateModel.LoginId);
                    return Json(vmRtn);
                }

                dbAccess.DbaInitialTransaction();

                string EncryptPw = String.Empty;
                if (!string.IsNullOrEmpty(vm.CreateModel.Password))
                    EncryptPw = auth.EncryptionText(vm.CreateModel.Password);

                var dbResult = dbAccess.InsertData(vm, LoginUser, EncryptPw);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.InsertLifeClass(vm, LoginUser);

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
        public IActionResult EditOldData(UserMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                string EncryptPw = String.Empty;
                if (!string.IsNullOrEmpty(vm.EditModel.Password))
                    EncryptPw = auth.EncryptionText(vm.EditModel.Password);

                var dbResult = dbAccess.UpdateData(vm, LoginUser, EncryptPw);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.UpdateLifeClass(vm, LoginUser);

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

                var dbResult = dbAccess.DeleteRole(Ser);

                if (!dbResult.isSuccess) 
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode =  (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.DeleteLifeClass(Ser);

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
