using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BuildMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BuildMangConditionModel ConditionModel { get; set; }

        public List<BuildMangResultModel> ResultModel { get; set; }

        public BuildMangCreateModel CreateModel { get; set; }

        public BuildMangEditModel EditModel { get; set; }

        public BuildMangExcelResultModel ExcelModel { get; set; }
    }

    public class BuildMangConditionModel
    {
        public BuildMangConditionModel()
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

        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Note { get; set; }
    }

    public class BuildMangResultModel
    {
        public int? BuildID { get; set; }

        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Note { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }
    }

    public class BuildMangExcelResultModel
    {
        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Note { get; set; }
    }

    public class BuildMangCreateModel
    {
        public int? BuildID { get; set; }

        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Note { get; set; }
    }

    public class BuildMangEditModel
    {
        public int? BuildID { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Note { get; set; }
    }
}
