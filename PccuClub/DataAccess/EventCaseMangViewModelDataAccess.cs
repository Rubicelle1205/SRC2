using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class EventCaseMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>
        public List<EventCaseMangResultModel> GetSearchResult(EventCaseMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainClass", model.MainClass);
            parameters.Add("@SecondClass", model.SecondClass);
            parameters.Add("@CaseID", model.CaseID);
            parameters.Add("@CaseStatus", model.CaseStatus);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.CaseID, A.MainClass, B.Text AS MainClassText, A.SecondClass, C.Text AS SecondClassText, A.CaseStatus, D.Text AS CaseStatusText, A.CaseFinishDateTime, A.OccurTime, A.KnowTime, A.Created
                               FROM hq_PccuCase.dbo.CaseMainMang A
                          LEFT JOIN hq_PccuClub.dbo.EventMainClassMang B ON B.ID = A.MainClass
                          LEFT JOIN hq_PccuClub.dbo.EventSecondClassMang C ON C.ID = A.SecondClass
                          LEFT JOIN hq_PccuClub.dbo.Code D ON D.Code = A.CaseStatus AND D.Type = 'CaseFinish'
WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@MainClass IS NULL OR A.MainClass = @MainClass)
AND (@SecondClass IS NULL OR A.SecondClass = @SecondClass)
AND (@CaseStatus IS NULL OR A.CaseStatus = @CaseStatus)
AND (@CaseID IS NULL OR A.CaseID LIKE '%' + @CaseID + '%')  ";


            (DbExecuteInfo info, IEnumerable<EventCaseMangResultModel> entitys) dbResult = DbaExecuteQuery<EventCaseMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventCaseMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EventCaseMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.CaseID, A.MainClass, A.SecondClass, A.CaseStatus, A.CaseFinishDateTime, A.OccurTime, A.KnowTime, A.ReferCode,
                                    A.Location, A.MediaKnow, A.DeathAmt, A.HurtAmt, A.SickAmt, A.ElseAmt, A.Created, A.LastModified, A.Memo
                               FROM hq_PccuCase.dbo.CaseMainMang A
                          LEFT JOIN hq_PccuClub.dbo.EventMainClassMang B ON B.ID = A.MainClass
                          LEFT JOIN hq_PccuClub.dbo.EventSecondClassMang C ON C.ID = A.SecondClass
