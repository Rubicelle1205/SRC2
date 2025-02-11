using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ConsultationApplyViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

		public ConsultationApplyCreateModel CreateModel { get; set; }
    }

	public class ConsultationApplyCreateModel
	{
        /// <summary>姓名</summary>
        [DisplayName("姓名")]
		public string? Name { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
		public string? Department { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
		public string? SNO { get; set; }

        /// <summary>電話</summary>
        [DisplayName("電話")]
		public string? Tel { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? Sex { get; set; }

        /// <summary>國籍</summary>
        [DisplayName("國籍")]
		public string? Citizenship { get; set; }

        /// <summary>國家名稱</summary>
        [DisplayName("國家名稱")]
		public string? CitizenshipName { get; set; }

        /// <summary>過去2週曾出現這些想法或計劃</summary>
        [DisplayName("過去2週曾出現這些想法或計劃")]
		public string? strCounsellingStatus { get; set; }

        /// <summary>可初談時段</summary>
        [DisplayName("可初談時段")]
        public string? strAppointmentTime { get; set; }
    }



}
