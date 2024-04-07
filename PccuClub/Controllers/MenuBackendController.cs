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
    [LogAttribute(LogActionChineseName.後台Menu頁)]
    public class MenuBackendController : Controller
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        SDGsMangDataAccess dbAccess = new SDGsMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public MenuBackendController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            UserInfo LoginUser = HttpContext.Session.GetObject<UserInfo>("FLoginUser");

            if (LoginUser != null)
                LoginUser.LoginSource = "B";

            ViewBag.LoginUser = LoginUser;

            return View();
        }
    }
}
