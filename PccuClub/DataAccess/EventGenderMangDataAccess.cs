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
    
    public class EventGenderMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<EventGenderMangResultModel> GetSearchResult(EventGenderMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@GenderMainClass", model?.GenderMainClass);
            parameters.Add("@GenderSecondClass", model?.GenderSecondClass);
            parameters.Add("@CaseStatus", model?.CaseStatus);
            parameters.Add("@AcceptStatus", model?.AcceptStatus);
            parameters.Add("@CaseID", model?.CaseID);
            parameters.Add("@SubCaseID", model?.SubCaseID);

            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.EventID, A.CaseID, A.SubCaseID, A.CaseSystemType, F.OccurTime, F.KnowTime,
       A.MainClass AS GenderMainClass, B.Text AS GenderMainClassText, 
       A.SecondClass AS GenderSecondClass, C.Text AS GenderSecondClassText, 
       A.AcceptStatus, D.Text AS AcceptStatusText, A.AcceptTime, A.CaseStatus, E.Text AS CaseStatusText, A.CaseFinishDateTime, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified
FROM hq_PccuCase.dbo.EventMainMang A
LEFT JOIN hq_PccuClub.dbo.EventMainClassMang B ON B.ID = A.MainClass AND B.CaseSystemType = '02'
LEFT JOIN hq_PccuClub.dbo.EventSecondClassMang C ON C.ID = A.SecondClass AND C.CaseSystemType = '02'
LEFT JOIN Code D ON D.Code = A.AcceptStatus AND D.Type = 'AcceptStatus' 
LEFT JOIN Code E ON E.Code = A.CaseStatus AND E.Type = 'CaseFinish' 
LEFT JOIN hq_PccuCase.dbo.CaseMainMang F ON F.CaseID = A.CaseID
WHERE 1 = 1
AND A.CaseSystemType = '02'
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@GenderMainClass IS NULL OR A.MainClass = @GenderMainClass)
AND (@GenderSecondClass IS NULL OR A.SecondClass = @GenderSecondClass)
AND (@CaseStatus IS NULL OR A.CaseStatus = @CaseStatus)
AND (@AcceptStatus IS NULL OR A.AcceptStatus = @AcceptStatus)
AND (@CaseID IS NULL OR A.CaseID LIKE '%' + @CaseID + '%')
AND (@SubCaseID IS NULL OR A.SubCaseID LIKE '%' + @SubCaseID + '%')  ";


            (DbExecuteInfo info, IEnumerable<EventGenderMangResultModel> entitys) dbResult = DbaExecuteQuery<EventGenderMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventGenderMangResultModel>();
        }

        public List<EventGenderMangResultModel> GetExportResult(EventGenderMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@GenderMainClass", model?.GenderMainClass);
            parameters.Add("@GenderSecondClass", model?.GenderSecondClass);
            parameters.Add("@CaseStatus", model?.CaseStatus);
            parameters.Add("@AcceptStatus", model?.AcceptStatus);
            parameters.Add("@CaseID", model?.CaseID);
            parameters.Add("@SubCaseID", model?.SubCaseID);

            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.EventID, A.CaseID, A.SubCaseID, A.CaseSystemType, F.OccurTime, F.KnowTime,
       A.MainClass AS GenderMainClass, B.Text AS GenderMainClassText, 
       A.SecondClass AS GenderSecondClass, C.Text AS GenderSecondClassText, 
       A.AcceptStatus, D.Text AS AcceptStatusText, A.AcceptTime, A.CaseStatus, E.Text AS CaseStatusText, A.CaseFinishDateTime, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified
FROM hq_PccuCase.dbo.EventMainMang A
LEFT JOIN hq_PccuClub.dbo.EventMainClassMang B ON B.ID = A.MainClass AND B.CaseSystemType = '02'
LEFT JOIN hq_PccuClub.dbo.EventSecondClassMang C ON C.ID = A.SecondClass AND C.CaseSystemType = '02'
LEFT JOIN Code D ON D.Code = A.AcceptStatus AND D.Type = 'AcceptStatus' 
LEFT JOIN Code E ON E.Code = A.CaseStatus AND E.Type = 'CaseFinish' 
LEFT JOIN hq_PccuCase.dbo.CaseMainMang F ON F.CaseID = A.CaseID
WHERE 1 = 1
AND A.CaseSystemType = '02'
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@GenderMainClass IS NULL OR A.MainClass = @GenderMainClass)
AND (@GenderSecondClass IS NULL OR A.SecondClass = @GenderSecondClass)
AND (@CaseStatus IS NULL OR A.CaseStatus = @CaseStatus)
AND (@AcceptStatus IS NULL OR A.AcceptStatus = @AcceptStatus)
AND (@CaseID IS NULL OR A.CaseID LIKE '%' + @CaseID + '%')
AND (@SubCaseID IS NULL OR A.SubCaseID LIKE '%' + @SubCaseID + '%')  ";


            (DbExecuteInfo info, IEnumerable<EventGenderMangResultModel> entitys) dbResult = DbaExecuteQuery<EventGenderMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventGenderMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EventGenderMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT D.EventID, A.CaseID, A.MainClass, B.Text AS MainClassText, A.SecondClass, C.Text AS SecondClassText, A.CaseStatus, A.CaseFinishDateTime, 
                                    A.OccurTime, A.KnowTime, A.ReferCode, A.Location, A.MediaKnow, A.DeathAmt, A.HurtAmt, A.SickAmt, A.ElseAmt, A.Created, A.LastModified, 
									D.SubCaseID, D.MainClass AS GenderMainClass, E.Text AS GenderMainClassText, D.SecondClass AS GenderSecondClass, F.Text AS GenderSecondClassText, D.AcceptStatus, D.AcceptTime, 
									D.CaseStatus, D.CaseFinishDateTime, D.Memo
                               FROM hq_PccuCase.dbo.CaseMainMang A
                          LEFT JOIN hq_PccuClub.dbo.EventMainClassMang B ON B.ID = A.MainClass AND B.CaseSystemType = '01'
                          LEFT JOIN hq_PccuClub.dbo.EventSecondClassMang C ON C.ID = A.SecondClass AND C.CaseSystemType = '01'
                          LEFT JOIN hq_pccuCase.dbo.EventMainMang D ON D.CaseID = A.CaseID AND D.CaseSystemType = '02'
						  LEFT JOIN hq_PccuClub.dbo.EventMainClassMang E ON E.ID = D.MainClass AND E.CaseSystemType = '02'
                          LEFT JOIN hq_PccuClub.dbo.EventSecondClassMang F ON F.ID = D.SecondClass AND F.CaseSystemType = '02'
