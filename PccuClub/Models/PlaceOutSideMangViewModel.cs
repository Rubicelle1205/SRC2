using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class PlaceOutSideMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public PlaceOutSideMangConditionModel ConditionModel { get; set; }

        public List<PlaceOutSideMangResultModel> ResultModel { get; set; }

        public PlaceOutSideMangCreateModel CreateModel { get; set; }

        public PlaceOutSideMangEditModel EditModel { get; set; }

        public PlaceOutSideMangExcelResultModel ExcelModel { get; set; }
    }

    public class PlaceOutSideMangConditionModel
    {
        public PlaceOutSideMangConditionModel()
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

        /// <summary>所在縣市	</summary>
        [DisplayName("所在縣市")]
        public string? CityCode { get; set; }

        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceType { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class PlaceOutSideMangResultModel
    {
        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceID { get; set; }

        /// <summary>所在縣市	</summary>
        [DisplayName("所在縣市")]
        public string? CityCode { get; set; }

        /// <summary>所在縣市	</summary>
        [DisplayName("所在縣市")]
        public string? CityCodeName { get; set; }

        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceType { get; set; }

        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceTypeName { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class PlaceOutSideMangExcelResultModel
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

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class PlaceOutSideMangCreateModel
    {
        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceID { get; set; }

        /// <summary>所在縣市	</summary>
        [DisplayName("所在縣市")]
        public string? CityCode { get; set; }

        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceType { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class PlaceOutSideMangEditModel
    {
        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceID { get; set; }

        /// <summary>所在縣市	</summary>
        [DisplayName("所在縣市")]
        public string? CityCode { get; set; }

        /// <summary>場地類型</summary>
        [DisplayName("場地類型")]
        public string? PlaceType { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        [DisplayName("更新時間")]
        public DateTime? LastModified { get; set; }
        
    }
}
