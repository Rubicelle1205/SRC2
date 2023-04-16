using DataAccess;
using WebAuth.Entity;
using PccuClub.WebAuth;


namespace WebAuth.DataAccess
{
    internal class AuthDataAccess : MsSqlDBAccess
    {
        private IDBAccess dbAccess = new BaseAccess();

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) SelectUserMain(string LoginId)
        {
            DBAParameter parameter = new DBAParameter();
            parameter.Add("@LoginId", LoginId);
            string SQL = @"--declare @LoginId nvarchar(30)
                           --set @LoginId=''

                           select M.*, U.Name UnitName
                           from UserMain M
                           left join Unit U on U.UnitId=M.UnitId
                           where LoginId=@LoginId";

            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) result = dbAccess.DbaExecuteQuery<UserInfo>(SQL, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) SelectUserMain(string LoginId, string EncryptPwd)
        {
            DBAParameter parameter = new DBAParameter();
            parameter.Add("@LoginId", LoginId);
            parameter.Add("@Password", EncryptPwd);
            string SQL = @"--declare @LoginId nvarchar(30), @Password nvarchar(30)
                           --set @LoginId=''
                           --set @Password=''

                           select M.*, U.Name UnitName
                           from UserMain M
                           left join Unit U on U.UnitId=M.UnitId
                           where LoginId=@LoginId and Password=@Password and IsEnable=1";

            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) result = dbAccess.DbaExecuteQuery<UserInfo>(SQL, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<RoleInfo> entityList) SelectRoleInfo(string LoginId)
        {
            DBAParameter parameter = new DBAParameter();
            parameter.Add("@LoginId", LoginId);
            string SQL = @"--declare @LoginId nvarchar(30)
                           --set @LoginId=''

                           select SR.RoleId, SR.RoleName
                           from SystemRole SR
                           inner join UserRole R on R.RoleId=SR.RoleId
                           where R.LoginId=@LoginId"
            ;
            (DbExecuteInfo Info, IEnumerable<RoleInfo> entitys) result = dbAccess.DbaExecuteQuery<RoleInfo>(SQL, parameter, false, null);
            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) SelectFunInfo(string LoginId)
        {
            DBAParameter parameter = new DBAParameter();
            parameter.Add("@LoginId", LoginId);
            string SQL = @"select SM.MenuNode, SM.MenuName, SM.MenuUpNode, SM.IconTag, F.Url, SM.IsEnable, SM.IsVisIble, SM.SortOrder
                           from SystemRole SR
                           inner join UserRole R on R.RoleId=SR.RoleId
                           inner join SystemRoleFun RF on RF.RoleId=R.RoleId
                           inner join SystemMenu SM on SM.MenuNode=RF.MenuNode
                           inner join SystemFun F on F.FunId=SM.FunId
                           where R.LoginId=@LoginId";

            (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) result = dbAccess.DbaExecuteQuery<FunInfo>(SQL, parameter, false, null);

            return result;
        }
    }
}
