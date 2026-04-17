using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class MenuBoardMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public MenuBoardMangConditionModel ConditionModel { get; set; }

        public List<MenuBoardMangResultModel> ResultModel { get; set; }

        public MenuBoardMangEditModel EditModel { get; set; }
    }

    public class MenuBoardMangConditionModel
    {
        public MenuBoardMangConditionModel()
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
        public string? Header { get; set; }

        /// <summary>功能描述</summary>
        [DisplayName("功能描述")]
        public string? ShortDesc { get; set; }
    }

    public class MenuBoardMangResultModel
    {
        public int? MenuBoardId { get; set; }

        /// <summary>功能名稱</summary>
        [DisplayName("功能名稱")]
        public string? Header { get; set; }

        /// <summary>功能描述</summary>
        [DisplayName("功能描述")]
        public string? ShortDesc { get; set; }

        /// <summary>最後更新人</summary>
        [DisplayName("最後更新人")]
        public string? LastModifier { get; set; }

        /// <summary>最後更新時間</summary>
        [DisplayName("最後更新時間")]
        public string? LastModified { get; set; }

    }

    public class MenuBoardMangEditModel
    {
        public int? MenuBoardId { get; set; }

        /// <summary>功能名稱</summary>
        [DisplayName("功能名稱")]
        public string? Header { get; set; }

        /// <summary>功能描述</summary>
        [DisplayName("功能描述")]
        public string? ShortDesc { get; set; }

        /// <summary>功能圖示</summary>
        [DisplayName("功能圖示")]
        public string? IconPath { get; set; }

        /// <summary>是否啟用</summary>
        [DisplayName("是否啟用")]
        public bool? IsEnable { get; set; }
        
    }
}
