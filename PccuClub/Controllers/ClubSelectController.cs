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
    [LogAttribute(LogActionChineseName.社團選擇頁面)]
    public class ClubSelectController : Controller
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubSelectDataAccess dbAccess = new ClubSelectDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubSelectController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            List<UserInfo> LstUserInfo = HttpContext.Session.GetObject<List<UserInfo>>("FLoginUser_MultipleClub");

            ViewBag.LstUserInfo = LstUserInfo;

            ClubSelectViewModel vm = new ClubSelectViewModel();
            vm.ResultModel = dbAccess.GetSearchResult().ToList();

            return View(vm);
        }
    }
}
