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

    public class ConsultationCaseMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ConsultationCaseMangResultModel> GetSearchResult(ConsultationCaseMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            parameters.Add("@Name", model.Name);
            parameters.Add("@Psychologist", model.Psychologist);
            parameters.Add("@RoomID", model.RoomID);
            parameters.Add("@AssignCaseStatus", model.AssignCaseStatus);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.Name, A.SNO, A.AssignCaseMan, B.UserName AS AssignCaseManText, A.AssignCaseTime, A.AssignCaseStatus, E.Text AS AssignCaseStatusText, A.FinishCaseTime, 
                                    A.Psychologist, C.UserName AS PsychologistText, A.RoomID, D.RoomName AS RoomIDText, A.TalkDate, A.TalkSTime, A.TalkETime, A.Created
                               FROM ConsultationCaseMang A
                          LEFT JOIN UserMain B on B.LoginId = A.AssignCaseMan
                          LEFT JOIN UserMain C on C.LoginId = A.Psychologist
                          LEFT JOIN ConsultationRoomMang D ON D.ID = A.RoomID
                          LEFT JOIN Code E on E.Code = A.AssignCaseStatus AND E.Type = 'AssignCase'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
{(model.Name != null ? " AND A.Name LIKE '%' + @Name + '%'" : " ")}
AND (@Psychologist IS NULL OR A.Psychologist = @Psychologist)
AND (@RoomID IS NULL OR A.RoomID = @RoomID)
AND (@AssignCaseStatus IS NULL OR A.AssignCaseStatus = @AssignCaseStatus)
";

            (DbExecuteInfo info, IEnumerable<ConsultationCaseMangResultModel> entitys) dbResult = DbaExecuteQuery<ConsultationCaseMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsultationCaseMangResultModel>();
        }


        public List<ConsultationCaseMangExcelModel> GetExportResult(ConsultationCaseMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            parameters.Add("@Name", model.Name);
            parameters.Add("@Psychologist", model.Psychologist);
            parameters.Add("@RoomID", model.RoomID);
            parameters.Add("@AssignCaseStatus", model.AssignCaseStatus);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.Name, A.SNO, A.AssignCaseMan, B.UserName AS AssignCaseManText, A.AssignCaseTime, A.AssignCaseStatus, E.Text AS AssignCaseStatusText, 
                                    A.Psychologist, C.UserName AS PsychologistText, A.RoomID, D.RoomName AS RoomIDText, A.TalkDate, A.TalkSTime, A.TalkETime, A.Created
                               FROM ConsultationCaseMang A
                          LEFT JOIN UserMain B on B.LoginId = A.AssignCaseMan
                          LEFT JOIN UserMain C on C.LoginId = A.Psychologist
                          LEFT JOIN ConsultationRoomMang D ON D.ID = A.RoomID
                          LEFT JOIN Code E on E.Code = A.AssignCaseStatus AND E.Type = 'AssignCase'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
{(model.Name != null ? " AND A.Name LIKE '%' + @Name + '%'" : " ")}
AND (@Psychologist IS NULL OR A.Psychologist = @Psychologist)
AND (@RoomID IS NULL OR A.RoomID = @RoomID)
AND (@AssignCaseStatus IS NULL OR A.AssignCaseStatus = @AssignCaseStatus)
";

            (DbExecuteInfo info, IEnumerable<ConsultationCaseMangExcelModel> entitys) dbResult = DbaExecuteQuery<ConsultationCaseMangExcelModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsultationCaseMangExcelModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ConsultationCaseMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.Name, A.SNO, A.TalkDate, A.TalkSTime, A.TalkETime, A.RoomID, B.RoomName AS RoomIDText, A.Psychologist, C.UserName AS PsychologistText, A.FinishCaseTime, 
                                    A.AssignCaseStatus, D.Text AS AssignCaseStatusText, A.AssignCaseTime, A.Memo, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM ConsultationCaseMang A
							   LEFT JOIN ConsultationRoomMang B ON B.ID = A.RoomID
							   LEFT JOIN UserMain C on C.LoginId = A.Psychologist
                               LEFT JOIN Code D on D.Code = A.AssignCaseStatus AND D.Type = 'AssignCase'
                              WHERE 1 = 1
                                AND A.ID = @ID 
";


            (DbExecuteInfo info, IEnumerable<ConsultationCaseMangEditModel> entitys) dbResult = DbaExecuteQuery<ConsultationCaseMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public List<Order> GetOrder()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID AS ID, A.SNO AS StudentNumber, A.TalkDate AS Date, A.TalkSTime AS StartTime, A.TalkETime AS EndTime, 
	                                A.RoomID, B.RoomName AS RoomTitle, A.Psychologist, C.UserName AS PsychologistTitle
                               FROM ConsultationCaseMang A
							   LEFT JOIN ConsultationRoomMang B ON B.ID = A.RoomID
							   LEFT JOIN UserMain C on C.LoginId = A.Psychologist
                              WHERE 1 = 1
";

            (DbExecuteInfo info, IEnumerable<Order> entitys) dbResult = DbaExecuteQuery<Order>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<Order>();
        }

        public DbExecuteInfo InsertData(ConsultationCaseMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Name", vm.CreateModel.Name);
            parameters.Add("@SNO", vm.CreateModel.SNO);
            parameters.Add("@Psychologist", vm.CreateModel.Psychologist);
            parameters.Add("@RoomID", vm.CreateModel.RoomID);
            parameters.Add("@TalkDate", vm.CreateModel.TalkDate);
            parameters.Add("@TalkSTime", vm.CreateModel.TalkSTime);
            parameters.Add("@TalkETime", vm.CreateModel.TalkETime);
            parameters.Add("@Memo", vm.CreateModel.Memo);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ConsultationCaseMang
                                               (Name
                                               ,SNO
                                               ,TalkDate
                                               ,TalkSTime
                                               ,TalkETime
                                               ,AssignCaseMan
                                               ,AssignCaseTime
                                               ,Psychologist
                                               ,RoomID
                                               ,AssignCaseStatus
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@Name
                                               ,@SNO
                                               ,@TalkDate
                                               ,@TalkSTime
                                               ,@TalkETime
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@Psychologist
                                               ,@RoomID
                                               ,'01'
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ConsultationCaseMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@AssignCaseStatus", vm.EditModel.AssignCaseStatus);
            parameters.Add("@FinishCaseTime", vm.EditModel.FinishCaseTime);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);

            parameters.Add("@ID", vm.EditModel.ID);
            #endregion 參數設定

            string CommendText = $@"UPDATE ConsultationCaseMang 
                                       SET AssignCaseStatus = @AssignCaseStatus
                                          ,FinishCaseTime = @FinishCaseTime
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

            string CommendText = $@"DELETE FROM ConsultationCaseMang WHERE ID = @ID ";

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

        public List<SelectListItem> GetddlAssignCase()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'AssignCase'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlRoom()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS VALUE, RoomName AS TEXT FROM ConsultationRoomMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlTalkSTime(string RoomID, string TalkDate)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            DateTime date = DateTime.Parse(TalkDate);
            DayOfWeek dayOfWeek = date.DayOfWeek;
            int dayOfWeekNumber = ((int)dayOfWeek == 0) ? 7 : (int)dayOfWeek;

            parameters.Add("@ID", RoomID);
            parameters.Add("@Week", dayOfWeekNumber);


            #region 參數設定
            #endregion

            //CommandText = @"SELECT STime AS VALUE, STime AS TEXT
            //                  FROM (SELECT '08:00' AS STime UNION ALL SELECT '09:00' AS STime UNION ALL SELECT '10:00' AS STime UNION ALL 
            //                        SELECT '11:00' AS STime UNION ALL SELECT '12:00' AS STime UNION ALL SELECT '13:00' AS STime UNION ALL 
            //                        SELECT '14:00' AS STime UNION ALL SELECT '15:00' AS STime UNION ALL SELECT '16:00' AS STime UNION ALL  
            //                        SELECT '17:00' AS STime UNION ALL SELECT '18:00' AS STime UNION ALL SELECT '19:00' AS STime UNION ALL 
            //                        SELECT '20:00' AS STime UNION ALL SELECT '21:00' AS STime) T";

            CommandText = @"SELECT Hour AS VALUE, Hour AS TEXT
                              FROM AppointmentTimeMang
                             WHERE TYPE = '02' 
                               AND ID = @ID 
                               AND Week = @Week 
";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
            {
                List<SelectListItem> list = new List<SelectListItem>();

                for (int i = 0; i < dbResult.entitys.Count() - 1; i++)
                {
                    SelectListItem item = new SelectListItem();
                    string formattedNumber = i.ToString("D2");

                    item.Value = string.Format("{0}:00", formattedNumber);
                    item.Text = string.Format("{0}:00", formattedNumber);

                    list.Add(item);
                }

                return list;
            }

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlTalkETime(string RoomID, string TalkDate)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            DateTime date = DateTime.Parse(TalkDate);
            DayOfWeek dayOfWeek = date.DayOfWeek;
            int dayOfWeekNumber = ((int)dayOfWeek == 0) ? 7 : (int)dayOfWeek;

            parameters.Add("@ID", RoomID);
            parameters.Add("@Week", dayOfWeekNumber);


            #region 參數設定
            #endregion

            //CommandText = @"SELECT STime AS VALUE, STime AS TEXT
            //                  FROM (SELECT '08:00' AS STime UNION ALL SELECT '09:00' AS STime UNION ALL SELECT '10:00' AS STime UNION ALL 
            //                        SELECT '11:00' AS STime UNION ALL SELECT '12:00' AS STime UNION ALL SELECT '13:00' AS STime UNION ALL 
            //                        SELECT '14:00' AS STime UNION ALL SELECT '15:00' AS STime UNION ALL SELECT '16:00' AS STime UNION ALL  
            //                        SELECT '17:00' AS STime UNION ALL SELECT '18:00' AS STime UNION ALL SELECT '19:00' AS STime UNION ALL 
            //                        SELECT '20:00' AS STime UNION ALL SELECT '21:00' AS STime) T";

            CommandText = @"SELECT Hour + 1 AS VALUE, Hour + 1 AS TEXT
                              FROM AppointmentTimeMang
                             WHERE TYPE = '02' 
                               AND ID = @ID 
                               AND Week = @Week 
";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
            {
                List<SelectListItem> list = new List<SelectListItem>();

                for (int i = 0; i < dbResult.entitys.Count() - 1; i++)
                {
                    SelectListItem item = new SelectListItem();
                    string formattedNumber = i.ToString("D2");

                    item.Value = string.Format("{0}:00", formattedNumber);
                    item.Text = string.Format("{0}:00", formattedNumber);

                    list.Add(item);
                }

                return list;
            }

            return new List<SelectListItem>();
        }
    }
}
