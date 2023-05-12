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

        /// <summary> 查詢結果 </summary>

        public List<ClubMangResultModel> GetSearchResult(ClubMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubId", model.ClubId);
            parameters.Add("@ClubName", model.ClubName);
            parameters.Add("@ClubLeader", model.ClubLeader);//
            parameters.Add("@Department", model.Department);//
            parameters.Add("@MailOrTel", model.MailOrTel);
            parameters.Add("@ClubClass", model.ClubClass);
            parameters.Add("@LifeClass", model.LifeClass);
            parameters.Add("@SchoolYear", model.SchoolYear);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

         
            #endregion

            CommandText = $@"SELECT A.ClubId, A.ClubCName, A.ClubEName, A.SchoolYear, A.LifeClass, C.Text AS LifeClassText, A.ClubClass, B.Text AS ClubClassText, A.EMail, A.Tel, A.Created
                               FROM ClubMang A
							   LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass'
							   LEFT JOIN Code C ON C.Code = A.LifeClass AND C.Type = 'LifeClass'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
{(model.ClubId != null ? " AND A.ClubId LIKE '%' + @ClubId + '%'" : " ")}
{(model.ClubName != null ? " AND A.ClubName LIKE '%' + @ClubName + '%'" : " ")}
{(model.MailOrTel != null ? " AND (A.EMail LIKE '%' + @MailOrTel + '%' OR A.Tel LIKE  '%' + @MailOrTel + '%') " : " ")}
AND (@ClubClass IS NULL OR A.ClubClass = @ClubClass)
AND (@LifeClass IS NULL OR A.LifeClass = @LifeClass)
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
";

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
                            SELECT A.ClubId, A.ClubCName, A.ClubEName, A.SchoolYear, A.LifeClass, A.ClubClass, A.Address, A.EMail, A.Tel, 
                                   A.Social1, A.Social2, A.Social3, A.LogoPath, A.ActImgPath, A.ShortInfo, A.Memo, A.Created, A.LastModified
                               FROM ClubMang A
							   LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass'
							   LEFT JOIN Code C ON C.Code = A.LifeClass AND C.Type = 'LifeClass'
                              WHERE 1 = 1
                               AND A.ClubId = @ClubId";


            (DbExecuteInfo info, IEnumerable<ClubMangEditModel> entitys) dbResult = DbaExecuteQuery<ClubMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ClubMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubId", vm.CreateModel.ClubId);
            parameters.Add("@ClubCName", vm.CreateModel.ClubCName);
            parameters.Add("@ClubEName", vm.CreateModel.ClubEName);
            parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@LifeClass", vm.CreateModel.LifeClass);
            parameters.Add("@ClubClass", vm.CreateModel.ClubClass);
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
                                               ,ClubCName
                                               ,ClubEName
                                               ,SchoolYear
                                               ,LifeClass
                                               ,ClubClass
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
                                               ,@ClubCName
                                               ,@ClubEName
                                               ,@SchoolYear
                                               ,@LifeClass
                                               ,@ClubClass
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

            CommendText = CommendText.Replace("%LogoPath%", "NULL");
            CommendText = CommendText.Replace("%ActImgPath%", "NULL");

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ClubMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定

            parameters.Add("@ClubId", vm.EditModel.ClubId);
            parameters.Add("@ClubCName", vm.EditModel.ClubCName);
            parameters.Add("@ClubEName", vm.EditModel.ClubEName);
            parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@LifeClass", vm.EditModel.LifeClass);
            parameters.Add("@ClubClass", vm.EditModel.ClubClass);
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
                                           SET ClubCName = @ClubCName, 
                                                ClubEName = @ClubEName, 
                                                SchoolYear = @SchoolYear, 
                                                LifeClass = @LifeClass, 
                                                ClubClass = @ClubClass, 
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

            if (!string.IsNullOrEmpty(vm.EditModel.LogoPath))
                CommendText = CommendText.Replace("%LogoPath%", "LogoPath = @LogoPath,");

            if (!string.IsNullOrEmpty(vm.EditModel.ActImgPath))
                CommendText = CommendText.Replace("%ActImgPath%", "ActImgPath = @ActImgPath,");

            CommendText = CommendText.Replace("%LogoPath%", "");
            CommendText = CommendText.Replace("%ActImgPath%", "");

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
            parameters.Add("@RoleId", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM SystemRole WHERE RoleId = @RoleId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 刪除角色資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeletetUserRole(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@RoleId", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM UserRole WHERE RoleId = @RoleId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 刪除功能資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeleteFunData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@RoleId", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM SystemRoleFun WHERE RoleId = @RoleId ";

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
                             WHERE B.MenuUpNode <> '-1'
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

            CommandText = @"SELECT MenuNode AS VALUE, MenuName AS TEXT FROM SystemMenu WHERE MenuUpNode <> '-1' ";

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

        public List<SelectListItem> GetAllClubClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'ClubClass'";

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
    }
}
