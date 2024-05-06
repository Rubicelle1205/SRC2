using Microsoft.AspNetCore.Mvc;
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

            //EventCaseMangViewModel vm = new EventCaseMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
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
                dbAccess.DbaInitialTransaction();

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
        public IActionResult EditOldData(EventCaseMangViewModel vm)
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
    }
}
