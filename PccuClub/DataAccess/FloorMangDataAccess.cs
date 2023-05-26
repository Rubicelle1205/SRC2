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
    
    public class FloorMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<FloorMangResultModel> GetSearchResult(FloorMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@FloorName", model?.FloorName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT FloorID, FloorName, Memo, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM FloorMang
WHERE 1 = 1
AND (@FloorName IS NULL OR FloorName LIKE '%' + @FloorName + '%') 
AND (@Memo IS NULL OR Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<FloorMangResultModel> entitys) dbResult = DbaExecuteQuery<FloorMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FloorMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public FloorMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@FloorID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT FloorID, FloorName, Memo, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM FloorMang
WHERE 1 = 1
AND (FloorID = @FloorID) ";


            (DbExecuteInfo info, IEnumerable<FloorMangEditModel> entitys) dbResult = DbaExecuteQuery<FloorMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(FloorMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@FloorName", vm.CreateModel.FloorName);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO FloorMang
                                               (FloorName
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@FloorName
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
        public DbExecuteInfo UpdateData(FloorMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@FloorID", vm.EditModel.FloorID);
            parameters.Add("@FloorName", vm.EditModel.FloorName);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE FloorMang 
                                       SET FloorName = @FloorName, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE FloorID = @FloorID";

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
            parameters.Add("@FloorID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM FloorMang WHERE FloorID = @FloorID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
        
    }
}
