using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class CadreMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public CadreMangConditionModel ConditionModel { get; set; }

        public List<CadreMangResultModel> ResultModel { get; set; }

        public CadreMangCreateModel CreateModel { get; set; }

        public CadreMangEditModel EditModel { get; set; }

        public CadreMangExcelResultModel ExcelModel { get; set; }
    }

    public class CadreMangConditionModel
    {
        public CadreMangConditionModel()
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

        /// <summary>職別</summary>
        [DisplayName("職別")]
        public string? CadreName { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class CadreMangResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? CadreID { get; set; }

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

        /// <summary>職別</summary>
        [DisplayName("職別")]
        public string? CadreName { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public DateTime? SDuring { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public DateTime? EDuring { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class CadreMangExcelResultModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? CadreID { get; set; }

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

        /// <summary>職別</summary>
        [DisplayName("職別")]
        public string? CadreName { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public DateTime? SDuring { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public DateTime? EDuring { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class CadreMangCreateModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public DateTime? SDuring { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public DateTime? EDuring { get; set; }

        /// <summary>職別</summary>
        [DisplayName("職別")]
        public string? CadreName { get; set; }

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

    public class CadreMangEditModel
    {
        /// <summary>代號</summary>
        [DisplayName("代號")]
        public string? CadreID { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubID { get; set; }

        /// <summary>社團名稱</summary>
        [DisplayName("社團名稱")]
        public string? ClubCName { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public string? SDuring { get; set; }

        /// <summary>任職期間</summary>
        [DisplayName("任職期間")]
        public string? EDuring { get; set; }

        /// <summary>職別</summary>
        [DisplayName("職別")]
        public string? CadreName { get; set; }

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
}
