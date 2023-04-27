using DataAccess;
using PccuClub.WebAuth;
using System.Data;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{

    public class BuildMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BuildMangResultModel> GetSearchResult(BuildMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@BuildName", model?.BuildName);
            parameters.Add("@Note", model?.Note);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT BuildID, BuildName, Note, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM BuildMang
WHERE 1 = 1
AND (@BuildName IS NULL OR BuildName LIKE '%' + @BuildName + '%') 
AND (@Note IS NULL OR Note LIKE '%' + @Note + '%') ";


            (DbExecuteInfo info, IEnumerable<BuildMangResultModel> entitys) dbResult = DbaExecuteQuery<BuildMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BuildMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BuildMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@BuildID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT BuildID, BuildName, Note, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM BuildMang
WHERE 1 = 1
AND (BuildID = @BuildID) ";


            (DbExecuteInfo info, IEnumerable<BuildMangEditModel> entitys) dbResult = DbaExecuteQuery<BuildMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(BuildMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@BuildName", vm.CreateModel.BuildName);
            parameters.Add("@Note", vm.CreateModel.Note);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO BuildMang
                                               (BuildName
                                               ,Note
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@BuildName
                                               ,@Note
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE()
                                               ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(BuildMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@BuildID", vm.EditModel.BuildID);
            parameters.Add("@BuildName", vm.EditModel.BuildName);
            parameters.Add("@Note", vm.EditModel.Note);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE BuildMang 
                                       SET BuildName = @BuildName, Note = @Note, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE BuildID = @BuildID";

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
            parameters.Add("@BuildID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM BuildMang WHERE BuildID = @BuildID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 匯入資料
        /// </summary>
        /// <param name="lstExcel"></param>
        /// <param name="loginUser"></param>
        /// <exception cref="NotImplementedException"></exception>
        public DbExecuteInfo ImportData(List<BuildMangExcelResultModel> dataList, UserInfo loginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO BuildMang
                                               (BuildName
                                               ,Note
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@BuildName
                                               ,@Note
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
        public List<BuildMangResultModel> GetExportResult(BuildMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@BuildName", model?.BuildName);
            parameters.Add("@Note", model?.Note);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT BuildID, BuildName, Note, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM BuildMang
WHERE 1 = 1
AND (@BuildName IS NULL OR BuildName LIKE '%' + @BuildName + '%') 
AND (@Note IS NULL OR Note LIKE '%' + @Note + '%') ";

            (DbExecuteInfo info, IEnumerable<BuildMangResultModel> entitys) dbResult = DbaExecuteQuery<BuildMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BuildMangResultModel>();
        }

        
    }
}
