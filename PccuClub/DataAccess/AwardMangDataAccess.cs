using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebPccuClub.DataAccess
{
    
    public class AwardMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<AwardMangResultModel> GetSearchResult(AwardMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@Organizer", model?.Organizer);
            parameters.Add("@AwdActName", model?.AwdActName);
            parameters.Add("@AwdType", model?.AwdType);
            parameters.Add("@AwdName", model?.AwdName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy-MM-dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy-MM-dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.AwdID, A.ClubID, B.ClubCName, A.SchoolYear, A.AwdDate, A.AwdActName, A.AwdType, A.AwdName, 
                                    A.Organizer, A.ActVerify, C.Text AS ActVerifyText, A.Created
                               FROM AwardMang A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                          LEFT JOIN Code C ON C.Code = A.ActVerify AND C.Type = 'ActVerify'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.AwdDate BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR A.ActVerify = @ActVerify)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR B.ClubCName LIKE '%' + @ClubCName + '%') 
AND (@Organizer IS NULL OR A.Organizer LIKE '%' + @Organizer + '%') 
AND (@AwdActName IS NULL OR A.AwdActName LIKE '%' + @AwdActName + '%') 
AND (@AwdType IS NULL OR A.AwdType LIKE '%' + @AwdType + '%') 
AND (@AwdName IS NULL OR A.AwdName LIKE '%' + @AwdName + '%') ";


            (DbExecuteInfo info, IEnumerable<AwardMangResultModel> entitys) dbResult = DbaExecuteQuery<AwardMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<AwardMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public AwardMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@AwdID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT AwdID, ClubID, SchoolYear, AwdName, AwdDate, AwdActName, AwdType, Organizer, ActVerify, Attachment, Memo, Creator, 
                                    Created, LastModifier, LastModified
                               FROM AwardMang 
                              WHERE 1 = 1
                                AND (AwdID = @AwdID) ";


            (DbExecuteInfo info, IEnumerable<AwardMangEditModel> entitys) dbResult = DbaExecuteQuery<AwardMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(AwardMangViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", vm.CreateModel.ClubID);
            parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@AwdActName", vm.CreateModel.AwdActName);
            parameters.Add("@AwdName", vm.CreateModel.AwdName);
            parameters.Add("@AwdDate", vm.CreateModel.AwdDate);
            parameters.Add("@AwdType", vm.CreateModel.AwdType);
            parameters.Add("@Organizer", vm.CreateModel.Organizer);
            parameters.Add("@ActVerify", vm.CreateModel.ActVerify);
            parameters.Add("@Attachment", vm.CreateModel.Attachment);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO AwardMang
                                               (ClubID, 
                                                SchoolYear, 
                                                AwdActName, 
                                                AwdName, 
                                                AwdDate, 
                                                AwdType, 
                                                Organizer, 
                                                ActVerify, 
                                                Attachment, 
                                                Memo, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified, 
                                                ModifiedReason)
                                         OUTPUT Inserted.AwdID
                                         VALUES
                                               (@ClubID, 
                                                @SchoolYear, 
                                                @AwdActName, 
                                                @AwdName, 
                                                @AwdDate, 
                                                @AwdType, 
                                                @Organizer, 
                                                @ActVerify, 
                                                @Attachment, 
                                                @Memo, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE(), 
                                                NULL)";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            dt = ds.Tables[0];

            return ExecuteResult;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertDetailData(string AwdID, List<AwdDetailModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"INSERT INTO AwardDetailMang
                                               (AwdID
                                               ,Name
                                               ,SNO
                                               ,Department
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               ('{AwdID}'
                                               ,@Name
                                               ,@SNO
                                               ,@Department
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(AwardMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@AwdID", vm.EditModel.AwdID);
            parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@AwdActName", vm.EditModel.AwdActName); 
            parameters.Add("@AwdDate", vm.EditModel.AwdDate);
            parameters.Add("@AwdType", vm.EditModel.AwdType);
            parameters.Add("@AwdName", vm.EditModel.AwdName);
            parameters.Add("@Organizer", vm.EditModel.Organizer);
            parameters.Add("@ActVerify", vm.EditModel.ActVerify);

            if(!string.IsNullOrEmpty(vm.EditModel.Attachment))
                parameters.Add("@Attachment", vm.EditModel.Attachment);

            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@" UPDATE AwardMang 
                                        SET SchoolYear = @SchoolYear, 
                                            AwdActName = @AwdActName, 
                                            AwdDate = @AwdDate, 
                                            AwdType = @AwdType, 
                                            AwdName = @AwdName, 
                                            Organizer = @Organizer, 
                                            ActVerify = @ActVerify, 
                                            {(vm.EditModel.Attachment != null ? "  Attachment = @Attachment, " : " ")}
                                            Memo = @Memo, 
                                            LastModifier = @LoginId, 
                                            LastModified = GETDATE() 
                                     WHERE AwdID = @AwdID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo EditDetailData(AwardMangViewModel vm, List<AwdDetailModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = $@"DELETE FROM AwardDetailMang WHERE AwdID = '{vm.EditModel.AwdID}'";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (ExecuteResult.isSuccess)
            {
                CommendText = $@"INSERT INTO AwardDetailMang
                                               (AwdID
                                               ,Name
                                               ,SNO
                                               ,Department
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified)
                                         VALUES
                                               ('{vm.EditModel.AwdID}'
                                               ,@Name
                                               ,@SNO
                                               ,@Department
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE())";

                ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);
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
            parameters.Add("@AwdID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM AwardMang WHERE AwdID = @AwdID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            if (ExecuteResult.isSuccess)
            {
                CommendText = $@"DELETE FROM AwardDetailMang WHERE AwdID = @AwdID ";

                ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);
            }

            return ExecuteResult;
        }

        /// <summary>
        /// Excel 取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<AwardMangExcelResultModel> GetExportResult(AwardMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@Organizer", model?.Organizer);
            parameters.Add("@AwdActName", model?.AwdActName);
            parameters.Add("@AwdType", model?.AwdType);
            parameters.Add("@AwdName", model?.AwdName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.AwdID, A.ClubID, B.ClubCName, A.SchoolYear, A.AwdDate, A.AwdActName, A.AwdType, A.AwdName, 
                                    A.Organizer, A.ActVerify, C.Text AS ActVerifyText, A.Created
                               FROM AwardMang A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                          LEFT JOIN Code C ON C.Code = A.ActVerify AND C.Type = 'ActVerify'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.AwdDate BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR A.ActVerify = @ActVerify)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR B.ClubCName LIKE '%' + @ClubCName + '%') 
AND (@Organizer IS NULL OR A.Organizer LIKE '%' + @Organizer + '%') 
AND (@AwdActName IS NULL OR A.AwdActName LIKE '%' + @AwdActName + '%') 
AND (@AwdType IS NULL OR A.AwdType LIKE '%' + @AwdType + '%') 
AND (@AwdName IS NULL OR A.AwdName LIKE '%' + @AwdName + '%') ";

            (DbExecuteInfo info, IEnumerable<AwardMangExcelResultModel> entitys) dbResult = DbaExecuteQuery<AwardMangExcelResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<AwardMangExcelResultModel>();
        }

        public List<SelectListItem> GetAllActVerify()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'ActVerify' AND Code <> '05'";

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

        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            for (int i = 108; i <= 130; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
        }
    }
}
