using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPccuClub.Models;
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;

namespace WebPccuClub.DataAccess
{
    public class ClubPersonMangDataAccess : BaseAccess
    {
        PublicFun PublicFun = new PublicFun();

        #region 幹部

        public List<ClubCadreMangResultModel> GetCadreSearchResult(ClubCadreMangConditionModel vm, UserInfo LoginUser)
		{
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
			#region 參數設定

			parameters.Add("@ClubId", LoginUser.LoginId);
			parameters.Add("@SchoolYear", vm.SchoolYear == null ? PublicFun.GetNowSchoolYear() : vm.SchoolYear);

            #endregion

            CommandText = $@"SELECT A.CadreID, A.ClubID, A.CadreName, A.SchoolYear, A.SNo, A.EMail, A.UserName, A.Sex, B.Text AS SexText,
                                    A.CellPhone, A.Department, A.SDuring, A.EDuring, A.Memo
                               FROM CadreMang A 
                          LEFT JOIN Code B ON B.Code = A.Sex AND B.Type = 'Sex'
                              WHERE A.ClubId = @ClubId 
                                AND A.SchoolYear = @SchoolYear";

            (DbExecuteInfo info, IEnumerable<ClubCadreMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubCadreMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubCadreMangResultModel>();
        }

		/// <summary> 取得編輯資料</summary>
		public ClubCadreMangEditModel GetCadreEditData(string Ser)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@CadreID", Ser);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT A.CadreID, A.ClubID, A.CadreName, A.SchoolYear, A.EMail, A.SNo, A.UserName, A.Sex, A.CellPhone, A.Department, A.SDuring, A.EDuring, A.Memo
                               FROM CadreMang A
                              WHERE 1 = 1
                                AND (CadreID = @CadreID) ";


			(DbExecuteInfo info, IEnumerable<ClubCadreMangEditModel> entitys) dbResult = DbaExecuteQuery<ClubCadreMangEditModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
		}

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo CadreMangInsertData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@CadreName", vm.CadreMangCreateModel.CadreName);
            parameters.Add("@SchoolYear", vm.CadreMangCreateModel.SchoolYear);
            parameters.Add("@SNo", vm.CadreMangCreateModel.SNo);
            parameters.Add("@EMail", vm.CadreMangCreateModel.EMail);
            parameters.Add("@UserName", vm.CadreMangCreateModel.UserName);
            parameters.Add("@Sex", vm.CadreMangCreateModel.Sex);
            parameters.Add("@CellPhone", vm.CadreMangCreateModel.CellPhone);
            parameters.Add("@Department", vm.CadreMangCreateModel.Department);
            parameters.Add("@SDuring", vm.CadreMangCreateModel.SDuring);
            parameters.Add("@EDuring", vm.CadreMangCreateModel.EDuring);
            parameters.Add("@Memo", vm.CadreMangCreateModel.Memo);
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
        public DbExecuteInfo CadreMangUpdateData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@CadreID", vm.CadreMangEditModel.CadreID);
            parameters.Add("@CadreName", vm.CadreMangEditModel.CadreName);
            parameters.Add("@SchoolYear", vm.CadreMangEditModel.SchoolYear);
            parameters.Add("@SNo", vm.CadreMangEditModel.SNo);
            parameters.Add("@EMail", vm.CadreMangEditModel.EMail);
            parameters.Add("@UserName", vm.CadreMangEditModel.UserName);
            parameters.Add("@Sex", vm.CadreMangEditModel.Sex);
            parameters.Add("@CellPhone", vm.CadreMangEditModel.CellPhone);
            parameters.Add("@Department", vm.CadreMangEditModel.Department);
            parameters.Add("@SDuring", vm.CadreMangEditModel.SDuring);
            parameters.Add("@EDuring", vm.CadreMangEditModel.EDuring);
            parameters.Add("@Memo", vm.CadreMangEditModel.Memo);
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

        public DbExecuteInfo CadreMangDeletetData(string ser)
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

