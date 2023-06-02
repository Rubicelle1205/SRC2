using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.ConstrainedExecution;

namespace WebPccuClub.DataAccess
{
    
    public class HandOverMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<HandOverMangResultModel> GetSearchResult(HandOverMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SchoolYear", model.SchoolYear);
            parameters.Add("@ClubID", model.ClubID);
            parameters.Add("@ClubCName", model.ClubCName);
            parameters.Add("@HandOverStatus", model.HandOverStatus);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.HoID, A.ClubID, B.ClubCName, A.SchoolYear, A.HandOverStatus, C.Text AS HandOverStatusText, A.Created
                               FROM HandOverMain A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                          LEFT JOIN Code C ON C.Code = A.HandOverStatus AND C.Type = 'HandOverStatus'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@HandOverStatus IS NULL OR A.HandOverStatus = @HandOverStatus)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR B.ClubCName LIKE '%' + @ClubCName + '%') ";


            (DbExecuteInfo info, IEnumerable<HandOverMangResultModel> entitys) dbResult = DbaExecuteQuery<HandOverMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HandOverMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HandOverMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@HoID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.HoID, A.ClubID, B.ClubCName, A.SchoolYear, A.HandOverStatus, A.Created
                               FROM HandOverMain A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                              WHERE 1 = 1
                                AND (A.HoID = @HoID) ";


            (DbExecuteInfo info, IEnumerable<HandOverMangEditModel> entitys) dbResult = DbaExecuteQuery<HandOverMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(HandOverMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", vm.EditModel.HoID);
            parameters.Add("@LoginId", LoginUser.LoginId);
            parameters.Add("@HandOverStatus", vm.EditModel.HandOverStatus);
            #endregion 參數設定

            string CommendText = $@" UPDATE HandOverMain SET HandOverStatus = @HandOverStatus, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE HoID = @HoID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetAllHandOverStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'HandOverStatus'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            for (int i = 108; i <= 130; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
        }
    }
}
