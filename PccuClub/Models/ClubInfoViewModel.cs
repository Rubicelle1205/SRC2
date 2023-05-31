using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubInfoViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

		public MyClubEditModel MyClubEditModel { get; set; }

		public List<MyClubExcelModel> ExcelResultModel { get; set; }


		public ClubScheduleConditionModel ClubScheduleConditionModel { get; set; }

        public List<ClubScheduleResultModel> ClubScheduleResultModel { get; set; }

		public ClubScheduleCreateModel ClubScheduleCreateModel { get; set; }

        public ClubScheduleEditModel ClubScheduleEditModel { get; set; }


    }

	public class MyClubEditModel
	{
		/// <summary>社團代號</summary>
		[DisplayName("社團代號")]
		public string? ClubId { get; set; }

		/// <summary>社團中文名稱</summary>
		[DisplayName("社團中文名稱")]
		public string? ClubCName { get; set; }

		/// <summary>社團英文名稱</summary>
		[DisplayName("社團英文名稱")]
		public string? ClubEName { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		/// <summary>社團組別</summary>
		[DisplayName("社團組別")]
		public string? LifeClass { get; set; }

		/// <summary>社團組別</summary>
		[DisplayName("社團組別")]
		public string? LifeClassText { get; set; }

		/// <summary>社團分類</summary>
		[DisplayName("社團分類")]
		public string? ClubClass { get; set; }

		/// <summary>社團分類</summary>
		[DisplayName("社團分類")]
		public string? ClubClassText { get; set; }

		/// <summary>社辦地址</summary>
		[DisplayName("社辦地址")]
		public string? Address { get; set; }

		/// <summary>聯絡電話</summary>
		[DisplayName("聯絡電話")]
		public string? Tel { get; set; }

		/// <summary>E-mail</summary>
		[DisplayName("E-mail")]
		public string? EMail { get; set; }

		/// <summary>社群連結一	</summary>
		[DisplayName("社群連結一")]
		public string? Social1 { get; set; }

		/// <summary>社群連結二</summary>
		[DisplayName("社群連結二")]
		public string? Social2 { get; set; }

		/// <summary>社群連結三</summary>
		[DisplayName("社群連結三")]
		public string? Social3 { get; set; }

		/// <summary>Logo</summary>
		[DisplayName("Logo")]
		public string? LogoPath { get; set; }

		/// <summary>活動簡圖</summary>
		[DisplayName("活動簡圖")]
		public string? ActImgPath { get; set; }

		/// <summary>簡介</summary>
		[DisplayName("簡介")]
		public string? ShortInfo { get; set; }
	}

	public class MyClubExcelModel
	{
		/// <summary>社團代號</summary>
		[DisplayName("社團代號")]
		public string? ClubId { get; set; }

		/// <summary>社團中文名稱</summary>
		[DisplayName("社團中文名稱")]
		public string? ClubCName { get; set; }

		/// <summary>社團英文名稱</summary>
		[DisplayName("社團英文名稱")]
		public string? ClubEName { get; set; }

		/// <summary>學年度</summary>
		[DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		/// <summary>社團組別</summary>
		[DisplayName("社團組別")]
		public string? LifeClassText { get; set; }

		/// <summary>社團分類</summary>
		[DisplayName("社團分類")]
		public string? ClubClassText { get; set; }

		/// <summary>社辦地址</summary>
		[DisplayName("社辦地址")]
		public string? Address { get; set; }

		/// <summary>聯絡電話</summary>
		[DisplayName("聯絡電話")]
		public string? Tel { get; set; }

		/// <summary>E-mail</summary>
		[DisplayName("E-mail")]
		public string? EMail { get; set; }

		/// <summary>社群連結一	</summary>
		[DisplayName("社群連結一")]
		public string? Social1 { get; set; }

		/// <summary>社群連結二</summary>
		[DisplayName("社群連結二")]
		public string? Social2 { get; set; }

		/// <summary>社群連結三</summary>
		[DisplayName("社群連結三")]
		public string? Social3 { get; set; }

		/// <summary>Logo</summary>
		[DisplayName("Logo")]
		public string? LogoPath { get; set; }

		/// <summary>活動簡圖</summary>
		[DisplayName("活動簡圖")]
		public string? ActImgPath { get; set; }

		/// <summary>簡介</summary>
		[DisplayName("簡介")]
		public string? ShortInfo { get; set; }
	}

	public class ClubScheduleConditionModel
	{
        public ClubScheduleConditionModel()
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

	public class ClubScheduleResultModel
    {

        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? CScheID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? CScheName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public DateTime? CScheDate { get; set; }

        /// <summary>是否舉辦</summary>
        [DisplayName("是否舉辦")]
        public string? ActHoldType { get; set; }

        /// <summary>是否舉辦</summary>
        [DisplayName("是否舉辦")]
        public string? ActHoldTypeText { get; set; }
    }

	public class ClubScheduleCreateModel
	{
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? CScheID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
		public string? SchoolYear { get; set; }

		/// <summary>活動類別</summary>
		[DisplayName("活動類別")]
		public string? ActType { get; set; }

		/// <summary>活動名稱</summary>
		[DisplayName("活動名稱")]
		public string? CScheName { get; set; }

		/// <summary>活動日期</summary>
		[DisplayName("活動日期")]
		public string? CScheDate { get; set; }

		/// <summary>預定場地</summary>
		[DisplayName("預定場地")]
		public string? BookingPlace { get; set; }

		/// <summary>經費預算</summary>
		[DisplayName("經費預算")]
		public string? Budget { get; set; }

		/// <summary>簡介</summary>
		[DisplayName("簡介")]
		public string? ShortDesc { get; set; }

	}

    public class ClubScheduleEditModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? CScheID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動類別</summary>
        [DisplayName("活動類別")]
        public string? ActTypeID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? CScheName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public string? CScheDate { get; set; }

        /// <summary>預定場地</summary>
        [DisplayName("預定場地")]
        public string? BookingPlace { get; set; }

        /// <summary>經費預算</summary>
        [DisplayName("經費預算")]
        public string? Budget { get; set; }

        /// <summary>簡介</summary>
        [DisplayName("簡介")]
        public string? ShortDesc { get; set; }

        /// <summary>自檢狀態項*</summary>
        [DisplayName("自檢狀態項")]
        public string? ActHoldType { get; set; }

        /// <summary>經費補助</summary>
        [DisplayName("經費補助")]
        public string? Support { get; set; }

        /// <summary>參與人數</summary>
        [DisplayName("參與人數")]
        public string? Participants { get; set; }

        /// <summary>滿意度</summary>
        [DisplayName("滿意度")]
        public string? Satisfaction { get; set; }

        /// <summary>附件檔</summary>
        [DisplayName("附件檔")]
        public string? Attachment { get; set; }
    }
}
