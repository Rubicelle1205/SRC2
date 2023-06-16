using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebPccuClub.Models
{
    public class PersonalViewModel
    {
        public PersonalEditModel EditModel { get; set; }
    }

    public class PersonalEditModel
    {
        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

        [DisplayName("名稱")]
        public string? UserName { get; set; }

        [DisplayName("信箱")]
        public string? EMail { get; set; }

        [DisplayName("帳號")]
        public string? LoginId { get; set; }

        [DisplayName("密碼")]
        public string? Pwd { get; set; }

        [DisplayName("密碼確認")]
        public string? ConformPwd { get; set; }
    }
}
