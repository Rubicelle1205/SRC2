using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class PlaceSchoolMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public PlaceSchoolMangConditionModel ConditionModel { get; set; }

        public List<PlaceSchoolMangResultModel> ResultModel { get; set; }

        public PlaceSchoolMangCreateModel CreateModel { get; set; }

        public PlaceSchoolMangEditModel EditModel { get; set; }

        public PlaceSchoolMangBatchAddActModel BatchAddActModel { get; set; }
    }

    public class PlaceSchoolMangConditionModel
    {
        public PlaceSchoolMangConditionModel()
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

        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>場地代碼</summary>
        [DisplayName("場地代碼")]
        public string? PlaceId { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? PlaceStatus { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class PlaceSchoolMangResultModel
    {
        /// <summary>場地代碼</summary>
        [DisplayName("場地代碼")]
        public string? PlaceId { get; set; }

        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildName { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>場地層級</summary>
        [DisplayName("場地層級")]
        public string? PlaceDesc { get; set; }

        /// <summary>可容納人數</summary>
        [DisplayName("可容納人數")]
        public string? Capacity { get; set; }

        /// <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? IsEnable { get; set; }

        // <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? IsEnableText { get; set; }
        
        [DisplayName("更新時間")]
        public DateTime? LastModified { get; set; }
    }

    public class PlaceSchoolMangCreateModel
    {
        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildId { get; set; }

        /// <summary>樓層</summary>
        [DisplayName("樓層")]
        public string? Floor { get; set; }

        /// <summary>場地代碼</summary>
        [DisplayName("場地代碼")]
        public string? PlaceID { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>場地層級</summary>
        [DisplayName("場地層級")]
        public string? PlaceDesc { get; set; }

        /// <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? IsEnable { get; set; }

        /// <summary>平日借用時間</summary>
        [DisplayName("平日借用時間")]
        public string? Normal_STime { get; set; }

        /// <summary>平日借用時間</summary>
        [DisplayName("平日借用時間")]
        public string? Normal_ETime { get; set; }

        /// <summary>假日借用時間</summary>
        [DisplayName("假日借用時間")]
        public string? Holiday_STime { get; set; }

        /// <summary>假日借用時間</summary>
        [DisplayName("假日借用時間")]
        public string? Holiday_ETime { get; set; }
        
        /// <summary>可容納人數</summary>
        [DisplayName("可容納人數")]
        public string? Capacity { get; set; }

        /// <summary>已配置資訊器材</summary>
        [DisplayName("已配置資訊器材")]
        public string? PlaceEquip { get; set; }

        [DisplayName("場地備註")]
        public string? Memo { get; set; }
    }

    public class PlaceSchoolMangEditModel
    {
        /// <summary>樓館名稱</summary>
        [DisplayName("樓館名稱")]
        public string? BuildId { get; set; }

        /// <summary>樓層</summary>
        [DisplayName("樓層")]
        public string? Floor { get; set; }

        /// <summary>場地代碼</summary>
        [DisplayName("場地代碼")]
        public string? PlaceID { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>場地層級</summary>
        [DisplayName("場地層級")]
        public string? PlaceDesc { get; set; }

        /// <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? IsEnable { get; set; }

        /// <summary>平日借用時間</summary>
        [DisplayName("平日借用時間")]
        public string? Normal_STime { get; set; }

        /// <summary>平日借用時間</summary>
        [DisplayName("平日借用時間")]
        public string? Normal_ETime { get; set; }

        /// <summary>假日借用時間</summary>
        [DisplayName("假日借用時間")]
        public string? Holiday_STime { get; set; }

        /// <summary>假日借用時間</summary>
        [DisplayName("假日借用時間")]
        public string? Holiday_ETime { get; set; }

        /// <summary>可容納人數</summary>
        [DisplayName("可容納人數")]
        public string? Capacity { get; set; }

        /// <summary>已配置資訊器材</summary>
        [DisplayName("已配置資訊器材")]
        public string? PlaceEquip { get; set; }

        [DisplayName("場地備註")]
        public string? Memo { get; set; }
    }

    public class PlaceSchoolMangBatchAddActModel
    {
        /// <summary>樓館分類</summary>
        [DisplayName("樓館分類")]
        public string? BuildId { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceID { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>借用類型</summary>
        [DisplayName("借用類型")]
        public string? BorrowType { get; set; }

        /// <summary>借用/關閉起訖日</summary>
        [DisplayName("借用/關閉起訖日")]
        public string? SDate{ get; set; }

        /// <summary>借用/關閉起訖日</summary>
        [DisplayName("借用/關閉起訖日")]
        public string? EDate { get; set; }

        /// <summary>借用/關閉星期幾</summary>
        [DisplayName("借用/關閉星期幾")]
        public string? Week { get; set; }

        /// <summary>借用/關閉借用時段</summary>
        [DisplayName("借用/關閉借用時段")]
        public string? STime { get; set; }

        /// <summary>借用/關閉借用時段</summary>
        [DisplayName("借用/關閉借用時段")]
        public string? ETime { get; set; }

        /// <summary>借用名稱(原因)</summary>
        [DisplayName("借用名稱(原因)")]
        public string? Reason { get; set; }

        /// <summary>簡介(備註)</summary>
        [DisplayName("簡介(備註)")]
        public string? Memo { get; set; }
    }
}
