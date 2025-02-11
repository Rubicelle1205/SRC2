using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class RoleMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public RoleMangConditionModel ConditionModel { get; set; }

        public List<RoleMangResultModel> ResultModel { get; set; }

        public RoleMangCreateModel CreateModel { get; set; }

        public RoleMangEditModel EditModel { get; set; }
    }

    public class RoleMangConditionModel
    {
        public RoleMangConditionModel()
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

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色</summary>
        [DisplayName("角色")]
        public string? RoleName { get; set; }

        /// <summary>權限	</summary>
        [DisplayName("權限")]
        public string? MenuNode { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class RoleMangResultModel
    {
        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色名稱</summary>
        [DisplayName("角色名稱")]
        public string? RoleName { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// 權限
        /// </summary>
        public List<FunInfo> LstFunInfo = new List<FunInfo>();
    }

    public class RoleMangCreateModel
    {
        /// <summary>
        /// 已有權限
        /// </summary>
        public List<SelectListItem> LstFunItem = new List<SelectListItem>();

        /// <summary>
        /// 新權限
        /// </summary>
        public string? strFunInfo { get; set; }

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色名稱</summary>
        [DisplayName("角色名稱")]
        public string? RoleName { get; set; }

        /// <summary>角色說明</summary>
        [DisplayName("角色說明")]
        public string? Comment { get; set; }

        /// <summary>角色種類</summary>
        [DisplayName("角色種類")]
        public string? SystemRoleCode { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }
    }

    public class RoleMangEditModel
    {
        /// <summary>
        /// 已有權限
        /// </summary>
        public List<SelectListItem> LstFunItem = new List<SelectListItem>();

        /// <summary>
        /// 新權限
        /// </summary>
        public string? strFunInfo { get; set; }

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色</summary>
        [DisplayName("角色")]
        public string? RoleName { get; set; }

        /// <summary>角色種類</summary>
        [DisplayName("角色種類")]
        public string? SystemRoleCode { get; set; }

        /// <summary>角色說明</summary>
        [DisplayName("角色說明")]
        public string? Comment { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }

        /// <summary>更新者</summary>
        [DisplayName("更新者")]
        public string? LastModifier { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }
    }

    public class FunSelectedItem
    { 
        public string Value { get; set; }
        public string Text { get; set; }
        public string Group { get; set; }
        public string SystemCode { get; set; }
        public string SystemCodeText { get; set; }
    }
}
