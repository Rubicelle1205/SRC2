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
using Utility;
using System.ServiceModel;
using WebAuth.Entity;
using Newtonsoft.Json;

namespace WebPccuClub.Controllers
{
    public class FrontLoginController : FBaseController
    {
        private LoginDataAccess dbAccess = new LoginDataAccess();
        public List<string> AlertMsg = new List<string>();

        ReturnViewModel vmRtn = new ReturnViewModel();
        AuthManager auth = new AuthManager();

        Utility.AuthUtil SSOAuth = new AuthUtil();

        /// <summary> SSO登入 </summary>
        public async Task<IActionResult> Index(string guid)
        {
            FrontLoginViewModel vm = new FrontLoginViewModel();
            LoginLogEntity loginEntity = GetLoginLogEntity(vm);

            if (string.IsNullOrEmpty(guid))
                return View(vm);

            HttpContext.Session.Clear();
            TempData.Clear();

            try
            {
                var result = await SSOAuth.GetSSOAuthData(guid);

                if (result.bError)
                    throw new Exception("登入轉換失敗，請使用帳號登入");

                SSOUserInfo sSOUserInfo = JsonConvert.DeserializeObject<SSOUserInfo>(result.JSONData);

				var dbResult = dbAccess.InsertNewUser(sSOUserInfo);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					throw new Exception("新增帳號失敗!");
				}

				if (!auth.GetUserByFUserID(sSOUserInfo.Account, out UserInfo user))
                {
                    loginEntity.Memo = "帳號不存在";
                    throw new Exception("登入失敗，帳號不存在!");
                }

				bool isAuth = false;

                isAuth = auth.FLogin(sSOUserInfo.Account, out UserInfo LoginUser);

                if (!isAuth)
                {
                    user.ErrorCount += 1;
                    loginEntity.Memo = "密碼錯誤";
                    throw new Exception("登入失敗，密碼錯誤!");
                }

                HttpContext.Session.Clear();
                TempData.Clear();

                LoginUser.LastLoginDate = DateTime.Now;
                LoginUser.ErrorCount = 0;
                LoginUser.LastLoginDate = DateTime.Now;
                LoginUser.LastModified = DateTime.Now;
                LoginUser.LastModifier = user.LoginId;
                loginEntity.Issuccess = true;
                loginEntity.Loginid = user.LoginId;

                HttpContext.Session.SetObject("LoginUser", LoginUser);

                UpdateLoginInfo(LoginUser);
                InsertLoginLog(loginEntity);
                InsertActionLog(loginEntity, LoginUser);

                return RedirectToAction("Index", "ClubList");
            }
            catch (FaultException)
            {
                loginEntity.Issuccess = false;
                loginEntity.Memo = "[API錯誤]登入轉換失敗";
                AlertMsg.Add(string.Format(@"{0}", "登入轉換失敗，請使用帳號登入"));
                InsertLoginLog(loginEntity);

                return View("Index", vm);
            }
            catch (Exception ex)
            {
                loginEntity.Issuccess = false;
                loginEntity.Memo = "[API回傳錯誤]" + ex.Message;
                AlertMsg.Add(string.Format(@"{0}", "登入轉換失敗，請使用帳號登入"));
                InsertLoginLog(loginEntity);

                return View("Index", vm);
            }
        }

        /// <summary> 帳號密碼登入 </summary>
        public IActionResult AuthLogin(FrontLoginViewModel vm)
        {
            LoginLogEntity loginEntity = GetLoginLogEntity(vm);

            List<string> ValidMsg = new List<string>();
            if (!Valid(vm, ValidMsg))
            {
                AlertMsg.Add(string.Join("<br/>", ValidMsg.ToArray()));
                return PartialView("Index", vm);
            }

            try
            {
                if (!auth.GetUserByFLogin(vm.LoginID, out UserInfo user))
                {
                    loginEntity.Memo = "帳號不存在";
                    throw new Exception("登入失敗，帳號不存在!");
                }

                bool isAuth = false;

                isAuth = auth.Login(vm.LoginID, vm.PassWord, out UserInfo LoginUser, "F");
                if (!isAuth)
                {
                    user.ErrorCount += 1;
                    loginEntity.Memo = "密碼錯誤";
                    throw new Exception("登入失敗，密碼錯誤!");
                }

                HttpContext.Session.Clear();
                TempData.Clear();

                LoginUser.LastLoginDate = DateTime.Now;
                LoginUser.ErrorCount = 0;
                LoginUser.LastLoginDate = DateTime.Now;
                LoginUser.LastModified = DateTime.Now;
                LoginUser.LastModifier = user.LoginId;
                loginEntity.Issuccess = true;

                HttpContext.Session.SetObject("LoginUser", LoginUser);

                UpdateLoginInfo(LoginUser);
                InsertLoginLog(loginEntity);
                InsertActionLog(loginEntity, LoginUser);

                return RedirectToAction("Index", "ClubList");
            }
            catch (Exception ex)
            {
                loginEntity.Issuccess = false;
                AlertMsg.Add(string.Format(@"{0}", ex.Message));
                InsertLoginLog(loginEntity);
                return PartialView("Index", vm);
            }
        }

        /// <summary> 登出 </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData.Clear();

            FrontLoginViewModel vm = new FrontLoginViewModel();

            return View("Index", vm);
        }

        #region 登入相關

        /// <summary> 登入檢核 - (True:通過，False:不通過) </summary>
        private bool Valid(FrontLoginViewModel vm, List<string> ValidMsg)
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
            DbExecuteInfo dbResult = dbAccess.UpdateEntity(user, "F");
            return dbResult.isSuccess;
        }

        #endregion

        #region Write Log

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
        private LoginLogEntity GetLoginLogEntity(FrontLoginViewModel vm)
        {
            LoginLogEntity dbEntity = new LoginLogEntity();
            dbEntity.Loginid = vm.LoginID;
            dbEntity.Logintime = DateTime.Now;

            IPAddress IpAddress = HttpContext.Connection.RemoteIpAddress;
            if (IpAddress != null)
            { dbEntity.Ip = IpAddress.ToString(); }

            return dbEntity;
        }

        #endregion

        #region Action Execute

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
