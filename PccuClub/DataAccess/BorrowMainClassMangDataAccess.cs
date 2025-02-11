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
    
    public class BorrowMainClassMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BorrowMainClassMangResultModel> GetSearchResult(BorrowMainClassMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@Text", model?.Text);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ID, Text, ActVerifyUnit, BorrowSDate, BorrowEDate, BorrowRule, BorrowFee, ReserveRule,
       CoverPath, Memo, Creator, Created, LastModifier, LastModified
FROM BorrowMainClassMang
WHERE 1 = 1
AND (@Text IS NULL OR Text LIKE '%' + @Text + '%') 
AND (@Memo IS NULL OR Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<BorrowMainClassMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowMainClassMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowMainClassMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BorrowMainClassMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ID, Text, ActVerifyUnit, BorrowSDate, BorrowEDate, BorrowRule, BorrowFee, ReserveRule,
       CoverPath, Memo, Creator, Created, LastModifier, LastModified
FROM BorrowMainClassMang
WHERE 1 = 1
AND (ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<BorrowMainClassMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowMainClassMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(BorrowMainClassMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Text", vm.CreateModel.Text);
            parameters.Add("@ActVerifyUnit", vm.CreateModel.ActVerifyUnit);
            parameters.Add("@BorrowSDate", vm.CreateModel.BorrowSDate.Value.ToString("yyyy-MM-dd"));
            parameters.Add("@BorrowEDate", vm.CreateModel.BorrowEDate.Value.ToString("yyyy-MM-dd"));
            parameters.Add("@BorrowRule", vm.CreateModel.BorrowRule);
            parameters.Add("@BorrowFee", vm.CreateModel.BorrowFee);
            parameters.Add("@ReserveRule", vm.CreateModel.ReserveRule);
            parameters.Add("@CoverPath", vm.CreateModel.CoverPath);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO BorrowMainClassMang
                                               (Text
                                               ,ActVerifyUnit
                                               ,BorrowSDate
                                               ,BorrowEDate
                                               ,BorrowRule
                                               ,BorrowFee
                                               ,ReserveRule
                                               ,CoverPath
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@Text
                                               ,@ActVerifyUnit
                                               ,@BorrowSDate
                                               ,@BorrowEDate
                                               ,@BorrowRule
                                               ,@BorrowFee
                                               ,@ReserveRule
                                               ,@CoverPath
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(BorrowMainClassMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", vm.EditModel.ID);

            parameters.Add("@ActVerifyUnit", vm.EditModel.ActVerifyUnit);
            parameters.Add("@BorrowSDate", vm.EditModel.BorrowSDate);
            parameters.Add("@BorrowEDate", vm.EditModel.BorrowEDate);
            parameters.Add("@BorrowRule", vm.EditModel.BorrowRule);
            parameters.Add("@BorrowFee", vm.EditModel.BorrowFee);
            parameters.Add("@ReserveRule", vm.EditModel.ReserveRule);
            parameters.Add("@CoverPath", vm.EditModel.CoverPath);

            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@Text", vm.EditModel.Text);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE BorrowMainClassMang 
                                       SET Text = @Text, ActVerifyUnit = @ActVerifyUnit, BorrowSDate = @BorrowSDate, BorrowEDate = @BorrowEDate, 
                                           BorrowRule = @BorrowRule, BorrowFee = @BorrowFee, ReserveRule = @ReserveRule, CoverPath = @CoverPath, Memo = @Memo, 
                                           LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ID = @ID";

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

            string CommendText = $@"DELETE FROM BorrowMainClassMang WHERE ID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
    }
}
