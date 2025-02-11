using NPOI.HPSF;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubActFinishViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubActFinishConditionModel ConditionModel { get; set; }

		public List<ClubActFinishResultModel> ResultModel { get; set; }

		public ClubActFinishEditModel EditModel { get; set; }

        public ClubActFinishDetailModel DetailModel { get; set; }

        public List<PersonModel> ExcelModel { get; set; }

        public List<ALLPersonModel> ALLExcelModel { get; set; }

    }

    public class ClubActFinishConditionModel
    {
        public ClubActFinishConditionModel()
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

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }
    }

    public class ClubActFinishResultModel
    {
        /// <summary>結案代號</summary>
        [DisplayName("結案代號")]
        public string? ActFinishId { get; set; }

        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? ActID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActFinishVerify { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActVerifyText { get; set; }

        /// <summary>報備日期</summary>
        [DisplayName("報備日期")]
        public DateTime? Created { get; set; }
    }

	public class ClubActFinishEditModel
    {
        public List<PersonModel> PersonModel = new List<PersonModel>();

        /// <summary>結案代碼</summary>
        [DisplayName("結案代碼")]
        public string? ActFinishId { get; set; }

        /// <summary>社團代碼</summary>
        [DisplayName("社團代碼")]
        public string? ClubId { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號\r\n")]
        public string? ActID { get; set; }

        /// <summary>活動報備編號Detail</summary>
        [DisplayName("活動報備編號Detail")]
        public string? ActDetailId { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? ClubCName { get; set; }

        /// <summary>承辦人</summary>
        [DisplayName("承辦人")]
        public string? Caseman { get; set; }

        /// <summary>聯絡Email</summary>
        [DisplayName("聯絡Email")]
        public string? Email { get; set; }

        /// <summary>聯絡電話/分機</summary>
        [DisplayName("聯絡電話/分機")]
        public string? Tel { get; set; }

        /// <summary>活動日期*</summary>
        [DisplayName("活動日期*")]
        public DateTime? ActDate { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動辦理時程</summary>
        [DisplayName("活動辦理時程")]
        public string? Course { get; set; }

        /// <summary>活動資訊簡述</summary>
        [DisplayName("活動資訊簡述")]
        public string? ShortInfo { get; set; }

        /// <summary>其他附件</summary>
        [DisplayName("其他附件")]
        public string? ElseFile { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActFinishVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>審核備註</summary>
        [DisplayName("審核備註")]
        public string? Memo { get; set; }
    }

    public class ClubActFinishDetailModel
    {
        public List<PersonModel> PersonModel = new List<PersonModel>();

        /// <summary>結案代碼</summary>
        [DisplayName("結案代碼")]
        public string? ActFinishId { get; set; }

        /// <summary>社團代碼</summary>
        [DisplayName("社團代碼")]
        public string? ClubId { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號\r\n")]
        public string? ActID { get; set; }

        /// <summary>活動報備編號Detail</summary>
        [DisplayName("活動報備編號Detail")]
        public string? ActDetailId { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? ClubCName { get; set; }

        /// <summary>承辦人</summary>
        [DisplayName("承辦人")]
        public string? Caseman { get; set; }

        /// <summary>聯絡Email</summary>
        [DisplayName("聯絡Email")]
        public string? Email { get; set; }

        /// <summary>聯絡電話/分機</summary>
        [DisplayName("聯絡電話/分機")]
        public string? Tel { get; set; }

        /// <summary>活動日期*</summary>
        [DisplayName("活動日期*")]
        public DateTime? ActDate { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動辦理時程</summary>
        [DisplayName("活動辦理時程")]
        public string? Course { get; set; }

        /// <summary>活動資訊簡述</summary>
        [DisplayName("活動資訊簡述")]
        public string? ShortInfo { get; set; }

        /// <summary>其他附件</summary>
        [DisplayName("其他附件")]
        public string? ElseFile { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActFinishVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>審核備註</summary>
        [DisplayName("審核備註")]
        public string? Memo { get; set; }
    }

    public class PersonModel
    {
        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }
    }

    public class ALLPersonModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動代號</summary>
        [DisplayName("活動代號")]
        public string? ActID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }
    }

    public class ClubActFinishConsentModel
    { 
        public string? Selected { get; set; }

        public string? InSchool { get; set; }

        public string? OutSchool { get; set; }

        public string? InAndOutSchool { get; set; }
    }

}
