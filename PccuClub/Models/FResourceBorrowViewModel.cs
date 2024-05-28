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

        public FResourceBorrowResultDetailModel ResultDetailModel { get; set; }

        public List<FResourceBorrowResourceResultModel> ResourceResultModel { get; set; }
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
        /// <summary>日期</summary>
        [DisplayName("日期")]
        public string? Date { get; set; }

        public List<PlaceData> LstPlaceData = new List<PlaceData>();
    }
}
