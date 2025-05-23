﻿using DataAccess;
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

            string SQL = @"SELECT A.FUserId, A.UserName, C.ClubId AS LoginId, A.EMail, A.CellPhone, A.Department, 
                                  C.ClubId, C.ClubCName, C.ClubEName, C.SchoolYear, C.LifeClass, C.ClubClass, 'F' AS LoginSource
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

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) SelectUserMain(string LoginId, bool isSSO)
        {
            DBAParameter parameter = new DBAParameter();
            string SQL = string.Empty;
            
            if (isSSO)
            {
                parameter.Add("@SSOAccount", LoginId);

                SQL = @"SELECT A.*, C.RoleName AS UnitName
                      FROM UserMain A
                 LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                 LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                     WHERE A.SSOAccount = @SSOAccount ";
            }
            else
            {
                parameter.Add("@LoginId", LoginId);

                SQL = @"SELECT A.*, C.RoleName AS UnitName
                      FROM UserMain A
                 LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                 LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                     WHERE A.LoginId = @LoginId ";
            }
            

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
                            WHERE A.LoginId = @LoginId AND A.Password = @Password AND A.IsEnable = 1";
            }
            else {
                SQL = @"SELECT A.*, A.ClubId AS LoginId, ISNULL(E.UserName, '') AS UserName, B.*, 'F' AS LoginSource
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
            
            string SQL = @"SELECT B.LoginId, A.RoleId, A.RoleName
                             FROM SystemRole A
                       INNER JOIN UserRole B on B.RoleId = A.RoleId
                            WHERE B.LoginId = @LoginId"
            ;
            (DbExecuteInfo Info, IEnumerable<RoleInfo> entitys) result = dbAccess.DbaExecuteQuery<RoleInfo>(SQL, parameter, false, null);
            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) SelectFunInfo(string LoginId, string BackOrFront)
        {
            DBAParameter parameter = new DBAParameter();
            
            parameter.Add("@LoginId", LoginId);
            parameter.Add("@BackOrFront", BackOrFront);

            string SQL = @"SELECT D.MenuNode, D.MenuName, D.MenuUpNode, D.IconTag, E.Url, D.IsEnable, D.IsVisIble, D.SortOrder, D.BackOrFront, D.SystemCode
                             FROM SystemRole A
                       INNER JOIN UserRole B on B.RoleId = A.RoleId
                       INNER JOIN SystemRoleFun C on C.RoleId = B.RoleId
                       INNER JOIN SystemMenu D on D.MenuNode = C.MenuNode
                       INNER JOIN SystemFun E on E.FunId = D.FunId
                            WHERE 1 = 1
                              AND B.LoginId = @LoginId
							  AND D.BackOrFront = @BackOrFront
                              AND D.IsEnable = 1
                         ORDER BY D.SortOrder";

            (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) result = dbAccess.DbaExecuteQuery<FunInfo>(SQL, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) SelectAllFunInfo(string BackOrFront = null)
        {
            DBAParameter parameter = new DBAParameter();

            parameter.Add("@BackOrFront", BackOrFront);

            string SQL = @"SELECT B.MenuNode, B.MenuUpNode, A.FunName, A.Url
                             FROM SystemFun A
                        LEFT JOIN SystemMenu B ON B.MenuNode = A.FunId AND B.SystemCode = A.SystemCode
                              AND (@BackOrFront IS NULL OR B.BackOrFront = @BackOrFront) ";

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

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) CheckBackendNewUser(SSOUserInfo sSOUserInfo)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            string CommandTxt = string.Empty;
            DBAParameter parameter = new DBAParameter();

            #region 參數設定
            // 登入帳號
            parameter.Add("SSOAccount", sSOUserInfo.Account);

            #endregion

            CommandTxt = $@"SELECT 1
                              FROM UserMain 
                             WHERE SSOAccount = @SSOAccount
";

            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) result = dbAccess.DbaExecuteQuery<UserInfo>(CommandTxt, parameter, false, null);

            return result;
        }

        public (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) CheckBackendUserEnable(SSOUserInfo sSOUserInfo, string BackOrFront)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            string CommandTxt = string.Empty;
            DBAParameter parameter = new DBAParameter();

            #region 參數設定
            // 登入帳號
            parameter.Add("SSOAccount", sSOUserInfo.Account);

            #endregion

            if (BackOrFront == "B")
            {
                CommandTxt = $@"SELECT IsEnable
                              FROM UserMain 
                             WHERE SSOAccount = @SSOAccount ";
            }
            else
            {
                CommandTxt = $@"SELECT IsEnable
                              FROM FUserMain 
                             WHERE FUserId = @SSOAccount ";
            }


            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) result = dbAccess.DbaExecuteQuery<UserInfo>(CommandTxt, parameter, false, null);

            return result;
        }
    }
}
