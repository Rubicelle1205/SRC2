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
        /// <summary>單位ID</summary>
        [DisplayName("單位ID")]
        public string? ClubId { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ClubCName { get; set; }

        /// <summary>Logo</summary>
        [DisplayName("Logo")]
        public string? LogoPath { get; set; }

        /// <summary>社團類別</summary>
        [DisplayName("社團類別")]
        public string? ClubClass { get; set; }

        /// <summary>社團類別</summary>
        [DisplayName("社團類別")]
        public string? ClubClassText { get; set; }

    }


}
