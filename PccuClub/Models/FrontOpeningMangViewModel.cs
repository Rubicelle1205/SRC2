using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class FrontOpeningMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public FrontOpeningMangConditionModel ConditionModel { get; set; }

        public List<FrontOpeningMangResultModel> ResultModel { get; set; }

        public FrontOpeningMangEditModel EditModel { get; set; }
    }

    public class FrontOpeningMangConditionModel
    {
        public FrontOpeningMangConditionModel()
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

        /// <summary>功能名稱</summary>
        [DisplayName("功能名稱")]
        public string? MenuName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? Enable { get; set; }
    }

    public class FrontOpeningMangResultModel
    {
        public int? FrontOpeningId { get; set; }

        /// <summary>功能名稱</summary>
        [DisplayName("功能名稱")]
        public string? MenuName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? Enable { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? EnableText { get; set; }

        /// <summary>開放時間[起]</summary>
        [DisplayName("開放時間[起]")]
        public string? OpenDate { get; set; }

        /// <summary>開放時間[迄]</summary>
        [DisplayName("開放時間[迄]")]
        public string? CloseDate { get; set; }
    }


    public class FrontOpeningMangEditModel
    {
        public int? FrontOpeningId { get; set; }

        /// <summary>功能名稱</summary>
        [DisplayName("功能名稱")]
        public string? MenuName { get; set; }

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? Enable { get; set; }

        /// <summary>開放時間[起]</summary>
        [DisplayName("開放時間[起]")]
        public string? OpenDate { get; set; }

        /// <summary>開放時間[迄]</summary>
        [DisplayName("開放時間[迄]")]
        public string? CloseDate { get; set; }
    }
}
