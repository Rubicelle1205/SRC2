using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class ConsentMangDataAccess : BaseAccess
    {
        public List<ConsentMangEditModel> GetSearchResult()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定
            
            #endregion

            CommandText = $@"SELECT InSchool, OutSchool, InAndOutSchool, Creator, Created, LastModifier, LastModified, ModifiedReason
                               FROM ConsentMang";

            (DbExecuteInfo info, IEnumerable<ConsentMangEditModel> entitys) dbResult = DbaExecuteQuery<ConsentMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsentMangEditModel>();
        }

        public DbExecuteInfo UpdateConsent(ConsentMangEditModel model, string userName)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@InSchool", model.InSchool);
            parameters.Add("@OutSchool", model.OutSchool);
            parameters.Add("@InAndOutSchool", model.InAndOutSchool);
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

        
    }
}
