using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ActTypeMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ActTypeMangConditionModel ConditionModel { get; set; }

        public List<ActTypeMangResultModel> ResultModel { get; set; }

        public ActTypeMangCreateModel CreateModel { get; set; }

        public ActTypeMangEditModel EditModel { get; set; }

        public ActTypeMangExcelResultModel ExcelModel { get; set; }
    }

    public class ActTypeMangConditionModel
    {
        public ActTypeMangConditionModel()
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
        public string? ActTypeName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ActTypeMangResultModel
    {
        public int? ActTypeID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? ActTypeName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ActTypeMangExcelResultModel
    {
        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? ActTypeName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ActTypeMangCreateModel
    {
        public int? ActTypeID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? ActTypeName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ActTypeMangEditModel
    {
        public int? ActTypeID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? ActTypeName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }
}
