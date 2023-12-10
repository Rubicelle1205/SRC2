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
using MathNet.Numerics.RootFinding;

namespace WebPccuClub.DataAccess
{
    
    public class ClubActReportDataAccess : BaseAccess
    {
        PublicFun PublicFun = new PublicFun();

        /// <summary> 查詢結果 </summary>
        public List<ClubActReportResultModel> GetSearchResult(ClubActReportConditionModel model, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@OrderBy", model?.OrderBy);
            parameters.Add("@LoginId", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT A.ActID, B.ActDetailId, B.ActName, B.SchoolYear, A.ActVerify, C.Text AS ActVerifyText, A.Created
                               FROM ActMain A
                          LEFT JOIN ActDetail B ON B.ActID = A.ActID
                          LEFT JOIN Code C ON C.Code = A.ActVerify AND C.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (@SchoolYear IS NULL OR B.SchoolYear = @SchoolYear)
                                AND (@LoginId IS NULL OR B.BrrowUnit = @LoginId)
                                { (!string.IsNullOrEmpty(model.OrderBy) ? " ORDER BY A.ActID " + model.OrderBy : " ORDER BY A.ActID DESC")}";


            (DbExecuteInfo info, IEnumerable<ClubActReportResultModel> entitys) dbResult = DbaExecuteQuery<ClubActReportResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubActReportResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClubActReportEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ActID, B.ActDetailId, B.ActName, B.SchoolYear, B.BrrowUnit, A.ActVerifyMemo, 
                                    B.StaticOrDynamic, D.Text AS StaticOrDynamicText, B.ActInOrOut, E.Text AS ActInOrOutText,
                                    B.Capacity, B.ActType, F.ActTypeName AS ActTypeText, B.UseITEquip, G.Text AS UseITEquipText,
                                    B.ShortDesc, B.SDGs, B.PassPort, H.Text AS PassPortText,
                                    I.LeaderName, I.LeaderTel, I.LeaderPhone, I.ManagerName, I.ManagerTel, I.ManagerPhone,
                                    A.ActVerify, C.Text AS ActVerifyText, A.Created
                              FROM ActMain A
                         LEFT JOIN ActDetail B ON B.ActID = A.ActID
                         LEFT JOIN Code C ON C.Code = A.ActVerify AND C.Type = 'ActVerify'
                         LEFT JOIN Code D ON D.Code = B.StaticOrDynamic AND D.Type = 'StaticOrDynamic'
                         LEFT JOIN Code E ON E.Code = B.ActInOrOut AND E.Type = 'ActInOrOut'
                         LEFT JOIN ActTypeMang F ON F.ActTypeID = B.ActType
                         LEFT JOIN Code G ON G.Code = B.UseITEquip AND G.Type = 'UseITEquip'
                         LEFT JOIN Code H ON H.Code = B.PassPort AND H.Type = 'PassPort'
                         LEFT JOIN ActOutSideInfo I ON I.ActID = A.ActID
                             WHERE 1 = 1
                                AND (A.ActID = @ActID) ";


            (DbExecuteInfo info, IEnumerable<ClubActReportEditModel> entitys) dbResult = DbaExecuteQuery<ClubActReportEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public ClubActReportConsentModel GetConsentData()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = $@"SELECT InSchool, OutSchool, InAndOutSchool FROM ConsentMang";


            (DbExecuteInfo info, IEnumerable<ClubActReportConsentModel> entitys) dbResult = DbaExecuteQuery<ClubActReportConsentModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        #region 新增

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertActMainData(ClubActReportViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@STime", vm.CreateModel.STime);
            parameters.Add("@ETime", vm.CreateModel.ETime);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ActMain
                                               (ActVerify
                                                ,Creator 
                                                ,Created 
                                                ,LastModifier 
                                                ,LastModified )
                                         OUTPUT Inserted.ActID
                                         VALUES
                                                ('01'
                                                ,@LoginId
                                                ,GETDATE()
                                                ,@LoginId
                                                ,GETDATE()) ";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        public DbExecuteInfo InsertActDetailData(ClubActReportViewModel vm, string ActId, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActID", ActId);
            parameters.Add("@BrrowUnit", LoginUser.LoginId);
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

            parameters.Add("@CreateSource", "02"); // 後台:01 前台:02
            parameters.Add("@ShortDesc", vm.CreateModel.ShortDesc);


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
        public DbExecuteInfo InsertActSectionData(ClubActReportViewModel vm, string ActId, string ActDetailId, DateTime date, UserInfo LoginUser, out DataTable dt)
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
        public DbExecuteInfo InsertActRundownData(ClubActReportViewModel vm, string ActId, string ActDetailId, string ActSectionId, ActListMangRundownModel RundownModel, UserInfo LoginUser)
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

        public DbExecuteInfo InsertActProposalData(ClubActReportViewModel vm, string ActId, string ActDetailId, UserInfo LoginUser, bool IsEdit = false)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            List<ActListFilesModel> dataList = new List<ActListFilesModel>();
            if (IsEdit)
                dataList = vm.EditModel.LstProposal;
            else
                dataList = vm.CreateModel.LstProposal;


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

        public DbExecuteInfo InsertOutSideData(ClubActReportViewModel vm, string ActId, string ActDetailId, UserInfo LoginUser)
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

        public DbExecuteInfo InsertOutSideFileData(ClubActReportViewModel vm, string ActId, string ActDetailId, UserInfo LoginUser, bool IsEdit = false)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            List<ActListFilesModel> dataList = new List<ActListFilesModel>();

            if (IsEdit)
                dataList = vm.EditModel.LstOutSideFile;
            else
                dataList = vm.CreateModel.LstOutSideFile;


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

        #region 編輯

        public DbExecuteInfo UpdateActDetailData(ClubActReportViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActDetailID", vm.EditModel.ActDetailID);
            parameters.Add("@ActName", vm.EditModel.ActName);
            parameters.Add("@StaticOrDynamic", vm.EditModel.StaticOrDynamic);
            parameters.Add("@ActInOrOut", vm.EditModel.ActInOrOut);
            parameters.Add("@Capacity", vm.EditModel.Capacity);
            parameters.Add("@ActType", vm.EditModel.ActType);
            parameters.Add("@UseITEquip", vm.EditModel.UseITEquip);
            parameters.Add("@ShortDesc", vm.EditModel.ShortDesc);
            parameters.Add("@SDGs", vm.EditModel.SDGs);
            parameters.Add("@PassPort", vm.EditModel.PassPort);

            parameters.Add("@LoginID", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ActDetail 
                                       SET ActName = @ActName, 
                                            Capacity = @Capacity, 
                                            ActType = @ActType, 
                                            SDGs = @SDGs, 
                                            StaticOrDynamic = @StaticOrDynamic, 
                                            ActInOrOut = @ActInOrOut, 
                                            UseITEquip = @UseITEquip, 
                                            PassPort = @PassPort, 
                                            ShortDesc = @ShortDesc, 
                                            LastModifier = @LoginID, 
                                            LastModified = GETDATE()
                                     WHERE ActDetailId = @ActDetailID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo UpdateOutSideData(ClubActReportViewModel vm, string ActId, string ActDetailId, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@ActID", ActId);
            parameters.Add("@ActDetailId", ActDetailId);

            parameters.Add("@LeaderName", vm.EditModel.LeaderName);
            parameters.Add("@LeaderTel", vm.EditModel.LeaderTel);
            parameters.Add("@LeaderPhone", vm.EditModel.LeaderPhone);
            parameters.Add("@ManagerName", vm.EditModel.ManagerName);
            parameters.Add("@ManagerTel", vm.EditModel.ManagerTel);
            parameters.Add("@ManagerPhone", vm.EditModel.ManagerPhone);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            CommendText = $@"UPDATE ActOutSideInfo 
                                SET LeaderName = @LeaderName, 
                                    LeaderTel = @LeaderTel, 
                                    LeaderPhone = @LeaderPhone, 
                                    ManagerName = @ManagerName, 
                                    ManagerTel = @ManagerTel, 
                                    ManagerPhone = @ManagerPhone, 
                                    LastModifier = @LoginId, 
                                    LastModified = GETDATE() 
                              WHERE ActID = @ActID AND ActDetailId = @ActDetailId";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 取消行程 </summary>
        public DbExecuteInfo CancelRundown(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string[] arr = ser.Split("|");

            #region 參數設定
            parameters.Add("@ActRundownID", arr[1]);
            #endregion 參數設定

            string CommendText = string.Empty;

            if (arr[0] == "01")
            {
                CommendText = $@"UPDATE ActRundown SET RundownStatus = '02' WHERE ActRundownID = @ActRundownID ";
            }
            else
            {
                CommendText = $@"UPDATE ActRundownElse SET RundownStatus = '02' WHERE ActRundownID = @ActRundownID ";
            }

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
        #endregion


        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            int NowSchoolYear = int.Parse(PublicFun.GetNowSchoolYear());

            for (int i = NowSchoolYear - 2; i <= NowSchoolYear + 2; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
        }

        public string? GetDefaultActName(UserInfo LoginUser)
        {
            string str = string.Empty;

            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT ClubID + '-' + ClubCName + '-' AS DefaultActName 
                               FROM ClubMang
                              WHERE ClubID = @ClubID
";


            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                str = ds.Tables[0].QueryFieldByDT("DefaultActName");
            }

            return str;
        }

    }
}
