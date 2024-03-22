using NPOI.HPSF;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ActFinishMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ActFinishMangConditionModel ConditionModel { get; set; }

        public List<ActFinishMangResultModel> ResultModel { get; set; }

        public ActFinishMangCreateModel CreateModel {get; set;}

        public ActFinishMangEditModel EditModel { get; set; }

        public ActFinishMangDetailModel DetailModel { get; set; }

        public List<ActFinishMangExcelModel> ExcelModel { get; set; }

        public List<PersonModel> PersonModel { get; set; }

        public List<ALLPersonModel> ALLPersonModel { get; set; }

    }

    public class ActFinishMangConditionModel
    {
        public ActFinishMangConditionModel()
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

        /// <summary>社團代碼</summary>
        [DisplayName("社團代碼")]
        public string? ClubId { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號")]
        public string? ActID { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? ClubCName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        [DisplayName("建立日期_起")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("建立日期_迄")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ActFinishMangResultModel
    {
        /// <summary>結案代號</summary>
        [DisplayName("結案代號")]
        public string? ActFinishId { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號")]
        public string? ActID { get; set; }

        /// <summary>社團代碼</summary>
        [DisplayName("社團代碼")]
        public string? ClubId { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>主辦單位名稱</summary>
        [DisplayName("主辦單位名稱")]
        public string? ClubCName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActFinishVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class ActFinishMangExcelModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>主辦單位名稱</summary>
        [DisplayName("主辦單位名稱")]
        public string? ClubCName { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號")]
        public string? ActID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class ActFinishMangCreateModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號")]
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

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ActFinishMangEditModel
    {
        public List<PersonModel> PersonModel = new List<PersonModel>();

        /// <summary>結案代號</summary>
        [DisplayName("結案代號")]
        public string? ActFinishId { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號")]
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

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public string? ActDate { get; set; }

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

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ActFinishMangDetailModel
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

}
