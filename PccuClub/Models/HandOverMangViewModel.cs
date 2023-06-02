using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class HandOverMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public HandOverMangConditionModel ConditionModel { get; set; }

        public List<HandOverMangResultModel> ResultModel { get; set; }

        public HandOverMangEditModel EditModel { get; set; }

    }

    public class HandOverMangConditionModel
    {
        public HandOverMangConditionModel()
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

        /// <summary>交接狀態</summary>
        [DisplayName("交接狀態")]
        public string? HandOverStatus { get; set; }
        
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class HandOverMangResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? HoID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? HandOverStatus { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? HandOverStatusText { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }
    
    public class HandOverMangEditModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? HoID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>申請狀態</summary>
        [DisplayName("申請狀態")]
        public string? HandOverStatus { get; set; }

        /// <summary>申請狀態</summary>
        [DisplayName("申請狀態")]
        public string? HandOverStatusText { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

}
