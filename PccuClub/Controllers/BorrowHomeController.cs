using Microsoft.AspNetCore.Mvc;
using WebPccuClub.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using NPOI.SS.Formula.Functions;
using WebAuth.Entity;

namespace WebPccuClub.Controllers
{
    public class BorrowHomeController : BaseController
    {
        private readonly ILogger<BorrowHomeController> _logger;

        public BorrowHomeController(ILogger<BorrowHomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.IP = HttpContext.Connection.RemoteIpAddress.ToString() == "::1" ? "127.0.0.1" : HttpContext.Connection.RemoteIpAddress.ToString();
            
            ViewBag.LoginID = LoginUser.LoginId;

            string strFun = string.Empty;

            List<FunInfo> LstFunInfo = LoginUser.UserRoleFun.Where(x => x.SystemCode == "04" && x.Url != "").ToList();

            for(int i = 0; i <= LstFunInfo.Count -1; i++)
            {
                if(i != LstFunInfo.Count - 1)
                    strFun += LstFunInfo[i].MenuName + "、";
                else
                    strFun += LstFunInfo[i].MenuName;
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