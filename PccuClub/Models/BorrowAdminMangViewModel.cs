using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class BorrowAdminMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowAdminMangConditionModel ConditionModel { get; set; }

        public List<BorrowAdminMangResultModel> ResultModel { get; set; }

        public BorrowAdminMangCreateModel CreateModel { get; set; }

        public BorrowAdminMangEditModel EditModel { get; set; }
    }

    public class BorrowAdminMangConditionModel
    {
        public BorrowAdminMangConditionModel()
        {
            this.Page = 0;
            this.PageSize = 10;
            this.TotalCount = 0;
        }

        /// <summary> 目前頁數 </summary>
        public int Page { get; set; }

        /// <summary> 預設每頁顯示筆數 - 依需求更改 </summary>
        public int PageSize { get; set; }

        /// <summary> 總筆數 </summary>
        public int TotalCount { get; set; }

        /// <summary>管理員帳號</summary>
        [DisplayName("管理員帳號")]
        public string? LoginId { get; set; }

        /// <summary>管理員名稱</summary>
        [DisplayName("管理員名稱")]
        public string? UserName { get; set; }

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string IsEnable { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class BorrowAdminMangResultModel
    {
        /// <summary>管理員帳號</summary>
        [DisplayName("管理員帳號")]
        public string? LoginId { get; set; }

        /// <summary>管理員名稱</summary>
        [DisplayName("管理員名稱")]
        public string? UserName { get; set; }

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色名稱</summary>
        [DisplayName("角色名稱")]
        public string? RoleName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? EnableText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

    }

    public class BorrowAdminMangCreateModel
    {
        public List<string> LstCanUseFun = new List<string>();

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

        /// <summary>管理員帳號</summary>
        [DisplayName("管理員帳號")]
        public string? LoginId { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? Pwd { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? ConformPwd { get; set; }

        /// <summary>管理員名稱</summary>
        [DisplayName("管理員名稱")]
        public string? UserName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? IsEnable { get; set; }

        /// <summary>角色-資源借用</summary>
        [DisplayName("角色-資源借用")]
        public string? RoleConsultation { get; set; }

        /// <summary>學校/人員代號</summary>
        [DisplayName("學校/人員代號")]
        public string? SSOAccount { get; set; }
        
        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>單位</summary>
        [DisplayName("單位")]
        public string? Department { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? EnableText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowAdminMangEditModel
    {
        public List<string> LstCanUseFun = new List<string>();

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

        /// <summary>管理員帳號</summary>
        [DisplayName("管理員帳號")]
        public string? LoginId { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? Pwd { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? ConformPwd { get; set; }

        /// <summary>管理員名稱</summary>
        [DisplayName("管理員名稱")]
        public string? UserName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? IsEnable { get; set; }

        /// <summary>角色-社團系統</summary>
        [DisplayName("角色-社團系統")]
        public string? RoleClub { get; set; }

        /// <summary>角色-案件系統</summary>
        [DisplayName("角色-案件系統")]
        public string? RoleCase { get; set; }

        /// <summary>角色-借用系統</summary>
        [DisplayName("角色-借用系統")]
        public string? RoleBorrow { get; set; }

        /// <summary>角色-資源借用</summary>
        [DisplayName("角色-資源借用")]
        public string? RoleConsultation { get; set; }

        /// <summary>學校/人員代號</summary>
        [DisplayName("學校/人員代號")]
        public string? SSOAccount { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>單位</summary>
        [DisplayName("單位")]
        public string? Department { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? EnableText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }
}
