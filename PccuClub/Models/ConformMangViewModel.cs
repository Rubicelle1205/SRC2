using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ConformMangViewModel
    {
        public ConformMangEditModel EditModel { get; set; }
    }

    public class ConformMangEditModel
    {
        [DisplayName("個人資料告知暨同意書")]
        public string? PersonalConform { get; set; }

        [DisplayName("活動報備管理提醒內容")]
        public string? ActivityConform { get; set; }

        [DisplayName("社團基本資料提醒內容")]
        public string? ClubInfoConform { get; set; }

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
