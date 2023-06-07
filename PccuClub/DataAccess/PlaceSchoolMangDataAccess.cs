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
    
    public class PlaceSchoolMangDataAccess : BaseAccess
    {
        PublicFun PublicFun = new PublicFun();

        /// <summary> 查詢結果 </summary>

        public List<PlaceSchoolMangResultModel> GetSearchResult(PlaceSchoolMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            
            parameters.Add("@PlaceStatus", model?.PlaceStatus);
            parameters.Add("@BuildName", model?.BuildName);
            parameters.Add("@PlaceId", model?.PlaceId);
            parameters.Add("@PlaceName", model?.PlaceName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
           
            #endregion

            CommandText = $@"SELECT A.PlaceID, A.PlaceName, A.Buildid, B.BuildName, A.Floor, A.PlaceDesc, A.Capacity, A.PlaceEquip, A.PlaceStatus, C.Text AS PlaceStatusText, 
                                    A.Memo, A.Normal_STime, A.Normal_ETime, A.Holiday_STime, A.Holiday_ETime, A.LastModified
                               FROM PlaceSchoolMang A
                          LEFT JOIN BuildMang B ON B.BuildID = A.Buildid
                          LEFT JOIN Code C ON C.Code = A.PlaceStatus AND C.Type = 'PlaceStatus'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
AND (@PlaceStatus IS NULL OR A.PlaceStatus = @PlaceStatus)
AND (@BuildName IS NULL OR B.BuildName LIKE '%' + @BuildName + '%') 
AND (@PlaceId IS NULL OR A.PlaceId LIKE '%' + @PlaceId + '%') 
AND (@PlaceName IS NULL OR A.PlaceName LIKE '%' + @PlaceName + '%') ";


            (DbExecuteInfo info, IEnumerable<PlaceSchoolMangResultModel> entitys) dbResult = DbaExecuteQuery<PlaceSchoolMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PlaceSchoolMangResultModel>();
        }

        /// <summary>取得編輯資料</summary>
        public PlaceSchoolMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@PlaceID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.PlaceID, A.PlaceName, A.Buildid, B.BuildName, A.Floor, A.PlaceDesc, A.Capacity, A.PlaceEquip, A.PlaceStatus, A.Memo, 
	                                A.Normal_STime, A.Normal_ETime, A.Holiday_STime, A.Holiday_ETime, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM PlaceSchoolMang A
                          LEFT JOIN BuildMang B ON B.Buildid = A.Buildid
                              WHERE 1 = 1
                                AND (A.PlaceID = @PlaceID) ";


            (DbExecuteInfo info, IEnumerable<PlaceSchoolMangEditModel> entitys) dbResult = DbaExecuteQuery<PlaceSchoolMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary>取得批次借用/關閉資料</summary>
        public PlaceSchoolMangBatchAddActModel GetBatchAddActData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@PlaceID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.BuildiD, A.PlaceID
                               FROM PlaceSchoolMang A
                              WHERE 1 = 1
                                AND (A.PlaceID = @PlaceID) ";


            (DbExecuteInfo info, IEnumerable<PlaceSchoolMangBatchAddActModel> entitys) dbResult = DbaExecuteQuery<PlaceSchoolMangBatchAddActModel>(CommandText, parameters, true, DBAccessException);

            return dbResult.entitys.ToList().FirstOrDefault();
        }

        /// <summary> 新增資料</summary>
        public DbExecuteInfo InsertData(PlaceSchoolMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PlaceID", vm.CreateModel.PlaceID);
            parameters.Add("@PlaceName", vm.CreateModel.PlaceName);
            parameters.Add("@Buildid", vm.CreateModel.BuildId);
            parameters.Add("@Floor", vm.CreateModel.Floor);
            parameters.Add("@PlaceDesc", vm.CreateModel.PlaceDesc);
            parameters.Add("@Capacity", vm.CreateModel.Capacity);
            parameters.Add("@PlaceEquip", vm.CreateModel.PlaceEquip);
            parameters.Add("@PlaceStatus", vm.CreateModel.PlaceStatus);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@Normal_STime", string.Format("{0}", vm.CreateModel.Normal_STime));
            parameters.Add("@Normal_ETime", string.Format("{0}", vm.CreateModel.Normal_ETime));
            parameters.Add("@Holiday_STime", string.Format("{0}", vm.CreateModel.Holiday_STime));
            parameters.Add("@Holiday_ETime", string.Format("{0}", vm.CreateModel.Holiday_ETime));
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO PlaceSchoolMang
                                               (PlaceID 
                                                ,PlaceName 
                                                ,Buildid 
                                                ,Floor 
                                                ,PlaceDesc 
                                                ,Capacity 
                                                ,PlaceEquip 
                                                ,PlaceStatus 
                                                ,Memo 
                                                ,Normal_STime 
                                                ,Normal_ETime 
                                                ,Holiday_STime 
                                                ,Holiday_ETime 
                                                ,Creator 
                                                ,Created 
                                                ,LastModifier 
                                                ,LastModified 
                                                ,ModifiedReason )
                                         VALUES
                                               (@PlaceID 
                                                ,@PlaceName 
                                                ,@Buildid 
                                                ,@Floor 
                                                ,@PlaceDesc 
                                                ,@Capacity 
                                                ,@PlaceEquip 
                                                ,@PlaceStatus 
                                                ,@Memo 
                                                ,@Normal_STime 
                                                ,@Normal_ETime 
                                                ,@Holiday_STime 
                                                ,@Holiday_ETime 
                                                ,@LoginId
                                                ,GETDATE()
                                                ,@LoginId
                                                ,GETDATE()
                                                ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料</summary>
        public DbExecuteInfo UpdateData(PlaceSchoolMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PlaceID", vm.EditModel.PlaceID);
            parameters.Add("@PlaceName", vm.EditModel.PlaceName);
            parameters.Add("@Buildid", vm.EditModel.BuildId);
            parameters.Add("@Floor", vm.EditModel.Floor);
            parameters.Add("@PlaceDesc", vm.EditModel.PlaceDesc);
            parameters.Add("@Capacity", vm.EditModel.Capacity);
            parameters.Add("@PlaceEquip", vm.EditModel.PlaceEquip);
            parameters.Add("@PlaceStatus", vm.EditModel.PlaceStatus);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@Normal_STime", string.Format("{0}", vm.EditModel.Normal_STime));
            parameters.Add("@Normal_ETime", string.Format("{0}", vm.EditModel.Normal_ETime));
            parameters.Add("@Holiday_STime", string.Format("{0}", vm.EditModel.Holiday_STime));
            parameters.Add("@Holiday_ETime", string.Format("{0}", vm.EditModel.Holiday_ETime));
            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE PlaceSchoolMang 
                                       SET PlaceName = @PlaceName
                                            ,Buildid = @Buildid
                                            ,Floor = @Floor
                                            ,PlaceDesc = @PlaceDesc
                                            ,Capacity = @Capacity
                                            ,PlaceEquip = @PlaceEquip
                                            ,PlaceStatus = @PlaceStatus
                                            ,Memo = @Memo
                                            ,Normal_STime = @Normal_STime
                                            ,Normal_ETime = @Normal_ETime
                                            ,Holiday_STime = @Holiday_STime
                                            ,Holiday_ETime = @Holiday_ETime
                                            ,LastModifier = @LastModifier
                                            ,LastModified = GETDATE()
                                     WHERE PlaceID = @PlaceID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>刪除資料</summary>
        public DbExecuteInfo DeletetData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PlaceID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM PlaceSchoolMang WHERE PlaceID = @PlaceID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 新增批次資料</summary>
        public DbExecuteInfo InsertActMainData(PlaceSchoolMangViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SDate", vm.BatchAddActModel.SDate);
            parameters.Add("@EDate", vm.BatchAddActModel.EDate);
            parameters.Add("@STime", vm.BatchAddActModel.STime);
            parameters.Add("@ETime", vm.BatchAddActModel.ETime);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActMain
                                               (SDate 
                                                ,EDate 
                                                ,STime 
                                                ,ETime 
                                                ,ActVerify 
                                                ,Creator 
                                                ,Created 
                                                ,LastModifier 
                                                ,LastModified )
                                         OUTPUT Inserted.ActID
                                         VALUES
                                                (@SDate 
                                                ,@EDate 
                                                ,@STime 
                                                ,@ETime
                                                ,'05'
                                                ,@LoginId
                                                ,GETDATE()
                                                ,@LoginId
                                                ,GETDATE()) ";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);
            
            dt = ds.Tables[0];

            return ExecuteResult;

        }

        public DbExecuteInfo InsertActDetailData(PlaceSchoolMangViewModel vm, string ActId, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActID", ActId);
            parameters.Add("@SchoolYear", PublicFun.GetSchoolYear(vm.BatchAddActModel.SDate));
            parameters.Add("@BuildID", vm.BatchAddActModel.BuildId);
            parameters.Add("@PlaceID", vm.BatchAddActModel.PlaceID); 
            parameters.Add("@ActName", vm.BatchAddActModel.Reason);
            parameters.Add("@Memo", vm.BatchAddActModel.Memo);
            parameters.Add("@BorrowType", vm.BatchAddActModel.BorrowType);
            parameters.Add("@Capacity", "0");
            
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActDetail
                                               (ActID, 
                                                SchoolYear, 
                                                BuildID, 
                                                PlaceID, 
                                                ActName, 
                                                BorrowType, 
                                                BrrowUnit, 
                                                Capacity, 
                                                CreateSource, 
                                                Memo, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         OUTPUT Inserted.ActDetailId
                                         VALUES
                                               (@ActID, 
                                                @SchoolYear, 
                                                @BuildID, 
                                                @PlaceID, 
                                                @ActName, 
                                                @BorrowType, 
                                                @LoginId, 
                                                @Capacity, 
                                                '01', 
                                                @Memo, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE() )";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        /// <summary> 新增日期資料</summary>
        public DbExecuteInfo InsertActSectionData(PlaceSchoolMangViewModel vm, string ActId, string ActDetailId, DateTime date, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActID", ActId);
            parameters.Add("@ActDetailId", ActDetailId);
            parameters.Add("@Date", date.ToString("yyyy-MM-dd"));
            parameters.Add("@Week", date.ToString("dddd"));
            parameters.Add("@Status", "01");
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActSection
                                               (ActID
                                                , ActDetailId
                                                , Date
                                                , Creator
                                                , Created
                                                , LastModifier
                                                , LastModified )
                                         OUTPUT Inserted.ActSectionId
                                         VALUES
                                               (@ActID
                                                , @ActDetailId
                                                , @Date
                                                , @LoginId
                                                , GETDATE()
                                                , @LoginId
                                                , GETDATE() )";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        /// <summary> 新增批次行程資料</summary>
        public DbExecuteInfo InsertActRundownData(PlaceSchoolMangViewModel vm, string ActId, string ActDetailId, string ActSectionId, DateTime date, int hour, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActID", ActId);
            parameters.Add("@ActDetailId", ActDetailId);
            parameters.Add("@ActSectionId", ActSectionId);
            parameters.Add("@ActPlaceID", vm.BatchAddActModel.PlaceID);
            parameters.Add("@Date", date.ToString("yyyy-MM-dd"));
            parameters.Add("@Stime", hour.ToString().PadLeft(2, '0'));
            parameters.Add("@ETime", (hour + 1).ToString().PadLeft(2, '0'));
            parameters.Add("@Week", date.ToString("dddd"));
            parameters.Add("@Status", "01");
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActRundown
                                               (ActID, 
                                                ActDetailId, 
                                                ActPlaceID, 
                                                Date, 
                                                STime, 
                                                ETime, 
                                                Week, 
                                                RundownStatus, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@ActID, 
                                                @ActDetailId, 
                                                @ActPlaceID, 
                                                @Date, 
                                                @STime, 
                                                @ETime, 
                                                @Week, 
                                                '01', 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        //抓取該日的所有已核准時間
        public DataTable GetRundown(string? placeID, DateTime date)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PlaceID", placeID);
            parameters.Add("@Date", date);

            #endregion

            CommandText = $@"SELECT ActRundownID, ActPlaceID, STime, ETime 
                               FROM ActRundown 
                              WHERE ActPlaceID = @PlaceID 
                                AND [Date] = @Date 
                                AND RundownStatus = '01'";


            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
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

        public List<SelectListItem> GetAllFloor()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT FloorID AS VALUE, FloorName AS TEXT FROM FloorMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
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

        public List<SelectListItem> GetAllWeek()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code where type = 'Week'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllPlaceStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'PlaceStatus'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllBorrowType()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'BorrowType'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllPlaceSchool()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT PlaceId AS VALUE, PlaceName AS TEXT FROM PlaceSchoolMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
