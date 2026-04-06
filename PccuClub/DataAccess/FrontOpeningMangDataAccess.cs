using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using NPOI.SS.Formula.Functions;
using PccuClub.WebAuth;
using System.Data;
using System.Runtime.ConstrainedExecution;
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

        /// <summary>取得時段資料</summary>
        public List<HourTimeFrame> GetTimeFrameData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@FrontOpeningId", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT FrontOpeningDetailId, FrontOpeningId, DayOfWeek, HourMask
                               FROM FrontOpeningDetailMang
                              WHERE 1 = 1
AND (FrontOpeningId = @FrontOpeningId) ";


            (DbExecuteInfo info, IEnumerable<HourTimeFrame> entitys) dbResult = DbaExecuteQuery<HourTimeFrame>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HourTimeFrame>();
        }

        /// <summary> 修改資料1  </summary>
        public DbExecuteInfo UpdateFrontOpeningMangData(FrontOpeningMangViewModel vm, UserInfo LoginUser)
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

        /// <summary>
        /// 刪除資料
        /// </summary>
        public DbExecuteInfo DeleteFrontOpeningDetailMangData(FrontOpeningMangViewModel vm)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@FrontOpeningId", vm.EditModel.FrontOpeningId);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM FrontOpeningDetailMang WHERE FrontOpeningId = @FrontOpeningId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料2 </summary>
        public DbExecuteInfo InsertFrontOpeningDetailMangData(FrontOpeningMangViewModel vm, HourTimeFrame timeFrame, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@FrontOpeningId", vm.EditModel.FrontOpeningId);
            parameters.Add("@DayOfWeek", timeFrame.DayOfWeek);
            parameters.Add("@HourMask", timeFrame.HourMask);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO FrontOpeningDetailMang
                                               (FrontOpeningId
                                              ,DayOfWeek
                                              ,HourMask
                                              ,Creator
                                              ,Created
                                              ,LastModifier
                                              ,LastModified)
                                         VALUES
                                               (@FrontOpeningId
                                               ,@DayOfWeek
                                               ,@HourMask
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
        

    }
}
