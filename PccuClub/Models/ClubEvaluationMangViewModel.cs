using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubEvaluationMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubEvaluationMangConditionModel ConditionModel { get; set; }

        public List<ClubEvaluationMangResultModel> ResultModel { get; set; }

        public ClubEvaluationMangCreateModel CreateModel { get; set; }

        public ClubEvaluationMangEditModel EditModel { get; set; }

    }

    public class ClubEvaluationMangConditionModel
    {
        public ClubEvaluationMangConditionModel()
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

        /// <summary>社團</summary>
        [DisplayName("社團")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }

        /// <summary>評鑑類別</summary>
        [DisplayName("評鑑類別")]
        public string? ClubEvaluationClassId { get; set; }

        /// <summary>評鑑項目</summary>
        [DisplayName("評鑑項目")]
        public string? ClubEvaluationItemId { get; set; }

    }

    public class ClubEvaluationMangResultModel
    {

        public int? ClubEvaluationId { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>社團</summary>
        [DisplayName("社團")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>評鑑類別</summary>
        [DisplayName("評鑑類別")]
        public string? ClubEvaluationClassId { get; set; }

        /// <summary>評鑑項目</summary>
        [DisplayName("評鑑項目")]
        public string? ClubEvaluationItemId { get; set; }

        /// <summary>評鑑類別</summary>
        [DisplayName("評鑑類別")]
        public string? ClassName { get; set; }

        /// <summary>評鑑項目</summary>
        [DisplayName("評鑑項目")]
        public string? ItemName { get; set; }

        /// <summary>分數</summary>
        [DisplayName("分數")]
        public string? Score { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class ClubEvaluationMangCreateModel
    {
		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>社團</summary>
        [DisplayName("社團")]
        public string? ClubID { get; set; }

        /// <summary>評鑑類別</summary>
        [DisplayName("評鑑類別")]
        public string? ClubEvaluationClassId { get; set; }

        /// <summary>評鑑項目</summary>
        [DisplayName("評鑑項目")]
        public string? ClubEvaluationItemId { get; set; }

        /// <summary>分數</summary>
        [DisplayName("分數")]
        public string? Score { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ClubEvaluationMangEditModel
    {
        public int? ClubEvaluationId { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>社團</summary>
        [DisplayName("社團")]
        public string? ClubID { get; set; }

        /// <summary>評鑑類別</summary>
        [DisplayName("評鑑類別")]
        public string? ClubEvaluationClassId { get; set; }

        /// <summary>評鑑項目</summary>
        [DisplayName("評鑑項目")]
        public string? ClubEvaluationItemId { get; set; }

        /// <summary>分數</summary>
        [DisplayName("分數")]
        public string? Score { get; set; }

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
