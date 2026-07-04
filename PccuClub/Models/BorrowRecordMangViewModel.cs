using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class BorrowRecordMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public BorrowRecordMangConditionModel ConditionModel { get; set; }

        public List<BorrowRecordMangResultModel> ResultModel { get; set; }

        public BorrowRecordMangCreateModel CreateModel { get; set; }

        public BorrowRecordMangEditModel EditModel { get; set; }

        public List<BorrowRecordMangExcelResultModel> ExcelModel { get; set; }

        public BorrowRecordMangddlModel ddlModel {get; set;}
    }

    public class BorrowRecordMangConditionModel
    {
        public BorrowRecordMangConditionModel()
        {
            this.Page = 0;
            this.PageSize = 10;
            this.TotalCount = 0;
        }

        public List<ColumnDataModel> LstColumnDataModel = new List<ColumnDataModel>();

        /// <summary> 選擇欄位 </summary>
        public string SelectedColumns { get; set; }

        public string SafeSqlColumns { get; set; }

        /// <summary> 目前頁數 </summary>
        public int Page { get; set; }

        /// <summary> 預設每頁顯示筆數 - 依需求更改 </summary>
        public int PageSize { get; set; }

        /// <summary> 總筆數 </summary>
        public int TotalCount { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClassID { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>申請單位(社團)</summary>
        [DisplayName("申請單位(社團)")]
        public string? ApplyUnitName { get; set; }

        /// <summary>申請人</summary>
        [DisplayName("申請人")]
        public string? ApplyMan { get; set; }

        /// <summary>申請人Email</summary>
        [DisplayName("申請人Email")]
        public string? ApplyEmail { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class BorrowRecordMangResultModel
    {
        /// <summary>申請單號</summary>
        [DisplayName("申請單號")]
        public string? BorrowMainID { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClassIDText { get; set; }

        /// <summary>申請單位</summary>
        [DisplayName("申請單位")]
        public string? ApplyUnitName { get; set; }

        /// <summary>申請人</summary>
        [DisplayName("申請人")]
        public string? ApplyMan { get; set; }

        /// <summary>申請人職稱</summary>
        [DisplayName("申請人職稱")]
        public string? ApplyTitle { get; set; }

        /// <summary>申請人Email</summary>
        [DisplayName("申請人Email")]
        public string? ApplyEmail { get; set; }

        /// <summary>申請人電話/分機</summary>
        [DisplayName("申請人電話/分機")]
        public string? ApplyTel { get; set; }

        /// <summary>申請目的</summary>
        [DisplayName("申請目的")]
        public string? ApplyPurpose { get; set; }

        /// <summary>活動名稱</summary>
        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        /// <summary>使用地點</summary>
        [DisplayName("使用地點")]
        public string? UseLocation { get; set; }

        /// <summary>用途及特殊需求說明</summary>
        [DisplayName("用途及特殊需求說明")]
        public string? UseDesc { get; set; }

        /// <summary>實際使用起日</summary>
        [DisplayName("實際使用起日")]
        public string? UseSDate { get; set; }

        /// <summary>實際使用訖日</summary>
        [DisplayName("實際使用訖日")]
        public string? UseEDate { get; set; }

        /// <summary>約定領取起日</summary>
        [DisplayName("約定領取起日")]
        public string? TakeSDate { get; set; }

        /// <summary>約定領取訖日</summary>
        [DisplayName("約定領取訖日")]
        public string? TakeEDate { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? BorrowMemo { get; set; }

        /// <summary>輔導老師或承辦人註記</summary>
        [DisplayName("輔導老師或承辦人註記")]
        public string? TeacherMark { get; set; }

        /// <summary>器材專業人員註記</summary>
        [DisplayName("器材專業人員註記")]
        public string? DeviceMark { get; set; }

        /// <summary>器材領取註記</summary>
        [DisplayName("器材領取註記")]
        public string? TakeMark { get; set; }

        /// <summary>器材歸還註記</summary>
        [DisplayName("器材歸還註記")]
        public string? ReturnMark { get; set; }

        /// <summary>其它備註</summary>
        [DisplayName("其它備註")]
        public string? Memo { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class BorrowRecordMangExcelResultModel
    {
        /// <summary>申請單號</summary>
        [DisplayName("申請單號")]
        public string? BorrowMainID { get; set; }

        /// <summary>申請單位(社團)</summary>
        [DisplayName("申請單位(社團)")]
        public string? ApplyUnitName { get; set; }

        /// <summary>申請人</summary>
        [DisplayName("申請人")]
        public string? ApplyMan { get; set; }

        /// <summary>職稱</summary>
        [DisplayName("職稱")]
        public string? ApplyTitle { get; set; }

        /// <summary>領取日期</summary>
        [DisplayName("領取日期")]
        public DateTime? TakeSDate { get; set; }

        /// <summary>歸還日期</summary>
        [DisplayName("歸還日期")]
        public DateTime? TakeEDate { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClassID { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClassIDText { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }
    }

    public class BorrowRecordMangExcelHeaderModel
    {
        /// <summary>申請單號</summary>
        [DisplayName("申請單號")]
        public string? BorrowMainID { get; set; }

        /// <summary>申請單位(社團)</summary>
        [DisplayName("申請單位(社團)")]
        public string? ApplyUnitName { get; set; }

        /// <summary>申請人</summary>
        [DisplayName("申請人")]
        public string? ApplyMan { get; set; }

        /// <summary>職稱</summary>
        [DisplayName("職稱")]
        public string? ApplyTitle { get; set; }

        /// <summary>領取日期</summary>
        [DisplayName("領取日期")]
        public DateTime? TakeSDate { get; set; }

        /// <summary>歸還日期</summary>
        [DisplayName("歸還日期")]
        public DateTime? TakeEDate { get; set; }

        /// <summary>業務分類</summary>
        [DisplayName("業務分類")]
        public string? MainClassIDText { get; set; }

        /// <summary>審核狀態</summary>
        [DisplayName("審核狀態")]
        public string? ActVerifyText { get; set; }

        [DisplayName("填表時間")]
        public DateTime? Created { get; set; }
    }

    public class BorrowRecordMangCreateModel
    {
        public List<BorrowRecordMangFileModel> LstFile = new List<BorrowRecordMangFileModel>();
        public List<BorrowRecordMangDeviceModel> LstDevice = new List<BorrowRecordMangDeviceModel>();
        public string? strDeviceData { get; set; }
        public string? MainResourceID { get; set; }

        public string? AmtShelves { get; set; }

        public string? AmtOnce { get; set; }

        public string? BorrowAmt { get; set; }

        [DisplayName("業務分類")]
        public string? MainClassID { get; set; }

        [DisplayName("申請單位類型")]
        public string? ApplyUnitType { get; set; }

        [DisplayName("申請單位")]
        public string? ApplyUnitName { get; set; }

        [DisplayName("申請人")]
        public string? ApplyMan { get; set; }

        [DisplayName("申請人職稱")]
        public string? ApplyTitle { get; set; }

        [DisplayName("申請人Email")]
        public string? ApplyEmail { get; set; }

        [DisplayName("申請人電話/分機")]
        public string? ApplyTel { get; set; }

        [DisplayName("申請目的")]
        public string? ApplyPurpose { get; set; }

        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        [DisplayName("使用地點")]
        public string? UseLocation { get; set; }

        [DisplayName("用途及特殊需求說明")]
        public string? UseDesc { get; set; }

        [DisplayName("實際使用起日")]
        public DateTime? UseSDate { get; set; }

        [DisplayName("實際使用訖日")]
        public DateTime? UseEDate { get; set; }

        [DisplayName("約定領取日期")]
        public DateTime? TakeSDate { get; set; }

        [DisplayName("約定領取日期")]
        public DateTime? TakeEDate { get; set; }

        [DisplayName("借用備註")]
        public string? BorrowMemo { get; set; }

        [DisplayName("輔導老師或承辦人註記")]
        public string? TeacherMark { get; set; }

        [DisplayName("器材專業人員註記")]
        public string? DeviceMark { get; set; }

        [DisplayName("器材領取註記")]
        public string? TakeMark { get; set; }

        [DisplayName("器材歸還註記")]
        public string? ReturnMark { get; set; }

        [DisplayName("備註")] 
        public string? Memo { get; set; }

        [DisplayName("審核狀態")] 
        public string? ActVerify { get; set; }
    }

    public class BorrowRecordMangEditModel
    {
        public List<BorrowRecordMangFileModel> LstFile = new List<BorrowRecordMangFileModel>();
        public List<BorrowRecordMangDeviceModel> LstDevice = new List<BorrowRecordMangDeviceModel>();
        public List<EventData> LstEventData = new List<EventData>();

        public string? BorrowMainID {get; set;}

        [DisplayName("業務分類")]
        public string? MainClassID { get; set; }

        [DisplayName("業務分類")]
        public string? MainClassIDText { get; set; }

        [DisplayName("申請單位類型")]
        public string? ApplyUnitType { get; set; }

        [DisplayName("申請單位")]
        public string? ApplyUnitName { get; set; }

        [DisplayName("申請人")]
        public string? ApplyMan { get; set; }

        [DisplayName("申請人職稱")]
        public string? ApplyTitle { get; set; }

        [DisplayName("申請人Email")]
        public string? ApplyEmail { get; set; }

        [DisplayName("申請人電話/分機")]
        public string? ApplyTel { get; set; }

        [DisplayName("申請目的")]
        public string? ApplyPurpose { get; set; }

        [DisplayName("活動名稱")]
        public string? ActName { get; set; }

        [DisplayName("使用地點")]
        public string? UseLocation { get; set; }

        [DisplayName("用途及特殊需求說明")]
        public string? UseDesc { get; set; }

        [DisplayName("實際使用起日")]
        public string? UseSDate { get; set; }

        [DisplayName("實際使用訖日")]
        public string? UseEDate { get; set; }

        [DisplayName("約定領取日期")]
        public string? TakeSDate { get; set; }

        [DisplayName("約定領取日期")]
        public string? TakeEDate { get; set; }

        [DisplayName("借用備註")]
        public string? BorrowMemo { get; set; }

        [DisplayName("輔導老師或承辦人註記")]
        public string? TeacherMark { get; set; }

        [DisplayName("器材專業人員註記")]
        public string? DeviceMark { get; set; }

        [DisplayName("器材領取註記")]
        public string? TakeMark { get; set; }

        [DisplayName("器材歸還註記")]
        public string? ReturnMark { get; set; }

        [DisplayName("備註")]
        public string? Memo { get; set; }

        [DisplayName("審核狀態")]
        public string? ActVerify { get; set; }



        [DisplayName("建立時間")]
        public DateTime? Created { get; set; }

        [DisplayName("最後修改時間")]
        public DateTime? LastModified { get; set; }



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

    public class BorrowRecordMangFileModel
    {
        public string? FileName { get; set; }

        public string? FilePath { get; set; }
    }

    public class BorrowRecordMangDeviceModel
    {
        public string? ID { get; set; }

        public string? BorrowMainID { get; set; }

        public string? MainClassID { get; set; }

        public string? MainResourceID { get; set; }

        public string? MainResourceIDText { get; set; }

        public string? BorrowStatus { get; set; }

        public string? BorrowAmt { get; set; }

        /// <summary>大量借用</summary>
        [DisplayName("大量借用")]
        public string? BorrowType { get; set; }

        /// <summary>大量借用</summary>
        [DisplayName("大量借用")]
        public string? BorrowTypeText { get; set; }

        public string? BorrowStatusText { get; set; }

        public string? BorrowSecondResourceID { get; set; }

        public string? BorrowRealAmt { get; set; }

        public string? ReturnRealAmt { get; set; }

        public string? AmtShelves { get; set; }

        public string? AmtSafe { get; set; }

        public string? SafeMessage { get; set; }
    }

    public class BorrowRecordMangddlModel
    { 
        public string? ID { get; set; }
    }

}
