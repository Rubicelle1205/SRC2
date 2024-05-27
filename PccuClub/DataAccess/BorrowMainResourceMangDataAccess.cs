using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Runtime.ConstrainedExecution;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using static WebPccuClub.Models.FBorrowRecordViewModel;

namespace WebPccuClub.DataAccess
{
    
    public class BorrowMainResourceMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BorrowMainResourceMangResultModel> GetSearchResult(BorrowMainResourceMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainClass", model?.MainClass);
            parameters.Add("@Enable", model?.Enable);
            parameters.Add("@MainResourceID", model?.MainResourceID);
            parameters.Add("@MainResourceName", model?.MainResourceName);
            parameters.Add("@Memo", model?.Memo);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.MainResourceID, A.MainResourceName, A.MainClass, B.Text AS MainClassText, A.SecondClass, A.BorrowType, C.Text AS BorrowTypeText, A.ShortDesc, A.BorrowRule, 
       A.ResourceImg1, A.ResourceImg2, A.ResourceImg3, A.ResourceImg4, 
       A.AmtReal, A.AmtShelves, A.AmtOnce, A.AmtSafe, A.SafeMessage, A.Enable, D.Text AS EnableText, A.Memo, A.InventoryStatus, 
       A.Creator, A.Created, A.LastModifier, A.LastModified
FROM BorrowMainResourceMang A
LEFT JOIN BorrowMainClassMang B ON B.ID = A.MainClass
LEFT JOIN Code C ON C.Code = A.BorrowType AND C.Type = 'BorrowMultType'
LEFT JOIN Code D ON D.Code = A.Enable AND D.Type = 'Enable'
WHERE 1 = 1
AND (@MainClass IS NULL OR A.MainClass = @MainClass) 
AND (@Enable IS NULL OR A.Enable = @Enable) 
AND (@MainResourceID IS NULL OR A.MainResourceID LIKE '%' + @MainResourceID + '%') 
AND (@MainResourceName IS NULL OR A.MainResourceName LIKE '%' + @MainResourceName + '%') 
AND (@Memo IS NULL OR A.Memo LIKE '%' + @Memo + '%') ";


            (DbExecuteInfo info, IEnumerable<BorrowMainResourceMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowMainResourceMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowMainResourceMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BorrowMainResourceMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainResourceID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT MainResourceID, MainResourceName, MainClass, SecondClass, BorrowType, ShortDesc, BorrowRule, 
       ResourceImg1, ResourceImg2, ResourceImg3, ResourceImg4, 
       AmtReal, AmtShelves, AmtOnce, AmtSafe, SafeMessage, Enable, Memo, InventoryStatus, 
       Creator, Created, LastModifier, LastModified
FROM BorrowMainResourceMang
WHERE 1 = 1
AND (MainResourceID = @MainResourceID) ";


            (DbExecuteInfo info, IEnumerable<BorrowMainResourceMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowMainResourceMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(BorrowMainResourceMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", vm.CreateModel.MainResourceID);
            parameters.Add("@MainResourceName", vm.CreateModel.MainResourceName);
            parameters.Add("@MainClass", vm.CreateModel.MainClass);
            parameters.Add("@SecondClass", vm.CreateModel.SecondClass);
            parameters.Add("@BorrowType", vm.CreateModel.BorrowType);
            parameters.Add("@ShortDesc", vm.CreateModel.ShortDesc);
            parameters.Add("@BorrowRule", vm.CreateModel.BorrowRule);
            parameters.Add("@ResourceImg1", vm.CreateModel.ResourceImg1);
            parameters.Add("@ResourceImg2", vm.CreateModel.ResourceImg2);
            parameters.Add("@ResourceImg3", vm.CreateModel.ResourceImg3);
            parameters.Add("@ResourceImg4", vm.CreateModel.ResourceImg4);
            parameters.Add("@AmtReal", vm.CreateModel.AmtReal);
            parameters.Add("@AmtShelves", vm.CreateModel.AmtShelves);
            parameters.Add("@AmtOnce", vm.CreateModel.AmtOnce);
            parameters.Add("@AmtSafe", vm.CreateModel.AmtSafe);
            parameters.Add("@SafeMessage", vm.CreateModel.SafeMessage);
            parameters.Add("@Enable", vm.CreateModel.Enable);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO BorrowMainResourceMang
                                               (MainResourceID
                                               ,MainResourceName
                                               ,MainClass
                                               ,SecondClass
                                               ,BorrowType
                                               ,ShortDesc
                                               ,BorrowRule
                                               ,ResourceImg1
                                               ,ResourceImg2
                                               ,ResourceImg3
                                               ,ResourceImg4
                                               ,AmtReal
                                               ,AmtShelves
                                               ,AmtOnce
                                               ,AmtSafe
                                               ,SafeMessage
                                               ,Enable
                                               ,Memo
                                               ,InventoryStatus
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               (@MainResourceID
                                               ,@MainResourceName
                                               ,@MainClass
                                               ,@SecondClass
                                               ,@BorrowType
                                               ,@ShortDesc
                                               ,@BorrowRule
                                               ,@ResourceImg1
                                               ,@ResourceImg2
                                               ,@ResourceImg3
                                               ,@ResourceImg4
                                               ,@AmtReal
                                               ,@AmtShelves
                                               ,@AmtOnce
                                               ,@AmtSafe
                                               ,@SafeMessage
                                               ,@Enable
                                               ,@Memo
                                               ,'01'
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(BorrowMainResourceMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", vm.EditModel.MainResourceID);
            parameters.Add("@MainResourceName", vm.EditModel.MainResourceName);
            parameters.Add("@MainClass", vm.EditModel.MainClass);
            parameters.Add("@SecondClass", vm.EditModel.SecondClass);
            parameters.Add("@BorrowType", vm.EditModel.BorrowType);
            parameters.Add("@ShortDesc", vm.EditModel.ShortDesc);
            parameters.Add("@BorrowRule", vm.EditModel.BorrowRule);
            parameters.Add("@ResourceImg1", vm.EditModel.ResourceImg1);
            parameters.Add("@ResourceImg2", vm.EditModel.ResourceImg2);
            parameters.Add("@ResourceImg3", vm.EditModel.ResourceImg3);
            parameters.Add("@ResourceImg4", vm.EditModel.ResourceImg4);
            parameters.Add("@AmtReal", vm.EditModel.AmtReal);
            parameters.Add("@AmtShelves", vm.EditModel.AmtShelves);
            parameters.Add("@AmtOnce", vm.EditModel.AmtOnce);
            parameters.Add("@AmtSafe", vm.EditModel.AmtSafe);
            parameters.Add("@SafeMessage", vm.EditModel.SafeMessage);
            parameters.Add("@Enable", vm.EditModel.Enable);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE BorrowMainResourceMang 
                                       SET MainResourceName = @MainResourceName, MainClass = @MainClass, SecondClass = @SecondClass, 
                                           BorrowType = @BorrowType, ShortDesc = @ShortDesc, BorrowRule = @BorrowRule, 
                                           ResourceImg1 = @ResourceImg1, ResourceImg2 = @ResourceImg2, ResourceImg3 = @ResourceImg3, ResourceImg4 = @ResourceImg4, 
                                           AmtReal = @AmtReal, AmtShelves = @AmtShelves, AmtOnce = @AmtOnce, AmtSafe = @AmtSafe, SafeMessage = @SafeMessage, 
                                           Enable = @Enable, Memo = @Memo, LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE MainResourceID = @MainResourceID";

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
            parameters.Add("@MainResourceID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM BorrowMainResourceMang WHERE MainResourceID = @MainResourceID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }



        public BorrowMainResourceMangInventoryRecordModel GetInventoryRecord(string MainResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainResourceID", MainResourceID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.MainResourceID, A.MainResourceName, A.MainClass, C.Text AS MainClassText, A.BorrowType, B.Text AS BorrowTypeText, A.AmtReal, A.InventoryStatus
                               FROM BorrowMainResourceMang A
                           LEFT JOIN Code B ON B.Code = A.BorrowType AND B.Type = 'BorrowMultType'
                           LEFT JOIN BorrowMainClassMang C ON C.ID = A.MainClass
                               WHERE 1 = 1
                                 AND (A.MainResourceID = @MainResourceID) ";


            (DbExecuteInfo info, IEnumerable<BorrowMainResourceMangInventoryRecordModel> entitys) dbResult = DbaExecuteQuery<BorrowMainResourceMangInventoryRecordModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public List<BorrowMainResourceMangInventoryDetailModel> GetInventoryDetail(string InventoryRecordID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@InventoryRecordID", InventoryRecordID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.SecondResourceNo, A.SecondResourceName, A.ShelvesStatus, B.Text AS ShelvesStatusText, 
                                    A.BorrowStatus, C.Text AS BorrowStatusText, A.ResourceInventoryStatus, D.Text AS ResourceInventoryStatusText
                               FROM InventoryDetail A
							   LEFT JOIN Code B ON B.Code = A.ShelvesStatus AND B.Type = 'ShelvesStatus'
							   LEFT JOIN Code C ON C.Code = A.BorrowStatus AND C.Type = 'BorrowStatus'
							   LEFT JOIN Code D ON D.Code = A.ResourceInventoryStatus AND D.Type = 'InventoryStatus'
                              WHERE A.InventoryRecordID = @InventoryRecordID ";


            (DbExecuteInfo info, IEnumerable<BorrowMainResourceMangInventoryDetailModel> entitys) dbResult = DbaExecuteQuery<BorrowMainResourceMangInventoryDetailModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowMainResourceMangInventoryDetailModel>();
        }

        public DbExecuteInfo CreateInventoryRecord(BorrowMainResourceMangInventoryRecordModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            DataSet ds = new DataSet();

            #region 參數設定
            parameters.Add("@MainResourceID", vm.MainResourceID);
            parameters.Add("@AmtReal", vm.AmtReal);
            parameters.Add("@BorrowType", vm.BorrowType);
            parameters.Add("@LoginId", LoginUser.LoginId);

            #endregion 參數設定

            string CommendText = $@"INSERT INTO InventoryRecord 
                                              (MainResourceID
                                              ,AmtReal
                                              ,BorrowType
                                              ,Creator
                                              ,Created
                                              ,LastModifier
                                              ,LastModified) 
                                       OUTPUT inserted.ID
                                       VALUES (@MainResourceID
                                               ,@AmtReal
                                               ,@BorrowType
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE())
";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        public DbExecuteInfo UpdMainResourceToInventory(string MainResourceID, string InventoryStatus)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", MainResourceID);
            #endregion 參數設定

            string CommendText = $@"UPDATE BorrowMainResourceMang 
                                       SET InventoryStatus = '{InventoryStatus}'
                                     WHERE MainResourceID = @MainResourceID
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo UpdSecondResourceToInventory(string MainResourceID)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", MainResourceID);
            #endregion 參數設定

            string CommendText = $@"UPDATE BorrowSecondResourceMang 
                                       SET BorrowStatus = '07'
                                     WHERE MainResourceID = @MainResourceID
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<BorrowMainResourceMangInventoryDetailModel> GetSecondResource(string MainResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainResourceID", MainResourceID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.SecondResourceNo, A.SecondResourceName, A.ShelvesStatus, B.Text AS ShelvesStatusText, A.BorrowStatus, C.Text AS BorrowStatusText, D.AmtReal AS ThinkAmt, D.BorrowType, E.Text
                               FROM BorrowSecondResourceMang A
							   LEFT JOIN Code B ON B.Code = A.ShelvesStatus AND B.Type = 'ShelvesStatus'
							   LEFT JOIN Code C ON C.Code = A.BorrowStatus AND C.Type = 'BorrowStatus'
							   LEFT JOIN BorrowMainResourceMang D ON D.MainResourceID = A.MainResourceID
							   LEFT JOIN Code E ON E.Code = D.BorrowType AND E.Type = 'BorrowMultType'
                              WHERE A.MainResourceID = @MainResourceID ";


            (DbExecuteInfo info, IEnumerable<BorrowMainResourceMangInventoryDetailModel> entitys) dbResult = DbaExecuteQuery<BorrowMainResourceMangInventoryDetailModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowMainResourceMangInventoryDetailModel>();
        }

        public DbExecuteInfo InserInventoryDetailData(List<BorrowMainResourceMangInventoryDetailModel> dataList, string RecodeOrder, UserInfo loginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            
            string CommendText = "";

            CommendText = $@"INSERT INTO InventoryDetail
                                            (InventoryRecordID
                                            ,SecondResourceNo
                                           ,SecondResourceName
                                           ,ShelvesStatus
                                           ,BorrowStatus
                                           ,ThinkAmt
                                           ,ResourceInventoryStatus
                                            ,Creator
                                            ,Created
                                            ,LastModifier
                                            ,LastModified)
                                        VALUES
                                            ('{RecodeOrder}'
                                            ,@SecondResourceNo
                                            ,@SecondResourceName
                                            ,@ShelvesStatus
                                            ,@BorrowStatus
                                           ,'1'
                                            ,'02'
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        public DbExecuteInfo updSecondResourceStatus(List<BorrowMainResourceMangInventoryDetailModel> dataList, string RecodeOrder, UserInfo loginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = "";

            CommendText = $@"UPDATE BorrowSecondResourceMang 
                                SET ShelvesStatus = @ShelvesStatus, 
                                    BorrowStatus = @BorrowStatus,
                                    LastModifier = '{loginUser.LoginId}', 
                                    LastModified = GETDATE()
                              WHERE SecondResourceNo = @SecondResourceNo
";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        public DbExecuteInfo UpdInventoryDetailData(string RecordOrderID, string DeviceID, UserInfo loginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"UPDATE InventoryDetail 
                                           SET InventoryAmt = '1',
                                               ResourceInventoryStatus = '03',    
                                               LastModifier = '{loginUser.LoginId}',
                                               LastModified = GETDATE()
                                         WHERE InventoryRecordID = '{RecordOrderID}'
                                           AND SecondResourceNo = '{DeviceID}'
";
            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo UpdInventoryDetailMultData(string RecordOrderID, string Amt, UserInfo loginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"UPDATE TOP ({Amt}) InventoryDetail 
                                       SET InventoryAmt = '1', 
                                           ResourceInventoryStatus = '03', 
                                           LastModifier = '{loginUser.LoginId}',
                                           LastModified = GETDATE() 
                                     WHERE InventoryRecordID = '{RecordOrderID}' ;";


            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo updInventoryAmtToRecord(string ID, string AmtInventory, UserInfo loginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"UPDATE InventoryRecord 
                                           SET AmtInventory = '{AmtInventory}',
                                               LastModifier = '{loginUser.LoginId}',
                                               LastModified = GETDATE()
                                         WHERE ID = '{ID}'
";
            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }





        public DataTable GetInventoryAmt(string InventoryRecordID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@InventoryRecordID", InventoryRecordID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT SUM(inventoryAmt) AS Amt 
                               FROM InventoryDetail
                              WHERE InventoryRecordID = @InventoryRecordID";


            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }


        public DataTable SearchInventoryRecord(string MainResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainResourceID", MainResourceID);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT ID 
                              FROM InventoryRecord
                             WHERE 1 = 1
                               AND MainResourceID = @MainResourceID AND AmtInventory is null";


            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }


        public DataTable GetMainResourceInventoryStatus(string MainResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainResourceID", MainResourceID);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT InventoryStatus 
                              FROM BorrowMainResourceMang
                             WHERE 1 = 1
                               AND MainResourceID = @MainResourceID";


            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }

        public DataTable GetMainResourceID(string MainResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainResourceID", MainResourceID);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT MainResourceID 
                              FROM BorrowMainResourceMang
                             WHERE 1 = 1
                               AND MainResourceID = @MainResourceID";


            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
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

        public List<SelectListItem> GetddlSecondClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM BorrowSecondClassMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlEnable()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'Enable'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }


        public List<SelectListItem> GetddlBorrowMultType()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'BorrowMultType'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
