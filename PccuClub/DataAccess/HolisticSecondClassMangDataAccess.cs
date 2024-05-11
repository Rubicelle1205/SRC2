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
    
    public class HolisticSecondClassMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<HolisticSecondClassMangResultModel> GetSearchResult(HolisticSecondClassMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@Text", model?.Text);
            parameters.Add("@Memo", model?.Memo);

            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.Text, A.MainID, B.Text AS MainIDText, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified
FROM HolisticSecondClassMang A
LEFT JOIN HolisticMainClassMang B ON B.ID = A.MainID
WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@Text IS NULL OR A.Text LIKE '%' + @Text + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<HolisticSecondClassMangResultModel> entitys) dbResult = DbaExecuteQuery<HolisticSecondClassMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HolisticSecondClassMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HolisticSecondClassMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.Text, A.MainID, B.Text AS MainIDText, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified
FROM HolisticSecondClassMang A
LEFT JOIN HolisticMainClassMang B ON B.ID = A.MainID
WHERE 1 = 1
AND (A.ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<HolisticSecondClassMangEditModel> entitys) dbResult = DbaExecuteQuery<HolisticSecondClassMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(HolisticSecondClassMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainID", vm.CreateModel.MainID);
            parameters.Add("@Text", vm.CreateModel.Text);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO HolisticSecondClassMang
                                               (MainID
                                               ,Text
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@MainID
                                               ,@Text
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(HolisticSecondClassMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", vm.EditModel.ID);
            parameters.Add("@MainID", vm.EditModel.MainID);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@Text", vm.EditModel.Text);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE HolisticSecondClassMang 
                                       SET Text = @Text, MainID = @MainID, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
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

            string CommendText = $@"DELETE FROM HolisticSecondClassMang WHERE ID = @ID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetddlHolisticMainClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM HolisticMainClassMang ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
