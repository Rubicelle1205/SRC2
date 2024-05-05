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
    
    public class EventCaseReferMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<EventCaseReferMangResultModel> GetSearchResult(EventCaseReferMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ReferName", model?.ReferName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ReferID,	ReferName, Memo, Creator, Created, LastModifier, LastModified
FROM ReferUnitMang
WHERE 1 = 1
AND (@ReferName IS NULL OR ReferName LIKE '%' + @ReferName + '%') 
AND (@Memo IS NULL OR Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<EventCaseReferMangResultModel> entitys) dbResult = DbaExecuteQuery<EventCaseReferMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventCaseReferMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EventCaseReferMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ReferID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ReferID,	ReferName, Memo, Creator, Created, LastModifier, LastModified
FROM ReferUnitMang
WHERE 1 = 1
AND (ReferID = @ReferID) ";


            (DbExecuteInfo info, IEnumerable<EventCaseReferMangEditModel> entitys) dbResult = DbaExecuteQuery<EventCaseReferMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(EventCaseReferMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ReferName", vm.CreateModel.ReferName);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ReferUnitMang
                                               (ReferName
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@ReferName
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(EventCaseReferMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ReferID", vm.EditModel.ReferID);
            parameters.Add("@ReferName", vm.EditModel.ReferName);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ReferUnitMang 
                                       SET ReferName = @ReferName, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ReferID = @ReferID";

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
            parameters.Add("@ReferID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM ReferUnitMang WHERE ReferID = @ReferID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

    }
}
