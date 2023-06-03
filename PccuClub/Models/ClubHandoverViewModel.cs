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


        public ClubHandoverDocCheckModel HandoverDocCheckModel { get; set; }

        public ClubHandover0101ViewModel Handover0101Model { get; set; }
        public ClubHandover0102ViewModel Handover0102Model { get; set; }
        public ClubHandover0103ViewModel Handover0103Model { get; set; }
        public ClubHandover0204ViewModel Handover0204Model { get; set; }
        public ClubHandover0205ViewModel Handover0205Model { get; set; }
        public ClubHandover0206ViewModel Handover0206Model { get; set; }
        public ClubHandover0307ViewModel Handover0307Model { get; set; }
        public ClubHandover0308ViewModel Handover0308Model { get; set; }
        public ClubHandover0309ViewModel Handover0309Model { get; set; }



    }

    #region 申請

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

    #endregion

    #region 已填寫表單

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

    #endregion

    #region 已上傳檔案

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

    #endregion

    public class ClubHandoverDocCheckModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? HoDetailID { get; set; }
    }

    #region 0101

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

    #endregion

    #region 0102

    public class ClubHandover0102ViewModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? HoID { get; set; }

        /// <summary>社團ID</summary>
        [DisplayName("社團ID")]
        public string? SchoolYear { get; set; }

        /// <summary>社團ID</summary>
        [DisplayName("社團ID")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubName { get; set; }

        /// <summary>改選形式</summary>
        [DisplayName("改選形式")]
        public string? ElectionType { get; set; }

		/// <summary>改選形式</summary>
		[DisplayName("改選形式")]
		public string? ElectionTypeText { get; set; }

		/// <summary>改選時間</summary>
		[DisplayName("改選時間")]
        public string? ElectionDate { get; set; }

        /// <summary>改選地點</summary>
        [DisplayName("改選地點")]
        public string? ElectionPlace { get; set; }

        /// <summary>社團人數</summary>
        [DisplayName("社團人數")]
        public string? TotleMan { get; set; }

        /// <summary>應到</summary>
        [DisplayName("應到")]
        public string? ShouldMan { get; set; }

        /// <summary>實到</summary>
        [DisplayName("實到")]
        public string? RealMan { get; set; }

        /// <summary>請假</summary>
        [DisplayName("請假")]
        public string? LeaveMan { get; set; }

        /// <summary>缺席</summary>
        [DisplayName("缺席")]
        public string? AbsentMan { get; set; }

        /// <summary>列席師長姓名</summary>
        [DisplayName("列席師長姓名")]
        public string? Teacher { get; set; }

        /// <summary>會議主席姓名</summary>
        [DisplayName("會議主席姓名")]
        public string? Chairman { get; set; }

        /// <summary>會議記錄姓名</summary>
        [DisplayName("會議記錄姓名")]
        public string? Recorder { get; set; }

        /// <summary>新任社長姓名</summary>
        [DisplayName("新任社長姓名")]
        public string? NewLeader { get; set; }

        /// <summary>當日會議記錄上傳</summary>
        [DisplayName("當日會議記錄上傳")]
        public string? MeetingRecord { get; set; }

        /// <summary>當日會議簽到表上傳</summary>
        [DisplayName("當日會議簽到表上傳")]
        public string? MeetingSign { get; set; }

		/// <summary>當日會議記錄上傳</summary>
		[DisplayName("當日會議記錄上傳")]
		public string? MeetingRecordName { get; set; }

		/// <summary>當日會議簽到表上傳</summary>
		[DisplayName("當日會議簽到表上傳")]
		public string? MeetingSignName { get; set; }
	}

    #endregion

    public class ClubHandover0103ViewModel 
    {
		public string? SchoolYear { get; set; }
		public string? ClubID { get; set; }
		public string? ClubCName { get; set; }
		public string? ClubEName { get; set; }
		public string? Creator { get; set; }
		public string? Created { get; set; }
		public string? LastModifier { get; set; }
		public string? LastModified { get; set; }
		public string? ClubBuildID { get; set; }

		public string? ClubBuildIDText { get; set; }
		public string? Location { get; set; }
		public string? Tel { get; set; }
		public string? UserCName { get; set; }
		public string? UserEName { get; set; }
		public string? Sex { get; set; }

		public string? SexText { get; set; }
		public string? IdentityType { get; set; }
		public string? IdentityTypeText { get; set; }
		public string? SNO { get; set; }
		public string? CDepartment { get; set; }
		public string? EDepartment { get; set; }
		public string? UserMail { get; set; }
		public string? UserCellphone { get; set; }
		public string? Transcript { get; set; }
		public string? TranscriptName { get; set; }
		public string? GPA { get; set; }
		public string? Behavior { get; set; }
		public string? Score60 { get; set; }
		public string? Score75 { get; set; }
		public string? IsMember { get; set; }
		public string? NoFire { get; set; }
		public string? NoReElection { get; set; }
		public string? NoTwoPosition { get; set; }

		public string? Score60Text { get; set; }
		public string? Score75Text { get; set; }
		public string? IsMemberText { get; set; }
		public string? NoFireText { get; set; }
		public string? NoReElectionText { get; set; }
		public string? NoTwoPositionText { get; set; }
	}
    public class ClubHandover0204ViewModel 
    {
		public string? SchoolYear { get; set; }
		public string? ClubID { get; set; }
		public string? ClubName { get; set; }
		public string? NameOfClub { get; set; }
	}
    public class ClubHandover0205ViewModel
    {
		public string? SchoolYear { get; set; }
		public string? ClubID { get; set; }
		public string? ClubName { get; set; }
		public string? ActSysAcc { get; set; }
		public string? ActSysPwd { get; set; }
		public string? ClubWebAcc { get; set; }
		public string? ClubWebPwd { get; set; }
		public string? RPageAcc { get; set; }
		public string? RPagePwd { get; set; }
		public string? PassportAcc { get; set; }
		public string? PassportPwd { get; set; }
		public string? OneDriveAcc { get; set; }
		public string? OneDrivePwd { get; set; }
		public string? HasSchoolProperty { get; set; }
		public string? HasSchoolPropertyText { get; set; }
		public string? UseRecord { get; set; }
		public string? ClubProperty { get; set; }
		public string? SchoolProperty { get; set; }

		public string? UseRecordName { get; set; }
		public string? ClubPropertyName { get; set; }
		public string? SchoolPropertyName { get; set; }
	}
    public class ClubHandover0206ViewModel { }
    public class ClubHandover0307ViewModel { }
    public class ClubHandover0308ViewModel { }
    public class ClubHandover0309ViewModel { }













}
