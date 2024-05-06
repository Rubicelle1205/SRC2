using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class EventCaseMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public EventCaseMangConditionModel ConditionModel { get; set; }

        public List<EventCaseMangResultModel> ResultModel { get; set; }

        public EventCaseMangCreateModel CreateModel { get; set; }

        public EventCaseMangEditModel EditModel { get; set; }

    }

    public class EventCaseMangConditionModel
    {
        public EventCaseMangConditionModel()
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

        /// <summary>主類別</summary>
        [DisplayName("主類別")]
        public string? MainClass { get; set; }

        /// <summary>次類別</summary>
        [DisplayName("次類別")]
        public string? SecondClass { get; set; }

        /// <summary>結案狀態</summary>
        [DisplayName("結案狀態")]
        public string? CaseStatus { get; set; }

        /// <summary>事件編號</summary>
        [DisplayName("事件編號")]
        public string? CaseID { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class EventCaseMangResultModel
    {
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

    public class EventCaseMangCreateModel
    {
        public List<Victim> LstVictim = new List<Victim>();

        public string? strLstVictim { get; set; }

        public List<EventData> LstEventData = new List<EventData>();

        /// <summary>事件編號</summary>
        [DisplayName("事件編號")]
        public string? CaseID { get; set; }

        /// <summary>校安事件主類別</summary>
        [DisplayName("校安事件主類別")]
        public string? MainClass { get; set; }

        /// <summary>校安事件次類別</summary>
        [DisplayName("校安事件次類別")]
        public string? SecondClass { get; set; }

        /// <summary>事件發生地點</summary>
        [DisplayName("事件發生地點")]
        public string? Location { get; set; }

        /// <summary>發生時間</summary>
        [DisplayName("發生時間")]
        public DateTime? OccurTime { get; set; }

        /// <summary>知悉時間</summary>
        [DisplayName("知悉時間")]
        public DateTime? KnowTime { get; set; }

        /// <summary>媒體是否得知</summary>
        [DisplayName("媒體是否得知")]
        public string? MediaKnow { get; set; }

        /// <summary>轉介單位</summary>
        [DisplayName("轉介單位")]
        public string? ReferCode { get; set; }

        /// <summary>死亡人數</summary>
        [DisplayName("死亡人數")]
        public string? DeathAmt { get; set; }

        /// <summary>受傷人數</summary>
        [DisplayName("受傷人數")]
        public string? HurtAmt { get; set; }

        /// <summary>患病人數</summary>
        [DisplayName("患病人數")]
        public string? SickAmt { get; set; }

        /// <summary>其他人數</summary>
        [DisplayName("其他人數")]
        public string? ElseAmt { get; set; }
        
        /// <summary>結案狀態</summary>
        [DisplayName("結案狀態")]
        public string? CaseStatus { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? CaseFinishDateTime { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }


        /// <summary>事件ID</summary>
        [DisplayName("事件ID")]
        public string? EventID { get; set; }

        /// <summary>事件時間</summary>
        [DisplayName("事件時間")]
        public string? EventDateTime { get; set; }

        /// <summary>事件說明</summary>
        [DisplayName("事件說明")]
        public string? EventText { get; set; }

    }

    public class EventCaseMangEditModel
    {
        public List<Victim> LstVictim = new List<Victim>();

        public List<EventData> LstEventData = new List<EventData>();

        /// <summary>事件編號</summary>
        [DisplayName("事件編號")]
        public string? CaseID { get; set; }

        /// <summary>校安事件主類別</summary>
        [DisplayName("校安事件主類別")]
        public string? MainClass { get; set; }

        /// <summary>校安事件次類別</summary>
        [DisplayName("校安事件次類別")]
        public string? SecondClass { get; set; }

        /// <summary>事件發生地點</summary>
        [DisplayName("事件發生地點")]
        public string? Location { get; set; }

        /// <summary>發生時間</summary>
        [DisplayName("發生時間")]
        public DateTime? OccurTime { get; set; }

        /// <summary>知悉時間</summary>
        [DisplayName("知悉時間")]
        public DateTime? KnowTime { get; set; }

        /// <summary>媒體是否得知</summary>
        [DisplayName("媒體是否得知")]
        public string? MediaKnow { get; set; }

        /// <summary>轉介單位</summary>
        [DisplayName("轉介單位")]
        public string? ReferCode { get; set; }

        /// <summary>結案狀態</summary>
        [DisplayName("結案狀態")]
        public string? CaseStatus { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? CaseFinishDateTime { get; set; }

        /// <summary>死亡人數</summary>
        [DisplayName("死亡人數")]
        public string? DeathAmt { get; set; }

        /// <summary>受傷人數</summary>
        [DisplayName("受傷人數")]
        public string? HurtAmt { get; set; }

        /// <summary>患病人數</summary>
        [DisplayName("患病人數")]
        public string? SickAmt { get; set; }

        /// <summary>其他人數</summary>
        [DisplayName("其他人數")]
        public string? ElseAmt { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        public string? LastModified { get; set; }
    }

    public class Victim
    {
        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? Sex { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>狀態</summary>
        [DisplayName("狀態")]
        public string? Status { get; set; }

        /// <summary>職稱</summary>
        [DisplayName("職稱")]
        public string? Title { get; set; }

        /// <summary>所屬單位</summary>
        [DisplayName("所屬單位")]
        public string? Unit { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>出生年</summary>
        [DisplayName("出生年")]
        public string? BirthYear { get; set; }

        /// <summary>目前位置</summary>
        [DisplayName("目前位置")]
        public string? Location { get; set; }

        /// <summary>角色</summary>
        [DisplayName("角色")]
        public string? Role { get; set; }
    }

    public class EventData
    {
        /// <summary>事件編號</summary>
        [DisplayName("事件編號")]
        public string? EventID { get; set; }

        /// <summary>事件時間</summary>
        [DisplayName("事件時間")]
        public DateTime? EventDateTime { get; set; }

        /// <summary>事件紀錄</summary>
        [DisplayName("事件紀錄")]
        public string? Text { get; set; }
    }
}
