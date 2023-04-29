using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class ConformMangDataAccess : BaseAccess
    {
        public List<ConformMangEditModel> GetSearchResult()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定
            
            #endregion

            CommandText = $@"SELECT PersonalConform, ActivityConform, ClubInfoConform, Creator, Created, LastModifier, LastModified, ModifiedReason
                               FROM ConformMang";

            (DbExecuteInfo info, IEnumerable<ConformMangEditModel> entitys) dbResult = DbaExecuteQuery<ConformMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConformMangEditModel>();
        }

        public DbExecuteInfo UpdateConsent(ConformMangEditModel model, string userName)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PersonalConform", model.PersonalConform.ToString().TrimStartAndEnd());
            parameters.Add("@ActivityConform", model.ActivityConform.ToString().TrimStartAndEnd());
            parameters.Add("@ClubInfoConform", model.ClubInfoConform.ToString().TrimStartAndEnd());
            parameters.Add("@LastModifier", userName);
            #endregion 參數設定

            string CommendText = $@"UPDATE ConformMang 
                                       SET PersonalConform = @PersonalConform,
                                           ActivityConform = @ActivityConform,
                                           ClubInfoConform = @ClubInfoConform,
                                           LastModified = GETDATE(),
                                           LastModifier = @LastModifier";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        
    }
}
