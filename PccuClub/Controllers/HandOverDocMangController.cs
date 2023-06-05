using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming.Values;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.後台已填寫表單)]
    public class HandOverDocMangController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        HandOverDocMangDataAccess dbAccess = new HandOverDocMangDataAccess();
        ClubHandoverDataAccess ClubdbAccess = new ClubHandoverDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public HandOverDocMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllDocType = dbAccess.GetAllDocType();

            HandOverDocMangViewModel vm = new HandOverDocMangViewModel();
            vm.ConditionModel = new HandOverDocMangConditionModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(HandOverDocMangViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.刪除)]
        [ValidateInput(false)]
        public IActionResult Delete(string Ser)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.DeletetData(Ser);

                if (!dbResult.isSuccess)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "刪除失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        public IActionResult HistorySwitch(string id, string docType)
        {
            switch (docType)
            {
                case "01":
                    return Redirect($"/HandOverDocMang/HandOver0101?id={id}");
                case "02":
                    return Redirect($"/HandOverDocMang/HandOver0102?id={id}");
                case "03":
                    return Redirect($"/HandOverDocMang/HandOver0103?id={id}");
                case "04":
                    return Redirect($"/HandOverDocMang/HandOver0204?id={id}");
                case "05":
                    return Redirect($"/HandOverDocMang/HandOver0205?id={id}");
                case "06":
                    return Redirect($"/HandOverDocMang/HandOver0206?id={id}");
                case "07":
                    return Redirect($"/HandOverDocMang/HandOver0307?id={id}");
                case "08":
                    return Redirect($"/HandOverDocMang/HandOver0308?id={id}");
                case "09":
                    return Redirect($"/HandOverDocMang/HandOver0309?id={id}");
                default:
                    Redirect("Index");
                    break;
            }

            return View();
        }


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0101(string id)
        {
            ViewBag.ddlAgree = dbAccess.getAllAgree();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0101Model = new ClubHandover0101ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0101Model = ClubdbAccess.GetHandover0101Data(id, LoginUser);
            }

            return View(vm);
        }





    }
}
