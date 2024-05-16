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
    [LogAttribute(LogActionChineseName.全人學習護照管理)]
    public class ClubHolisticPassportController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubHolisticPassportDataAccess dbAccess = new ClubHolisticPassportDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubHolisticPassportController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlAllSchoolYear = dbAccess.GetSchoolYear(2);
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");
            ViewBag.ddlOrderBy = dbAccess.GetOrderBy();

            ClubHolisticPassportViewModel vm = new ClubHolisticPassportViewModel();
            vm.ConditionModel = new ClubHolisticPassportConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Detail(string id, ClubHolisticPassportViewModel vm)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            vm.DetailModel = dbAccess.GetDetailData(id);

            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ClubHolisticPassportViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAllClub = dbAccess.GetAllClubID();
            ViewBag.ddlAllActName = dbAccess.GetAllActName();
            ViewBag.ddlHolisticMainClass = dbAccess.GetddlHolisticMainClass();
            ViewBag.ddlHolisticSecondClass = dbAccess.GetddlHolisticSecondClass();
            ViewBag.ddlHolisticThridClass = dbAccess.GetddlHolisticThirdClass();
            ViewBag.ddlActInOrOut = dbAccess.GetddlActInOrOut();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");
            
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubHolisticPassportViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldData(ClubHolisticPassportViewModel vm)
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


        [Log(LogActionChineseName.取得樓館選單)]
        [ValidateInput(false)]
        public IActionResult InitBuildSelect(string PlaceSource)
        {
            if (PlaceSource == "01")
            {
                ViewBag.ddlBuild = dbAccess.GetBuild();
            }

            ClubHolisticPassportViewModel vm = new ClubHolisticPassportViewModel();
            vm.EditModel = new ClubHolisticPassportEditModel();
            vm.EditModel.PlaceSource = PlaceSource;

            return PartialView("_HolisticPlaceDataPartial", vm);
        }

        [Log(LogActionChineseName.取得場地選單)]
        [ValidateInput(false)]
        public IActionResult InitPlaceSelect(string PlaceSource, string Buildid)
        {
            ViewBag.ddlBuild = dbAccess.GetBuild();
            ViewBag.ddlPlace = dbAccess.GetPlace(PlaceSource, Buildid);


            ClubHolisticPassportViewModel vm = new ClubHolisticPassportViewModel();
            vm.EditModel = new ClubHolisticPassportEditModel();
            vm.EditModel.PlaceSource = PlaceSource;
            vm.EditModel.BuildID = Buildid;


            return PartialView("_HolisticPlaceDataPartial", vm);
        }


    }
}
