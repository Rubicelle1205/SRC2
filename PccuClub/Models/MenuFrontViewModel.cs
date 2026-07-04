using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class MenuFrontViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public MenuFrontConditionModel ConditionModel { get; set; }

        public List<MenuFrontResultModel> ResultModel { get; set; }
    }

    public class MenuFrontConditionModel
    {
        public MenuFrontConditionModel()
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

    }

    public class MenuFrontResultModel
    {
        public int? MenuBoardId { get; set; }

        /// <summary>目錄代碼</summary>
        [DisplayName("目錄代碼")]
        public string? MenuBoardCode { get; set; }

        /// <summary>標題</summary>
        [DisplayName("標題")]
        public string? Header { get; set; }

        /// <summary>說明</summary>
        [DisplayName("說明")]
        public string? ShortDesc { get; set; }

        /// <summary>icon路徑</summary>
        [DisplayName("icon路徑")]
        public string? IconPath { get; set; }

        /// <summary>是否啟用</summary>
        [DisplayName("是否啟用")]
        public bool IsEnable { get; set; }
    }

}
