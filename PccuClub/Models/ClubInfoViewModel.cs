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



}
