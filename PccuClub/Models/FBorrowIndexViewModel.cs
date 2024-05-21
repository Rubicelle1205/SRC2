using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebPccuClub.Models
{
    public class FBorrowIndexViewModel
    {
		public List<FBorrowIndexResultModel> ResultModel { get; set; }

        public FBorrowIndexDetailModel Detail { get; set; }
    }

    public class FBorrowIndexResultModel
	{
		[DisplayName("ID")]
		public string? ID { get; set; }

		[DisplayName("分類名稱")]
        public string? Text { get; set; }

        [DisplayName("封面圖片")]
        public string? CoverPath { get; set; }

    }

    public class FBorrowIndexDetailModel
    {
        [DisplayName("ID")]
        public string? ID { get; set; }

        [DisplayName("類別名稱")]
        public string? Text { get; set; }

        [DisplayName("可借用日期")]
        public DateTime? BorrowSDate { get; set; }

        [DisplayName("可借用日期")]
        public DateTime? BorrowEDate { get; set; }

        [DisplayName("借用規定")]
        public string? BorrowRule { get; set; }

        [DisplayName("費用")]
        public string? BorrowFee { get; set; }

        [DisplayName("庫存機制")]
        public string? ReserveRule { get; set; }

    }
}
