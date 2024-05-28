using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class FResourceBorrowViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public FResourceBorrowConditionModel ConditionModel { get; set; }

        public List<FResourceBorrowResultModel> ResultModel { get; set; }

        public FResourceBorrowEditModel EditModel { get; set; }

        public FResourceBorrowResultDetailModel ResultDetailModel { get; set; }

        public List<FResourceBorrowResourceResultModel> ResourceResultModel { get; set; }

        public List<FResourceBorrowResourceBorrowedModel> ResourceBorrowedResultModel { get; set; }
    }

    public class FResourceBorrowConditionModel
    {
        public FResourceBorrowConditionModel()
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
        public string? BorrowSecondClassID { get; set; }

        /// <summary>SDate</summary>
        [DisplayName("SDate")]
        public DateTime? SDate { get; set; }

    }

    public class FResourceBorrowResultModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? BorrowMainID { get; set; }

        /// <summary>申請單位</summary>
        [DisplayName("申請單位")]
        public string? ApplyUnitName { get; set; }
    }

    public class FResourceBorrowResultDetailModel
    {
        public List<FResourceBorrowResultDetailResourceModel> LstResource = new List<FResourceBorrowResultDetailResourceModel>();

        /// <summary>提出申請日</summary>
        [DisplayName("提出申請日")]
        public DateTime? Created { get; set; }

        /// <summary>領取日期</summary>
        [DisplayName("領取日期")]
        public DateTime? TakeSDate { get; set; }

        /// <summary>歸還日期</summary>
        [DisplayName("歸還日期")]
        public DateTime? TakeEDate { get; set; }

        /// <summary>申請單位類型</summary>
        [DisplayName("申請單位類型")]
        public string? ApplyUnitType { get; set; }

        /// <summary>申請單位類型</summary>
        [DisplayName("申請單位類型")]
        public string? ApplyUnitTypeText { get; set; }

        /// <summary>申請單位</summary>
        [DisplayName("申請單位")]
        public string? ApplyUnitName { get; set; }

        /// <summary>申請人員</summary>
        [DisplayName("申請人員")]
        public string? ApplyMan { get; set; }

        /// <summary>申請目的</summary>
        [DisplayName("申請目的")]
        public string? ApplyPurpose { get; set; }
    }

    public class FResourceBorrowResultDetailResourceModel
    {
        /// <summary>借用資源</summary>
        [DisplayName("借用資源")]
        public string? ResourceName { get; set; }

        /// <summary>數量</summary>
        [DisplayName("數量")]
        public string? Amt { get; set; }
    }


    public class FResourceBorrowResourceResultModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? MainResourceID { get; set; }

        /// <summary>資源類別</summary>
        [DisplayName("資源類別")]
        public string? MainClass { get; set; }

        /// <summary>資源類別</summary>
        [DisplayName("資源類別")]
        public string? MainClassText { get; set; }

        /// <summary>資源名稱</summary>
        [DisplayName("資源名稱")]
        public string? MainResourceName { get; set; }

        /// <summary>簡述</summary>
        [DisplayName("簡述")]
        public string? ShortDesc { get; set; }

        /// <summary>當日剩餘數量</summary>
        [DisplayName("當日剩餘數量")]
        public string? RemainAmt { get; set; }

        /// <summary>資源總數量</summary>
        [DisplayName("資源總數量")]
        public string? AmtShelves { get; set; }
    }

    public class FResourceBorrowResourceBorrowedModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? MainResourceID { get; set; }

        /// <summary>借用數量</summary>
        [DisplayName("借用數量")]
        public string? BorrowAmt { get; set; }

    }

    public class FResourceBorrowEditModel
    {
        public List<FResourceBorrowEditDetailModel> LstDetail = new List<FResourceBorrowEditDetailModel>();

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
        public string? SecondClassText { get; set; }

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

    }

    public class FResourceBorrowEditDetailModel
    {
        /// <summary>資源名稱</summary>
        [DisplayName("資源名稱")]
        public string? MainResourceID { get; set; }

        /// <summary>資源名稱</summary>
        [DisplayName("資源名稱")]
        public string? MainResourceName { get; set; }

        /// <summary>資產號碼</summary>
        [DisplayName("資產號碼")]
        public string? SecondResourceName { get; set; }

        /// <summary>借出狀態</summary>
        [DisplayName("借出狀態")]
        public string? BorrowStatus { get; set; }

        /// <summary>借出狀態</summary>
        [DisplayName("借出狀態")]
        public string? BorrowStatusText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }
}
