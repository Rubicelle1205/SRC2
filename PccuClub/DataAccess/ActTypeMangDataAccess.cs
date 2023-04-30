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
    
    public class ActTypeMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ActTypeMangResultModel> GetSearchResult(ActTypeMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActTypeName", model?.ActTypeName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ActTypeID, ActTypeName, Memo, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM ActTypeMang
WHERE 1 = 1
AND (@ActTypeName IS NULL OR ActTypeName LIKE '%' + @ActTypeName + '%') 
AND (@Memo IS NULL OR Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<ActTypeMangResultModel> entitys) dbResult = DbaExecuteQuery<ActTypeMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActTypeMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ActTypeMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActTypeID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ActTypeID, ActTypeName, Memo, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM ActTypeMang
WHERE 1 = 1
AND (ActTypeID = @ActTypeID) ";


            (DbExecuteInfo info, IEnumerable<ActTypeMangEditModel> entitys) dbResult = DbaExecuteQuery<ActTypeMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ActTypeMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActTypeName", vm.CreateModel.ActTypeName);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActTypeMang
                                               (ActTypeName
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ActTypeName
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE()
                                               ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ActTypeMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActTypeID", vm.EditModel.ActTypeID);
            parameters.Add("@ActTypeName", vm.EditModel.ActTypeName);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ActTypeMang 
                                       SET ActTypeName = @ActTypeName, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ActTypeID = @ActTypeID";

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
            parameters.Add("@ActTypeID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM ActTypeMang WHERE ActTypeID = @ActTypeID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 匯入資料
        /// </summary>
        /// <param name="lstExcel"></param>
        /// <param name="loginUser"></param>
        /// <exception cref="NotImplementedException"></exception>
        public DbExecuteInfo ImportData(List<ActTypeMangExcelResultModel> dataList, UserInfo loginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO ActTypeMang
                                               (ActTypeName
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ActTypeName
                                               ,@Memo
                                               ,'{loginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{loginUser.LoginId}'
                                               ,GETDATE()
                                               ,NULL)";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);
     
            return ExecuteResult;
        }

        /// <summary>
        /// Excel 取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ActTypeMangResultModel> GetExportResult(ActTypeMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActTypeName", model?.ActTypeName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ActTypeID, ActTypeName, Memo, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM ActTypeMang
WHERE 1 = 1
AND (@ActTypeName IS NULL OR ActTypeName LIKE '%' + @ActTypeName + '%') 
AND (@Memo IS NULL OR Memo LIKE '%' + @Memo + '%') ";

            (DbExecuteInfo info, IEnumerable<ActTypeMangResultModel> entitys) dbResult = DbaExecuteQuery<ActTypeMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActTypeMangResultModel>();
        }

        
    }
}
