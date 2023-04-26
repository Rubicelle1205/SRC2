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
    [LogAttribute(LogActionChineseName.場地同意書)]
    public class ConsentMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ConsentMangDataAccess dbAccess = new ConsentMangDataAccess();

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ConsentMangViewModel vm = new ConsentMangViewModel();
            vm.EditModel = dbAccess.GetSearchResult().FirstOrDefault();

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldData(ConsentMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateConsent(vm.EditModel, LoginUser.UserName);

                if (!dbResult.isSuccess)
                    throw new Exception("Update/Insert Doc News failed");
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
