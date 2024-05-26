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
        //FResourceBorrowDataAccess dbAccess = new FResourceBorrowDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public FResourceBorrowController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            //FResourceBorrowViewModel vm = new FResourceBorrowViewModel();
            //vm.ConditionModel = new FResourceBorrowConditionModel();
            return View();
        }
    }
}
