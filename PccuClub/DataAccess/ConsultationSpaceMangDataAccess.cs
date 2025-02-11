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

    public class ConsultationSpaceMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ConsultationSpaceMangResultModel> GetSearchResult(ConsultationSpaceMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@RoomName", model?.RoomName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.RoomName, A.Memo
                               FROM ConsultationRoomMang A
                              WHERE 1 = 1
                                AND (@RoomName IS NULL OR A.RoomName LIKE '%' + @RoomName + '%') 
                                AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') 
";

            (DbExecuteInfo info, IEnumerable<ConsultationSpaceMangResultModel> entitys) dbResult = DbaExecuteQuery<ConsultationSpaceMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ConsultationSpaceMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ConsultationSpaceMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.RoomName, A.Memo
                               FROM ConsultationRoomMang A
                              WHERE 1 = 1
                                AND A.ID = @ID ";


            (DbExecuteInfo info, IEnumerable<ConsultationSpaceMangEditModel> entitys) dbResult = DbaExecuteQuery<ConsultationSpaceMangEditModel>(CommandText, parameters, true, DBAccessException);

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
                                AND Type = '02'
                                AND ID = @ID ";


            (DbExecuteInfo info, IEnumerable<AppointmentTimeModel> entitys) dbResult = DbaExecuteQuery<AppointmentTimeModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ConsultationSpaceMangViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@RoomName", vm.CreateModel.RoomName);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ConsultationRoomMang
                                               (RoomName
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         OUTPUT Inserted.ID
                                         VALUES
                                               (@RoomName
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE() )";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);
            dt = ds.Tables[0];

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ConsultationSpaceMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@RoomName", vm.EditModel.RoomName);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@ID", vm.EditModel.ID);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ConsultationRoomMang 
                                       SET RoomName = @RoomName, 
                                           Memo = @Memo, 
                                           LastModifier = @LoginId, 
                                           LastModified = GETDate()
                                     WHERE ID = @ID;
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改可預約時間資料 </summary>
        public DbExecuteInfo UpdateAppointmentTime(ConsultationSpaceMangViewModel vm, UserInfo LoginUser, string ID = null)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = string.Empty;
            List<AppointmentTimeModel> Lstdata = new List<AppointmentTimeModel>();

            if (vm.CreateModel != null)
            {
                if (null != vm.CreateModel.strAppointmentTime && vm.CreateModel.strAppointmentTime.Length > 0)
                {
                    string[] arr1 = vm.CreateModel.strAppointmentTime.Trim().Split("|");

                    for (int i = 0; i <= arr1.Length - 1; i++)
                    {
                        string[] arr2 = arr1[i].Trim().Split(":");
                        string[] arr3 = arr2[1].Trim().Split(",");

                        for (int j = 0; j <= arr3.Length - 1; j++)
                        {
                            AppointmentTimeModel TimeModel = new AppointmentTimeModel();

                            TimeModel.ID = ID;
                            TimeModel.Type = "02";
                            TimeModel.Week = arr2[0].Trim().ToString();
                            TimeModel.Hour = arr3[j].Trim().ToString();
                            Lstdata.Add(TimeModel);
                        }
                    }
                }
            }


            if (vm.EditModel != null)
            {
                #region 參數設定
                parameters.Add("@ID", vm.EditModel.ID);
                #endregion

                CommendText = $@"DELETE FROM AppointmentTimeMang WHERE ID = @ID AND Type = '02'";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (!ExecuteResult.isSuccess && ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
                {
                    if (ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
                        return ExecuteResult;
                }

                if (null != vm.EditModel.strAppointmentTime && vm.EditModel.strAppointmentTime.Length > 0)
                {
                    string[] arr1 = vm.EditModel.strAppointmentTime.Trim().Split("|");

                    for (int i = 0; i <= arr1.Length - 1; i++)
                    {
                        string[] arr2 = arr1[i].Trim().Split(":");
                        string[] arr3 = arr2[1].Trim().Split(",");

                        for (int j = 0; j <= arr3.Length - 1; j++)
                        {
                            AppointmentTimeModel TimeModel = new AppointmentTimeModel();

                            TimeModel.ID = vm.EditModel.ID;
                            TimeModel.Type = "02";
                            TimeModel.Week = arr2[0].Trim().ToString();
                            TimeModel.Hour = arr3[j].Trim().ToString();
                            Lstdata.Add(TimeModel);
                        }
                    }
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

            string CommendText = $@"DELETE FROM ConsultationRoomMang WHERE ID = @ID";

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
            parameters.Add("@ID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM AppointmentTimeMang WHERE ID = @ID AND Type = '02'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

    }
}
