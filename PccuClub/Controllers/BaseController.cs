using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PccuClub.WebAuth;
using System.Text;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using WebPccuClub.Global;
using WebPccuClub.DataAccess;

namespace WebPccuClub.Controllers
{
    public class BaseController : Controller
    {
        BaseDataAccess dbAccess = new BaseDataAccess();

        #region 共用屬性
        private const string strConst_LoginPageUrl = @"/BakeendLogin";
        //private const string strConst_LoginPageUrl = @"/FrontLogin";
        private const string strConst_DefaultPageUrl = @"/Home/Index?node=-1";
        private const string strConst_Timeout = "操作逾時，請重新登入！";
        private const string strConst_NoAccess = "很抱歉 您無此頁面的存取權限！！";
        private const string keywordConditionSessionName = "SessionKeywordCondition";
        public Dictionary<string, string> localDictionary = new Dictionary<string, string>()
        {

        };


        public List<string> AlertMsg = new List<string>();

        /// <summary> 目前所在功能 </summary>
        public string MenuNode
        {
            get
            {
                if (HttpContext.Request.Query.TryGetValue("node", out StringValues node))
                { return node.FirstOrDefault(); }

                return string.Empty;
            }
        }

        /// <summary> 登入使用者 </summary>
        protected UserInfo LoginUser
        {
            get
            { return HttpContext.Session.GetObject<UserInfo>("LoginUser"); }
            set
            { HttpContext.Session.SetObject("LoginUser", value); }
        }

        #endregion

        #region 權限相關

        /// <summary>
        /// 檢查使用者的角色權限
        /// (
        /// 0:不做任何檢查 即使未登入也可操作,
        /// 1:只要登入便可操作,
        /// 2:需檢查使用者的角色權限
        /// 3:必須是supervisor才有權限
        /// )
        /// </summary>
        /// <param name="iSecurity"></param>
        /// <returns></returns>
        protected virtual bool CheckUserAuth(int iSecurity)
        {
            switch (iSecurity)
            {
                case 0:
                    //未登入時使用
                    return true;

                case 1:
                    //檢查是否已逾時操作，可用於登入後首頁
                    return TimeoutHandle();

                case 2:
                default:
                    return TimeoutHandle() && CheckPageAuth();
            }
        }

        /// <summary> 檢查使用者是否可以使用此功能 </summary>
        private bool CheckPageAuth()
        {
            string AreaName = string.Empty;
            string ControllerName = string.Empty;
            StringBuilder AuthUrl = new StringBuilder();

            if (ControllerContext.RouteData.Values["area"] != null)
            {
                AreaName = ControllerContext.RouteData.Values["area"].ToString();
                AuthUrl.Append(string.Format("/{0}", AreaName));
            }

            if (ControllerContext.RouteData.Values["Controller"] != null)
            {
                ControllerName = ControllerContext.RouteData.Values["Controller"].ToString();
                AuthUrl.Append(string.Format("/{0}", ControllerName));
            }

            bool isAllowAction = LoginUser.IsAllowAction(GetAuthUrlForMVC(AuthUrl.ToString()));

            if (isAllowAction || LoginUser.isSupervisor())
            { return true; }

            return false;
        }

        /// <summary> 取得權限判斷用的網址 </summary>
        public string GetAuthUrlForMVC(string URL)
        {
            if (string.IsNullOrEmpty(URL))
            { return string.Empty; }

            List<string> UrlList = URL.Replace(@"\", "/").Split('/').ToList<string>();
            UrlList.RemoveAll(s => string.IsNullOrEmpty(s));

            if (UrlList.Count < 2)
            { return string.Format("/{0}", UrlList[0]).ToLower(); }

            return string.Format("/{0}/{1}", UrlList[0], UrlList[1]).ToLower();
        }

        /// <summary> 是否 Timeout(False:Timeout) </summary>
        private bool TimeoutHandle()
        {
            return LoginUser != null;
        }


        #endregion 權限相關

        #region 頁面導向
        /// <summary>
        /// 呈現訊息之後會導到其他網頁 - MVC 用
        /// </summary>
        /// <param name="strMessage">顯示的訊息(傳入string.Empty則不顯示訊息，直接導頁)</param>
        /// <param name="PagePath">要導向的網頁</param>	
        protected ContentResult AlertMsgRedirect(string strMessage, string PagePath)
        {
            strMessage = strMessage.Replace("\r", "\\r").Replace("\n", "\\n").Replace("'", "\\'");

            string shtml = @"<script type='text/javascript'>";

            if (!string.IsNullOrEmpty(strMessage))
                shtml += "alert('" + strMessage + "');";

            shtml += $@"top.location.href ='{PagePath}';</script>";

            ContentResult Content = new ContentResult();
            Content.Content = shtml;
            Content.ContentType = "text/html;charset=utf-8";

            return Content;
        }

        #endregion 頁面導向

        public BaseController()
        { }

        /// <summary> OnActionExecuting </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CheckUserAuth(1))
            {
                filterContext.Result = AlertMsgRedirect(strConst_Timeout, SystemMenu.GetSubUrl() + strConst_LoginPageUrl);
            }
            else if (!CheckUserAuth(2))
            {
                filterContext.Result = AlertMsgRedirect(strConst_NoAccess, SystemMenu.GetSubUrl() + strConst_DefaultPageUrl);
            }

            ModelState.Clear();

            var controller = (ControllerBase)filterContext.Controller;
            var actionName = controller.ControllerContext.ActionDescriptor.ActionName;
            var controllerAttributes = controller.ControllerContext.ActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(LogAttribute), false);
            var actionAttributes = controller.ControllerContext.ActionDescriptor.ControllerTypeInfo.GetMethod(actionName)?.GetCustomAttributes(typeof(LogAttribute), false);
            if (LoginUser != null && (controllerAttributes?.Any() ?? false) && (actionAttributes?.Any() ?? false))
            {
                LogViewModel logModel = new LogViewModel()
                {
                    LoginID = LoginUser.LoginId,
                    UserName = LoginUser.UserName,
                    RoleName = string.Join(",", LoginUser.UserRole[0].RoleName),
                    IP = filterContext.HttpContext.Connection?.RemoteIpAddress?.ToString(),
                    FunName = controllerAttributes != null ? (controllerAttributes.FirstOrDefault() as LogAttribute)?.LogDisplayName : controller.ControllerContext.ActionDescriptor.ControllerName,
                    ActionName = actionAttributes != null ? (actionAttributes.FirstOrDefault() as LogAttribute)?.LogDisplayName : controller.ControllerContext.ActionDescriptor.ActionName
                };
                dbAccess.InsertLog(logModel);
            }

