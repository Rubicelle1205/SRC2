using Microsoft.AspNetCore.Mvc;
using WebPccuClub.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebPccuClub.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.IP = HttpContext.Connection.RemoteIpAddress.ToString() == "::1" ? "127.0.0.1" : HttpContext.Connection.RemoteIpAddress.ToString();
            
            ViewBag.LoginID = LoginUser.LoginId;

            string strFun = string.Empty;

            for(int i = 0; i <= LoginUser.UserRoleFun.Count -1; i++)
            {
                if(i != LoginUser.UserRoleFun.Count - 1)
                    strFun += LoginUser.UserRoleFun[i].MenuName + "、";
                else
                    strFun += LoginUser.UserRoleFun[i].MenuName;
            }
            ViewBag.Fun = strFun;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}