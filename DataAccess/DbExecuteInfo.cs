using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DbExecuteInfo
    {
        /// <summary> 是否執行成功 </summary>
        public int AffectRowCount { get; set; }

        /// <summary> 是否執行成功 </summary>
        public bool isSuccess { get; set; }

        /// <summary> 執行結果資訊 </summary>
        public Exception SystemResult { get; set; }

        /// <summary> 錯誤代碼 </summary>
        public dbErrorCode ErrorCode { get; set; }

        /// <summary> 錯誤訊息 </summary>
        public string ErrorMessage
        {
            get
            {
                return GetErrorMessage(ErrorCode);
            }
        }

        /// <summary>
        /// 取得錯誤訊息
        /// </summary>
        /// <param name="ErrCode">錯誤代碼</param>
        /// <returns>錯誤訊息</returns>
        private string GetErrorMessage(dbErrorCode ErrCode)
        {
            string ErrMessage = "";

            switch (ErrCode)
            {
                case dbErrorCode._EC_NotMatchData:
                    ErrMessage = "查無符合資料";
                    break;
                case dbErrorCode._EC_UniqueViolation:
                    ErrMessage = "違反資料唯一性原則";
                    break;
                case dbErrorCode._EC_NotAffect:
                    ErrMessage = "無資料異動";
                    break;
                default:
                    ErrMessage = SystemResult != null ? SystemResult.Message : string.Empty;
                    break;
            }

            return ErrMessage;
        }
    }

}
