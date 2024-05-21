using DataAccess;
using Microsoft.AspNetCore.Mvc;
using PccuClub.WebAuth;
using System.Web.Mvc;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.前台借用類別)]
    public class FBorrowIndexController : FBaseController
	{
        ReturnViewModel vmRtn = new ReturnViewModel();
        FBorrowIndexDataAccess dbAccess = new FBorrowIndexDataAccess();

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            FBorrowIndexViewModel vm = new FBorrowIndexViewModel();
            vm.ResultModel = dbAccess.GetSearchResult();
            return View(vm);
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Detail(string submitBtn)
        {
            FBorrowIndexViewModel vm = new FBorrowIndexViewModel();
            vm.Detail = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }
    }
}
