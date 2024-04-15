using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    public class ConsultationPersonalDataAccess : BaseAccess
    {
        public List<ConsultationPersonalEditModel> GetSearchResult(string LoginId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定

            parameters.Add("@LoginId", LoginId);

            #endregion

            CommandText = $@"SELECT LoginId, UserName, EMail, Creator, Created, LastModifier, LastModified
                               FROM UserMain
                              WHERE LoginId = @LoginId";

            (DbExecuteInfo info, IEnumerable<ConsultationPersonalEditModel> entitys) dbResult = DbaExecuteQuery<ConsultationPersonalEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsultationPersonalEditModel>();
        }

        public DbExecuteInfo UpdateConsultationPersonalData(string EncryptPw, ConsultationPersonalEditModel editModel, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            if (!string.IsNullOrEmpty(EncryptPw))
            {
                #region 參數設定
                parameters.Add("@Password", EncryptPw);
                parameters.Add("@EMail", editModel.EMail);
                parameters.Add("@LastModifier", LoginUser.UserName);
                parameters.Add("@LoginId", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE UserMain 
                                       SET Password = @Password,
                                           EMail = @EMail,
                                           LastModified = GETDATE(),
                                           LastModifier = @LastModifier
                                     WHERE LoginId = @LoginId";
            }
            else
            {
                #region 參數設定
                parameters.Add("@EMail", editModel.EMail);
                parameters.Add("@LastModifier", LoginUser.UserName);
                parameters.Add("@LoginId", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE UserMain 
                                       SET EMail = @EMail,
                                           LastModified = GETDATE(),
                                           LastModifier = @LastModifier
                                     WHERE LoginId = @LoginId";
            }
            

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
    }
}
