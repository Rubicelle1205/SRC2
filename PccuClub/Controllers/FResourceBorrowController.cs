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
    [LogAttribute(LogActionChineseName.業務分類維護)]
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

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(FResourceBorrowViewModel vm)
        {
            List<DateTime> LstDate = new List<DateTime>();

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























        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResourceResult(FResourceBorrowViewModel vm)
        {
            //List<DateTime> LstDate = new List<DateTime>();

            //DateTime SDate = DateTime.Parse(vm.ConditionModel.SDate);


            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }
    }
}
