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
using System.Runtime.ConstrainedExecution;

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

            CommandText = $@"SELECT A.ActID, A.SchoolYear, A.ActName, B.ActVerify, C.Text AS ActVerifyText, A.BrrowUnit, 
                                    CASE WHEN B.ActVerify = '05' THEN C.Text + '(' + B.Creator + ')' ELSE D.ClubCName END ClubName, 
	                                F.MinDate AS SDate, F.MaxDate AS EDate, A.Created
                               FROM ActDetail A
                          LEFT JOIN ActMain B ON B.ActID = A.ActID
                          LEFT JOIN Code C ON C.Code = B.ActVerify AND C.Type = 'ActVerify'
                          LEFT JOIN ClubMang D ON D.ClubId = A.BrrowUnit
                          LEFT JOIN ActSection E ON E.ActDetailId = A.ActDetailId
                          LEFT JOIN (SELECT ActID, MIN(Date) AS MinDate, MAX(Date) AS MaxDate FROM ActSection GROUP BY ActID) F ON F.ActID = B.ActID
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@ActId IS NULL OR A.ActId = @ActId)
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR B.ActVerify = @ActVerify)
AND (@ActName IS NULL OR A.ActName LIKE '%' + @ActName + '%') 
GROUP BY A.ActID, A.SchoolYear, A.ActName, B.ActVerify, C.Text, A.BrrowUnit, F.MinDate, F.MaxDate, B.Creator, D.ClubCName, A.Created";


            (DbExecuteInfo info, IEnumerable<ActListMangResultModel> entitys) dbResult = DbaExecuteQuery<ActListMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActListMangResultModel>();
        }

        #region 取得編輯資料

        /// <summary>取得編輯資料</summary>
        public ActListMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ActId", Ser);

            #endregion

            CommandText = $@"SELECT A.ActDetailId, A.ActID, A.BrrowUnit AS ClubID, B.ClubCName AS BrrowUnitText, A.SchoolYear, A.ActName, 
	                                A.Capacity, A.ActType, C.ActTypeName, A.SDGs, A.StaticOrDynamic, D.Text AS StaticOrDynamicText,
	                                A.ActInOrOut, E.Text AS ActInOrOutText, A.UseITEquip, F.Text AS UseITEquipText, A.PassPort, G.Text AS PassPortText,
	                                A.ShortDesc, A.Created, A.LastModified, H.ActVerify, H.ActVerifyMemo,
									I.LeaderName, I.LeaderTel, I.LeaderPhone, I.ManagerName, I.ManagerTel, I.ManagerPhone
                               FROM ActDetail A
                          LEFT JOIN ClubMang B ON B.ClubId = A.BrrowUnit
                          LEFT JOIN ActTypeMang C ON C.ActTypeID = A.ActType
                          LEFT JOIN Code D ON D.Code = A.StaticOrDynamic AND D.Type = 'StaticOrDynamic'
                          LEFT JOIN Code E ON E.Code = A.ActInOrOut AND E.Type = 'ActInOrOut'
                          LEFT JOIN Code F ON F.Code = A.UseITEquip AND F.Type = 'UseITEquip'
                          LEFT JOIN Code G ON G.Code = A.PassPort AND G.Type = 'PassPort'
                          LEFT JOIN ActMain H ON H.ActId = A.ActId
                          LEFT JOIN ActOutSideInfo I ON I.ActID = A.ActID AND I.ActDetailId = I.ActDetailId
                              WHERE 1 = 1 AND A.ActID = @ActID";


            (DbExecuteInfo info, IEnumerable<ActListMangEditModel> entitys) dbResult = DbaExecuteQuery<ActListMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }


        #endregion

        #region 新增

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertActMainData(ActListMangViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@STime", vm.CreateModel.STime);
            parameters.Add("@ETime", vm.CreateModel.ETime);
            parameters.Add("@ActVerify", vm.CreateModel.ActVerify);
            parameters.Add("@ActVerifyMemo", vm.CreateModel.ActVerifyMemo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActMain
                                               (STime 
                                                ,ETime 
                                                ,ActVerify 
                                                ,ActVerifyMemo 
                                                ,Creator 
                                                ,Created 
                                                ,LastModifier 
                                                ,LastModified )
                                         OUTPUT Inserted.ActID
                                         VALUES
                                                (@STime 
                                                ,@ETime
                                                ,@ActVerify
                                                ,@ActVerifyMemo
                                                ,@LoginId
                                                ,GETDATE()
                                                ,@LoginId
                                                ,GETDATE()) ";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        public DbExecuteInfo InsertActDetailData(ActListMangViewModel vm, string ActId, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActID", ActId);
            parameters.Add("@BrrowUnit", vm.CreateModel.ClubId);
            parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@ActName", vm.CreateModel.ActName);
            parameters.Add("@Buildid", vm.CreateModel.Buildid);
            parameters.Add("@PlaceID", vm.CreateModel.PlaceId);
            parameters.Add("@BorrowType", "01"); // 借用:01 關閉:02
            parameters.Add("@Capacity", vm.CreateModel.Capacity);
            parameters.Add("@ActType", vm.CreateModel.ActType);
            parameters.Add("@SDGs", vm.CreateModel.SDGs);
            parameters.Add("@StaticOrDynamic", vm.CreateModel.StaticOrDynamic);
            parameters.Add("@ActInOrOut", vm.CreateModel.ActInOrOut);
            parameters.Add("@UseITEquip", vm.CreateModel.UseITEquip);
            parameters.Add("@PassPort", vm.CreateModel.PassPort);

            parameters.Add("@CreateSource", "01"); // 後台:01 前台:02
            parameters.Add("@ShortDesc", vm.CreateModel.ActShortDesc);


            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActDetail
                                               (ActID, 
                                                BrrowUnit, 
                                                SchoolYear, 
                                                ActName, 
                                                Buildid, 
                                                PlaceID, 
                                                BorrowType, 
                                                Capacity, 
                                                ActType, 
                                                SDGs, 
                                                StaticOrDynamic, 
                                                ActInOrOut, 
                                                UseITEquip, 
                                                PassPort, 
                                                CreateSource, 
                                                ShortDesc, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         OUTPUT Inserted.ActDetailId
                                         VALUES
                                               (@ActID, 
                                                @BrrowUnit, 
                                                @SchoolYear, 
                                                @ActName, 
                                                @Buildid, 
                                                @PlaceID, 
                                                @BorrowType, 
                                                @Capacity, 
                                                @ActType, 
                                                @SDGs, 
                                                @StaticOrDynamic, 
                                                @ActInOrOut, 
                                                @UseITEquip, 
                                                @PassPort, 
                                                @CreateSource, 
                                                @ShortDesc, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE() )";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        /// <summary> 新增日期資料</summary>
        public DbExecuteInfo InsertActSectionData(ActListMangViewModel vm, string ActId, string ActDetailId, DateTime date, UserInfo LoginUser, out DataTable dt)
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
        public DbExecuteInfo InsertActRundownData(ActListMangViewModel vm, string ActId, string ActDetailId, string ActSectionId, ActListMangRundownModel RundownModel, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            string PlaceSource = RundownModel.PlaceSource;
            string PlaceID = RundownModel.PlaceID;
            string PlaceText = RundownModel.PlaceText;

            if (PlaceSource == "01")
            {
                for (int i = 0; i <= RundownModel.LstStime.Count - 1; i++)
                {
                    int hour = RundownModel.LstStime[i];

                    #region 參數設定
                    parameters.Add("@ActID", ActId);
                    parameters.Add("@ActDetailId", ActDetailId);
                    parameters.Add("@ActSectionId", ActSectionId);
                    parameters.Add("@ActPlaceID", PlaceID);
                    parameters.Add("@ActPlaceText", PlaceText);
                    parameters.Add("@PlaceSource", PlaceSource);
                    parameters.Add("@Date", DateTime.Parse(RundownModel.Date).ToString("yyyy-MM-dd"));
                    parameters.Add("@Stime", hour.ToString().PadLeft(2, '0'));
                    parameters.Add("@ETime", (hour + 1).ToString().PadLeft(2, '0'));
                    parameters.Add("@Week", DateTime.Parse(RundownModel.Date).ToString("dddd"));
                    parameters.Add("@Status", "01");
                    parameters.Add("@LoginId", LoginUser.LoginId);
                    #endregion 參數設定

                    CommendText = $@"INSERT INTO ActRundown
                                               (ActID, 
                                                ActDetailId, 
                                                ActSectionId, 
                                                ActPlaceID, 
                                                ActPlaceText, 
                                                PlaceSource, 
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
                                                @ActSectionId, 
                                                @ActPlaceID, 
                                                @ActPlaceText, 
                                                @PlaceSource,  
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

                    if (!ExecuteResult.isSuccess) { return ExecuteResult; }
                }
            }
            else
            {
                for (int i = 0; i <= RundownModel.LstStime.Count - 1; i++)
                {
                    int hour = RundownModel.LstStime[i];

                    #region 參數設定
                    parameters.Add("@ActID", ActId);
                    parameters.Add("@ActDetailId", ActDetailId);
                    parameters.Add("@ActSectionId", ActSectionId);
                    parameters.Add("@ActPlaceID", PlaceID);
                    parameters.Add("@ActPlaceText", PlaceText);
                    parameters.Add("@PlaceSource", PlaceSource);
                    parameters.Add("@Date", DateTime.Parse(RundownModel.Date).ToString("yyyy-MM-dd"));
                    parameters.Add("@Stime", hour.ToString().PadLeft(2, '0'));
                    parameters.Add("@ETime", (hour + 1).ToString().PadLeft(2, '0'));
                    parameters.Add("@Week", DateTime.Parse(RundownModel.Date).ToString("dddd"));
                    parameters.Add("@Status", "01");
                    parameters.Add("@LoginId", LoginUser.LoginId);
                    #endregion 參數設定

                    CommendText = $@"INSERT INTO ActRundownElse
                                               (ActID, 
                                                ActDetailId, 
                                                ActSectionId, 
                                                ActPlaceID, 
                                                ActPlaceText, 
                                                PlaceSource, 
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
                                                @ActSectionId, 
                                                @ActPlaceID, 
                                                @ActPlaceText, 
                                                @PlaceSource,  
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

                    if (!ExecuteResult.isSuccess) { return ExecuteResult; }
                }
            }


            return ExecuteResult;

        }

        public DbExecuteInfo InsertActProposalData(ActListMangViewModel vm, string ActId, string ActDetailId, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            List<ActListFilesModel> dataList = vm.CreateModel.LstProposal;


            #region 參數設定
            #endregion 參數設定

            CommendText = $@"INSERT INTO ActProposal 
                                               (ActID, 
                                                ActDetailId, 
                                                FileName, 
                                                FilePath, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               ('{ActId}', 
                                                '{ActDetailId}', 
                                                @FileName, 
                                                @FilePath, 
                                                '{LoginUser.LoginId}', 
                                                GETDATE(), 
                                                '{LoginUser.LoginId}', 
                                                GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        public DbExecuteInfo InsertOutSideData(ActListMangViewModel vm, string ActId, string ActDetailId, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@ActID", ActId);
            parameters.Add("@ActDetailId", ActDetailId);

            parameters.Add("@LeaderName", vm.CreateModel.LeaderName);
            parameters.Add("@LeaderTel", vm.CreateModel.LeaderTel);
            parameters.Add("@LeaderPhone", vm.CreateModel.LeaderPhone);
            parameters.Add("@ManagerName", vm.CreateModel.ManagerName);
            parameters.Add("@ManagerTel", vm.CreateModel.ManagerTel);
            parameters.Add("@ManagerPhone", vm.CreateModel.ManagerPhone);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            CommendText = $@"INSERT INTO ActOutSideInfo 
                                               (ActID, 
                                                ActDetailId, 
                                                LeaderName, 
                                                LeaderTel, 
                                                LeaderPhone, 
                                                ManagerName, 
                                                ManagerTel, 
                                                ManagerPhone, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@ActID, 
                                                @ActDetailId, 
                                                @LeaderName, 
                                                @LeaderTel, 
                                                @LeaderPhone, 
                                                @ManagerName, 
                                                @ManagerTel, 
                                                @ManagerPhone,  
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo InsertOutSideFileData(ActListMangViewModel vm, string ActId, string ActDetailId, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            List<ActListFilesModel> dataList = vm.CreateModel.LstOutSideFile;


            #region 參數設定
            #endregion 參數設定

            CommendText = $@"INSERT INTO ActOutSideInfoFile 
                                               (ActID, 
                                                ActDetailId, 
                                                FileName, 
                                                FilePath, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               ('{ActId}', 
                                                '{ActDetailId}', 
                                                @FileName, 
                                                @FilePath, 
                                                '{LoginUser.LoginId}', 
                                                GETDATE(), 
                                                '{LoginUser.LoginId}', 
                                                GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }
        #endregion

        #region 修改
        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateActMainData(ActListMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActID", vm.EditModel.ActID);

            parameters.Add("@ActVerify", vm.EditModel.ActVerify);
            parameters.Add("@ActVerifyMemo", vm.EditModel.ActVerifyMemo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ActMain 
                                       SET ActVerify = @ActVerify, ActVerifyMemo = @ActVerifyMemo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ActID = @ActID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateActDetailData(ActListMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActID", vm.EditModel.ActID);
            parameters.Add("@SDGs", vm.EditModel.SDGs);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ActDetail 
                                       SET SDGs = @SDGs, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE ActID = @ActID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        #endregion

        /// <summary> Excel 取得資料 </summary>
        public List<ActListMangResultModel> GetExportResult(ActListMangConditionModel model)
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

            CommandText = $@"SELECT A.ActID, A.SchoolYear, A.ActName, B.ActVerify, C.Text AS ActVerifyText, A.BrrowUnit, 
                                    CASE WHEN B.ActVerify = '05' THEN C.Text + '(' + B.Creator + ')' ELSE D.ClubCName END ClubName, 
	                                F.MinDate AS SDate, F.MaxDate AS EDate, A.Created
                               FROM ActDetail A
                          LEFT JOIN ActMain B ON B.ActID = A.ActID
                          LEFT JOIN Code C ON C.Code = B.ActVerify AND C.Type = 'ActVerify'
                          LEFT JOIN ClubMang D ON D.ClubId = A.BrrowUnit
                          LEFT JOIN ActSection E ON E.ActDetailId = A.ActDetailId
                          LEFT JOIN (SELECT ActID, MIN(Date) AS MinDate, MAX(Date) AS MaxDate FROM ActSection GROUP BY ActID) F ON F.ActID = B.ActID
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@ActId IS NULL OR A.ActId = @ActId)
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR B.ActVerify = @ActVerify)
AND (@ActName IS NULL OR A.ActName LIKE '%' + @ActName + '%') 
GROUP BY A.ActID, A.SchoolYear, A.ActName, B.ActVerify, C.Text, A.BrrowUnit, F.MinDate, F.MaxDate, B.Creator, D.ClubCName, A.Created";

            (DbExecuteInfo info, IEnumerable<ActListMangResultModel> entitys) dbResult = DbaExecuteQuery<ActListMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActListMangResultModel>();
        }

        #region 其他

        public bool ChkHasAct(ActListMangViewModel vm)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommandText = string.Empty;

            parameters.Add("@PlaceSource", vm.RundownModel.PlaceID);
            parameters.Add("@Date", vm.RundownModel.Date);
            parameters.Add("@STime", vm.RundownModel.STime);
            parameters.Add("@ETime", vm.RundownModel.ETime);

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

        public DataTable GetAllRundownStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'RundownStatus'";


            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }


        #endregion

    }
}
