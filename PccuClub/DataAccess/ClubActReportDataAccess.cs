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
using System.Runtime.ConstrainedExecution;

namespace WebPccuClub.DataAccess
{
    
    public class ClubActReportDataAccess : BaseAccess
    {
        PublicFun PublicFun = new PublicFun();

        /// <summary> 查詢結果 </summary>
        public List<ClubActReportResultModel> GetSearchResult(ClubActReportConditionModel model, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@LoginId", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT A.ActID, B.ActDetailId, B.ActName, B.SchoolYear, A.ActVerify, C.Text AS ActVerifyText, A.Created
                               FROM ActMain A
                          LEFT JOIN ActDetail B ON B.ActID = A.ActID
                          LEFT JOIN Code C ON C.Code = A.ActVerify AND C.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (@SchoolYear IS NULL OR B.SchoolYear = @SchoolYear)
                                AND (@LoginId IS NULL OR B.BrrowUnit = @LoginId)";


            (DbExecuteInfo info, IEnumerable<ClubActReportResultModel> entitys) dbResult = DbaExecuteQuery<ClubActReportResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubActReportResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClubActReportEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ActID, B.ActDetailId, B.ActName, B.SchoolYear, B.BrrowUnit, A.ActVerifyMemo, 
                                    B.StaticOrDynamic, D.Text AS StaticOrDynamicText, B.ActInOrOut, E.Text AS ActInOrOutText,
                                    B.Capacity, B.ActType, F.ActTypeName AS ActTypeText, B.UseITEquip, G.Text AS UseITEquipText,
                                    B.ShortDesc, B.SDGs, B.PassPort, H.Text AS PassPortText,
                                    I.LeaderName, I.LeaderTel, I.LeaderPhone, I.ManagerName, I.ManagerTel, I.ManagerPhone,
                                    A.ActVerify, C.Text AS ActVerifyText, A.Created
                              FROM ActMain A
                         LEFT JOIN ActDetail B ON B.ActID = A.ActID
                         LEFT JOIN Code C ON C.Code = A.ActVerify AND C.Type = 'ActVerify'
                         LEFT JOIN Code D ON D.Code = B.StaticOrDynamic AND D.Type = 'StaticOrDynamic'
                         LEFT JOIN Code E ON E.Code = B.ActInOrOut AND E.Type = 'ActInOrOut'
                         LEFT JOIN ActTypeMang F ON F.ActTypeID = B.ActType
                         LEFT JOIN Code G ON G.Code = B.UseITEquip AND G.Type = 'UseITEquip'
                         LEFT JOIN Code H ON H.Code = B.PassPort AND H.Type = 'PassPort'
                         LEFT JOIN ActOutSideInfo I ON I.ActID = A.ActID
                             WHERE 1 = 1
                                AND (A.ActID = @ActID) ";


            (DbExecuteInfo info, IEnumerable<ClubActReportEditModel> entitys) dbResult = DbaExecuteQuery<ClubActReportEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }


        public ClubActReportConsentModel GetConsentData()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = $@"SELECT InSchool, OutSchool, InAndOutSchool FROM ConsentMang";


            (DbExecuteInfo info, IEnumerable<ClubActReportConsentModel> entitys) dbResult = DbaExecuteQuery<ClubActReportConsentModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }



















        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(ClubActReportViewModel vm, UserInfo LoginUser, out DataTable dt)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            //parameters.Add("@ClubID", LoginUser.LoginId);
            //parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            //parameters.Add("@AwdActName", vm.CreateModel.AwdActName);
            //parameters.Add("@AwdName", vm.CreateModel.AwdName);
            //parameters.Add("@AwdDate", vm.CreateModel.AwdDate);
            //parameters.Add("@AwdType", vm.CreateModel.AwdType);
            //parameters.Add("@Organizer", vm.CreateModel.Organizer);
            //parameters.Add("@ActVerify", "01");
            //parameters.Add("@Attachment", vm.CreateModel.Attachment);
            //parameters.Add("@LoginId", LoginUser.LoginId);
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
        public DbExecuteInfo UpdateData(ClubActReportViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            //parameters.Add("@AwdID", vm.EditModel.AwdID);
            //parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            //parameters.Add("@AwdActName", vm.EditModel.AwdActName); 
            //parameters.Add("@AwdDate", vm.EditModel.AwdDate);
            //parameters.Add("@AwdType", vm.EditModel.AwdType);
            //parameters.Add("@AwdName", vm.EditModel.AwdName);
            //parameters.Add("@Organizer", vm.EditModel.Organizer);
            //parameters.Add("@ActVerify", vm.EditModel.ActVerify);

            //if(!string.IsNullOrEmpty(vm.EditModel.Attachment))
            //    parameters.Add("@Attachment", vm.EditModel.Attachment);

            //parameters.Add("@Memo", vm.EditModel.Memo);
            //parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@" UPDATE AwardMang 
                                        SET SchoolYear = @SchoolYear, 
                                            AwdActName = @AwdActName, 
                                            AwdDate = @AwdDate, 
                                            AwdType = @AwdType, 
                                            AwdName = @AwdName, 
                                            Organizer = @Organizer, 
                                            ActVerify = @ActVerify, 

                                            Memo = @Memo, 
                                            LastModifier = @LoginId, 
                                            LastModified = GETDATE() 
                                     WHERE AwdID = @AwdID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo EditDetailData(ClubActReportViewModel vm, List<AwdDetailModel> dataList, UserInfo LoginUser)
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


		public List<SelectListItem> GetAllSDGs()
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

    }
}
