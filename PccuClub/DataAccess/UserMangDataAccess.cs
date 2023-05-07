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

    public class UserMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>
        public List<UserMangResultModel> GetSearchResult(UserMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@LoginId", model.LoginId);
            parameters.Add("@UserName", model.UserName);
            parameters.Add("@RoleId", model.RoleId);
            parameters.Add("@LifeClass", model.LifeClass);
            parameters.Add("@IsEnable", model.IsEnable);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.LoginId, A.UserName, A.Memo, A.IsEnable, A.LastModified, 
                                    B.RoleId, C.RoleName, D.LifeClass, E.Text AS LifeClassText, F.Text AS EnableText
                               FROM UserMain A
                          LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                          LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                          LEFT JOIN MatchLifeClass D ON D.MatchID = A.LoginId
                          LEFT JOIN CODE E ON E.Code = D.LifeClass AND E.Type = 'LifeClass'
                          LEFT JOIN CODE F ON F.Code = A.IsEnable AND F.Type = 'Enable'
                              WHERE 1 = 1
AND (A.UserType = '02')
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
{(model.LoginId != null ? " AND A.LoginId LIKE '%' + @LoginId + '%'" : " ")}
{(model.UserName != null ? " AND A.UserName LIKE '%' + @UserName + '%'" : " ")}
AND (@RoleId IS NULL OR B.RoleId = @RoleId)
AND (@LifeClass IS NULL OR D.LifeClass = @LifeClass)
AND (@IsEnable IS NULL OR A.IsEnable = @IsEnable)
";

            (DbExecuteInfo info, IEnumerable<UserMangResultModel> entitys) dbResult = DbaExecuteQuery<UserMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<UserMangResultModel>();
        }

        /// <summary>取得編輯資料 </summary>
        public UserMangEditModel GetEditData(string LoginId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@LoginId", LoginId);
            #endregion

            CommandText = $@"
                            SELECT A.LoginId, A.UserName, A.EMail, A.Memo, 
                                    A.IsEnable,
                                    A.Created, A.LastModified, B.RoleId, C.RoleName, D.LifeClass, E.Text AS LifeClassText, F.Text AS EnableText
                               FROM UserMain A
                          LEFT JOIN UserRole B ON B.LoginId = A.LoginId
                          LEFT JOIN SystemRole C ON C.RoleId = B.RoleId
                          LEFT JOIN MatchLifeClass D ON D.MatchID = A.LoginId
                          LEFT JOIN CODE E ON E.Code = D.LifeClass AND E.Type = 'LifeClass'
                          LEFT JOIN CODE F ON F.Code = A.IsEnable AND F.Type = 'Enable'
                              WHERE 1 = 1
AND (A.UserType = '01')
AND (A.LoginId = @LoginId)";


            (DbExecuteInfo info, IEnumerable<UserMangEditModel> entitys) dbResult = DbaExecuteQuery<UserMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        #region 新增


        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(UserMangViewModel vm, UserInfo LoginUser, string EncryptPw)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@IsEnable", vm.CreateModel.Enable);
            parameters.Add("@LifeClass", vm.CreateModel.LifeClass);
            parameters.Add("@RoleId", vm.CreateModel.RoleId);
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
                                                ,'02'
                                                ,@Memo
                                                ,@IsEnable
                                                ,@LastModifier
                                                ,GETDATE()
                                                ,@LastModifier
                                                ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改組別資料 </summary>
        public DbExecuteInfo InsertLifeClass(UserMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@MatchID", vm.CreateModel.LoginId.TrimStartAndEnd());
            parameters.Add("@LifeClass", vm.CreateModel.LifeClass);

            #endregion 參數設定

            CommendText = $@"INSERT INTO MatchLifeClass 
                                (MatchID, LifeClass) VALUES (@MatchID, @LifeClass) ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改角色資料 </summary>
        public DbExecuteInfo InsertRole(UserMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@LoginId", vm.CreateModel.LoginId.TrimStartAndEnd());
            parameters.Add("@RoleId", vm.CreateModel.RoleId);

            #endregion 參數設定

            CommendText = $@"INSERT INTO UserRole 
                                (RoleId, LoginId) VALUES (@RoleId, @LoginId) ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        #endregion

        #region 修改


        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(UserMangViewModel vm, UserInfo LoginUser, string EncryptPw)
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
                parameters.Add("@Memo", vm.EditModel.Memo.TrimStartAndEnd());

                parameters.Add("@LastModifier", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE UserMain 
                                SET Password = @Password,
                                    UserName = @UserName,
                                    EMail = @EMail,
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
                parameters.Add("@Memo", vm.EditModel.Memo.TrimStartAndEnd());

                parameters.Add("@LastModifier", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE UserMain 
                                SET UserName = @UserName,
                                    EMail = @EMail,
                                    Memo = @Memo,
                                    LastModifier = @LastModifier,
                                    LastModified = GETDATE()
                              WHERE LoginID = @LoginId ";
            }

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改組別資料 </summary>
        public DbExecuteInfo UpdateLifeClass(UserMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@LoginId", vm.EditModel.LoginId.TrimStartAndEnd());
            parameters.Add("@LifeClass", vm.EditModel.LifeClass);

            #endregion 參數設定

            CommendText = $@"UPDATE MatchLifeClass 
                                SET LifeClass = @LifeClass
                              WHERE MatchID = @LoginId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改角色資料 </summary>
        public DbExecuteInfo UpdateRole(UserMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@LoginId", vm.EditModel.LoginId.TrimStartAndEnd());
            parameters.Add("@RoleId", vm.EditModel.RoleId);

            #endregion 參數設定

            CommendText = $@"UPDATE UserRole 
                                SET RoleId = @RoleId
                              WHERE LoginId = @LoginId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

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
        /// 刪除組別資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeleteLifeClass(string LoginId)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@LoginId", LoginId);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM MatchLifeClass WHERE MatchID = @LoginId ";

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
        public List<UserMangResultModel> GetExportResult(UserMangConditionModel model)
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
FROM UserMang
WHERE 1 = 1
AND (@BuildName IS NULL OR BuildName LIKE '%' + @BuildName + '%') 
AND (@Note IS NULL OR Note LIKE '%' + @Note + '%') ";

            (DbExecuteInfo info, IEnumerable<UserMangResultModel> entitys) dbResult = DbaExecuteQuery<UserMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<UserMangResultModel>();
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

        public List<SelectListItem> GetAllLifeClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'LifeClass'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        #endregion

    }
}
