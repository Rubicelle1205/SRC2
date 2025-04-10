using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubBasicScoreMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubBasicScoreMangConditionModel ConditionModel { get; set; }

        public List<ClubBasicScoreMangResultModel> ResultModel { get; set; }

        public ClubBasicScoreMangCreateModel CreateModel { get; set; }

        public ClubBasicScoreMangEditModel EditModel { get; set; }

    }

    public class ClubBasicScoreMangConditionModel
    {
        public ClubBasicScoreMangConditionModel()
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

		/// <summary>基本分</summary>
		[DisplayName("基本分")]
        public string? BasicScore { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ClubBasicScoreMangResultModel
    {

        public int? ClubBasicScoreId { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>基本分</summary>
		[DisplayName("基本分")]
        public string? BasicScore { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class ClubBasicScoreMangCreateModel
    {
		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>基本分</summary>
        [DisplayName("基本分")]
        public string? BasicScore { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ClubBasicScoreMangEditModel
    {
        public int? ClubBasicScoreId { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

        /// <summary>基本分</summary>
        [DisplayName("基本分")]
        public string? BasicScore { get; set; }

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
