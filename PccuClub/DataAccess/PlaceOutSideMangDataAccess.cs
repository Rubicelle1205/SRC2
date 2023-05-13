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

namespace WebPccuClub.DataAccess
{
    
    public class PlaceOutSideMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<PlaceOutSideMangResultModel> GetSearchResult(PlaceOutSideMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@PlaceName", model?.PlaceName);
            parameters.Add("@CityCode", model?.CityCode);
            parameters.Add("@PlaceType", model?.PlaceType);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

           
            #endregion

            CommandText = $@"
SELECT  A.PlaceID, A.PlaceName, A.PlaceType, B.Text AS PlaceTypeName, A.CityCode, C.Text AS CityCodeName, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified, A.ModifiedReason
FROM PlaceOutSideMang A 
LEFT JOIN Code B ON B.Code = A.PlaceType AND B.Type = 'Placetype'
LEFT JOIN Code C ON C.Code = A.CityCode AND C.Type = 'CityCode'
WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
AND (@PlaceName IS NULL OR A.PlaceName LIKE '%' + @PlaceName + '%') 
AND (@CityCode IS NULL OR A.CityCode = @CityCode)
AND (@PlaceType IS NULL OR A.PlaceType = @PlaceType) ";


            (DbExecuteInfo info, IEnumerable<PlaceOutSideMangResultModel> entitys) dbResult = DbaExecuteQuery<PlaceOutSideMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PlaceOutSideMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public PlaceOutSideMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@PlaceID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT PlaceID, PlaceName, PlaceType, CityCode, Memo, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM PlaceOutSideMang
WHERE 1 = 1
AND (PlaceID = @PlaceID) ";


            (DbExecuteInfo info, IEnumerable<PlaceOutSideMangEditModel> entitys) dbResult = DbaExecuteQuery<PlaceOutSideMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(PlaceOutSideMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PlaceName", vm.CreateModel.PlaceName);
            parameters.Add("@PlaceType", vm.CreateModel.PlaceType);
            parameters.Add("@CityCode", vm.CreateModel.CityCode);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO PlaceOutSideMang
                                               (PlaceName
                                               ,PlaceType
                                               ,CityCode
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@PlaceName
                                               ,@PlaceType
                                               ,@CityCode
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
        public DbExecuteInfo UpdateData(PlaceOutSideMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PlaceID", vm.EditModel.PlaceID);
            parameters.Add("@PlaceName", vm.EditModel.PlaceName);
            parameters.Add("@PlaceType", vm.EditModel.PlaceType);
            parameters.Add("@CityCode", vm.EditModel.CityCode);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE PlaceOutSideMang 
                                       SET PlaceName = @PlaceName, PlaceType = @PlaceType, CityCode = @CityCode, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE PlaceID = @PlaceID";

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
            parameters.Add("@PlaceID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM PlaceOutSideMang WHERE PlaceID = @PlaceID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 匯入資料
        /// </summary>
        /// <param name="lstExcel"></param>
        /// <param name="loginUser"></param>
        /// <exception cref="NotImplementedException"></exception>
        public DbExecuteInfo ImportData(List<PlaceOutSideMangExcelResultModel> dataList, UserInfo loginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO PlaceOutSideMang
                                               (PlaceName
                                               ,PlaceType
                                               ,CityCode
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@PlaceName
                                               ,@PlaceType
                                               ,@CityCode
                                               ,''
                                               ,'{loginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{loginUser.LoginId}'
                                               ,GETDATE()
                                               ,NULL) ";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);
     
            return ExecuteResult;
        }

        /// <summary>
        /// Excel 取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<PlaceOutSideMangResultModel> GetExportResult(PlaceOutSideMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@PlaceName", model?.PlaceName);
            parameters.Add("@CityCode", model?.CityCode);
            parameters.Add("@PlaceType", model?.PlaceType);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);


            #endregion

            CommandText = $@"
SELECT  A.PlaceID, A.PlaceName, A.PlaceType, B.Text AS PlaceTypeName, A.CityCode, C.Text AS CityCodeName, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified, A.ModifiedReason
FROM PlaceOutSideMang A 
LEFT JOIN Code B ON B.Code = A.PlaceType AND B.Type = 'Placetype'
LEFT JOIN Code C ON C.Code = A.CityCode AND C.Type = 'CityCode'
WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
AND (@PlaceName IS NULL OR A.PlaceName LIKE '%' + @PlaceName + '%') 
AND (@CityCode IS NULL OR A.CityCode = @CityCode)
AND (@PlaceType IS NULL OR A.PlaceType = @PlaceType) ";

            (DbExecuteInfo info, IEnumerable<PlaceOutSideMangResultModel> entitys) dbResult = DbaExecuteQuery<PlaceOutSideMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PlaceOutSideMangResultModel>();
        }

        public List<SelectListItem> GetAllCityCode()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'CityCode'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllPlaceType()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'Placetype'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
