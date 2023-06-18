using System.Web.Services;
using System.Data;
using Newtonsoft.Json;
using System;
using WebPccuClub.Models;
using System.Security.Policy;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;

namespace WebPccuClub.WS.ActivityDateOuput
{
    /// <summary>
    /// Member 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class Activity : System.Web.Services.WebService
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ConnectionString;

        [WebMethod]
        public string GetActivityData(string SDate, string EDate)
        {
            string startDate = string.Empty;
            string endDate = string.Empty;

            string chkStr = ChkDate(SDate, EDate, out startDate, out endDate);

            if (chkStr != "")
                return chkStr;

            string query = $@"SELECT ActPlaceID AS SpaceID, ActPlaceText AS SpaceName, [Date] + ' ' + STime + ':00:00' AS UnitBegin
                                FROM ActRundown
                               WHERE  1 = 1 
                                 AND RundownStatus = '01' 
                                 AND [Date] BETWEEN '{startDate}' AND '{endDate}' ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    return JsonConvert.SerializeObject(dataTable);
                }
            }
        }

        private string ChkDate(string SDate, string EDate, out string startDate, out string endDate)
        {
            startDate = "";
            endDate = "";

            DateTime d1 = new DateTime();
            DateTime d2 = new DateTime();

            string f1 = SDate.Length == 8 ? DateTime.TryParse(string.Format("{0}-{1}-{2}", SDate.Substring(0, 4), SDate.Substring(4, 2), SDate.Substring(6, 2)), out d1) ? string.Format("{0}-{1}-{2}", SDate.Substring(0, 4), SDate.Substring(4, 2), SDate.Substring(6, 2)) : "error" : "error";
            string f2 = EDate.Length == 8 ? DateTime.TryParse(string.Format("{0}-{1}-{2}", EDate.Substring(0, 4), EDate.Substring(4, 2), EDate.Substring(6, 2)), out d2) ? string.Format("{0}-{1}-{2}", EDate.Substring(0, 4), EDate.Substring(4, 2), EDate.Substring(6, 2)) : "error" : "error";
            string f3 = f1 != "error" && f2 != "error" ? GetDay(f1, f2) : "error";
            if (f1 == "error" || f2 == "error")
            {
                vmRtn.ErrorCode = 1;
                vmRtn.ErrorMsg = "錯誤的日期輸入";
                return JsonConvert.SerializeObject(vmRtn);
            }
            
            if (f3 != "ok")
            {
                vmRtn.ErrorCode = 1;
                vmRtn.ErrorMsg = "輸入期間超過90天限制";
                return JsonConvert.SerializeObject(vmRtn);
            }

            startDate = d1.ToString("yyyy-MM-dd 00:00:00");
            endDate = d2.ToString("yyyy-MM-dd 23:59:59");

            return "";
        }
        private static string GetDay(string f1, string f2)
        {
            TimeSpan ts = DateTime.Parse(f1) - DateTime.Parse(f2);
            int diff = Int32.Parse(ts.ToString("dd"));

            if (diff > 90)
                return "error";
            else
                return "ok";
        }
    }
}
