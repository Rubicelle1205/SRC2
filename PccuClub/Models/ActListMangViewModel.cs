using NPOI.OpenXmlFormats.Dml.Diagram;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ActListMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ActListMangConditionModel ConditionModel { get; set; }
        public List<ActListMangResultModel> ResultModel { get; set; }

        public ActListMangCreateModel CreateModel { get; set; }

        //public ActListMangGetPlaceModel GetPlaceModel { get; set; }

        public List<ActListMangPlaceUsedModel> LstPlaceUsedModel { get; set; }

        public List<ActListMangTodayActModel1> LstTodayActModel { get; set; }

        public ActListMangRundownModel RundownModel { get; set; }






        public ActListMangEditModel EditModel { get; set; }

        public ActListMangExcelResultModel ExcelModel { get; set; }


        public List<ActListMangPlaceDataModel> PlaceDataModel { get; set; }
    }

    public class ActListMangConditionModel
    {
        public ActListMangConditionModel()
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

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>社團組別</summary>
        [DisplayName("社團組別")]
        public string? LifeClass { get; set; }

        /// <summary>活動編號</summary>
        [DisplayName("活動編號")]
        public string? ActId { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ClubName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ActListMangResultModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>活動編號</summary>
        [DisplayName("活動編號")]
        public string? ActId { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ClubName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動開始日</summary>
        [DisplayName("活動開始日")]
        public DateTime? SDate { get; set; }

        /// <summary>活動結束日</summary>
        [DisplayName("活動結束日")]
        public DateTime? EDate { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }
    }






    public class ActListMangRundownModel
    {
        public string? PlaceSource { get; set; }
        public string? Date { get; set; }
        public string? STime { get; set; }
        public string? ETime { get; set; }
        public string? PlaceID { get; set; }
        public string? strRundown { get; set; }

    }

    public class ActListMangPlaceUsedModel
    { 
        public string? PlaceName { get; set; }
        public string? STime { get; set; }
        public string? ETime { get; set; }
    }

    public class ActListMangTodayActModel1
    {
        public string? ActName { get; set; }
        public string? BrrowClubID { get; set; }
        public string? BrrowClubName { get; set; }
        public string? STime { get; set; }
        public string? ETime { get; set; }
    }

    #region 建立


    public class ActListMangCreateModel
    {
        public List<AllPlaceUsedStatus> LstAllPlaceUseStatus = new List<AllPlaceUsedStatus>();

        public string? strRundown { get; set; }

        #region Section1

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>活動類型</summary>
        [DisplayName("活動類型")]
        public string? StaticOrDynamic { get; set; }

        /// <summary>活動地點</summary>
        [DisplayName("活動地點")]
        public string? ActInOrOut { get; set; }

        /// <summary>活動人數</summary>
        [DisplayName("活動人數")]
        public string? Capacity { get; set; }

        /// <summary>活動性質</summary>
        [DisplayName("活動性質")]
        public string? ActType { get; set; }

        /// <summary>使用資訊設備</summary>
        [DisplayName("使用資訊設備")]
        public string? UseITEquip { get; set; }

        /// <summary>活動簡介</summary>
        [DisplayName("活動簡介")]
        public string? Memo { get; set; }

        /// <summary>聯合國SDGs永續發展目標</summary>
        [DisplayName("聯合國SDGs永續發展目標")]
        public string? SDGs { get; set; }

        /// <summary>是否申請全人學習護照</summary>
        [DisplayName("是否申請全人學習護照")]
        public string? PassPort { get; set; }

        #endregion

        #region Section2

        /// <summary>選擇日期</summary>
        [DisplayName("選擇日期")]
        public string? ActDate { get; set; }

        /// <summary>開始時間</summary>
        [DisplayName("開始時間")]
        public string? STime { get; set; }

        /// <summary>結束時間</summary>
        [DisplayName("結束時間")]
        public string? ETime { get; set; }

        /// <summary>校內/校內其他/校外</summary>
        [DisplayName("校內/校內其他/校外")]
        public string? PlaceSource { get; set; }

        /// <summary>選擇樓館</summary>
        [DisplayName("選擇樓館")]
        public string? Buildid { get; set; }

        /// <summary>選擇校內場地</summary>
        [DisplayName("選擇校內場地")]
        public string? PlaceId { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        #endregion

        #region Section3

        /// <summary>上傳活動企劃書</summary>
        [DisplayName("上傳活動企劃書")]
        public string? ActProposal { get; set; }

        #endregion

        #region Section4

        /// <summary>領隊姓名</summary>
        [DisplayName("領隊姓名")]
        public string? LeaderName { get; set; }

        /// <summary>領隊電話</summary>
        [DisplayName("領隊電話")]
        public string? LeaderTel { get; set; }

        /// <summary>領隊手機</summary>
        [DisplayName("領隊手機")]
        public string? LeaderPhone { get; set; }

        /// <summary>活動負責人姓名</summary>
        [DisplayName("活動負責人姓名")]
        public string? ManagerName { get; set; }

        /// <summary>活動負責人電話</summary>
        [DisplayName("活動負責人電話")]
        public string? ManagerTel { get; set; }

        /// <summary>活動負責人手機</summary>
        [DisplayName("活動負責人手機")]
        public string? ManagerPhone { get; set; }

        #endregion

        #region Section5

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核備註</summary>
        [DisplayName("審核備註")]
        public string? ActVerifyMemo { get; set; }

        #endregion

    }

    public class AllPlaceUsedStatus
    { 
        public string? PlaceID { get; set; }
        public string? PlaceName { get; set; }
        public string? STime { get; set; }
        public string? ETime { get; set; }
    }

    public class PlaceOPENStatus
    {
        /// <summary>場地代號</summary>
        [DisplayName("場地代號")]
        public string? PlaceId { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>可使用人數</summary>
        [DisplayName("可使用人數")]
        public string? Capacity { get; set; }

        /// <summary>已配置資訊器材</summary>
        [DisplayName("已配置資訊器材")]
        public string? PlaceEquip { get; set; }

        /// <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? PlaceStatus { get; set; }

        /// <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? PlaceStatusText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>平日開放時間</summary>
        [DisplayName("平日開放時間")]
        public string? Normal_STime { get; set; }

        /// <summary>平日開放時間</summary>
        [DisplayName("平日開放時間")]
        public string? Normal_ETime { get; set; }

        /// <summary>假日開放時間</summary>
        [DisplayName("假日開放時間")]
        public string? Holiday_STime { get; set; }

        /// <summary>假日開放時間</summary>
        [DisplayName("假日開放時間")]
        public string? Holiday_ETime { get; set; }
    }

    public class PlaceRunDown
    { 
        public string? ActName { get; set; }
        public string? ClubID { get; set; }
        public string? STime { get; set; }
        public string? ETime { get; set; } 
    }

    #endregion



















    public class ActListMangEditModel
    {
        
    }

    public class ActListMangPlaceDataModel
    {
        /// <summary>場地代號</summary>
        [DisplayName("場地代號")]
        public string? PlaceId { get; set; }

        /// <summary>場地名稱</summary>
        [DisplayName("場地名稱")]
        public string? PlaceName { get; set; }

        /// <summary>可使用人數</summary>
        [DisplayName("可使用人數")]
        public string? Capacity { get; set; }

        /// <summary>已配置資訊器材</summary>
        [DisplayName("已配置資訊器材")]
        public string? PlaceEquip { get; set; }

        /// <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? PlaceStatus { get; set; }

        /// <summary>場地狀態</summary>
        [DisplayName("場地狀態")]
        public string? PlaceStatusText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>平日開放時間</summary>
        [DisplayName("平日開放時間")]
        public string? Normal_STime { get; set; }

        /// <summary>平日開放時間</summary>
        [DisplayName("平日開放時間")]
        public string? Normal_ETime { get; set; }

        /// <summary>假日開放時間</summary>
        [DisplayName("假日開放時間")]
        public string? Holiday_STime { get; set; }

        /// <summary>假日開放時間</summary>
        [DisplayName("假日開放時間")]
        public string? Holiday_ETime { get; set; }
    }


    public class ActListMangExcelResultModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>活動編號</summary>
        [DisplayName("活動編號")]
        public string? ActId { get; set; }

        /// <summary>單位名稱</summary>
        [DisplayName("單位名稱")]
        public string? ClubName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }
    }
}
