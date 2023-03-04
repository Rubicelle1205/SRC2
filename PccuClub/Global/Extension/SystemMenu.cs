using PccuClub.WebAuth;
using System.Text;
using WebAuth.Entity;

namespace WebPccuClub.Global.Extension
{
    public static class SystemMenu
    {
        /// <summary>
        /// 建立使用者選單
        /// </summary>
        /// <param name="thisUser">LoginUser</param>
        public static string CreateUserMenu(this UserInfo thisUser, string baseurl)
        {
            StringBuilder Menu = new StringBuilder();

            List<FunInfo> rootMenu = thisUser.UserRoleFun.FindAll(f => f.MenuUpNode == "-1" && f.IsVisIble == true);
            foreach (FunInfo menu in rootMenu.OrderBy(p => p.SortOrder))
            {
                StringBuilder thisUserMenu = BuildUserMenu(menu, thisUser.UserRoleFun, baseurl);
                Menu.Append(thisUserMenu.ToString());
            }
            return Menu.ToString();
        }

        /// <summary>
        /// 建立使用者選單項目
        /// </summary>
        /// <param name="rootNode">選單</param>
        /// <returns></returns>
        private static StringBuilder BuildUserMenu(FunInfo rootMenu, List<FunInfo> roleFuns, string baseurl)
        {
            StringBuilder MenuBuilder = new StringBuilder();
            List<FunInfo> subMenus = roleFuns.FindAll(f => f.MenuUpNode == rootMenu.MenuNode);

            if (subMenus.Count > 0)
            {
                MenuBuilder.Append(@"<ul>");
                foreach (FunInfo fun in subMenus)
                {
                    StringBuilder thisSubFun = BuildUserMenu(rootMenu, roleFuns, baseurl);
                    MenuBuilder.Append(thisSubFun.ToString());
                }
                MenuBuilder.Append(@"</ul>");
            }
            else
            {
                string suburl = GetSubUrl();
                string funUrl = string.IsNullOrEmpty(rootMenu.Url) ? "#" : rootMenu.Url;
                if (rootMenu.MenuName == "有害生物防治資訊資料庫")
                {
                    MenuBuilder.Append($@"<li><a href='{baseurl}{funUrl}' title='{rootMenu.MenuName}'>有害生物防治<br>資訊資料庫</a></li>");
                }
                else
                {
                    MenuBuilder.Append($@"<li><a href='{baseurl}{funUrl}' title='{rootMenu.MenuName}'>{rootMenu.MenuName}</a></li>");
                }
            }

            return MenuBuilder;
        }

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
