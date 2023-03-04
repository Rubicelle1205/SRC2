using DataAccess;
using Utility;
using WebAuth.DataAccess;
using WebAuth.Entity;

namespace PccuClub.WebAuth
{
    public class AuthManager
    {
        private AuthDataAccess dbAccess = new AuthDataAccess();

        /// <summary> 
        /// 使用者登入
        /// </summary>
        /// <param name="LoginId">帳號</param>
        /// <param name="Pwd">密碼</param>
        /// <param name="oUser">使用者資訊</param>
        /// <returns>驗證結果(True:驗證成功，False:驗證失敗)</returns>
        public bool Login(string LoginId, string Pwd, out UserInfo oUser)
        {
            oUser = null;
            try
            {
                UserInfo LoginUser = new UserInfo();
                EncryptUtil Encrypt = new EncryptUtil("WebSolSystemAuth");
                // 密碼加密
                string EncryptPwd = Encrypt.Encrypt(Pwd);

                // 查詢基本資料
                (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) mainResult = dbAccess.SelectUserMain(LoginId, EncryptPwd);
                if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0 || mainResult.entitys.Count() > 1)
                { return false; }

                LoginUser = mainResult.entitys.First();
                if (LoginUser == null)
                { return false; }

                // 查詢使用者角色
                (DbExecuteInfo Info, IEnumerable<RoleInfo> entitys) reolResult = dbAccess.SelectRoleInfo(LoginId);
                if (!mainResult.Info.isSuccess)
                { return false; }
                LoginUser.UserRole = reolResult.entitys.ToList();

                // 查詢角色功能
                (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) funResult = dbAccess.SelectFunInfo(LoginId);
                if (!funResult.Info.isSuccess)
                { return false; }
                LoginUser.UserRoleFun = funResult.entitys.ToList();

                oUser = LoginUser;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="LoginId">帳號</param>
        /// <param name="oUser">使用者資訊</param>
        /// <returns>驗證結果(True:驗證成功，False:驗證失敗)</returns>
        public bool Login(string LoginId, out UserInfo oUser)
        {
            oUser = null;
            try
            {
                UserInfo LoginUser = new UserInfo();

                // 查詢基本資料
                (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) mainResult = dbAccess.SelectUserMain(LoginId);
                if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0 || mainResult.entitys.Count() > 1)
                { return false; }

                LoginUser = mainResult.entitys.First();
                if (LoginUser == null)
                { return false; }

                // 查詢使用者角色
                (DbExecuteInfo Info, IEnumerable<RoleInfo> entitys) reolResult = dbAccess.SelectRoleInfo(LoginId);
                if (!mainResult.Info.isSuccess)
                { return false; }
                LoginUser.UserRole = reolResult.entitys.ToList();

                // 查詢角色功能
                (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) funResult = dbAccess.SelectFunInfo(LoginId);
                if (!funResult.Info.isSuccess)
                { return false; }
                LoginUser.UserRoleFun = funResult.entitys.ToList();

                oUser = LoginUser;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="LoginId">帳號</param>
        /// <returns>True:存在，False:不存在</returns>
        public bool GetUserMain(string LoginId, out UserInfo oUser)
        {
            oUser = null;

            // 查詢基本資料
            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) mainResult = dbAccess.SelectUserMain(LoginId);
            if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0 || mainResult.entitys.Count() > 1)
            { return false; }

            oUser = mainResult.entitys.First();

            return true;
        }

    }
}