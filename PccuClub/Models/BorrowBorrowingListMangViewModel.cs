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

        /// <summary>全部資源</summary>
        [DisplayName("全部資源")]
        public string? BorrowMainClassID { get; set; }

        /// <summary>SDate</summary>
        [DisplayName("SDate")]
        public string? SDate { get; set; }

    }

    public class BorrowBorrowingListMangResultModel
    {
        /// <summary>日期</summary>
        [DisplayName("日期")]
        public string? Date { get; set; }

        public List<BorrowBorrowingUnitData> LstPlaceData = new List<BorrowBorrowingUnitData>();
    }

    public class BorrowBorrowingUnitData
    {
        /// <summary>子資源ID</summary>
        [DisplayName("子資源ID")]
        public string? MainResourceID { get; set; }

        /// <summary>子資源名稱</summary>
        [DisplayName("子資源名稱")]
        public string? SecondResourceName { get; set; }

        public List<BorrowUnitData> LstBorrowUnitData = new List<BorrowUnitData>();
    }

    public class BorrowUnitData
    {
        public string? MainResourceID { get; set; }

        /// <summary>日期</summary>
        [DisplayName("日期")]
        public DateTime? Date { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>借用資源</summary>
        [DisplayName("借用資源")]
        public string? MainResourceName { get; set; }

        /// <summary>借用單位</summary>
        [DisplayName("借用單位")]
        public string? ApplyUnitName { get; set; }

    }
}
