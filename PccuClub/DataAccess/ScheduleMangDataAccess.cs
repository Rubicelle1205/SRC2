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

    public class ScheduleMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ScheduleMangResultModel> GetSearchResult(ScheduleMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubId", model.ClubId);
            parameters.Add("@ActTypeID", model.ActTypeID);
            parameters.Add("@ActHoldType", model.ActHoldType);
            parameters.Add("@CScheName", model.CScheName);
            parameters.Add("@SchoolYear", model.SchoolYear);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy-MM-dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy-MM-dd 23:59:59") : null);

         
            #endregion

            CommandText = $@"SELECT A.CScheID, A.ClubID, A.SchoolYear, A.ActTypeID, B.ActTypeName, A.CScheName, 
                                    A.CScheDate, A.Budget, A.ActHoldType, C.Text AS ActHoldTypeText, A.Created
                               FROM ClubSchedule A
                          LEFT JOIN ActTypeMang B ON B.ActTypeID = A.ActTypeID
                          LEFT JOIN Code C ON C.Code = A.ActHoldType AND C.Type = 'ActHoldType'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.CScheDate BETWEEN @FromDate AND @ToDate" : " ")}
{(model.ClubId != null ? " AND A.ClubId LIKE '%' + @ClubId + '%'" : " ")}
{(model.CScheName != null ? " AND A.CScheName LIKE '%' + @CScheName + '%'" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear) 
AND (@ActTypeID IS NULL OR A.ActTypeID = @ActTypeID) 
AND (@ActHoldType IS NULL OR A.ActHoldType = @ActHoldType) 
";

            (DbExecuteInfo info, IEnumerable<ScheduleMangResultModel> entitys) dbResult = DbaExecuteQuery<ScheduleMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ScheduleMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ScheduleMangEditModel GetEditData(string CScheID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@CScheID", CScheID);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT A.CScheID, A.ClubID, A.SchoolYear, A.ActTypeID, A.CScheName, A.CScheDate, A.Budget, A.BookingPlace, A.ShortDesc, 
                                   A.ActHoldType, A.Support, A.Participants, A.Satisfaction, A.Attachment, A.Memo, A.Created, A.LastModified
                               FROM ClubSchedule A
                              WHERE 1 = 1
                               AND A.CScheID = @CScheID";


            (DbExecuteInfo info, IEnumerable<ScheduleMangEditModel> entitys) dbResult = DbaExecuteQuery<ScheduleMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ScheduleMangViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubId", vm.CreateModel.ClubId);
            parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@ActTypeID", vm.CreateModel.ActTypeID);
            parameters.Add("@CScheName", vm.CreateModel.CScheName);
            parameters.Add("@CScheDate", vm.CreateModel.CScheDate);
            parameters.Add("@Budget", vm.CreateModel.Budget);
            parameters.Add("@BookingPlace", vm.CreateModel.BookingPlace);
            parameters.Add("@ShortDesc", vm.CreateModel.ShortDesc);
            parameters.Add("@ActHoldType", vm.CreateModel.ActHoldType);
            parameters.Add("@Support", vm.CreateModel.Support);
            parameters.Add("@Participants", vm.CreateModel.Participants);
            parameters.Add("@Satisfaction", vm.CreateModel.Satisfaction);
            parameters.Add("@Attachment", vm.CreateModel.Attachment);
            parameters.Add("@Memo", vm.CreateModel.Memo);

            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ClubSchedule
                                                (ClubID, 
                                                SchoolYear, 
                                                ActTypeID, 
                                                CScheName, 
                                                CScheDate, 
                                                Budget, 
                                                BookingPlace, 
                                                ShortDesc,  
                                                ActHoldType, 
                                                Support, 
                                                Participants, 
                                                Satisfaction, 
                                                Attachment, 
                                                Memo, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified, 
                                                ModifiedReason)
                                         VALUES
                                               (@ClubID, 
                                                @SchoolYear, 
                                                @ActTypeID, 
                                                @CScheName, 
                                                @CScheDate, 
                                                @Budget, 
                                                @BookingPlace, 
                                                @ShortDesc,  
                                                @ActHoldType, 
                                                @Support, 
                                                @Participants, 
                                                @Satisfaction, 
                                                @Attachment, 
                                                @Memo, 
                                                @LastModifier,
                                                GETDATE(),
                                                @LastModifier,
                                                GETDATE(),
                                                NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ScheduleMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定


            parameters.Add("@CScheID", vm.EditModel.CScheID);
            parameters.Add("@ClubId", vm.EditModel.ClubId);
            parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@ActTypeID", vm.EditModel.ActTypeID);
            parameters.Add("@CScheName", vm.EditModel.CScheName);
            parameters.Add("@CScheDate", vm.EditModel.CScheDate);
            parameters.Add("@Budget", vm.EditModel.Budget);
            parameters.Add("@BookingPlace", vm.EditModel.BookingPlace);
            parameters.Add("@ShortDesc", vm.EditModel.ShortDesc);
            parameters.Add("@ActHoldType", vm.EditModel.ActHoldType);
            parameters.Add("@Support", vm.EditModel.Support);
            parameters.Add("@Participants", vm.EditModel.Participants);
            parameters.Add("@Satisfaction", vm.EditModel.Satisfaction);
            parameters.Add("@Attachment", vm.EditModel.Attachment);
            parameters.Add("@Memo", vm.EditModel.Memo);

            parameters.Add("@LastModifier", LoginUser.LoginId);
                #endregion 參數設定

                CommendText = $@"UPDATE ClubSchedule 
                                           SET  SchoolYear = @SchoolYear, 
                                                ActTypeID = @ActTypeID, 
                                                CScheName = @CScheName, 
                                                CScheDate = @CScheDate, 
                                                Budget = @Budget, 
                                                BookingPlace = @BookingPlace, 
                                                ShortDesc = @ShortDesc,  
                                                ActHoldType = @ActHoldType, 
                                                Support = @Support, 
                                                Participants = @Participants, 
                                                Satisfaction = @Satisfaction, 
                                                Attachment = @Attachment, 
                                                Memo = @Memo, 
                                                LastModifier = @LastModifier, 
                                                LastModified = GETDATE()
                                         WHERE CScheID = @CScheID ";


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
            parameters.Add("@CScheID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM ClubSchedule WHERE CScheID = @CScheID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }


        /// <summary>
        /// Excel 取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ScheduleMangExcelModel> GetExportResult(ScheduleMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubId", model.ClubId);
            parameters.Add("@ActTypeID", model.ActTypeID);
            parameters.Add("@ActHoldType", model.ActHoldType);
            parameters.Add("@CScheName", model.CScheName);
            parameters.Add("@SchoolYear", model.SchoolYear);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);


            #endregion

            CommandText = $@"SELECT A.CScheID, A.ClubID, A.SchoolYear, A.ActTypeID, B.ActTypeName, A.CScheName, 
                                    A.CScheDate, A.Budget, A.ActHoldType, C.Text AS ActHoldTypeText, A.Created
                               FROM ClubSchedule A
                          LEFT JOIN ActTypeMang B ON B.ActTypeID = A.ActTypeID
                          LEFT JOIN Code C ON C.Code = A.ActHoldType AND C.Type = 'ActHoldType'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.CScheDate BETWEEN @FromDate AND @ToDate" : " ")}
{(model.ClubId != null ? " AND A.ClubId LIKE '%' + @ClubId + '%'" : " ")}
{(model.CScheName != null ? " AND A.CScheName LIKE '%' + @CScheName + '%'" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear) 
AND (@ActTypeID IS NULL OR A.ActTypeID = @ActTypeID) 
AND (@ActHoldType IS NULL OR A.ActHoldType = @ActHoldType) ";

            (DbExecuteInfo info, IEnumerable<ScheduleMangExcelModel> entitys) dbResult = DbaExecuteQuery<ScheduleMangExcelModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ScheduleMangExcelModel>();
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

        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            for (int i = 108; i <= 130; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(),  Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
        }

        public List<SelectListItem> GetAllActHoldType()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'ActHoldType'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllActType()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ActTypeID AS VALUE, ActTypeName AS TEXT FROM ActTypeMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetAllClub()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ClubID AS VALUE,  '(' + ClubID + ')' + ClubCName AS TEXT FROM ClubMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo ImportData(List<ScheduleMangImportExcelModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion 參數設定

            string CommendText = $@"INSERT INTO ClubSchedule
                                               (ClubID, 
                                                SchoolYear, 
                                                ActTypeID, 
                                                CScheName, 
                                                CScheDate, 
                                                Budget, 
                                                BookingPlace, 
                                                ShortDesc, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified, 
                                                ModifiedReason)
                                         VALUES
                                               (@ClubID, 
                                                @SchoolYear, 
                                                @ActTypeID, 
                                                @CScheName, 
                                                @CScheDate, 
                                                @Budget, 
                                                @BookingPlace, 
                                                @ShortDesc, 
                                               '{LoginUser.LoginId}', 
                                               GETDATE(), 
                                               '{LoginUser.LoginId}', 
                                               GETDATE(), 
                                               NULL)";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

    }
}
