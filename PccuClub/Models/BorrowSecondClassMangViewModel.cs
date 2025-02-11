using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BorrowSecondClassMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowSecondClassMangConditionModel ConditionModel { get; set; }

        public List<BorrowSecondClassMangResultModel> ResultModel { get; set; }

        public BorrowSecondClassMangCreateModel CreateModel { get; set; }

        public BorrowSecondClassMangEditModel EditModel { get; set; }

    }

    public class BorrowSecondClassMangConditionModel
    {
        public BorrowSecondClassMangConditionModel()
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

        /// <summary>主分類</summary>
        [DisplayName("主分類")]
        public string? MainClass { get; set; }

        /// <summary>子分類名稱</summary>
        [DisplayName("子分類名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowSecondClassMangResultModel
    {
        public int? ID { get; set; }

        /// <summary>主分類</summary>
        [DisplayName("主分類")]
        public string? MainClass { get; set; }

        /// <summary>主分類</summary>
        [DisplayName("主分類")]
        public string? MainClassText { get; set; }

        /// <summary>子分類名稱</summary>
        [DisplayName("子分類名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowSecondClassMangCreateModel
    {
        /// <summary>主分類</summary>
        [DisplayName("主分類")]
        public string? MainClass { get; set; }

        /// <summary>子分類名稱</summary>
        [DisplayName("子分類名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowSecondClassMangEditModel
    {
        public int? ID { get; set; }

        /// <summary>主分類</summary>
        [DisplayName("主分類")]
        public string? MainClass { get; set; }

        /// <summary>子分類名稱</summary>
        [DisplayName("子分類名稱")]
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
