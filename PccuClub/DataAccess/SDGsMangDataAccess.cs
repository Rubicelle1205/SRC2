using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class SDGsMangDataAccess : BaseAccess
    {
        public List<SDGsMangDataAccess> GetSearchResult()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定
            
            #endregion

            CommandText = $@"SELECT SDGID, ShortName, [Desc], Creator, Created, LastModifier, LastModified, ModifiedReason
                               FROM SDGsMang";

            (DbExecuteInfo info, IEnumerable<SDGsMangDataAccess> entitys) dbResult = DbaExecuteQuery<SDGsMangDataAccess>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SDGsMangDataAccess>();
        }

        public DbExecuteInfo UpdateConsent(SDGsMangDataAccess model, string userName)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            //parameters.Add("@InSchool", model.InSchool);
            //parameters.Add("@OutSchool", model.OutSchool);
            //parameters.Add("@InAndOutSchool", model.InAndOutSchool);
            parameters.Add("@LastModifier", userName);
            #endregion 參數設定

            string CommendText = $@"UPDATE ConsentMang 
                                       SET InSchool = @InSchool,
                                           OutSchool = @OutSchool,
                                           InAndOutSchool = @InAndOutSchool,
                                           LastModified = GETDATE(),
                                           LastModifier = @LastModifier";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 查詢結果 </summary>

        public List<SDGsMangResultModel> GetSearchResult(SDGsMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ShortName", model?.ShortName);
            parameters.Add("@Desc", model.Desc);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT SDGID, ShortName, [Desc], Creator, Created, LastModifier, LastModified, ModifiedReason
FROM SDGsMang
WHERE 1 = 1
AND (@ShortName IS NULL OR ShortName LIKE '%' + @ShortName + '%') 
AND (@Desc IS NULL OR [Desc] LIKE '%' + @Desc + '%') ";


            (DbExecuteInfo info, IEnumerable<SDGsMangResultModel> entitys) dbResult = DbaExecuteQuery<SDGsMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SDGsMangResultModel>();
        }

    }
}
