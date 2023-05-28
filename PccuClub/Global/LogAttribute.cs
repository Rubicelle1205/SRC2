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
        角色權限設定 = 19,
        管理員維護 = 20,
        使用者維護 = 21,
        社團基本資料 = 22,
        校外建議場地 = 23,
        社團一覽 = 24,
        校內其他場地 = 25,
        校內場地 = 26,
        批次借用或關閉場地 = 27,
        執行批次借用或關閉場地 = 28,
        已報備活動 = 29,
        取得樓館選單 = 30,
        取得場地選單 = 31,
        取得場地資料 = 32,
        樓層維護 = 33,
        幹部名冊 = 34,
		會員及幹部登錄 = 35,
        前台幹部名冊 = 36,
		前台會員名冊 = 37,
		前台幹部名冊匯入 = 38,
		前台會員名冊匯入 = 39,
        下載template檔案 = 40,
	}

    public enum DBActionChineseName
    {
        成功 = 0,
        失敗 = 1,
    }
}
