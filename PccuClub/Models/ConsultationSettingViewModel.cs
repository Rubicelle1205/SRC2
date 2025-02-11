using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ConsultationSettingViewModel
    {
        public ConsultationSettingEditModel EditModel { get; set; }
    }

    public class ConsultationSettingEditModel
    {
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("派案學號萬用碼")]
        public string? UniversalCode { get; set; }

        [DisplayName("初談預約通知用信箱")]
        public string? NotifyMail { get; set; }

        [DisplayName("建立者")]
        public string? Creator { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }

        [DisplayName("最後異動者")]
        public string? LastModifier { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }
    }
}
