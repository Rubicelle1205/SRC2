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
    
    public class BorrowRecordMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BorrowRecordMangResultModel> GetSearchResult(BorrowRecordMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainClassID", model?.MainClassID);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ApplyUnitName", model?.ApplyUnitName);
            parameters.Add("@ApplyMan", model?.ApplyMan);
            parameters.Add("@ApplyEmail", model?.ApplyEmail);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy-MM-dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy-MM-dd 23:59:59") : null);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.BorrowMainID, A.MainClassID, B.Text AS MainClassIDText, 
                                    A.ApplyUnitType, A.ApplyUnitName, A.ApplyMan, A.ApplyTitle, A.TakeSDate, A.TakeEDate, 
                                    A.ActVerify, C.Text AS ActVerifyText, A.Created
                               FROM BorrowMain A
                          LEFT JOIN BorrowMainClassMang B ON B.ID = A.MainClassID
                          LEFT JOIN Code C ON C.Code = A.ActVerify AND C.Type = 'BorrowActVerify'
WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@MainClassID IS NULL OR A.MainClassID = @MainClassID) 
AND (@ActVerify IS NULL OR A.ActVerify = @ActVerify) 
AND (@ApplyUnitName IS NULL OR A.ApplyUnitName LIKE '%' + @ApplyUnitName + '%') 
AND (@ApplyMan IS NULL OR A.ApplyMan LIKE '%' + @ApplyMan + '%') 
AND (@ApplyEmail IS NULL OR A.ApplyEmail LIKE '%' + @ApplyEmail + '%') ";


            (DbExecuteInfo info, IEnumerable<BorrowRecordMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowRecordMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowRecordMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BorrowRecordMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.BorrowMainID, A.MainClassID, B.Text AS MainClassIDText, A.ApplyUnitType, A.ApplyUnitName, A.ApplyMan, A.ApplyTitle, A.ApplyEmail, A.ApplyTel, 
                                    A.ApplyPurpose, A.ActName, A.UseLocation, A.UseDesc, A.UseSDate, A.UseEDate, A.TakeSDate, A.TakeEDate, A.BorrowMemo, 
                                    A.TeacherMark, A.DeviceMark, A.TakeMark, A.ReturnMark, A.Memo, A.ActVerify, A.Creator, A.Created, A.LastModifier, A.LastModified
                              FROM BorrowMain A
                         LEFT JOIN BorrowMainClassMang B ON B.ID = A.MainClassID
WHERE 1 = 1
AND (A.BorrowMainID = @ID) ";


            (DbExecuteInfo info, IEnumerable<BorrowRecordMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowRecordMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public List<EventData> GetEventData(string BorrowMainID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@BorrowMainID", BorrowMainID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.EventID, B.Text AS EventIDText, A.EventDateTime, A.Text
                               FROM BorrowHistroy A
						  LEFT JOIN Code B ON B.Code = A.EventID AND B.Type = 'BorrowActVerify'
                              WHERE 1 = 1
                                AND BorrowMainID = @BorrowMainID
						   ORDER BY A.EventDateTime DESC

";


            (DbExecuteInfo info, IEnumerable<EventData> entitys) dbResult = DbaExecuteQuery<EventData>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<EventData>();
        }

        public List<BorrowRecordMangFileModel> GetFileData(string submitBtn)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();


            #region 參數設定

            parameters.Add("@BorrowMainID", submitBtn);

            #endregion

            CommandText = $@"SELECT ID
      ,BorrowMainID
      ,FileName
      ,FilePath
      ,Creator
      ,Created
      ,LastModifier
      ,LastModified
                               FROM BorrowFile
WHERE 1 = 1
AND BorrowMainID = @BorrowMainID";


            (DbExecuteInfo info, IEnumerable<BorrowRecordMangFileModel> entitys) dbResult = DbaExecuteQuery<BorrowRecordMangFileModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowRecordMangFileModel>();
        }


        public List<BorrowRecordMangDeviceModel> GetDeviceData(string submitBtn)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();


            #region 參數設定

            parameters.Add("@BorrowMainID", submitBtn);

            #endregion

            CommandText = $@"SELECT A.ID, A.MainClassID, B.Text AS MainClassIDText, A.MainResourceID, C.MainResourceName AS MainResourceIDText, A.BorrowAmt, 
                                    D.BorrowType, E.Text AS BorrowTypeText, A.BorrowStatus, F.Text AS BorrowStatusText, A.BorrowSecondResourceID, A.BorrowRealAmt, A.ReturnRealAmt
                               FROM BorrowDevice A
                          LEFT JOIN BorrowMainClassMang B ON b.ID = A.MainClassID
                          LEFT JOIN BorrowMainResourceMang C ON C.MainResourceID = A.MainResourceID
						  LEFT JOIN BorrowMainResourceMang D ON D.MainResourceID = A.MainResourceID
						  LEFT JOIN Code E ON E.Code = D.BorrowType AND E.Type = 'BorrowMultType'
						  LEFT JOIN Code F ON F.Code = A.BorrowStatus AND F.Type = 'Borrowstatus'
                              WHERE 1 = 1
                                AND A.BorrowMainID = @BorrowMainID";


            (DbExecuteInfo info, IEnumerable<BorrowRecordMangDeviceModel> entitys) dbResult = DbaExecuteQuery<BorrowRecordMangDeviceModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowRecordMangDeviceModel>();
        }

        #region 新增

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertMainData(BorrowRecordMangViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainClassID", vm.CreateModel.MainClassID);
            parameters.Add("@ApplyUnitType", vm.CreateModel.ApplyUnitType);
            parameters.Add("@ApplyUnitName", vm.CreateModel.ApplyUnitName);
            parameters.Add("@ApplyMan", vm.CreateModel.ApplyMan);
            parameters.Add("@ApplyTitle", vm.CreateModel.ApplyTitle);
            parameters.Add("@ApplyEmail", vm.CreateModel.ApplyEmail);
            parameters.Add("@ApplyTel", vm.CreateModel.ApplyTel);
            parameters.Add("@ApplyPurpose", vm.CreateModel.ApplyPurpose);
            parameters.Add("@ActName", vm.CreateModel.ActName);
            parameters.Add("@UseLocation", vm.CreateModel.UseLocation);
            parameters.Add("@UseDesc", vm.CreateModel.UseDesc);
            parameters.Add("@UseSDate", vm.CreateModel.UseSDate);
            parameters.Add("@UseEDate", vm.CreateModel.UseEDate);
            parameters.Add("@TakeSDate", vm.CreateModel.TakeSDate);
            parameters.Add("@TakeEDate", vm.CreateModel.TakeEDate);
            parameters.Add("@BorrowMemo", vm.CreateModel.BorrowMemo);
            parameters.Add("@TeacherMark", vm.CreateModel.TeacherMark);
            parameters.Add("@DeviceMark", vm.CreateModel.DeviceMark);
            parameters.Add("@TakeMark", vm.CreateModel.TakeMark);
            parameters.Add("@ReturnMark", vm.CreateModel.ReturnMark);
            parameters.Add("@ActVerify", vm.CreateModel.ActVerify);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO BorrowMain
                                               (MainClassID, 
                                                ApplyUnitType, 
                                                ApplyUnitName, 
                                                ApplyMan, 
                                                ApplyTitle, 
                                                ApplyEmail, 
                                                ApplyTel, 
                                                ApplyPurpose, 
                                                ActName, 
                                                UseLocation, 
                                                UseDesc, 
                                                UseSDate, 
                                                UseEDate, 
                                                TakeSDate, 
                                                TakeEDate, 
                                                BorrowMemo, 
                                                TeacherMark, 
                                                DeviceMark, 
                                                TakeMark, 
                                                ReturnMark, 
                                                ActVerify, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified )
                                         OUTPUT Inserted.BorrowMainID
                                         VALUES
                                               (@MainClassID, 
                                                @ApplyUnitType, 
                                                @ApplyUnitName, 
                                                @ApplyMan, 
                                                @ApplyTitle, 
                                                @ApplyEmail, 
                                                @ApplyTel, 
                                                @ApplyPurpose, 
                                                @ActName, 
                                                @UseLocation, 
                                                @UseDesc, 
                                                @UseSDate, 
                                                @UseEDate, 
                                                @TakeSDate, 
                                                @TakeEDate, 
                                                @BorrowMemo, 
                                                @TeacherMark, 
                                                @DeviceMark, 
                                                @TakeMark, 
                                                @ReturnMark, 
                                                @ActVerify, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE() )";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        public DbExecuteInfo InsertDeviceData(List<BorrowRecordMangDeviceModel> dataList, UserInfo loginUser, string BorrowMainID)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO BorrowDevice
                                            (BorrowMainID
                                            ,MainClassID
                                            ,MainResourceID
                                            ,BorrowStatus
                                            ,BorrowAmt
                                            ,Creator
                                            ,Created
                                            ,LastModifier
                                            ,LastModified)
                                        VALUES
                                            ('{BorrowMainID}'
                                            ,@MainClassID
                                            ,@MainResourceID
                                            ,@BorrowStatus
                                            ,@BorrowAmt
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        public DbExecuteInfo InsertFileData(List<BorrowRecordMangFileModel> dataList, UserInfo loginUser, string BorrowMainID)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO BorrowFile
                                            (BorrowMainID
                                            ,FileName
                                            ,FilePath
                                            ,Creator
                                            ,Created
                                            ,LastModifier
                                            ,LastModified)
                                        VALUES
                                            ('{BorrowMainID}'
                                            ,@FileName
                                            ,@FilePath
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        /// <summary> 新增事件原因及經過資料 </summary>
        public DbExecuteInfo InsertEventData(List<EventData> dataList, UserInfo loginUser, string BorrowMainID)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO BorrowHistroy
                                            (BorrowMainID
                                            ,EventID
                                            ,EventDateTime
                                            ,Text
                                            ,Creator
                                            ,Created
                                            ,LastModifier
                                            ,LastModified)
                                        VALUES
                                            ('{BorrowMainID}'
                                            ,@EventID
                                            ,@EventDateTime
                                            ,@Text
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        #endregion

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(BorrowRecordMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@BorrowMainID", vm.EditModel.BorrowMainID);

            parameters.Add("@ApplyUnitType", vm.EditModel.ApplyUnitType);
            parameters.Add("@ApplyUnitName", vm.EditModel.ApplyUnitName);
            parameters.Add("@ApplyMan", vm.EditModel.ApplyMan);
            parameters.Add("@ApplyTitle", vm.EditModel.ApplyTitle);
            parameters.Add("@ApplyEmail", vm.EditModel.ApplyEmail);
            parameters.Add("@ApplyTel", vm.EditModel.ApplyTel);
            parameters.Add("@ApplyPurpose", vm.EditModel.ApplyPurpose);
            parameters.Add("@ActName", vm.EditModel.ActName);
            parameters.Add("@UseLocation", vm.EditModel.UseLocation);
            parameters.Add("@UseDesc", vm.EditModel.UseDesc);
            parameters.Add("@UseSDate", vm.EditModel.UseSDate);
            parameters.Add("@UseEDate", vm.EditModel.UseEDate);
            parameters.Add("@TakeSDate", vm.EditModel.TakeSDate);
            parameters.Add("@TakeEDate", vm.EditModel.TakeEDate);
            parameters.Add("@BorrowMemo", vm.EditModel.BorrowMemo);
            parameters.Add("@ActVerify", vm.EditModel.ActVerify);
            parameters.Add("@TeacherMark", vm.EditModel.TeacherMark);
            parameters.Add("@DeviceMark", vm.EditModel.DeviceMark);
            parameters.Add("@TakeMark", vm.EditModel.TakeMark);
            parameters.Add("@ReturnMark", vm.EditModel.ReturnMark);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE BorrowMain 
                                       SET ApplyUnitType = @ApplyUnitType,
                                            ApplyUnitName = @ApplyUnitName, 
                                            ApplyMan = @ApplyMan, 
                                            ApplyTitle = @ApplyTitle, 
                                            ApplyEmail = @ApplyEmail, 
                                            ApplyTel = @ApplyTel, 
                                            ApplyPurpose = @ApplyPurpose, 
                                            ActName = @ActName, 
                                            UseLocation = @UseLocation, 
                                            UseDesc = @UseDesc, 
                                            UseSDate = @UseSDate, 
                                            UseEDate = @UseEDate, 
                                            TakeSDate = @TakeSDate, 
                                            TakeEDate = @TakeEDate, 
                                            BorrowMemo = @BorrowMemo, 
                                            ActVerify = @ActVerify, 
                                            TeacherMark = @TeacherMark, 
                                            TakeMark = @TakeMark, 
                                            ReturnMark = @ReturnMark, 
                                            Memo = @Memo, 
                                            LastModifier = @LoginId, 
                                            LastModified = GETDATE()

                                     WHERE BorrowMainID = @BorrowMainID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 新增事件原因及經過資料 </summary>
        public DbExecuteInfo UpdateEventData(BorrowRecordMangViewModel vm, UserInfo loginUser, string BorrowMainID)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO BorrowHistroy
                                           (BorrowMainID
                                            ,EventID
                                            ,EventDateTime
                                            ,Text
                                            ,Creator
                                            ,Created
                                            ,LastModifier
                                            ,LastModified)
                                        VALUES
                                            ('{BorrowMainID}'
                                            ,'{vm.EditModel.EventID}'
                                            ,'{vm.EditModel.EventDateTime}'
                                            ,'{vm.EditModel.EventText}'
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo UpdDeviceBorrowSecondResource(string DeviceID, string selectedSecondResourceID, UserInfo loginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"UPDATE BorrowDevice 
                                           SET BorrowSecondResourceID = '{selectedSecondResourceID}',
                                               BorrowRealAmt = '1',
                                               LastModifier = '{loginUser.LoginId}',
                                               LastModified = GETDATE()
                                         WHERE ID = '{DeviceID}'
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo UpdMultDeviceBorrowSecondResource(string DeviceID, string BorrowSecondResourceID, string BorrowAmt, UserInfo loginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"UPDATE BorrowDevice 
                                           SET BorrowSecondResourceID = '{BorrowSecondResourceID}',
                                               BorrowRealAmt = '{BorrowAmt}',
                                               BorrowStatus = '02', 
                                               LastModifier = '{loginUser.LoginId}',
                                               LastModified = GETDATE()
                                         WHERE ID = '{DeviceID}'
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo UpdMultDeviceRetrunSecondResource(string DeviceID, string BorrowSecondResourceID, string BorrowAmt, UserInfo loginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"UPDATE BorrowDevice 
                                           SET BorrowSecondResourceID = '{BorrowSecondResourceID}',
                                               ReturnRealAmt = '{BorrowAmt}',
                                               BorrowStatus = '03',    
                                               LastModifier = '{loginUser.LoginId}',
                                               LastModified = GETDATE()
                                         WHERE ID = '{DeviceID}'
";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DataTable GetMailReceiver(string BorrowMainID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@BorrowMainID", BorrowMainID);
            #endregion

            CommandText = @"SELECT ApplyMan, ApplyEmail
                              FROM BorrowMain
                             WHERE 1 = 1
                               AND BorrowMainID = @BorrowMainID ";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }

        #region 刪除

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeletetMainData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = "";

            #region 參數設定
            parameters.Add("@ID", ser);
            #endregion 參數設定

            CommendText = $@"DELETE FROM BorrowMain WHERE BorrowMainID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo DeletetDeviceData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = "";

            #region 參數設定
            parameters.Add("@ID", ser);
            #endregion 參數設定

            CommendText = $@"DELETE FROM BorrowDevice WHERE ID = @ID ";
            
            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo DeletetFileData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = "";

            #region 參數設定
            parameters.Add("@ID", ser);
            #endregion 參數設定

            CommendText = $@"DELETE FROM BorrowFile WHERE BorrowMainID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo DeletetHistroyData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();
            string CommendText = "";

            #region 參數設定
            parameters.Add("@ID", ser);
            #endregion 參數設定

            CommendText = $@"DELETE FROM BorrowHistroy WHERE BorrowMainID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        #endregion




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

        public List<SelectListItem> GetddlBorrowActVerify()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type like 'BorrowActVerify'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlApplyUnitType()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type like 'ApplyUnitType'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlSecondResurce()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT A.MainResourceID AS VALUE, '(' + B.Text + ')' + A.MainResourceName AS TEXT
                              FROM BorrowMainResourceMang A
                         LEFT JOIN BorrowMainClassMang B ON B.ID = A.MainClass";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public DataTable GetBorrowAmt(string MainResourceID, out DataTable dt)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", MainResourceID);
            #endregion

            CommandText = @"SELECT MainResourceID, AmtShelves, AmtOnce
                              FROM BorrowMainResourceMang
                             WHERE 1 = 1
                               AND MainResourceID = @MainResourceID ";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return dt;
        }

        public DataTable GetMainResourceID(string SecondResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SecondResourceID", SecondResourceID);
            #endregion

            CommandText = @"SELECT MainClass, BorrowType
                              FROM BorrowMainResourceMang
                             WHERE 1 = 1
                               AND MainResourceID = @SecondResourceID ";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }

        public List<SelectListItem> GetddlSecondResource(string MainResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", MainResourceID);
            #endregion

            CommandText = @"SELECT A.SecondResourceNo AS VALUE, '(' + A.SecondResourceNo + ')' + A.SecondResourceName AS TEXT
                              FROM BorrowSecondResourceMang A
                         WHERE MainResourceID = @MainResourceID";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public DataTable GetOrderBorrowAmt(string BorrowMainID, string MainResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@BorrowMainID", BorrowMainID);
            parameters.Add("@MainResourceID", MainResourceID);
            #endregion

            CommandText = @"SELECT BorrowAmt
                              FROM BorrowDevice
                             WHERE 1 = 1
                               AND MainResourceID = @MainResourceID 
                               AND BorrowMainID = @BorrowMainID ";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }


    }
}
