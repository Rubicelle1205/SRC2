using DataAccess;
using WebAuth.Entity;
using PccuClub.WebAuth;
using MathNet.Numerics.RootFinding;


namespace WebAuth.DataAccess
{
    internal class AuthDataAccess : MsSqlDBAccess
    {
        private IDBAccess dbAccess = new BaseAccess();

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) SelectFUserMain(string FUserId)
        {
            DBAParameter parameter = new DBAParameter();
            parameter.Add("@FUserId", FUserId);

            string SQL = @"SELECT A.FUserId AS LoginId, A.UserName, C.ClubId AS LoginId, A.EMail, A.CellPhone, A.Department, C.ClubId, C.ClubCName, C.ClubEName, C.SchoolYear, C.LifeClass, C.ClubClass, 'F' AS LoginSource
                             FROM FUserMain A
                        LEFT JOIN ClubUser B ON B.FUserId = A.FUserId
                        LEFT JOIN ClubMang C ON C.ClubId = B.ClubId
                        LEFT JOIN UserRole D ON D.LoginId = C.ClubId
                        LEFT JOIN SystemRole E ON E.RoleId = D.RoleId
                            WHERE A.FUserId = @FUserId AND A.IsEnable = 1
 ";

            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) result = dbAccess.DbaExecuteQuery<UserInfo>(SQL, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) SelectFLoginUserMain(string ClubId)
        {
            DBAParameter parameter = new DBAParameter();
            parameter.Add("@ClubId", ClubId);

            string SQL = @"SELECT A.*
                             FROM ClubMang A
                            WHERE A.ClubId = @ClubId
 ";

            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) result = dbAccess.DbaExecuteQuery<UserInfo>(SQL, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) SelectUserMain(string LoginId)
        {
            DBAParameter parameter = new DBAParameter();
            parameter.Add("@LoginId", LoginId);

            string SQL = @"SELECT A.*, C.RoleName AS UnitName
                             FROM UserMain A
                        LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                        LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                            WHERE A.LoginId = @LoginId ";

            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) result = dbAccess.DbaExecuteQuery<UserInfo>(SQL, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) SelectUserMain(string LoginId, string EncryptPwd, string LoginFrom)
        {
            DBAParameter parameter = new DBAParameter();
            parameter.Add("@LoginId", LoginId);
            parameter.Add("@Password", EncryptPwd);

            string SQL = string.Empty;
            if (LoginFrom == "B"){
                SQL = @"SELECT A.*, C.RoleName AS UnitName, 'B' AS LoginSource
                             FROM UserMain A
                        LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                        LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                            WHERE A.LoginId = @LoginId AND A.Password = @Password and A.IsEnable = 1";
            }
            else {
                SQL = @"SELECT A.*, A.ClubId AS LoginId, E.UserName AS UserName, B.*, 'F' AS LoginSource
                             FROM ClubMang A
                        LEFT JOIN UserRole B ON B.LoginId = A.ClubId
                        LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
						LEFT JOIN ClubUser D ON D.ClubId = A.ClubId
						LEFT JOIN FUserMain E ON E.FUserId = D.FUserId
                            WHERE A.ClubId = @LoginId AND A.Password = @Password and A.IsEnable = 1";
            }
          

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

        public (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) SelectFunInfo(string LoginId, string BackOrFront)
        {
            DBAParameter parameter = new DBAParameter();
            
            parameter.Add("@LoginId", LoginId);
            parameter.Add("@BackOrFront", BackOrFront);

            string SQL = @"select SM.MenuNode, SM.MenuName, SM.MenuUpNode, SM.IconTag, F.Url, SM.IsEnable, SM.IsVisIble, SM.SortOrder
                           from SystemRole SR
                           inner join UserRole R on R.RoleId=SR.RoleId
                           inner join SystemRoleFun RF on RF.RoleId=R.RoleId
                           inner join SystemMenu SM on SM.MenuNode=RF.MenuNode
                           inner join SystemFun F on F.FunId=SM.FunId
                           where 1 = 1
                             AND R.LoginId = @LoginId 
                             AND SM.BackOrFront = @BackOrFront
                             AND SM.IsEnable = 1
                        ORDER BY SM.SortOrder";

            (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) result = dbAccess.DbaExecuteQuery<FunInfo>(SQL, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) SelectAllFunInfo(string BackOrFront = null)
        {
            DBAParameter parameter = new DBAParameter();

            parameter.Add("@BackOrFront", BackOrFront);

            string SQL = @"select SM.MenuNode, SM.MenuName, SM.MenuUpNode, SM.IconTag, F.Url, SM.IsEnable, SM.IsVisIble, SM.SortOrder
                           from SystemRole SR
                           inner join UserRole R on R.RoleId=SR.RoleId
                           inner join SystemRoleFun RF on RF.RoleId=R.RoleId
                           inner join SystemMenu SM on SM.MenuNode=RF.MenuNode
                           inner join SystemFun F on F.FunId=SM.FunId
                           where 1 = 1
                           AND (@BackOrFront IS NULL OR SM.BackOrFront = @BackOrFront) ";

            (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) result = dbAccess.DbaExecuteQuery<FunInfo>(SQL, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<RoleFunInfo> entitys) SelectAllFunInfo()
        {
            DBAParameter parameter = new DBAParameter();

            

            string SQL = @"select * from SystemRoleFun ";

            (DbExecuteInfo Info, IEnumerable<RoleFunInfo> entitys) result = dbAccess.DbaExecuteQuery<RoleFunInfo>(SQL, parameter, false, null);

            return result;
        }
    }
}
