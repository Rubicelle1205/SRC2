using DataAccess;
using WebPccuClub.Entity;
using PccuClub.WebAuth;
using WebPccuClub.Global;

namespace WebPccuClub.DataAccess
{
    public class LoginDataAccess : BaseAccess
    {
        /// <summary> 新增 </summary>
        public DbExecuteInfo Insert(LoginLogEntity dbEntity)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            string CommandText = string.Empty;

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            // 使用者帳號
            parameters.Add("Loginid", dbEntity.Loginid == null ? "" : dbEntity.Loginid);
            // 登入時間
            parameters.Add("Logintime", dbEntity.Logintime);
            // 登入IP
            parameters.Add("Ip", dbEntity.Ip);
            // 說明
            parameters.Add("Memo", dbEntity.Memo);
            // 登入結果
            parameters.Add("Issuccess", dbEntity.Issuccess);
            #endregion 參數設定

            #region CommandText
            CommandText = @"insert into UserLoginLog
					        (
						        Loginid,
						        Logintime,
						        Ip,
						        Memo,
						        Issuccess
					        )
					        values
					        (
						        @Loginid,
						        @Logintime,
						        @Ip,
						        @Memo,
						        @Issuccess
					        )";

            #endregion CommandText

            ExecuteResult = DbaExecuteNonQuery(CommandText, parameters, true, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 登入動作新增 </summary>
        public DbExecuteInfo ActionInsert(LoginLogEntity thisEntity, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            string CommandText = string.Empty;

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            // 使用者帳號
            parameters.Add("Loginid", thisEntity.Loginid);
            // 使用者姓名
            parameters.Add("UserName", LoginUser.UserName);
            // 角色姓名
            parameters.Add("RoleName", LoginUser.UserRole[0].RoleName);
            // 登入IP
            parameters.Add("IP", thisEntity.Ip);
            // 功能
            parameters.Add("FunName", "系統登入");
            // 動作
            parameters.Add("ActionName", "登入");
            // 建立時間
            parameters.Add("Create_Date", thisEntity.Logintime);
            // 建立者
            parameters.Add("Create_By", "SYSTEM");
            #endregion 參數設定

            #region CommandText
            CommandText = @"insert into Log_User_Action
					        (
                                LoginId, 
                                UserName, 
                                RoleName, 
                                IP, 
                                FunName, 
                                ActionName, 
                                Create_Date, 
                                Create_By
					        )
					        values
					        (
                                @LoginId, 
                                @UserName, 
                                @RoleName, 
                                @IP, 
                                @FunName, 
                                @ActionName, 
                                @Create_Date, 
                                @Create_By
					        )";

            #endregion CommandText

            ExecuteResult = DbaExecuteNonQuery(CommandText, parameters, true, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 更新使用者登入資訊 </summary>
        public DbExecuteInfo UpdateEntity(UserInfo dbEntity)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            string CommandTxt = string.Empty;
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            // 登入帳號
            parameters.Add("Loginid", dbEntity.LoginId);
            // 最後登入時間
            parameters.Add("Lastlogindate", dbEntity.LastLoginDate);
            // 記錄登入錯誤次數
            parameters.Add("Errorcount", dbEntity.ErrorCount);
            // 最後異動者
            parameters.Add("Lastmodifier", dbEntity.LastModifier);
            // 最後修改日期時間
            parameters.Add("Lastmodified", dbEntity.LastModified);
            // 修改原因
            parameters.Add("Modifiedreason", dbEntity.ModifiedReason);
            #endregion 參數設定


            #region CommandText
            CommandTxt = @"update UserMain set 
						Lastlogindate=ISNULL(@Lastlogindate,Lastlogindate),
						Errorcount=ISNULL(@Errorcount,Errorcount),
						Lastmodifier=ISNULL(@Lastmodifier,Lastmodifier),
						Lastmodified=ISNULL(@Lastmodified,Lastmodified),
						Modifiedreason=ISNULL(@Modifiedreason,Modifiedreason)
					where
						Loginid=@Loginid";
            #endregion CommandText

            ExecuteResult = DbaExecuteNonQuery(CommandTxt, parameters, true, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 重製密碼 </summary>
        public DbExecuteInfo SetToDefaultPwd(UserInfo dbEntity, string newPwd)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            string CommandTxt = string.Empty;
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            // 登入帳號
            parameters.Add("Loginid", dbEntity.LoginId);

            #endregion 參數設定


            #region CommandText
            CommandTxt = $@"UPDATE UserMain SET 
						Password = '{newPwd}'
					WHERE
						Loginid = @Loginid";
            #endregion CommandText

            ExecuteResult = DbaExecuteNonQuery(CommandTxt, parameters, true, DBAccessException);

            return ExecuteResult;
        }
    }
}
