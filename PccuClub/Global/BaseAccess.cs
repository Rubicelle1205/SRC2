﻿using Dapper;
using DataAccess;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using NPOI.SS.Formula.Eval;
using PccuClub.WebAuth;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using WebPccuClub.Entity;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using X.PagedList;

namespace WebPccuClub.Global
{
    public class BaseAccess : MsSqlDBAccess
    {
        PublicFun PublicFun = new PublicFun();

        public BaseAccess() : base()
        { }

        public BaseAccess(String ConnectionString) : base(ConnectionString)
        { }

        public StaticPagedList<T> GetPageData<T>(string SQL, DBAParameter Parameter, int? CurrentPage, int? PageSize)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DataSet ds = new DataSet();
            int thisCurrentPage = CurrentPage ?? 1;
            int thisPageSize = PageSize ?? 1;
            int TotalRow = 0;

            if (Parameter == null)
            { Parameter = new DBAParameter(); }

            Parameter.Add("Page", thisCurrentPage);
            Parameter.Add("PageSize", thisPageSize);

            ExecuteResult = DbaExecuteQuery(SQL, Parameter, ds, true, DBAccessException);

            if (ExecuteResult.isSuccess && !ds.IsNullOrEmpty())
            {
                TotalRow = int.Parse(ds.Tables[0].Rows[0]["TotalRowCount"].ToString());
                ds.Tables[0].Columns.Remove("TotalRowCount");

                IEnumerable<T> thisSource = ds.Tables[0].DataTableToEntities<T>();
                return new StaticPagedList<T>(thisSource, thisCurrentPage, thisPageSize, TotalRow);
            }
            else
            {
                List<T> empty = new List<T>();
                return new StaticPagedList<T>(empty, 1, 1, 0);
            }

        }

        public override DbExecuteInfo DbaExecuteQuery(string commandText, DBAParameter Parameters, DataSet ResultDS, bool exceptionHandle, DBAccessException ExHandle)
        {
            return base.DbaExecuteQuery(commandText, Parameters, ResultDS, exceptionHandle, ExHandle);
        }

        public override DbExecuteInfo DbaExecuteNonQuery(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle)
        {
            return base.DbaExecuteNonQuery(commandText, Parameters, exceptionHandle, ExHandle);
        }

        /// <summary> 錯誤處理 </summary>
        public void DBAccessException(DbExecuteInfo DAException)
        {
            // 直接回傳錯誤給上層處理好了...
            throw new Exception("[DAException]", DAException.SystemResult);
        }

		public virtual void WriteLog(string strMsg, UserInfo LoginUser = null, enumLogConst enumLog = enumLogConst.None)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            string CommandText = string.Empty;

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("Type", Enum.GetName(enumLog));

			if (null != LoginUser)
			{
                // 使用者帳號
                parameters.Add("Loginid", LoginUser.LoginId);
                // 使用者姓名
                parameters.Add("LoginName", LoginUser.UserName);
                // 角色姓名
                parameters.Add("RoleName", LoginUser.UserRole.Count > 0 ? LoginUser.UserRole[0].RoleName: "");
                // 登入IP
                parameters.Add("IP", LoginUser.IP);
            }
            
