using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class DateLineMangViewModel
    {
        public DateLineMangEditModel EditModel { get; set; }
    }

    public class DateLineMangEditModel
    {
        [DisplayName("報備完成期限")]
        public int? ActivityReport { get; set; }

        [DisplayName("行程取消期限")]
        public int? TripCancel { get; set; }

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

        [DisplayName("異動原因")]
        public string? ModifiedReason { get; set; }
    }
}
