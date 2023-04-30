using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Web.Mvc;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.期限日期)]
    public class DateLineMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        DateLineMangDataAccess dbAccess = new DateLineMangDataAccess();

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            DateLineMangViewModel vm = new DateLineMangViewModel();
            vm.EditModel = dbAccess.GetSearchResult().FirstOrDefault();

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldData(DateLineMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateConsent(vm.EditModel, LoginUser.UserName);

                if (!dbResult.isSuccess)
                {
                    vmRtn.ErrorCode = 1;
                    vmRtn.ErrorMsg = "儲存失敗";
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = 1;
                vmRtn.ErrorMsg = "儲存失敗" + ex.Message;
            }
            return Json(vmRtn);
        }

    }
}
