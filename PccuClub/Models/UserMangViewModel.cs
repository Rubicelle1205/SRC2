using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class UserMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public UserMangConditionModel ConditionModel { get; set; }

        public List<UserMangResultModel> ResultModel { get; set; }

        public UserMangCreateModel CreateModel { get; set; }

        public UserMangEditModel EditModel { get; set; }
    }

    public class UserMangConditionModel
    {
        public UserMangConditionModel()
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

        /// <summary>社團帳號</summary>
        [DisplayName("社團帳號")]
        public string? Clubid { get; set; }

        /// <summary>負責人姓名</summary>
        [DisplayName("負責人姓名")]
        public string? UserName { get; set; }

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>組別</summary>
        [DisplayName("組別")]
        public string? LifeClass { get; set; }

        /// <summary>負責人電話</summary>
        [DisplayName("負責人電話")]
        public string CellPhone { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class UserMangResultModel
    {
        [DisplayName("單位帳號")]
        public string? Clubid { get; set; }

        [DisplayName("單位名稱")]
        public string? ClubCName { get; set; }

        /// <summary>單位身分</summary>
        [DisplayName("單位身分")]
        public string? Roleid { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? RoleName { get; set; }

        /// <summary>單位組別</summary>
        [DisplayName("單位組別")]
        public string? LifeClass { get; set; }

        /// <summary>單位組別名稱</summary>
        [DisplayName("單位組別名稱")]
        public string? LifeClassName { get; set; }

        /// <summary>負責人姓名</summary>
        [DisplayName("負責人姓名")]
        public string? UserName { get; set; }

        /// <summary>電話</summary>
        [DisplayName("電話")]
        public string? CellPhone { get; set; }

        /// <summary>E-mail</summary>
        [DisplayName("E-mail")]
        public string? EMail { get; set; }

        [DisplayName("最後登入時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? IsEnableText { get; set; }

    }

    public class UserMangCreateModel
    {

        /// <summary>使用者帳號</summary>
        [DisplayName("使用者帳號")]
        public string? LoginId { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? Password { get; set; }

        /// <summary>密碼</summary>
        [DisplayName("密碼")]
        public string? ConformPassword { get; set; }
        
        /// <summary>使用者名稱</summary>
        [DisplayName("使用者名稱")]
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

    public class UserMangEditModel
    {
        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

        /// <summary>單位帳號</summary>
        [DisplayName("單位帳號")]
        public string? ClubId { get; set; }

        /// <summary>單位中文名稱</summary>
        [DisplayName("單位中文名稱")]
        public string? ClubCName { get; set; }

        /// <summary>單位英文名稱</summary>
        [DisplayName("單位英文名稱")]
        public string? ClubEName { get; set; }

        /// <summary>學校/人員代號</summary>
        [DisplayName("學校/人員代號")]
        public string? FUserId { get; set; }

        /// <summary>學校/人員代號</summary>
        [DisplayName("學校/人員代號")]
        public string? OldFUserId { get; set; }

        /// <summary>負責人姓名</summary>
        [DisplayName("負責人姓名")]
        public string? UserName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? IsEnable { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>連絡電話</summary>
        [DisplayName("連絡電話")]
        public string? CellPhone { get; set; }

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
