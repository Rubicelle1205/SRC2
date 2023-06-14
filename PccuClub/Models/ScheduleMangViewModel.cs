using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class ScheduleMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ScheduleMangConditionModel ConditionModel { get; set; }

        public List<ScheduleMangResultModel> ResultModel { get; set; }

        public ScheduleMangCreateModel CreateModel { get; set; }

        public ScheduleMangEditModel EditModel { get; set; }

        public List<ScheduleMangExcelModel>  ExcelModel { get; set; }
    }

    public class ScheduleMangConditionModel
    {
        public ScheduleMangConditionModel()
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

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? CScheName { get; set; }

        /// <summary>舉辦狀態	</summary>
        [DisplayName("舉辦狀態")]
        public string? ActHoldType { get; set; }

        /// <summary>活動類別	</summary>
        [DisplayName("活動類別")]
        public string? ActTypeID { get; set; }

        /// <summary>學年度	</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ScheduleMangResultModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? CScheID { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>學年度	</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動類別	</summary>
        [DisplayName("活動類別")]
        public string? ActTypeID { get; set; }

        /// <summary>活動類別	</summary>
        [DisplayName("活動類別")]
        public string? ActTypeName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? CScheName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public DateTime? CScheDate { get; set; }

        /// <summary>經費預算	</summary>
        [DisplayName("經費預算")]
        public string? Budget { get; set; }

        /// <summary>舉辦狀態	</summary>
        [DisplayName("舉辦狀態")]
        public string? ActHoldType { get; set; }

        /// <summary>舉辦狀態	</summary>
        [DisplayName("舉辦狀態")]
        public string? ActHoldTypeText { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }
    }

    public class ScheduleMangCreateModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動類別	</summary>
        [DisplayName("活動類別")]
        public string? ActTypeID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? CScheName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public DateTime? CScheDate { get; set; }

        /// <summary>舉辦狀態	</summary>
        [DisplayName("舉辦狀態")]
        public string? ActHoldType { get; set; }

        /// <summary>預定場地</summary>
        [DisplayName("預定場地")]
        public string? BookingPlace { get; set; }

        /// <summary>經費預算</summary>
        [DisplayName("經費預算")]
        public string? Budget { get; set; }

        /// <summary>簡介</summary>
        [DisplayName("簡介")]
        public string? ShortDesc { get; set; }

        /// <summary>接受那些經費補助</summary>
        [DisplayName("接受那些經費補助")]
        public string? Support { get; set; }

        /// <summary>參與人數</summary>
        [DisplayName("參與人數")]
        public string? Participants { get; set; }

        /// <summary>滿意度</summary>
        [DisplayName("滿意度")]
        public string? Satisfaction { get; set; }

        /// <summary>附件檔</summary>
        [DisplayName("附件檔")]
        public string? Attachment { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

    }

    public class ScheduleMangEditModel
    {
        /// <summary>ID</summary>
        [DisplayName("ID")]
        public string? CScheID { get; set; }

        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動類別	</summary>
        [DisplayName("活動類別")]
        public string? ActTypeID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? CScheName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public string? CScheDate { get; set; }

        /// <summary>舉辦狀態	</summary>
        [DisplayName("舉辦狀態")]
        public string? ActHoldType { get; set; }

        /// <summary>預定場地</summary>
        [DisplayName("預定場地")]
        public string? BookingPlace { get; set; }

        /// <summary>經費預算</summary>
        [DisplayName("經費預算")]
        public string? Budget { get; set; }

        /// <summary>簡介</summary>
        [DisplayName("簡介")]
        public string? ShortDesc { get; set; }

        /// <summary>經費補助</summary>
        [DisplayName("經費補助")]
        public string? Support { get; set; }

        /// <summary>參與人數</summary>
        [DisplayName("參與人數")]
        public string? Participants { get; set; }

        /// <summary>滿意度</summary>
        [DisplayName("滿意度")]
        public string? Satisfaction { get; set; }

        /// <summary>附件檔</summary>
        [DisplayName("附件檔")]
        public string? Attachment { get; set; }

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

    public class ScheduleMangExcelModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>學年度	</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動類別	</summary>
        [DisplayName("活動類別")]
        public string? ActTypeName { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? CScheName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public DateTime? CScheDate { get; set; }

        /// <summary>舉辦狀態	</summary>
        [DisplayName("舉辦狀態")]
        public string? ActHoldTypeText { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }
    }

    public class ScheduleMangImportExcelModel
    {
        /// <summary>社團代號</summary>
        [DisplayName("社團代號")]
        public string? ClubId { get; set; }

        /// <summary>學年度</summary>
        [DisplayName("學年度")]
        public string? SchoolYear { get; set; }

        /// <summary>活動類別	</summary>
        [DisplayName("活動類別")]
        public string? ActTypeID { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? CScheName { get; set; }

        /// <summary>活動日期</summary>
        [DisplayName("活動日期")]
        public string? CScheDate { get; set; }

        /// <summary>舉辦狀態	</summary>
        [DisplayName("舉辦狀態")]
        public string? ActHoldType { get; set; }

        /// <summary>預定場地</summary>
        [DisplayName("預定場地")]
        public string? BookingPlace { get; set; }

        /// <summary>經費預算</summary>
        [DisplayName("經費預算")]
        public string? Budget { get; set; }

        /// <summary>簡介</summary>
        [DisplayName("簡介")]
        public string? ShortDesc { get; set; }
    }
}
