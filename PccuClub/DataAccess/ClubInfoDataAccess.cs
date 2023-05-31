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

namespace WebPccuClub.DataAccess
{

    public class ClubInfoDataAccess : BaseAccess
    {
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

        public List<SelectListItem> GetAllClubClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'ClubClass'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }


    }
}
