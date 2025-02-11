using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class EventCaseReferMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public EventCaseReferMangConditionModel ConditionModel { get; set; }

        public List<EventCaseReferMangResultModel> ResultModel { get; set; }

        public EventCaseReferMangCreateModel CreateModel { get; set; }

        public EventCaseReferMangEditModel EditModel { get; set; }

    }

    public class EventCaseReferMangConditionModel
    {
        public EventCaseReferMangConditionModel()
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

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ReferName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventCaseReferMangResultModel
    {
        public int? ReferID { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ReferName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventCaseReferMangCreateModel
    {
        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ReferName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventCaseReferMangEditModel
    {
        public int? ReferID { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ReferName { get; set; }

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
