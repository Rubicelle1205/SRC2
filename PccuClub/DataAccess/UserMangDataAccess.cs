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
using MathNet.Numerics.RootFinding;
using System.Runtime.ConstrainedExecution;

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

            parameters.Add("@Clubid", model.Clubid);
            parameters.Add("@UserName", model.UserName);
            parameters.Add("@RoleId", model.RoleId);
            parameters.Add("@LifeClass", model.LifeClass);
            parameters.Add("@CellPhone", model.CellPhone);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.Clubid, E.RoleName, A.ClubCName, A.ClubEName, A.SchoolYear, A.LifeClass, F.Text AS LifeClassName,
                                    A.ClubClass, G.Text AS ClubClassText, C.UserName, C.CellPhone, C.EMail, C.LastLoginDate, C.IsEnable,
                                    CASE C.IsEnable WHEN 1 THEN '啟用' WHEN 0 THEN '停用' ELSE '' END IsEnableText
                               FROM ClubMang A
                          LEFT JOIN Clubuser B on B.Clubid = A.Clubid
                          LEFT JOIN FUsermain C on C.FUserid = B.FUserid
						  LEFT JOIN UserRole D on D.LoginId = A.ClubId
						  LEFT JOIN SystemRole E ON E.RoleId = D.RoleId
                          LEFT JOIN Code F ON F.Code = A.LifeClass AND F.Type = 'LifeClass'
                          LEFT JOIN Code G ON G.Code = A.ClubClass AND G.Type = 'ClubClass'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND C.LastLoginDate BETWEEN @FromDate AND @ToDate" : " ")}
{(model.Clubid != null ? " AND A.Clubid LIKE '%' + @Clubid + '%'" : " ")}
{(model.UserName != null ? " AND C.UserName LIKE '%' + @UserName + '%'" : " ")}
{(model.CellPhone != null ? " AND C.CellPhone LIKE '%' + @CellPhone + '%'" : " ")}
AND (@RoleId IS NULL OR E.RoleId = @RoleId)
AND (@LifeClass IS NULL OR A.LifeClass = @LifeClass)

";

            (DbExecuteInfo info, IEnumerable<UserMangResultModel> entitys) dbResult = DbaExecuteQuery<UserMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<UserMangResultModel>();
        }

        /// <summary>取得編輯資料 </summary>
        public UserMangEditModel GetEditData(string ClubId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubId", ClubId);
            #endregion

            CommandText = $@"SELECT A.ClubId, A.ClubCName, A.ClubEName, 
	                                C.FUserId, C.FUserId AS OldFUserId, C.UserName, C.EMail, CellPhone, C.Department, C.Created, C.LastLoginDate, C.Memo, C.IsEnable, C.LastModified
                               FROM ClubMang A
                          LEFT JOIN ClubUser B on B.ClubId = A.ClubId
                          LEFT JOIN FUserMain C on C.FUserId = B.FUserId
                              WHERE A.ClubId = @ClubId";


            (DbExecuteInfo info, IEnumerable<UserMangEditModel> entitys) dbResult = DbaExecuteQuery<UserMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public DataTable GetFUserData(string FUserId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@FUserId", FUserId);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT *
                              FROM FUserMain
                             WHERE 1 = 1
                               AND FUserId = @FUserId";


            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }

        #region 新增


        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(UserMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubId", vm.EditModel.ClubId);
            parameters.Add("@FUserId", vm.EditModel.FUserId);
            parameters.Add("@OldFUserId", vm.EditModel.OldFUserId);
            parameters.Add("@IsEnable", vm.EditModel.IsEnable);
            parameters.Add("@UserName", vm.EditModel.UserName);
            parameters.Add("@Department", vm.EditModel.Department);
            parameters.Add("@EMail", vm.EditModel.EMail);
            parameters.Add("@CellPhone", vm.EditModel.CellPhone);
            parameters.Add("@Memo", vm.EditModel.Memo);

            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = string.Empty;

            CommendText = $@"UPDATE FUserMain SET IsEnable = 0 WHERE FUserId = @OldFUserId";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (ExecuteResult.isSuccess)
            {

                CommendText = $@"INSERT INTO FUserMain
                                                (FUserId 
                                                ,UserName 
                                                ,EMail 
                                                ,CellPhone 
                                                ,Department 
                                                ,Memo 
                                                ,IsEnable 
                                                ,Creator 
                                                ,Created 
                                                ,LastModifier 
                                                ,LastModified)
                                            VALUES
                                                 (@FUserId 
                                                ,@UserName 
                                                ,@EMail 
                                                ,@CellPhone
                                                ,@Department
                                                ,@Memo 
                                                ,@IsEnable 
                                                ,@LastModifier
                                                ,GETDATE()
                                                ,@LastModifier
                                                ,GETDATE())";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);
            }
            return ExecuteResult;
        }

        #endregion

        #region 修改

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateUserClub(UserMangViewModel vm)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定
            parameters.Add("@ClubId", vm.EditModel.ClubId);
            parameters.Add("@FUserId", vm.EditModel.FUserId);
            #endregion 參數設定

            CommendText = $@"DELETE ClubUser WHERE ClubId = @ClubId";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (ExecuteResult.isSuccess || ExecuteResult.ErrorCode == dbErrorCode._EC_NotAffect)
            {
                CommendText = $@"INSERT INTO ClubUser (ClubId, FUserID) VALUES (@ClubId, @FUserId) ";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);
            }

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(UserMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();


            #region 參數設定
            parameters.Add("@ClubId", vm.EditModel.ClubId);
            parameters.Add("@OldFUserId", vm.EditModel.OldFUserId);
            parameters.Add("@FUserId", vm.EditModel.FUserId);
            parameters.Add("@IsEnable", vm.EditModel.IsEnable);
            parameters.Add("@UserName", vm.EditModel.UserName);
            parameters.Add("@Department", vm.EditModel.Department);
            parameters.Add("@EMail", vm.EditModel.EMail);
            parameters.Add("@CellPhone", vm.EditModel.CellPhone);
            parameters.Add("@Memo", vm.EditModel.Memo);

            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定


            string CommendText = string.Empty;

            CommendText = $@"UPDATE FUserMain SET IsEnable = 0 WHERE FUserId = @OldFUserId";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (ExecuteResult.isSuccess)
            {
                CommendText = $@"UPDATE FUserMain 
                                SET UserName = @UserName,
                                    EMail = @EMail,
                                    CellPhone = @CellPhone,
                                    Department = @Department,
                                    IsEnable = @IsEnable,
                                    Memo = @Memo,
                                    LastModifier = @LastModifier,
                                    LastModified = GETDATE()
                              WHERE FUserId = @FUserId ";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);
            }
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
