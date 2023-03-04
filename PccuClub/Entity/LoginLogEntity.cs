namespace WebPccuClub.Entity
{
    public class LoginLogEntity
    {
        /// <summary> 使用者登入紀錄識別碼 </summary>
        public Int64 Id { get; set; }

        /// <summary> 使用者帳號 </summary>
        public String Loginid { get; set; }

        /// <summary> 登入時間 </summary>
        public DateTime Logintime { get; set; }

        /// <summary> 登入IP </summary>
        public String Ip { get; set; }

        /// <summary> 說明 </summary>
        public String Memo { get; set; }

        /// <summary> 登入結果 </summary>
        public Boolean Issuccess { get; set; }
    }
}
