namespace WebPccuClub.Global
{
    public class LogAttribute : Attribute
    {
        public LogAttribute(LogActionChineseName logDisplayName)
        {
            LogDisplayName = logDisplayName.ToString();
        }

        public string? LogDisplayName { get; set; }
    }

    public enum LogActionChineseName
    {
        首頁 = 0,
        查詢 = 1,
        匯入 = 2,
        匯出 = 3,
        新增 = 4,
        新增儲存 = 5,
        編輯 = 6,
        編輯儲存 = 7,
        刪除 = 9,
        場地同意書 = 10,
        個人資料 = 11,
        SDGs維護 = 12,
        匯入Excel = 13,
        匯出Excel = 14,
        校內樓館 = 15,
        提醒內容 = 16,
        期限日期 = 17,
        活動性質項目 = 18,
        管理員及權限設定 = 19,
    }

    public enum DBActionChineseName
    {
        成功 = 0,
        失敗 = 1,
    }
}