			// 建立時間
            parameters.Add("Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.sss"));
            parameters.Add("text", strMsg);
            #endregion 參數設定

            #region CommandText
            
			if (null != LoginUser)
			{
                CommandText = @"insert into Log_Record
					        (
								Type,
								LoginId,
								LoginName,
								Time,
								IP,
								text
					        )
					        values
					        (
                                @Type,
								@LoginId,
								@LoginName,
								@Time,
								@IP,
								@text
					        )";
            }
			else
			{
                CommandText = @"insert into Log_Record
					        (
								Type,
								Time,
								text
					        )
					        values
					        (
                                @Type,
								@Time,
								@text
					        )";
            }

            #endregion CommandText

            ExecuteResult = DbaExecuteNonQuery(CommandText, parameters, true, DBAccessException);

        }

        #region Method

        /// <summary> 取X流水號 </summary>
        public int GetXSequence()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();
            DBAParameter parameters = new DBAParameter();
            CommandText = $@"SELECT next value for　X_Sequence";

            (DbExecuteInfo info, IEnumerable<int> entitys) dbResult = DbaExecuteQuery<int>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.FirstOrDefault();

            return -1;
        }


        /// <summary>
        /// 排除特殊字元，並傳回調整後字串
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string TransSpecialChar(string Keyword)
        {
            string PaperKeyword = string.Empty;
            PaperKeyword = Keyword?.IndexOf("[") >= 0 ? Keyword.Replace("[", "[[]") : Keyword;

            return PaperKeyword;
        }


        public string QueryField(string StrTable, string StrField, string StrWhereSQL = "", string StrOrderbySQL = "")
        {
            DBAParameter parameters = new DBAParameter();
            DataSet ds = new DataSet();
            string strRtn = string.Empty;


            string CommandText = string.Format("SELECT {0} FROM {1} ", StrField, StrTable);

            if (!string.IsNullOrEmpty(StrWhereSQL))
                CommandText = CommandText + " WHERE " + StrWhereSQL;


            if (!string.IsNullOrEmpty(StrOrderbySQL))
                CommandText = CommandText + " Order By " + StrOrderbySQL;


            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRtn = ds.Tables[0].Rows[0].ToString();
                }
            }

            return strRtn;
        }

        #endregion

        #region 基本資料撈取

        public DataTable GetLastReportDate()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion

            CommandText = $@"SELECT ActivityReport FROM DateLineMang";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }

        public DataTable GetCancelDay()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            #endregion

            CommandText = $@"SELECT TripCancel FROM DateLineMang";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }

        public List<SelectListItem> GetStaticOrDynamic()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'StaticOrDynamic'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<SelectListItem> GetActInOrOut()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'ActInOrOut'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<SelectListItem> GetActType()
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

        public List<SelectListItem> GetActHoldType()
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

        public List<SelectListItem> GetUseITEquip()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'UseITEquip'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<SelectListItem> GetPassport()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'PassPort'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<SelectListItem> GetPlaceSource()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'PlaceSource'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<SelectListItem> GetAllHour()
		{
			List<SelectListItem> LstItem = new List<SelectListItem>();

			for (int i = 0; i <= 24; i++)
			{
				LstItem.Add(new SelectListItem() { Value = i.ToString().PadLeft(2, '0'), Text = i.ToString().PadLeft(2, '0') });
			}

			return LstItem;
		}

		public List<SelectListItem> GetSDGs()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT SDGID AS VALUE, ShortName AS TEXT FROM SDGsMang";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

        /// <summary>
        /// 撈取ActVerify Code
        /// </summary>
        /// <param name="type">1:顯示01~04；2 => 顯示01~05；3 => 顯示01~07; 4 => 顯示01~07，沒有05</param>
        /// <returns></returns>
        public List<SelectListItem> GetAllActVerify(string type = "")
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion
            //type = 1 => 顯示01~04，其他頁面
            //type = 2 => 顯示01~05，活動報備(新增)
            //type = 3 => 顯示01~07，活動報備(首頁、編輯)
            //type = 3 => 顯示01~07，沒有05，活動報備前台(首頁)
            switch (type)
            {
                case "1":
                    CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code 
									 WHERE Type = 'ActVerify' 
									   AND Code IN ('01', '02', '03', '04')";
                    break;
                case "2":
                    CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code 
									 WHERE Type = 'ActVerify' 
									   AND Code IN ('01', '02', '03', '04', '05')";
                    break;
                case "3":
                    CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code 
									 WHERE Type = 'ActVerify' 
									   AND Code IN ('01', '02', '03', '04', '05', '06', '07')";
                    break;
                case "4":
                    CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code 
									 WHERE Type = 'ActVerify' 
									   AND Code IN ('01', '02', '03', '04', '06', '07')";
                    break;
                default:
                    CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'ActVerify'";
                    break;
            }

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

        public List<SelectListItem> GetAllActName()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT A.ActID AS VALUE, A.ActName + '(' + B.ClubCName + ')' AS Text 
							  FROM ActDetail A 
					     LEFT JOIN ClubMang B ON B.ClubId = A.BrrowUnit
