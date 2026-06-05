using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class SystemLogMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public SystemLogMangConditionModel ConditionModel { get; set; }

        public List<SystemLogMangResultModel> ResultModel { get; set; }
    }

    public class SystemLogMangConditionModel
    {
        public SystemLogMangConditionModel()
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

        /// <summary>登入帳號</summary>
        [DisplayName("登入帳號")]
        public string? LoginId { get; set; }

        /// <summary>使用者名稱</summary>
        [DisplayName("使用者名稱")]
        public string? UserName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class SystemLogMangResultModel
    {
        public string? LU_Action_Id { get; set; }

        /// <summary>登入帳號</summary>
        [DisplayName("登入帳號")]
        public string? LoginId { get; set; }

        /// <summary>使用者名稱</summary>
        [DisplayName("使用者名稱")]
        public string? UserName { get; set; }

        /// <summary>角色</summary>
        [DisplayName("角色")]
        public string? RoleName { get; set; }

        /// <summary>功能名稱</summary>
        [DisplayName("功能名稱")]
        public string? FunName { get; set; }

        /// <summary>動作名稱</summary>
        [DisplayName("動作名稱")]
        public string? ActionName { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Create_Date { get; set; }
    }

}
