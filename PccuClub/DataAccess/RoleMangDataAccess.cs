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

    public class RoleMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<RoleMangResultModel> GetSearchResult(RoleMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@RoleId", model.RoleId);
            parameters.Add("@RoleName", model.RoleName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT RoleId, RoleName, Comment, IsEnable, Creator, Created, LastModifier, LastModified, ModifiedReason
                               FROM SystemRole A
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
{(model.RoleId != null ? " AND A.RoleId LIKE '%' + @RoleId + '%'" : " ")}
AND (@RoleName IS NULL OR A.RoleName LIKE '%' + @RoleName + '%')

";

            (DbExecuteInfo info, IEnumerable<RoleMangResultModel> entitys) dbResult = DbaExecuteQuery<RoleMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<RoleMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public RoleMangEditModel GetEditData(string RoleId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@RoleId", RoleId);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT RoleId, RoleName, Comment, IsEnable, Creator, Created, LastModifier, LastModified, ModifiedReason
                              FROM SystemRole
                             WHERE 1 = 1
                               AND RoleId = @RoleId";


            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(RoleMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@RoleId", vm.CreateModel.RoleId.TrimStartAndEnd());
            parameters.Add("@RoleName", vm.CreateModel.RoleName.TrimStartAndEnd());
            parameters.Add("@Comment", vm.CreateModel.Comment.TrimStartAndEnd());

            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO SystemRole
                                               (RoleId
                                               ,RoleName
                                               ,Comment
                                               ,IsEnable
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@RoleId
                                               ,@RoleName
                                               ,@Comment
                                               ,1
                                               ,@LastModifier
                                               ,GETDATE()
                                               ,@LastModifier
                                               ,GETDATE()
                                               ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 新增功能資料 </summary>
        public DbExecuteInfo InsertFunData(RoleMangViewModel vm)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定

            parameters.Add("@RoleId", vm.CreateModel.RoleId);

            #endregion 參數設定

            if (!string.IsNullOrEmpty(vm.CreateModel.strFunInfo))
            {
                string[] arrFun = vm.CreateModel.strFunInfo.Split(",");

                if (arrFun.Count() > 0)
                {
                    CommendText = $@"DELETE FROM SystemRoleFun WHERE RoleId = @RoleId ";

                    ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                    if (!ExecuteResult.isSuccess)
                    {
                        if (ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
                            return ExecuteResult;
                    }

                    for (int i = 0; i <= arrFun.Count() - 1; i++)
                    {
                        parameters.Add("@MenuNode", arrFun[i].ToString());

                        CommendText = $@"INSERT INTO SystemRoleFun
                                               (RoleId
                                               ,MenuNode)
                                         VALUES
                                               (@RoleId
                                               ,@MenuNode)";

                        ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                        if (!ExecuteResult.isSuccess)
                            return ExecuteResult;
                    }
                }
            }

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(RoleMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定

            parameters.Add("@RoleId", vm.EditModel.RoleId);
            parameters.Add("@RoleName", vm.EditModel.RoleName.TrimStartAndEnd());
            parameters.Add("@Comment", vm.EditModel.Comment.TrimStartAndEnd());

            parameters.Add("@LastModifier", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE SystemRole 
                                           SET RoleName = @RoleName, 
                                               Comment = @Comment, 
                                               LastModifier = @LastModifier, 
                                               LastModified = GETDATE()
                                         WHERE RoleId = @RoleId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改功能資料 </summary>
        public DbExecuteInfo UpdateFunData(RoleMangViewModel vm)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定

            parameters.Add("@RoleId", vm.EditModel.RoleId);

            #endregion 參數設定

            if (!string.IsNullOrEmpty(vm.EditModel.strFunInfo))
            {
                string[] arrFun = vm.EditModel.strFunInfo.Split(",");

                if (arrFun.Count() > 0)
                {
                    CommendText = $@"DELETE FROM SystemRoleFun WHERE RoleId = @RoleId ";

                    ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                    if (!ExecuteResult.isSuccess)
                    { 
                        if(ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
                            return ExecuteResult;
                    }
                        

                    for (int i = 0; i <= arrFun.Count() - 1; i++)
                    {
                        parameters.Add("@MenuNode", arrFun[i].ToString());

                        CommendText = $@"INSERT INTO SystemRoleFun
                                               (RoleId
                                               ,MenuNode)
                                         VALUES
                                               (@RoleId
                                               ,@MenuNode)";

                        ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                        if (!ExecuteResult.isSuccess)
                            return ExecuteResult;
                    }
                }
            }

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
        public List<RoleMangResultModel> GetExportResult(RoleMangConditionModel model)
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
FROM RoleMang
WHERE 1 = 1
AND (@BuildName IS NULL OR BuildName LIKE '%' + @BuildName + '%') 
AND (@Note IS NULL OR Note LIKE '%' + @Note + '%') ";

            (DbExecuteInfo info, IEnumerable<RoleMangResultModel> entitys) dbResult = DbaExecuteQuery<RoleMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<RoleMangResultModel>();
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

    }
}
