using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class SDGsMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public SDGsMangConditionModel ConditionModel { get; set; }

        public List<SDGsMangResultModel> ResultModel { get; set; }
    }

    public class SDGsMangConditionModel
    {
        public SDGsMangConditionModel()
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

        /// <summary>簡稱</summary>
        [DisplayName("簡稱")]
        public string? ShortName { get; set; }

        /// <summary>描述</summary>
        [DisplayName("行政院國發會標準中文翻譯")]
        public string? Desc { get; set; }
    }

    public class SDGsMangResultModel
    {
        /// <summary> import date line Number </summary>
        public int? SDGID { get; set; }

        /// <summary>簡稱</summary>
        [DisplayName("簡稱")]
        public string? ShortName { get; set; }

        /// <summary>描述</summary>
        [DisplayName("描述")]
        public string? Desc { get; set; }
    }
}
