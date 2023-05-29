using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class MemberMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public MemberMangConditionModel ConditionModel { get; set; }

        public List<MemberMangResultModel> ResultModel { get; set; }

        public MemberMangCreateModel CreateModel { get; set; }

        public MemberMangEditModel EditModel { get; set; }

        public MemberMangExcelResultModel ExcelModel { get; set; }

        public MemberMangPersonalConsentConditionModel PersonalConsentConditionModel { get; set; }

        public List<MemberMangPersonalConsentResultModel> PersonalConsentResultModel { get; set; }

        public MemberMangPersonalConsentEditModel PersonalConsentEditModel { get; set; }
        
    }

    public class MemberMangConditionModel
    {
        public MemberMangConditionModel()
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

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubName { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class MemberMangResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? MemberID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubName { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>參與期間</summary>
        [DisplayName("參與期間")]
        public DateTime? SDuring { get; set; }

        /// <summary>參與期間</summary>
        [DisplayName("參與期間")]
        public DateTime? EDuring { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class MemberMangExcelResultModel
    {
        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubName { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>任職開始期間</summary>
        [DisplayName("參與期間")]
        public string? SDuring { get; set; }

        /// <summary>任職結束期間</summary>
        [DisplayName("參與期間")]
        public string? EDuring { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class MemberMangImportExcelResultModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>參與期間</summary>
        [DisplayName("參與期間")]
        public string? SDuring { get; set; }

        /// <summary>參與期間</summary>
        [DisplayName("參與期間")]
        public string? EDuring { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNo { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? Sex { get; set; }

        /// <summary>聯絡電話</summary>
        [DisplayName("聯絡電話")]
        public string? CellPhone { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class MemberMangCreateModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>參與期間</summary>
        [DisplayName("參與期間")]
        public DateTime? SDuring { get; set; }

        /// <summary>參與期間</summary>
        [DisplayName("參與期間")]
        public DateTime? EDuring { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNo { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? Sex { get; set; }

        /// <summary>聯絡電話</summary>
        [DisplayName("聯絡電話")]
        public string? CellPhone { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class MemberMangEditModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? MemberID { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>參與期間</summary>
        [DisplayName("參與期間")]
        public string? SDuring { get; set; }

        /// <summary>參與期間</summary>
        [DisplayName("參與期間")]
        public string? EDuring { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? UserName { get; set; }

        /// <summary>信箱</summary>
        [DisplayName("信箱")]
        public string? EMail { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNo { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? Sex { get; set; }

        /// <summary>聯絡電話</summary>
        [DisplayName("聯絡電話")]
        public string? CellPhone { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        [DisplayName("最後修改時間")]
        public DateTime? LastModified { get; set; }
    }


    public class MemberMangPersonalConsentConditionModel
    {
        public MemberMangPersonalConsentConditionModel()
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

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class MemberMangPersonalConsentResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? PersonalConID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubName { get; set; }

        [DisplayName("更新時間")]
        public DateTime? LastModified { get; set; }
    }

    public class MemberMangPersonalConsentEditModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? PersonalConID { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubName { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>檔案</summary>
        [DisplayName("檔案")]
        public string? FilePath { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        [DisplayName("最後修改時間")]
        public DateTime? LastModified { get; set; }
    }

}
