using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class ClubHolisticPassportDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ClubHolisticPassportResultModel> GetSearchResult(ClubHolisticPassportConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ActName", model?.ActName);
            parameters.Add("@OrderBy", model?.OrderBy);
            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.SchoolYear, A.ActID, A.HolisticActName, A.ActVerify, B.Text AS ActVerifyText, A.ActVerifyMemo, A.Created
                               FROM HolisticPassportMang A
                          LEFT JOIN Code B ON B.Code = A.ActVerify AND B.Type = 'ActVerify'
WHERE 1 = 1
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear) 
AND (@ActVerify IS NULL OR ActVerify = @ActVerify) 
AND (@ActName IS NULL OR A.ActName LIKE '%' + @ActName + '%') 
{(!string.IsNullOrEmpty(model.OrderBy) ? " ORDER BY A.ActID " + model.OrderBy : " ORDER BY A.ActID DESC")}";


            (DbExecuteInfo info, IEnumerable<ClubHolisticPassportResultModel> entitys) dbResult = DbaExecuteQuery<ClubHolisticPassportResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubHolisticPassportResultModel>();
        }

        public ClubHolisticPassportDetailModel GetDetailData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.SchoolYear, A.ClubID, A.ActID, A.ActName, A.HolisticActName, A.ActDesc, 
                                    A.MainID, B.Text AS MainIDText, A.SecondID, C.Text AS SecondIDText, A.ThridID, D.Text AS ThridIDText,
                                    A.ActSTime, A.ActETime, A.RegistrationWay, A.PlaceSource, E.Text AS PlaceSourceText,
                                    A.BuildID, A.PlaceID, A.PlaceName, A.Presenter, A.PresenterIntro, A.Host, A.HostIntro, A.ClubCName, A.ContactMan, 
                                    A.RegistrationMan, A.OpenObject, A.Tag, A.PosterIconPath, A.Memo, A.ActVerify, A.ActVerifyMemo, A.Creator, A.Created, A.LastModifier, A.LastModified
                               FROM HolisticPassportMang A
                          LEFT JOIN HolisticMainClassMang B ON B.ID = A.MainID
                          LEFT JOIN HolisticSecondClassMang C ON C.ID = A.SecondID
                          LEFT JOIN HolisticThirdClassMang D ON D.ID = A.ThridID
                          LEFT JOIN code E ON E.Code = A.PlaceSource AND E.Type = 'PlaceSource'
WHERE 1 = 1
AND (A.ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<ClubHolisticPassportDetailModel> entitys) dbResult = DbaExecuteQuery<ClubHolisticPassportDetailModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClubHolisticPassportEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT ID, SchoolYear, ClubID, ActID, ActName, HolisticActName, ActDesc, MainID, SecondID, ThridID, ActSTime, ActETime, 
                                    RegistrationWay, PlaceSource, BuildID, PlaceID, PlaceName, Presenter, PresenterIntro, Host, HostIntro, ClubCName, ContactMan, 
                                    RegistrationMan, OpenObject, Tag, PosterIconPath, Memo, ActVerify, ActVerifyMemo, Creator, Created, LastModifier, LastModified
                               FROM HolisticPassportMang
WHERE 1 = 1
AND (ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<ClubHolisticPassportEditModel> entitys) dbResult = DbaExecuteQuery<ClubHolisticPassportEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(ClubHolisticPassportViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SchoolYear", vm.EditModel.SchoolYear);
            parameters.Add("@ClubID", vm.EditModel.ClubID);
            parameters.Add("@ActID", vm.EditModel.ActID);
            parameters.Add("@ActName", vm.EditModel.ActName);
            parameters.Add("@HolisticActName", vm.EditModel.HolisticActName);
            parameters.Add("@ActDesc", vm.EditModel.ActDesc);
            parameters.Add("@MainID", vm.EditModel.MainID);
            parameters.Add("@SecondID", vm.EditModel.SecondID);
            parameters.Add("@ThridID", vm.EditModel.ThridID);
            parameters.Add("@ActSTime", vm.EditModel.ActSTime);
            parameters.Add("@ActETime", vm.EditModel.ActETime);
            parameters.Add("@RegistrationWay", vm.EditModel.RegistrationWay);
            parameters.Add("@PlaceSource", vm.EditModel.PlaceSource);
            parameters.Add("@BuildID", vm.EditModel.BuildID);
            parameters.Add("@PlaceID", vm.EditModel.PlaceID);
            parameters.Add("@PlaceName", vm.EditModel.PlaceName);
            parameters.Add("@Presenter", vm.EditModel.Presenter);
            parameters.Add("@PresenterIntro", vm.EditModel.PresenterIntro);
            parameters.Add("@Host", vm.EditModel.Host);
            parameters.Add("@HostIntro", vm.EditModel.HostIntro);
            parameters.Add("@ClubCName", vm.EditModel.ClubCName);
            parameters.Add("@ContactMan", vm.EditModel.ContactMan);
            parameters.Add("@RegistrationMan", vm.EditModel.RegistrationMan);
            parameters.Add("@OpenObject", vm.EditModel.OpenObject);
            parameters.Add("@Tag", vm.EditModel.Tag);
            parameters.Add("@PosterIconPath", vm.EditModel.PosterIconPath);
            parameters.Add("@Memo", vm.EditModel.Memo);
            parameters.Add("@LoginId", LoginUser.LoginId);

            parameters.Add("@ID", vm.EditModel.ID);
            #endregion 參數設定

            string CommendText = $@"UPDATE HolisticPassportMang 
                                       SET SchoolYear = @SchoolYear, 
                                            ClubID = @ClubID, 
                                            ActID = @ActID, 
                                            ActName = @ActName, 
                                            HolisticActName = @HolisticActName, 
                                            ActDesc = @ActDesc,
                                            MainID = @MainID, 
                                            SecondID = @SecondID, 
                                            ThridID = @ThridID, 
                                            ActSTime = @ActSTime, 
                                            ActETime = @ActETime, 
                                            RegistrationWay = @RegistrationWay, 
                                            PlaceSource = @PlaceSource, 
                                            BuildID = @BuildID, 
                                            PlaceID = @PlaceID, 
                                            PlaceName = @PlaceName, 
                                            Presenter = @Presenter, 
                                            PresenterIntro = @PresenterIntro, 
                                            Host = @Host, 
                                            HostIntro = @HostIntro, 
                                            ClubCName = @ClubCName, 
                                            ContactMan = @ContactMan, 
                                            RegistrationMan = @RegistrationMan, 
                                            OpenObject = @OpenObject, 
                                            Tag = @Tag, 
                                            PosterIconPath = @PosterIconPath, 
                                            Memo = @Memo, 
                                            LastModifier = @LoginId, 
                                            LastModified = GETDATE()
                                     WHERE ID = @ID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }


        public List<SelectListItem> GetAllClubID()
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

        public List<SelectListItem> GetddlHolisticMainClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM HolisticMainClassMang ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlHolisticSecondClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM HolisticSecondClassMang ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlHolisticThirdClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM HolisticThirdClassMang ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlActInOrOut()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS Text FROM code WHERE Type = 'ActInOrOut' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        
    }
}
