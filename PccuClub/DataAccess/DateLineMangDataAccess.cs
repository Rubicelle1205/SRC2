using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class DateLineMangDataAccess : BaseAccess
    {
        public List<DateLineMangEditModel> GetSearchResult()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定
            
            #endregion

            CommandText = $@"SELECT ActivityReport, TripCancel, Creator, Created, LastModifier, LastModified, ModifiedReason
                               FROM DateLineMang";

            (DbExecuteInfo info, IEnumerable<DateLineMangEditModel> entitys) dbResult = DbaExecuteQuery<DateLineMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<DateLineMangEditModel>();
        }

        public DbExecuteInfo UpdateConsent(DateLineMangEditModel model, string userName)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActivityReport", model.ActivityReport);
            parameters.Add("@TripCancel", model.TripCancel);
            parameters.Add("@LastModifier", userName);
            #endregion 參數設定

            string CommendText = $@"UPDATE DateLineMang 
                                       SET ActivityReport = @ActivityReport,
                                           TripCancel = @TripCancel,
                                           LastModified = GETDATE(),
                                           LastModifier = @LastModifier";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        
    }
}
