using DataAccess;
using Microsoft.AspNetCore.Mvc;
using PccuClub.WebAuth;
using System.Web.Mvc;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.個人資料)]
    public class PersonalController : BaseController
    {
        PersonalDataAccess dbAccess = new PersonalDataAccess();

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            PersonalViewModel vm = new PersonalViewModel();
            vm.EditModel = dbAccess.GetSearchResult().FirstOrDefault();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        public IActionResult EditOldData(PersonalViewModel vm)
        {
            AuthManager auth = new AuthManager();
            string strErr = string.Empty;

            if (!ChkData(vm.EditModel, out strErr))
            {
                AlertMsg.Add(string.Format(@"{0}", strErr));
                return View("Index", vm);
            }

            try
            {
                dbAccess.DbaInitialTransaction();

                LoginUser.Password = auth.EncryptionText(vm.EditModel.Password);

                var dbResult = dbAccess.UpdatePersonalData(vm.EditModel, LoginUser);

                if (!dbResult.isSuccess)
                    throw new Exception("Update/Insert Doc News failed");
                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }

        private bool ChkData(PersonalEditModel editModel, out string msg)
        {
            msg = string.Empty;

            if (string.IsNullOrEmpty(editModel.Password))
                msg += "密碼欄位資料錯誤!<br/>";

            if (string.IsNullOrEmpty(editModel.ConformPassword))
                msg += "密碼確認欄位資料錯誤!<br/>";

            if (!string.IsNullOrEmpty(editModel.Password))
            {
                if (editModel.Password != editModel.ConformPassword)
                    msg += "請確認密碼與密碼確認輸入的資料相等!<br/>";

                if (6 > editModel.Password.Length || 15 < editModel.Password.Length)
                    msg += "密碼長度錯誤!<br/>";
            }


            return msg == "";
        }
    }
}
