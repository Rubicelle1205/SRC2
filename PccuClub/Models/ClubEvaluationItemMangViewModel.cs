using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubEvaluationItemMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubEvaluationItemMangConditionModel ConditionModel { get; set; }

        public List<ClubEvaluationItemMangResultModel> ResultModel { get; set; }

        public ClubEvaluationItemMangCreateModel CreateModel { get; set; }

        public ClubEvaluationItemMangEditModel EditModel { get; set; }

    }

    public class ClubEvaluationItemMangConditionModel
    {
        public ClubEvaluationItemMangConditionModel()
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

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>類別名稱</summary>
		[DisplayName("類別名稱")]
        public string? ClassId { get; set; }

        /// <summary>項目名稱</summary>
        [DisplayName("項目名稱")]
        public string? ItemName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ClubEvaluationItemMangResultModel
    {
		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		public int? ClubEvaluationItemId { get; set; }

        /// <summary>類別名稱</summary>
		[DisplayName("類別名稱")]
        public string? ClassId { get; set; }

        /// <summary>類別名稱</summary>
		[DisplayName("類別名稱")]
        public string? ClassName { get; set; }

        /// <summary>項目名稱</summary>
        [DisplayName("項目名稱")]
        public string? ItemName { get; set; }

        /// <summary>分數上限</summary>
        [DisplayName("分數上限")]
        public string? ScoreUpper { get; set; }

        /// <summary>分數下限</summary>
        [DisplayName("分數下限")]
        public string? ScoreLower { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class ClubEvaluationItemMangCreateModel
    {
		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>類別名稱</summary>
		[DisplayName("類別名稱")]
        public string? ClassId { get; set; }

        /// <summary>項目名稱</summary>
        [DisplayName("項目名稱")]
        public string? ItemName { get; set; }

        /// <summary>分數上限</summary>
        [DisplayName("分數上限")]
        public string? ScoreUpper { get; set; }

        /// <summary>分數下限</summary>
        [DisplayName("分數下限")]
        public string? ScoreLower { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ClubEvaluationItemMangEditModel
    {
        public int? ClubEvaluationItemId { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>類別名稱</summary>
		[DisplayName("類別名稱")]
        public string? ClassId { get; set; }

        /// <summary>項目名稱</summary>
        [DisplayName("項目名稱")]
        public string? ItemName { get; set; }

        /// <summary>分數上限</summary>
        [DisplayName("分數上限")]
        public string? ScoreUpper { get; set; }

        /// <summary>分數下限</summary>
        [DisplayName("分數下限")]
        public string? ScoreLower { get; set; }

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
