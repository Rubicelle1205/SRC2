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
    public class ClubLeaderInfoController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubLeaderInfoDataAccess dbAccess = new ClubLeaderInfoDataAccess();

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ClubLeaderInfoViewModel vm = new ClubLeaderInfoViewModel();
            vm.EditModel = dbAccess.GetSearchResult(LoginUser.LoginId).FirstOrDefault();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        public IActionResult EditOldData(ClubLeaderInfoViewModel vm)
        {
            AuthManager auth = new AuthManager();
            string strErr = string.Empty;

            try
            {
                dbAccess.DbaInitialTransaction();
                string EncryptPw = String.Empty;

                var dbResult = dbAccess.UpdateClubLeaderInfoData(vm.EditModel, LoginUser);

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
    }
}
