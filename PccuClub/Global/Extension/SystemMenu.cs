using Microsoft.AspNetCore.Http.Extensions;
using Org.BouncyCastle.Asn1.Ocsp;
using PccuClub.WebAuth;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using WebAuth.Entity;

namespace WebPccuClub.Global.Extension
{
    public static class SystemMenu
    {
        #region 後台

        /// <summary>
        /// 建立使用者選單
        /// </summary>
        /// <param name="thisUser">LoginUser</param>
        public static string CreateUserMenu(this UserInfo thisUser, string baseurl, object routeurl = null)
        {
            StringBuilder Menu = new StringBuilder();

            List<FunInfo> rootMenu = thisUser.UserRoleFun.FindAll(f => f.MenuUpNode == "-1" && f.IsVisIble == true);
            foreach (FunInfo menu in rootMenu.OrderBy(p => p.SortOrder))
            {
                StringBuilder thisUserMenu = BuildUserMenu(menu, thisUser, baseurl, routeurl);
                Menu.Append(thisUserMenu.ToString());
            }
            return Menu.ToString();
        }

        /// <summary>
        /// 建立使用者選單項目
        /// </summary>
        /// <param name="rootNode">選單</param>
        /// <returns></returns>
        private static StringBuilder BuildUserMenu(FunInfo rootMenu, UserInfo thisUser, string baseurl, object routeurl = null)
        {
            List<FunInfo> roleFuns = thisUser.UserRoleFun;
            StringBuilder MenuBuilder = new StringBuilder();
            List<FunInfo> subMenus = roleFuns.FindAll(f => f.MenuUpNode == rootMenu.MenuNode);

            if (thisUser.LoginSource == "B")
            {
				string suburl = GetSubUrl();
				string funUrl = string.IsNullOrEmpty(rootMenu.Url) ? "#" : rootMenu.Url;
				string leftHtml = "";

				string sitemap = GetUserSiteMap(thisUser, routeurl);
				string[] arr = sitemap.Split("|");
				string RouteUpNode = thisUser.UserRoleFun.Find(f => f.MenuNode == arr[0]) == null ? arr[0] : thisUser.UserRoleFun.Find(f => f.MenuNode == arr[0]).MenuNode;
				string RouteFunUrl = thisUser.UserRoleFun.Find(f => f.MenuNode == arr[1]) == null ? arr[1] : thisUser.UserRoleFun.Find(f => f.MenuNode == arr[1]).Url;

				if (subMenus.Count > 0)
				{
					if (funUrl == "#" && rootMenu.MenuNode == RouteUpNode)
					{
						MenuBuilder.Append($@"<li class='nav-item menu-open'><a href='{baseurl}{funUrl}' class='nav-link active' target='_self'><i class='{rootMenu.IconTag}' aria-hidden='true'></i><p>{rootMenu.MenuName}<i class='right fas fa-angle-left'></i></p></a>");
					}
					else
					{
						MenuBuilder.Append($@"<li class='nav-item'><a href='{baseurl}{funUrl}' class='nav-link' target='_self'><i class='{rootMenu.IconTag}' aria-hidden='true'></i><p>{rootMenu.MenuName}<i class='right fas fa-angle-left'></i></p></a>");
					}


					MenuBuilder.Append(@"<ul class='nav nav-treeview'>");
					foreach (FunInfo fun in subMenus)
					{
						StringBuilder thisSubFun = BuildUserMenu(fun, thisUser, baseurl, routeurl);
						MenuBuilder.Append(thisSubFun.ToString());
					}
					MenuBuilder.Append(@"</ul></li>");
				}
				else
				{
					if (routeurl == "Home")
					{
						leftHtml = $@"<li class='nav-item'><a href='{baseurl}{funUrl}' class='nav-link active' target='_self'><i class='{rootMenu.IconTag}' aria-hidden='true'></i><p>{rootMenu.MenuName}</p></a></li>";
					}
					else
					{
						if (RouteFunUrl == funUrl)
						{
							leftHtml = $@"<li class='nav-item'><a href='{baseurl}{funUrl}' class='nav-link active' target='_self'><i class='{rootMenu.IconTag}'></i><p>{rootMenu.MenuName}</p></a></li>";
						}
						else
						{
							leftHtml = $@"<li class='nav-item'><a href='{baseurl}{funUrl}' class='nav-link' target='_self'><i class='{rootMenu.IconTag}'></i><p>{rootMenu.MenuName}</p></a></li>";
						}
					}

					MenuBuilder.Append(leftHtml);

				}
			}

            return MenuBuilder;
        }

        /// <summary>
        /// 取得麵包屑
        /// </summary>
        /// <param name="thisUser"></param>
        /// <param name="baseurl"></param>
        /// <returns></returns>
        public static string GetUserSiteMap(this UserInfo thisUser, object baseurl)
        {
            string SiteMap = string.Empty;
            string MenuUpNode = string.Empty;
            string MenuNode = string.Empty;

            if (null != baseurl)
            {
                MenuNode = thisUser.GetMenuNodeByURL("/" + baseurl.ToString());
                MenuUpNode = thisUser.GetMenuNodeByParentMenuNode(MenuNode);
            }

            if(MenuNode != null && MenuUpNode != null)
                SiteMap = MenuUpNode + "|" + MenuNode;

            return SiteMap;
        }

        #endregion

        #region 前台

        #region Header


