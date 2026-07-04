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
using System.Runtime.ConstrainedExecution;

namespace WebPccuClub.DataAccess
{

    public class ClubMangDataAccess : BaseAccess
    {
        public List<ColumnDataModel> GetDefaultColumnData()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = $@"SELECT T.ColumnValue, T.ColumnName, T.IsDefault
                               FROM (VALUES
('ClubId', '社團代號', 1),
('ClubCName', '中文名稱', 1),
('ClubEName', '英文名稱', 1),
('SchoolYear', '學年度', 1),
('RoleName', '角色', 1),
('FrontShowText', '前台顯示', 0),
('LifeClassText', '社團組別', 1),
('ClubClassText', '社團分類', 1),
('ClubLeader', '負責人姓名', 1),
('Department', '負責人系級', 1),
('EMail', 'E-mail', 1),
('Tel', '聯絡電話', 1),
('Address', '社辦地址', 1),
('Social1', '社群連結一', 0),
('Social2', '社群連結二', 0),
('Social3', '社群連結三', 0),
('Created', '建立時間', 1),
('ShortInfo', '簡介', 0),
('Memo', '備註', 0)
                                    ) AS T(ColumnValue, ColumnName, IsDefault);
";


            (DbExecuteInfo info, IEnumerable<ColumnDataModel> entitys) dbResult = DbaExecuteQuery<ColumnDataModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ColumnDataModel>();
        }

        /// <summary> 查詢結果 </summary>

