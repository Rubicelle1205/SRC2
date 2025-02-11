using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BorrowSecondResourceMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowSecondResourceMangConditionModel ConditionModel { get; set; }

        public List<BorrowSecondResourceMangResultModel> ResultModel { get; set; }

        public BorrowSecondResourceMangEditModel EditModel { get; set; }

    }

    public class BorrowSecondResourceMangConditionModel
    {
        public BorrowSecondResourceMangConditionModel()
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

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClass { get; set; }

        /// <summary>上下架狀態</summary>
        [DisplayName("上下架狀態")]
        public string? ShelvesStatus { get; set; }

        /// <summary>借用狀態</summary>
        [DisplayName("借用狀態")]
        public string? BorrowStatus { get; set; }

        /// <summary>主資源名稱</summary>
        [DisplayName("主資源名稱")]
        public string? MainResourceName { get; set; }

        /// <summary>資產號碼</summary>
        [DisplayName("資產號碼")]
        public string? SecondResourceNo { get; set; }

        /// <summary>子資源名稱</summary>
        [DisplayName("子資源名稱")]
        public string? SecondResourceName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowSecondResourceMangResultModel
    {
        public int? ID { get; set; }

        /// <summary>主資源名稱</summary>
        [DisplayName("主資源名稱")]
        public string? MainResourceID { get; set; }

        /// <summary>主資源名稱</summary>
        [DisplayName("主資源名稱")]
        public string? MainResourceIDText { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClass { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClassText { get; set; }

        /// <summary>資產號碼</summary>
        [DisplayName("資產號碼")]
        public string? SecondResourceNo { get; set; }

        /// <summary>子資源名稱</summary>
        [DisplayName("子資源名稱")]
        public string? SecondResourceName { get; set; }

        /// <summary>上下架狀態</summary>
        [DisplayName("上下架狀態")]
        public string? ShelvesStatus { get; set; }

        /// <summary>上下架狀態</summary>
        [DisplayName("上下架狀態")]
        public string? ShelvesStatusText { get; set; }

        /// <summary>借用狀態</summary>
        [DisplayName("借用狀態")]
        public string? BorrowStatus { get; set; }

        /// <summary>借用狀態</summary>
        [DisplayName("借用狀態")]
        public string? BorrowStatusText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowSecondResourceMangImportModel
    {
        /// <summary>主資源名稱</summary>
        [DisplayName("主資源名稱")]
        public string? MainResourceID { get; set; }

        /// <summary>資產號碼</summary>
        [DisplayName("資產號碼")]
        public string? SecondResourceNo { get; set; }

        /// <summary>子資源名稱</summary>
        [DisplayName("子資源名稱")]
        public string? SecondResourceName { get; set; }

        /// <summary>上下架狀態</summary>
        [DisplayName("上下架狀態")]
        public string? ShelvesStatus { get; set; }

        /// <summary>借用狀態</summary>
        [DisplayName("借用狀態")]
        public string? BorrowStatus { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class BorrowSecondResourceMangEditModel
    {
        public int? ID { get; set; }

        /// <summary>主資源名稱</summary>
        [DisplayName("主資源名稱")]
        public string? MainResourceID { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClass { get; set; }

        /// <summary>資產號碼</summary>
        [DisplayName("資產號碼")]
        public string? SecondResourceNo { get; set; }

        /// <summary>子資源名稱</summary>
        [DisplayName("子資源名稱")]
        public string? SecondResourceName { get; set; }

        /// <summary>上下架狀態</summary>
        [DisplayName("上下架狀態")]
        public string? ShelvesStatus { get; set; }

        /// <summary>借用狀態</summary>
        [DisplayName("借用狀態")]
        public string? BorrowStatus { get; set; }

        /// <summary>資產編號</summary>
        [DisplayName("資產編號")]
        public string? SecondResourceSerNo { get; set; }

        /// <summary>設備機號</summary>
        [DisplayName("設備機號")]
        public string? DeviceNo { get; set; }

        /// <summary>品牌型號</summary>
        [DisplayName("品牌型號")]
        public string? Brand { get; set; }

        /// <summary>規格</summary>
        [DisplayName("規格")]
        public string? Specification { get; set; }

        /// <summary>存放位置</summary>
        [DisplayName("存放位置")]
        public string? Location { get; set; }

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

    public class BorrowSecondResourceMangExcelHeaderModel 
    {
        /// <summary>主資源名稱</summary>
        [DisplayName("主資源名稱")]
        public string? MainResourceID { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClass { get; set; }

        /// <summary>資產號碼</summary>
        [DisplayName("資產號碼")]
        public string? SecondResourceNo { get; set; }

        /// <summary>子資源名稱</summary>
        [DisplayName("子資源名稱")]
        public string? SecondResourceName { get; set; }

        /// <summary>上下架狀態</summary>
        [DisplayName("上下架狀態")]
        public string? ShelvesStatus { get; set; }

        /// <summary>借用狀態</summary>
        [DisplayName("借用狀態")]
        public string? BorrowStatus { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }
}
