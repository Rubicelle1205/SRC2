using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class FBorrowRecordViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public FBorrowRecordConditionModel ConditionModel { get; set; }

        public List<FBorrowRecordResultModel> ResultModel { get; set; }

        public FBorrowRecordCreateModel CreateModel { get; set; }


        public class FBorrowRecordConditionModel
        {
            public FBorrowRecordConditionModel()
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
        }

        public class FBorrowRecordResultModel
        {
            /// <summary>申請單號</summary>
            [DisplayName("申請單號")]
            public string? BorrowMainID { get; set; }

            /// <summary>審核狀態</summary>
            [DisplayName("審核狀態")]
            public string? ActVerify { get; set; }

            /// <summary>審核狀態</summary>
            [DisplayName("審核狀態")]
            public string? ActVerifyText { get; set; }

            [DisplayName("建立時間")]
            public DateTime? Created { get; set; }
        }

        public class FBorrowRecordCreateModel
        {
            public List<FBorrowRecordFileModel> LstFile = new List<FBorrowRecordFileModel>();
            public List<FBorrowRecordDeviceModel> LstDevice = new List<FBorrowRecordDeviceModel>();
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

            [DisplayName("約定領取時間")]
            public DateTime? TakeSDate { get; set; }

            [DisplayName("約定領取時間")]
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

        public class FBorrowRecordFileModel
        {
            public string? FileName { get; set; }

            public string? FilePath { get; set; }
        }

        public class FBorrowRecordDeviceModel
        {
            public string? BorrowMainID { get; set; }

            public string? MainResourceID { get; set; }

            public string? SecondResourceNo { get; set; }

            public string? BorrowStatus { get; set; }

            public string? BorrowAmt { get; set; }
        }

    }
}
