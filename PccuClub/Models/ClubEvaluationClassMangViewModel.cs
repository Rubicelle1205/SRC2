using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubEvaluationClassMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubEvaluationClassMangConditionModel ConditionModel { get; set; }

        public List<ClubEvaluationClassMangResultModel> ResultModel { get; set; }

        public ClubEvaluationClassMangCreateModel CreateModel { get; set; }

        public ClubEvaluationClassMangEditModel EditModel { get; set; }

    }

    public class ClubEvaluationClassMangConditionModel
    {
        public ClubEvaluationClassMangConditionModel()
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

		/// <summary>名稱</summary>
		[DisplayName("名稱")]
        public string? ClassName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ClubEvaluationClassMangResultModel
    {
		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		public int? ClubEvaluationClassId { get; set; }

        /// <summary>類別名稱</summary>
        [DisplayName("類別名稱")]
        public string? ClassName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class ClubEvaluationClassMangCreateModel
    {
		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		/// <summary>類別名稱</summary>
		[DisplayName("類別名稱")]
        public string? ClassName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ClubEvaluationClassMangEditModel
    {
        public int? ClubEvaluationClassId { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		/// <summary>類別名稱</summary>
		[DisplayName("類別名稱")]
        public string? ClassName { get; set; }

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
