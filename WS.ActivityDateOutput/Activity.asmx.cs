using System.Web.Services;
using System.Data;
using Newtonsoft.Json;
using System;
using WebPccuClub.Models;
using System.Security.Policy;

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

        [WebMethod]
        public string GetActivityData(string SDate, string EDate)
        {
            string chkStr = ChkDate(SDate, EDate);

            if (chkStr != "")
                return chkStr;

            DataTable dt = new DataTable();

            dt.Columns.Add("SpaceID");
            dt.Columns.Add("SpaceName");
            dt.Columns.Add("UnitBegin");

            for (int i = 0; i <= 3; i++)
            {
                DataRow dr = dt.NewRow();

                dr["SpaceID"] = "SP0000" + i.ToString();
                dr["SpaceName"] = "恩100" + i.ToString();
                dr["UnitBegin"] = "09:00";
                dt.Rows.Add(dr);
            }

            return JsonConvert.SerializeObject(dt);
        }

        private string ChkDate(string SDate, string EDate)
        {
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
