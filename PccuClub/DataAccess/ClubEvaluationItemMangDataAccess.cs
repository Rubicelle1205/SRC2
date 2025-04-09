using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class ClubEvaluationItemMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>
        public List<ClubEvaluationItemMangResultModel> GetSearchResult(ClubEvaluationItemMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ClassId", model?.ClassId);
            parameters.Add("@ItemName", model?.ItemName);
            parameters.Add("@Memo", model?.Memo);

            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
     
            #endregion

            CommandText = $@"SELECT A.ClubEvaluationItemId, A.SchoolYear, A.ClassId, B.ClassName, A.ItemName, A.ScoreUpper, A.ScoreLower, 
                                    A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM ClubEvaluationItemMang A
                          LEFT JOIN ClubEvaluationClassMang B ON B.ClubEvaluationClassId = A.ClassId
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear LIKE '%' + @SchoolYear + '%') 
AND (@ClassId IS NULL OR A.ClassId LIKE '%' + @ClassId + '%') 
AND (@ItemName IS NULL OR A.ItemName LIKE '%' + @ItemName + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<ClubEvaluationItemMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubEvaluationItemMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubEvaluationItemMangResultModel>();
        }

        /// <summary> 取得編輯資料 </summary>
        public ClubEvaluationItemMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@ID", Ser);

            #endregion

            CommandText = $@"SELECT ClubEvaluationItemId, SchoolYear, ClassId, ItemName, ScoreUpper, ScoreLower, 
                                    Memo, Creator, Created, LastModifier, LastModified
                               FROM ClubEvaluationItemMang
                              WHERE 1 = 1
                                AND (ClubEvaluationItemId = @ID) ";


            (DbExecuteInfo info, IEnumerable<ClubEvaluationItemMangEditModel> entitys) dbResult = DbaExecuteQuery<ClubEvaluationItemMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ClubEvaluationItemMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@ClassId", vm.CreateModel.ClassId);
            parameters.Add("@ItemName", vm.CreateModel.ItemName);
            parameters.Add("@ScoreUpper", vm.CreateModel.ScoreUpper);
            parameters.Add("@ScoreLower", vm.CreateModel.ScoreLower);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ClubEvaluationItemMang
                                               (SchoolYear
                                               ,ClassId 
                                               ,ItemName
                                               ,ScoreUpper
                                               ,ScoreLower 
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@SchoolYear
                                               ,@ClassId 
                                               ,@ItemName
                                               ,@ScoreUpper
                                               ,@ScoreLower
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ClubEvaluationItemMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubEvaluationItemId", vm.EditModel.ClubEvaluationItemId);
			parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@ClassId", vm.EditModel.ClassId);
            parameters.Add("@ItemName", vm.EditModel.ItemName);
            parameters.Add("@ScoreUpper", vm.EditModel.ScoreUpper);
            parameters.Add("@ScoreLower", vm.EditModel.ScoreLower);
			parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ClubEvaluationItemMang 
                                       SET SchoolYear = @SchoolYear, ClassId = @ClassId, ItemName = @ItemName,
                                           ScoreUpper = @ScoreUpper, ScoreLower = @ScoreLower, 
                                           Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ClubEvaluationItemId = @ClubEvaluationItemId";

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

            string CommendText = $@"DELETE FROM ClubEvaluationItemMang WHERE ClubEvaluationItemId = @ID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }


        public List<SelectListItem> GetClassId()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT A.ClubEvaluationClassId AS VALUE, A.ClassName AS Text 
							  FROM ClubEvaluationClassMang A 
";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
