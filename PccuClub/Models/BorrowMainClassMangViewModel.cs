using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BorrowMainClassMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowMainClassMangConditionModel ConditionModel { get; set; }

        public List<BorrowMainClassMangResultModel> ResultModel { get; set; }

        public BorrowMainClassMangCreateModel CreateModel { get; set; }

        public BorrowMainClassMangEditModel EditModel { get; set; }

    }

    public class BorrowMainClassMangConditionModel
    {
        public BorrowMainClassMangConditionModel()
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

        /// <summary>分類名稱</summary>
        [DisplayName("分類名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowMainClassMangResultModel
    {
        public int? ID { get; set; }

        /// <summary>分類名稱</summary>
        [DisplayName("分類名稱")]
        public string? Text { get; set; }

        /// <summary>可借用日期</summary>
        [DisplayName("可借用日期")]
        public DateTime? BorrowSDate { get; set; }

        /// <summary>可借用日期</summary>
        [DisplayName("可借用日期")]
        public DateTime? BorrowEDate { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowMainClassMangCreateModel
    {
        /// <summary>分類名稱</summary>
        [DisplayName("分類名稱")]
        public string? Text { get; set; }

        /// <summary>審核單位</summary>
        [DisplayName("審核單位")]
        public string? ActVerifyUnit { get; set; }

        /// <summary>可借用日期</summary>
        [DisplayName("可借用日期")]
        public DateTime? BorrowSDate { get; set; }

        /// <summary>可借用日期</summary>
        [DisplayName("可借用日期")]
        public DateTime? BorrowEDate { get; set; }

        /// <summary>借用規定</summary>
        [DisplayName("借用規定")]
        public string? BorrowRule { get; set; }

        /// <summary>費用</summary>
        [DisplayName("費用")]
        public string? BorrowFee { get; set; }

        /// <summary>庫存機制</summary>
        [DisplayName("庫存機制")]
        public string? ReserveRule { get; set; }

        /// <summary>封面圖片</summary>
        [DisplayName("封面圖片")]
        public string? CoverPath { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowMainClassMangEditModel
    {
        public int? ID { get; set; }

        /// <summary>分類名稱</summary>
        [DisplayName("分類名稱")]
        public string? Text { get; set; }

        /// <summary>審核單位</summary>
        [DisplayName("審核單位")]
        public string? ActVerifyUnit { get; set; }

        /// <summary>可借用日期</summary>
        [DisplayName("可借用日期")]
        public DateTime? BorrowSDate { get; set; }

        /// <summary>可借用日期</summary>
        [DisplayName("可借用日期")]
        public DateTime? BorrowEDate { get; set; }

        /// <summary>借用規定</summary>
        [DisplayName("借用規定")]
        public string? BorrowRule { get; set; }

        /// <summary>費用</summary>
        [DisplayName("費用")]
        public string? BorrowFee { get; set; }

        /// <summary>庫存機制</summary>
        [DisplayName("庫存機制")]
        public string? ReserveRule { get; set; }

        /// <summary>封面圖片</summary>
        [DisplayName("封面圖片")]
        public string? CoverPath { get; set; }

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