";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetOrderBy()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT 'ASC' AS VALUE, '正向排序' AS TEXT
							 UNION
							SELECT 'DESC' AS VALUE, '逆向排序' AS TEXT";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetSchoolYear(int type = 0)
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            int NowSchoolYear = int.Parse(PublicFun.GetNowSchoolYear());

			if (type == 0)  //取得本學年度資料 (-2 ~ +2)
			{
				for (int i = NowSchoolYear - 2; i <= NowSchoolYear + 2; i++)
				{
					LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
				}
			}
			else if (type == 1)  //取得本學年度資料 (-2)
			{
				for (int i = NowSchoolYear - 2; i <= NowSchoolYear; i++)
				{
					LstItem.Add(new SelectListItem() { Value = string.Format("{0}1", i), Text = string.Format("{0}1", i) });
					LstItem.Add(new SelectListItem() { Value = string.Format("{0}2", i), Text = string.Format("{0}2", i) });
				}
			}
			else if (type == 2)
			{
                for (int i = NowSchoolYear - 10; i <= NowSchoolYear; i++)
                {
                    LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
                }
            }

            return LstItem;
        }

        #region Rundown 資料


        /// <summary> 取得Rundown資料 </summary>
        public List<ActListMangEditRundownModel> GetEditRundownData(string Ser)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@ActId", Ser);

			#endregion


			CommandText = $@"SELECT * FROM (SELECT C.ActRundownID, C.PlaceSource, C.Date, C.STime, C.ETime, C.ActPlaceID, C.ActPlaceText AS PlaceText, C.RundownStatus, D.Text AS RundownStatusText
                                              FROM ActDetail A
                                         LEFT JOIN PlaceSchoolMang B ON B.PlaceID = A.PlaceID
                                         LEFT JOIN ActRundown C ON C.ActID = A.ActID AND C.ActDetailId = A.ActDetailId
                                         LEFT JOIN Code D ON D.Code = C.RundownStatus AND D.Type = 'RundownStatus'
                                             WHERE 1 = 1 AND A.ActID = @ActId 
                                     UNION
                                            SELECT C.ActRundownID, C.PlaceSource, C.Date, C.STime, C.ETime, C.ActPlaceID, C.ActPlaceText AS PlaceText, C.RundownStatus, D.Text AS RundownStatusText
                                              FROM ActDetail A
                                         LEFT JOIN PlaceSchoolMang B ON B.PlaceID = A.PlaceID
                                         LEFT JOIN ActRundownELSE C ON C.ActID = A.ActID AND C.ActDetailId = A.ActDetailId
                                         LEFT JOIN Code D ON D.Code = C.RundownStatus AND D.Type = 'RundownStatus'
                                             WHERE 1 = 1 AND A.ActID = @ActId 
                                            ) T ORDER BY PlaceSource";

			(DbExecuteInfo info, IEnumerable<ActListMangEditRundownModel> entitys) dbResult = DbaExecuteQuery<ActListMangEditRundownModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<ActListMangEditRundownModel>();
		}

		public List<ActListFilesModel> GetEditProposalData(string Ser)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@ActId", Ser);

			#endregion


			CommandText = $@"SELECT FileName, FilePath FROM ActProposal WHERE 1 = 1 AND ActID = @ActID";

			(DbExecuteInfo info, IEnumerable<ActListFilesModel> entitys) dbResult = DbaExecuteQuery<ActListFilesModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<ActListFilesModel>();
		}

		public List<ActListFilesModel> GetEditOutSideFileData(string Ser)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@ActId", Ser);

			#endregion


			CommandText = $@"SELECT FileName, FilePath FROM ActOutSideInfoFile WHERE 1 = 1 AND ActID = @ActID";

			(DbExecuteInfo info, IEnumerable<ActListFilesModel> entitys) dbResult = DbaExecuteQuery<ActListFilesModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<ActListFilesModel>();
		}


		public List<ActListMangPlaceUsedModel> GetPlaceUsedData(string Date)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();


			parameters.Add("@Date", Date);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT C.PlaceName, B.Date, B.STime, B.ETime, A.ActVerify
                               FROM ActMain A
                          LEFT JOIN ActRundown B ON B.ActID = A.ActID
						  LEFT JOIN PlaceSchoolMang C ON C.PlaceID = B.ActPlaceID
                              WHERE B.Date = @Date AND B.RundownStatus = '01'";
							  
			//AND A.ActVerify IN ('02', '04', '05')


			(DbExecuteInfo info, IEnumerable<ActListMangPlaceUsedModel> entitys) dbResult = DbaExecuteQuery<ActListMangPlaceUsedModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<ActListMangPlaceUsedModel>();
		}


		public List<SelectListItem> GetBuild()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT BuildID AS VALUE, BuildName AS TEXT FROM BuildMang";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}


		public List<SelectListItem> GetPlace(string PlaceSource, string Buildid)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@Buildid", Buildid);

			#endregion

			CommandText = @"SELECT PlaceID AS VALUE, PlaceName AS TEXT FROM PlaceSchoolMang WHERE Buildid = @Buildid";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<ActListMangPlaceDataModel> GetPlaceData(string PlaceSource, string PlaceId)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@PlaceId", PlaceId);

			#endregion

			CommandText = @"SELECT A.PlaceID, A.PlaceName, A.Capacity, A.PlaceEquip, A.PlaceStatus, B.Text AS PlaceStatusText, A.Memo, 
                                       A.Normal_STime, A.Normal_ETime, A.Holiday_STime, A.Holiday_ETime
                                  FROM PlaceSchoolMang A
                             LEFT JOIN Code B ON B.Code = A.PlaceStatus AND B.Type = 'PlaceStatus'
                                 WHERE A.PlaceId = @PlaceId";

			(DbExecuteInfo info, IEnumerable<ActListMangPlaceDataModel> entitys) dbResult = DbaExecuteQuery<ActListMangPlaceDataModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<ActListMangPlaceDataModel>();
		}


		public List<ActListMangTodayActModel1> GetTodayAct(string PlaceId, string Date)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定

			parameters.Add("@PlaceId", PlaceId);
			parameters.Add("@Date", Date);

			#endregion

			CommandText = $@"SELECT B.PlaceName, A.Date, A.STime, A.ETime, C.ActName, C.BrrowUnit, CASE WHEN D.ClubCName IS NULL THEN '學務處' ELSE D.ClubCName END BrrowClubName
                               FROM ActRundown A
                          LEFT JOIN PlaceSchoolMang B ON B.PlaceID = A.ActPlaceID
						  LEFT JOIN ActDetail C ON C.ActDetailId = A.ActDetailId
						  LEFT JOIN ClubMang D ON D.ClubId = C.BrrowUnit
                              WHERE A.Date = @Date AND A.ActPlaceID = @PlaceId ";


			(DbExecuteInfo info, IEnumerable<ActListMangTodayActModel1> entitys) dbResult = DbaExecuteQuery<ActListMangTodayActModel1>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<ActListMangTodayActModel1>();
		}


		public bool ChkPlaceSchoolCanUse(ActListMangViewModel vm)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			bool IsHoliday = false;
			string CommendText = string.Empty;

			string PlaceSource = vm.RundownModel.PlaceSource;
			string dayOfWeek = DateTime.Parse(vm.RundownModel.Date).ToString("dddd");

			if (dayOfWeek == "星期六" || dayOfWeek == "星期日")
			{
				IsHoliday = true;
			}

			#region 參數設定
			parameters.Add("@PlaceID", vm.RundownModel.PlaceID);
			parameters.Add("@Date", vm.RundownModel.Date);
			parameters.Add("@STime", vm.RundownModel.STime);
			parameters.Add("@ETime", vm.RundownModel.ETime);
			parameters.Add("@PlaceStatus", "01");   //可借用

			#endregion

			if (PlaceSource == "01")
			{
				CommendText = $@"SELECT * 
                                   FROM PlaceSchoolMang
                                  WHERE PlaceID = @PlaceID 
                                    AND PlaceStatus = @PlaceStatus 
{(IsHoliday ? "AND Holiday_STime <= @STime AND @ETime <= Holiday_ETime" : "AND Normal_STime <= @STime AND @ETime <= Normal_ETime")} 
";
			}

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ds.Tables[0].Rows.Count > 0;
		}



		//抓取該日的所有已核准時間
		public DataTable GetRundown(string? placeID, DateTime date)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@PlaceID", placeID);
			parameters.Add("@Date", date);

			#endregion

			CommandText = $@"SELECT B.ActRundownID, B.ActPlaceID, B.STime, B.ETime 
                               FROM ActMain A
					      LEFT JOIN ActRundown B ON B.ActId = A.ActId
                              WHERE B.ActPlaceID = @PlaceID 
                                AND B.[Date] = @Date 
                                AND B.RundownStatus = '01'
                                AND A.ActVerify IN ('02', '04', '05')
