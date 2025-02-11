using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class EventGenderSecondClassMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public EventGenderSecondClassMangConditionModel ConditionModel { get; set; }

        public List<EventGenderSecondClassMangResultModel> ResultModel { get; set; }

        public EventGenderSecondClassMangCreateModel CreateModel { get; set; }

        public EventGenderSecondClassMangEditModel EditModel { get; set; }

    }

    public class EventGenderSecondClassMangConditionModel
    {
        public EventGenderSecondClassMangConditionModel()
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

        /// <summary>主類別</summary>
        [DisplayName("主類別")]
        public string? MainID { get; set; }

        /// <summary>次類別</summary>
        [DisplayName("次類別")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventGenderSecondClassMangResultModel
    {
        public int? ID { get; set; }

        /// <summary>主類別</summary>
        [DisplayName("主類別")]
        public string? MainID { get; set; }

        /// <summary>主類別</summary>
        [DisplayName("主類別")]
        public string? MainIDText { get; set; }

        /// <summary>次類別</summary>
        [DisplayName("次類別")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventGenderSecondClassMangCreateModel
    {
        /// <summary>主類別</summary>
        [DisplayName("主類別")]
        public string? MainID { get; set; }

        /// <summary>次類別</summary>
        [DisplayName("次類別")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventGenderSecondClassMangEditModel
    {
        public int? ID { get; set; }

        /// <summary>主類別</summary>
        [DisplayName("主類別")]
        public string? MainID { get; set; }

        /// <summary>次類別</summary>
        [DisplayName("次類別")]
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