WHERE 1 = 1
AND (A.CaseID = @ID) ";


            (DbExecuteInfo info, IEnumerable<EventCaseMangEditModel> entitys) dbResult = DbaExecuteQuery<EventCaseMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public List<Victim> GetLstVictimData(string CaseID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@CaseID", CaseID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT CaseID, Name, SNO, BirthYear, Sex, Status, Title, Unit, Location, Role, 
                                    Creator, Created, LastModifier, LastModified
                               FROM hq_PccuCase.dbo.CaseVictimMang
                              WHERE 1 = 1
                                AND CaseID = @CaseID
";


            (DbExecuteInfo info, IEnumerable<Victim> entitys) dbResult = DbaExecuteQuery<Victim>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<Victim>();
        }

        public List<EventData> GetEventData(string CaseID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@CaseID", CaseID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.EventID, B.Text AS EventIDText, A.EventDateTime, A.Text
                               FROM hq_PccuCase.dbo.EventDetailMang A
						  LEFT JOIN hq_PccuClub.dbo.EventStatusMang B ON B.ID = A.EventID AND B.CaseSystemType = '01'
                              WHERE 1 = 1
                                AND A.CaseID = @CaseID AND A.CaseSystemType = '01'
						   ORDER BY A.EventDateTime DESC
";


            (DbExecuteInfo info, IEnumerable<EventData> entitys) dbResult = DbaExecuteQuery<EventData>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventData>();
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(EventCaseMangViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@CaseID", vm.CreateModel.CaseID);

            parameters.Add("@MainClass", vm.CreateModel.MainClass);
            parameters.Add("@SecondClass", vm.CreateModel.SecondClass);

            parameters.Add("@CaseStatus", vm.CreateModel.CaseStatus);
            parameters.Add("@CaseFinishDateTime", vm.CreateModel.CaseFinishDateTime);

            parameters.Add("@Location", vm.CreateModel.Location);
            parameters.Add("@OccurTime", vm.CreateModel.OccurTime);
            parameters.Add("@KnowTime", vm.CreateModel.KnowTime);
            parameters.Add("@MediaKnow", vm.CreateModel.MediaKnow);

            parameters.Add("@DeathAmt", vm.CreateModel.DeathAmt);
            parameters.Add("@HurtAmt", vm.CreateModel.HurtAmt);
            parameters.Add("@SickAmt", vm.CreateModel.SickAmt);
            parameters.Add("@ElseAmt", vm.CreateModel.ElseAmt);

            parameters.Add("@ReferCode", vm.CreateModel.ReferCode);

            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO hq_PccuCase.dbo.CaseMainMang
                                               (CaseID
                                               ,MainClass
                                               ,SecondClass
                                               ,CaseStatus
                                               ,CaseFinishDateTime
                                               ,Location
                                               ,OccurTime
                                               ,KnowTime
                                               ,MediaKnow
                                               ,DeathAmt
                                               ,HurtAmt
                                               ,SickAmt
                                               ,ElseAmt
                                               ,ReferCode
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         OUTPUT Inserted.CaseID
                                         VALUES
                                               (@CaseID
                                               ,@MainClass
                                               ,@SecondClass
                                               ,@CaseStatus
                                               ,@CaseFinishDateTime
                                               ,@Location
                                               ,@OccurTime
                                               ,@KnowTime
                                               ,@MediaKnow
                                               ,@DeathAmt
                                               ,@HurtAmt
                                               ,@SickAmt
                                               ,@ElseAmt
                                               ,@ReferCode
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";


            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        /// <summary> 新增相關人員資料 </summary>
        public DbExecuteInfo InsertVictim(List<Victim> dataList, UserInfo loginUser, string CaseID)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"DELETE FROM hq_PccuCase.dbo.CaseVictimMang WHERE CaseID = '{CaseID}' ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (ExecuteResult.isSuccess || ExecuteResult.ErrorCode == dbErrorCode._EC_NotAffect)
            {
                CommendText = $@"INSERT INTO hq_PccuCase.dbo.CaseVictimMang
                                               (CaseID
                                               ,Name
                                               ,SNO
                                               ,BirthYear
                                               ,Sex
                                               ,Status
                                               ,Title
                                               ,Unit
                                               ,Location
                                               ,Role
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               ('{CaseID}'
                                               ,@Name
                                               ,@SNO
                                               ,@BirthYear
                                               ,@Sex
                                               ,@Status
                                               ,@Title
                                               ,@Unit
                                               ,@Location
                                               ,@Role
                                               ,'{loginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{loginUser.LoginId}'
                                               ,GETDATE() )";

                ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            }

            return ExecuteResult;
        }

        /// <summary> 新增事件原因及經過資料 </summary>
        public DbExecuteInfo InsertEventData(List<EventData> dataList, UserInfo loginUser, string CaseID)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO hq_PccuCase.dbo.EventDetailMang
                                            (CaseID
                                            ,CaseSystemType
                                            ,EventID
                                            ,EventDateTime
                                            ,Text
                                            ,Creator
                                            ,Created
                                            ,LastModifier
                                            ,LastModified)
                                        VALUES
                                            ('{CaseID}'
                                            ,'01'
                                            ,@EventID
                                            ,@EventDateTime
                                            ,@Text
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(EventCaseMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@CaseID", vm.EditModel.CaseID);

            parameters.Add("@MainClass", vm.EditModel.MainClass);
            parameters.Add("@SecondClass", vm.EditModel.SecondClass);

            parameters.Add("@CaseStatus", vm.EditModel.CaseStatus);
            parameters.Add("@CaseFinishDateTime", vm.EditModel.CaseFinishDateTime);

            parameters.Add("@Location", vm.EditModel.Location);
            parameters.Add("@OccurTime", vm.EditModel.OccurTime);
            parameters.Add("@KnowTime", vm.EditModel.KnowTime);
            parameters.Add("@MediaKnow", vm.EditModel.MediaKnow);

            parameters.Add("@DeathAmt", vm.EditModel.DeathAmt);
            parameters.Add("@HurtAmt", vm.EditModel.HurtAmt);
            parameters.Add("@SickAmt", vm.EditModel.SickAmt);
            parameters.Add("@ElseAmt", vm.EditModel.ElseAmt);

            parameters.Add("@ReferCode", vm.EditModel.ReferCode);

            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE hq_PccuCase.dbo.CaseMainMang 
                                       SET MainClass = @MainClass, SecondClass = @SecondClass, 
                                           CaseStatus = @CaseStatus, CaseFinishDateTime = @CaseFinishDateTime, 
                                           Location = @Location, OccurTime = @OccurTime, KnowTime = @KnowTime, MediaKnow = @MediaKnow, 
                                           DeathAmt = @DeathAmt, HurtAmt = @HurtAmt, SickAmt = @SickAmt, ElseAmt = @ElseAmt, 
                                           ReferCode = @ReferCode, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE CaseID = @CaseID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 新增事件原因及經過資料 </summary>
        public DbExecuteInfo UpdateEventData(EventCaseMangViewModel vm, UserInfo loginUser, string CaseID)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO hq_PccuCase.dbo.EventDetailMang
                                           (CaseID
                                            ,CaseSystemType
                                            ,EventID
                                            ,EventDateTime
                                            ,Text
                                            ,Creator
                                            ,Created
                                            ,LastModifier
                                            ,LastModified)
                                        VALUES
                                            ('{CaseID}'
                                            ,'01'
                                            ,'{vm.EditModel.EventID}'
                                            ,'{vm.EditModel.EventDateTime}'
                                            ,'{vm.EditModel.EventText}'
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

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
            parameters.Add("@ID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM hq_PccuCase.dbo.CaseMainMang WHERE CaseID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }


        /// <summary> 查詢結果 </summary>
        public List<EventCaseMangResultModel> GetExportResult(EventCaseMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainClass", model.MainClass);
            parameters.Add("@SecondClass", model.SecondClass);
            parameters.Add("@CaseID", model.CaseID);
            parameters.Add("@CaseStatus", model.CaseStatus);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.CaseID, A.MainClass, B.Text AS MainClassText, A.SecondClass, C.Text AS SecondClassText, A.CaseStatus, D.Text AS CaseStatusText, A.CaseFinishDateTime, A.OccurTime, A.KnowTime, A.Created
                               FROM hq_PccuCase.dbo.CaseMainMang A
                          LEFT JOIN hq_PccuClub.dbo.EventMainClassMang B ON B.ID = A.MainClass
                          LEFT JOIN hq_PccuClub.dbo.EventSecondClassMang C ON C.ID = A.SecondClass
                          LEFT JOIN hq_PccuClub.dbo.Code D ON D.Code = A.CaseStatus AND D.Type = 'CaseFinish'
WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@MainClass IS NULL OR A.MainClass = @MainClass)
AND (@SecondClass IS NULL OR A.SecondClass = @SecondClass)
AND (@CaseStatus IS NULL OR A.CaseStatus = @CaseStatus)
AND (@CaseID IS NULL OR A.CaseID LIKE '%' + @CaseID + '%')  ";


            (DbExecuteInfo info, IEnumerable<EventCaseMangResultModel> entitys) dbResult = DbaExecuteQuery<EventCaseMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventCaseMangResultModel>();
        }




        /// <summary> 查詢結果 </summary>

        public List<EventCaseReferDataMangResultModel> GetReferDataSearchResult(EventCaseReferDataMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@CaseID", model?.CaseID);
            parameters.Add("@ReferID", model?.ReferID);
            parameters.Add("@HandleEvent", model?.HandleEvent);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);


            #endregion

            CommandText = $@"SELECT A.ID, A.CaseID, A.ReferID, B.ReferName AS ReferIDText, A.HandleMan, A.HandleTime, A.HandleEvent, A.Leader, A.Director, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM hq_PccuCase.dbo.ReferMang A
                          LEFT JOIN hq_PccuClub.dbo.ReferUnitMang B ON B.ReferID = A.ReferID
                              WHERE 1 = 1 
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.HandleTime BETWEEN @FromDate AND @ToDate" : " ")}
AND (@ReferID IS NULL OR A.ReferID = @ReferID)
AND (@CaseID IS NULL OR A.CaseID LIKE '%' + @CaseID + '%') 
AND (@HandleEvent IS NULL OR A.HandleEvent LIKE '%' + @HandleEvent + '%') 
";


            (DbExecuteInfo info, IEnumerable<EventCaseReferDataMangResultModel> entitys) dbResult = DbaExecuteQuery<EventCaseReferDataMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventCaseReferDataMangResultModel>();
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo ImportData(List<EventCaseImportMangResultModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion 參數設定

            string CommendText = $@"INSERT INTO hq_PccuCase.dbo.CaseMainMang
                                               (CaseID
                                               ,MainClass
                                               ,SecondClass
                                               ,CaseStatus
                                               ,CaseFinishDateTime
                                               ,OccurTime
                                               ,KnowTime
                                                ,Creator
                                                ,Created
                                                ,LastModifier
                                                ,LastModified)
                                         VALUES
                                               (@CaseID
                                               ,@MainClass
                                               ,@SecondClass
                                               ,@CaseStatus
                                               ,@CaseFinishDateTime
                                               ,@OccurTime
                                               ,@KnowTime
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo ReferDataImportData(List<EventCaseReferDataImportMangResultModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion 參數設定

            string CommendText = $@"INSERT INTO hq_PccuCase.dbo.ReferMang
                                               (CaseID
                                                ,ReferID
                                                ,HandleMan
                                                ,HandleTime
                                                ,HandleEvent
                                                ,Leader
                                                ,Director
                                                ,Creator
                                                ,Created
                                                ,LastModifier
                                                ,LastModified)
                                         VALUES
                                               (@CaseID
                                               ,@ReferID
                                               ,@HandleMan
                                               ,@HandleTime
                                               ,@HandleEvent
                                               ,@Leader
                                               ,@Director
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }


        public List<SelectListItem> GetddlMainClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventMainClassMang WHERE CaseSystemType = '01'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlSecondClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventSecondClassMang WHERE CaseSystemType = '01'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlCaseFinishClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'CaseFinish' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlReferUnit()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ReferID AS Value, ReferName AS Text FROM ReferUnitMang ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlYesOrNo()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'YesOrNo' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlEventStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventStatusMang WHERE CaseSystemType = '01' and Enable = 1 ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlSex()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'Sex' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
        public List<SelectListItem> GetddlVictimTitle()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'VictimTitle' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
        public List<SelectListItem> GetddlVictimUnit()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'VictimUnit' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
        public List<SelectListItem> GetddlVictimLocation()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'VictimLocation' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
        public List<SelectListItem> GetddlVictimRole()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'VictimRole' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
        public List<SelectListItem> GetddlBirth()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            int NowYear = DateTime.Now.Year - 1911;

            for (int i = NowYear; i >= NowYear - 100; i--)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("民國{0}年", i) });
            }

            return LstItem;
        }
        public List<SelectListItem> GetddlVictimStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'VictimStatus' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlCaseID()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT CaseID AS Value, CaseID AS Text FROM hq_PccuCase.dbo.CaseMainMang ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
