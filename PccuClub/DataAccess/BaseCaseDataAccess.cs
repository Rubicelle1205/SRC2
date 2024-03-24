using DataAccess;
using System.Data;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    public class BaseCaseDataAccess : BaseAccess
    {
        public BaseCaseDataAccess() : base() { }
        public BaseCaseDataAccess(string ConnectionStringName) : base(ConnectionStringName) { }

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
