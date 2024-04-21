using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ConsultationSpaceMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ConsultationSpaceMangConditionModel ConditionModel { get; set; }

        public List<ConsultationSpaceMangResultModel> ResultModel { get; set; }

        public ConsultationSpaceMangCreateModel CreateModel { get; set; }

        public ConsultationSpaceMangEditModel EditModel { get; set; }
    }

    public class ConsultationSpaceMangConditionModel
    {
        public ConsultationSpaceMangConditionModel()
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

        /// <summary>空間名稱</summary>
        [DisplayName("空間名稱")]
        public string? RoomName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ConsultationSpaceMangResultModel
    {
        public string? ID { get; set; }

        /// <summary>空間名稱</summary>
        [DisplayName("空間名稱")]
        public string? RoomName { get; set; }

        /// <summary>Memo</summary>
        [DisplayName("Memo")]
        public string? Memo { get; set; }
    }

    public class ConsultationSpaceMangCreateModel
    {
        public string? ID { get; set; }

        /// <summary>空間名稱</summary>
        [DisplayName("空間名稱")]
        public string? RoomName { get; set; }

        /// <summary>Memo</summary>
        [DisplayName("Memo")]
        public string? Memo { get; set; }

        public string? strAppointmentTime { get; set; }

        public List<AppointmentTimeModel> LstAppointmentTimeModel = new List<AppointmentTimeModel>();
    }

    public class ConsultationSpaceMangEditModel
    {
        public string? ID { get; set; }

        /// <summary>空間名稱</summary>
        [DisplayName("空間名稱")]
        public string? RoomName { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>預約時間</summary>
        [DisplayName("預約時間")]
        public string? strAppointmentTime { get; set; }

        public List<AppointmentTimeModel> LstAppointmentTimeModel = new List<AppointmentTimeModel>();
    }

}
