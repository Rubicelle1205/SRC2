using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class EventGenderMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public EventGenderMangConditionModel ConditionModel { get; set; }

        public List<EventGenderMangResultModel> ResultModel { get; set; }

        public EventGenderMangEditModel EditModel { get; set; }

        public EventGenderMangExcelModel ExcelModel { get; set; }

        public EventCaseReferDataMangConditionModel ReferDataConditionModel { get; set; }

        public List<EventCaseReferDataMangResultModel> ReferDataResultModel { get; set; }

    }

    public class EventGenderMangConditionModel
    {
        public EventGenderMangConditionModel()
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

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClass { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClass { get; set; }

        /// <summary>受理狀態</summary>
        [DisplayName("受理狀態")]
        public string? AcceptStatus { get; set; }

        /// <summary>結案狀態</summary>
        [DisplayName("結案狀態")]
        public string? CaseStatus { get; set; }

        /// <summary>校安事件編號</summary>
        [DisplayName("校安事件編號")]
        public string? CaseID { get; set; }

        /// <summary>性平號</summary>
        [DisplayName("性平號")]
        public string? SubCaseID { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class EventGenderMangResultModel
    {
        public string? EventID { get; set; }

        /// <summary>校安事件編號</summary>
        [DisplayName("校安事件編號")]
        public string? CaseID { get; set; }

        /// <summary>性平號</summary>
        [DisplayName("性平號")]
        public string? SubCaseID { get; set; }

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClass { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClass { get; set; }

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClassText { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClassText { get; set; }

        /// <summary>發生時間</summary>
        [DisplayName("發生時間")]
        public DateTime? OccurTime { get; set; }

        /// <summary>知悉時間</summary>
        [DisplayName("知悉時間")]
        public DateTime? KnowTime { get; set; }

        /// <summary>是否受理</summary>
        [DisplayName("是否受理")]
        public string? AcceptStatus { get; set; }

        /// <summary>是否受理</summary>
        [DisplayName("受理是否受理狀態")]
        public string? AcceptStatusText { get; set; }

        /// <summary>受理時間</summary>
        [DisplayName("受理時間")]
        public DateTime? AcceptTime { get; set; }
        
        /// <summary>是否結案</summary>
        [DisplayName("是否結案")]
        public string? CaseStatus { get; set; }

        /// <summary>是否結案</summary>
        [DisplayName("是否結案")]
        public string? CaseStatusText { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? CaseFinishDateTime { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class EventGenderMangImportModel
    {
        public string? EventID { get; set; }

        /// <summary>校安事件編號</summary>
        [DisplayName("校安事件編號")]
        public string? CaseID { get; set; }

        /// <summary>性平號</summary>
        [DisplayName("性平號")]
        public string? SubCaseID { get; set; }

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClass { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClass { get; set; }

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClassText { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClassText { get; set; }

        /// <summary>發生時間</summary>
        [DisplayName("發生時間")]
        public DateTime? OccurTime { get; set; }

        /// <summary>知悉時間</summary>
        [DisplayName("知悉時間")]
        public DateTime? KnowTime { get; set; }

        /// <summary>是否受理</summary>
        [DisplayName("是否受理")]
        public string? AcceptStatus { get; set; }

        /// <summary>是否受理</summary>
        [DisplayName("受理是否受理狀態")]
        public string? AcceptStatusText { get; set; }

        /// <summary>受理時間</summary>
        [DisplayName("受理時間")]
        public DateTime? AcceptTime { get; set; }

        /// <summary>是否結案</summary>
        [DisplayName("是否結案")]
        public string? CaseStatus { get; set; }

        /// <summary>是否結案</summary>
        [DisplayName("是否結案")]
        public string? CaseStatusText { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? CaseFinishDateTime { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class EventGenderMangEditModel
    {
        public List<Victim> LstVictim = new List<Victim>();

        public string? strLstVictim { get; set; }

        public List<EventData> LstEventData = new List<EventData>();

        public string? EventID { get; set; }

        /// <summary>事件編號</summary>
        [DisplayName("事件編號")]
        public string? CaseID { get; set; }

        /// <summary>校安事件主類別</summary>
        [DisplayName("校安事件主類別")]
        public string? MainClass { get; set; }

        /// <summary>校安事件次類別</summary>
        [DisplayName("校安事件次類別")]
        public string? SecondClass { get; set; }

        /// <summary>校安事件主類別</summary>
        [DisplayName("校安事件主類別")]
        public string? MainClassText { get; set; }

        /// <summary>校安事件次類別</summary>
        [DisplayName("校安事件次類別")]
        public string? SecondClassText { get; set; }

        /// <summary>發生時間</summary>
        [DisplayName("發生時間")]
        public DateTime? OccurTime { get; set; }

        /// <summary>知悉時間</summary>
        [DisplayName("知悉時間")]
        public DateTime? KnowTime { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        public string? LastModified { get; set; }



        /// <summary>性平號</summary>
        [DisplayName("性平號")]
        public string? SubCaseID { get; set; }

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClass { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClass { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? AcceptStatus { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? AcceptTime { get; set; }
        
        /// <summary>是否結案</summary>
        [DisplayName("是否結案")]
        public string? CaseStatus { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? CaseFinishDateTime { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }




        /// <summary>事件ID</summary>
        [DisplayName("事件ID")]
        public string? GenderEventID { get; set; }

        /// <summary>事件時間</summary>
        [DisplayName("事件時間")]
        public string? GenderEventDateTime { get; set; }

        /// <summary>事件說明</summary>
        [DisplayName("事件說明")]
        public string? GenderEventText { get; set; }
    }

    public class EventGenderMangExcelModel
    {
        /// <summary>校安事件編號</summary>
        [DisplayName("校安事件編號")]
        public string? CaseID { get; set; }

        /// <summary>性平號</summary>
        [DisplayName("性平號")]
        public string? SubCaseID { get; set; }

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClass { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClass { get; set; }

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClassText { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClassText { get; set; }

        /// <summary>發生時間</summary>
        [DisplayName("發生時間")]
        public DateTime? OccurTime { get; set; }

        /// <summary>知悉時間</summary>
        [DisplayName("知悉時間")]
        public DateTime? KnowTime { get; set; }

        /// <summary>受理狀態</summary>
        [DisplayName("受理狀態")]
        public string? AcceptStatus { get; set; }

        /// <summary>受理狀態</summary>
        [DisplayName("受理狀態")]
        public string? AcceptStatusText { get; set; }

        /// <summary>受理時間</summary>
        [DisplayName("受理時間")]
        public DateTime? AcceptTime { get; set; }

        /// <summary>是否結案</summary>
        [DisplayName("是否結案")]
        public string? CaseStatus { get; set; }

        /// <summary>是否結案</summary>
        [DisplayName("是否結案")]
        public string? CaseStatusText { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? CaseFinishDateTime { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class EventGenderMangExcelHeaderModel
    {
        /// <summary>校安事件編號</summary>
        [DisplayName("校安事件編號")]
        public string? CaseID { get; set; }

        /// <summary>性平號</summary>
        [DisplayName("性平號")]
        public string? SubCaseID { get; set; }

        /// <summary>發生時間</summary>
        [DisplayName("發生時間")]
        public DateTime? OccurTime { get; set; }

        /// <summary>知悉時間</summary>
        [DisplayName("知悉時間")]
        public DateTime? KnowTime { get; set; }

        /// <summary>性平事件主類別</summary>
        [DisplayName("性平事件主類別")]
        public string? GenderMainClass { get; set; }

        /// <summary>性平事件次類別</summary>
        [DisplayName("性平事件次類別")]
        public string? GenderSecondClass { get; set; }

        /// <summary>受理狀態</summary>
        [DisplayName("受理狀態")]
        public string? AcceptStatus { get; set; }

        /// <summary>受理時間</summary>
        [DisplayName("受理時間")]
        public DateTime? AcceptTime { get; set; }

        /// <summary>是否結案</summary>
        [DisplayName("是否結案")]
        public string? CaseStatus { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? CaseFinishDateTime { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

}
