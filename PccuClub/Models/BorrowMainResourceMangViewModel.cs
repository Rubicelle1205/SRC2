using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BorrowMainResourceMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowMainResourceMangConditionModel ConditionModel { get; set; }

        public List<BorrowMainResourceMangResultModel> ResultModel { get; set; }

        public BorrowMainResourceMangCreateModel CreateModel { get; set; }

        public BorrowMainResourceMangEditModel EditModel { get; set; }

    }

    public class BorrowMainResourceMangConditionModel
    {
        public BorrowMainResourceMangConditionModel()
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

        /// <summary>業務名稱</summary>
        [DisplayName("業務名稱")]
        public string? MainClass { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? Enable { get; set; }

        /// <summary>資源代碼</summary>
        [DisplayName("資源代碼")]
        public string? MainResourceID { get; set; }

        /// <summary>資源名稱</summary>
        [DisplayName("資源名稱")]
        public string? MainResourceName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowMainResourceMangResultModel
    {
        public string? MainResourceID { get; set; }

        /// <summary>資源名稱</summary>
        [DisplayName("資源名稱")]
        public string? MainResourceName { get; set; }

        /// <summary>業務名稱</summary>
        [DisplayName("業務名稱")]
        public string? MainClass { get; set; }

        /// <summary>業務名稱</summary>
        [DisplayName("業務名稱")]
        public string? MainClassText { get; set; }

        /// <summary>簡易說明</summary>
        [DisplayName("簡易說明")]
        public string? ShortDesc { get; set; }

        /// <summary>實際庫存</summary>
        [DisplayName("實際庫存")]
        public string? AmtReal { get; set; }

        /// <summary>上架庫存</summary>
        [DisplayName("上架庫存")]
        public string? AmtShelves { get; set; }

        /// <summary>物品類型</summary>
        [DisplayName("物品類型")]
        public string? BorrowType { get; set; }

        /// <summary>物品類型</summary>
        [DisplayName("物品類型")]
        public string? BorrowTypeText { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? Enable { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? EnableText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowMainResourceMangCreateModel
    {
        /// <summary>資源代碼</summary>
        [DisplayName("資源代碼")]
        public string? MainResourceID { get; set; }

        /// <summary>資源名稱</summary>
        [DisplayName("資源名稱")]
        public string? MainResourceName { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClass { get; set; }

        /// <summary>子業務分類</summary>
        [DisplayName("子業務分類")] 
        public string? SecondClass { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? BorrowType { get; set; }

        /// <summary>簡易說明</summary>
        [DisplayName("簡易說明")]
        public string? ShortDesc { get; set; }

        /// <summary>借用規範</summary>
        [DisplayName("借用規範")]
        public string? BorrowRule { get; set; }

        /// <summary>資源圖片</summary>
        [DisplayName("資源圖片")]
        public string? ResourceImg1 { get; set; }

        /// <summary>資源圖片</summary>
        [DisplayName("資源圖片")]
        public string? ResourceImg2 { get; set; }

        /// <summary>資源圖片</summary>
        [DisplayName("備資源圖片註")]
        public string? ResourceImg3 { get; set; }

        /// <summary>資源圖片</summary>
        [DisplayName("資源圖片")]
        public string? ResourceImg4 { get; set; }

        /// <summary>實際總庫存</summary>
        [DisplayName("實際總庫存")]
        public string? AmtReal { get; set; }

        /// <summary>上架庫存</summary>
        [DisplayName("上架庫存")]
        public string? AmtShelves { get; set; }

        /// <summary>單次借用上限</summary>
        [DisplayName("單次借用上限")]
        public string? AmtOnce { get; set; }

        /// <summary>安全庫存量</summary>
        [DisplayName("安全庫存量")]
        public string? AmtSafe { get; set; }

        /// <summary>安全庫存提示訊息</summary>
        [DisplayName("安全庫存提示訊息")]
        public string? SafeMessage { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? Enable { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowMainResourceMangEditModel
    {
        /// <summary>資源代碼</summary>
        [DisplayName("資源代碼")]
        public string? MainResourceID { get; set; }

        /// <summary>資源名稱</summary>
        [DisplayName("資源名稱")]
        public string? MainResourceName { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClass { get; set; }

        /// <summary>子業務分類</summary>
        [DisplayName("子業務分類")]
        public string? SecondClass { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? BorrowType { get; set; }

        /// <summary>簡易說明</summary>
        [DisplayName("簡易說明")]
        public string? ShortDesc { get; set; }

        /// <summary>借用規範</summary>
        [DisplayName("借用規範")]
        public string? BorrowRule { get; set; }

        /// <summary>資源圖片</summary>
        [DisplayName("資源圖片")]
        public string? ResourceImg1 { get; set; }

        /// <summary>資源圖片</summary>
        [DisplayName("資源圖片")]
        public string? ResourceImg2 { get; set; }

        /// <summary>資源圖片</summary>
        [DisplayName("備資源圖片註")]
        public string? ResourceImg3 { get; set; }

        /// <summary>資源圖片</summary>
        [DisplayName("資源圖片")]
        public string? ResourceImg4 { get; set; }

        /// <summary>實際總庫存</summary>
        [DisplayName("實際總庫存")]
        public string? AmtReal { get; set; }

        /// <summary>上架庫存</summary>
        [DisplayName("上架庫存")]
        public string? AmtShelves { get; set; }

        /// <summary>單次借用上限</summary>
        [DisplayName("單次借用上限")]
        public string? AmtOnce { get; set; }

        /// <summary>安全庫存量</summary>
        [DisplayName("安全庫存量")]
        public string? AmtSafe { get; set; }

        /// <summary>安全庫存提示訊息</summary>
        [DisplayName("安全庫存提示訊息")]
        public string? SafeMessage { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? Enable { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public string? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        public string? LastModified { get; set; }
    }
}
