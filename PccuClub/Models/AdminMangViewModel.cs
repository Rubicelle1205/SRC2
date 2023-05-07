using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class AdminMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public AdminMangConditionModel ConditionModel { get; set; }

        public List<AdminMangResultModel> ResultModel { get; set; }

        public AdminMangCreateModel CreateModel { get; set; }

        public AdminMangEditModel EditModel { get; set; }
    }

    public class AdminMangConditionModel
    {
        public AdminMangConditionModel()
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

        /// <summary>組別</summary>
        [DisplayName("組別")]
        public string? LifeClass { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string IsEnable { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class AdminMangResultModel
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

        /// <summary>組別</summary>
        [DisplayName("組別")]
        public string? LifeClassText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

    }

    public class AdminMangCreateModel
    {

        /// <summary>管理員帳號</summary>
        [DisplayName("管理員帳號")]
        public string? LoginId { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? Password { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? ConformPassword { get; set; }
        
        /// <summary>管理員名稱</summary>
        [DisplayName("管理員名稱")]
        public string? UserName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? Enable { get; set; }

        /// <summary>角色</summary>
        [DisplayName("角色")]
        public string? RoleId { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>單位</summary>
        [DisplayName("單位")]
        public string? Department { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? EnableText { get; set; }

        /// <summary>組別</summary>
        [DisplayName("組別")]
        public string? LifeClass { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class AdminMangEditModel
    {
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
        public string? Password { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? ConformPassword { get; set; }

        /// <summary>管理員名稱</summary>
        [DisplayName("管理員名稱")]
        public string? UserName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? IsEnable { get; set; }

        /// <summary>角色</summary>
        [DisplayName("角色")]
        public string? RoleId { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>單位</summary>
        [DisplayName("單位")]
        public string? Department { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? EnableText { get; set; }

        /// <summary>組別</summary>
        [DisplayName("組別")]
        public string? LifeClass { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }
}
