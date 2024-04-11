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

namespace WebPccuClub.DataAccess
{

    public class HyperAdminMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>
        public List<HyperAdminMangResultModel> GetSearchResult(HyperAdminMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@LoginId", model.LoginId);
            parameters.Add("@UserName", model.UserName);
            parameters.Add("@RoleId", model.RoleId);
            parameters.Add("@IsEnable", model.IsEnable);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.LoginId, A.UserName, A.Memo, A.IsEnable, A.LastModified, 
                                    B.RoleId, C.RoleName, F.Text AS EnableText
                               FROM UserMain A
                          LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                          LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                          LEFT JOIN CODE F ON F.Code = A.IsEnable AND F.Type = 'Enable'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
{(model.LoginId != null ? " AND A.LoginId LIKE '%' + @LoginId + '%'" : " ")}
{(model.UserName != null ? " AND A.UserName LIKE '%' + @UserName + '%'" : " ")}
AND (@RoleId IS NULL OR B.RoleId = @RoleId)
AND (@IsEnable IS NULL OR A.IsEnable = @IsEnable)
";

            (DbExecuteInfo info, IEnumerable<HyperAdminMangResultModel> entitys) dbResult = DbaExecuteQuery<HyperAdminMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HyperAdminMangResultModel>();
        }

        /// <summary>取得編輯資料 </summary>
        public HyperAdminMangEditModel GetEditData(string LoginId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@LoginId", LoginId);
            #endregion

            CommandText = $@"
                            SELECT A.LoginId, A.UserName, A.EMail, A.UserType, A.Memo, A.IsEnable, A.Created, A.LastModified, 
	                               B.RoleId AS RoleClub, F.RoleName AS RoleClubText, C.RoleId AS RoleCase, G.RoleName AS RoleCaseText, 
	                               D.RoleId AS RoleBorrow, H.RoleName AS RoleBorrowText, E.RoleId AS RoleConsultation, I.RoleName AS RoleConsultationText, 
	                               J.Text AS EnableText
                              FROM UserMain A
                         LEFT JOIN UserRole B ON B.LoginId = A.LoginId AND B.SystemCode = '02'
						 LEFT JOIN UserRole C ON C.LoginId = A.LoginId AND C.SystemCode = '03'
						 LEFT JOIN UserRole D ON D.LoginId = A.LoginId AND D.SystemCode = '04'
						 LEFT JOIN UserRole E ON E.LoginId = A.LoginId AND E.SystemCode = '05'
                         LEFT JOIN SystemRole F ON F.RoleId = B.RoleId AND F.SystemCode = '02'
						 LEFT JOIN SystemRole G ON G.RoleId = C.RoleId AND G.SystemCode = '03'
						 LEFT JOIN SystemRole H ON H.RoleId = D.RoleId AND H.SystemCode = '04'
						 LEFT JOIN SystemRole I ON I.RoleId = E.RoleId AND I.SystemCode = '05'
                         LEFT JOIN CODE J ON J.Code = A.IsEnable AND J.Type = 'Enable'
                             WHERE 1 = 1
                               AND (A.LoginId = @LoginId)";


            (DbExecuteInfo info, IEnumerable<HyperAdminMangEditModel> entitys) dbResult = DbaExecuteQuery<HyperAdminMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        #region 新增


        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(HyperAdminMangViewModel vm, UserInfo LoginUser, string EncryptPw)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@UserType", vm.CreateModel.UserType == "01" ? "03" : "01");
            parameters.Add("@IsEnable", vm.CreateModel.IsEnable);
            parameters.Add("@LoginId", vm.CreateModel.LoginId.TrimStartAndEnd());
            parameters.Add("@Password", EncryptPw);
            parameters.Add("@UserName", vm.CreateModel.UserName.TrimStartAndEnd());
            parameters.Add("@EMail", vm.CreateModel.EMail.TrimStartAndEnd());
            parameters.Add("@Memo", vm.CreateModel.Memo.TrimStartAndEnd());

            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO UserMain
                                                (LoginId
                                                ,Password
                                                ,UserName
                                                ,EMail
                                                ,UserType
                                                ,Memo
                                                ,IsEnable
                                                ,Creator
                                                ,Created
                                                ,LastModifier
                                                ,LastModified)
                                            VALUES
                                                 (@LoginId
                                                ,@Password
                                                ,@UserName
                                                ,@EMail
                                                ,@UserType
                                                ,@Memo
                                                ,@IsEnable
                                                ,@LastModifier
                                                ,GETDATE()
                                                ,@LastModifier
                                                ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改角色資料 </summary>
        public DbExecuteInfo InsertRole(HyperAdminMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@LoginId", vm.CreateModel.LoginId.TrimStartAndEnd());

            parameters.Add("@RoleClub", vm.CreateModel.RoleClub);
            parameters.Add("@RoleCase", vm.CreateModel.RoleCase);
            parameters.Add("@RoleBorrow", vm.CreateModel.RoleBorrow);
            parameters.Add("@RoleConsultation", vm.CreateModel.RoleConsultation);

            #endregion 參數設定

            #region HyperUser

            CommendText = $@"DELETE FROM UserRole WHERE LoginId = @LoginId AND SystemCode = '01'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (vm.EditModel.UserType == "01")
            {
                CommendText = $@"INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, 'hyperuser', '01')";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            #region 社團

            if (!string.IsNullOrEmpty(vm.CreateModel.RoleClub))
            {
                CommendText = $@"INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, @RoleClub, '02')";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            #region 案件系統

            if (!string.IsNullOrEmpty(vm.CreateModel.RoleCase))
            {
                CommendText = $@"INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, @RoleCase, '03')";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            #region 資源借用

            if (!string.IsNullOrEmpty(vm.CreateModel.RoleBorrow))
            {
                CommendText = $@"INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, @RoleBorrow, '04')";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            #region 輔導諮商

            if (!string.IsNullOrEmpty(vm.CreateModel.RoleConsultation))
            {
                CommendText = $@"INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, @RoleConsultation, '05')";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            return ExecuteResult;
        }

        #endregion

        #region 修改


        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(HyperAdminMangViewModel vm, UserInfo LoginUser, string EncryptPw)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            if (!string.IsNullOrEmpty(EncryptPw))
            {
                #region 參數設定
                parameters.Add("@IsEnable", vm.EditModel.IsEnable);
                parameters.Add("@LoginId", vm.EditModel.LoginId.TrimStartAndEnd());
                parameters.Add("@Password", EncryptPw);
                parameters.Add("@UserName", vm.EditModel.UserName.TrimStartAndEnd());
                parameters.Add("@EMail", vm.EditModel.EMail.TrimStartAndEnd());
                parameters.Add("@IsEnable", vm.EditModel.IsEnable == "True" ? "1" : "0");
                parameters.Add("@UserType", vm.EditModel.UserType == "01" ? "03" : "01");
                parameters.Add("@Memo", vm.EditModel.Memo.TrimStartAndEnd());

                parameters.Add("@LastModifier", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE UserMain 
                                SET Password = @Password,
                                    UserName = @UserName,
                                    EMail = @EMail,
                                    IsEnable = @IsEnable,
                                    UserType = @UserType,
                                    Memo = @Memo,
                                    LastModifier = @LastModifier,
                                    LastModified = GETDATE()
                              WHERE LoginID = @LoginId ";
            }
            else
            {
                #region 參數設定
                parameters.Add("@IsEnable", vm.EditModel.IsEnable);
                parameters.Add("@LoginId", vm.EditModel.LoginId.TrimStartAndEnd());
                parameters.Add("@UserName", vm.EditModel.UserName.TrimStartAndEnd());
                parameters.Add("@EMail", vm.EditModel.EMail.TrimStartAndEnd());
                parameters.Add("@IsEnable", vm.EditModel.IsEnable == "True" ? "1" : "0");
                parameters.Add("@UserType", vm.EditModel.UserType == "01" ? "03" : "01");

                parameters.Add("@Memo", vm.EditModel.Memo.TrimStartAndEnd());

                parameters.Add("@LastModifier", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE UserMain 
                                SET UserName = @UserName,
                                    EMail = @EMail,
                                    IsEnable = @IsEnable,
                                    UserType = @UserType,
                                    Memo = @Memo,
                                    LastModifier = @LastModifier,
                                    LastModified = GETDATE()
                              WHERE LoginID = @LoginId ";
            }

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改角色資料 </summary>
        public DbExecuteInfo UpdateRole(HyperAdminMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@LoginId", vm.EditModel.LoginId.TrimStartAndEnd());

            parameters.Add("@RoleClub", vm.EditModel.RoleClub);
            parameters.Add("@RoleCase", vm.EditModel.RoleCase);
            parameters.Add("@RoleBorrow", vm.EditModel.RoleBorrow);
            parameters.Add("@RoleConsultation", vm.EditModel.RoleConsultation);

            #endregion 參數設定

            #region HyperUser

            CommendText = $@"DELETE FROM UserRole WHERE LoginId = @LoginId AND SystemCode = '01'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (vm.EditModel.UserType == "01")
            {
                CommendText = $@"INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, 'hyperuser', '01')";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            #region 社團

            if (!string.IsNullOrEmpty(vm.EditModel.RoleClub))
            {
                CommendText = $@"IF EXISTS (SELECT 1 FROM UserRole WHERE 1 = 1 AND LoginId = @LoginId AND SystemCode = '02')
                                     BEGIN
                                            UPDATE UserRole
                                            SET RoleId = @RoleClub
                                            WHERE LoginId = @LoginId  AND SystemCode = '02';
                                        END
                                    ELSE
                                        BEGIN
                                            INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, @RoleClub, '02');
                                        END";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            #region 案件系統

            if (!string.IsNullOrEmpty(vm.EditModel.RoleCase))
            {
                CommendText = $@"IF EXISTS (SELECT 1 FROM UserRole WHERE 1 = 1 AND LoginId = @LoginId AND SystemCode = '03')
                                     BEGIN
                                            UPDATE UserRole
                                            SET RoleId = @RoleCase
                                            WHERE LoginId = @LoginId  AND SystemCode = '03';
                                        END
                                    ELSE
                                        BEGIN
                                            INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, @RoleCase, '03');
                                        END";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            #region 資源借用

            if (!string.IsNullOrEmpty(vm.EditModel.RoleBorrow))
            {
                CommendText = $@"IF EXISTS (SELECT 1 FROM UserRole WHERE 1 = 1 AND LoginId = @LoginId AND SystemCode = '04')
                                     BEGIN
                                            UPDATE UserRole
                                            SET RoleId = @RoleBorrow
                                            WHERE LoginId = @LoginId  AND SystemCode = '04';
                                        END
                                    ELSE
                                        BEGIN
                                            INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, @RoleBorrow, '04');
                                        END";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            #region 輔導諮商

            if (!string.IsNullOrEmpty(vm.EditModel.RoleConsultation))
            {
                CommendText = $@"IF EXISTS (SELECT 1 FROM UserRole WHERE 1 = 1 AND LoginId = @LoginId AND SystemCode = '05')
                                     BEGIN
                                            UPDATE UserRole
                                            SET RoleId = @RoleConsultation
                                            WHERE LoginId = @LoginId  AND SystemCode = '05';
                                        END
                                    ELSE
                                        BEGIN
                                            INSERT INTO UserRole (LoginId, RoleId, SystemCode) VALUES (@LoginId, @RoleConsultation, '05');
                                        END";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (ExecuteResult.isSuccess == false) { return ExecuteResult; }
            }

            #endregion

            return ExecuteResult;
        }

        #endregion

        #region 刪除


        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeletetData(string LoginId)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@LoginId", LoginId);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM UserMain WHERE LoginId = @LoginId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 刪除角色資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeleteRole(string LoginId)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@LoginId", LoginId);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM UserRole WHERE LoginId = @LoginId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        #endregion


        /// <summary>Excel 取得資料</summary>
        public List<HyperAdminMangResultModel> GetExportResult(HyperAdminMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            //parameters.Add("@BuildName", model?.BuildName);
            //parameters.Add("@Note", model?.Note);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT BuildID, BuildName, Note, Creator, Created, LastModifier, LastModified, ModifiedReason
FROM HyperAdminMang
WHERE 1 = 1
AND (@BuildName IS NULL OR BuildName LIKE '%' + @BuildName + '%') 
AND (@Note IS NULL OR Note LIKE '%' + @Note + '%') ";

            (DbExecuteInfo info, IEnumerable<HyperAdminMangResultModel> entitys) dbResult = DbaExecuteQuery<HyperAdminMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HyperAdminMangResultModel>();
        }

        #region GetData

        public List<SelectListItem> GetAllRole()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT RoleId AS VALUE, RoleName AS TEXT FROM SystemRole";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetIsEnable()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'Enable'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetSystemCode()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'SystemCode' AND Code <> '01'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetYesOrNo()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'YesOrNo'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetRoleData(string SystemCode)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@SystemCode", SystemCode);

            #endregion

            CommandText = @"SELECT A.RoleId AS VALUE, A.RoleName AS TEXT
                              FROM SystemRole A
                             WHERE A.SystemCode = @SystemCode ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        #endregion

    }
}