WHERE 1 = 1
AND (A.CaseID = @ID) ";


            (DbExecuteInfo info, IEnumerable<EventGenderMangEditModel> entitys) dbResult = DbaExecuteQuery<EventGenderMangEditModel>(CommandText, parameters, true, DBAccessException);

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

            CommandText = $@"SELECT A.CaseID, A.Name, A.SNO, A.BirthYear, A.Sex, B.Text AS SexText, A.Status, C.Text AS StatusText, A.Title, D.Text AS TitleText, 
                                    A.Unit, E.Text AS UnitText, A.Location, F.Text AS LocationText, A.Role, G.Text AS RoleText,
                                    A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM hq_PccuCase.dbo.CaseVictimMang A
							   LEFT JOIN hq_PccuClub.dbo.Code B ON B.Code = A.Sex AND B.Type = 'Sex'
							   LEFT JOIN hq_PccuClub.dbo.Code C ON C.Code = A.Status AND C.Type = 'VictimStatus'
							   LEFT JOIN hq_PccuClub.dbo.Code D ON D.Code = A.Title AND D.Type = 'VictimTitle'
							   LEFT JOIN hq_PccuClub.dbo.Code E ON E.Code = A.Unit AND E.Type = 'VictimUnit'
							   LEFT JOIN hq_PccuClub.dbo.Code F ON F.Code = A.Location AND F.Type = 'VictimLocation'
							   LEFT JOIN hq_PccuClub.dbo.Code G ON G.Code = A.Role AND G.Type = 'VictimRole'
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
						  LEFT JOIN hq_PccuClub.dbo.EventStatusMang B ON B.ID = A.EventID AND B.CaseSystemType = '02'
                              WHERE 1 = 1
                                AND A.CaseID = @CaseID AND A.CaseSystemType = '02'
						   ORDER BY A.EventDateTime DESC
