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
    [LogAttribute(LogActionChineseName.資源借用狀況)]
    public class FResourceBorrowController : FBaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        FResourceBorrowDataAccess dbAccess = new FResourceBorrowDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public FResourceBorrowController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlAllBuild = dbAccess.GetBorrowSecondClassMang();

            FResourceBorrowViewModel vm = new FResourceBorrowViewModel();
            vm.ConditionModel = new FResourceBorrowConditionModel();
            return View();
        }

        #region 本日借用者

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(FResourceBorrowViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.取得上架數量)]
        [ValidateInput(false)]
        public IActionResult InitResourceDetail(string BorrowMainID)
        {
            FResourceBorrowViewModel vm = new FResourceBorrowViewModel();
            vm.ResultDetailModel = dbAccess.GetResultDetail(BorrowMainID);
            vm.ResultDetailModel.LstResource = dbAccess.GetResultResourceDetail(BorrowMainID);

            return PartialView("_SearchResultDetailPartial", vm);
        }

        #endregion

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResourceResult(FResourceBorrowViewModel vm)
        {

            vm.ResourceResultModel = dbAccess.GetResourceSearchResult(vm.ConditionModel).ToList();
            vm.ResourceBorrowedResultModel = dbAccess.GetBorrowedResult(vm.ConditionModel).ToList();

            foreach (FResourceBorrowResourceResultModel item in vm.ResourceResultModel)
            {
                int diffAmt = 0;
                string MainResourceID = item.MainResourceID;

                List<FResourceBorrowResourceBorrowedModel>  LstDiff = vm.ResourceBorrowedResultModel.Where(x => x.MainResourceID == MainResourceID).ToList();

                foreach (FResourceBorrowResourceBorrowedModel item2 in LstDiff)
                {
                    diffAmt = diffAmt + Int32.Parse(item2.BorrowAmt);
                }

                item.RemainAmt = (Int32.Parse(item.RemainAmt) - diffAmt).ToString();
            }

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResourceResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResourceResultModel = vm.ResourceResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultBorrowedPartial", vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, FResourceBorrowViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.LstDetail = dbAccess.GetEditDetailData(submitBtn);

            return View(vm);
        }
    }
}
