using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class FloorMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public FloorMangConditionModel ConditionModel { get; set; }

        public List<FloorMangResultModel> ResultModel { get; set; }

        public FloorMangCreateModel CreateModel { get; set; }

        public FloorMangEditModel EditModel { get; set; }

        public FloorMangExcelResultModel ExcelModel { get; set; }
    }

    public class FloorMangConditionModel
    {
        public FloorMangConditionModel()
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

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? FloorName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class FloorMangResultModel
    {
        public int? FloorID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? FloorName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class FloorMangExcelResultModel
    {
        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? FloorName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class FloorMangCreateModel
    {
        public int? FloorID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? FloorName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class FloorMangEditModel
    {
        public int? FloorID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? FloorName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }
}
