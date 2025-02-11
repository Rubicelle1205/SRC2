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
    [LogAttribute(LogActionChineseName.管理員維護)]
    public class HyperAdminMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        HyperAdminMangDataAccess dbAccess = new HyperAdminMangDataAccess();
        AuthManager auth = new AuthManager();

        private readonly IHostingEnvironment hostingEnvironment;

        public HyperAdminMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlRole = dbAccess.GetAllRole();
            ViewBag.ddlIsEnable = dbAccess.GetIsEnable();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();

            HyperAdminMangViewModel vm = new HyperAdminMangViewModel();
            vm.ConditionModel = new HyperAdminMangConditionModel();

            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlRole = dbAccess.GetAllRole();
            ViewBag.ddlIsEnable = dbAccess.GetIsEnable();
            ViewBag.ddlYesOrNo = dbAccess.GetYesOrNo();

            ViewBag.ddlSystemCode = dbAccess.GetSystemCode();
            ViewBag.ddlRoleClub = dbAccess.GetRoleData("02");
            ViewBag.ddlRoleCase = dbAccess.GetRoleData("03");
            ViewBag.ddlRoleBorrow = dbAccess.GetRoleData("04");
            ViewBag.ddlRoleConsultation = dbAccess.GetRoleData("05");

            HyperAdminMangViewModel vm = new HyperAdminMangViewModel();
            vm.CreateModel = new HyperAdminMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, HyperAdminMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlRole = dbAccess.GetAllRole();
            ViewBag.ddlIsEnable = dbAccess.GetIsEnable();
            ViewBag.ddlYesOrNo = dbAccess.GetYesOrNo();

            ViewBag.ddlSystemCode = dbAccess.GetSystemCode();
            ViewBag.ddlRoleClub = dbAccess.GetRoleData("02");
            ViewBag.ddlRoleCase = dbAccess.GetRoleData("03");
            ViewBag.ddlRoleBorrow = dbAccess.GetRoleData("04");
            ViewBag.ddlRoleConsultation = dbAccess.GetRoleData("05");

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.LstCanUseFun = dbAccess.GetFunData(vm.EditModel);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(HyperAdminMangViewModel vm)
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
        public IActionResult SaveNewData(HyperAdminMangViewModel vm)
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
                if (!string.IsNullOrEmpty(vm.CreateModel.Pwd))
                    EncryptPw = auth.EncryptionText(vm.CreateModel.Pwd);

                var dbResult = dbAccess.InsertData(vm, LoginUser, EncryptPw);

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
        public IActionResult EditOldData(HyperAdminMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                string EncryptPw = String.Empty;
                if (!string.IsNullOrEmpty(vm.EditModel.Pwd))
                    EncryptPw = auth.EncryptionText(vm.EditModel.Pwd);

                var dbResult = dbAccess.UpdateData(vm, LoginUser, EncryptPw);

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
