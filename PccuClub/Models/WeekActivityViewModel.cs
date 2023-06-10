using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class WeekActivityViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public WeekActivityConditionModel ConditionModel { get; set; }

        public List<WeekActivityResultModel> ResultModel { get; set; }
    }

    public class WeekActivityConditionModel
    {
        public WeekActivityConditionModel()
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

        /// <summary>場域</summary>
        [DisplayName("場域")]
        public string? BuildID { get; set; }

        /// <summary>SDate</summary>
        [DisplayName("SDate")]
        public string? SDate { get; set; }

    }

    public class WeekActivityResultModel
    {
        /// <summary>日期</summary>
        [DisplayName("日期")]
        public string? Date { get; set; }

        public List<PlaceData> LstPlaceData = new List<PlaceData>();
    }

    public class PlaceData
    {

        /// <summary>場地ID</summary>
        [DisplayName("場地ID")]
        public string? PlaceID { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        public List<WeekActClubData> LstActClubData = new List<WeekActClubData>();
    }
    public class WeekActClubData
    {
        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>日期</summary>
        [DisplayName("日期")]
        public string? Date { get; set; }

        /// <summary>場地ID</summary>
        [DisplayName("場地ID")]
        public string? ActPlaceID { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? ActPlaceText { get; set; }

        /// <summary>STime</summary>
        [DisplayName("STime")]
        public string? STime { get; set; }

        /// <summary>ETime</summary>
        [DisplayName("ETime")]
        public string? ETime { get; set; }

        /// <summary>社團ID</summary>
        [DisplayName("社團ID")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

    }

}
