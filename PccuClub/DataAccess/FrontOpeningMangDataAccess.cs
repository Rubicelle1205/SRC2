using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using NPOI.SS.Formula.Functions;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class FrontOpeningMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<FrontOpeningMangResultModel> GetSearchResult(FrontOpeningMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MenuName", model?.MenuName);
            parameters.Add("@Enable", model?.Enable);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.FrontOpeningId, A.MenuNode, A.MenuName, A.Enable, B.Text AS EnableText, 
                                    A.OpenDate, A.CloseDate, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM FrontOpeningMang A
                               LEFT JOIN Code B ON B.Code = A.Enable AND B.Type = 'Enable'
                              WHERE 1 = 1
                                AND (@MenuName IS NULL OR MenuName LIKE '%' + @MenuName + '%') 
                                AND (@Enable IS NULL OR A.Enable = @Enable)" ;


            (DbExecuteInfo info, IEnumerable<FrontOpeningMangResultModel> entitys) dbResult = DbaExecuteQuery<FrontOpeningMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FrontOpeningMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public FrontOpeningMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@FrontOpeningId", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT FrontOpeningId, MenuNode, MenuName, Enable, 
                                    OpenDate, CloseDate, Creator, Created, LastModifier, LastModified
                               FROM FrontOpeningMang
                              WHERE 1 = 1
AND (FrontOpeningId = @FrontOpeningId) ";


            (DbExecuteInfo info, IEnumerable<FrontOpeningMangEditModel> entitys) dbResult = DbaExecuteQuery<FrontOpeningMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(FrontOpeningMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@FrontOpeningId", vm.EditModel.FrontOpeningId);
            parameters.Add("@Enable", vm.EditModel.Enable);
            parameters.Add("@OpenDate", vm.EditModel.OpenDate);
            parameters.Add("@CloseDate", vm.EditModel.CloseDate);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE FrontOpeningMang 
                                       SET Enable = @Enable, OpenDate = @OpenDate, CloseDate = @CloseDate, 
                                           LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE FrontOpeningId = @FrontOpeningId";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

    }
}
