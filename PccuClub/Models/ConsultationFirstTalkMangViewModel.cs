using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class ConsultationFirstTalkMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ConsultationFirstTalkMangConditionModel ConditionModel { get; set; }

        public List<ConsultationFirstTalkMangResultModel> ResultModel { get; set; }

        public ConsultationFirstTalkMangEditModel EditModel { get; set; }

        public List<ConsultationFirstTalkMangExcelModel> ExcelModel { get; set; }
    }

    public class ConsultationFirstTalkMangConditionModel
    {
        public ConsultationFirstTalkMangConditionModel()
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

        /// <summary>是否完成初談</summary>
        [DisplayName("是否完成初談")]
        public string? FirstTalkStatus { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ConsultationFirstTalkMangResultModel
    {
        public string? ID { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }
        
        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? Sex { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? SexText { get; set; }

        /// <summary>過去2週曾出現這些想法或計劃</summary>
        [DisplayName("過去2週曾出現這些想法或計劃")]
        public string? CounsellingStatus { get; set; }

        /// <summary>過去2週曾出現這些想法或計劃</summary>
        [DisplayName("過去2週曾出現這些想法或計劃")]
        public string? CounsellingStatusText { get; set; }

        /// <summary>接案同仁</summary>
        [DisplayName("接案同仁")]
        public string? AssignCaseMan { get; set; }

        /// <summary>接案同仁</summary>
        [DisplayName("接案同仁")]
        public string? AssignCaseManText { get; set; }

        /// <summary>派案時間</summary>
        [DisplayName("派案時間")]
        public DateTime? AssignCaseTime { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? Psychologist { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? PsychologistText { get; set; }

        /// <summary>是否完成初談</summary>
        [DisplayName("是否完成初談")]
        public string? FirstTalkStatus { get; set; }

        /// <summary>是否完成初談</summary>
        [DisplayName("是否完成初談")]
        public string? FirstTalkStatusText { get; set; }

        /// <summary>完成初談時間</summary>
        [DisplayName("完成初談時間")]
        public DateTime? FirstTalkTime { get; set; }

        /// <summary>填表時間</summary>
        [DisplayName("填表時間")]
        public DateTime? Created { get; set; }

    }

    public class ConsultationFirstTalkMangEditModel
    {
        public string? ID { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>學號</summary>
        [DisplayName("學號")]
        public string? SNO { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? Sex { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? SexText { get; set; }

        /// <summary>電話</summary>
        [DisplayName("電話")]
        public string? Tel { get; set; }

        /// <summary>國籍</summary>
        [DisplayName("國籍")]
        public string? Citizenship { get; set; }

        /// <summary>國籍</summary>
        [DisplayName("國籍")]
        public string? CitizenshipText { get; set; }

        /// <summary>國家名稱</summary>
        [DisplayName("國家名稱")]
        public string? CitizenshipName { get; set; }

        /// <summary>過去2週曾出現這些想法或計劃</summary>
        [DisplayName("過去2週曾出現這些想法或計劃")]
        public string? CounsellingStatus { get; set; }

        /// <summary>過去2週曾出現這些想法或計劃</summary>
        [DisplayName("過去2週曾出現這些想法或計劃")]
        public string? CounsellingStatusText { get; set; }

        /// <summary>接案同仁</summary>
        [DisplayName("接案同仁")]
        public string? AssignCaseMan { get; set; }

        /// <summary>接案同仁</summary>
        [DisplayName("接案同仁")]
        public string? AssignCaseManText { get; set; }

        /// <summary>派案時間</summary>
        [DisplayName("派案時間")]
        public DateTime? AssignCaseTime { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? Psychologist { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? PsychologistText { get; set; }

        /// <summary>是否完成初談</summary>
        [DisplayName("是否完成初談")]
        public string? FirstTalkStatus { get; set; }

        /// <summary>是否完成初談</summary>
        [DisplayName("是否完成初談")]
        public string? FirstTalkStatusText { get; set; }

        /// <summary>完成初談時間</summary>
        [DisplayName("完成初談時間")]
        public DateTime? FirstTalkTime { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>填表時間</summary>
        [DisplayName("填表時間")]
        public DateTime? Created { get; set; }

        /// <summary>修改時間</summary>
        [DisplayName("修改時間")]
        public DateTime? LastModified { get; set; }

        public string? strAppointmentTime { get; set; }

        public List<AppointmentTimeModel> LstAppointmentTimeModel = new List<AppointmentTimeModel>();
    }

    public class ConsultationFirstTalkMangExcelModel
    {
        public string? ID { get; set; }

        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? Sex { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? SexText { get; set; }

        /// <summary>過去2週曾出現這些想法或計劃</summary>
        [DisplayName("過去2週曾出現這些想法或計劃")]
        public string? CounsellingStatus { get; set; }

        /// <summary>過去2週曾出現這些想法或計劃</summary>
        [DisplayName("過去2週曾出現這些想法或計劃")]
        public string? CounsellingStatusText { get; set; }

        /// <summary>接案同仁</summary>
        [DisplayName("接案同仁")]
        public string? AssignCaseMan { get; set; }

        /// <summary>接案同仁</summary>
        [DisplayName("接案同仁")]
        public string? AssignCaseManText { get; set; }

        /// <summary>派案時間</summary>
        [DisplayName("派案時間")]
        public DateTime? AssignCaseTime { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? Psychologist { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? PsychologistText { get; set; }

        /// <summary>是否完成初談</summary>
        [DisplayName("是否完成初談")]
        public string? FirstTalkStatus { get; set; }

        /// <summary>是否完成初談</summary>
        [DisplayName("是否完成初談")]
        public string? FirstTalkStatusText { get; set; }

        /// <summary>完成初談時間</summary>
        [DisplayName("完成初談時間")]
        public DateTime? FirstTalkTime { get; set; }

        /// <summary>填表時間</summary>
        [DisplayName("填表時間")]
        public DateTime? Created { get; set; }

    }

    public class ConsultationFirstTalkMangExcelHeaderModel
    {
        /// <summary>姓名</summary>
        [DisplayName("姓名")]
        public string? Name { get; set; }

        /// <summary>系級</summary>
        [DisplayName("系級")]
        public string? Department { get; set; }

        /// <summary>性別</summary>
        [DisplayName("性別")]
        public string? SexText { get; set; }

        /// <summary>過去2週曾出現這些想法或計劃</summary>
        [DisplayName("過去2週曾出現這些想法或計劃")]
        public string? CounsellingStatusText { get; set; }

        /// <summary>接案同仁</summary>
        [DisplayName("接案同仁")]
        public string? AssignCaseManText { get; set; }

        /// <summary>派案時間</summary>
        [DisplayName("派案時間")]
        public DateTime? AssignCaseTime { get; set; }

        /// <summary>負責心理師</summary>
        [DisplayName("負責心理師")]
        public string? PsychologistText { get; set; }

        /// <summary>是否完成初談</summary>
        [DisplayName("是否完成初談")]
        public string? FirstTalkStatusText { get; set; }

        /// <summary>完成初談時間</summary>
        [DisplayName("完成初談時間")]
        public DateTime? FirstTalkTime { get; set; }

        /// <summary>填表時間</summary>
        [DisplayName("填表時間")]
        public DateTime? Created { get; set; }

    }
}
