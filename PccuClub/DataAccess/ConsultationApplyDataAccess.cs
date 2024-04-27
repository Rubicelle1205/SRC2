using DataAccess;
using PccuClub.WebAuth;
using System.Data;
using WebPccuClub.Global;
using WebPccuClub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.XPath;
using WebPccuClub.Global.Extension;
using NPOI.POIFS.Crypt;
using X.PagedList;
using MathNet.Numerics.Optimization;
using System.Runtime.ConstrainedExecution;

namespace WebPccuClub.DataAccess
{

    public class ConsultationApplyDataAccess : BaseAccess
    {

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ConsultationApplyCreateModel GetEditData(string ClubId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ClubId", ClubId);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT A.ClubId, A.ClubCName, A.ClubEName, A.SchoolYear, A.LifeClass, C.Text AS LifeClassText, A.ClubClass, B.Text AS ClubClassText, A.Address, A.EMail, A.Tel, 
                                   A.Social1, A.Social2, A.Social3, A.LogoPath, A.ActImgPath, A.ShortInfo, A.Memo, A.Created, A.LastModified, D.RoleId
                               FROM ClubMang A
							   LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass'
							   LEFT JOIN Code C ON C.Code = A.LifeClass AND C.Type = 'LifeClass'
                               LEFT JOIN UserRole D ON D.LoginId = A.ClubId
                              WHERE 1 = 1
                               AND A.ClubId = @ClubId";


            (DbExecuteInfo info, IEnumerable<ConsultationApplyCreateModel> entitys) dbResult = DbaExecuteQuery<ConsultationApplyCreateModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ConsultationApplyViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Name", vm.CreateModel.Name);
            parameters.Add("@Department", vm.CreateModel.Department);
            parameters.Add("@SNO", vm.CreateModel.SNO);
            parameters.Add("@Tel", vm.CreateModel.Tel);
            parameters.Add("@Citizenship", vm.CreateModel.Citizenship);
            parameters.Add("@CitizenshipName", vm.CreateModel.CitizenshipName);
            parameters.Add("@CounsellingStatus", vm.CreateModel.strCounsellingStatus);
            parameters.Add("@LoginId", LoginUser == null ? vm.CreateModel.SNO : LoginUser.LoginId);

            #endregion 參數設定

            string CommendText = $@"INSERT INTO ConsultationFirstTalkMang
                                               (Name
                                               ,Department
                                               ,SNO
                                               ,Tel
                                               ,Citizenship
                                               ,CitizenshipName
                                               ,CounsellingStatus
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         OUTPUT Inserted.ID
                                         VALUES
                                               (@Name
                                               ,@Department
                                               ,@SNO
                                               ,@Tel
                                               ,@Citizenship
                                               ,@CitizenshipName
                                               ,@CounsellingStatus
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE() )";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        /// <summary> 修改可預約時間資料 </summary>
        public DbExecuteInfo UpdateAppointmentTime(ConsultationApplyViewModel vm, UserInfo LoginUser, DataTable dt)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ID", dt.QueryFieldByDT("ID"));

            #endregion

            string CommendText = $@"DELETE FROM AppointmentTimeMang WHERE ID = @ID AND Type = '03'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (!ExecuteResult.isSuccess && ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
            {
                if (ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
                    return ExecuteResult;
            }

            if (null != vm.CreateModel.strAppointmentTime && vm.CreateModel.strAppointmentTime.Length > 0)
            {
                List<AppointmentTimeModel> Lstdata = new List<AppointmentTimeModel>();
                string[] arr1 = vm.CreateModel.strAppointmentTime.Trim().Split("|");

                for (int i = 0; i <= arr1.Length - 1; i++)
                {
                    string[] arr2 = arr1[i].Trim().Split(":");
                    string[] arr3 = arr2[1].Trim().Split(",");

                    for (int j = 0; j <= arr3.Length - 1; j++)
                    {
                        AppointmentTimeModel TimeModel = new AppointmentTimeModel();

                        TimeModel.ID = dt.QueryFieldByDT("ID");
                        TimeModel.Type = "03";
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

        public List<SelectListItem> GetAllNational()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM code WHERE Type = 'Citizenship'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllCounsellingStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM code WHERE Type = 'CounsellingStatus'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
