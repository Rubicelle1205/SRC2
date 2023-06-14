using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class AwardMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public AwardMangConditionModel ConditionModel { get; set; }

        public List<AwardMangResultModel> ResultModel { get; set; }

        public AwardMangCreateModel CreateModel { get; set; }

        public AwardMangEditModel EditModel { get; set; }

        public List<AwdDetailModel> DetailModel { get; set; }

        public List<AwardMangExcelResultModel> ExcelModel { get; set; }
    }

    public class AwardMangConditionModel
    {
        public AwardMangConditionModel()
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
        
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? Organizer { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>獲獎類別</summary>
        [DisplayName("獲獎類別")]
        public string? AwdType { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class AwardMangResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? AwdID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? Organizer { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>獲獎類別</summary>
        [DisplayName("獲獎類別")]
        public string? AwdType { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        /// <summary>獲獎日期</summary>
        [DisplayName("獲獎日期")]
        public DateTime? AwdDate { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class AwardMangExcelResultModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? Organizer { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>獲獎類別</summary>
        [DisplayName("獲獎類別")]
        public string? AwdType { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        /// <summary>獲獎日期</summary>
        [DisplayName("獲獎日期")]
        public DateTime? AwdDate { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class AwardMangCreateModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? Organizer { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>獲獎類別</summary>
        [DisplayName("獲獎類別")]
        public string? AwdType { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        /// <summary>獲獎日期</summary>
        [DisplayName("獲獎日期")]
        public string? AwdDate { get; set; }

        /// <summary>附件</summary>
        [DisplayName("附件")]
        public string? Attachment { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? strGetAwd { get; set; }
    }

    public class AwardMangEditModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? AwdID { get; set; }
        
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? Organizer { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>獲獎類別</summary>
        [DisplayName("獲獎類別")]
        public string? AwdType { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        /// <summary>獲獎日期</summary>
        [DisplayName("獲獎日期")]
        public string? AwdDate { get; set; }

        /// <summary>附件</summary>
        [DisplayName("附件")]
        public string? Attachment { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? strGetAwd { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        [DisplayName("最後修改時間")]
        public DateTime? LastModified { get; set; }
    }

    public class AwdDetailModel 
    {
        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }
    }
}
