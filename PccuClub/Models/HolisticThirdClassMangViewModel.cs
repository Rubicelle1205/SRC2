using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class HolisticThirdClassMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public HolisticThirdClassMangConditionModel ConditionModel { get; set; }

        public List<HolisticThirdClassMangResultModel> ResultModel { get; set; }

        public HolisticThirdClassMangCreateModel CreateModel { get; set; }

        public HolisticThirdClassMangEditModel EditModel { get; set; }

    }

    public class HolisticThirdClassMangConditionModel
    {
        public HolisticThirdClassMangConditionModel()
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

    public class HolisticThirdClassMangResultModel
    {
        public int? ID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? Text { get; set; }

        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? MainID { get; set; }

        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? MainIDText { get; set; }

        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? SecondID { get; set; }

        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? SecondIDText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class HolisticThirdClassMangCreateModel
    {
        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? SecondID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class HolisticThirdClassMangEditModel
    {
        public int? ID { get; set; }

        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? SecondID { get; set; }

        /// <summary>名稱</summary>
        [DisplayName("名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        public DateTime? LastModified { get; set; }
    }
}
