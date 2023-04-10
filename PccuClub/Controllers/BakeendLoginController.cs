using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DataAccess;
using WebPccuClub.Models;
using PccuClub.WebAuth;
using WebPccuClub.Entity;
using WebPccuClub.Global;
using WebPccuClub.DataAccess;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Controllers
{
    public class BakeendLoginController : Controller
    {
        private LoginDataAccess dbAccess = new LoginDataAccess();
        public List<string> AlertMsg = new List<string>();

        /// <summary> 功能首頁 </summary>
        public IActionResult Index()
        {
            BakeendLoginViewModel vm = new BakeendLoginViewModel();

            HttpContext.Session.Clear();
            TempData.Clear();

            return View("Index", vm);
        }

        /// <summary> 一般登入 </summary>
        //[HttpPost]
        public IActionResult AuthLogin(BakeendLoginViewModel vm)
        {
            AuthManager auth = new AuthManager();
            LoginLogEntity loginEntity = GetLoginLogEntity(vm);

            List<string> ValidMsg = new List<string>();
            if (!Valid(vm, ValidMsg))
            {
                AlertMsg.Add(string.Join(Environment.NewLine, ValidMsg.ToArray()));
                return PartialView("Index", vm);
            }

            try
            {
                if (!auth.GetUserMain(vm.LoginID, out UserInfo user))
                {
                    loginEntity.Memo = "帳號不存在";
                    throw new Exception("登入失敗，帳號不存在!");
                }

                bool isAuth = false;

                isAuth = auth.Login(vm.LoginID, vm.PassWord, out UserInfo LoginUser);
                if (!isAuth)
                {
                    user.ErrorCount += 1;
                    loginEntity.Memo = "密碼錯誤";
                    throw new Exception("登入失敗，密碼錯誤!");
                }

                HttpContext.Session.Clear();
                TempData.Clear();

                LoginUser.LastLoginDate = DateTime.Now;
                user.ErrorCount = 0;
                user.LastLoginDate = DateTime.Now;
                user.LastModified = DateTime.Now;
                user.LastModifier = user.LoginId;
                loginEntity.Issuccess = true;

                HttpContext.Session.SetObject("LoginUser", LoginUser);

                UpdateLoginInfo(user);
                InsertLoginLog(loginEntity);
                InsertActionLog(loginEntity, LoginUser);

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                loginEntity.Issuccess = false;
                AlertMsg.Add(string.Format(@"{0}", ex.Message));
                InsertLoginLog(loginEntity);
            }

            return View("Index", vm);
        }

        /// <summary> 登出 </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData.Clear();

            BakeendLoginViewModel vm = new BakeendLoginViewModel();

            return View("Index", vm);
        }


        /// <summary> 登入檢核 - (True:通過，False:不通過) </summary>
        private bool Valid(BakeendLoginViewModel vm, List<string> ValidMsg)
        {
            if (string.IsNullOrEmpty(vm.LoginID))
            { ValidMsg.Add("請輸入帳號!"); }

            if (string.IsNullOrEmpty(vm.PassWord))
            { ValidMsg.Add("請輸入密碼!"); }

            if (ValidMsg.Count > 0)
            { return false; }

            return true;
        }

        /// <summary> 更新使用者登入資訊(最後登入日期、登日次數等) </summary>
        private bool UpdateLoginInfo(UserInfo user)
        {
            DbExecuteInfo dbResult = dbAccess.UpdateEntity(user);
            return dbResult.isSuccess;
        }

        /// <summary> 寫入 Login Log </summary>
        private bool InsertLoginLog(LoginLogEntity thisEntity)
        {
            DbExecuteInfo dbResult = dbAccess.Insert(thisEntity);
            return dbResult.isSuccess;
        }

        /// <summary> 寫入 Login Action Log </summary>
        private bool InsertActionLog(LoginLogEntity thisEntity, UserInfo LoginUser)
        {
            DbExecuteInfo dbResult = dbAccess.ActionInsert(thisEntity, LoginUser);
            return dbResult.isSuccess;
        }

        /// <summary> 取得 Login Entity </summary>
        private LoginLogEntity GetLoginLogEntity(BakeendLoginViewModel vm)
        {
            LoginLogEntity dbEntity = new LoginLogEntity();
            dbEntity.Loginid = vm.LoginID;
            dbEntity.Logintime = DateTime.Now;

            IPAddress IpAddress = HttpContext.Connection.RemoteIpAddress;
            if (IpAddress != null)
            { dbEntity.Ip = IpAddress.ToString(); }

            return dbEntity;
        }


        #region

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ModelState.Clear();
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (TempData.ContainsKey("WEBSOL_ALERT_MESSAGE") && TempData["WEBSOL_ALERT_MESSAGE"] != null)
            {
                List<string> TempMsg = TempData["WEBSOL_ALERT_MESSAGE"] as List<String>;
                if (TempMsg != null)
                { this.AlertMsg.AddRange(TempMsg); }
            }

            if (AlertMsg.Count > 0)
            {
                TempData["WEBSOL_ALERT_MESSAGE"] = AlertMsg;
            }
        }

        #endregion
    }
}
