using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class HolisticSecondClassMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public HolisticSecondClassMangConditionModel ConditionModel { get; set; }

        public List<HolisticSecondClassMangResultModel> ResultModel { get; set; }

        public HolisticSecondClassMangCreateModel CreateModel { get; set; }

        public HolisticSecondClassMangEditModel EditModel { get; set; }

    }

    public class HolisticSecondClassMangConditionModel
    {
        public HolisticSecondClassMangConditionModel()
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
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class HolisticSecondClassMangResultModel
    {
        public int? ID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? Text { get; set; }

        /// <summary>群組</summary>
        [DisplayName("群組")]
        public string? MainID { get; set; }

        /// <summary>群組</summary>
        [DisplayName("群組")]
        public string? MainIDText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class HolisticSecondClassMangCreateModel
    {
        /// <summary>群組</summary>
        [DisplayName("群組")]
        public string? MainID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class HolisticSecondClassMangEditModel
    {
        public int? ID { get; set; }

        /// <summary>群組</summary>
        [DisplayName("群組")]
        public string? MainID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public string? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        public string? LastModified { get; set; }
    }
}
