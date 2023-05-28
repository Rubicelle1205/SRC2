using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubPersonMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubCadreMangConditionModel CadreMangConditionModel { get; set; }

        public ClubMemberMangConditionModel MemberMangConditionModel { get; set; }

		public List<ClubCadreMangResultModel> CadreMangResultModel { get; set; }

        public ClubCadreMangCreateModel CadreMangCreateModel { get; set; }

		public List<ClubCadreMangExcelResultModel> CadreMangExcelModel { get; set; }

		public ClubCadreMangEditModel CadreMangEditModel { get; set; }

        public ClubCadreMangPersonalConsentModel CadreMangPersonalConsentModel { get; set; }

        public List<ClubMemberMangResultModel> MemberMangResultModel { get; set; }

        

    }

    public class ClubCadreMangConditionModel
    {
        public ClubCadreMangConditionModel()
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
    }

    public class ClubCadreMangResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? CadreID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public DateTime? SDuring { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public DateTime? EDuring { get; set; }

        /// <summary>職別</summary>
        [DisplayName("職別")]
        public string? CadreName { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNo { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>生理性別</summary>
        [DisplayName("生理性別")]
        public string? Sex { get; set; }

        /// <summary>生理性別</summary>
        [DisplayName("生理性別")]
        public string? SexText { get; set; }
        
        /// <summary>聯絡電話</summary>
        [DisplayName("聯絡電話")]
        public string? CellPhone { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ClubCadreMangCreateModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? CadreID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public string? SDuring { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public string? EDuring { get; set; }

        /// <summary>職別</summary>
        [DisplayName("職別")]
        public string? CadreName { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNo { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>生理性別</summary>
        [DisplayName("生理性別")]
        public string? Sex { get; set; }

        /// <summary>生理性別</summary>
        [DisplayName("生理性別")]
        public string? SexText { get; set; }

        /// <summary>聯絡電話</summary>
        [DisplayName("聯絡電話")]
        public string? CellPhone { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ClubCadreMangEditModel
	{
		/// <summary>代號</summary>
		[DisplayName("代號")]
		public string? CadreID { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		/// <summary>任職期間</summary>
		[DisplayName("任職期間")]
		public string? SDuring { get; set; }

		/// <summary>任職期間</summary>
		[DisplayName("任職期間")]
		public string? EDuring { get; set; }

		/// <summary>職別</summary>
		[DisplayName("職別")]
		public string? CadreName { get; set; }

		/// <summary>姓名</summary>
		[DisplayName("姓名")]
		public string? UserName { get; set; }

		/// <summary>信箱</summary>
		[DisplayName("信箱")]
		public string? EMail { get; set; }

		/// <summary>學號</summary>
		[DisplayName("學號")]
		public string? SNo { get; set; }

		/// <summary>系級</summary>
		[DisplayName("系級")]
		public string? Department { get; set; }

		/// <summary>生理性別</summary>
		[DisplayName("生理性別")]
		public string? Sex { get; set; }

		/// <summary>生理性別</summary>
		[DisplayName("生理性別")]
		public string? SexText { get; set; }

		/// <summary>聯絡電話</summary>
		[DisplayName("聯絡電話")]
		public string? CellPhone { get; set; }

		/// <summary>備註</summary>
		[DisplayName("備註")]
		public string? Memo { get; set; }
	}

    public class ClubCadreMangExcelResultModel
	{
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubName { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>職別</summary>
        [DisplayName("職別")]
        public string? CadreName { get; set; }

        /// <summary>任職開始期間</summary>
        [DisplayName("任職期間")]
        public DateTime? SDuring { get; set; }

        /// <summary>任職結束期間</summary>
        [DisplayName("任職期間")]
        public DateTime? EDuring { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

	public class ClubCadreMangImportExcelResultModel
	{
		/// <summary>社團代號</summary>
		[DisplayName("社團代號")]
		public string? ClubID { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		/// <summary>任職期間</summary>
		[DisplayName("任職期間")]
		public string? SDuring { get; set; }

		/// <summary>任職期間</summary>
		[DisplayName("任職期間")]
		public string? EDuring { get; set; }

		/// <summary>職別</summary>
		[DisplayName("職別")]
		public string? CadreName { get; set; }

		/// <summary>姓名</summary>
		[DisplayName("姓名")]
		public string? UserName { get; set; }

		/// <summary>信箱</summary>
		[DisplayName("信箱")]
		public string? EMail { get; set; }

		/// <summary>學號</summary>
		[DisplayName("學號")]
		public string? SNo { get; set; }

		/// <summary>系級</summary>
		[DisplayName("系級")]
		public string? Department { get; set; }

		/// <summary>性別</summary>
		[DisplayName("性別")]
		public string? Sex { get; set; }

		/// <summary>聯絡電話</summary>
		[DisplayName("聯絡電話")]
		public string? CellPhone { get; set; }

		/// <summary>備註</summary>
		[DisplayName("備註")]
		public string? Memo { get; set; }
	}

    public class ClubCadreMangPersonalConsentModel 
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>PersonalConsent</summary>
        [DisplayName("PersonalConsent")]
        public string? PersonalConsent { get; set; }

        
    }

	public class ClubMemberMangConditionModel
    {
        public ClubMemberMangConditionModel()
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
    }

    public class ClubMemberMangResultModel
    {


    }
}
