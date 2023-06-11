using NPOI.HPSF;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubActReportViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubActReportConditionModel ConditionModel { get; set; }

		public List<ClubActReportResultModel> ResultModel { get; set; }

        public ClubActReportCreateModel CreateModel { get; set; }

		public ClubActReportEditModel EditModel { get; set; }

        public ClubActReportConsentModel ConsentModel { get; set; }

    }

    public class ClubActReportConditionModel
    {
        public ClubActReportConditionModel()
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

    public class ClubActReportResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? ActID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActVerify { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActVerifyText { get; set; }

        /// <summary>報備日期</summary>
        [DisplayName("報備日期")]
        public DateTime? Created { get; set; }
    }

    public class ClubActReportCreateModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動類型</summary>
        [DisplayName("活動類型")]
        public string? StaticOrDynamic { get; set; }

        /// <summary>活動地點</summary>
        [DisplayName("活動地點")]
        public string? ActInOrOut { get; set; }

        /// <summary>活動人數</summary>
        [DisplayName("活動人數")]
        public string? Capacity { get; set; }

        /// <summary>活動性質</summary>
        [DisplayName("活動性質")]
        public string? ActType { get; set; }

        /// <summary>使用資訊設備</summary>
        [DisplayName("使用資訊設備")]
        public string? UseITEquip { get; set; }

        /// <summary>活動簡介</summary>
        [DisplayName("活動簡介")]
        public string? ShortDesc { get; set; }

        /// <summary>SDGs</summary>
        [DisplayName("SDGs")]
        public string? SDGs { get; set; }

        /// <summary>全人學習護照</summary>
        [DisplayName("全人學習護照")]
        public string? PassPort { get; set; }
    }

    public class Section1
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動類型</summary>
        [DisplayName("活動類型")]
        public string? StaticOrDynamic { get; set; }

        /// <summary>活動地點</summary>
        [DisplayName("活動地點")]
        public string? ActInOrOut { get; set; }

        /// <summary>活動人數</summary>
        [DisplayName("活動人數")]
        public string? Capacity { get; set; }

        /// <summary>活動性質</summary>
        [DisplayName("活動性質")]
        public string? ActType { get; set; }

        /// <summary>使用資訊設備</summary>
        [DisplayName("使用資訊設備")]
        public string? UseITEquip { get; set; }

        /// <summary>活動簡介</summary>
        [DisplayName("活動簡介")]
        public string? ShortDesc { get; set; }

        /// <summary>SDGs</summary>
        [DisplayName("SDGs")]
        public string? SDGs { get; set; }

        /// <summary>全人學習護照</summary>
        [DisplayName("全人學習護照")]
        public string? PassPort { get; set; }
    }

    public class Section2
    { 
        public string? AA { get; set; }
    }

    public class Section3
    {

    }

    public class Section4
    {

    }






    public class ClubActReportEditModel
    {
		#region Section1
		/// <summary>ID</summary>
		[DisplayName("ID")]
        public string? AwdID { get; set; }

		/// <summary>ID</summary>
		[DisplayName("ID")]
		public string? AwdDetailID { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

		/// <summary>活動類型</summary>
		[DisplayName("活動類型")]
		public string? StaticOrDynamic { get; set; }

		/// <summary>活動類型</summary>
		[DisplayName("活動類型")]
		public string? StaticOrDynamicText { get; set; }

		/// <summary>活動地點</summary>
		[DisplayName("活動地點")]
		public string? ActInOrOut { get; set; }

		/// <summary>活動地點</summary>
		[DisplayName("活動地點")]
		public string? ActInOrOutText { get; set; }

		/// <summary>活動人數</summary>
		[DisplayName("活動人數")]
		public string? Capacity { get; set; }

		/// <summary>活動性質</summary>
		[DisplayName("活動性質")]
		public string? ActType { get; set; }

		/// <summary>活動性質</summary>
		[DisplayName("活動性質")]
		public string? ActTypeText { get; set; }

		/// <summary>使用資訊設備</summary>
		[DisplayName("使用資訊設備")]
		public string? UseITEquip { get; set; }

		/// <summary>使用資訊設備</summary>
		[DisplayName("使用資訊設備")]
		public string? UseITEquipText { get; set; }

		/// <summary>活動簡介</summary>
		[DisplayName("活動簡介")]
		public string? ShortDesc { get; set; }

		/// <summary>SDGs</summary>
		[DisplayName("SDGs")]
		public string? SDGs { get; set; }

		/// <summary>全人學習護照</summary>
		[DisplayName("全人學習護照")]
		public string? PassPort { get; set; }

		/// <summary>全人學習護照</summary>
		[DisplayName("全人學習護照")]
		public string? PassPortText { get; set; }

		/// <summary>審核狀態</summary>
		[DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

		/// <summary>審核備註</summary>
		[DisplayName("審核備註")]
		public string? ActVerifyMemo { get; set; }

		/// <summary>報備日期</summary>
		[DisplayName("報備日期")]
		public DateTime? Created { get; set; }
		#endregion

		#region Section2

		public List<ActListMangEditRundownModel> LstActRundown = new List<ActListMangEditRundownModel>();

		#endregion

		#region Section3

		public List<ActListFilesModel> LstProposal = new List<ActListFilesModel>();

		#endregion

		#region Section4

		/// <summary>領隊</summary>
		[DisplayName("領隊")]
		public string? LeaderName { get; set; }

		/// <summary>領隊</summary>
		[DisplayName("領隊")]
		public string? LeaderTel { get; set; }

		/// <summary>領隊</summary>
		[DisplayName("領隊")]
		public string? LeaderPhone { get; set; }

		/// <summary>活動負責人</summary>
		[DisplayName("活動負責人")]
		public string? ManagerName { get; set; }

		/// <summary>活動負責人</summary>
		[DisplayName("活動負責人")]
		public string? ManagerTel { get; set; }

		/// <summary>活動負責人</summary>
		[DisplayName("活動負責人")]
		public string? ManagerPhone { get; set; }

		public List<ActListFilesModel> LstOutSideFile = new List<ActListFilesModel>();

		#endregion

	}

    public class ClubActReportConsentModel
    { 
        public string? Selected { get; set; }
        public string? InSchool { get; set; }

        public string? OutSchool { get; set; }

        public string? InAndOutSchool { get; set; }
    }
}
