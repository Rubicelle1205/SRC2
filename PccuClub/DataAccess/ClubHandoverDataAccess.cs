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
using NPOI.SS.Formula.Functions;
using System.Runtime.ConstrainedExecution;

namespace WebPccuClub.DataAccess
{
    
    public class ClubHandoverDataAccess : BaseAccess
    {
        PublicFun PublicFun = new PublicFun();

		#region 交接申請

		public List<ClubHandoverCheckModel> GetCheckData(string LoginId, bool IsApply = false)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ClubID", LoginId);
            parameters.Add("@SchoolYear", PublicFun.GetNowSchoolYear());

            #endregion

            CommandText = $@"SELECT HoID, ClubID, SchoolYear, HandOverStatus
                               FROM HandOverMain
                              WHERE SchoolYear = @SchoolYear AND ClubID = @ClubID 
                                {(IsApply ? "" : " AND HandOverStatus<> '01' " )} ";


            (DbExecuteInfo info, IEnumerable<ClubHandoverCheckModel> entitys) dbResult = DbaExecuteQuery<ClubHandoverCheckModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubHandoverCheckModel>();
        }

        /// <summary> 交接申請 </summary>
        public DbExecuteInfo NewHandOver(string LoginId)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", LoginId);
            parameters.Add("@SchoolYear", PublicFun.GetNowSchoolYear());
            parameters.Add("@LoginId", LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO HandOverMain
                                               (ClubID, 
                                                SchoolYear, 
                                                HandOverStatus,
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@ClubID, 
                                                @SchoolYear, 
                                                '01',
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

		#endregion

		#region Doc 0101

		public DbExecuteInfo InsertDetail(string HoID, string HandOverClass, string DocType, UserInfo LoginUser, out DataTable dt)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HandOverClass", HandOverClass);
			parameters.Add("@DocType", DocType);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDocDetail
                                               (HoID, 
                                                HandOverClass, 
                                                DocType,
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         OUTPUT Inserted.HoDetailID
                                         VALUES
                                               (@HoID, 
                                                @HandOverClass, 
                                                @DocType,
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);
            dt = ds.Tables[0];

			return ExecuteResult;
		}

		public DbExecuteInfo Insert0101(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HoDetailID", HoDetailID);
			parameters.Add("@ClubID", vm.Handover0101Model.ClubID);
			parameters.Add("@ClubName", vm.Handover0101Model.ClubName);
			parameters.Add("@UserName", vm.Handover0101Model.UserName);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDoc01
                                               (HoID, 
                                                HoDetailID,
                                                ClubID, 
                                                ClubName, 
                                                UserName, 
                                                Agree,
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID,
                                                @ClubID, 
                                                @ClubName, 
                                                @UserName, 
                                                '01',
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}


		public ClubHandover0101ViewModel GetHandover0101Data(string HoID, UserInfo Login)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@HoID", HoID);
            parameters.Add("@ClubID", Login.LoginId);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.ClubID, A.ClubName, A.UserName
                               FROM HandOverDoc01 A
                                LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                              WHERE 1 = 1
                                AND A.HoID = @HoID 
                                AND B.ClubID = @ClubID";


			(DbExecuteInfo info, IEnumerable<ClubHandover0101ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0101ViewModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
		}

        #endregion

        #region File 01

        public DbExecuteInfo InsertFileDetail(string HoID, string HandOverClass, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", HoID);
            parameters.Add("@HandOverClass", HandOverClass);
            parameters.Add("@ActVerify", "01");
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO HandOverFileDetail
                                               (HoID, 
                                                HandOverClass, 
                                                ActVerify,
                                                DataEnable,
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         OUTPUT Inserted.HoDetailID
                                         VALUES
                                               (@HoID, 
                                                @HandOverClass, 
                                                @ActVerify,
                                                '01',
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);
            dt = ds.Tables[0];

            return ExecuteResult;
        }

        public DbExecuteInfo InsertFile01(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            List<ClubHandoverFileEditModel> dataList = vm.LstFileEditModel;
            #endregion 參數設定

            string CommendText = $@"INSERT INTO HandOverFile
                                               (HoID, 
                                                HoDetailID,
                                                FilePath, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               ('{HoID}', 
                                                '{HoDetailID}',
                                                @FilePath, 
                                                '{LoginUser.LoginId}', 
                                                GETDATE(), 
                                                '{LoginUser.LoginId}', 
                                                GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

            return ExecuteResult;
        }

        public ClubHandoverFileDetailModel GetFileDetail(string HoID, UserInfo Login, string HandOverClass)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@HoID", HoID);
            parameters.Add("@ClubID", Login.LoginId);
            parameters.Add("@HandOverClass", HandOverClass);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.HoDetailID
                               FROM HandOverFileDetail A
                                LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                              WHERE 1 = 1
                                AND A.HoID = @HoID 
                                AND B.ClubID = @ClubID 
                                AND A.HandOverClass = @HandOverClass 
                                AND A.DataEnable = '01' ";


            (DbExecuteInfo info, IEnumerable<ClubHandoverFileDetailModel> entitys) dbResult = DbaExecuteQuery<ClubHandoverFileDetailModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public DbExecuteInfo UpdateFileDetailToNoUse(string HoID, string HandOverClass, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", HoID);
            parameters.Add("@HandOverClass", HandOverClass);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE HandOverFileDetail 
                                       SET DataEnable = '02', LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE HoID = @HoID AND HandOverClass = @HandOverClass ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        #endregion


        #region 已上傳表單

        public List<ClubHandoverHistroyResultModel> GetHistorySearchResult(ClubHandoverHistroyConditionModel vm, UserInfo LoginUser)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();
			#region 參數設定

			parameters.Add("@ClubId", LoginUser.LoginId);
			parameters.Add("@SchoolYear", vm.SchoolYear == null ? PublicFun.GetNowSchoolYear() : vm.SchoolYear);

			#endregion

			CommandText = $@"SELECT A.HoID, A.HoDetailID, A.DocType, C.Text AS DocTypeText, B.SchoolYear, A.Created
                               FROM HandOverDocDetail A
                          LEFT JOIN HandOverMain B ON B.HoID = A.HoID
                          LEFT JOIN Code C ON C.Code = A.DocType AND C.Type = 'DocType'
                              WHERE B.SchoolYear = @SchoolYear";

			(DbExecuteInfo info, IEnumerable<ClubHandoverHistroyResultModel> entitys) dbResult = DbaExecuteQuery<ClubHandoverHistroyResultModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<ClubHandoverHistroyResultModel>();
		}

        #endregion

        #region 已上傳檔案

        public List<ClubHandoverFileResultModel> GetFileSearchResult(ClubHandoverFileConditionModel vm, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定

            parameters.Add("@ClubId", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.SchoolYear == null ? PublicFun.GetNowSchoolYear() : vm.SchoolYear);

            #endregion

            CommandText = $@"SELECT A.HoID, A.HoDetailID, A.HandOverClass, C.Text AS HandOverClassText, 
                                    A.ActVerify, D.Text AS ActVerifyText, A.VerifyMemo, B.SchoolYear, A.Created
                               FROM HandOverFileDetail A
                          LEFT JOIN HandOverMain B ON B.HoID = A.HoID
                          LEFT JOIN Code C ON C.Code = A.HandOverClass AND C.Type = 'HandOverClass'
                          LEFT JOIN Code D ON D.Code = A.ActVerify AND D.Type = 'ActVerify'
                              WHERE B.SchoolYear = @SchoolYear
                                AND A.DataEnable = '01' ";

            (DbExecuteInfo info, IEnumerable<ClubHandoverFileResultModel> entitys) dbResult = DbaExecuteQuery<ClubHandoverFileResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubHandoverFileResultModel>();
        }


        public List<ClubHandoverFileDataModel> GetAllFileData(string? DetailID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定

            parameters.Add("@HoDetailID", DetailID);

            #endregion

            CommandText = $@"SELECT FileID, FilePath
                               FROM HandOverFile
                              WHERE HoDetailID = @HoDetailID";

            (DbExecuteInfo info, IEnumerable<ClubHandoverFileDataModel> entitys) dbResult = DbaExecuteQuery<ClubHandoverFileDataModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubHandoverFileDataModel>();
        }

        #endregion






        #region else

        public DataTable GetHoID(string ClubID, string SchoolYear)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = $@"SELECT HoID
                              FROM HandOverMain
                             WHERE 1 = 1
                               AND ClubID = '{ClubID}' AND SchoolYear = '{SchoolYear}' AND HandOverStatus <> '04' ";


			(DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

			DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
			return ds.Tables[0];
		}

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



        #endregion






    }
}
