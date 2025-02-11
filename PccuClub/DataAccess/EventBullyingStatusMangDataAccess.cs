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
    
    public class EventBullyingStatusMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<EventBullyingStatusMangResultModel> GetSearchResult(EventBullyingStatusMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@Text", model?.Text);
            parameters.Add("@Enable", model?.Enable);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.Text, A.CaseSystemType, A.Enable, B.Text AS EnableText, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified
FROM EventStatusMang A
LEFT JOIN Code B ON B.Code = A.Enable AND B.Type = 'Enable'
WHERE 1 = 1
AND A.CaseSystemType = '03'
AND (@Text IS NULL OR A.Text LIKE '%' + @Text + '%') 
AND (@Enable IS NULL OR A.Enable LIKE '%' + @Enable + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<EventBullyingStatusMangResultModel> entitys) dbResult = DbaExecuteQuery<EventBullyingStatusMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventBullyingStatusMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EventBullyingStatusMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ID, Text, CaseSystemType, Enable, Memo, Creator, Created, LastModifier, LastModified
FROM EventStatusMang
WHERE 1 = 1
AND CaseSystemType = '03'
AND (ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<EventBullyingStatusMangEditModel> entitys) dbResult = DbaExecuteQuery<EventBullyingStatusMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(EventBullyingStatusMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Text", vm.CreateModel.Text);
            parameters.Add("@Enable", vm.CreateModel.Enable);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO EventStatusMang
                                               (Text
                                               ,CaseSystemType
                                               ,Enable
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@Text
                                               ,'03'
                                               ,@Enable
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(EventBullyingStatusMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", vm.EditModel.ID);
            parameters.Add("@Text", vm.EditModel.Text);
            parameters.Add("@Enable", vm.EditModel.Enable);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE EventStatusMang 
                                       SET Text = @Text, Enable = @Enable, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ID = @ID AND CaseSystemType = '03'";

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

            string CommendText = $@"DELETE FROM EventStatusMang WHERE ID = @ID AND CaseSystemType = '03s'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetddlEnable()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'Enable'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
