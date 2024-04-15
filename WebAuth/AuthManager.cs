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
        /// 使用者使用帳號登入
        /// </summary>
        /// <param name="LoginId">帳號</param>
        /// <param name="Pwd">密碼</param>
        /// <param name="oUser">使用者資訊</param>
        /// <returns>驗證結果(True:驗證成功，False:驗證失敗)</returns>
        public bool Login(string LoginId, string Pwd, out UserInfo oUser, string LoginFrom)
        {
            oUser = null;
            try
            {
                UserInfo LoginUser = new UserInfo();
                
                // 密碼加密
                string EncryptPwd = EncryptionText(Pwd);

                // 查詢基本資料
                (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) mainResult = dbAccess.SelectUserMain(LoginId, EncryptPwd, LoginFrom);
                if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0)
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
                (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) funResult = dbAccess.SelectFunInfo(LoginId, LoginFrom);
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
        /// 使用者使用SSO登入
        /// </summary>
        /// <param name="LoginId">帳號</param>
        /// <param name="oUser">使用者資訊</param>
        /// <returns>驗證結果(True:驗證成功，False:驗證失敗)</returns>
        public bool FLogin(string FUserId, out UserInfo oUser)
        {
            oUser = null;
            try
            {
                UserInfo LoginUser = new UserInfo();

                // 查詢基本資料
                (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) mainResult = dbAccess.SelectFUserMain(FUserId);
                if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0)
                { return false; }

                LoginUser = mainResult.entitys.First();
                if (LoginUser == null)
                { return false; }

                // 查詢使用者角色
                (DbExecuteInfo Info, IEnumerable<RoleInfo> entitys) roleResult = dbAccess.SelectRoleInfo(LoginUser.LoginId);
                if (!mainResult.Info.isSuccess)
                { return false; }

				if (roleResult.entitys.ToList().Count == 0)
				{ return false; }

				LoginUser.UserRole = roleResult.entitys.ToList();

                // 查詢角色功能
                (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) funResult = dbAccess.SelectFunInfo(LoginUser.LoginId, "F");
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
        /// 新版使用者使用SSO登入(對應學生、老師、同仁)
        /// </summary>
        /// <param name="LoginId">帳號</param>
        /// <param name="oUser">使用者資訊</param>
        /// <returns>驗證結果(True:驗證成功，False:驗證失敗)</returns>
        public bool SSOLogin(string LoginId, out UserInfo oUser, string LoginType)
        {
            oUser = null;
            try
            {
                UserInfo LoginUser = new UserInfo();

                // 查詢基本資料
                (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) mainResult = LoginType == "F" ? dbAccess.SelectFUserMain(LoginId) : dbAccess.SelectUserMain(LoginId);
                
                if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0)
                { return false; }

                LoginUser = mainResult.entitys.First();
                if (LoginUser == null)
                { return false; }

                // 查詢使用者角色
                (DbExecuteInfo Info, IEnumerable<RoleInfo> entitys) roleResult = dbAccess.SelectRoleInfo(LoginUser.LoginId);
                if (!mainResult.Info.isSuccess)
                { return false; }

                LoginUser.UserRole = roleResult.entitys.ToList(); 

                // 查詢角色功能
                (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) funResult = LoginType == "F" ? dbAccess.SelectFunInfo(LoginUser.LoginId, "F") : dbAccess.SelectFunInfo(LoginUser.LoginId, "B");
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
            if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0)
            { return false; }

            oUser = mainResult.entitys.First();

            return true;
        }

        public bool GetUserByFUserID(string FUserId, out UserInfo oUser)
        {
            oUser = null;

            // 查詢基本資料
            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) mainResult = dbAccess.SelectFUserMain(FUserId);
            if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0)
            { return false; }

            oUser = mainResult.entitys.First();

            return true;
        }

        public bool GetUserByFLogin(string ClubId, out UserInfo oUser)
        {
            oUser = null;

            // 查詢基本資料
            (DbExecuteInfo Info, IEnumerable<UserInfo> entitys) mainResult = dbAccess.SelectFLoginUserMain(ClubId);
            if (!mainResult.Info.isSuccess || mainResult.entitys.Count() == 0)
            { return false; }

            oUser = mainResult.entitys.First();

            return true;
        }


        /// <summary>
        /// 取得功能清單
        /// </summary>
        /// <param name="BackOrFront">前台或後台或全部</param>
        /// <param name="LstFunInfo">功能清單List</param>
        /// <returns>驗證結果(True:驗證成功，False:驗證失敗)</returns>
        public bool SelectAllFunInfo(string BackOrFront, out List<FunInfo> LstFunInfo)
        {
            LstFunInfo = new List<FunInfo>();

            try
            {
                // 查詢功能
                (DbExecuteInfo Info, IEnumerable<FunInfo> entitys) funResult = dbAccess.SelectAllFunInfo(BackOrFront);
                if (!funResult.Info.isSuccess)
                { return false; }

                LstFunInfo = funResult.entitys.ToList();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SelectAllRoleFun(out List<RoleFunInfo> lstRoleFun)
        {
            lstRoleFun = new List<RoleFunInfo>();

            try
            {
                // 查詢功能
                (DbExecuteInfo Info, IEnumerable<RoleFunInfo> entitys) funResult = dbAccess.SelectAllFunInfo();
                if (!funResult.Info.isSuccess)
                { return false; }

                lstRoleFun = funResult.entitys.ToList();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string EncryptionText(string str)
        {
            EncryptUtil Encrypt = new EncryptUtil("WebPccuAuth");
            
            return Encrypt.Encrypt(str);
        }

    }
}