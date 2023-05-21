using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ActListMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ActListMangConditionModel ConditionModel { get; set; }

        public List<ActListMangResultModel> ResultModel { get; set; }

        public ActListMangCreateModel CreateModel { get; set; }

        public ActListMangEditModel EditModel { get; set; }

        public ActListMangExcelResultModel ExcelModel { get; set; }
    }

    public class ActListMangConditionModel
    {
        public ActListMangConditionModel()
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

        /// <summary>社團組別</summary>
        [DisplayName("社團組別")]
        public string? LifeClass { get; set; }

        /// <summary>活動編號</summary>
        [DisplayName("活動編號")]
        public string? ActId { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ClubName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ActListMangResultModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>活動編號</summary>
        [DisplayName("活動編號")]
        public string? ActId { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ClubName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動開始日</summary>
        [DisplayName("活動開始日")]
        public DateTime? SDate { get; set; }

        /// <summary>活動結束日</summary>
        [DisplayName("活動結束日")]
        public DateTime? EDate { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }
    }

    public class ActListMangExcelResultModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>活動編號</summary>
        [DisplayName("活動編號")]
        public string? ActId { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ClubName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }
    }

    public class ActListMangCreateModel
    {
        
    }

    public class ActListMangEditModel
    {
        
    }
}
