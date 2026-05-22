using System;
using System.IO;
using System.Data;
using System.Text;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration; // 💡 請確認已安裝 Microsoft.Extensions.Configuration.Json NuGet 套件

namespace AutoReturnApp
{
    class Program
    {
        // 讀取自 appsettings.json 的全域連線字串變數
        private static string ConnectionString;

        static void Main(string[] args)
        {
            // 1. 初始化環境：從同目錄下的 appsettings.json 讀取資料庫連線設定
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                var builder = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                IConfiguration config = builder.Build();

                // 對應 JSON 結構: ConnectionString -> DefaultDatabase
                ConnectionString = config.GetSection("ConnectionString")?["DefaultDatabase"];

                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new Exception("在 appsettings.json 中找不到 [ConnectionString] -> [DefaultDatabase] 的設定值！");
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
            string sqlQuery = @"
                SELECT 
                    bm.Id AS BorrowMainID,
                    bm.TakeEDate,
                    bm.ActVerify AS CurrentActVerify,
                    '05' AS TargetActVerify,
                    bd.MainResourceID,
                    bd.BorrowSecondResourceID,
                    bd.BorrowRealAmt
                FROM BorrowDevice bd
                INNER JOIN BorrowMain bm ON bd.BorrowMainID = bm.Id -- 👈 請確認實際主外鍵關聯欄位
                INNER JOIN BorrowMainResourceMang bmr ON bd.MainResourceID = bmr.MainResourceID
                WHERE bm.TakeEDate < @TargetDate  
                  AND bmr.IsAutoReturn = 1       -- 👈 請確認「可自動歸還」的實際欄位
                  AND bm.ActVerify <> '05'
                FOR JSON PATH, ROOT('ToUpdateList');";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add("@TargetDate", SqlDbType.DateTime).Value = simulateDate;

                    try
                    {
                        conn.Open();

                        // 使用 StringBuilder 串接，避免大 JSON 格式資料被 Reader 預設大小截斷
                        StringBuilder jsonResult = new StringBuilder();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jsonResult.Append(reader.GetValue(0).ToString());
                            }
                        }

                        string output = jsonResult.ToString();

                        // 若無符合資料，組出空的標準結構
                        if (string.IsNullOrEmpty(output))
                        {
                            output = $"{{\"SimulatedDate\": \"{simulateDate:yyyy-MM-dd HH:mm:ss}\", \"ToUpdateList\": []}}";
                        }
                        else
                        {
                            // 若有資料，在開頭塞入模擬的時間標記
                            string dateHeader = $"\"SimulatedDate\": \"{simulateDate:yyyy-MM-dd HH:mm:ss}\",";
                            output = output.Insert(1, dateHeader);
                        }

                        // 輸出 JSON 到標準控制台
                        Console.WriteLine(output);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{{\"Error\": \"SQL執行錯誤: {ex.Message.Replace("\"", "\\\"")}\"}}");
                        Environment.ExitCode = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 模式 2：正式更新資料庫（生產環境一律使用資料庫當下時間 GETDATE()，確保排程安全）
        /// </summary>
        private static void ExecuteReturnAction()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 自動歸還排程開始執行...");

            string sqlUpdate = @"
                -- 宣告 Table 變數，用以記錄本次真正有更新變動的 BorrowMainID
                DECLARE @UpdatedMains TABLE (BorrowMainID INT);

                -- 1. 更新子表 BorrowDevice 的欄位，並利用 OUTPUT 撈出對應的主表 ID
                UPDATE bd
                SET 
                    bd.ReturnSecondResourceID = bd.BorrowSecondResourceID,
                    bd.ReturnRealAmt = bd.BorrowRealAmt
                OUTPUT inserted.BorrowMainID INTO @UpdatedMains -- 👈 請確認關聯外鍵欄位名稱
                FROM BorrowDevice bd
                INNER JOIN BorrowMain bm ON bd.BorrowMainID = bm.Id 
                INNER JOIN BorrowMainResourceMang bmr ON bd.MainResourceID = bmr.MainResourceID
                WHERE bm.TakeEDate < GETDATE()  
                  AND bmr.IsAutoReturn = 1      -- 👈 請確認「可自動歸還」的實際欄位
                  AND bm.ActVerify <> '05';

                -- 2. 根據剛剛有受影響的主表 ID，將主狀態全面改為 '05'
                UPDATE bm
                SET bm.ActVerify = '05'
                FROM BorrowMain bm
                WHERE bm.Id IN (SELECT DISTINCT BorrowMainID FROM @UpdatedMains);
                
                -- 回傳本次更新受影響的明細總筆數
                SELECT @@ROWCOUNT;";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn))
                {
                    try
                    {
                        conn.Open();
                        int rowsAffected = (int)cmd.ExecuteScalar();
                        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 執行成功！共自動歸還了 {rowsAffected} 筆裝置紀錄。");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 發生嚴重錯誤: {ex.Message}");
                        Console.ResetColor();
                        Environment.ExitCode = 1;
                    }
                }
            }
        }
    }
}