";


            (DbExecuteInfo info, IEnumerable<EventData> entitys) dbResult = DbaExecuteQuery<EventData>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventData>();
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo ImportData(List<EventGenderMangImportModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion 參數設定

            string CommendText = $@"INSERT INTO hq_PccuCase.dbo.EventMainMang
                                               (CaseID
                                               ,SubCaseID
                                               ,CaseSystemType
                                               ,MainClass
                                               ,SecondClass
                                               ,AcceptStatus
                                               ,AcceptTime
                                               ,CaseStatus
                                               ,CaseFinishDateTime
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@CaseID
                                               ,@SubCaseID
                                               ,'02'
                                               ,@GenderMainClass
                                               ,@GenderSecondClass
                                               ,@AcceptStatus
                                               ,@AcceptTime
                                               ,@CaseStatus
                                               ,@CaseFinishDateTime
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }


        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(EventGenderMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@EventID", vm.EditModel.EventID);
            parameters.Add("@SubCaseID", vm.EditModel.SubCaseID);
            parameters.Add("@GenderMainClass", vm.EditModel.GenderMainClass);
            parameters.Add("@GenderSecondClass", vm.EditModel.GenderSecondClass);
            parameters.Add("@AcceptStatus", vm.EditModel.AcceptStatus);
            parameters.Add("@AcceptTime", vm.EditModel.AcceptTime);
            parameters.Add("@CaseStatus", vm.EditModel.CaseStatus);
            parameters.Add("@CaseFinishDateTime", vm.EditModel.CaseFinishDateTime);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE hq_PccuCase.dbo.EventMainMang 
                                       SET SubCaseID = @SubCaseID, 
                                           MainClass = @GenderMainClass, 
                                           SecondClass = @GenderSecondClass, 
                                           AcceptStatus = @AcceptStatus, 
                                           AcceptTime = @AcceptTime,
                                           CaseStatus = @CaseStatus, 
                                           CaseFinishDateTime = @CaseFinishDateTime, 
                                           Memo = @Memo, 
                                           LastModifier = @LoginId, 
                                           LastModified = GETDATE()
                                     WHERE EventID = @EventID AND CaseSystemType = '02'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 新增事件原因及經過資料 </summary>
        public DbExecuteInfo UpdateEventData(EventGenderMangViewModel vm, UserInfo loginUser, string CaseID)
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
                                            ,'02'
                                            ,'{vm.EditModel.GenderEventID}'
                                            ,'{vm.EditModel.GenderEventDateTime}'
                                            ,'{vm.EditModel.GenderEventText}'
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

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
                                            ,'02'
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

            string CommendText = $@"DELETE FROM hq_PccuCase.dbo.EventMainMang WHERE EventID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }


        public List<SelectListItem> GetddlGenderMainClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventMainClassMang WHERE CaseSystemType = '02'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlGenderSecondClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventSecondClassMang WHERE CaseSystemType = '02'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlAcceptStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE type = 'AcceptStatus'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlCaseFinish()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE type = 'CaseFinish'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlGenderCaseID(string CaseID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("CaseID", CaseID);
            #endregion

            CommandText = $@"SELECT SubCaseID AS Value, SubCaseID AS Text FROM hq_PccuCase.dbo.EventMainMang WHERE CaseID = @CaseID AND CaseSystemType = '02'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlGenderEventStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventStatusMang WHERE CaseSystemType = '02' and Enable = 1 ";

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

        public List<SelectListItem> GetddlMainClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventMainClassMang WHERE CaseSystemType = '02'";

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

            CommandText = @"SELECT ID AS Value, Text AS Text FROM EventSecondClassMang WHERE CaseSystemType = '02'";

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
    }
}
