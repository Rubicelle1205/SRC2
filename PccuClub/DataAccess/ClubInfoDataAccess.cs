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
using MathNet.Numerics.RootFinding;
using Microsoft.AspNetCore.Mvc;

namespace WebPccuClub.DataAccess
{

    public class ClubInfoDataAccess : BaseAccess
    {
		PublicFun PublicFun = new PublicFun();


		/// <summary>
		/// 取得編輯資料
		/// </summary>
		/// <param name="submitBtn"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public MyClubEditModel GetEditData(string ClubId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@ClubId", ClubId);

            #endregion

            CommandText = $@"SELECT A.ClubId, A.ClubCName, A.ClubEName, A.SchoolYear, A.LifeClass, C.Text AS LifeClassText, A.ClubClass, B.Text AS ClubClassText, A.Address, A.EMail, A.Tel, 
                                   A.Social1, A.Social2, A.Social3, A.LogoPath, A.ActImgPath, A.ShortInfo, A.Memo, A.Created, A.LastModified, D.RoleId
                               FROM ClubMang A
							   LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass'
							   LEFT JOIN Code C ON C.Code = A.LifeClass AND C.Type = 'LifeClass'
                               LEFT JOIN UserRole D ON D.LoginId = A.ClubId
                              WHERE 1 = 1
                               AND A.ClubId = @ClubId";


