using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class PlaceSchoolElseMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public PlaceSchoolElseMangConditionModel ConditionModel { get; set; }

        public List<PlaceSchoolElseMangResultModel> ResultModel { get; set; }

        public PlaceSchoolElseMangCreateModel CreateModel { get; set; }

        public PlaceSchoolElseMangEditModel EditModel { get; set; }

        public PlaceSchoolElseMangExcelResultModel ExcelModel { get; set; }
    }

    public class PlaceSchoolElseMangConditionModel
    {
        public PlaceSchoolElseMangConditionModel()
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

        /// <summary>樓層</summary>
        [DisplayName("樓層")]
        public string? Floor { get; set; }

        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>場地代碼</summary>
        [DisplayName("場地代碼")]
        public string? PlaceId { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class PlaceSchoolElseMangResultModel
    {
        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>樓層</summary>
        [DisplayName("樓層")]
        public string? Floor { get; set; }

        /// <summary>場地代碼</summary>
        [DisplayName("場地代碼")]
        public string? PlaceId { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        [DisplayName("建立時間")]
        public DateTime? LastModified { get; set; }
    }

    public class PlaceSchoolElseMangExcelResultModel
    {
        /// <summary>所在縣市	</summary>
        [DisplayName("所在縣市")]
        public string? CityCode { get; set; }

        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceType { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>樓層</summary>
        [DisplayName("樓層")]
        public string? Floor { get; set; }

        [DisplayName("更新時間")]
        public DateTime? LastModified { get; set; }
    }

    public class PlaceSchoolElseMangCreateModel
    {
        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildId { get; set; }

        /// <summary>場地代碼</summary>
        [DisplayName("場地代碼")]
        public string? PlaceID { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>樓層</summary>
        [DisplayName("樓層")]
        public string? Floor { get; set; }

        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class PlaceSchoolElseMangEditModel
    {
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        [DisplayName("更新時間")]
        public DateTime? LastModified { get; set; }

        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildId { get; set; }

        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceID { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>樓層</summary>
        [DisplayName("樓層")]
        public string? Floor { get; set; }

        [DisplayName("備註")]
        public string? Memo { get; set; }

    }
}
