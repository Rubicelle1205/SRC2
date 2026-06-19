using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class BorrowBorrowingListMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowBorrowingListMangConditionModel ConditionModel { get; set; }

        public List<BorrowBorrowingListMangResultModel> ResultModel { get; set; }
    }

    public class BorrowBorrowingListMangConditionModel
    {
        public BorrowBorrowingListMangConditionModel()
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

    public class BorrowBorrowingListMangResultModel
    {
        /// <summary>日期</summary>
        [DisplayName("日期")]
        public string? Date { get; set; }

        public List<PlaceData> LstPlaceData = new List<PlaceData>();
    }


}
