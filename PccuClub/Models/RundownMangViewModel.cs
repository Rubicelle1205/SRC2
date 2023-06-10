using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class RundownMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public RundownMangConditionModel ConditionModel { get; set; }

        public List<RundownMangResultModel> ResultModel { get; set; }

        public RundownMangEditModel EditModel { get; set; }

        public List<RundownMangExcelResultModel> ExcelModel { get; set; }
    }

    public class RundownMangConditionModel
    {
        public RundownMangConditionModel()
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

        /// <summary>活動性質</summary>
        [DisplayName("活動性質")]
        public string? ActType { get; set; }

        /// <summary>核心能力</summary>
        [DisplayName("核心能力")]
        public string? SDGs { get; set; }

        /// <summary>場地分類</summary>
        [DisplayName("場地分類")]
        public string? PlaceSource { get; set; }

        /// <summary>社團組別</summary>
        [DisplayName("社團組別")]
        public string? LifeClass { get; set; }

        /// <summary>行程狀態</summary>
        [DisplayName("行程狀態")]
        public string? RundownStatus { get; set; }

        /// <summary>申請單位</summary>
        [DisplayName("申請單位")]
        public string? ClubCName { get; set; }

        /// <summary>申請人</summary>
        [DisplayName("申請人")]
        public string? ClubID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class RundownMangResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? ActRundownID { get; set; }
        
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? ActID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動人數</summary>
        [DisplayName("活動人數")]
        public string? Capacity { get; set; }

        /// <summary>活動性質</summary>
        [DisplayName("活動性質")]
        public string? ActType { get; set; }

        /// <summary>活動性質</summary>
        [DisplayName("活動性質")]
        public string? ActTypeText { get; set; }

        /// <summary>核心能力</summary>
        [DisplayName("核心能力")]
        public string? SDGs { get; set; }

        /// <summary>核心能力</summary>
        [DisplayName("核心能力")]
        public string? SDateText { get; set; }
        
        /// <summary>申請單位</summary>
        [DisplayName("申請單位")]
        public string? ClubCName { get; set; }

        /// <summary>申請人</summary>
        [DisplayName("申請人")]
        public string? ClubId { get; set; }

        /// <summary>地點類型</summary>
        [DisplayName("地點類型")]
        public string? PlaceSource { get; set; }

        /// <summary>地點類型</summary>
        [DisplayName("地點類型")]
        public string? PlaceSourceText { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceText { get; set; }

        /// <summary>活動日</summary>
        [DisplayName("活動日")]
        public DateTime? Date { get; set; }

        /// <summary>STime</summary>
        [DisplayName("STime")]
        public string? STime { get; set; }

        /// <summary>ETime</summary>
        [DisplayName("ETime")]
        public string? ETime { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>行程狀態</summary>
        [DisplayName("行程狀態")]
        public string? RundownStatus { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class RundownMangEditModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? ActRundownID { get; set; }

        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? RundownStatus { get; set; }
    }

    public class RundownMangExcelResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? ActID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動人數</summary>
        [DisplayName("活動人數")]
        public string? Capacity { get; set; }

        /// <summary>活動性質</summary>
        [DisplayName("活動性質")]
        public string? ActType { get; set; }

        /// <summary>核心能力</summary>
        [DisplayName("核心能力")]
        public string? SDGs { get; set; }

        /// <summary>申請單位</summary>
        [DisplayName("申請單位")]
        public string? ClubCName { get; set; }

        /// <summary>申請人</summary>
        [DisplayName("申請人")]
        public string? ClubId { get; set; }

        /// <summary>地點類型</summary>
        [DisplayName("地點類型")]
        public string? PlaceSource { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceText { get; set; }

        /// <summary>活動開始日</summary>
        [DisplayName("活動開始日")]
        public DateTime? SDate { get; set; }

        /// <summary>活動結束日</summary>
        [DisplayName("活動結束日")]
        public DateTime? EDate { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

}