        /// <summary>Excel 取得資料</summary>
        public List<ClubCadreMangExcelResultModel> GetCadreMangExportResult(ClubCadreMangConditionModel model, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定


            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ClubID", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT A.CadreID, A.ClubID, B.ClubCName AS ClubName, A.CadreName, A.SchoolYear, A.UserName, A.Department, A.SDuring, A.EDuring, A.Created
                               FROM CadreMang A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                              WHERE 1 = 1
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') ";

            (DbExecuteInfo info, IEnumerable<ClubCadreMangExcelResultModel> entitys) dbResult = DbaExecuteQuery<ClubCadreMangExcelResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubCadreMangExcelResultModel>();
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo CadreMangImportData(List<ClubCadreMangImportExcelResultModel> dataList, UserInfo LoginUser)
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

        #region 個人

        public DataTable ChkHasCadrePersonConData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.CadreMangPersonalConsentModel.SchoolYear);
            parameters.Add("@FilePath", vm.CadreMangPersonalConsentModel.PersonalConsent);
            parameters.Add("@LastModifier", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT FilePath
                               FROM PersonalConsent
                              WHERE ClubID = @ClubID AND SchoolYear = @SchoolYear AND CadreOrMember = '01' ";


            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }

        public DbExecuteInfo CadreMangUpdatePersonalConsentData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.CadreMangPersonalConsentModel.SchoolYear);
            parameters.Add("@FilePath", vm.CadreMangPersonalConsentModel.PersonalConsent);
            parameters.Add("@LastModifier", LoginUser.LoginId);

            #endregion 參數設定

