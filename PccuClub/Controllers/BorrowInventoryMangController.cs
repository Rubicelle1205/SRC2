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
    public class BorrowInventoryMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        //BorrowInventoryMangDataAccess dbAccess = new BorrowInventoryMangDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public BorrowInventoryMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            //BorrowInventoryMangViewModel vm = new BorrowInventoryMangViewModel();
            //vm.ConditionModel = new BorrowInventoryMangConditionModel();
            return View();
        }
    }
}
