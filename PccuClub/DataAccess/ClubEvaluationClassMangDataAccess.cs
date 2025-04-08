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
    
    public class ClubEvaluationClassMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>
        public List<ClubEvaluationClassMangResultModel> GetSearchResult(ClubEvaluationClassMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			
            parameters.Add("@SchoolYear", model?.SchoolYear);
			parameters.Add("@ClassName", model?.ClassName);
            parameters.Add("@Memo", model?.Memo);

            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
     
            #endregion

            CommandText = $@"SELECT ClubEvaluationClassId, SchoolYear, ClassName, Memo, Creator, Created, LastModifier, LastModified
                               FROM ClubEvaluationClassMang
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR SchoolYear LIKE '%' + @SchoolYear + '%') 
AND (@ClassName IS NULL OR ClassName LIKE '%' + @ClassName + '%') 
AND (@Memo IS NULL OR Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<ClubEvaluationClassMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubEvaluationClassMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubEvaluationClassMangResultModel>();
        }

        /// <summary> 取得編輯資料 </summary>
        public ClubEvaluationClassMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@ID", Ser);

            #endregion

            CommandText = $@"SELECT ClubEvaluationClassId, SchoolYear, ClassName, Memo, Creator, Created, LastModifier, LastModified
                               FROM ClubEvaluationClassMang
                              WHERE 1 = 1
                                AND (ClubEvaluationClassId = @ID) ";


            (DbExecuteInfo info, IEnumerable<ClubEvaluationClassMangEditModel> entitys) dbResult = DbaExecuteQuery<ClubEvaluationClassMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ClubEvaluationClassMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
			parameters.Add("@ClassName", vm.CreateModel.ClassName);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ClubEvaluationClassMang
                                               (SchoolYear
                                               ,ClassName
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@SchoolYear
                                               ,@ClassName
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ClubEvaluationClassMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubEvaluationClassId", vm.EditModel.ClubEvaluationClassId);
			parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
			parameters.Add("@ClassName", vm.EditModel.ClassName);
			parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ClubEvaluationClassMang 
                                       SET SchoolYear = @SchoolYear, ClassName = @ClassName, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ClubEvaluationClassId = @ClubEvaluationClassId";

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

            string CommendText = $@"DELETE FROM ClubEvaluationClassMang WHERE ClubEvaluationClassId = @ID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
    }
}
