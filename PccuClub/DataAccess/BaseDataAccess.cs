using DataAccess;
using PccuClub.WebAuth;
using System.Data;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    public class BaseDataAccess : BaseAccess
    {
        public BaseDataAccess() : base() { }
        public BaseDataAccess(string ConnectionStringName) : base(ConnectionStringName) { }

        /// <summary> 儲存/更新 </summary>
        public DbExecuteInfo InsertLog(LogViewModel dbEntity)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("LoginID", dbEntity.LoginID);
            parameters.Add("UserName", dbEntity.UserName);
            parameters.Add("RoleName", dbEntity.RoleName);
            parameters.Add("IP", dbEntity.IP);
            parameters.Add("FunName", dbEntity.FunName);
            parameters.Add("ActionName", dbEntity.ActionName);
            #endregion 參數設定

            string CommendText = @"INSERT INTO Log_User_Action VALUES(@LoginID,@UserName,@RoleName,@IP,@FunName,@ActionName,GETDATE(),'SYSTEM')";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public string GetSystemCode(string Url)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            DataSet ds = new DataSet();

            string str = "";
            string CommendText = "";

            CommendText = $@"SELECT A.SystemCode
                                   FROM SystemFun A
                                  WHERE A.Url = '/' + '{Url}'";

            DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                str = ds.Tables[0].QueryFieldByDT("SystemCode");
            }

            return str;
        }

        public string GetUnitName(UserInfo userinfo, string SystemCode)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            DataSet ds = new DataSet();

            string str = "";
            string CommendText = "";

            CommendText = $@"SELECT C.SystemCode, C.RoleName AS UnitName
                               FROM UserMain A
                          LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                          LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                              WHERE 1 = 1
                                AND A.LoginId = '{userinfo.LoginId}' AND C.SystemCode = '{SystemCode}'";

            DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                str = ds.Tables[0].QueryFieldByDT("UnitName");
            }

            return str;
        }

        public string GetRemind(string ControllerName)
		{
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();
			DataSet ds = new DataSet();

            string str = "";
            string CommendText = "";

            if (ControllerName == "ClubInfo")
            {
				CommendText = "SELECT ClubInfoConform AS Content FROM ConformMang ";
			}
			else if (ControllerName == "ClubActReport")
			{
				CommendText = "SELECT ActivityConform AS Content FROM ConformMang ";
			}
            else if (ControllerName == "ClubActFinish")
            {
                CommendText = "SELECT ActFinishConform AS Content FROM ConformMang ";
            }

            DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            if (ds != null && ds.Tables[0].Rows.Count > 0) {
                str = ds.Tables[0].QueryFieldByDT("Content");
            }

			return str;
		}

        public string GetFunctionSource(string controllerName)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            DataSet ds = new DataSet();

            string str = "";
            string CommendText = "";

                CommendText = $@"SELECT B.BackOrFront
                                   FROM SystemFun A
                              LEFT JOIN SystemMenu B ON B.FunId = A.FunId
                                  WHERE A.Url = '/' + '{controllerName}'";

            DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                str = ds.Tables[0].QueryFieldByDT("BackOrFront");
            }

            return str;
        }


    }
}
