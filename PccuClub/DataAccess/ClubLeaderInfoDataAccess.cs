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
    public class ClubLeaderInfoDataAccess : BaseAccess
    {
        public List<ClubLeaderInfoEditModel> GetSearchResult(string ClubId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定

            parameters.Add("@ClubId", ClubId);

            #endregion

            CommandText = $@"SELECT A.FUserId, A.UserName, A.EMail, A.CellPhone, A.Department, C.ClubId, C.ClubCName
                               FROM FUserMain A
                          LEFT JOIN ClubUser B ON B.FUserId = A.FUserId
                          LEFT JOIN ClubMang C ON C.ClubId = B.ClubId
                              WHERE C.ClubId = @ClubId";

            (DbExecuteInfo info, IEnumerable<ClubLeaderInfoEditModel> entitys) dbResult = DbaExecuteQuery<ClubLeaderInfoEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubLeaderInfoEditModel>();
        }

        public DbExecuteInfo UpdateClubLeaderInfoData(ClubLeaderInfoEditModel editModel, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@FUserId", editModel.FUserId);
            parameters.Add("@UserName", editModel.UserName);
            parameters.Add("@Department", editModel.Department);
            parameters.Add("@CellPhone", editModel.CellPhone);
            parameters.Add("@EMail", editModel.EMail);
            parameters.Add("@LastModifier", LoginUser.UserName);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            CommendText = $@"UPDATE FUserMain 
                                    SET UserName = @UserName,
                                        Department = @Department,
                                        CellPhone = @CellPhone,
                                        EMail = @EMail,
                                        LastModified = GETDATE(),
                                        LastModifier = @LastModifier
                                  WHERE FUserId = @FUserId";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
    }
}
