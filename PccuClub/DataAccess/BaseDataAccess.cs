using DataAccess;
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
    }
}
