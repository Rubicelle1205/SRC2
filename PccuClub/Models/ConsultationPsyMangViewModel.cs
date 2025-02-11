using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ConsultationPsyMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ConsultationPsyMangConditionModel ConditionModel { get; set; }

        public List<ConsultationPsyMangResultModel> ResultModel { get; set; }

        public ConsultationPsyMangCreateModel CreateModel { get; set; }

        public ConsultationPsyMangEditModel EditModel { get; set; }
    }

    public class ConsultationPsyMangConditionModel
    {
        public ConsultationPsyMangConditionModel()
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

        /// <summary>心理師名稱</summary>
        [DisplayName("心理師名稱")]
        public string? UserName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ConsultationPsyMangResultModel
    {
        public string? LoginID { get; set; }

        /// <summary>心理師名稱</summary>
        [DisplayName("心理師名稱")]
        public string? UserName { get; set; }

        /// <summary>Memo</summary>
        [DisplayName("Memo")]
        public string? Memo { get; set; }
    }

    public class ConsultationPsyMangCreateModel
    {
        public string? LoginID { get; set; }

        /// <summary>心理師名稱</summary>
        [DisplayName("心理師名稱")]
        public string? UserName { get; set; }

        /// <summary>Memo</summary>
        [DisplayName("Memo")]
        public string? Memo { get; set; }
    }

    public class ConsultationPsyMangEditModel
    {
        public string? LoginID { get; set; }

        /// <summary>心理師名稱</summary>
        [DisplayName("心理師名稱")]
        public string? UserName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>預約時間</summary>
        [DisplayName("預約時間")]
        public string? strAppointmentTime { get; set; }

        public List<AppointmentTimeModel> LstAppointmentTimeModel = new List<AppointmentTimeModel>();
    }

    public class AppointmentTimeModel
    {
        public string? ID { get; set; }
        public string? Type { get; set; }
        public string? Week { get; set; }
        public string? Hour { get; set; }
    }
}