        /// <summary>
        /// 取得HeaderMenu
        /// </summary>
        /// <param name="thisUser">LoginUser</param>
        public static string CreateFrontUserHeadMenu(this UserInfo thisUser, string baseurl, object routeurl = null)
        {
            StringBuilder Menu = new StringBuilder();

            if (thisUser != null && thisUser.LoginSource == "F")
            {
                List<FunInfo> rootMenu = thisUser.UserRoleFun.FindAll(f => f.MenuUpNode == "-1" && f.IsVisIble == true);
                foreach (FunInfo menu in rootMenu.OrderBy(p => p.SortOrder))
                {
                    StringBuilder thisUserMenu = BuildFUserHeadMenu(menu, thisUser, baseurl, routeurl);
                    Menu.Append(thisUserMenu.ToString());
                }
            }

            return Menu.ToString();
        }

        /// <summary>
        /// 建立使用者選單項目
        /// </summary>
        /// <param name="rootNode">選單</param>
        /// <returns></returns>
        private static StringBuilder BuildFUserHeadMenu(FunInfo rootMenu, UserInfo thisUser, string baseurl, object routeurl = null)
        {
            List<FunInfo> roleFuns = thisUser.UserRoleFun;
            StringBuilder MenuBuilder = new StringBuilder();
            List<FunInfo> subMenus = roleFuns.FindAll(f => f.MenuUpNode == rootMenu.MenuNode);

            string suburl = GetSubUrl();
            string funUrl = string.IsNullOrEmpty(rootMenu.Url) ? "#" : rootMenu.Url;
            string leftHtml = "";

            string sitemap = GetUserSiteMap(thisUser, routeurl);
            string[] arr = sitemap.Split("|");
            string RouteUpNode = thisUser.UserRoleFun.Find(f => f.MenuNode == arr[0]) == null ? arr[0] : thisUser.UserRoleFun.Find(f => f.MenuNode == arr[0]).MenuNode;
            string RouteFunUrl = thisUser.UserRoleFun.Find(f => f.MenuNode == arr[1]) == null ? arr[1] : thisUser.UserRoleFun.Find(f => f.MenuNode == arr[1]).Url;

            if (subMenus.Any(f => f.Url == RouteFunUrl))
            {

                leftHtml = $@"<li><a class='nav-link active current-page-active' href='{baseurl}{funUrl}'>{rootMenu.MenuName}</a></li>";
            }
            else
            {
                leftHtml = $@"<li><a class='nav-link' href='{baseurl}{funUrl}'>{rootMenu.MenuName}</a></li>";
            }

            MenuBuilder.Append(leftHtml);

            return MenuBuilder;
        }

        #endregion

        #region Side

        /// <summary>
        /// 建立使用者選單
        /// </summary>
        /// <param name="thisUser">LoginUser</param>
        public static string CreateFrontUserMenu(this UserInfo thisUser, string baseurl, object routeurl = null)
        {
            StringBuilder Menu = new StringBuilder();

            if (thisUser != null)
            {
                List<FunInfo> rootMenu = thisUser.UserRoleFun.FindAll(f => f.MenuUpNode == "-1" && f.IsVisIble == true);

                foreach (FunInfo menu in rootMenu.OrderBy(p => p.SortOrder))
                {
                    StringBuilder thisUserMenu = BuildFUserMenu(menu, thisUser, baseurl, routeurl);
                    Menu.Append(thisUserMenu.ToString());
                }
            }

            return Menu.ToString();
        }

        /// <summary>
        /// 建立使用者選單項目
        /// </summary>
        /// <param name="rootNode">選單</param>
        /// <returns></returns>
        private static StringBuilder BuildFUserMenu(FunInfo rootMenu, UserInfo thisUser, string baseurl, object routeurl = null)
        {
            List<FunInfo> roleFuns = thisUser.UserRoleFun;
            StringBuilder MenuBuilder = new StringBuilder();
            List<FunInfo> subMenus = roleFuns.FindAll(f => f.MenuUpNode == rootMenu.MenuNode);

            if (thisUser.LoginSource == "F")
            {
				string suburl = GetSubUrl();
				string funUrl = string.IsNullOrEmpty(rootMenu.Url) ? "#" : rootMenu.Url;

				string sitemap = GetUserSiteMap(thisUser, routeurl);
				string[] arr = sitemap.Split("|");
				string RouteUpNode = thisUser.UserRoleFun.Find(f => f.MenuNode == arr[0]) == null ? arr[0] : thisUser.UserRoleFun.Find(f => f.MenuNode == arr[0]).MenuNode;
				string RouteFunUrl = thisUser.UserRoleFun.Find(f => f.MenuNode == arr[1]) == null ? arr[1] : thisUser.UserRoleFun.Find(f => f.MenuNode == arr[1]).Url;

				if (subMenus.Any(f => f.Url == "/" + routeurl))
				{
					foreach (var item in subMenus)
					{
						if ("/" + routeurl == item.Url)
						{
							MenuBuilder.Append($@"<li><a class='sideitem active' href='{baseurl}{item.Url}'>{item.MenuName}</a></li>");
						}
						else
						{
							MenuBuilder.Append($@"<li><a class='sideitem' href='{baseurl}{item.Url}'>{item.MenuName}</a></li>");
						}
					}
				}
			}
 
            return MenuBuilder;
        }

        #endregion

        #endregion

        /// <summary>
        /// IIS子目錄
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string GetSubUrl()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            IConfiguration config = builder.Build();
            return config.GetValue<string>("SubUrlstring:suburl");
        }
    }
}
