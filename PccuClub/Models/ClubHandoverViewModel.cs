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


        public ClubHandoverFileConditionModel FileConditionModel { get; set; }
        public List<ClubHandoverFileResultModel> FileResultModel { get; set; }
        public List<ClubHandoverFileEditModel> LstFileEditModel { get; set; }
        public ClubHandoverFileDetailModel FileDetailModel { get; set; }


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

        /// <summary>文件類型</summary>
        [DisplayName("文件類型")]
        public string? DocType { get; set; }

        /// <summary>文件名稱</summary>
        [DisplayName("文件名稱")]
        public string? DocTypeText { get; set; }

        /// <summary>學年度</summary>
		[DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>建立日期</summary>
        [DisplayName("建立日期")]
        public DateTime? Created { get; set; }







    }


    public class ClubHandoverFileConditionModel
    {
        public ClubHandoverFileConditionModel()
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

    public class ClubHandoverFileResultModel
    {
        public List<ClubHandoverFileDataModel> FileData = new List<ClubHandoverFileDataModel>();

        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? HoID { get; set; }

        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? HoDetailID { get; set; }

        /// <summary>學年度</summary>
		[DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>文件類別</summary>
        [DisplayName("文件類別")]
        public string? HandOverClass { get; set; }

        /// <summary>文件類別</summary>
        [DisplayName("文件類別")]
        public string? HandOverClassText { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>審核備註</summary>
        [DisplayName("審核備註")]
        public string? VerifyMemo { get; set; }

        /// <summary>建立日期</summary>
        [DisplayName("建立日期")]
        public DateTime? Created { get; set; }
    }

    public class ClubHandoverFileDataModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? FileID { get; set; }

        /// <summary>文件</summary>
        [DisplayName("文件")]
        public string? FilePath { get; set; }
    }

    public class ClubHandoverFileDetailModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? HoDetailID { get; set; }
    }

    public class ClubHandoverFileEditModel
    {
        /// <summary>文件</summary>
        [DisplayName("文件")]
        public string? FilePath { get; set; }
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
