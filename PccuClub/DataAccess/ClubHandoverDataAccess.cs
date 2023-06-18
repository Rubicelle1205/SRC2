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
using MathNet.Numerics.RootFinding;

namespace WebPccuClub.DataAccess
{
    
    public class ClubHandoverDataAccess : BaseAccess
    {
        PublicFun PublicFun = new PublicFun();

		#region 交接申請

		public ClubHandoverCheckModel GetCheckData(string LoginId, bool IsApply = false)
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
                                {(IsApply ? " AND HandOverStatus = '01' " : " AND HandOverStatus <> '04' ")} ";

			(DbExecuteInfo info, IEnumerable<ClubHandoverCheckModel> entitys) dbResult = DbaExecuteQuery<ClubHandoverCheckModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

            return null;
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

		#region 交接作業

		public string GetNewLeader(string LoginId, string? schoolYear)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@SchoolYear", schoolYear);
			parameters.Add("@LoginId", LoginId);
			#endregion 參數設定

			string CommendText = $@"SELECT SNO
                                      FROM HandOverDoc03 
                                     WHERE SchoolYear = @SchoolYear AND ClubID = @LoginId";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            if (ExecuteResult.isSuccess)
            {
                return ds.Tables[0].QueryFieldByDT("SNO");
            }

			return null;
		}

		public DbExecuteInfo UpdateUserClub(string ClubID, string NewSNO)
		{
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			string CommendText = string.Empty;

			#region 參數設定
			parameters.Add("@ClubId", ClubID);
			parameters.Add("@FUserId", NewSNO);
			#endregion 參數設定

			CommendText = $@"DELETE ClubUser WHERE ClubId = @ClubId";

			ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

			if (ExecuteResult.isSuccess)
			{
				CommendText = $@"INSERT INTO ClubUser (ClubId, FUserID) VALUES (@ClubId, @FUserId) ";

				ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);
			}

