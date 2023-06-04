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
using System.Diagnostics;

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
                            SELECT A.ActID, B.ActName, B.SchoolYear, A.ActVerify, C.Text AS ActVerifyText, A.SDate, A.EDate, 
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





        public List<ActListMangPlaceUsedModel> GetPlaceUsedData(string Date)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();


            parameters.Add("@Date", Date);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT PlaceName, STime, ETime
                               FROM ActMain
                              WHERE SDate < @Date AND @Date <= EDate ";


            (DbExecuteInfo info, IEnumerable<ActListMangPlaceUsedModel> entitys) dbResult = DbaExecuteQuery<ActListMangPlaceUsedModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActListMangPlaceUsedModel>();
        }

        public List<ActListMangTodayActModel1> GetTodayAct(string PlaceSource, string Date)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@PlaceSource", PlaceSource);
            parameters.Add("@Date", Date);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT C.ActName, A.STime, A.ETime, A.BrrowClubID, CASE WHEN B.ClubCName IS NULL THEN '學務處' ELSE B.ClubCName END BrrowClubName
                               FROM ActMain A
                          LEFT JOIN ClubMang B ON B.ClubID = A.BrrowClubID 
						  LEFT JOIN ActDetail C ON C.ActID = A.ActID
                              WHERE A.SDate < @Date AND @Date <= A.EDate 
                                AND A.PlaceID = @PlaceSource ";


            (DbExecuteInfo info, IEnumerable<ActListMangTodayActModel1> entitys) dbResult = DbaExecuteQuery<ActListMangTodayActModel1>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActListMangTodayActModel1>();
        }



















        #region 取得預設資料

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

        public List<SelectListItem> GetStaticOrDynamic()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'StaticOrDynamic'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetActInOrOut()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'ActInOrOut'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetActType()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ActTypeID AS VALUE, ActTypeName AS TEXT FROM ActTypeMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetUseITEquip()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'UseITEquip'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetSDGs()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT SDGID AS VALUE, ShortName AS TEXT FROM SDGsMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetPassport()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'PassPort'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetPlaceSource()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'PlaceSource'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllHour()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            for (int i = 0; i <= 24; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString().PadLeft(2, '0'), Text = i.ToString().PadLeft(2, '0') });
            }

            return LstItem;
        }

        #endregion

        #region 取得樓館資料

        #endregion

        public List<SelectListItem> GetBuild()
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


        public List<SelectListItem> GetPlace(string PlaceSource, string Buildid)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Buildid", Buildid);

            #endregion

            if (PlaceSource == "01")
                CommandText = @"SELECT PlaceID AS VALUE, PlaceName AS TEXT FROM PlaceSchoolMang WHERE Buildid = @Buildid";
            else
                CommandText = @"SELECT PlaceID AS VALUE, PlaceName AS TEXT FROM PlaceSchoolElseMang WHERE Buildid = @Buildid";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<ActListMangPlaceDataModel> GetPlaceData(string PlaceSource, string PlaceId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PlaceId", PlaceId);

            #endregion

            if (PlaceSource == "01")
            {
                CommandText = @"SELECT A.PlaceID, A.PlaceName, A.Capacity, A.PlaceEquip, A.PlaceStatus, B.Text AS PlaceStatusText, A.Memo, 
                                       A.Normal_STime, A.Normal_ETime, A.Holiday_STime, A.Holiday_ETime
                                  FROM PlaceSchoolMang A
                             LEFT JOIN Code B ON B.Code = A.PlaceStatus AND B.Type = 'PlaceStatus'
                                 WHERE A.PlaceId = @PlaceId";
            }

            else
            {
                CommandText = @"SELECT A.PlaceID, A.PlaceName, A.Memo
                                  FROM PlaceSchoolElseMang A
                                 WHERE A.PlaceId = @PlaceId";
            }

            (DbExecuteInfo info, IEnumerable<ActListMangPlaceDataModel> entitys) dbResult = DbaExecuteQuery<ActListMangPlaceDataModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActListMangPlaceDataModel>();
        }


        public bool ChkPlaceSchoolCanUse(ActListMangViewModel vm)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            bool IsHoliday = false;
            string CommendText = string.Empty;

            string PlaceSource = vm.RundownModel.PlaceSource;
            string dayOfWeek = DateTime.Parse(vm.RundownModel.Date).ToString("dddd");

            if (dayOfWeek == "星期六" || dayOfWeek == "星期日")
            {
                IsHoliday = true;
            }

            #region 參數設定
            parameters.Add("@PlaceID", vm.RundownModel.PlaceID);
            parameters.Add("@Date", vm.RundownModel.Date);
            parameters.Add("@STime", vm.RundownModel.STime);
            parameters.Add("@ETime", vm.RundownModel.ETime);
            parameters.Add("@PlaceStatus", "01");   //可借用

            #endregion

            if (PlaceSource == "01")
            {
                CommendText = $@"SELECT * 
                                   FROM PlaceSchoolMang
                                  WHERE PlaceID = @PlaceID 
                                    AND PlaceStatus = @PlaceStatus 
{(IsHoliday ? "AND Holiday_STime <= @STime AND @ETime < Holiday_ETime": "AND Normal_STime <= @STime AND @ETime < Normal_ETime")} 
";
            }

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ds.Tables[0].Rows.Count > 0;
        }

        public bool ChkHasAct(ActListMangViewModel vm)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommandText = string.Empty;

            parameters.Add("@PlaceSource", vm.RundownModel.PlaceID);
            parameters.Add("@Date", vm.RundownModel.Date);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT *
                               FROM ActMain A
                          LEFT JOIN ClubMang B ON B.ClubID = A.BrrowClubID 
						  LEFT JOIN ActDetail C ON C.ActID = A.ActID
                              WHERE A.SDate < @Date AND @Date <= A.EDate 
                                AND A.PlaceID = @PlaceSource ";
           
            ExecuteResult = DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return !(ds.Tables[0].Rows.Count > 0);
        }
    }
}
