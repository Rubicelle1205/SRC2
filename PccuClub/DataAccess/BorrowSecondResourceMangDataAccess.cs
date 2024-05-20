using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class BorrowSecondResourceMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BorrowSecondResourceMangResultModel> GetSearchResult(BorrowSecondResourceMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainClass", model?.MainClass);
            parameters.Add("@ShelvesStatus", model?.ShelvesStatus);
            parameters.Add("@BorrowStatus", model?.BorrowStatus);
            parameters.Add("@MainResourceName", model?.MainResourceName);
            parameters.Add("@SecondResourceNo", model?.SecondResourceNo);
            parameters.Add("@SecondResourceName", model?.SecondResourceName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.MainResourceID, B.MainResourceName AS MainResourceIDText, C.ID AS MainClass, C.Text AS MainClassText, 
       A.SecondResourceNo, A.SecondResourceName, A.ShelvesStatus, D.Text AS ShelvesStatusText, A.BorrowStatus, F.Text AS BorrowStatusText, A.SecondResourceSerNo, 
       A.DeviceNo, A.Brand, A.Specification, A.Location, A.Memo, A.Creator, 
       A.Created, A.LastModifier, A.LastModified
FROM BorrowSecondResourceMang A
LEFT JOIN BorrowMainResourceMang B ON B.MainResourceID = A.MainResourceID
LEFT JOIN BorrowMainClassMang C ON C.ID = B.MainClass
LEFT JOIN Code D ON D.Code = A.ShelvesStatus AND D.Type = 'ShelvesStatus'
LEFT JOIN Code F ON F.Code = A.BorrowStatus AND F.Type = 'BorrowStatus'
WHERE 1 = 1
AND (@MainClass IS NULL OR C.ID = @MainClass) 
AND (@ShelvesStatus IS NULL OR A.ShelvesStatus = @ShelvesStatus) 
AND (@BorrowStatus IS NULL OR A.BorrowStatus = @BorrowStatus) 
AND (@MainResourceName IS NULL OR B.MainResourceName LIKE '%' + @MainResourceName + '%') 
AND (@SecondResourceNo IS NULL OR A.SecondResourceNo LIKE '%' + @SecondResourceNo + '%') 
AND (@SecondResourceName IS NULL OR A.SecondResourceName LIKE '%' + @SecondResourceName + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<BorrowSecondResourceMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowSecondResourceMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowSecondResourceMangResultModel>();
        }

        /// <summary> 查詢結果 </summary>

        public List<BorrowSecondResourceMangResultModel> GetExportResult(BorrowSecondResourceMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainClass", model?.MainClass);
            parameters.Add("@ShelvesStatus", model?.ShelvesStatus);
            parameters.Add("@BorrowStatus", model?.BorrowStatus);
            parameters.Add("@MainResourceName", model?.MainResourceName);
            parameters.Add("@SecondResourceNo", model?.SecondResourceNo);
            parameters.Add("@SecondResourceName", model?.SecondResourceName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.MainResourceID, B.MainResourceName AS MainResourceIDText, C.ID AS MainClass, C.Text AS MainClassText, 
       A.SecondResourceNo, A.SecondResourceName, A.ShelvesStatus, D.Text AS ShelvesStatusText, A.BorrowStatus, F.Text AS BorrowStatusText, A.SecondResourceSerNo, 
       A.DeviceNo, A.Brand, A.Specification, A.Location, A.Memo, A.Creator, 
       A.Created, A.LastModifier, A.LastModified
FROM BorrowSecondResourceMang A
LEFT JOIN BorrowMainResourceMang B ON B.MainResourceID = A.MainResourceID
LEFT JOIN BorrowMainClassMang C ON C.ID = B.MainClass
LEFT JOIN Code D ON D.Code = A.ShelvesStatus AND D.Type = 'ShelvesStatus'
LEFT JOIN Code F ON F.Code = A.BorrowStatus AND F.Type = 'BorrowStatus'
WHERE 1 = 1
AND (@MainClass IS NULL OR C.ID = @MainClass) 
AND (@ShelvesStatus IS NULL OR A.ShelvesStatus = @ShelvesStatus) 
AND (@BorrowStatus IS NULL OR A.BorrowStatus = @BorrowStatus) 
AND (@MainResourceName IS NULL OR B.MainResourceName LIKE '%' + @MainResourceName + '%') 
AND (@SecondResourceNo IS NULL OR A.SecondResourceNo LIKE '%' + @SecondResourceNo + '%') 
AND (@SecondResourceName IS NULL OR A.SecondResourceName LIKE '%' + @SecondResourceName + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<BorrowSecondResourceMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowSecondResourceMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowSecondResourceMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BorrowSecondResourceMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ID, MainResourceID, SecondResourceNo, SecondResourceName, ShelvesStatus, BorrowStatus, SecondResourceSerNo, 
       DeviceNo, Brand, Specification, Location, Memo, Creator, 
       Created, LastModifier, LastModified
FROM BorrowSecondResourceMang
WHERE 1 = 1
AND (ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<BorrowSecondResourceMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowSecondResourceMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo ImportData(List<BorrowSecondResourceMangImportModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion 參數設定

            string CommendText = $@"INSERT INTO BorrowSecondResourceMang
                                               (MainResourceID
                                               ,SecondResourceNo
                                               ,SecondResourceName
                                               ,ShelvesStatus
                                               ,BorrowStatus
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@MainResourceID
                                               ,@SecondResourceNo
                                               ,@SecondResourceName
                                               ,@ShelvesStatus
                                               ,@BorrowStatus
                                               ,@Memo
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(BorrowSecondResourceMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", vm.EditModel.ID);
            parameters.Add("@MainClass", vm.EditModel.MainClass);

            parameters.Add("@MainResourceID", vm.EditModel.MainResourceID);
            parameters.Add("@SecondResourceNo", vm.EditModel.SecondResourceNo);
            parameters.Add("@SecondResourceName", vm.EditModel.SecondResourceName);
            parameters.Add("@ShelvesStatus", vm.EditModel.ShelvesStatus);
            parameters.Add("@BorrowStatus", vm.EditModel.BorrowStatus);
            parameters.Add("@SecondResourceSerNo", vm.EditModel.SecondResourceSerNo);
            parameters.Add("@DeviceNo", vm.EditModel.DeviceNo);
            parameters.Add("@Brand", vm.EditModel.Brand);
            parameters.Add("@Specification", vm.EditModel.Specification);
            parameters.Add("@Location", vm.EditModel.Location);

            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE BorrowSecondResourceMang 
                                       SET MainResourceID = @MainResourceID, 
                                           SecondResourceNo = @SecondResourceNo, 
                                           SecondResourceName = @SecondResourceName, 
                                           ShelvesStatus = @ShelvesStatus, 
                                           BorrowStatus = @BorrowStatus, 
                                           SecondResourceSerNo = @SecondResourceSerNo, 
                                           DeviceNo = @DeviceNo, 
                                           Brand = @Brand, 
                                           Specification = @Specification, 
                                           Location = @Location, 
                                           Memo = @Memo, 
                                           LastModifier = @LoginId, 
                                           LastModified = GETDATE()
                                     WHERE ID = @ID";

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
            parameters.Add("@ID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM BorrowSecondResourceMang WHERE ID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetddlMainClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM BorrowMainClassMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlMainResource()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT MainResourceID AS Value, MainResourceName AS Text FROM BorrowMainResourceMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlShelvesStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'ShelvesStatus' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlBorrowStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'BorrowStatus' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
