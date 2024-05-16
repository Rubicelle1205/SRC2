using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ClubHolisticPassportViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubHolisticPassportConditionModel ConditionModel { get; set; }

        public List<ClubHolisticPassportResultModel> ResultModel { get; set; }

        public ClubHolisticPassportEditModel EditModel { get; set; }

        public ClubHolisticPassportDetailModel DetailModel { get; set; }

    }

    public class ClubHolisticPassportConditionModel
    {
        public ClubHolisticPassportConditionModel()
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

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>排序</summary>
        [DisplayName("排序")]
        public string? OrderBy { get; set; }
    }

    public class ClubHolisticPassportResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? ID { get; set; }

        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? ActID { get; set; }

        /// <summary>全人活動名稱</summary>
        [DisplayName("全人活動名稱")]
        public string? HolisticActName { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActVerify { get; set; }

        /// <summary>審核</summary>
        [DisplayName("審核")]
        public string? ActVerifyText { get; set; }

        /// <summary>報備日期</summary>
        [DisplayName("報備日期")]
        public DateTime? Created { get; set; }
    }

    public class ClubHolisticPassportEditModel
    {
        public string? ID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號")]
        public string? ActID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>全人端名稱</summary>
        [DisplayName("全人端名稱")]
        public string? HolisticActName { get; set; }

        /// <summary>活動說明</summary>
        [DisplayName("活動說明")]
        public string? ActDesc { get; set; }

        /// <summary>群組</summary>
        [DisplayName("群組")]
        public string? MainID { get; set; }

        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? SecondID { get; set; }

        /// <summary>項目</summary>
        [DisplayName("項目")]
        public string? ThridID { get; set; }

        /// <summary>活動開始時間</summary>
        [DisplayName("活動開始時間")]
        public DateTime? ActSTime { get; set; }

        /// <summary>活動結束時間</summary>
        [DisplayName("活動結束時間")]
        public DateTime? ActETime { get; set; }

        /// <summary>報名方式</summary>
        [DisplayName("報名方式")]
        public string? RegistrationWay { get; set; }

        /// <summary>活動地點</summary>
        [DisplayName("活動地點")]
        public string? PlaceSource { get; set; }

        /// <summary>校內建築</summary>
        [DisplayName("校內建築")]
        public string? BuildID { get; set; }

        /// <summary>校內場地</summary>
        [DisplayName("校內場地/")]
        public string? PlaceID { get; set; }

        /// <summary>校內其他場地/校外場地</summary>
        [DisplayName("校內其他場地/校外場地")]
        public string? PlaceName { get; set; }

        /// <summary>主講人</summary>
        [DisplayName("主講人")]
        public string? Presenter { get; set; }

        /// <summary>主講人介紹</summary>
        [DisplayName("主講人介紹")]
        public string? PresenterIntro { get; set; }

        /// <summary>主持人</summary>
        [DisplayName("主持人")]
        public string? Host { get; set; }

        /// <summary>主持人介紹</summary>
        [DisplayName("主持人介紹")]
        public string? HostIntro { get; set; }

        /// <summary>主辦單位名稱</summary>
        [DisplayName("主辦單位名稱")]
        public string? ClubCName { get; set; }

        /// <summary>聯絡人</summary>
        [DisplayName("聯絡人")]
        public string? ContactMan { get; set; }

        /// <summary>負責補登者</summary>
        [DisplayName("負責補登者")]
        public string? RegistrationMan { get; set; }

        /// <summary>開放對象</summary>
        [DisplayName("開放對象")]
        public string? OpenObject { get; set; }

        /// <summary>關鍵字標籤</summary>
        [DisplayName("關鍵字標籤")]
        public string? Tag { get; set; }

        /// <summary>海報縮圖</summary>
        [DisplayName("海報縮圖")]
        public string? PosterIconPath { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }
    }

    public class ClubHolisticPassportDetailModel
    {
        public string? ID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>活動報備編號</summary>
        [DisplayName("活動報備編號")]
        public string? ActID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>全人端名稱</summary>
        [DisplayName("全人端名稱")]
        public string? HolisticActName { get; set; }

        /// <summary>活動說明</summary>
        [DisplayName("活動說明")]
        public string? ActDesc { get; set; }

        /// <summary>群組</summary>
        [DisplayName("群組")]
        public string? MainID { get; set; }

        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? SecondID { get; set; }

        /// <summary>項目</summary>
        [DisplayName("項目")]
        public string? ThridID { get; set; }

        /// <summary>群組</summary>
        [DisplayName("群組")]
        public string? MainIDText { get; set; }

        /// <summary>類別</summary>
        [DisplayName("類別")]
        public string? SecondIDText { get; set; }

        /// <summary>項目</summary>
        [DisplayName("項目")]
        public string? ThridIDText { get; set; }

        /// <summary>活動開始時間</summary>
        [DisplayName("活動開始時間")]
        public DateTime? ActSTime { get; set; }

        /// <summary>活動結束時間</summary>
        [DisplayName("活動結束時間")]
        public DateTime? ActETime { get; set; }

        /// <summary>報名方式</summary>
        [DisplayName("報名方式")]
        public string? RegistrationWay { get; set; }

        /// <summary>活動地點</summary>
        [DisplayName("活動地點")]
        public string? PlaceSource { get; set; }

        /// <summary>活動地點</summary>
        [DisplayName("活動地點")]
        public string? PlaceSourceText { get; set; }

        /// <summary>校內建築</summary>
        [DisplayName("校內建築")]
        public string? BuildID { get; set; }

        /// <summary>校內場地</summary>
        [DisplayName("校內場地/")]
        public string? PlaceID { get; set; }

        /// <summary>校內其他場地/校外場地</summary>
        [DisplayName("校內其他場地/校外場地")]
        public string? PlaceName { get; set; }

        /// <summary>主講人</summary>
        [DisplayName("主講人")]
        public string? Presenter { get; set; }

        /// <summary>主講人介紹</summary>
        [DisplayName("主講人介紹")]
        public string? PresenterIntro { get; set; }

        /// <summary>主持人</summary>
        [DisplayName("主持人")]
        public string? Host { get; set; }

        /// <summary>主持人介紹</summary>
        [DisplayName("主持人介紹")]
        public string? HostIntro { get; set; }

        /// <summary>主辦單位名稱</summary>
        [DisplayName("主辦單位名稱")]
        public string? ClubCName { get; set; }

        /// <summary>聯絡人</summary>
        [DisplayName("聯絡人")]
        public string? ContactMan { get; set; }

        /// <summary>負責補登者</summary>
        [DisplayName("負責補登者")]
        public string? RegistrationMan { get; set; }

        /// <summary>開放對象</summary>
        [DisplayName("開放對象")]
        public string? OpenObject { get; set; }

        /// <summary>關鍵字標籤</summary>
        [DisplayName("關鍵字標籤")]
        public string? Tag { get; set; }

        /// <summary>海報縮圖</summary>
        [DisplayName("海報縮圖")]
        public string? PosterIconPath { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }
    }
}
