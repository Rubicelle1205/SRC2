using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BorrowMailTemplateMangViewModel    
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowMailTemplateMangConditionModel ConditionModel { get; set; }

        public List<BorrowMailTemplateMangResultModel> ResultModel { get; set; }

        public BorrowMailTemplateMangCreateModel CreateModel { get; set; }

        public BorrowMailTemplateMangEditModel EditModel { get; set; }

    }

    public class BorrowMailTemplateMangConditionModel
    {
        public BorrowMailTemplateMangConditionModel()
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

        /// <summary>範本名稱</summary>
        [DisplayName("範本名稱")]
        public string? TemplateName { get; set; }

        /// <summary>借用類別</summary>
        [DisplayName("借用類別")]
        public string? BorrowMainClassID { get; set; }

        /// <summary>是否啟用</summary>
        [DisplayName("是否啟用")]
        public bool? IsEnable { get; set; }

        [DisplayName("建立日期_起")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("建立日期_迄")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class BorrowMailTemplateMangResultModel
    {
        public int? TemplateId { get; set; }

        /// <summary>範本名稱</summary>
        [DisplayName("範本名稱")]
        public string? TemplateName { get; set; }

        /// <summary>借用類別</summary>
        [DisplayName("借用類別")]
        public string? BorrowMainClassID { get; set; }

        /// <summary>借用類別</summary>
        [DisplayName("借用類別")]
        public string? BorrowMainClassIDText { get; set; }

        /// <summary>信件主旨</summary>
        [DisplayName("信件主旨")]
        public string? SubjectTemplate { get; set; }

        /// <summary>是否啟用</summary>
        [DisplayName("是否啟用")]
        public string? IsEnable { get; set; }

        /// <summary>是否啟用</summary>
        [DisplayName("是否啟用")]
        public string? IsEnableText { get; set; }
    }

    public class BorrowMailTemplateMangCreateModel
    {
        /// <summary>範本名稱</summary>
        [DisplayName("範本名稱")]
        public string? TemplateName { get; set; }

        /// <summary>借用類別</summary>
        [DisplayName("借用類別")]
        public string? BorrowMainClassID { get; set; }

        /// <summary>是否啟用</summary>
        [DisplayName("是否啟用")]
        public string? IsEnable { get; set; }

        /// <summary>信件主旨</summary>
        [DisplayName("信件主旨")]
        public string? SubjectTemplate { get; set; }

        /// <summary>信件內容</summary>
        [DisplayName("信件內容")]
        public string? BodyTemplate { get; set; }
    }

    public class BorrowMailTemplateMangEditModel
    {
        public int? TemplateId { get; set; }

        /// <summary>範本名稱</summary>
        [DisplayName("範本名稱")]
        public string? TemplateName { get; set; }

        /// <summary>借用類別</summary>
        [DisplayName("借用類別")]
        public string? BorrowMainClassID { get; set; }

        /// <summary>是否啟用</summary>
        [DisplayName("是否啟用")]
        public string? IsEnable { get; set; }

        /// <summary>信件主旨</summary>
        [DisplayName("信件主旨")]
        public string? SubjectTemplate { get; set; }

        /// <summary>信件內容</summary>
        [DisplayName("信件內容")]
        public string? BodyTemplate { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public string? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        public string? LastModified { get; set; }
    }
}
