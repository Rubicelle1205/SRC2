using MathNet.Numerics.RootFinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubHandoverViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public List<ClubHandoverCheckModel> CheckModel { get; set; }

        public ClubHandover0101ViewModel Handover0101Model { get; set; }

    }

    public class ClubHandoverCheckModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? HoID { get; set; }

        /// <summary>社團ID</summary>
        [DisplayName("社團ID")]
        public string? ClubID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }


        /// <summary>型態</summary>
        [DisplayName("型態")]
        public string? HandOverStatus { get; set; }
    }



    public class ClubHandover0101ViewModel
    {
		/// <summary>ID</summary>
		[DisplayName("ID")]
		public string? HoID { get; set; }

		/// <summary>社團ID</summary>
		[DisplayName("社團ID")]
		public string? ClubID { get; set; }

		/// <summary>社團名稱</summary>
		[DisplayName("社團名稱")]
		public string? ClubName { get; set; }

		/// <summary>當事人姓名</summary>
		[DisplayName("當事人姓名")]
		public string? UserName { get; set; }

		/// <summary>同意</summary>
		[DisplayName("同意")]
		public string? Agree { get; set; }
	}
}
