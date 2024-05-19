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
    
    public class BorrowSecondClassMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BorrowSecondClassMangResultModel> GetSearchResult(BorrowSecondClassMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainClass", model?.MainClass);
            parameters.Add("@Text", model?.Text);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.Text, A.MainClass, B.Text AS MainClassText, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified
FROM BorrowSecondClassMang A
LEFT JOIN BorrowMainClassMang B ON B.ID = A.MainClass
WHERE 1 = 1
AND (@MainClass IS NULL OR A.MainClass = @MainClass) 
AND (@Text IS NULL OR A.Text LIKE '%' + @Text + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<BorrowSecondClassMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowSecondClassMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowSecondClassMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BorrowSecondClassMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ID, Text, MainClass, Memo, Creator, Created, LastModifier, LastModified
FROM BorrowSecondClassMang
WHERE 1 = 1
AND (ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<BorrowSecondClassMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowSecondClassMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(BorrowSecondClassMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Text", vm.CreateModel.Text);
            parameters.Add("@MainClass", vm.CreateModel.MainClass);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO BorrowSecondClassMang
                                               (Text
                                               ,MainClass
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@Text
                                               ,@MainClass
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(BorrowSecondClassMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", vm.EditModel.ID);
            parameters.Add("@MainClass", vm.EditModel.MainClass);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@Text", vm.EditModel.Text);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE BorrowSecondClassMang 
                                       SET Text = @Text, MainClass = @MainClass , Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
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

            string CommendText = $@"DELETE FROM BorrowSecondClassMang WHERE ID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetddlMainClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM BorrowMainClassMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
