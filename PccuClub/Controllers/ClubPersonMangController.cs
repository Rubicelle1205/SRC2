using DataAccess;
using Microsoft.AspNetCore.Mvc;
using PccuClub.WebAuth;
using System.Web.Mvc;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.會員及幹部登錄)]
    public class ClubPersonMangController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubPersonMangDataAccess dbAccess = new ClubPersonMangDataAccess();

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            return View();
        }

		[Log(LogActionChineseName.前台幹部名冊)]
		public IActionResult CadreIndex()
		{
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.CadreMangConditionModel = new ClubCadreMangConditionModel();
            return View(vm);
        }

		[LogAttribute(LogActionChineseName.查詢)]
		public IActionResult GetSearchResult(ClubPersonMangViewModel vm)
		{
			vm.CadreMangResultModel = dbAccess.GetSearchResult(vm.CadreMangConditionModel, LoginUser).ToList();

			#region 分頁
			vm.CadreMangConditionModel.TotalCount = vm.CadreMangResultModel.Count();
			int StartRow = vm.CadreMangConditionModel.Page * vm.CadreMangConditionModel.PageSize;
			vm.CadreMangResultModel = vm.CadreMangResultModel.Skip(StartRow).Take(vm.CadreMangConditionModel.PageSize).ToList();
			#endregion

			return PartialView("_SearchCadreMangResultPartial", vm);
		}

		[Log(LogActionChineseName.新增)]
		public IActionResult CadreCreate()
		{
			ViewBag.ddlAllSex = dbAccess.GetAllSex();

            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
			vm.CadreMangCreateModel = new ClubCadreMangCreateModel();
            vm.CadreMangCreateModel.SchoolYear = PublicFun.GetNowSchoolYear();

            return View(vm);
		}

		[Log(LogActionChineseName.編輯)]
        public IActionResult CadreEdit(string submitBtn, ClubPersonMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAllSex = dbAccess.GetAllSex();

            //ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.CadreMangEditModel = dbAccess.GetCadreEditData(submitBtn);
			return View(vm);
        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public IActionResult CadreMangSaveNewData(ClubPersonMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.CadreMangInsertData(vm, LoginUser);

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
        public IActionResult CadreMangEditOldData(ClubPersonMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.CadreMangUpdateData(vm, LoginUser);

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
        public IActionResult CadreMangDelete(string Ser)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.CadreMangDeletetData(Ser);

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












        [Log(LogActionChineseName.前台會員名冊)]
        public IActionResult MemberIndex()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.MemberMangConditionModel = new ClubMemberMangConditionModel();
            return View(vm);
        }
    }
}