			return ExecuteResult;
		}


		public DbExecuteInfo UpdateHandoverMain(ClubHandoverViewModel vm)
		{
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			string CommendText = string.Empty;

			#region 參數設定
			parameters.Add("@HoID", vm.CheckModel.HoID);
			#endregion 參數設定

			CommendText = $@"UPDATE HandOverMain SET HandOverStatus = '04' WHERE HoID = @HoID";

			ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

			return ExecuteResult;
		}

		#endregion

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

        public ClubHandoverDocCheckModel GetHandoverDocData(string HoID, string DocType)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@HoID", HoID);
            parameters.Add("@DocType", DocType);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.HoDetailID
                               FROM HandOverDocDetail A
                              WHERE 1 = 1
                                AND A.HoID = @HoID 
                                AND A.DocType = @DocType";


            (DbExecuteInfo info, IEnumerable<ClubHandoverDocCheckModel> entitys) dbResult = DbaExecuteQuery<ClubHandoverDocCheckModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        #region Doc 0101

        public DbExecuteInfo Insert0101(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HoDetailID", HoDetailID);
			parameters.Add("@SchoolYear", vm.Handover0101Model.SchoolYear);
			parameters.Add("@ClubID", vm.Handover0101Model.ClubID);
			parameters.Add("@ClubName", vm.Handover0101Model.ClubName);
			parameters.Add("@UserName", vm.Handover0101Model.UserName);
			parameters.Add("@Agree", vm.Handover0101Model.Agree);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDoc01
                                               (HoID, 
                                                HoDetailID,
                                                SchoolYear,
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
                                                @SchoolYear,
                                                @ClubID, 
                                                @ClubName, 
                                                @UserName, 
                                                @Agree,
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}

		public DbExecuteInfo Update0101(ClubHandoverViewModel vm, UserInfo LoginUser)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", vm.Handover0101Model.HoID);
			parameters.Add("@HoDetailID", vm.Handover0101Model.HoDetailID);
			parameters.Add("@ClubID", vm.Handover0101Model.ClubID);
            parameters.Add("@SchoolYear", vm.Handover0101Model.SchoolYear);
            parameters.Add("@ClubName", vm.Handover0101Model.ClubName);
			parameters.Add("@UserName", vm.Handover0101Model.UserName);
			parameters.Add("@Agree", vm.Handover0101Model.Agree);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定



			string CommendText = $@"UPDATE HandOverDoc01 
                                       SET SchoolYear = @SchoolYear,
                                           ClubID =@ClubID, 
                                           ClubName =@ClubName, 
                                           UserName =@UserName, 
                                           Agree=@Agree, 
                                           LastModifier =@LoginId, 
                                           LastModified= GETDATE()
                                     WHERE HoID =@HoID AND HoDetailID=@HoDetailID";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}

		public ClubHandover0101ViewModel GetHandover0101Data(string HoID, UserInfo Login)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@HoID", HoID);
            //parameters.Add("@ClubID", Login.LoginId);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.SchoolYear, A.ClubID, A.ClubName, A.UserName, A.Agree, C.Text AS AgreeText, A.Created, A.LastModified
                               FROM HandOverDoc01 A
                                LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                                LEFT JOIN Code C ON C.Code = A.Agree AND C.Type = 'Agree'
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


			(DbExecuteInfo info, IEnumerable<ClubHandover0101ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0101ViewModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
		}

        #endregion

        #region Doc 0102

        public DbExecuteInfo Insert0102(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", HoID);
            parameters.Add("@HoDetailID", HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0102Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0102Model.ClubID);
            parameters.Add("@ClubName", vm.Handover0102Model.ClubName);
            parameters.Add("@ElectionType", vm.Handover0102Model.ElectionType);
            parameters.Add("@ElectionDate", vm.Handover0102Model.ElectionDate);
            parameters.Add("@ElectionPlace", vm.Handover0102Model.ElectionPlace);
            parameters.Add("@TotleMan", vm.Handover0102Model.TotleMan);
            parameters.Add("@ShouldMan", vm.Handover0102Model.ShouldMan);
            parameters.Add("@RealMan", vm.Handover0102Model.RealMan);
            parameters.Add("@LeaveMan", vm.Handover0102Model.LeaveMan);
            parameters.Add("@AbsentMan", vm.Handover0102Model.AbsentMan);
            parameters.Add("@Teacher", vm.Handover0102Model.Teacher);
            parameters.Add("@Chairman", vm.Handover0102Model.Chairman);
            parameters.Add("@Recorder", vm.Handover0102Model.Recorder);
            parameters.Add("@NewLeader", vm.Handover0102Model.NewLeader);
            parameters.Add("@MeetingRecord", vm.Handover0102Model.MeetingRecord);
            parameters.Add("@MeetingSign", vm.Handover0102Model.MeetingSign);
            parameters.Add("@MeetingRecordName", vm.Handover0102Model.MeetingRecordName);
            parameters.Add("@MeetingSignName", vm.Handover0102Model.MeetingSignName);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO HandOverDoc02
                                               (HoID, 
                                                HoDetailID, 
                                                SchoolYear, 
                                                ClubID, 
                                                ClubName, 
                                                ElectionType, 
                                                ElectionDate, 
                                                ElectionPlace, 
                                                TotleMan,      
                                                ShouldMan, 
                                                RealMan, 
                                                LeaveMan, 
                                                AbsentMan, 
                                                Teacher, 
                                                Chairman, 
                                                Recorder, 
                                                NewLeader, 
                                                MeetingRecord, 
                                                MeetingSign, 
                                                MeetingRecordName, 
                                                MeetingSignName, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID, 
                                                @SchoolYear, 
                                                @ClubID, 
                                                @ClubName, 
                                                @ElectionType, 
                                                @ElectionDate, 
                                                @ElectionPlace, 
                                                @TotleMan,      
                                                @ShouldMan, 
                                                @RealMan, 
                                                @LeaveMan, 
                                                @AbsentMan, 
                                                @Teacher, 
                                                @Chairman, 
                                                @Recorder, 
                                                @NewLeader, 
                                                @MeetingRecord, 
                                                @MeetingSign, 
                                                @MeetingRecordName, 
                                                @MeetingSignName, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

		public DbExecuteInfo Update0102(ClubHandoverViewModel vm, UserInfo LoginUser)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", vm.Handover0102Model.HoID);
			parameters.Add("@HoDetailID", vm.Handover0102Model.HoDetailID);
			parameters.Add("@SchoolYear", vm.Handover0102Model.SchoolYear);
			parameters.Add("@ClubID", vm.Handover0102Model.ClubID);
			parameters.Add("@ClubName", vm.Handover0102Model.ClubName);
			parameters.Add("@ElectionType", vm.Handover0102Model.ElectionType);
			parameters.Add("@ElectionDate", vm.Handover0102Model.ElectionDate);
			parameters.Add("@ElectionPlace", vm.Handover0102Model.ElectionPlace);
			parameters.Add("@TotleMan", vm.Handover0102Model.TotleMan);
			parameters.Add("@ShouldMan", vm.Handover0102Model.ShouldMan);
			parameters.Add("@RealMan", vm.Handover0102Model.RealMan);
			parameters.Add("@LeaveMan", vm.Handover0102Model.LeaveMan);
			parameters.Add("@AbsentMan", vm.Handover0102Model.AbsentMan);
			parameters.Add("@Teacher", vm.Handover0102Model.Teacher);
			parameters.Add("@Chairman", vm.Handover0102Model.Chairman);
			parameters.Add("@Recorder", vm.Handover0102Model.Recorder);
			parameters.Add("@NewLeader", vm.Handover0102Model.NewLeader);
			parameters.Add("@MeetingRecord", vm.Handover0102Model.MeetingRecord);
			parameters.Add("@MeetingSign", vm.Handover0102Model.MeetingSign);

			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

            string CommendText = $@"UPDATE HandOverDoc02 
                                       SET SchoolYear =@SchoolYear,  
                                           ClubID =@ClubID,  
                                           ClubName =@ClubName,  
                                           ElectionType =@ElectionType,  
                                           ElectionDate =@ElectionDate,  
                                           ElectionPlace =@ElectionPlace,  
                                           TotleMan =@TotleMan,   
                                           ShouldMan =@ShouldMan,  
                                           RealMan =@RealMan,  
                                           LeaveMan =@LeaveMan,  
                                           AbsentMan =@AbsentMan,  
                                           Teacher =@Teacher,  
                                           Chairman =@Chairman,  
                                           Recorder =@Recorder,  
                                           NewLeader =@NewLeader,  
                                           MeetingRecord =@MeetingRecord,  
                                           MeetingSign =@MeetingSign,  
                                           LastModifier =@LoginId,
                                           LastModified=GETDATE()
                                     WHERE HoID =@HoID AND HoDetailID=@HoDetailID";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}

		public ClubHandover0102ViewModel GetHandover0102Data(string HoID, UserInfo Login)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@HoID", HoID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.SchoolYear, A.ClubID, A.ClubName, A.ElectionType, B.Text AS ElectionTypeText, A.ElectionDate, A.ElectionPlace, 
                                    A.TotleMan, A.ShouldMan, A.RealMan, A.LeaveMan, A.AbsentMan, A.Teacher, A.Chairman, A.Recorder, A.NewLeader, 
                                    A.MeetingRecord, A.MeetingSign, A.MeetingRecordName, A.MeetingSignName, A.Created, A.LastModified
                               FROM HandOverDoc02 A
                          LEFT JOIN Code B ON B.Code = A.ElectionType AND B.Type = 'ElectionType'
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


            (DbExecuteInfo info, IEnumerable<ClubHandover0102ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0102ViewModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        #endregion

        #region Doc 0103

        public DbExecuteInfo Insert0103(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", HoID);
            parameters.Add("@HoDetailID", HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0103Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0103Model.ClubID);
			parameters.Add("@ClubCName", vm.Handover0103Model.ClubCName);
			parameters.Add("@ClubEName", vm.Handover0103Model.ClubEName);
            parameters.Add("@ClubBuildID", vm.Handover0103Model.ClubBuildID);
            parameters.Add("@Location", vm.Handover0103Model.Location);
            parameters.Add("@Tel", vm.Handover0103Model.Tel);
            parameters.Add("@UserCName", vm.Handover0103Model.UserCName);
            parameters.Add("@UserEName", vm.Handover0103Model.UserEName);
            parameters.Add("@Sex", vm.Handover0103Model.Sex);
            parameters.Add("@IdentityType", vm.Handover0103Model.IdentityType);
            parameters.Add("@SNO", vm.Handover0103Model.SNO);
            parameters.Add("@CDepartment", vm.Handover0103Model.CDepartment);
            parameters.Add("@EDepartment", vm.Handover0103Model.EDepartment);
            parameters.Add("@UserMail", vm.Handover0103Model.UserMail);
            parameters.Add("@UserCellphone", vm.Handover0103Model.UserCellphone);
            parameters.Add("@Transcript", vm.Handover0103Model.Transcript);
            parameters.Add("@TranscriptName", vm.Handover0103Model.TranscriptName);
            parameters.Add("@GPA", vm.Handover0103Model.GPA);
            parameters.Add("@Behavior", vm.Handover0103Model.Behavior);
            parameters.Add("@Score60", vm.Handover0103Model.Score60);
            parameters.Add("@Score75", vm.Handover0103Model.Score75);
            parameters.Add("@IsMember", vm.Handover0103Model.IsMember);
            parameters.Add("@NoFire", vm.Handover0103Model.NoFire);
            parameters.Add("@NoReElection", vm.Handover0103Model.NoReElection);
            parameters.Add("@NoTwoPosition", vm.Handover0103Model.NoTwoPosition);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO HandOverDoc03
                                               (HoID, 
                                                HoDetailID, 
                                                SchoolYear, 
                                                ClubID, 
                                                ClubCName, 
                                                ClubEName, 
                                                ClubBuildID, 
                                                Location, 
                                                Tel, 
                                                UserCName, 
                                                UserEName, 
                                                Sex, 
                                                IdentityType, 
                                                SNO, 
                                                CDepartment, 
                                                EDepartment, 
                                                UserMail, 
                                                UserCellphone, 
                                                Transcript, 
                                                TranscriptName, 
                                                GPA, 
                                                Behavior, 
                                                Score60, 
                                                Score75, 
                                                IsMember, 
                                                NoFire, 
                                                NoReElection, 
                                                NoTwoPosition, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID, 
                                                @SchoolYear, 
                                                @ClubID, 
                                                @ClubCName, 
                                                @ClubEName, 
                                                @ClubBuildID, 
                                                @Location, 
                                                @Tel, 
                                                @UserCName, 
                                                @UserEName, 
                                                @Sex, 
                                                @IdentityType, 
                                                @SNO, 
                                                @CDepartment, 
                                                @EDepartment, 
                                                @UserMail, 
                                                @UserCellphone, 
                                                @Transcript, 
                                                @TranscriptName, 
                                                @GPA, 
                                                @Behavior, 
                                                @Score60, 
                                                @Score75, 
                                                @IsMember, 
                                                @NoFire, 
                                                @NoReElection, 
                                                @NoTwoPosition, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo Update0103(ClubHandoverViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", vm.Handover0103Model.HoID);
            parameters.Add("@HoDetailID", vm.Handover0103Model.HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0103Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0103Model.ClubID);
            parameters.Add("@ClubCName", vm.Handover0103Model.ClubCName);
            parameters.Add("@ClubEName", vm.Handover0103Model.ClubEName);
            parameters.Add("@ClubBuildID", vm.Handover0103Model.ClubBuildID);
            parameters.Add("@Location", vm.Handover0103Model.Location);
            parameters.Add("@Tel", vm.Handover0103Model.Tel);
            parameters.Add("@UserCName", vm.Handover0103Model.UserCName);
            parameters.Add("@UserEName", vm.Handover0103Model.UserEName);
            parameters.Add("@Sex", vm.Handover0103Model.Sex);
            parameters.Add("@IdentityType", vm.Handover0103Model.IdentityType);
            parameters.Add("@SNO", vm.Handover0103Model.SNO);
            parameters.Add("@CDepartment", vm.Handover0103Model.CDepartment);
            parameters.Add("@EDepartment", vm.Handover0103Model.EDepartment);
            parameters.Add("@UserMail", vm.Handover0103Model.UserMail);
            parameters.Add("@UserCellphone", vm.Handover0103Model.UserCellphone);
            parameters.Add("@Transcript", vm.Handover0103Model.Transcript);
            parameters.Add("@TranscriptName", vm.Handover0103Model.TranscriptName);
            parameters.Add("@GPA", vm.Handover0103Model.GPA);
            parameters.Add("@Behavior", vm.Handover0103Model.Behavior);
            parameters.Add("@Score60", vm.Handover0103Model.Score60);
            parameters.Add("@Score75", vm.Handover0103Model.Score75);
            parameters.Add("@IsMember", vm.Handover0103Model.IsMember);
            parameters.Add("@NoFire", vm.Handover0103Model.NoFire);
            parameters.Add("@NoReElection", vm.Handover0103Model.NoReElection);
            parameters.Add("@NoTwoPosition", vm.Handover0103Model.NoTwoPosition);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE HandOverDoc03 
                                       SET SchoolYear =@SchoolYear,  
                                           ClubID =@ClubID,  
                                           ClubCName =@ClubCName,  
                                           ClubEName =@ClubEName,  
                                           ClubBuildID =@ClubBuildID,  
                                           Location =@Location,  
                                           Tel =@Tel,  
                                           UserCName =@UserCName,  
                                           UserEName =@UserEName,  
                                           Sex =@Sex,  
                                           IdentityType =@IdentityType,  
                                           SNO =@SNO,  
                                           CDepartment =@CDepartment,  
                                           EDepartment =@EDepartment,  
                                           UserMail =@UserMail,  
                                           UserCellphone =@UserCellphone,  
                                           Transcript =@Transcript,  
                                           TranscriptName =@TranscriptName,  
                                           GPA =@GPA,  
                                           Behavior =@Behavior,  
                                           Score60 =@Score60,  
                                           Score75 =@Score75,  
                                           IsMember =@IsMember,  
                                           NoFire =@NoFire,  
                                           NoReElection =@NoReElection,  
                                           NoTwoPosition =@NoTwoPosition, 
                                           LastModifier =@LoginId,
                                           LastModified=GETDATE()
                                     WHERE HoID =@HoID AND HoDetailID=@HoDetailID";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

        public ClubHandover0103ViewModel GetHandover0103Data(string HoID, UserInfo Login)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@HoID", HoID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.SchoolYear, A.ClubID, A.ClubCName, A.ClubEName, 
                                    A.ClubBuildID, B.Text AS ClubBuildIDText, A.Location, A.Tel, A.UserCName, A.UserEName, 
                                    A.Sex, C.Text AS SexText, A.IdentityType, D.Text AS IdentityTypeText,
                                    A.SNO, A.CDepartment, A.EDepartment, A.UserMail, A.UserCellphone, A.Transcript, A.TranscriptName, 
		                            A.GPA, A.Behavior, A.Score60, A.Score75, A.IsMember, A.NoFire, A.NoReElection, A.NoTwoPosition,
                                    E.Text AS Score60Text, F.Text AS Score75Text, G.Text AS IsMemberText, 
                                    H.Text AS NoFireText, I.Text AS NoReElectionText, J.Text AS NoTwoPositionText, A.Created, A.LastModified
                               FROM HandOverDoc03 A
                          LEFT JOIN Code B ON B.Code = A.ClubBuildID AND B.Type = 'ClubBuild'
                          LEFT JOIN Code C ON C.Code = A.Sex AND C.Type = 'Sex'
                          LEFT JOIN Code D ON C.Code = A.IdentityType AND D.Type = 'IdentityType'
                          LEFT JOIN Code E ON E.Code = A.Score60 AND E.Type = 'Conform'
                          LEFT JOIN Code F ON F.Code = A.Score75 AND F.Type = 'Conform'
                          LEFT JOIN Code G ON G.Code = A.IsMember AND G.Type = 'Conform'
                          LEFT JOIN Code H ON H.Code = A.NoFire AND H.Type = 'Conform'
                          LEFT JOIN Code I ON I.Code = A.NoReElection AND I.Type = 'Conform'
                          LEFT JOIN Code J ON J.Code = A.NoTwoPosition AND J.Type = 'Conform'
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


            (DbExecuteInfo info, IEnumerable<ClubHandover0103ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0103ViewModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

		#endregion

		#region Doc 0204

		public DbExecuteInfo Insert0204(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HoDetailID", HoDetailID);
			parameters.Add("@SchoolYear", vm.Handover0204Model.SchoolYear);
			parameters.Add("@ClubID", vm.Handover0204Model.ClubID);
			parameters.Add("@ClubName", vm.Handover0204Model.ClubName);
			parameters.Add("@NameOfClub", vm.Handover0204Model.NameOfClub);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDoc04
                                               (HoID, 
                                                HoDetailID,
                                                SchoolYear, 
                                                ClubID, 
                                                ClubName, 
                                                NameOfClub, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID,
                                                @SchoolYear, 
                                                @ClubID, 
                                                @ClubName, 
                                                @NameOfClub, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}

        public DbExecuteInfo Update0204(ClubHandoverViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", vm.Handover0204Model.HoID);
            parameters.Add("@HoDetailID", vm.Handover0204Model.HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0204Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0204Model.ClubID);
            parameters.Add("@ClubName", vm.Handover0204Model.ClubName);
            parameters.Add("@NameOfClub", vm.Handover0204Model.NameOfClub);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定


            string CommendText = $@"UPDATE HandOverDoc04
                                       SET SchoolYear=@SchoolYear, 
                                           ClubID=@ClubID, 
                                           ClubName=@ClubName, 
                                           NameOfClub=@NameOfClub, 
                                           LastModifier=@LoginId, 
                                           LastModified=GETDATE()
                                     WHERE HoID =@HoID AND HoDetailID=@HoDetailID";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

        public ClubHandover0204ViewModel GetHandover0204Data(string HoID, UserInfo Login)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@HoID", HoID);
			//parameters.Add("@ClubID", Login.LoginId);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.ClubID, A.ClubName, A.SchoolYear, A.NameOfClub, A.Created, A.LastModified
                               FROM HandOverDoc04 A
                                LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


			(DbExecuteInfo info, IEnumerable<ClubHandover0204ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0204ViewModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
		}

		#endregion

		#region Doc 0205

		public DbExecuteInfo Insert0205(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HoDetailID", HoDetailID);
			parameters.Add("@SchoolYear", vm.Handover0205Model.SchoolYear);
			parameters.Add("@ClubID", vm.Handover0205Model.ClubID);
			parameters.Add("@ClubName", vm.Handover0205Model.ClubName);
			parameters.Add("@ActSysAcc", vm.Handover0205Model.ActSysAcc);
			parameters.Add("@ActSysPwd", vm.Handover0205Model.ActSysPwd);
			parameters.Add("@ClubWebAcc", vm.Handover0205Model.ClubWebAcc);
			parameters.Add("@ClubWebPwd", vm.Handover0205Model.ClubWebPwd);
			parameters.Add("@RPageAcc", vm.Handover0205Model.RPageAcc);
			parameters.Add("@RPagePwd", vm.Handover0205Model.RPagePwd);
			parameters.Add("@PassportAcc", vm.Handover0205Model.PassportAcc);
			parameters.Add("@PassportPwd", vm.Handover0205Model.PassportPwd);
			parameters.Add("@OneDriveAcc", vm.Handover0205Model.OneDriveAcc);
			parameters.Add("@OneDrivePwd", vm.Handover0205Model.OneDrivePwd);
			parameters.Add("@HasSchoolProperty", vm.Handover0205Model.HasSchoolProperty);
			parameters.Add("@UseRecord", vm.Handover0205Model.UseRecord);
			parameters.Add("@ClubProperty", vm.Handover0205Model.ClubProperty);
			parameters.Add("@SchoolProperty", vm.Handover0205Model.SchoolProperty);
			parameters.Add("@UseRecordName", vm.Handover0205Model.UseRecordName);
			parameters.Add("@ClubPropertyName", vm.Handover0205Model.ClubPropertyName);
			parameters.Add("@SchoolPropertyName", vm.Handover0205Model.SchoolPropertyName);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDoc05
                                               (HoID, 
                                                HoDetailID,
                                                SChoolYear, 
                                                ClubID, 
                                                ClubName, 
                                                ActSysAcc, 
                                                ActSysPwd, 
                                                ClubWebAcc, 
                                                ClubWebPwd, 
                                                RPageAcc, 
                                                RPagePwd, 
                                                PassportAcc, 
                                                PassportPwd, 
                                                OneDriveAcc, 
                                                OneDrivePwd, 
                                                HasSchoolProperty, 
                                                UseRecord, 
                                                ClubProperty, 
                                                SchoolProperty, 
                                                UseRecordName, 
                                                ClubPropertyName, 
                                                SchoolPropertyName, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID,
                                                @SChoolYear, 
                                                @ClubID, 
                                                @ClubName, 
                                                @ActSysAcc, 
                                                @ActSysPwd, 
                                                @ClubWebAcc, 
                                                @ClubWebPwd, 
                                                @RPageAcc, 
                                                @RPagePwd, 
                                                @PassportAcc, 
                                                @PassportPwd, 
                                                @OneDriveAcc, 
                                                @OneDrivePwd, 
                                                @HasSchoolProperty, 
                                                @UseRecord, 
                                                @ClubProperty, 
                                                @SchoolProperty, 
                                                @UseRecordName, 
                                                @ClubPropertyName, 
                                                @SchoolPropertyName,
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}

        public DbExecuteInfo Update0205(ClubHandoverViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", vm.Handover0205Model.HoID);
            parameters.Add("@HoDetailID", vm.Handover0205Model.HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0205Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0205Model.ClubID);
            parameters.Add("@ClubName", vm.Handover0205Model.ClubName);
            parameters.Add("@ActSysAcc", vm.Handover0205Model.ActSysAcc);
            parameters.Add("@ActSysPwd", vm.Handover0205Model.ActSysPwd);
            parameters.Add("@ClubWebAcc", vm.Handover0205Model.ClubWebAcc);
            parameters.Add("@ClubWebPwd", vm.Handover0205Model.ClubWebPwd);
            parameters.Add("@RPageAcc", vm.Handover0205Model.RPageAcc);
            parameters.Add("@RPagePwd", vm.Handover0205Model.RPagePwd);
            parameters.Add("@PassportAcc", vm.Handover0205Model.PassportAcc);
            parameters.Add("@PassportPwd", vm.Handover0205Model.PassportPwd);
            parameters.Add("@OneDriveAcc", vm.Handover0205Model.OneDriveAcc);
            parameters.Add("@OneDrivePwd", vm.Handover0205Model.OneDrivePwd);
            parameters.Add("@HasSchoolProperty", vm.Handover0205Model.HasSchoolProperty);
            parameters.Add("@UseRecord", vm.Handover0205Model.UseRecord);
            parameters.Add("@ClubProperty", vm.Handover0205Model.ClubProperty);
            parameters.Add("@SchoolProperty", vm.Handover0205Model.SchoolProperty);
            parameters.Add("@UseRecordName", vm.Handover0205Model.UseRecordName);
            parameters.Add("@ClubPropertyName", vm.Handover0205Model.ClubPropertyName);
            parameters.Add("@SchoolPropertyName", vm.Handover0205Model.SchoolPropertyName);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定


            string CommendText = $@"UPDATE HandOverDoc05 
                                       SET SChoolYear=@SChoolYear, 
                                           ClubID=@ClubID, 
                                           ClubName=@ClubName, 
                                           ActSysAcc=@ActSysAcc, 
                                           ActSysPwd=@ActSysPwd, 
                                           ClubWebAcc=@ClubWebAcc, 
                                           ClubWebPwd=@ClubWebPwd, 
                                           RPageAcc=@RPageAcc, 
                                           RPagePwd=@RPagePwd, 
                                           PassportAcc=@PassportAcc, 
                                           PassportPwd=@PassportPwd, 
                                           OneDriveAcc=@OneDriveAcc, 
                                           OneDrivePwd=@OneDrivePwd, 
                                           HasSchoolProperty=@HasSchoolProperty, 
                                           UseRecord=@UseRecord, 
                                           ClubProperty=@ClubProperty, 
                                           SchoolProperty=@SchoolProperty, 
                                           UseRecordName=@UseRecordName, 
                                           ClubPropertyName=@ClubPropertyName, 
                                           SchoolPropertyName=@SchoolPropertyName, 
                                           LastModifier=@LoginId,
                                           LastModified=GETDATE()
                                     WHERE HoID =@HoID AND HoDetailID=@HoDetailID";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

        public ClubHandover0205ViewModel GetHandover0205Data(string HoID, UserInfo Login)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@HoID", HoID);
			//parameters.Add("@ClubID", Login.LoginId);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.SchoolYear, A.ClubID, A.ClubName, 
                                    A.ActSysAcc, A.ActSysPwd, A.ClubWebAcc, A.ClubWebPwd, A.RPageAcc, A.RPagePwd, 
                                    A.PassportAcc, A.PassportPwd, A.OneDriveAcc, A.OneDrivePwd, A.HasSchoolProperty, C.Text AS HasSchoolPropertyText, 
                                    A.UseRecordName, A.ClubPropertyName, A.SchoolPropertyName, A.UseRecord, A.ClubProperty, A.SchoolProperty, A.Created, A.LastModified
                               FROM HandOverDoc05 A
                          LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                          LEFT JOIN Code C ON C.Code = A.HasSchoolProperty AND C.Type = 'YesOrNo'
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


			(DbExecuteInfo info, IEnumerable<ClubHandover0205ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0205ViewModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
		}

		#endregion

		#region Doc 0206

		public DbExecuteInfo Insert0206(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HoDetailID", HoDetailID);
			parameters.Add("@SchoolYear", vm.Handover0206Model.SchoolYear);
			parameters.Add("@ClubID", vm.Handover0206Model.ClubID);
			parameters.Add("@ClubName", vm.Handover0206Model.ClubName);
			parameters.Add("@Sheet", vm.Handover0206Model.Sheet);
			parameters.Add("@InnerFile", vm.Handover0206Model.InnerFile);
			parameters.Add("@SheetName", vm.Handover0206Model.SheetName);
			parameters.Add("@InnerFileName", vm.Handover0206Model.InnerFileName);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDoc06
                                               (HoID, 
                                                HoDetailID,
                                                SChoolYear, 
                                                ClubID, 
                                                ClubName, 
                                                Sheet, 
                                                InnerFile, 
                                                SheetName, 
                                                InnerFileName, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID,
                                                @SChoolYear, 
                                                @ClubID, 
                                                @ClubName, 
                                                @Sheet, 
                                                @InnerFile, 
                                                @SheetName, 
                                                @InnerFileName, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}

        public DbExecuteInfo Update0206(ClubHandoverViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", vm.Handover0206Model.HoID);
            parameters.Add("@HoDetailID", vm.Handover0206Model.HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0206Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0206Model.ClubID);
            parameters.Add("@ClubName", vm.Handover0206Model.ClubName);
            parameters.Add("@Sheet", vm.Handover0206Model.Sheet);
            parameters.Add("@InnerFile", vm.Handover0206Model.InnerFile);
            parameters.Add("@SheetName", vm.Handover0206Model.SheetName);
            parameters.Add("@InnerFileName", vm.Handover0206Model.InnerFileName);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE HandOverDoc06 
                                       SET SChoolYear=@SChoolYear, 
                                           ClubID=@ClubID, 
                                           ClubName=@ClubName, 
                                           Sheet=@Sheet, 
                                           InnerFile=@InnerFile, 
                                           SheetName=@SheetName, 
                                           InnerFileName=@InnerFileName, 
                                           LastModifier=@LoginId,  
                                           LastModified= GETDATE()
                                     WHERE HoID =@HoID AND HoDetailID=@HoDetailID";



            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

        public ClubHandover0206ViewModel GetHandover0206Data(string HoID, UserInfo Login)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@HoID", HoID);
			//parameters.Add("@ClubID", Login.LoginId);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.SchoolYear, A.ClubID, A.ClubName, A.Sheet, A.InnerFile, A.SheetName, A.InnerFileName, A.Created, A.LastModified
                               FROM HandOverDoc06 A
                          LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


			(DbExecuteInfo info, IEnumerable<ClubHandover0206ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0206ViewModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
		}

		#endregion

		#region Doc 0307

		public DbExecuteInfo Insert0307(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HoDetailID", HoDetailID);
			parameters.Add("@SchoolYear", vm.Handover0307Model.SchoolYear);
			parameters.Add("@ClubID", vm.Handover0307Model.ClubID);
			parameters.Add("@ClubName", vm.Handover0307Model.ClubName);
			parameters.Add("@Teacher1", vm.Handover0307Model.Teacher1);
			parameters.Add("@Sex1", vm.Handover0307Model.Sex1);
			parameters.Add("@Unit1", vm.Handover0307Model.Unit1);
			parameters.Add("@Position1", vm.Handover0307Model.Position1);
			parameters.Add("@Mail1", vm.Handover0307Model.Mail1);
			parameters.Add("@CellPhone1", vm.Handover0307Model.CellPhone1);
			parameters.Add("@Teacher2", vm.Handover0307Model.Teacher2);
			parameters.Add("@Sex2", vm.Handover0307Model.Sex2);
			parameters.Add("@Unit2", vm.Handover0307Model.Unit2);
			parameters.Add("@Position2", vm.Handover0307Model.Position2);
			parameters.Add("@Mail2", vm.Handover0307Model.Mail2);
			parameters.Add("@CellPhone2", vm.Handover0307Model.CellPhone2);
			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDoc07
                                               (HoID, 
                                                HoDetailID,
                                                SChoolYear, 
                                                ClubID, 
                                                ClubName, 
                                                Teacher1, 
                                                Sex1, 
                                                Unit1, 
                                                Position1, 
                                                Mail1, 
                                                CellPhone1, 
                                                Teacher2, 
                                                Sex2, 
                                                Unit2, 
                                                Position2, 
                                                Mail2, 
                                                CellPhone2, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID,
                                                @SChoolYear, 
                                                @ClubID, 
                                                @ClubName, 
                                                @Teacher1, 
                                                @Sex1, 
                                                @Unit1, 
                                                @Position1, 
                                                @Mail1, 
                                                @CellPhone1, 
                                                @Teacher2, 
                                                @Sex2, 
                                                @Unit2, 
                                                @Position2, 
                                                @Mail2, 
                                                @CellPhone2, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}

        public DbExecuteInfo Update0307(ClubHandoverViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", vm.Handover0307Model.HoID);
            parameters.Add("@HoDetailID", vm.Handover0307Model.HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0307Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0307Model.ClubID);
            parameters.Add("@ClubName", vm.Handover0307Model.ClubName);
            parameters.Add("@Teacher1", vm.Handover0307Model.Teacher1);
            parameters.Add("@Sex1", vm.Handover0307Model.Sex1);
            parameters.Add("@Unit1", vm.Handover0307Model.Unit1);
            parameters.Add("@Position1", vm.Handover0307Model.Position1);
            parameters.Add("@Mail1", vm.Handover0307Model.Mail1);
            parameters.Add("@CellPhone1", vm.Handover0307Model.CellPhone1);
            parameters.Add("@Teacher2", vm.Handover0307Model.Teacher2);
            parameters.Add("@Sex2", vm.Handover0307Model.Sex2);
            parameters.Add("@Unit2", vm.Handover0307Model.Unit2);
            parameters.Add("@Position2", vm.Handover0307Model.Position2);
            parameters.Add("@Mail2", vm.Handover0307Model.Mail2);
            parameters.Add("@CellPhone2", vm.Handover0307Model.CellPhone2);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE HandOverDoc07 
                                       SET SChoolYear=@SChoolYear, 
                                           ClubID=@ClubID, 
                                           ClubName=@ClubName, 
                                           Teacher1=@Teacher1, 
                                           Sex1=@Sex1, 
                                           Unit1=@Unit1, 
                                           Position1=@Position1, 
                                           Mail1=@Mail1, 
                                           CellPhone1=@CellPhone1, 
                                           Teacher2=@Teacher2, 
                                           Sex2=@Sex2, 
                                           Unit2=@Unit2, 
                                           Position2=@Position2, 
                                           Mail2=@Mail2, 
                                           CellPhone2=@CellPhone2, 
                                           LastModifier=@LoginId, 
                                           LastModified= GETDATE()
                                     WHERE HoID =@HoID AND HoDetailID=@HoDetailID";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

        public ClubHandover0307ViewModel GetHandover0307Data(string HoID, UserInfo Login)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@HoID", HoID);
			//parameters.Add("@ClubID", Login.LoginId);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.SchoolYear, A.ClubID, A.ClubName, 
                                    A.Teacher1, A.Sex1, C.Text AS SexText1, A.Unit1, A.Position1, A.Mail1, A.CellPhone1, 
                                    A.Teacher2, A.Sex2, D.Text AS SexText2, A.Unit2, A.Position2, A.Mail2, A.CellPhone2, A.Created, A.LastModified
                               FROM HandOverDoc07 A
                          LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                          LEFT JOIN Code C ON C.Code = A.Sex1 AND C.Type = 'Sex'
                          LEFT JOIN Code D ON D.Code = A.Sex2 AND D.Type = 'Sex'
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


			(DbExecuteInfo info, IEnumerable<ClubHandover0307ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0307ViewModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
		}

        #endregion

        #region Doc 0308

        public DbExecuteInfo Insert0308(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", HoID);
            parameters.Add("@HoDetailID", HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0308Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0308Model.ClubID);
            parameters.Add("@ClubName", vm.Handover0308Model.ClubName);
            parameters.Add("@Teacher", vm.Handover0308Model.Teacher);
            parameters.Add("@Sex", vm.Handover0308Model.Sex);
            parameters.Add("@Unit", vm.Handover0308Model.Unit);
            parameters.Add("@Position", vm.Handover0308Model.Position);
            parameters.Add("@Mail", vm.Handover0308Model.Mail);
            parameters.Add("@Tel", vm.Handover0308Model.Tel);
            parameters.Add("@CellPhone", vm.Handover0308Model.CellPhone);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO HandOverDoc08
                                               (HoID, 
                                                HoDetailID,
                                                SChoolYear, 
                                                ClubID, 
                                                ClubName, 
                                                Teacher, 
                                                Sex, 
                                                Unit, 
                                                Position, 
                                                Mail, 
                                                Tel, 
                                                CellPhone, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID,
                                                @SChoolYear, 
                                                @ClubID, 
                                                @ClubName, 
                                                @Teacher, 
                                                @Sex, 
                                                @Unit, 
                                                @Position, 
                                                @Mail, 
                                                @Tel, 
                                                @CellPhone, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo Update0308(ClubHandoverViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", vm.Handover0308Model.HoID);
            parameters.Add("@HoDetailID", vm.Handover0308Model.HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0308Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0308Model.ClubID);
            parameters.Add("@ClubName", vm.Handover0308Model.ClubName);
            parameters.Add("@Teacher", vm.Handover0308Model.Teacher);
            parameters.Add("@Sex", vm.Handover0308Model.Sex);
            parameters.Add("@Unit", vm.Handover0308Model.Unit);
            parameters.Add("@Position", vm.Handover0308Model.Position);
            parameters.Add("@Mail", vm.Handover0308Model.Mail);
            parameters.Add("@Tel", vm.Handover0308Model.Tel);
            parameters.Add("@CellPhone", vm.Handover0308Model.CellPhone);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE HandOverDoc08 
                                       SET SChoolYear=@SChoolYear, 
                                           ClubID=@ClubID, 
                                           ClubName=@ClubName, 
                                           Teacher=@Teacher, 
                                           Sex=@Sex, 
                                           Unit=@Unit, 
                                           Position=@Position, 
                                           Mail=@Mail, 
                                           Tel=@Tel, 
                                           CellPhone=@CellPhone, 
                                           LastModifier=@LoginId, 
                                           LastModified= GETDATE()
                                     WHERE HoID =@HoID AND HoDetailID=@HoDetailID";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }
        public ClubHandover0308ViewModel GetHandover0308Data(string HoID, UserInfo Login)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@HoID", HoID);
            //parameters.Add("@ClubID", Login.LoginId);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.SchoolYear, A.ClubID, A.ClubName, A.Teacher, A.Sex, C.Text AS SexText, A.Unit, A.Position, A.Mail, A.Tel, A.CellPhone, A.Created, A.LastModified
                               FROM HandOverDoc08 A
                          LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                          LEFT JOIN Code C ON C.Code = A.Sex AND C.Type = 'Sex'
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


            (DbExecuteInfo info, IEnumerable<ClubHandover0308ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0308ViewModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

		#endregion

		#region Doc 0309

		public DbExecuteInfo Insert0309(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
		{
			DataSet ds = new DataSet();
			DbExecuteInfo ExecuteResult = new DbExecuteInfo();
			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			parameters.Add("@HoID", HoID);
			parameters.Add("@HoDetailID", HoDetailID);
			parameters.Add("@SchoolYear", vm.Handover0309Model.SchoolYear);
			parameters.Add("@ClubID", vm.Handover0309Model.ClubID);
			parameters.Add("@ClubName", vm.Handover0309Model.ClubName);
			parameters.Add("@BookManName", vm.Handover0309Model.BookManName);
			parameters.Add("@BookManDepartment", vm.Handover0309Model.BookManDepartment);
			parameters.Add("@BookManPosition", vm.Handover0309Model.BookManPosition);
			parameters.Add("@BookManSNO", vm.Handover0309Model.BookManSNO);
			parameters.Add("@SealManName", vm.Handover0309Model.SealManName);
			parameters.Add("@SealManDepartment", vm.Handover0309Model.SealManDepartment);
			parameters.Add("@SealManPosition", vm.Handover0309Model.SealManPosition);
			parameters.Add("@SealManSNO", vm.Handover0309Model.SealManSNO);
			parameters.Add("@BookName", vm.Handover0309Model.BookName);
			parameters.Add("@BookNo", vm.Handover0309Model.BookNo);
			parameters.Add("@BookCover", vm.Handover0309Model.BookCover);
			parameters.Add("@BookCoverName", vm.Handover0309Model.BookCoverName);

			parameters.Add("@LoginId", LoginUser.LoginId);
			#endregion 參數設定

			string CommendText = $@"INSERT INTO HandOverDoc09
                                               (HoID, 
                                                HoDetailID,
                                                SChoolYear, 
                                                ClubID, 
                                                ClubName, 
                                                BookManName, 
                                                BookManDepartment, 
                                                BookManPosition, 
                                                BookManSNO, 
                                                SealManName, 
                                                SealManDepartment, 
                                                SealManPosition, 
                                                SealManSNO, 
                                                BookName, 
                                                BookNo, 
                                                BookCover, 
                                                BookCoverName, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               (@HoID, 
                                                @HoDetailID,
                                                @SChoolYear, 
                                                @ClubID, 
                                                @ClubName, 
                                                @BookManName, 
                                                @BookManDepartment, 
                                                @BookManPosition, 
                                                @BookManSNO, 
                                                @SealManName, 
                                                @SealManDepartment, 
                                                @SealManPosition, 
                                                @SealManSNO, 
                                                @BookName, 
                                                @BookNo, 
                                                @BookCover, 
                                                @BookCoverName,
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

			ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

			return ExecuteResult;
		}

        public DbExecuteInfo Update0309(ClubHandoverViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@HoID", vm.Handover0309Model.HoID);
            parameters.Add("@HoDetailID", vm.Handover0309Model.HoDetailID);
            parameters.Add("@SchoolYear", vm.Handover0309Model.SchoolYear);
            parameters.Add("@ClubID", vm.Handover0309Model.ClubID);
            parameters.Add("@ClubName", vm.Handover0309Model.ClubName);
            parameters.Add("@BookManName", vm.Handover0309Model.BookManName);
            parameters.Add("@BookManDepartment", vm.Handover0309Model.BookManDepartment);
            parameters.Add("@BookManPosition", vm.Handover0309Model.BookManPosition);
            parameters.Add("@BookManSNO", vm.Handover0309Model.BookManSNO);
            parameters.Add("@SealManName", vm.Handover0309Model.SealManName);
            parameters.Add("@SealManDepartment", vm.Handover0309Model.SealManDepartment);
            parameters.Add("@SealManPosition", vm.Handover0309Model.SealManPosition);
            parameters.Add("@SealManSNO", vm.Handover0309Model.SealManSNO);
            parameters.Add("@BookName", vm.Handover0309Model.BookName);
            parameters.Add("@BookNo", vm.Handover0309Model.BookNo);
            parameters.Add("@BookCover", vm.Handover0309Model.BookCover);
            parameters.Add("@BookCoverName", vm.Handover0309Model.BookCoverName);

            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE HandOverDoc09
                                       SET SChoolYear=@SChoolYear, 
                                            ClubID=@ClubID, 
                                            ClubName=@ClubName, 
                                            BookManName=@BookManName, 
                                            BookManDepartment=@BookManDepartment, 
                                            BookManPosition=@BookManPosition, 
                                            BookManSNO=@BookManSNO, 
                                            SealManName=@SealManName, 
                                            SealManDepartment=@SealManDepartment, 
                                            SealManPosition=@SealManPosition, 
                                            SealManSNO=@SealManSNO, 
                                            BookName=@BookName, 
                                            BookNo=@BookNo, 
                                            BookCover=@BookCover, 
                                            BookCoverName=@BookCoverName, 
                                            LastModifier=@LastModifier, 
                                            LastModified)=@LastModified), 
                                      WHERE HoID =@HoID AND HoDetailID=@HoDetailID";

            ExecuteResult = DbaExecuteQuery(CommendText, parameters, ds, true, DBAccessException);

            return ExecuteResult;
        }

        public ClubHandover0309ViewModel GetHandover0309Data(string HoID, UserInfo Login)
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			parameters.Add("@HoID", HoID);
			//parameters.Add("@ClubID", Login.LoginId);

			#region 參數設定
			#endregion

			CommandText = $@"SELECT A.DocID, A.HoID, A.HoDetailID, A.SchoolYear, A.ClubID, A.ClubName, 
                                    A.BookManName, A.BookManDepartment, A.BookManPosition, A.BookManSNO, 
                                    A.SealManName, A.SealManDepartment, A.SealManPosition, A.SealManSNO, 
                                    A.BookName, A.BookNo, A.BookCover, A.BookCoverName, A.Created, A.LastModified
                               FROM HandOverDoc09 A
                          LEFT JOIN HandOVerMain B ON B.HoID = A.HoID
                              WHERE 1 = 1
                                AND A.HoID = @HoID ";


			(DbExecuteInfo info, IEnumerable<ClubHandover0309ViewModel> entitys) dbResult = DbaExecuteQuery<ClubHandover0309ViewModel>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList().FirstOrDefault();

			return null;
		}

		#endregion

		#region File

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

        public DbExecuteInfo InsertFile(ClubHandoverViewModel vm, UserInfo LoginUser, string HoID, string HoDetailID)
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

			CommandText = $@"SELECT A.HoID, A.HoDetailID, A.DocType, C.Text AS DocTypeText, B.HandOverStatus, B.SchoolYear, A.Created
                               FROM HandOverDocDetail A
                          LEFT JOIN HandOverMain B ON B.HoID = A.HoID
                          LEFT JOIN Code C ON C.Code = A.DocType AND C.Type = 'DocType'
                              WHERE B.SchoolYear = @SchoolYear 
                           ORDER BY B.HandOverStatus, DocType ";

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

		public List<SelectListItem> getAllAgree()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'Agree' AND Code = '01'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<SelectListItem> getAllElectionType()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'ElectionType'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

		public List<SelectListItem> GetClubBuild()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'ClubBuild'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
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

		public List<SelectListItem> GetAllIdentityType()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'IdentityType'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<SelectListItem> GetAllConform()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'Conform'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}

		public List<SelectListItem> GetYesOrNo()
		{
			string CommandText = string.Empty;
			DataSet ds = new DataSet();

			DBAParameter parameters = new DBAParameter();

			#region 參數設定
			#endregion

			CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'YesOrNo'";

			(DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

			if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
				return dbResult.entitys.ToList();

			return new List<SelectListItem>();
		}


		#endregion

	}
}

