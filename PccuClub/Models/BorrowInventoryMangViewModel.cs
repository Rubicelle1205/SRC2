using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BorrowInventoryMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowInventoryMangConditionModel ConditionModel { get; set; }

        public List<BorrowInventoryMangResultModel> ResultModel { get; set; }

        public List<BorrowInventoryMangExcelModel> ExcelModel { get; set; }


    }

    public class BorrowInventoryMangConditionModel
    {
        public BorrowInventoryMangConditionModel()
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

        /// <summary>資源名稱</summary>
        [DisplayName("資源名稱")]
        public string? Text { get; set; }

    }

    public class BorrowInventoryMangResultModel
    {
        public int? ID { get; set; }

        /// <summary>分類名稱</summary>
        [DisplayName("分類名稱")]
        public string? MainResourceID { get; set; }

        /// <summary>分類名稱</summary>
        [DisplayName("分類名稱")]
        public string? MainResourceIDText { get; set; }

        /// <summary>物品類型</summary>
        [DisplayName("物品類型")]
        public string? BorrowType { get; set; }

        /// <summary>物品類型</summary>
        [DisplayName("物品類型")]
        public string? BorrowTypeText { get; set; }

        /// <summary>庫存數</summary>
        [DisplayName("庫存數")]
        public string? AmtReal { get; set; }

        /// <summary>實際盤點數量</summary>
        [DisplayName("實際盤點數量")]
        public string? AmtInventory { get; set; }

        /// <summary>盤點日期</summary>
        [DisplayName("盤點日期")]
        public DateTime? Created { get; set; }
    }

    public class BorrowInventoryMangExcelModel
    {
        /// <summary>資產號碼</summary>
        [DisplayName("資產號碼")]
        public string? SecondResourceNo { get; set; }

        /// <summary>子資源名稱</summary>
        [DisplayName("子資源名稱")]
        public string? SecondResourceName { get; set; }

        /// <summary>上下架狀態</summary>
        [DisplayName("上下架狀態")]
        public string? ShelvesStatusText { get; set; }

        /// <summary>借用狀態</summary>
        [DisplayName("借用狀態")]
        public string? BorrowStatusText { get; set; }

        /// <summary>盤點狀態</summary>
        [DisplayName("盤點狀態")]
        public string? ResourceInventoryStatusText { get; set; }
    }

    public class BorrowInventoryMangExcelHeaderModel
    {
        /// <summary>資產號碼</summary>
        [DisplayName("資產號碼")]
        public string? SecondResourceNo { get; set; }

        /// <summary>子資源名稱</summary>
        [DisplayName("子資源名稱")]
        public string? SecondResourceName { get; set; }

        /// <summary>上下架狀態</summary>
        [DisplayName("上下架狀態")]
        public string? TEShelvesStatusText { get; set; }

        /// <summary>借用狀態</summary>
        [DisplayName("借用狀態")]
        public string? BorrowStatusText { get; set; }

        /// <summary>盤點狀態</summary>
        [DisplayName("盤點狀態")]
        public string? ResourceInventoryStatusText { get; set; }
    }
}
