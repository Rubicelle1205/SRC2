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
using static WebPccuClub.Models.FBorrowRecordViewModel;

namespace WebPccuClub.DataAccess
{
    
    public class FBorrowRecordDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<FBorrowRecordResultModel> GetSearchResult(UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();


            #region 參數設定

            parameters.Add("@Creator", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT A.BorrowMainID, A.ActVerify, B.Text AS ActVerifyText, A.ActName, A.Created
                               FROM BorrowMain A
                          LEFT JOIN Code B ON B.Code = A.ActVerify AND B.Type = 'BorrowActVerify'
WHERE 1 = 1
AND Creator = @Creator";


            (DbExecuteInfo info, IEnumerable<FBorrowRecordResultModel> entitys) dbResult = DbaExecuteQuery<FBorrowRecordResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FBorrowRecordResultModel>();
        }


        #region 新增

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertMainData(FBorrowRecordViewModel vm, UserInfo LoginUser, out DataTable dt)
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

        public DbExecuteInfo InsertDeviceData(List<FBorrowRecordDeviceModel> dataList, UserInfo loginUser, string BorrowMainID)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO BorrowDevice
                                            (BorrowMainID
                                            ,MainResourceID
                                            ,SecondResourceNo
                                            ,BorrowStatus
                                            ,BorrowAmt
                                            ,Creator
                                            ,Created
                                            ,LastModifier
                                            ,LastModified)
                                        VALUES
                                            ('{BorrowMainID}'
                                            ,@MainResourceID
                                            ,@SecondResourceNo
                                            ,@BorrowStatus
                                            ,@BorrowAmt
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE()
                                            ,'{loginUser.LoginId}'
                                            ,GETDATE() )";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        public DbExecuteInfo InsertFileData(List<FBorrowRecordFileModel> dataList, UserInfo loginUser, string BorrowMainID)
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



    }
}
