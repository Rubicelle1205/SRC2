using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BakeendLoginViewModel
    {
        /// <summary> 使用者帳號 </summary>
        [MaxLength(10, ErrorMessage = "請勿輸入超過10個字!!")]
        [Required(ErrorMessage = "使用者帳號必填")]

        public string LoginID { get; set; }

        /// <summary> 使用者密碼 </summary>
        public string PassWord { get; set; }

        public string Mail { get; set; }
    }
}
