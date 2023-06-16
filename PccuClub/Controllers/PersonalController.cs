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
        ReturnViewModel vmRtn = new ReturnViewModel();
        PersonalDataAccess dbAccess = new PersonalDataAccess();

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            PersonalViewModel vm = new PersonalViewModel();
            vm.EditModel = dbAccess.GetSearchResult(LoginUser.LoginId).FirstOrDefault();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        public IActionResult EditOldData(PersonalViewModel vm)
        {
            AuthManager auth = new AuthManager();
            string strErr = string.Empty;

            if (!ChkData(vm.EditModel, out strErr))
            {
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = string.Format(@"{0}", strErr);
                return Json(vmRtn);
            }

            try
            {
                dbAccess.DbaInitialTransaction();
                string EncryptPw = String.Empty;
                
                if (!string.IsNullOrEmpty(vm.EditModel.Pwd))
                    EncryptPw = auth.EncryptionText(vm.EditModel.Pwd);

                var dbResult = dbAccess.UpdatePersonalData(EncryptPw, vm.EditModel, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = string.Format(@"更新失敗:{0}", dbResult.ErrorMessage);
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = string.Format(@"更新失敗:{0}", ex.Message);
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        private bool ChkData(PersonalEditModel editModel, out string msg)
        {
            msg = string.Empty;

            if (!string.IsNullOrEmpty(editModel.Pwd))
            {
                if (editModel.Pwd != editModel.ConformPwd)
                    msg += "請確認密碼與密碼確認輸入的資料相等!<br/>";

                if (6 > editModel.Pwd.Length || 15 < editModel.Pwd.Length)
                    msg += "密碼長度錯誤!<br/>";

                if (!editModel.Pwd.HasNumber())
                    msg += "密碼需包含至少一個數字!<br/>";

                if (!editModel.Pwd.HasUpperText() && !editModel.Pwd.HasLowerText())
                    msg += "至少包含一大寫或小寫英文字母!<br/>";
            }

            if (!string.IsNullOrEmpty(editModel.EMail))
            {
                if (!editModel.EMail.Contains("@"))
                {
                    msg += "信箱格式錯誤!<br/>";
                }
            }

            return msg == "";
        }
    }
}
