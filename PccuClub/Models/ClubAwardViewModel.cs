using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubAwardViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubAwardConditionModel ConditionModel { get; set; }

		public List<ClubAwardResultModel> ResultModel { get; set; }

        public List<ClubAwardExcelResultModel> ExcelModel { get; set; }

        public ClubAwardCreateModel CreateModel { get; set; }

		public ClubAwardEditModel EditModel { get; set; }

        public List<ClubAwardDetailModel> DetailModel { get; set; }

    }

    public class ClubAwardConditionModel
    {
        public ClubAwardConditionModel()
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

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>校內/校外</summary>
        [DisplayName("校內/校外")]
        public string? AwardInOrOut { get; set; }
    }

    public class ClubAwardResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? AwdID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public DateTime? AwdDate { get; set; }

        /// <summary>活動類型</summary>
        [DisplayName("活動類型")]
        public string? AwdType { get; set; }

        /// <summary>校內/校外</summary>
        [DisplayName("校內/校外")]
        public string? AwardInOrOut { get; set; }

        /// <summary>校內/校外</summary>
        [DisplayName("校內/校外")]
        public string? AwardInOrOutText { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActVerify { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActVerifyText { get; set; }

    }

    public class ClubAwardCreateModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? Organizer { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>獲獎類別</summary>
        [DisplayName("獲獎類別")]
        public string? AwdType { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        /// <summary>獲獎日期</summary>
        [DisplayName("獲獎日期")]
        public string? AwdDate { get; set; }

        /// <summary>附件</summary>
        [DisplayName("附件")]
        public string? Attachment { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }
    }

    public class ClubAwardEditModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? AwdID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>主辦單位</summary>
        [DisplayName("主辦單位")]
        public string? Organizer { get; set; }

        /// <summary>校內/校外</summary>
        [DisplayName("校內/校外")]
        public string? AwardInOrOut { get; set; }

        /// <summary>校內/校外</summary>
        [DisplayName("校內/校外")]
        public string? AwardInOrOutText { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>獲獎類別</summary>
        [DisplayName("獲獎類別")]
        public string? AwdType { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        /// <summary>獲獎日期</summary>
        [DisplayName("獲獎日期")]
        public DateTime? AwdDate { get; set; }

        /// <summary>附件</summary>
        [DisplayName("附件")]
        public string? Attachment { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ClubAwardExcelResultModel
	{
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? AwdActName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public DateTime? AwdDate { get; set; }

        /// <summary>活動類型</summary>
        [DisplayName("活動類型")]
        public string? AwdType { get; set; }

        /// <summary>獎項名稱</summary>
        [DisplayName("獎項名稱")]
        public string? AwdName { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActVerifyText { get; set; }
    }

    public class ClubAwardDetailModel
    {
        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }
    }
}
