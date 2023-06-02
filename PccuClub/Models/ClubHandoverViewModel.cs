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


        public ClubHandoverHistroyConditionModel HistoryConditionModel { get; set; }
		public List<ClubHandoverHistroyResultModel> HistoryResultModel { get; set; }


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

    public class ClubHandoverHistroyConditionModel
    {
		public ClubHandoverHistroyConditionModel()
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

	public class ClubHandoverHistroyResultModel
	{
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? HoID { get; set; }

        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? HoDetailID { get; set; }

        /// <summary>文件編號</summary>
        [DisplayName("文件編號")]
        public string? DocNo { get; set; }

        /// <summary>文件名稱</summary>
        [DisplayName("文件名稱")]
        public string? DocNoText { get; set; }

        /// <summary>學年度</summary>
		[DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>建立日期</summary>
        [DisplayName("建立日期")]
        public DateTime? Created { get; set; }
        
			
			
			
			
			
			
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
