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

		#region HO 01

		public DbExecuteInfo InsertDetail(string HoID, string HandOverClass, string DocNo, UserInfo LoginUser, out DataTable dt)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HandOverClass", HandOverClass);
			parameters.Add("@DocNo", DocNo);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDetail
                                               (HoID, 
                                                HandOverClass, 
                                                DocNo,
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         OUTPUT Inserted.HoDetailID
                                         VALUES
                                               (@HoID, 
                                                @HandOverClass, 
                                                @DocNo,
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


		public ClubHandover0101ViewModel GetHandover0101Data(string HoID)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@HoID", HoID);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT DocID, HoID, HoDetailID, ClubID, ClubName, UserName
                               FROM HandOverDoc01 
                              WHERE 1 = 1
                                AND HoID = @HoID ";


			(DbExecuteInfo info, IEnumerable<ClubHandover0101ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0101ViewModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
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
                               AND ClubID = '{ClubID}' AND SchoolYear = '{SchoolYear}'";


			(DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

			DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
			return ds.Tables[0];
		}


		#endregion






	}
}
