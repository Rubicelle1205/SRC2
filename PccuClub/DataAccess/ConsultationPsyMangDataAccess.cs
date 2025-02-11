using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{

    public class ConsultationPsyMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ConsultationPsyMangResultModel> GetSearchResult(ConsultationPsyMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@UserName", model?.UserName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.LoginID, A.UserName, D.Memo
                               FROM UserMain A
                          LEFT JOIN UserRole B on B.LoginId = A.LoginId
                          LEFT JOIN SystemRole C on C.RoleId = B.RoleId
                          LEFT JOIN PsychologistMang D ON D.LoginId = A.LoginId
                              WHERE 1 = 1
                                AND C.SystemCode = '05' 
                                AND C.SystemRoleCode = '02' 
                                AND (@UserName IS NULL OR A.UserName LIKE '%' + @UserName + '%') 
                                AND (@Memo IS NULL OR D.Memo LIKE '%' + @Memo + '%') 
";

            (DbExecuteInfo info, IEnumerable<ConsultationPsyMangResultModel> entitys) dbResult = DbaExecuteQuery<ConsultationPsyMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsultationPsyMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ConsultationPsyMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@LoginID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.LoginID, A.UserName, D.Memo
                               FROM UserMain A
                          LEFT JOIN UserRole B on B.LoginId = A.LoginId
                          LEFT JOIN SystemRole C on C.RoleId = B.RoleId
                          LEFT JOIN PsychologistMang D ON D.LoginId = A.LoginId
                              WHERE 1 = 1
                                AND C.SystemCode = '05' 
                                AND C.SystemRoleCode = '02' 
                                AND A.IsEnable = '1'
                                AND A.LoginID = @LoginID ";


            (DbExecuteInfo info, IEnumerable<ConsultationPsyMangEditModel> entitys) dbResult = DbaExecuteQuery<ConsultationPsyMangEditModel>(CommandText, parameters, true, DBAccessException);

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
                                AND Type = '01' 
                                AND ID = @ID ";


            (DbExecuteInfo info, IEnumerable<AppointmentTimeModel> entitys) dbResult = DbaExecuteQuery<AppointmentTimeModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return null;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ConsultationPsyMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", vm.EditModel.LoginID);
            #endregion 參數設定

            string CommendText = $@"IF EXISTS (SELECT 1
                                         FROM PsychologistMang 
                                        WHERE LoginId = @LoginId)
                                    
                                BEGIN
                                        UPDATE PsychologistMang 
                                       SET Memo = @Memo
                                     WHERE LoginID = @LoginID;
                                    END
                                ELSE
                                    BEGIN
                                        INSERT INTO PsychologistMang
                                                (LoginId
                                                ,Memo)
                                         VALUES
                                               (@LoginId
                                               ,@Memo);
                                    END
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改可預約時間資料 </summary>
        public DbExecuteInfo UpdateAppointmentTime(ConsultationPsyMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ID", vm.EditModel.LoginID);

            #endregion

            string CommendText = $@"DELETE FROM AppointmentTimeMang WHERE ID = @ID AND Type = '01'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (!ExecuteResult.isSuccess && ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
            {
                if (ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
                    return ExecuteResult;
            }

            if (null != vm.EditModel.strAppointmentTime && vm.EditModel.strAppointmentTime.Length > 0)
            {
                List<AppointmentTimeModel> Lstdata = new List<AppointmentTimeModel>();
                string[] arr1 = vm.EditModel.strAppointmentTime.Trim().Split("|");

                for (int i = 0; i <= arr1.Length - 1; i++)
                {
                    string[] arr2 = arr1[i].Trim().Split(":");
                    string [] arr3 = arr2[1].Trim().Split(",");

                    for (int j = 0; j <= arr3.Length - 1; j++)
                    {
                        AppointmentTimeModel TimeModel = new AppointmentTimeModel();

                        TimeModel.ID = vm.EditModel.LoginID;
                        TimeModel.Type = "01";
                        TimeModel.Week = arr2[0].Trim().ToString();
                        TimeModel.Hour = arr3[j].Trim().ToString();
                        Lstdata.Add(TimeModel);
                    }
                }

                CommendText = $@"INSERT INTO AppointmentTimeMang
                                               (ID
                                               ,Type
                                               ,Week
                                               ,Hour)
                                         VALUES
                                               (@ID
                                               ,@Type
                                               ,@Week
                                               ,@Hour)";

                ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, Lstdata, false, DBAccessException, null);

            }

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
            parameters.Add("@LoginID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM PsychologistMang WHERE LoginID = @LoginID ";

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

            string CommendText = $@"DELETE FROM AppointmentTimeMang WHERE ID = @LoginID AND Type = '01' ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

    }
}
