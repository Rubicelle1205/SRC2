using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebPccuClub.Models
{
    public class ClubLeaderInfoViewModel
    {
        public ClubLeaderInfoEditModel EditModel { get; set; }
    }

    public class ClubLeaderInfoEditModel
    {
		[DisplayName("帳號")]
		public string? ClubId { get; set; }

		[DisplayName("學校/人員代號")]
        public string? FUserId { get; set; }

        [DisplayName("單位名稱")]
        public string? ClubCName { get; set; }

        [DisplayName("姓名")]
        public string? UserName { get; set; }

        [DisplayName("系級")]
        public string? Department { get; set; }

        [DisplayName("行動電話")]
        public string? CellPhone { get; set; }

        [DisplayName("常用EMAIL")]
        public string? EMail { get; set; }
    }
}
