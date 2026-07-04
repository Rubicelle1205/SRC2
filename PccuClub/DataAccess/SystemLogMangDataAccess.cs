using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebPccuClub.DataAccess
{
    
    public class SystemLogMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<SystemLogMangResultModel> GetSearchResult(SystemLogMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            
            parameters.Add("@LoginId", model?.LoginId);
            parameters.Add("@UserName", model?.UserName);

            parameters.Add("@FromDate", model?.From_ReleaseDate?.Date);
            parameters.Add("@ToDate", model?.To_ReleaseDate?.Date.AddDays(1).AddTicks(-1));
           
            #endregion

            CommandText = $@"SELECT LU_Action_Id, LoginId, UserName, RoleName, IP, FunName, ActionName, Create_Date, Create_By
                               FROM Log_User_Action 
                              WHERE 1 = 1

{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? "AND Create_Date >= @FromDate AND Create_Date < @ToDate" : "")}

AND (@LoginId IS NULL OR LoginId LIKE '%' + @LoginId + '%') 
AND (@UserName IS NULL OR UserName LIKE '%' + @UserName + '%')  ";


            (DbExecuteInfo info, IEnumerable<SystemLogMangResultModel> entitys) dbResult = DbaExecuteQuery<SystemLogMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SystemLogMangResultModel>();
        }

    }
}
