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
    
    public class CadreMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<CadreMangResultModel> GetSearchResult(CadreMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubName", model?.ClubName);
            parameters.Add("@CadreName", model?.CadreName);
            parameters.Add("@UserName", model?.UserName);
            parameters.Add("@Department", model?.Department);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

           
            #endregion

            CommandText = $@"SELECT A.CadreID, A.ClubID, B.ClubCName AS ClubName, A.CadreName, A.SchoolYear, A.UserName, A.Department, A.SDuring, A.EDuring, A.Created
                               FROM CadreMang A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubName IS NULL OR B.ClubCName LIKE '%' + @ClubName + '%') 
AND (@CadreName IS NULL OR A.CadreName LIKE '%' + @CadreName + '%') 
AND (@UserName IS NULL OR A.UserName LIKE '%' + @UserName + '%') 
AND (@Department IS NULL OR A.Department LIKE '%' + @Department + '%') ";


            (DbExecuteInfo info, IEnumerable<CadreMangResultModel> entitys) dbResult = DbaExecuteQuery<CadreMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<CadreMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CadreMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@CadreID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.CadreID, A.ClubID, B.ClubCName, A.CadreName, A.SchoolYear, A.EMail, A.SNo, A.UserName, A.Sex, A.CellPhone, A.Department, A.SDuring, A.EDuring, 
                                    A.Memo, A.Created, A.LastModified
                               FROM CadreMang A
                              LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                              WHERE 1 = 1
                                AND (CadreID = @CadreID) ";


            (DbExecuteInfo info, IEnumerable<CadreMangEditModel> entitys) dbResult = DbaExecuteQuery<CadreMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(CadreMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", vm.CreateModel.ClubID);
            parameters.Add("@CadreName", vm.CreateModel.CadreName);
            parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@SNo", vm.CreateModel.SNo);
            parameters.Add("@EMail", vm.CreateModel.EMail);
            parameters.Add("@UserName", vm.CreateModel.UserName);
            parameters.Add("@Sex", vm.CreateModel.Sex);
            parameters.Add("@CellPhone", vm.CreateModel.CellPhone);
            parameters.Add("@Department", vm.CreateModel.Department);
            parameters.Add("@SDuring", vm.CreateModel.SDuring);
            parameters.Add("@EDuring", vm.CreateModel.EDuring);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO CadreMang
                                               (ClubID
                                               ,CadreName
                                               ,SchoolYear
                                               ,SNo
                                                ,EMail
                                               ,UserName
                                               ,Sex
                                               ,CellPhone
                                               ,Department
                                               ,SDuring
                                               ,EDuring
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ClubID
                                               ,@CadreName
                                               ,@SchoolYear
                                               ,@SNo
                                                ,@EMail
                                               ,@UserName
                                               ,@Sex
                                               ,@CellPhone
                                               ,@Department
                                               ,@SDuring
                                               ,@EDuring
                                               ,@Memo
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE()
                                               ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(CadreMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@CadreID", vm.EditModel.CadreID);
            parameters.Add("@CadreName", vm.EditModel.CadreName);
            parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@SNo", vm.EditModel.SNo);
            parameters.Add("@EMail", vm.EditModel.EMail);
            parameters.Add("@UserName", vm.EditModel.UserName);
            parameters.Add("@Sex", vm.EditModel.Sex);
            parameters.Add("@CellPhone", vm.EditModel.CellPhone);
            parameters.Add("@Department", vm.EditModel.Department);
            parameters.Add("@SDuring", vm.EditModel.SDuring);
            parameters.Add("@EDuring", vm.EditModel.EDuring);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE CadreMang 
                                       SET CadreName = @CadreName
                                            ,SchoolYear = @SchoolYear
                                            ,SNo = @SNo
                                            ,EMail = @EMail
                                            ,UserName = @UserName
                                            ,Sex = @Sex
                                            ,CellPhone = @CellPhone
                                            ,Department = @Department
                                            ,SDuring = @SDuring
                                            ,EDuring = @EDuring
                                            ,Memo = @Memo
                                            ,LastModifier = @LoginId 
                                            ,LastModified = GETDATE() 
                                     WHERE CadreID = @CadreID";

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
            parameters.Add("@CadreID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM CadreMang WHERE CadreID = @CadreID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// Excel 取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<CadreMangResultModel> GetExportResult(CadreMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定


            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubName", model?.ClubName);
            parameters.Add("@CadreName", model?.CadreName);
            parameters.Add("@UserName", model?.UserName);
            parameters.Add("@Department", model?.Department);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);


            #endregion

            CommandText = $@"SELECT A.CadreID, A.ClubID, B.ClubCName AS ClubName, A.CadreName, A.SchoolYear, A.UserName, A.Department, A.SDuring, A.EDuring, A.Created
                               FROM CadreMang A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubName IS NULL OR B.ClubCName LIKE '%' + @ClubName + '%') 
AND (@CadreName IS NULL OR A.CadreName LIKE '%' + @CadreName + '%') 
AND (@UserName IS NULL OR A.UserName LIKE '%' + @UserName + '%') 
AND (@Department IS NULL OR A.Department LIKE '%' + @Department + '%') ";

            (DbExecuteInfo info, IEnumerable<CadreMangResultModel> entitys) dbResult = DbaExecuteQuery<CadreMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<CadreMangResultModel>();
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo ImportData(List<CadreMangImportExcelResultModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion 參數設定

            string CommendText = $@"INSERT INTO CadreMang
                                               (ClubID
                                               ,CadreName
                                               ,SchoolYear
                                               ,SNo
                                                ,EMail
                                               ,UserName
                                               ,Sex
                                               ,CellPhone
                                               ,Department
                                               ,SDuring
                                               ,EDuring
                                               ,Memo
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ClubID
                                               ,@CadreName
                                               ,@SchoolYear
                                               ,@SNo
                                                ,@EMail
                                               ,@UserName
                                               ,@Sex
                                               ,@CellPhone
                                               ,@Department
                                               ,@SDuring
                                               ,@EDuring
                                               ,@Memo
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE()
                                               ,'{LoginUser.LoginId}'
                                               ,GETDATE()
                                               ,NULL)";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }


        /// <summary> 查詢結果 </summary>

        public List<CadreMangPersonalConsentResultModel> GetPersonalConsentSearchResult(CadreMangPersonalConsentConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定


            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubName", model?.ClubName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);


            #endregion

            CommandText = $@"SELECT A.PersonalConID, A.ClubID, B.ClubCName AS ClubName, A.SchoolYear, A.CadreOrMember, A.FilePath, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM PersonalConsent A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.LastModified BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubName IS NULL OR B.ClubCName LIKE '%' + @ClubName + '%') ";


            (DbExecuteInfo info, IEnumerable<CadreMangPersonalConsentResultModel> entitys) dbResult = DbaExecuteQuery<CadreMangPersonalConsentResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<CadreMangPersonalConsentResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CadreMangPersonalConsentEditModel GetPersonalConsentEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@PersonalConID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.PersonalConID, A.ClubID, B.ClubCName AS ClubName, A.SchoolYear, A.CadreOrMember, A.FilePath, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM PersonalConsent A
                              LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                              WHERE 1 = 1
                                AND (PersonalConID = @PersonalConID) ";


            (DbExecuteInfo info, IEnumerable<CadreMangPersonalConsentEditModel> entitys) dbResult = DbaExecuteQuery<CadreMangPersonalConsentEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdatePersonalConsentData(CadreMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PersonalConID", vm.PersonalConsentEditModel.PersonalConID);
            parameters.Add("@FilePath", vm.PersonalConsentEditModel.FilePath);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE PersonalConsent 
                                       SET FilePath = @FilePath
                                            ,LastModifier = @LoginId 
                                            ,LastModified = GETDATE() 
                                     WHERE PersonalConID = @PersonalConID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeletetPersonalConsentData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@PersonalConID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM PersonalConsent WHERE PersonalConID = @PersonalConID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetAllSex()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'Sex'";

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

            CommandText = @"SELECT ClubID AS VALUE, ClubCName AS TEXT FROM ClubMang";

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
