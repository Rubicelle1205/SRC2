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
    
    public class ClubEvaluationMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>
        public List<ClubEvaluationMangResultModel> GetSearchResult(ClubEvaluationMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			
            parameters.Add("@SchoolYear", model?.SchoolYear);
			parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@Memo", model?.Memo);
            parameters.Add("@ClubEvaluationClassId", model?.ClubEvaluationClassId);
            parameters.Add("@ClubEvaluationItemId", model?.ClubEvaluationItemId);

            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
     
            #endregion

            CommandText = $@"SELECT A.ClubEvaluationId, A.SchoolYear, A.ClubID, A.ClubEvaluationItemId, A.Score, A.Memo, 
                                    B.ClubCName, D.ClassName, C.ItemName, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM ClubEvaluationMang A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID 
                          LEFT JOIN ClubEvaluationItemMang C ON C.ClubEvaluationItemId = A.ClubEvaluationItemId 
                          LEFT JOIN ClubEvaluationClassMang D ON D.ClubEvaluationClassId = A.ClubEvaluationClassId 
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear LIKE '%' + @SchoolYear + '%') 
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR B.ClubCName LIKE '%' + @ClubCName + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') 
AND (@ClubEvaluationItemId IS NULL OR C.ClubEvaluationItemId LIKE '%' + @ClubEvaluationItemId + '%') 
AND (@ClubEvaluationClassId IS NULL OR D.ClubEvaluationClassId LIKE '%' + @ClubEvaluationClassId + '%') ";


            (DbExecuteInfo info, IEnumerable<ClubEvaluationMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubEvaluationMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubEvaluationMangResultModel>();
        }

        /// <summary> 取得編輯資料 </summary>
        public ClubEvaluationMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@ID", Ser);

            #endregion

            CommandText = $@"SELECT ClubEvaluationId, SchoolYear, ClubID, ClubEvaluationClassId, ClubEvaluationItemId, 
                                    Score, Memo, Creator, Created, LastModifier, LastModified
                               FROM ClubEvaluationMang
                              WHERE 1 = 1
                                AND (ClubEvaluationId = @ID) ";


            (DbExecuteInfo info, IEnumerable<ClubEvaluationMangEditModel> entitys) dbResult = DbaExecuteQuery<ClubEvaluationMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }


        public List<ClubEvaluationHistory> GetHistoryData(ClubEvaluationMangViewModel vm, string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@ClubID", vm.EditModel.ClubID);
            parameters.Add("@ID", Ser);

            #endregion

            CommandText = $@"SELECT A.Score, A.Memo, C.ItemName, A.Created
                               FROM ClubEvaluationMang A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID 
                          LEFT JOIN ClubEvaluationItemMang C ON C.ClubEvaluationItemId = A.ClubEvaluationItemId 
                          LEFT JOIN ClubEvaluationClassMang D ON D.ClubEvaluationClassId = A.ClubEvaluationClassId 
                              WHERE 1 = 1
AND A.SchoolYear = @SchoolYear 
AND A.ClubID = @ClubID ";


            (DbExecuteInfo info, IEnumerable<ClubEvaluationHistory> entitys) dbResult = DbaExecuteQuery<ClubEvaluationHistory>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubEvaluationHistory>();
        }

        public string GetBaseScore(ClubEvaluationMangViewModel vm)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            #endregion

            CommandText = $@"SELECT A.BasicScore
                               FROM ClubBasicScoreMang A
                              WHERE 1 = 1
                                AND (A.SchoolYear = @SchoolYear) ";

            (DbExecuteInfo info, IEnumerable<string> entitys) dbResult = DbaExecuteQuery<string>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ClubEvaluationMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@ClubID", vm.CreateModel.ClubID);
            parameters.Add("@ClubEvaluationClassId", vm.CreateModel.ClubEvaluationClassId);
            parameters.Add("@ClubEvaluationItemId", vm.CreateModel.ClubEvaluationItemId);
			parameters.Add("@Score", vm.CreateModel.Score);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ClubEvaluationMang
                                               (SchoolYear
                                               ,ClubID
                                               ,ClubEvaluationClassId
                                               ,ClubEvaluationItemId
                                               ,Score
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@SchoolYear
                                               ,@ClubID
                                               ,@ClubEvaluationClassId
                                               ,@ClubEvaluationItemId
                                               ,@Score
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ClubEvaluationMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubEvaluationId", vm.EditModel.ClubEvaluationId);
			parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@ClubID", vm.EditModel.ClubID);
            parameters.Add("@ClubEvaluationClassId", vm.EditModel.ClubEvaluationClassId);
            parameters.Add("@ClubEvaluationItemId", vm.EditModel.ClubEvaluationItemId);
			parameters.Add("@Score", vm.EditModel.Score);
			parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ClubEvaluationMang 
                                       SET SchoolYear = @SchoolYear, ClubEvaluationClassId = @ClubEvaluationClassId, ClubEvaluationItemId = @ClubEvaluationItemId, 
                                           ClubID = @ClubID, Score = @Score, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ClubEvaluationId = @ClubEvaluationId";

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

            string CommendText = $@"DELETE FROM ClubEvaluationMang WHERE ClubEvaluationId = @ID";

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

        public List<SelectListItem> GetItemId()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT A.ClubEvaluationItemId AS VALUE, A.ItemName AS Text 
							  FROM ClubEvaluationItemMang A 
";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
