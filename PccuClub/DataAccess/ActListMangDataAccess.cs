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
    
    public class ActListMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ActListMangResultModel> GetSearchResult(ActListMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActId", model?.ActId);
            parameters.Add("@ActName", model?.ActName);
            parameters.Add("@ClubName", model?.ClubName);  //
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@LifeClass", model?.LifeClass);  //
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT A.ActID, A.ActName, B.SchoolYear, A.ActVerify, C.Text AS ActVerifyText, A.SDate, A.EDate, 
                                   A.BuildId, A.PlaceID, A.PlaceName, A.SDate, A.EDate, A.Created,
                         CASE WHEN A.ActVerify = '05' THEN C.Text + '(' + A.Creator + ')'
                               END ClubName
                              FROM ActMain A
                         LEFT JOIN ActDetail B ON B.ActId = A.ActID
                         LEFT JOIN Code C ON C.Code = A.ActVerify AND C.Type = 'ActVerify'
                             WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
AND (@ActId IS NULL OR A.ActId = @ActId)
AND (@SchoolYear IS NULL OR B.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR A.ActVerify = @ActVerify)
AND (@ActName IS NULL OR A.ActName LIKE '%' + @ActName + '%') 

";


            (DbExecuteInfo info, IEnumerable<ActListMangResultModel> entitys) dbResult = DbaExecuteQuery<ActListMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActListMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ActListMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActTypeID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ActTypeID, ActTypeName, Memo, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM ActListMang
WHERE 1 = 1
AND (ActTypeID = @ActTypeID) ";


            (DbExecuteInfo info, IEnumerable<ActListMangEditModel> entitys) dbResult = DbaExecuteQuery<ActListMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ActListMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            //parameters.Add("@ActTypeName", vm.CreateModel.ActTypeName);
            //parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActListMang
                                               (ActTypeName
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ActTypeName
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
        public DbExecuteInfo UpdateData(ActListMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            //parameters.Add("@ActTypeID", vm.EditModel.ActTypeID);
            //parameters.Add("@ActTypeName", vm.EditModel.ActTypeName);
            //parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ActListMang 
                                       SET ActTypeName = @ActTypeName, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ActTypeID = @ActTypeID";

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
            parameters.Add("@ActTypeID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM ActListMang WHERE ActTypeID = @ActTypeID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// Excel 取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ActListMangResultModel> GetExportResult(ActListMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            //parameters.Add("@ActTypeName", model?.ActTypeName);
            //parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ActTypeID, ActTypeName, Memo, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM ActListMang
WHERE 1 = 1
AND (@ActTypeName IS NULL OR ActTypeName LIKE '%' + @ActTypeName + '%') 
AND (@Memo IS NULL OR Memo LIKE '%' + @Memo + '%') ";

            (DbExecuteInfo info, IEnumerable<ActListMangResultModel> entitys) dbResult = DbaExecuteQuery<ActListMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActListMangResultModel>();
        }

            public List<SelectListItem> GetAllActVerify()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'ActVerify'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllLifeClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'LifeClass'";

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
