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

    public class BorrowRoleMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BorrowRoleMangResultModel> GetSearchResult(BorrowRoleMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            #region 參數設定

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@RoleId", model.RoleId);
            parameters.Add("@RoleName", model.RoleName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
            
            #endregion

            CommandText = $@"SELECT A.RoleId, A.RoleName, A.Comment, A.SystemCode, B.Text AS SystemCodeText, 
                                    A.IsEnable, A.Creator, A.Created, A.LastModifier, A.LastModified, A.ModifiedReason
                               FROM SystemRole A
							   LEFT JOIN Code B ON B.Code = A.SystemCode AND B.Type = 'SystemCode'
                              WHERE 1 = 1
AND A.SystemCode = '04'
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
{(model.RoleId != null ? " AND A.RoleId LIKE '%' + @RoleId + '%'" : " ")}
AND (@RoleName IS NULL OR A.RoleName LIKE '%' + @RoleName + '%')

";

            (DbExecuteInfo info, IEnumerable<BorrowRoleMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowRoleMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowRoleMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BorrowRoleMangEditModel GetEditData(string RoleId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@RoleId", RoleId);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT RoleId, RoleName, Comment, SystemCode, 
	                     CASE WHEN SystemRoleCode = '02' THEN '01' ELSE '02' END AS SystemRoleCode, 
	                               IsEnable, Creator, Created, LastModifier, LastModified, ModifiedReason
                              FROM SystemRole
                             WHERE 1 = 1
                               AND RoleId = @RoleId";


            (DbExecuteInfo info, IEnumerable<BorrowRoleMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowRoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(BorrowRoleMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@RoleId", vm.CreateModel.RoleId.TrimStartAndEnd());
            parameters.Add("@RoleName", vm.CreateModel.RoleName.TrimStartAndEnd());
            parameters.Add("@SystemRoleCode", vm.CreateModel.SystemRoleCode.TrimStartAndEnd() == "01" ? "02" : "01");
            parameters.Add("@Comment", vm.CreateModel.Comment.TrimStartAndEnd());

            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO SystemRole
                                               (RoleId
                                               ,RoleName
                                               ,SystemRoleCode
                                               ,Comment
                                               ,SystemCode
                                               ,IsEnable
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@RoleId
                                               ,@RoleName
                                               ,@SystemRoleCode
                                               ,@Comment
                                               ,'04'
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
        public DbExecuteInfo InsertFunData(BorrowRoleMangViewModel vm)
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
                    CommendText = $@"DELETE FROM SystemRoleFun WHERE RoleId = @RoleId AND SystemCode = '04'";

                    ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                    if (!ExecuteResult.isSuccess)
                    {
                        if (ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
                            return ExecuteResult;
                    }

                    for (int i = 0; i <= arrFun.Count() - 1; i++)
                    {
                        parameters.Add("@MenuNode", arrFun[i].ToString());

                        CommendText = $@"INSERT INTO SystemRoleFun (RoleId, MenuNode, SystemCode) VALUES (@RoleId, @MenuNode, '04')";

                        ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                        if (!ExecuteResult.isSuccess)
                            return ExecuteResult;
                    }
                }
            }

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(BorrowRoleMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定

            parameters.Add("@RoleId", vm.EditModel.RoleId);
            parameters.Add("@RoleName", vm.EditModel.RoleName.TrimStartAndEnd());
            parameters.Add("@SystemRoleCode", vm.EditModel.SystemRoleCode.TrimStartAndEnd() == "01" ? "02" : "01");
            parameters.Add("@Comment", vm.EditModel.Comment.TrimStartAndEnd());

            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            CommendText = $@"UPDATE SystemRole 
                                SET RoleName = @RoleName, 
                                    SystemRoleCode = @SystemRoleCode, 
                                    Comment = @Comment, 
                                    SystemCode = '04', 
                                    LastModifier = @LastModifier, 
                                    LastModified = GETDATE()
                                WHERE RoleId = @RoleId ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改功能資料 </summary>
        public DbExecuteInfo UpdateFunData(BorrowRoleMangViewModel vm, string[] arrFun)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定

            if(vm.EditModel != null)
                parameters.Add("@RoleId", vm.EditModel.RoleId);
            else if(vm.CreateModel != null)
                parameters.Add("@RoleId", vm.CreateModel.RoleId);

            #endregion 參數設定

            CommendText = $@"DELETE FROM SystemRoleFun WHERE RoleId = @RoleId AND SystemCode = '04'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (!ExecuteResult.isSuccess)
            {
                if (ExecuteResult.ErrorCode != dbErrorCode._EC_NotAffect)
                    return ExecuteResult;
            }

            for (int i = 0; i <= arrFun.Count() - 1; i++)
            {
                parameters.Add("@MenuNode", arrFun[i].ToString());

                CommendText = $@"INSERT INTO SystemRoleFun (RoleId, MenuNode, SystemCode) VALUES (@RoleId, @MenuNode, '04')";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

                if (!ExecuteResult.isSuccess)
                    return ExecuteResult;
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

            string CommendText = $@"DELETE FROM SystemRole WHERE RoleId = @RoleId AND SystemCode = '04'";

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

            string CommendText = $@"DELETE FROM UserRole WHERE RoleId = @RoleId AND SystemCode = '04' ";

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

            CommandText = @"SELECT A.MenuNode AS VALIE, A.MenuName AS TEXT
                              FROM SystemMenu A
                         LEFT JOIN Code B ON B.Code = A.SystemCode AND B.Type = 'SystemCode'
                         LEFT JOIN SystemFun C ON C.FunId = A.FunId
                             WHERE C.url <> ''
                               AND A.SystemCode = '04'
                               AND A.MenuName <> '初始頁'
                          ORDER BY A.MenuNode";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<FunSelectedItem> GetAllFunInfo2()
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
                             WHERE C.Url <> ''
                               AND A.SystemCode = '04'
                               AND A.MenuName <> '初始頁'
";

            (DbExecuteInfo info, IEnumerable<FunSelectedItem> entitys) dbResult = DbaExecuteQuery<FunSelectedItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FunSelectedItem>();
        }

        public DataTable ChkHasOtherUseRole(string ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@RoleId", ser);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT A.RoleId, A.LoginId, B.RoleName
                              FROM UserRole A
                         LEFT JOIN SystemRole B ON B.RoleId = A.RoleId
                             WHERE 1 = 1
                               AND A.RoleId = @RoleId";


            (DbExecuteInfo info, IEnumerable<BorrowRoleMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowRoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }

        public DataTable GetUpMenuNode(string[] arr)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            string strMenuNode = string.Empty;

            for (int i = 0; i <= arr.Length - 1; i++)
            {
                if (i != arr.Length - 1)
                {
                    strMenuNode = strMenuNode + string.Format("'{0}',", arr[i]);
                }
                else
                {
                    strMenuNode = strMenuNode + string.Format("'{0}'", arr[i]);
                }
            }
            
            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT DISTINCT MenuUpNode
                              FROM SystemMenu
                             WHERE 1 = 1
                               AND MenuNode IN ({strMenuNode})";


            (DbExecuteInfo info, IEnumerable<BorrowRoleMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowRoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }

        public List<SelectListItem> GetAllSystemCode()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS Text FROM Code WHERE Type = 'SystemCode'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public string[] GetDefaultPage(string[] arr)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            string strMenuNode = string.Empty;

            for (int i = 0; i <= arr.Length - 1; i++)
            {
                if (i != arr.Length - 1)
                {
                    strMenuNode = strMenuNode + string.Format("'{0}',", arr[i]);
                }
                else
                {
                    strMenuNode = strMenuNode + string.Format("'{0}'", arr[i]);
                }
            }

            #region 參數設定
            #endregion

            CommandText = $@"SELECT MenuNode 
                               FROM SystemMenu
                              WHERE 1 = 1
                                AND MenuName = '初始頁'
                                AND SystemCode IN (SELECT DISTINCT SystemCode
                                                     FROM SystemMenu
                                                    WHERE 1 = 1
                                                      AND MenuNode IN ({strMenuNode}))";

            (DbExecuteInfo info, IEnumerable<BorrowRoleMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowRoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.DataSetToStringArray("MenuNode");
        }

        public List<SelectListItem> GetYesOrNo()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'YesOrNo'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