            (DbExecuteInfo info, IEnumerable<MyClubEditModel> entitys) dbResult = DbaExecuteQuery<MyClubEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary>
        /// Excel 取得資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MyClubExcelModel> GetExportResult(string ClubId)
		{
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();


			#region 參數設定

			parameters.Add("@ClubId", ClubId);

			#endregion

			CommandText = $@"SELECT A.ClubId, A.ClubCName, A.ClubEName, A.SchoolYear, A.LifeClass, C.Text AS LifeClassText, A.ClubClass, B.Text AS ClubClassText, A.Address, A.EMail, A.Tel, 
                                   A.Social1, A.Social2, A.Social3, A.LogoPath, A.ActImgPath, A.ShortInfo, A.Memo, A.Created, A.LastModified, D.RoleId
                               FROM ClubMang A
							   LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass'
							   LEFT JOIN Code C ON C.Code = A.LifeClass AND C.Type = 'LifeClass'
                               LEFT JOIN UserRole D ON D.LoginId = A.ClubId
                              WHERE 1 = 1
                               AND A.ClubId = @ClubId";

			(DbExecuteInfo info, IEnumerable<MyClubExcelModel> entitys) dbResult = DbaExecuteQuery<MyClubExcelModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<MyClubExcelModel>();
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo MyClubUpdateData(ClubInfoViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            string CommendText = string.Empty;

            #region 參數設定

            if (!string.IsNullOrEmpty(vm.MyClubEditModel.LogoPath))
                parameters.Add("@LogoPath", vm.MyClubEditModel.LogoPath);

            if (!string.IsNullOrEmpty(vm.MyClubEditModel.ActImgPath))
                parameters.Add("@ActImgPath", vm.MyClubEditModel.ActImgPath);
            
            parameters.Add("@ClubId", vm.MyClubEditModel.ClubId);
            parameters.Add("@Address", vm.MyClubEditModel.Address);
            parameters.Add("@Tel", vm.MyClubEditModel.Tel);
            parameters.Add("@EMail", vm.MyClubEditModel.EMail);
            parameters.Add("@Social1", vm.MyClubEditModel.Social1);
            parameters.Add("@Social2", vm.MyClubEditModel.Social2);
            parameters.Add("@Social3", vm.MyClubEditModel.Social3);
            parameters.Add("@ShortInfo", vm.MyClubEditModel.ShortInfo);
            parameters.Add("@LastModifier", LoginUser.LoginId);
            #endregion 參數設定

            CommendText = $@"UPDATE ClubMang 
                                           SET  Address = @Address, 
                                                Tel = @Tel, 
                                                EMail = @EMail, 
                                                Social1 = @Social1, 
                                                Social2 = @Social2, 
                                                Social3 = @Social3, 
                                                %LogoPath%
                                                %ActImgPath%
                                                ShortInfo = @ShortInfo,
                                                LastModifier = @LastModifier, 
                                                LastModified = GETDATE()
                                         WHERE ClubId = @ClubId ";


            if (!string.IsNullOrEmpty(vm.MyClubEditModel.LogoPath))
                CommendText = CommendText.Replace("%LogoPath%", "LogoPath = @LogoPath,");

            if (!string.IsNullOrEmpty(vm.MyClubEditModel.ActImgPath))
                CommendText = CommendText.Replace("%ActImgPath%", "ActImgPath = @ActImgPath,");

            CommendText = CommendText.Replace("%LogoPath%", "");
            CommendText = CommendText.Replace("%ActImgPath%", "");

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

		public List<ClubScheduleResultModel> GetScheduleSearchResult(ClubScheduleConditionModel vm, UserInfo LoginUser)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();
			#region 參數設定

			parameters.Add("@ClubId", LoginUser.LoginId);
			parameters.Add("@SchoolYear", vm.SchoolYear == null ? PublicFun.GetNowSchoolYear() : vm.SchoolYear);

			#endregion

			CommandText = $@"SELECT A.CScheID, A.ClubID, A.SchoolYear, A.CScheName, A.CScheDate, A.ActHoldType, B.Text AS ActHoldTypeText 
                               FROM ClubSchedule A
                          LEFT JOIN Code B ON B.Code = A.ActHoldType AND B.Type = 'ActHoldType'
                              WHERE A.ClubId = @ClubId 
                                AND A.SchoolYear = @SchoolYear";

			(DbExecuteInfo info, IEnumerable<ClubScheduleResultModel> entitys) dbResult = DbaExecuteQuery<ClubScheduleResultModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<ClubScheduleResultModel>();
		}

        public DbExecuteInfo ClubScheduleInserNewData(ClubInfoViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubID", LoginUser.LoginId);
            parameters.Add("@SchoolYear", vm.ClubScheduleCreateModel.SchoolYear);
            parameters.Add("@ActTypeID", vm.ClubScheduleCreateModel.ActType);
            parameters.Add("@CScheName", vm.ClubScheduleCreateModel.CScheName);
            parameters.Add("@CScheDate", vm.ClubScheduleCreateModel.CScheDate);
            parameters.Add("@Budget", vm.ClubScheduleCreateModel.Budget);
            parameters.Add("@BookingPlace", vm.ClubScheduleCreateModel.BookingPlace);
            parameters.Add("@ShortDesc", vm.ClubScheduleCreateModel.ShortDesc);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO ClubSchedule
                                               (ClubID 
                                               ,SchoolYear
                                               ,ActTypeID
                                               ,CScheName
                                               ,CScheDate
                                               ,Budget
                                               ,BookingPlace
                                               ,ShortDesc
                                               ,ActHoldType 
                                               ,Creator
                                               ,Created
                                               ,LastModifier
                                               ,LastModified
                                               ,ModifiedReason)
                                         VALUES
                                               (@ClubID 
                                               ,@SchoolYear 
                                               ,@ActTypeID
                                               ,@CScheName
                                               ,@CScheDate
                                               ,@Budget
                                               ,@BookingPlace
                                               ,'01'
                                               ,@ActHoldType 
                                               ,@LoginId
                                               ,GETDATE()
                                               ,@LoginId
                                               ,GETDATE()
                                               ,NULL)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary>
		/// 取得編輯資料
		/// </summary>
		/// <param name="submitBtn"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public ClubScheduleEditModel GetClubScheduleEditData(string CScheID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@CScheID", CScheID);

            #endregion

            CommandText = $@"SELECT A.CScheID, A.ClubID, A.SchoolYear, A.ActTypeID, B.ActTypeName, A.CScheName, A.CScheDate, A.Budget, A.BookingPlace, A.ShortDesc, 
                                    A.ActHoldType, A.Support, A.Participants, A.Satisfaction, A.Attachment
                               FROM ClubSchedule A
							   LEFT JOIN ActTypeMang B ON B.ActTypeID = A.ActTypeID
                              WHERE 1 = 1
                               AND A.CScheID = @CScheID";


            (DbExecuteInfo info, IEnumerable<ClubScheduleEditModel> entitys) dbResult = DbaExecuteQuery<ClubScheduleEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
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

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo ClubScheduleUpdateData(ClubInfoViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@CScheID", vm.ClubScheduleEditModel.CScheID);
            parameters.Add("@ActTypeID", vm.ClubScheduleEditModel.ActTypeID);
            parameters.Add("@CScheName", vm.ClubScheduleEditModel.CScheName);
            parameters.Add("@CScheDate", vm.ClubScheduleEditModel.CScheDate);
            parameters.Add("@BookingPlace", vm.ClubScheduleEditModel.BookingPlace);
            parameters.Add("@Budget", vm.ClubScheduleEditModel.Budget);
            parameters.Add("@ShortDesc", vm.ClubScheduleEditModel.ShortDesc);
            parameters.Add("@ActHoldType", vm.ClubScheduleEditModel.ActHoldType);
            parameters.Add("@Participants", vm.ClubScheduleEditModel.Participants);
            parameters.Add("@Support", vm.ClubScheduleEditModel.Support);
            parameters.Add("@Satisfaction", vm.ClubScheduleEditModel.Satisfaction);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE ClubSchedule 
                                       SET ActTypeID = @ActTypeID
                                           ,CScheName = @CScheName
                                           ,CScheDate = @CScheDate
                                           ,BookingPlace = @BookingPlace
                                           ,Budget = @Budget
                                           ,ShortDesc = @ShortDesc
                                           ,ActHoldType = @ActHoldType
                                           ,Participants = @Participants
                                           ,Support = @Support
                                           ,Satisfaction = @Satisfaction
                                           ,LastModifier = @LoginId 
                                           ,LastModified = GETDATE() 
                                     WHERE CScheID = @CScheID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

    }
}
