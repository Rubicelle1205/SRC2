using Microsoft.AspNetCore.Mvc;
using WebPccuClub.Models;
using System.Diagnostics;
using WebPccuClub.Global;
using WebPccuClub.DataAccess;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.社團一覽)]
    public class ClubListController : FBaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubListDataAccess dbAccess = new ClubListDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubListController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.ddlClubClass = dbAccess.GetAllClubClass();

            ClubListViewModel vm = new ClubListViewModel();
            vm.ConditionModel = new ClubListConditionModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubListViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string id, ClubListViewModel vm)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");


            vm.EditModel = dbAccess.GetEditData(id);

            return View(vm);
        }

    }
}