            base.OnActionExecuting(filterContext);
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

        /// <summary>
        /// 建立 SiteMap
        /// </summary>
        /// <param name="Menu">選單List</param>
        /// <param name="searchMenuNode">所在的選單節點</param>
        /// <returns>Html String</returns>
        protected string GetSiteMap(List<FunInfo> Menu, string searchMenuNode)
        {
            searchMenuNode = searchMenuNode == null ? "" : searchMenuNode;

            // 迴圈找父節點，找到後暫存在 stack
            Stack<FunInfo> stack = new Stack<FunInfo>();
            FunInfo menuInfo = Menu.Find(x => x.MenuNode == searchMenuNode);
            while (menuInfo != null)
            {
                stack.Push(menuInfo);
                menuInfo = Menu.Find(x => x.MenuNode == menuInfo.MenuUpNode);

                // 避免因為資料問題導致無窮迴圈
                if (stack.Count > Menu.Count)
                {
                    throw new Exception("取得siteMap異常");
                }
            }

            // 先產生前面固定的部分
            StringBuilder siteMap = new StringBuilder();
            siteMap.Append("目前瀏覽頁面: ");
            // url 修改
            string linkUrl = strConst_DefaultPageUrl;
            string cssClass = stack.Count < 1 ? "selected" : "";
            siteMap.Append(@$"<a href='{linkUrl}' class='{cssClass}'>系統首頁</a>");

            // 將 stack 中的資料反向取出，組 HTML
            while (stack.Count > 0)
            {
                menuInfo = stack.Pop();
                // url 修改
                linkUrl = string.IsNullOrEmpty(menuInfo.Url) ? @$"/Home/Catalog/?node={menuInfo.MenuNode}"
                    : @$"{menuInfo.Url}?node={menuInfo.MenuNode}";

                cssClass = searchMenuNode == menuInfo.MenuNode ? "selected" : "";

                siteMap.Append(" > ");
                siteMap.Append(@$"<a href='{linkUrl}' class='{cssClass}'>{menuInfo.MenuName}</a>");
            }
            return siteMap.ToString();
        }

        /// <summary> 取得使用者登入資訊 </summary>
        private string GetUserInfoText()
        {
            return $@"{LoginUser.UserName}";
        }

        #region Method
        /// <summary> 從Session中取關鍵字清單 </summary>
        public virtual Dictionary<string, string>? GetKeywordCondtionFromTempData()
        {
            var temp = HttpContext.Session.GetObject<Dictionary<string, string>>(keywordConditionSessionName);
            return temp ?? new Dictionary<string, string>();
        }

        /// <summary> Table/Column Translate </summary>
        public virtual string GetLocalDictionaryValue(string dicKey)
            => localDictionary.ContainsKey(dicKey.ToUpper()) ? localDictionary[dicKey.ToUpper()] : dicKey;

        /// <summary> Insert KeywordObject into Session </summary>
        public virtual void InsertKeywordTempData(object temp)
            => HttpContext.Session.SetObject(keywordConditionSessionName, temp);

        /// <summary> 移除整個Keyword obejct Session </summary>
        public virtual void ClearKeywordTempData()
            => HttpContext.Session.SetObject(keywordConditionSessionName, null);


        public virtual string GetControllerIdentity()
            => DateTime.Now.ToString("yyyyMMddHHmmssffff");

        /// <summary>
        /// 使否能看見該功能
        /// </summary>
        /// <param name="funtionname">功能名稱</param>
        public virtual bool FuntionVisiable(string funtionname)
        {
            UserInfo LoginUser = HttpContext.Session.GetObject<UserInfo>("LoginUser");
            List<FunInfo> rootMenu = LoginUser.UserRoleFun.FindAll(f => f.MenuName == funtionname);
            return (LoginUser.isSupervisor() || rootMenu.Count() > 0) ? true : false;
        }
        #endregion


    }
}
