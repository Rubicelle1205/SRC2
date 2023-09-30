using WebAuth.Entity;

namespace PccuClub.WebAuth
{
    /// <summary>
    /// 使用者資料
    /// </summary>
    public class UserInfo
    {
        /// <summary> 登入帳號 </summary>
		public string LoginId { get; set; }

        /// <summary> 登入密碼 </summary>
        public string Password { get; set; }

        /// <summary> 登入名稱 </summary>
        public string UserName { get; set; }

        /// <summary> 單位代碼 </summary>
        public int? UnitId { get; set; }

        /// <summary> 單位名稱 </summary>
        public string UnitName { get; set; }

        /// <summary> 社團ID </summary>
        public string ClubId { get; set; }

        /// <summary> 社團中文名稱 </summary>
        public string ClubCName { get; set; }

        /// <summary> 社團英文名稱 </summary>
        public string ClubEName { get; set; }

        /// <summary> 帳號來源 </summary>
        public string LoginSource { get; set; }

		/// <summary> 出生日期 </summary>
		public DateTime? Birthday { get; set; }

        /// <summary> 性別(0:女，1:男) </summary>
        public int? Sex { get; set; }

        /// <summary> E-mail </summary>
        public string Email { get; set; }

        /// <summary> 聯絡電話 </summary>
        public string Tel { get; set; }

        /// <summary> 行動電話 </summary>
        public string CellPhone { get; set; }

        /// <summary> SSO帳號 </summary>
        public string SSOAccount { get; set; }

        /// <summary> SSO角色 </summary>
        public string SSORole { get; set; }

        /// <summary> SSO姓名 </summary>
        public string SSOName { get; set; }

        /// <summary> SSO系級/單位 </summary>
        public string SSODepartment { get; set; }

        /// <summary> 最後登入時間 </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary> 記錄登入錯誤次數 </summary>
        public int ErrorCount { get; set; }

        /// <summary> 是否啟用(0:不啟用 </summary>
        public bool IsEnable { get; set; }

        /// <summary> 1:啟用) </summary>
        public string Creator { get; set; }

        /// <summary> 建立者 </summary>
        public DateTime Created { get; set; }

        /// <summary> 建立日期時間 </summary>
        public string LastModifier { get; set; }

        /// <summary> 最後異動者 </summary>
        public DateTime? LastModified { get; set; }

        /// <summary> 最後修改日期時間 </summary>
        public string ModifiedReason { get; set; }

        /// <summary> 使用者角色 </summary>
        public List<RoleInfo> UserRole { get; set; }

        /// <summary> 使用者角色功能 </summary>
        public List<FunInfo> UserRoleFun { get; set; }

        /// <summary> 檢查使用者是否有該功能的使用權限 </summary>
        public bool IsAllowAction(string URL)
        {
            return UserRoleFun.FindAll(p => !string.IsNullOrEmpty(p.Url) && p.Url.ToLower() == URL.ToLower() && p.IsEnable == true).Count > 0;
        }

        /// <summary> 根據 URL 取得 MenuNode (僅能查詢到有權限的 MenuNode )</summary>
        public string GetMenuNodeByURL(string URL)
        {
			return UserRoleFun.Where(p => p.Url == URL).FirstOrDefault() != null ? UserRoleFun.Where(p => p.Url == URL).FirstOrDefault().MenuNode : null;
        }

        /// <summary> 根據 MenuNode 取得上層選單 </summary>
        public string GetMenuNodeByParentMenuNode(string MenuNode)
        {
            return UserRoleFun.Find(p => p.MenuNode == MenuNode) != null ? UserRoleFun.Find(p => p.MenuNode == MenuNode).MenuUpNode : null;
        }

        /// <summary> 取得所有最上層選單 </summary>
        public List<FunInfo> GetAllParentNode()
        {
            return UserRoleFun.FindAll(p => p.MenuNode == "-1");
        }
    }
}
