using System.Web.Services;
using System.Data;
using Newtonsoft.Json;

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
        [WebMethod]
        public string GetActivityData(string SDate, string EDate)
        {
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
    }
}
