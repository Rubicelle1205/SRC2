using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ConsultationCaseMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ConsultationCaseMangConditionModel ConditionModel { get; set; }

        public List<ConsultationCaseMangResultModel> ResultModel { get; set; }

        public ConsultationCaseMangCreateModel CreateModel { get; set; }

        public ConsultationCaseMangEditModel EditModel { get; set; }

        public List<ConsultationCaseMangExcelModel> ExcelModel { get; set; }
    }

    public class ConsultationCaseMangConditionModel
    {
        public ConsultationCaseMangConditionModel()
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

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? Psychologist { get; set; }

        /// <summary>諮商空間</summary>
        [DisplayName("諮商空間")]
        public string? RoomID { get; set; }

        /// <summary>派案狀態</summary>
        [DisplayName("派案狀態")]
        public string? AssignCaseStatus { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ConsultationCaseMangResultModel
    {
        public string? ID { get; set; }

        /// <summary>諮商日期</summary>
        [DisplayName("諮商日期")]
        public string? TalkDate { get; set; }

        /// <summary>諮商開始時間</summary>
        [DisplayName("諮商開始時間")]
        public string? TalkSTime { get; set; }

        /// <summary>諮商結束時間</summary>
        [DisplayName("諮商結束時間")]
        public string? TalkETime { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>派案人</summary>
        [DisplayName("派案人")]
        public string? AssignCaseMan { get; set; }

        /// <summary>派案人</summary>
        [DisplayName("派案人")]
        public string? AssignCaseManText { get; set; }

        /// <summary>派案時間</summary>
        [DisplayName("派案時間")]
        public DateTime? AssignCaseTime { get; set; }

        /// <summary>派案狀態</summary>
        [DisplayName("派案狀態")]
        public string? AssignCaseStatus { get; set; }

        /// <summary>派案狀態</summary>
        [DisplayName("派案狀態")]
        public string? AssignCaseStatusText { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? FinishCaseTime { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? Psychologist { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? PsychologistText { get; set; }

        /// <summary>使用空間</summary>
        [DisplayName("使用空間")]
        public string? RoomID { get; set; }

        /// <summary>使用空間</summary>
        [DisplayName("使用空間")]
        public string? RoomIDText { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

    }

    public class ConsultationCaseMangCreateModel
    {
        /// <summary>諮商日期</summary>
        [DisplayName("諮商日期")]
        public string? TalkDate { get; set; }

        /// <summary>諮商開始時間</summary>
        [DisplayName("諮商開始時間")]
        public string? TalkSTime { get; set; }

        /// <summary>諮商結束時間</summary>
        [DisplayName("諮商結束時間")]
        public string? TalkETime { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? Psychologist { get; set; }

        /// <summary>使用空間</summary>
        [DisplayName("使用空間")]
        public string? RoomID { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class ConsultationCaseMangEditModel
    {
        public string? ID { get; set; }

        /// <summary>諮商日期</summary>
        [DisplayName("諮商日期")]
        public string? TalkDate { get; set; }

        /// <summary>諮商開始時間</summary>
        [DisplayName("諮商開始時間")]
        public string? TalkSTime { get; set; }

        /// <summary>諮商結束時間</summary>
        [DisplayName("諮商結束時間")]
        public string? TalkETime { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>派案人</summary>
        [DisplayName("派案人")]
        public string? AssignCaseMan { get; set; }

        /// <summary>派案時間</summary>
        [DisplayName("派案時間")]
        public DateTime? AssignCaseTime { get; set; }

        /// <summary>派案狀態</summary>
        [DisplayName("派案狀態")]
        public string? AssignCaseStatus { get; set; }

        /// <summary>結案時間</summary>
        [DisplayName("結案時間")]
        public DateTime? FinishCaseTime { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? Psychologist { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? PsychologistText { get; set; }

        /// <summary>使用空間</summary>
        [DisplayName("使用空間")]
        public string? RoomID { get; set; }

        /// <summary>使用空間</summary>
        [DisplayName("使用空間")]
        public string? RoomIDText { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        public DateTime? LastModified { get; set; }
    }


    public class ConsultationCaseMangExcelModel
    {
        public string? ID { get; set; }

        /// <summary>諮商日期</summary>
        [DisplayName("諮商日期")]
        public string? TalkDate { get; set; }

        /// <summary>諮商開始時間</summary>
        [DisplayName("諮商開始時間")]
        public string? TalkSTime { get; set; }

        /// <summary>諮商結束時間</summary>
        [DisplayName("諮商結束時間")]
        public string? TalkETime { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>派案人</summary>
        [DisplayName("派案人")]
        public string? AssignCaseMan { get; set; }

        /// <summary>派案人</summary>
        [DisplayName("派案人")]
        public string? AssignCaseManText { get; set; }

        /// <summary>派案時間</summary>
        [DisplayName("派案時間")]
        public DateTime? AssignCaseTime { get; set; }

        /// <summary>派案狀態</summary>
        [DisplayName("派案狀態")]
        public string? AssignCaseStatus { get; set; }

        /// <summary>派案狀態</summary>
        [DisplayName("派案狀態")]
        public string? AssignCaseStatusText { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? Psychologist { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? PsychologistText { get; set; }

        /// <summary>使用空間</summary>
        [DisplayName("使用空間")]
        public string? RoomID { get; set; }

        /// <summary>使用空間</summary>
        [DisplayName("使用空間")]
        public string? RoomIDText { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

    }

    public class ConsultationCaseMangExcelHeaderModel
    {
        /// <summary>諮商時間</summary>
        [DisplayName("諮商時間")]
        public string? TalkDate { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>派案人</summary>
        [DisplayName("派案人")]
        public string? AssignCaseManText { get; set; }

        /// <summary>派案時間</summary>
        [DisplayName("派案時間")]
        public DateTime? AssignCaseTime { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? PsychologistText { get; set; }

        /// <summary>派案狀態</summary>
        [DisplayName("派案狀態")]
        public string? AssignCaseStatus { get; set; }

        /// <summary>使用空間</summary>
        [DisplayName("使用空間")]
        public string? RoomIDText { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

    }
}
