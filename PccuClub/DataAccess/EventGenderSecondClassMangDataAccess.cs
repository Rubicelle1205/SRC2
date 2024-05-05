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
    
    public class EventGenderSecondClassMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<EventGenderSecondClassMangResultModel> GetSearchResult(EventGenderSecondClassMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainID", model?.MainID);
            parameters.Add("@Text", model?.Text);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.MainID, B.Text AS MainIDText, A.CaseSystemType, A.Memo, A.Creator, A.Text, A.Created, A.LastModifier, A.LastModified
FROM EventSecondClassMang A
LEFT JOIN EventMainClassMang B ON B.ID = A.MainID
WHERE 1 = 1
AND A.CaseSystemType = '02'
AND (@MainID IS NULL OR A.MainID LIKE '%' + @MainID + '%') 
AND (@Text IS NULL OR A.Text LIKE '%' + @Text + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<EventGenderSecondClassMangResultModel> entitys) dbResult = DbaExecuteQuery<EventGenderSecondClassMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventGenderSecondClassMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EventGenderSecondClassMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.MainID, B.Text AS MainIDText, A.CaseSystemType, A.Memo, A.Creator, A.Text, A.Created, A.LastModifier, A.LastModified
FROM EventSecondClassMang A
LEFT JOIN EventMainClassMang B ON B.ID = A.MainID
WHERE 1 = 1
AND A.CaseSystemType = '02'
AND (A.ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<EventGenderSecondClassMangEditModel> entitys) dbResult = DbaExecuteQuery<EventGenderSecondClassMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(EventGenderSecondClassMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainID", vm.CreateModel.MainID);
            parameters.Add("@Text", vm.CreateModel.Text);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO EventSecondClassMang
                                               (MainID, 
	                                            Text, 
	                                            CaseSystemType, 
	                                            Memo, 
	                                            Creator, 
	                                            Created, 
	                                            LastModifier, 
	                                            LastModified)
                                         VALUES
                                               (@MainID
	                                           ,@Text
	                                           ,'02'
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(EventGenderSecondClassMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", vm.EditModel.ID);
            parameters.Add("@MainID", vm.EditModel.MainID);
            parameters.Add("@Text", vm.EditModel.Text);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE EventSecondClassMang 
                                       SET MainID = @MainID, Text = @Text, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ID = @ID AND CaseSystemType = '02'";

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

            string CommendText = $@"DELETE FROM EventSecondClassMang WHERE ID = @ID AND CaseSystemType = '02'";

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

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventMainClassMang WHERE CaseSystemType = '02'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
