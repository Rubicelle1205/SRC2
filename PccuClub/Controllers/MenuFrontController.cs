using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using PccuClub.WebAuth;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.前台Menu頁)]
    public class MenuFrontController : Controller
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        MenuFrontDataAccess dbAccess = new MenuFrontDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public MenuFrontController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            UserInfo LoginUser = HttpContext.Session.GetObject<UserInfo>("FLoginUser");
            ViewBag.LoginUser = LoginUser;

            MenuFrontViewModel vm = new MenuFrontViewModel();
            vm.ResultModel = dbAccess.GetSearchResult().ToList();

            return View(vm);
        }
    }
}
