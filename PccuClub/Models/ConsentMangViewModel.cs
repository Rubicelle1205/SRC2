using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ConsentMangViewModel
    {
        public ConsentMangEditModel EditModel { get; set; }
    }

    public class ConsentMangEditModel
    {
        [DisplayName("校內")]
        public string? InSchool { get; set; }

        [DisplayName("校外")]
        public string? OutSchool { get; set; }

        [DisplayName("校內外皆有")]
        public string? InAndOutSchool { get; set; }

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