            string CommendText = $@"UPDATE PersonalConsent
                                       SET FilePath = @FilePath, LastModifier = @LastModifier, LastModified = GETDATE()
                                     WHERE ClubID = @ClubID AND SchoolYear = @SchoolYear AND CadreOrMember = '01' ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo CadreMangInsertPersonalConsentData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.CadreMangPersonalConsentModel.SchoolYear);
            parameters.Add("@FilePath", vm.CadreMangPersonalConsentModel.PersonalConsent);
            parameters.Add("@LastModifier", LoginUser.LoginId);

            #endregion 參數設定

            string CommendText = $@"INSERT INTO PersonalConsent
                                               (ClubID 
                                                ,SchoolYear 
                                                ,CadreOrMember 
                                                ,FilePath 
                                                ,Creator 
                                                ,Created 
                                                ,LastModifier 
                                                ,LastModified 
                                                ,ModifiedReason)
                                         VALUES
                                               (@ClubID 
                                                ,@SchoolYear 
                                                ,'01'
                                                ,@FilePath 
                                                ,@LastModifier 
                                                ,GETDATE()
                                                ,@LastModifier 
                                                ,GETDATE()
                                                ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        #endregion

        #endregion

        #region 會員

        public List<ClubMemberMangResultModel> GetMemberSearchResult(ClubMemberMangConditionModel vm, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定

            parameters.Add("@ClubId", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.SchoolYear == null ? PublicFun.GetNowSchoolYear() : vm.SchoolYear);

            #endregion

            CommandText = $@"SELECT A.MemberID, A.ClubID, A.SchoolYear, A.SNo, A.EMail, A.UserName, A.Sex, B.Text AS SexText,
                                    A.CellPhone, A.Department, A.SDuring, A.EDuring, A.Memo
                               FROM MemberMang A 
                          LEFT JOIN Code B ON B.Code = A.Sex AND B.Type = 'Sex'
                              WHERE A.ClubId = @ClubId 
                                AND A.SchoolYear = @SchoolYear";

            (DbExecuteInfo info, IEnumerable<ClubMemberMangResultModel> entitys) dbResult = DbaExecuteQuery<ClubMemberMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubMemberMangResultModel>();
        }

        /// <summary> 取得編輯資料</summary>
        public ClubMemberMangEditModel GetMemberEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MemberID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.MemberID, A.ClubID, A.SchoolYear, A.EMail, A.SNo, A.UserName, A.Sex, A.CellPhone, A.Department, A.SDuring, A.EDuring, A.Memo
                               FROM MemberMang A
                              WHERE 1 = 1
                                AND (MemberID = @MemberID) ";


            (DbExecuteInfo info, IEnumerable<ClubMemberMangEditModel> entitys) dbResult = DbaExecuteQuery<ClubMemberMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo MemberMangInsertData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.MemberMangCreateModel.SchoolYear);
            parameters.Add("@SNo", vm.MemberMangCreateModel.SNo);
            parameters.Add("@EMail", vm.MemberMangCreateModel.EMail);
            parameters.Add("@UserName", vm.MemberMangCreateModel.UserName);
            parameters.Add("@Sex", vm.MemberMangCreateModel.Sex);
            parameters.Add("@CellPhone", vm.MemberMangCreateModel.CellPhone);
            parameters.Add("@Department", vm.MemberMangCreateModel.Department);
            parameters.Add("@SDuring", vm.MemberMangCreateModel.SDuring);
            parameters.Add("@EDuring", vm.MemberMangCreateModel.EDuring);
            parameters.Add("@Memo", vm.MemberMangCreateModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO MemberMang
                                               (ClubID
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
        public DbExecuteInfo MemberMangUpdateData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MemberID", vm.MemberMangEditModel.MemberID);
            parameters.Add("@SchoolYear", vm.MemberMangEditModel.SchoolYear);
            parameters.Add("@SNo", vm.MemberMangEditModel.SNo);
            parameters.Add("@EMail", vm.MemberMangEditModel.EMail);
            parameters.Add("@UserName", vm.MemberMangEditModel.UserName);
            parameters.Add("@Sex", vm.MemberMangEditModel.Sex);
            parameters.Add("@CellPhone", vm.MemberMangEditModel.CellPhone);
            parameters.Add("@Department", vm.MemberMangEditModel.Department);
            parameters.Add("@SDuring", vm.MemberMangEditModel.SDuring);
            parameters.Add("@EDuring", vm.MemberMangEditModel.EDuring);
            parameters.Add("@Memo", vm.MemberMangEditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE MemberMang 
                                       SET SchoolYear = @SchoolYear
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
                                     WHERE MemberID = @MemberID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo MemberMangDeletetData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MemberID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM MemberMang WHERE MemberID = @MemberID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>Excel 取得資料</summary>
        public List<ClubMemberMangExcelResultModel> GetMemberMangExportResult(ClubMemberMangConditionModel model, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定


            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ClubID", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT A.MemberID, A.ClubID, B.ClubCName AS ClubName, A.SchoolYear, A.UserName, A.Department, A.SDuring, A.EDuring, A.Created
                               FROM MemberMang A
                          LEFT JOIN ClubMang B ON B.ClubID = A.ClubID
                              WHERE 1 = 1
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') ";

            (DbExecuteInfo info, IEnumerable<ClubMemberMangExcelResultModel> entitys) dbResult = DbaExecuteQuery<ClubMemberMangExcelResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubMemberMangExcelResultModel>();
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo MemberMangImportData(List<ClubMemberMangImportExcelResultModel> dataList, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion 參數設定

            string CommendText = $@"INSERT INTO MemberMang
                                               (ClubID
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

        #region 個人

        public DataTable ChkHasMemberPersonConData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.MemberMangPersonalConsentModel.SchoolYear);
            parameters.Add("@FilePath", vm.MemberMangPersonalConsentModel.PersonalConsent);
            parameters.Add("@LastModifier", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT FilePath
                               FROM PersonalConsent
                              WHERE ClubID = @ClubID AND SchoolYear = @SchoolYear AND CadreOrMember = '02' ";


            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }

        public DbExecuteInfo MemberMangUpdatePersonalConsentData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.MemberMangPersonalConsentModel.SchoolYear);
            parameters.Add("@FilePath", vm.MemberMangPersonalConsentModel.PersonalConsent);
            parameters.Add("@LastModifier", LoginUser.LoginId);

            #endregion 參數設定

            string CommendText = $@"UPDATE PersonalConsent
                                       SET FilePath = @FilePath, LastModifier = @LastModifier, LastModified = GETDATE()
                                     WHERE ClubID = @ClubID AND SchoolYear = @SchoolYear AND CadreOrMember = '02' ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo MemberMangInsertPersonalConsentData(ClubPersonMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.MemberMangPersonalConsentModel.SchoolYear);
            parameters.Add("@FilePath", vm.MemberMangPersonalConsentModel.PersonalConsent);
            parameters.Add("@LastModifier", LoginUser.LoginId);

            #endregion 參數設定

            string CommendText = $@"INSERT INTO PersonalConsent
                                               (ClubID 
                                                ,SchoolYear 
                                                ,CadreOrMember 
                                                ,FilePath 
                                                ,Creator 
                                                ,Created 
                                                ,LastModifier 
                                                ,LastModified 
                                                ,ModifiedReason)
                                         VALUES
                                               (@ClubID 
                                                ,@SchoolYear 
                                                ,'02'
                                                ,@FilePath 
                                                ,@LastModifier 
                                                ,GETDATE()
                                                ,@LastModifier 
                                                ,GETDATE()
                                                ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        #endregion

        #endregion

        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            int NowSchoolYear = int.Parse(PublicFun.GetNowSchoolYear());

            for (int i = NowSchoolYear - 2; i <= NowSchoolYear + 2; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
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



        

    }
}
