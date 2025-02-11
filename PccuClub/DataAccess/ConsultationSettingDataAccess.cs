using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{

    public class ConsultationSettingDataAccess : BaseAccess
    {
        public List<ConsultationSettingEditModel> GetSearchResult()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定

            #endregion

            CommandText = $@"SELECT UniversalCode, NotifyMail, Memo, Creator, Created, LastModifier, LastModified
                               FROM ConsultationSetting";

            (DbExecuteInfo info, IEnumerable<ConsultationSettingEditModel> entitys) dbResult = DbaExecuteQuery<ConsultationSettingEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsultationSettingEditModel>();
        }

        public DbExecuteInfo UpdateConsent(ConsultationSettingEditModel model, string userName)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@UniversalCode", model.UniversalCode);
            parameters.Add("@NotifyMail", model.NotifyMail);
            parameters.Add("@Memo", model.Memo);
            parameters.Add("@LastModifier", userName);
            #endregion 參數設定

            string CommendText = $@"IF EXISTS (SELECT 1
                                         FROM ConsultationSetting)
                                    
                                BEGIN
                                       UPDATE ConsultationSetting 
                                       SET UniversalCode = @UniversalCode,
                                           NotifyMail = @NotifyMail,
                                           Memo = @Memo,
                                           LastModified = GETDATE(),
                                           LastModifier = @LastModifier;
                                    END
                                ELSE
                                    BEGIN
                                        INSERT INTO ConsultationSetting
                                                (UniversalCode, 
                                                 NotifyMail, 
                                                 Memo, 
                                                 Creator, 
                                                 Created, 
                                                 LastModified, 
                                                 LastModifier)
                                         VALUES
                                               (@UniversalCode, 
                                                @NotifyMail, 
                                                @Memo, 
                                                @LastModifier, 
                                                GETDATE(),
                                                GETDATE(), 
                                                @LastModifier);
                                    END
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }


    }
}
