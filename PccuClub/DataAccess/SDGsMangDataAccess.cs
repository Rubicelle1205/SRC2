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
    
    public class SDGsMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<SDGsMangResultModel> GetSearchResult(SDGsMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ShortName", model?.ShortName);
            parameters.Add("@Desc", model?.Desc);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT SDGID, ShortName, [Desc], Creator, Created, LastModifier, LastModified, ModifiedReason
FROM SDGsMang
WHERE 1 = 1
AND (@ShortName IS NULL OR ShortName LIKE '%' + @ShortName + '%') 
AND (@Desc IS NULL OR [Desc] LIKE '%' + @Desc + '%') ";


            (DbExecuteInfo info, IEnumerable<SDGsMangResultModel> entitys) dbResult = DbaExecuteQuery<SDGsMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SDGsMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public SDGsMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@SDGID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT SDGID, ShortName, [Desc], Creator, Created, LastModifier, LastModified, ModifiedReason
FROM SDGsMang
WHERE 1 = 1
AND (SDGID = @SDGID) ";


            (DbExecuteInfo info, IEnumerable<SDGsMangEditModel> entitys) dbResult = DbaExecuteQuery<SDGsMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(SDGsMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ShortName", vm.CreateModel.ShortName);
            parameters.Add("@Desc", vm.CreateModel.Desc);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO SDGsMang
                                               (ShortName
                                               ,[Desc]
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ShortName
                                               ,@Desc
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE()
                                               ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(SDGsMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SDGID", vm.EditModel.SDGID);
            parameters.Add("@ShortName", vm.EditModel.ShortName);
            parameters.Add("@Desc", vm.EditModel.Desc);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE SDGsMang 
                                       SET ShortName = @ShortName, [Desc] = @Desc, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE SDGID = @SDGID";

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
            parameters.Add("@SDGID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM SDGsMang WHERE SDGID = @SDGID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 匯入資料
        /// </summary>
        /// <param name="lstExcel"></param>
        /// <param name="loginUser"></param>
        /// <exception cref="NotImplementedException"></exception>
        public DbExecuteInfo ImportData(List<SDGsMangExcelResultModel> dataList, UserInfo loginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO SDGsMang
                                               (ShortName
                                               ,[Desc]
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ShortName
                                               ,@Desc
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
        public List<SDGsMangResultModel> GetExportResult(SDGsMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ShortName", model?.ShortName);
            parameters.Add("@Desc", model?.Desc);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT SDGID, ShortName, [Desc], Creator, Created, LastModifier, LastModified, ModifiedReason
FROM SDGsMang
WHERE 1 = 1
AND (@ShortName IS NULL OR ShortName LIKE '%' + @ShortName + '%') 
AND (@Desc IS NULL OR [Desc] LIKE '%' + @Desc + '%') ";

            (DbExecuteInfo info, IEnumerable<SDGsMangResultModel> entitys) dbResult = DbaExecuteQuery<SDGsMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SDGsMangResultModel>();
        }

        
    }
}