        public List<ClubMangResultModel> GetSearchResult(ClubMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubId", string.IsNullOrEmpty(model.ClubId) ? null : $"%{model.ClubId}%");
            parameters.Add("@ClubName", string.IsNullOrEmpty(model.ClubName) ? null : $"%{model.ClubName}%");
            parameters.Add("@ClubLeader", string.IsNullOrEmpty(model.ClubLeader) ? null : $"%{model.ClubLeader}%");
            parameters.Add("@Department", string.IsNullOrEmpty(model.Department) ? null : $"%{model.Department}%");
            parameters.Add("@MailOrTel", string.IsNullOrEmpty(model.MailOrTel) ? null : $"%{model.MailOrTel}%");

            parameters.Add("@ClubClass", model.ClubClass);
            parameters.Add("@LifeClass", model.LifeClass);
            parameters.Add("@SchoolYear", model.SchoolYear);

            parameters.Add("@FromDate", model.From_ReleaseDate?.Date);                    // 00:00:00.000
            parameters.Add("@ToDate", model.To_ReleaseDate?.Date.AddDays(1).AddTicks(-1)); // 23:59:59.999

            #endregion


            #region 2. 動態 SQL 語法拼裝

            CommandText = $@"
SELECT 
    A.ClubId, A.ClubCName, A.ClubEName, A.SchoolYear, E.ClubLeader, E.Department, G.RoleName, 
    A.FrontShow, H.[Text] AS FrontShowText, A.Address, A.Social1, A.Social2, A.Social3, A.ShortInfo, A.Memo, 
    A.LifeClass, C.[Text] AS LifeClassText, A.ClubClass, B.[Text] AS ClubClassText, A.EMail, A.Tel, A.Created
FROM ClubMang A

LEFT JOIN Code B ON B.Code = A.ClubClass AND B.[Type] = 'ClubClass'
LEFT JOIN Code C ON C.Code = A.LifeClass AND C.[Type] = 'LifeClass'
LEFT JOIN Code H ON H.Code = A.FrontShow AND H.[Type] = 'YesOrNo'

OUTER APPLY (
    SELECT TOP 1 FU.UserName AS ClubLeader, FU.Department
    FROM ClubUser CU
    INNER JOIN FUserMain FU ON FU.FUserId = CU.FUserId
    WHERE CU.ClubId = A.ClubId
) E

OUTER APPLY (
    SELECT TOP 1 SR.RoleName
    FROM UserRole UR
    INNER JOIN SystemRole SR ON SR.RoleId = UR.RoleId
    WHERE UR.LoginId = A.ClubId
) G

WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : "")}
{(!string.IsNullOrEmpty(model.ClubId) ? " AND A.ClubId LIKE @ClubId" : "")}
{(!string.IsNullOrEmpty(model.ClubName) ? " AND A.ClubCName LIKE @ClubName" : "")}
{(!string.IsNullOrEmpty(model.ClubLeader) ? " AND E.ClubLeader LIKE @ClubLeader" : "")}
{(!string.IsNullOrEmpty(model.Department) ? " AND E.Department LIKE @Department" : "")}
{(!string.IsNullOrEmpty(model.MailOrTel) ? " AND (A.EMail LIKE @MailOrTel OR A.Tel LIKE @MailOrTel)" : "")}
{(!string.IsNullOrEmpty(model.ClubClass) ? " AND A.ClubClass = @ClubClass" : "")}
{(!string.IsNullOrEmpty(model.LifeClass) ? " AND A.LifeClass = @LifeClass" : "")}
{(!string.IsNullOrEmpty(model.SchoolYear) ? " AND A.SchoolYear = @SchoolYear" : "")}
";

            #endregion

            (DbExecuteInfo info, IEnumerable<ClubMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClubMangEditModel GetEditData(string ClubId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ClubId", ClubId);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT A.ClubId, A.ClubCName, A.ClubEName, A.SchoolYear, A.LifeClass, A.ClubClass, A.FrontShow, A.Address, A.EMail, A.Tel, 
                                   A.Social1, A.Social2, A.Social3, A.LogoPath, A.ActImgPath, A.ShortInfo, A.Memo, A.Created, A.LastModified, D.RoleId
                               FROM ClubMang A
							   LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass'
							   LEFT JOIN Code C ON C.Code = A.LifeClass AND C.Type = 'LifeClass'
                               LEFT JOIN UserRole D ON D.LoginId = A.ClubId
                              WHERE 1 = 1
                               AND A.ClubId = @ClubId";


            (DbExecuteInfo info, IEnumerable<ClubMangEditModel> entitys) dbResult = DbaExecuteQuery<ClubMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(string EncryptPw, ClubMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubId", vm.CreateModel.ClubId);
            parameters.Add("@Password", EncryptPw);
            parameters.Add("@ClubCName", vm.CreateModel.ClubCName);
            parameters.Add("@ClubEName", vm.CreateModel.ClubEName);
            parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@LifeClass", vm.CreateModel.LifeClass);
            parameters.Add("@ClubClass", vm.CreateModel.ClubClass);
            parameters.Add("@FrontShow", vm.CreateModel.FrontShow);
            parameters.Add("@Address", vm.CreateModel.Address);
            parameters.Add("@Tel", vm.CreateModel.Tel);
            parameters.Add("@EMail", vm.CreateModel.EMail);
            parameters.Add("@Social1", vm.CreateModel.Social1);
            parameters.Add("@Social2", vm.CreateModel.Social2);
            parameters.Add("@Social3", vm.CreateModel.Social3);

            if (!string.IsNullOrEmpty(vm.CreateModel.LogoPath))
                parameters.Add("@LogoPath", vm.CreateModel.LogoPath);

            if (!string.IsNullOrEmpty(vm.CreateModel.ActImgPath))
                parameters.Add("@ActImgPath", vm.CreateModel.ActImgPath);

            parameters.Add("@ShortInfo", vm.CreateModel.ShortInfo);
            parameters.Add("@Memo", vm.CreateModel.Memo);

            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ClubMang
                                                (ClubId
                                               ,Password
                                               ,ClubCName
                                               ,ClubEName
                                               ,SchoolYear
                                               ,LifeClass
                                               ,ClubClass
                                               ,FrontShow
                                               ,Address
                                               ,EMail
                                               ,Tel
                                               ,Social1
                                               ,Social2
                                               ,Social3
                                               ,LogoPath
                                               ,ActImgPath
                                               ,ShortInfo
                                               ,Memo
                                               ,IsEnable
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ClubId
                                               ,@Password
                                               ,@ClubCName
                                               ,@ClubEName
                                               ,@SchoolYear
                                               ,@LifeClass
                                               ,@ClubClass
                                               ,@FrontShow
                                               ,@Address
                                               ,@EMail
                                               ,@Tel
                                               ,@Social1
                                               ,@Social2
                                               ,@Social3
                                                %LogoPath%
                                                %ActImgPath%
                                               ,@ShortInfo
                                               ,@Memo
                                               ,1
                                               ,@LastModifier
                                               ,GETDATE()
                                               ,@LastModifier
                                               ,GETDATE()
                                               ,NULL)";

                if (!string.IsNullOrEmpty(vm.CreateModel.LogoPath))
                CommendText = CommendText.Replace("%LogoPath%", ",@LogoPath");

            if (!string.IsNullOrEmpty(vm.CreateModel.ActImgPath))
                CommendText = CommendText.Replace("%ActImgPath%", ",@ActImgPath");

            CommendText = CommendText.Replace("%LogoPath%", ",NULL");
            CommendText = CommendText.Replace("%ActImgPath%", ",NULL");

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(string EncryptPw, ClubMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定

            parameters.Add("@ClubId", vm.EditModel.ClubId);

            if (!string.IsNullOrEmpty(EncryptPw))
                parameters.Add("@Password", EncryptPw);

            parameters.Add("@ClubCName", vm.EditModel.ClubCName);
            parameters.Add("@ClubEName", vm.EditModel.ClubEName);
            parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@LifeClass", vm.EditModel.LifeClass);
            parameters.Add("@ClubClass", vm.EditModel.ClubClass);
            parameters.Add("@FrontShow", vm.EditModel.FrontShow);
            parameters.Add("@Address", vm.EditModel.Address);
            parameters.Add("@Tel", vm.EditModel.Tel);
            parameters.Add("@EMail", vm.EditModel.EMail);
            parameters.Add("@Social1", vm.EditModel.Social1);
            parameters.Add("@Social2", vm.EditModel.Social2);
            parameters.Add("@Social3", vm.EditModel.Social3);
            
            if(!string.IsNullOrEmpty(vm.EditModel.LogoPath))
                parameters.Add("@LogoPath", vm.EditModel.LogoPath);

            if (!string.IsNullOrEmpty(vm.EditModel.ActImgPath))
                parameters.Add("@ActImgPath", vm.EditModel.ActImgPath);

            parameters.Add("@ShortInfo", vm.EditModel.ShortInfo);
            parameters.Add("@Memo", vm.EditModel.Memo);

            parameters.Add("@LastModifier", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE ClubMang 
                                           SET  %Password%
                                                ClubCName = @ClubCName, 
                                                ClubEName = @ClubEName, 
                                                SchoolYear = @SchoolYear, 
                                                LifeClass = @LifeClass, 
                                                ClubClass = @ClubClass, 
                                                FrontShow = @FrontShow, 
                                                Address = @Address, 
                                                Tel = @Tel, 
                                                EMail = @EMail, 
                                                Social1 = @Social1, 
                                                Social2 = @Social2, 
                                                Social3 = @Social3, 
                                                %LogoPath%
                                                %ActImgPath%
                                                ShortInfo = @ShortInfo,
                                                Memo = @Memo, 
                                                LastModifier = @LastModifier, 
                                                LastModified = GETDATE()
                                         WHERE ClubId = @ClubId ";

            if (!string.IsNullOrEmpty(EncryptPw))
            {
                CommendText = CommendText.Replace("%Password%", "Password = @Password, ");
            }

            if (!string.IsNullOrEmpty(vm.EditModel.LogoPath))
                CommendText = CommendText.Replace("%LogoPath%", "LogoPath = @LogoPath,");

            if (!string.IsNullOrEmpty(vm.EditModel.ActImgPath))
                CommendText = CommendText.Replace("%ActImgPath%", "ActImgPath = @ActImgPath,");

            if (vm.EditModel.IsDeleteLogo)
                CommendText = CommendText.Replace("%LogoPath%", "LogoPath = '',");

            if (vm.EditModel.isDeleteActImg)
                CommendText = CommendText.Replace("%ActImgPath%", "ActImgPath = '',");

            CommendText = CommendText.Replace("%Password%", "");
            CommendText = CommendText.Replace("%LogoPath%", "");
            CommendText = CommendText.Replace("%ActImgPath%", "");

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 更新角色 </summary>
        public DbExecuteInfo UpdateRole(ClubMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            if (vm.EditModel != null)
            {
                parameters.Add("@ClubId", vm.EditModel.ClubId);
                parameters.Add("@RoleId", vm.EditModel.RoleId);
            }
            else if (vm.CreateModel != null)
            {
                parameters.Add("@ClubId", vm.CreateModel.ClubId);
                parameters.Add("@RoleId", vm.CreateModel.RoleId);
            }
            
            #endregion 參數設定

            string CommendText = $@"IF EXISTS (SELECT 1
                                         FROM UserRole 
                                        WHERE LoginId = @ClubId)
                                    
                                BEGIN
                                        UPDATE UserRole
                                        SET LoginId = @ClubId
                                           ,RoleId = @RoleId
                                        WHERE LoginId = @ClubId;
                                    END
                                ELSE
                                    BEGIN
                                        INSERT INTO UserRole
                                                (LoginId
                                               ,RoleId)
                                         VALUES
                                               (@ClubId
                                               ,@RoleId);
                                    END";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeletetData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubId", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM ClubMang WHERE ClubId = @ClubId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// Excel 取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ClubMangResultModel> GetExportResult(ClubMangConditionModel model)
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
FROM ClubMang
WHERE 1 = 1
AND (@BuildName IS NULL OR BuildName LIKE '%' + @BuildName + '%') 
AND (@Note IS NULL OR Note LIKE '%' + @Note + '%') ";

            (DbExecuteInfo info, IEnumerable<ClubMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubMangResultModel>();
        }

        public List<SelectListItem> GetUserFunInfo(string RoldId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.MenuNode AS VALUE, B.MenuName AS TEXT 
                              FROM SystemRoleFun A
                         LEFT JOIN SystemMenu B ON B.MenuNode = A.MenuNode
                         LEFT JOIN SystemFun C ON C.FunId = B.FunId
                             WHERE C.url <> ''
                               AND B.MenuName <> '初始頁'
                               AND A.RoleId =  '{RoldId}' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllFunInfo()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT A.MenuNode AS VALUE, A.MenuName AS TEXT, A.BackOrFront AS [GROUP], A.SystemCode, B.Text AS SystemCodeText, C.Url
                              FROM SystemMenu A
                         LEFT JOIN Code B ON B.Code = A.SystemCode AND B.Type = 'SystemCode'
						 LEFT JOIN SystemFun C ON C.FunId = A.FunId
                             WHERE C.url <> ''
                               AND A.MenuName <> '初始頁'
";

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

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'LifeClass' ORDER BY Code";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllClubClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'ClubClass' ORDER BY Code";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllRoleClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT RoleId AS VALUE, RoleName AS TEXT FROM SystemRole WHERE RoleId <> 'hyperuser' AND SystemCode = '02' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            for (int i = 108; i <= 130; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(),  Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
        }

        public List<SelectListItem> GetFrontShow()
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

        public List<ClubMangResultModel> ChkClubExist(ClubMangViewModel vm)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubId", vm.CreateModel.ClubId);

            #endregion

            CommandText = $@"SELECT A.ClubId, A.ClubCName
                               FROM ClubMang A
                              WHERE 1 = 1
                                AND A.ClubId = @ClubId
";

            (DbExecuteInfo info, IEnumerable<ClubMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubMangResultModel>();
        }
    }
}