";


			DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

			return ds.Tables[0];
		}

        #endregion

        #endregion

        public DataTable GetClubLifeClass(UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@ClubId", LoginUser.LoginId);
            #endregion

            CommandText = $@"SELECT ClubId, LifeClass FROM ClubMang WHERE ClubId = @ClubId";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }

        public DataTable GetTeacherByLifeClass(string LifeClass)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@LifeClass", LifeClass);
            #endregion

            CommandText = $@"SELECT B.LoginId, B.EMail, A.LifeClass
							   FROM MatchLifeClass A
						  LEFT JOIN UserMain B ON B.LoginId = A.MatchID
							  WHERE A.LifeClass = @LifeClass";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }

		/// <summary>
		/// 取得全部社團組別
		/// </summary>
		/// <returns></returns>
        public List<SelectListItem> GetAllLifeClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'LifeClass'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public bool MemberInClub(string? sNo, string? clubID, string? schoolYear)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SNo", sNo);
            parameters.Add("@ClubID", clubID);
            parameters.Add("@SchoolYear", schoolYear);
            #endregion

            CommandText = $@"SELECT Count(1) AS RCount
							FROM MemberMang
							WHERE SNo = @SNo AND ClubID = @ClubID AND SchoolYear = @SchoolYear";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

			int RCount = 0;
			bool Bln = int.TryParse(ds.Tables[0].Rows[0]["RCount"].ToString(), out RCount);

            return RCount > 0;
        }


        public virtual DataTable GetPlaceName(string placeSource)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            if (placeSource == "02")
            {
                CommandText = $@"SELECT PlaceName FROM PlaceSchoolElseMang";
            }
            else if (placeSource == "03")
            {
                CommandText = $@"SELECT PlaceName FROM PlaceOutSideMang";
            }

            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
            return ds.Tables[0];
        }


		public virtual string GetActDetailID(string ActID)
		{
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ActID", ActID);

            #endregion

            CommandText = $@"SELECT ActDetailId FROM ActDetail WHERE ActID = @ActID";

            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);
         
			return ds.Tables[0].QueryFieldByDT("ActDetailId");
		}

		public virtual List<SelectListItem> GetEnable() {

            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'Enable'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public virtual DataTable GetFunctionEnable(string Url) {

            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Url", Url);
            #endregion

            CommandText = @"SELECT A.Enable, A.OpenDate, A.CloseDate 
                              FROM FrontOpeningMang A 
                         LEFT JOIN SystemMenu B ON B.MenuNode = A.MenuNode
                         LEFT JOIN SystemFun C ON C.FunId = B.FunId
                             WHERE C.Url = '/' + @Url ";

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            return ds.Tables[0];
        }
    }

	public enum enumLogConst 
	{
		None = 0,
		Run = 1, 
		Start = 2,
		End = 3,
		Information = 4,
		Warning = 5,
		Error = 6
    }

}
