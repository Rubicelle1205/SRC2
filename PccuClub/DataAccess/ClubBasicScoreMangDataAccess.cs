using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class ClubBasicScoreMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>
        public List<ClubBasicScoreMangResultModel> GetSearchResult(ClubBasicScoreMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@Memo", model?.Memo);

            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
     
            #endregion

            CommandText = $@"SELECT ClubBasicScoreId, SchoolYear, BasicScore, Memo, Creator, Created, LastModifier, LastModified
                               FROM ClubBasicScoreMang
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR SchoolYear LIKE '%' + @SchoolYear + '%') 
AND (@Memo IS NULL OR Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<ClubBasicScoreMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubBasicScoreMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubBasicScoreMangResultModel>();
        }

        /// <summary> 取得編輯資料 </summary>
        public ClubBasicScoreMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@ID", Ser);

            #endregion

            CommandText = $@"SELECT ClubBasicScoreId, SchoolYear, BasicScore, Memo, Creator, Created, LastModifier, LastModified
                               FROM ClubBasicScoreMang
                              WHERE 1 = 1
                                AND (ClubBasicScoreId = @ID) ";


            (DbExecuteInfo info, IEnumerable<ClubBasicScoreMangEditModel> entitys) dbResult = DbaExecuteQuery<ClubBasicScoreMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ClubBasicScoreMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
			parameters.Add("@BasicScore", vm.CreateModel.BasicScore);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ClubBasicScoreMang
                                               (SchoolYear
                                               ,BasicScore
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@SchoolYear
                                               ,@BasicScore
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ClubBasicScoreMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubBasicScoreId", vm.EditModel.ClubBasicScoreId);
			parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
			parameters.Add("@BasicScore", vm.EditModel.BasicScore);
			parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ClubBasicScoreMang 
                                       SET SchoolYear = @SchoolYear, BasicScore = @BasicScore, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ClubBasicScoreId = @ClubBasicScoreId";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeletetData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM ClubBasicScoreMang WHERE ClubBasicScoreId = @ID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DataTable CheckSomeBaseScore(ClubBasicScoreMangViewModel vm)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            if(vm.CreateModel != null)
            {
                parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            }
            else if (vm.EditModel != null)
            {
                parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            }


            #endregion

            CommandText = @"SELECT ClubBasicScoreId, SchoolYear, BasicScore
                              FROM ClubBasicScoreMang 
                             WHERE SchoolYear = @SchoolYear";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }


    }
}
