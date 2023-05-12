using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class ClubMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ClubMangConditionModel ConditionModel { get; set; }

        public List<ClubMangResultModel> ResultModel { get; set; }

        public ClubMangCreateModel CreateModel { get; set; }

        public ClubMangEditModel EditModel { get; set; }
    }

    public class ClubMangConditionModel
    {
        public ClubMangConditionModel()
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

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>社團名稱(中文、英文)</summary>
        [DisplayName("社團名稱(中文、英文)")]
        public string? ClubName { get; set; }

        /// <summary>負責人</summary>
        [DisplayName("負責人")]
        public string? ClubLeader { get; set; }

        /// <summary>負責人系級</summary>
        [DisplayName("負責人系級")]
        public string? Department { get; set; }

        /// <summary>Mail或Tel</summary>
        [DisplayName("Mail或Tel")]
        public string? MailOrTel { get; set; }
        
        /// <summary>社團分類	</summary>
        [DisplayName("社團分類")]
        public string? ClubClass { get; set; }

        /// <summary>社團組別	</summary>
        [DisplayName("社團組別")]
        public string? LifeClass { get; set; }

        /// <summary>學年度	</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ClubMangResultModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>中文名稱</summary>
        [DisplayName("中文名稱")]
        public string? ClubCName { get; set; }

        /// <summary>英文名稱</summary>
        [DisplayName("英文名稱")]
        public string? ClubEName { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團組別</summary>
        [DisplayName("社團組別")]
        public string? LifeClassText { get; set; }

        /// <summary>社團分類</summary>
        [DisplayName("社團分類")]
        public string? ClubClassText { get; set; }

        /// <summary>負責人姓名</summary>
        [DisplayName("負責人姓名")]
        public string? ClubLeader { get; set; }

        /// <summary>負責人系級</summary>
        [DisplayName("負責人系級")]
        public string? Department { get; set; }

        /// <summary>E-mail</summary>
        [DisplayName("E-mail")]
        public string? EMail { get; set; }

        /// <summary>聯絡電話</summary>
        [DisplayName("聯絡電話")]
        public string? Tel { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }
    }

    public class ClubMangCreateModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>社團中文名稱</summary>
        [DisplayName("社團中文名稱")]
        public string? ClubCName { get; set; }

        /// <summary>社團英文名稱</summary>
        [DisplayName("社團英文名稱")]
        public string? ClubEName { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團組別</summary>
        [DisplayName("社團組別")]
        public string? LifeClass { get; set; }

        /// <summary>社團分類</summary>
        [DisplayName("社團分類")]
        public string? ClubClass { get; set; }

        /// <summary>社辦地址</summary>
        [DisplayName("社辦地址")]
        public string? Address { get; set; }

        /// <summary>聯絡電話</summary>
        [DisplayName("聯絡電話")]
        public string? Tel { get; set; }

        /// <summary>E-mail</summary>
        [DisplayName("E-mail")]
        public string? EMail { get; set; }

        /// <summary>社群連結一	</summary>
        [DisplayName("社群連結一")]
        public string? Social1 { get; set; }

        /// <summary>社群連結二</summary>
        [DisplayName("社群連結二")]
        public string? Social2 { get; set; }

        /// <summary>社群連結三</summary>
        [DisplayName("社群連結三")]
        public string? Social3 { get; set; }

        /// <summary>Logo</summary>
        [DisplayName("Logo")]
        public string? LogoPath { get; set; }

        /// <summary>活動簡圖</summary>
        [DisplayName("活動簡圖")]
        public string? ActImgPath { get; set; }

        /// <summary>簡介</summary>
        [DisplayName("簡介")]
        public string? ShortInfo { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

    }

    public class ClubMangEditModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>社團中文名稱</summary>
        [DisplayName("社團中文名稱")]
        public string? ClubCName { get; set; }

        /// <summary>社團英文名稱</summary>
        [DisplayName("社團英文名稱")]
        public string? ClubEName { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>社團組別</summary>
        [DisplayName("社團組別")]
        public string? LifeClass { get; set; }

        /// <summary>社團分類</summary>
        [DisplayName("社團分類")]
        public string? ClubClass { get; set; }

        /// <summary>社辦地址</summary>
        [DisplayName("社辦地址")]
        public string? Address { get; set; }

        /// <summary>聯絡電話</summary>
        [DisplayName("聯絡電話")]
        public string? Tel { get; set; }

        /// <summary>E-mail</summary>
        [DisplayName("E-mail")]
        public string? EMail { get; set; }

        /// <summary>社群連結一	</summary>
        [DisplayName("社群連結一")]
        public string? Social1 { get; set; }

        /// <summary>社群連結二</summary>
        [DisplayName("社群連結二")]
        public string? Social2 { get; set; }

        /// <summary>社群連結三</summary>
        [DisplayName("社群連結三")]
        public string? Social3 { get; set; }

        /// <summary>Logo</summary>
        [DisplayName("Logo")]
        public string? LogoPath { get; set; }

        /// <summary>活動簡圖</summary>
        [DisplayName("活動簡圖")]
        public string? ActImgPath { get; set; }

        /// <summary>簡介</summary>
        [DisplayName("簡介")]
        public string? ShortInfo { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }
    }
}
