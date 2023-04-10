using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    public class PersonalDataAccess : BaseAccess
    {
        public List<PersonalEditModel> GetSearchResult()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定

            #endregion

            CommandText = $@"SELECT LoginId, UserName, Creator, Created, LastModifier, LastModified
                               FROM UserMain";

            (DbExecuteInfo info, IEnumerable<PersonalEditModel> entitys) dbResult = DbaExecuteQuery<PersonalEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PersonalEditModel>();
        }

        public DbExecuteInfo UpdatePersonalData(string EncryptPw, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Password", EncryptPw);
            parameters.Add("@LastModifier", LoginUser.UserName);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE UserMain 
                                       SET Password = @Password,
                                           LastModified = GETDATE(),
                                           LastModifier = @LastModifier
                                     WHERE LoginId = @LoginId";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
    }
}
