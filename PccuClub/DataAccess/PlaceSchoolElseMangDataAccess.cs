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
    
    public class PlaceSchoolElseMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<PlaceSchoolElseMangResultModel> GetSearchResult(PlaceSchoolElseMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            
            parameters.Add("@Floor", model?.Floor);
            parameters.Add("@BuildName", model?.BuildName);
            parameters.Add("@PlaceId", model?.PlaceId);
            parameters.Add("@PlaceName", model?.PlaceName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

           
            #endregion

            CommandText = $@"SELECT  A.PlaceID, A.PlaceName, A.Buildid, B.BuildName, A.Floor, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified, A.ModifiedReason
                               FROM PlaceSchoolElseMang A
                          LEFT JOIN BuildMang B ON B.BuildID = A.Buildid
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
AND (@Floor IS NULL OR A.Floor = @Floor)
AND (@BuildName IS NULL OR B.BuildName LIKE '%' + @BuildName + '%') 
AND (@PlaceId IS NULL OR A.PlaceId LIKE '%' + @PlaceId + '%') 
AND (@PlaceName IS NULL OR A.PlaceName LIKE '%' + @PlaceName + '%') ";


            (DbExecuteInfo info, IEnumerable<PlaceSchoolElseMangResultModel> entitys) dbResult = DbaExecuteQuery<PlaceSchoolElseMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PlaceSchoolElseMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public PlaceSchoolElseMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@PlaceID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT  A.PlaceID, A.PlaceName, A.Buildid, B.BuildName, A.Floor, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified, A.ModifiedReason
                               FROM PlaceSchoolElseMang A
                          LEFT JOIN BuildMang B ON B.BuildID = A.Buildid
                              WHERE 1 = 1
                                AND (A.PlaceID = @PlaceID) ";


            (DbExecuteInfo info, IEnumerable<PlaceSchoolElseMangEditModel> entitys) dbResult = DbaExecuteQuery<PlaceSchoolElseMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(PlaceSchoolElseMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@BuildId", vm.CreateModel.BuildId);
            parameters.Add("@Floor", vm.CreateModel.Floor);
            parameters.Add("@PlaceID", vm.CreateModel.PlaceID);
            parameters.Add("@PlaceName", vm.CreateModel.PlaceName);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO PlaceSchoolElseMang
                                               (PlaceID
                                               ,PlaceName 
                                               ,Buildid 
                                               ,Floor 
                                               ,Memo 
                                               ,Creator 
                                               ,Created 
                                               ,LastModifier 
                                               ,LastModified 
                                               ,ModifiedReason)
                                         VALUES
                                               (@PlaceID
                                               ,@PlaceName 
                                               ,@Buildid 
                                               ,@Floor 
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
        public DbExecuteInfo UpdateData(PlaceSchoolElseMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@BuildId", vm.EditModel.BuildId);
            parameters.Add("@Floor", vm.EditModel.Floor);
            parameters.Add("@PlaceID", vm.EditModel.PlaceID);
            parameters.Add("@PlaceName", vm.EditModel.PlaceName);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE PlaceSchoolElseMang 
                                       SET PlaceName = @PlaceName, Buildid = @Buildid, Floor = @Floor, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
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

            string CommendText = $@"DELETE FROM PlaceSchoolElseMang WHERE PlaceID = @PlaceID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetAllFloor()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            for (int i = 1; i <= 15; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}F", i) });
            }

            return LstItem;
        }

        public List<SelectListItem> GetAllBuild()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT BuildID AS VALUE, BuildName AS TEXT FROM BuildMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
