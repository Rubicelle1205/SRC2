using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DataDel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string Path = AppPath() + "DataDel.json";

            if (!System.IO.File.Exists(Path))
            {
                string info = GetParentInfo();
                WriteRecord("找不到DataDel.json檔案", info.Split('_')[0], info.Split('_')[1]);
                return;
            }
            
            string strJson = System.IO.File.ReadAllText(Path);

            JObject jObject = JObject.Parse(strJson);


            JObject joA = (JObject)jObject["DataDel"];
            JArray jaA = (JArray)joA["DelTab"];

            foreach (JToken item in jaA)
            {
                string Type = item["Type"].ToString();

                if (Type == "DB")
                {
                    JArray jaB = (JArray)item["DelNode"];

                    foreach (JToken item2 in jaB)
                    {
                        string Table = item2["Table"].ToString();
                        int Days = int.Parse(item2["Days"].ToString());
                        DelData(Days, Table);
                    }
                }
                else if (Type == "File")
                {
                    JArray jaB = (JArray)item["DelNode"];

                    foreach (JToken item2 in jaB)
                    {
                        string itemPath = item2["Path"].ToString();
                        int Days = int.Parse(item2["Days"].ToString());
                        DelFile(Days, itemPath);
                    }
                }
            }
        }

        private static void DelFile(int diffDay, string strPath)
        {
            string info = GetParentInfo();

            List<string> LstDel = new List<string>();
            string delDate = DateTime.Now.AddDays(-diffDay).ToString("yyyyMMdd");
            string[] arrPath = strPath.Split('\\');

            switch (arrPath[arrPath.Length - 2])
            {
                case "Backup":
                    string[] files = Directory.GetFiles(strPath, "*.zip");

                    foreach (string item in files)
                    {
                        string fileName = item.Split('\\')[item.Split('\\').Length - 1].Split('.')[0];
                        if (int.Parse(delDate) > int.Parse(fileName.Substring(0, 8)))
                        {
                            LstDel.Add(item);
                        }
                    }

                    if (LstDel.Count > 0)
                    {
                        foreach (string item in LstDel)
                        {
                            System.IO.File.Delete(item);
                        }
                    }

                    break;

                case "cron_logs":
                    string[] dirs = Directory.GetDirectories(strPath);

                    foreach (string item in dirs)
                    {
                        string fileName = item.Split('\\')[item.Split('\\').Length - 1].Split('.')[0];
                        if (int.Parse(delDate) > int.Parse(fileName.Substring(0, 8)))
                        {
                            WriteRecord("Delete File:" + item, info.Split('_')[0], info.Split('_')[1]);
                            LstDel.Add(item);
                        }
                    }

                    if (LstDel.Count > 0)
                    {
                        foreach (string item in LstDel)
                        {
                            WriteRecord("Delete Dir:" + item, info.Split('_')[0], info.Split('_')[1]);
                            System.IO.Directory.Delete(item, true);
                        }
                    }

                    break;

                default:
                    break;
            }

            try
            {

                
            }
            catch (Exception e)
            {
                WriteRecord(e.Message, info.Split('_')[0], info.Split('_')[1]);
            }
        }

        private static void DelData(int diffDay, string Table)
        {
            string Col = string.Empty;

            switch (Table)
            {
                case "Log_Record":
                    Col = "TIME";
                    break;
                case "UserLoginLog":
                    Col = "LoginTime";
                    break;
                case "Log_User_Action":
                    Col = "Create_Date";
                    break;
                default:
                    break;
            }

            DateTime delDate = DateTime.Now.AddDays(-diffDay);

            string strSQL = $"DELETE FROM {Table} WHERE {Col} < '{delDate.ToString("yyyy-MM-dd")} 23:59:59'";

            RunSQL(strSQL);
        }


        private static void WriteRecord(string strMsg, string ProgName, string MethodName)
        {
            string strSQL = $"INSERT INTO Log_Record (Type, LoginId, LoginName, Time, IP, Text) " +
                           $" VALUES ('System', '{ProgName}', '{MethodName}', '{DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss.sss")}', '127.0.0.1', '{strMsg}')";

            RunSQL(strSQL);
        }

        private static void RunSQL(string strSQL)
        {
            string info = GetParentInfo();
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

        private static string AppPath()
        {
            string S1 = string.Empty;

            S1 = System.AppDomain.CurrentDomain.BaseDirectory;
            if (S1.EndsWith("\\") == false)
                S1 = S1 + "\\";

            return S1;
        }

        public static String GetParentInfo()
        {
            String showString = "";
            StackTrace ss = new StackTrace(true);
            //取得呼叫當前方法之上一層類別(GetFrame(1))的屬性
            MethodBase mb = ss.GetFrame(1).GetMethod();

            //取得呼叫當前方法之上一層類別(父方)的Full class Name
            showString += mb.DeclaringType.FullName + "_";

            //取得呼叫當前方法之上一層類別(父方)的Function Name
            showString += mb.Name;

            return showString;
        }


    }
}
