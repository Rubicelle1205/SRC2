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

    public class ConsultationFirstTalkMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ConsultationFirstTalkMangResultModel> GetSearchResult(ConsultationFirstTalkMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            parameters.Add("@Name", model.Name);
            parameters.Add("@Psychologist", model.Psychologist);
            parameters.Add("@FirstTalkStatus", model.FirstTalkStatus);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.Name, A.Department, A.Sex, B.Text AS SexText, A.CounsellingStatus, A.AssignCaseMan, E.UserName AS AssignCaseManText, A.AssignCaseTime, 
                                    A.Psychologist, D.UserName AS PsychologistText, A.FirstTalkStatus, C.Text AS FirstTalkStatusText, A.FirstTalkTime, A.Created
                               FROM ConsultationFirstTalkMang A
                          LEFT JOIN Code B on B.Code = A.Sex AND B.Type = 'Sex'
                          LEFT JOIN Code C on C.Code = A.FirstTalkStatus AND C.Type = 'FirstTalkStatus'
                          LEFT JOIN UserMain D on D.LoginId = A.Psychologist
                          LEFT JOIN UserMain E on E.LoginId = A.AssignCaseMan
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
{(model.Name != null ? " AND A.Name LIKE '%' + @Name + '%'" : " ")}
AND (@Psychologist IS NULL OR A.Psychologist = @Psychologist)
AND (@FirstTalkStatus IS NULL OR A.FirstTalkStatus = @FirstTalkStatus)
                                
";

            (DbExecuteInfo info, IEnumerable<ConsultationFirstTalkMangResultModel> entitys) dbResult = DbaExecuteQuery<ConsultationFirstTalkMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsultationFirstTalkMangResultModel>();
        }


        public List<ConsultationFirstTalkMangExcelModel> GetExportResult(ConsultationFirstTalkMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            parameters.Add("@Name", model.Name);
            parameters.Add("@Psychologist", model.Psychologist);
            parameters.Add("@FirstTalkStatus", model.FirstTalkStatus);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.Name, A.Department, A.Sex, B.Text AS SexText, A.CounsellingStatus, A.AssignCaseMan, E.UserName AS AssignCaseManText, A.AssignCaseTime, 
                                    A.Psychologist, D.UserName AS PsychologistText, A.FirstTalkStatus, C.Text AS FirstTalkStatusText, A.FirstTalkTime, A.Created
                               FROM ConsultationFirstTalkMang A
                          LEFT JOIN Code B on B.Code = A.Sex AND B.Type = 'Sex'
                          LEFT JOIN Code C on C.Code = A.FirstTalkStatus AND C.Type = 'FirstTalkStatus'
                          LEFT JOIN UserMain D on D.LoginId = A.Psychologist
                          LEFT JOIN UserMain E on E.LoginId = A.AssignCaseMan
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
{(model.Name != null ? " AND A.Name LIKE '%' + @Name + '%'" : " ")}
AND (@Psychologist IS NULL OR A.Psychologist = @Psychologist)
AND (@FirstTalkStatus IS NULL OR A.FirstTalkStatus = @FirstTalkStatus)
                                
";

            (DbExecuteInfo info, IEnumerable<ConsultationFirstTalkMangExcelModel> entitys) dbResult = DbaExecuteQuery<ConsultationFirstTalkMangExcelModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsultationFirstTalkMangExcelModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ConsultationFirstTalkMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.Name, A.Department, A.SNO, A.Tel, A.Sex, B.Text AS SexText, A.Citizenship, C.Text AS CitizenshipText, A.CitizenshipName, A.CounsellingStatus, A.Psychologist,
                                    A.FirstTalkStatus, A.FirstTalkTime, A.AssignCaseMan, D.UserName AS AssignCaseManText, A.AssignCaseTime, A.Memo, 
                                    A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM ConsultationFirstTalkMang A
							   LEFT JOIN Code B ON B.Code = A.Sex AND B.Type = 'Sex'
							   LEFT JOIN Code C ON C.Code = A.Citizenship AND C.Type = 'Citizenship'
                               LEFT JOIN UserMain D on D.LoginId = A.AssignCaseMan
                              WHERE 1 = 1
                                AND A.ID = @ID 
";


            (DbExecuteInfo info, IEnumerable<ConsultationFirstTalkMangEditModel> entitys) dbResult = DbaExecuteQuery<ConsultationFirstTalkMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary>
        /// 取得可預約資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<AppointmentTimeModel> GetApppoointemntTime(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT ID, Type, Week, Hour
                               FROM AppointmentTimeMang
                              WHERE 1 = 1 
                                AND Type = '03' 
                                AND ID = @ID ";


            (DbExecuteInfo info, IEnumerable<AppointmentTimeModel> entitys) dbResult = DbaExecuteQuery<AppointmentTimeModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return null;
        }

        /// <summary> 修改資料1 </summary>
        public DbExecuteInfo UpdateDataFirst(ConsultationFirstTalkMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Psychologist", vm.EditModel.Psychologist);
            parameters.Add("@FirstTalkStatus", vm.EditModel.FirstTalkStatus);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);

            parameters.Add("@ID", vm.EditModel.ID);
            #endregion 參數設定

            string CommendText = $@"UPDATE ConsultationFirstTalkMang 
                                       SET Psychologist = @Psychologist
                                          ,FirstTalkStatus = @FirstTalkStatus
                                          ,AssignCaseMan = @LoginId
                                          ,AssignCaseTime = GETDATE()
                                          ,Memo = @Memo
                                          ,LastModifier = @LoginId
                                          ,LastModified = GETDATE()
                                     WHERE ID = @ID;
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料2 </summary>
        public DbExecuteInfo UpdateDataSecond(ConsultationFirstTalkMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Psychologist", vm.EditModel.Psychologist);
            parameters.Add("@FirstTalkStatus", vm.EditModel.FirstTalkStatus);
            parameters.Add("@FirstTalkTime", vm.EditModel.FirstTalkTime);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);

            parameters.Add("@ID", vm.EditModel.ID);
            #endregion 參數設定

            string CommendText = $@"UPDATE ConsultationFirstTalkMang 
                                       SET Psychologist = @Psychologist
                                          ,FirstTalkStatus = @FirstTalkStatus
                                          ,FirstTalkTime = @FirstTalkTime
                                          ,Memo = @Memo
                                          ,LastModifier = @LoginId
                                          ,LastModified = GETDATE()
                                     WHERE ID = @ID;
";

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

            string CommendText = $@"DELETE FROM ConsultationFirstTalkMang WHERE ID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 刪除可預約時間資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeletetAppointmentTimeMangData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@LoginID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM AppointmentTimeMang WHERE ID = @LoginID AND Type = '03' ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetddlPsy()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT A.LoginId AS VALUE, A.UserName AS TEXT
                              FROM UserMain A
                         LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                         LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                             WHERE C.SystemRoleCode = '02'
";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlFirstTalkStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'FirstTalkStatus'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlCounsellingStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'CounsellingStatus'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
