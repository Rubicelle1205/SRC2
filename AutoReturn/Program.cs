using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReturn
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ConnectionString;

                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new Exception("沒有找到Connect String");
                }
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[啟動失敗] 在目錄中找不到 appsettings.json 檔案！");
                Console.WriteLine("提示：請確認該檔案存在，且屬性中的「複製到輸出目錄」已設定為「有更新時才複製」。");
                Console.ResetColor();
                Console.WriteLine("\n按下任意鍵關閉視窗...");
                Console.ReadKey();
                return;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[啟動失敗] 初始化設定時發生錯誤: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("\n按下任意鍵關閉視窗...");
                Console.ReadKey();
                return;
            }

            // 2. 參數解析與變數初始化
            string mode = "1";                  // 預設模式為 1 (預覽)
            DateTime targetDate = DateTime.Now; // 預設基準時間為系統現在時間
            bool shouldPause = false;           // 控制最後是否要卡住 Console 畫面

            if (args.Length > 0)
            {
                mode = args[0].Trim();
            }

            // 3. 核心路由邏輯
            if (mode == "2")
            {
                // 【正式執行更新模式】（排程專用，執行完直接結束不卡畫面）
                ExecuteReturnAction();
            }
            else if (mode == "1" || string.IsNullOrEmpty(mode))
            {
                // 【預覽 / 模擬模式】（手動手控，結束後會卡住畫面供檢查）
                shouldPause = true;

                // 檢查是否有帶入第二個參數（指定模擬日期）
                if (args.Length > 1)
                {
                    // 同時支援 YYYY-MM-DD 與 YYYY/MM/DD 格式
                    string[] allowedFormats = { "yyyy-MM-dd", "yyyy/MM/dd" };

                    if (DateTime.TryParseExact(args[1].Trim(),
                                               allowedFormats,
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out DateTime parsedDate))
                    {
                        // 貼心防呆：將時間調整為該日的 23:59:59，以便把當天到期的資料也一併模擬撈出
                        targetDate = parsedDate.Date.AddDays(1).AddSeconds(-1);
                    }
                    else
                    {
                        Console.WriteLine($"{{\"Error\": \"輸入的日期格式錯誤: '{args[1]}'。請使用 YYYY-MM-DD 或 YYYY/MM/DD 格式。\"}}");
                        PressAnyKeyToExit();
                        return;
                    }
                }

                // 執行預覽並輸出 JSON
                FetchPreviewJson(targetDate);
            }
            else
            {
                Console.WriteLine("不支援的參數。請輸入 1 (預覽資料) 或 2 (執行更新)。");
                shouldPause = true;
            }

            // 4. 畫面凍結機制
            if (shouldPause)
            {
                PressAnyKeyToExit();
            }
        }

        /// <summary>
        /// 停住 Console 畫面的輔助函式
        /// </summary>
        private static void PressAnyKeyToExit()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("執行完畢。請確認上方資料，按下任意鍵後關閉視窗...");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.ReadKey();
        }

        /// <summary>
        /// 模式 1：撈出即將更新的資料並轉為 JSON 輸出（支援模擬日期）
        /// </summary>
        private static void FetchPreviewJson(DateTime simulateDate)
        {
            // 使用 SQL Server 內建的 FOR JSON PATH 在資料庫端直接揉成 JSON
            string sqlQuery = @"SELECT 
    A.ID, B.TakeSDate, B.TakeEDate, B.ActVerify, A.MainResourceID, A.BorrowSecondResourceID, A.BorrowRealAmt, A.ReturnRealAmt
FROM BorrowDevice A
INNER JOIN BorrowMain B ON A.BorrowMainID = B.BorrowMainID 
INNER JOIN BorrowMainResourceMang C ON A.MainResourceID = C.MainResourceID
WHERE B.TakeEDate < @TargetDate
  AND C.IsAutoReturn = 1
  AND A.BorrowStatus = '02'
  AND A.ReturnRealAmt IS NULL

FOR JSON PATH";

            DataTable dt = RunSQL(sqlQuery, true, new SqlParameter("@TargetDate", simulateDate));

            string json = JsonConvert.SerializeObject(dt);
            Console.WriteLine(json);
        }

        /// <summary>
        /// 模式 2：正式更新資料庫（生產環境一律使用資料庫當下時間 GETDATE()，確保排程安全）
        /// </summary>
        private static void ExecuteReturnAction()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 自動歸還排程開始執行...");

            string sqlUpdate = @"
                -- 宣告 Table 變數，精準記錄本次受影響的主表 ID
                DECLARE @UpdatedMains TABLE (BorrowMainID INT);

                -- 1. 更新明細表 (BorrowDevice) 
                UPDATE A
                SET 
                    A.ReturnSecondResourceID = A.BorrowSecondResourceID,
                    A.ReturnRealAmt = A.BorrowRealAmt,
                    A.BorrowStatus = '01'
                OUTPUT inserted.BorrowMainID INTO @UpdatedMains
                FROM BorrowDevice A
                INNER JOIN BorrowMain B ON A.BorrowMainID = B.BorrowMainID 
                INNER JOIN BorrowMainResourceMang C ON A.MainResourceID = C.MainResourceID
                WHERE B.TakeEDate < GETDATE()
                  AND C.IsAutoReturn = 1
                  AND A.BorrowStatus = '02'
                  AND A.ReturnRealAmt IS NULL;

                -- 2. 同步將受影響的主表狀態改為 '05'
                UPDATE B
                SET B.ActVerify = '05'
                FROM BorrowMain B
                WHERE B.BorrowMainID IN (SELECT DISTINCT BorrowMainID FROM @UpdatedMains);
                
                -- 回傳總更新筆數
                SELECT @@ROWCOUNT;";

            RunSQL(sqlUpdate);

        }
        private static void RunSQL(string strSQL)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(strSQL, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }

        private static DataTable RunSQL(string strSQL, bool isQuery, params SqlParameter[] parameters)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ConnectionString;

            // 建立一個空的 DataTable 準備用來裝資料
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(strSQL, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    // 使用 SqlDataAdapter 來執行查詢並填入 DataTable
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
    }
}
