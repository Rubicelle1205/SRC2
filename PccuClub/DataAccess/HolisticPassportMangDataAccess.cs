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
    
    public class HolisticPassportMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<HolisticPassportMangResultModel> GetSearchResult(HolisticPassportMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@ActID", model?.ActID);
            parameters.Add("@ActName", model?.ActName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.SchoolYear, A.ClubID, A.ClubCName, A.ActID, A.ActName, A.ActVerify, B.Text AS ActVerifyText, A.Created
                               FROM HolisticPassportMang A
                          LEFT JOIN Code B ON B.Code = A.ActVerify AND B.Type = 'ActVerify'
WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear) 
AND (@ActVerify IS NULL OR ActVerify = @ActVerify) 
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR A.ClubCName LIKE '%' + @ClubCName + '%') 
AND (@ActID IS NULL OR A.ActID LIKE '%' + @ActID + '%') 
AND (@ActName IS NULL OR A.ActName LIKE '%' + @ActName + '%') 
";


            (DbExecuteInfo info, IEnumerable<HolisticPassportMangResultModel> entitys) dbResult = DbaExecuteQuery<HolisticPassportMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HolisticPassportMangResultModel>();
        }

        public List<HolisticPassportMangResultModel> GetExportResult(HolisticPassportMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@ActID", model?.ActID);
            parameters.Add("@ActName", model?.ActName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);
            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ID, A.SchoolYear, A.ClubID, A.ClubCName, A.ActID, A.ActName, A.ActVerify, B.Text AS ActVerifyText, A.Creator
                               FROM HolisticPassportMang A
                          LEFT JOIN Code B ON B.Code = A.ActVerify AND B.Type = 'ActVerify'
WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR A.SchoolYear = @SchoolYear) 
AND (@ActVerify IS NULL OR ActVerify = @ActVerify) 
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR A.ClubCName LIKE '%' + @ClubCName + '%') 
AND (@ActID IS NULL OR A.ActID LIKE '%' + @ActID + '%') 
AND (@ActName IS NULL OR A.ActName LIKE '%' + @ActName + '%') 
";


            (DbExecuteInfo info, IEnumerable<HolisticPassportMangResultModel> entitys) dbResult = DbaExecuteQuery<HolisticPassportMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HolisticPassportMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HolisticPassportMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT ID, ClubID, ActID, ActName, HolisticActName, ActDesc, MainID, SecondID, ThridID, ActSTime, ActETime, RegistrationWay, 
                                    PlaceSource, BuildID, PlaceID, PlaceName, Presenter, PresenterIntro, Host, HostIntro, 
                                    ClubCName, ContactMan, RegistrationMan, OpenObject, Tag, PosterIconPath, Memo, ActVerify, ActVerifyMemo, 
                                    Creator, Created, LastModifier, LastModified
                               FROM HolisticPassportMang
WHERE 1 = 1
AND (ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<HolisticPassportMangEditModel> entitys) dbResult = DbaExecuteQuery<HolisticPassportMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(HolisticPassportMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SchoolYear", vm.CreateModel.SchoolYear);
            parameters.Add("@ClubID", vm.CreateModel.ClubID);
            parameters.Add("@ActID", vm.CreateModel.ActID);
            parameters.Add("@ActName", vm.CreateModel.ActName);
            parameters.Add("@HolisticActName", vm.CreateModel.HolisticActName);
            parameters.Add("@ActDesc", vm.CreateModel.ActDesc);
            parameters.Add("@MainID", vm.CreateModel.MainID);
            parameters.Add("@SecondID", vm.CreateModel.SecondID);
            parameters.Add("@ThridID", vm.CreateModel.ThridID);
            parameters.Add("@ActSTime", vm.CreateModel.ActSTime);
            parameters.Add("@ActETime", vm.CreateModel.ActETime);
            parameters.Add("@RegistrationWay", vm.CreateModel.RegistrationWay);
            parameters.Add("@PlaceSource", vm.CreateModel.PlaceSource);
            parameters.Add("@BuildID", vm.CreateModel.BuildID);
            parameters.Add("@PlaceID", vm.CreateModel.PlaceID);
            parameters.Add("@PlaceName", vm.CreateModel.PlaceName);
            parameters.Add("@Presenter", vm.CreateModel.Presenter);
            parameters.Add("@PresenterIntro", vm.CreateModel.PresenterIntro);
            parameters.Add("@Host", vm.CreateModel.Host);
            parameters.Add("@HostIntro", vm.CreateModel.HostIntro);
            parameters.Add("@ClubCName", vm.CreateModel.ClubCName);
            parameters.Add("@ContactMan", vm.CreateModel.ContactMan);
            parameters.Add("@RegistrationMan", vm.CreateModel.RegistrationMan);
            parameters.Add("@OpenObject", vm.CreateModel.OpenObject);
            parameters.Add("@Tag", vm.CreateModel.Tag);
            parameters.Add("@PosterIconPath", vm.CreateModel.PosterIconPath);
            parameters.Add("@Memo", vm.CreateModel.Memo);
            parameters.Add("@ActVerify", vm.CreateModel.ActVerify);
            parameters.Add("@ActVerifyMemo", vm.CreateModel.ActVerifyMemo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO HolisticPassportMang
                                               (SchoolYear, 
                                                ClubID, 
                                                ActID, 
                                                ActName, 
                                                HolisticActName, 
                                                ActDesc,
                                                MainID, 
                                                SecondID, 
                                                ThridID, 
                                                ActSTime, 
                                                ActETime, 
                                                RegistrationWay, 
                                                PlaceSource, 
                                                BuildID, 
                                                PlaceID, 
                                                PlaceName, 
                                                Presenter, 
                                                PresenterIntro, 
                                                Host, 
                                                HostIntro, 
                                                ClubCName, 
                                                ContactMan, 
                                                RegistrationMan, 
                                                OpenObject, 
                                                Tag, 
                                                PosterIconPath, 
                                                Memo, 
                                                ActVerify, 
                                                ActVerifyMemo, 
                                                Creator, 
                                                Created, 
                                                LastModifier,
                                                LastModified)
                                         VALUES
                                               (@SchoolYear, 
                                                @ClubID, 
                                                @ActID, 
                                                @ActName, 
                                                @HolisticActName, 
                                                @ActDesc,
                                                @MainID, 
                                                @SecondID, 
                                                @ThridID, 
                                                @ActSTime, 
                                                @ActETime, 
                                                @RegistrationWay, 
                                                @PlaceSource, 
                                                @BuildID, 
                                                @PlaceID, 
                                                @PlaceName, 
                                                @Presenter, 
                                                @PresenterIntro, 
                                                @Host, 
                                                @HostIntro, 
                                                @ClubCName, 
                                                @ContactMan, 
                                                @RegistrationMan, 
                                                @OpenObject, 
                                                @Tag, 
                                                @PosterIconPath, 
                                                @Memo, 
                                                @ActVerify, 
                                                @ActVerifyMemo, 
                                                @LoginId, 
                                                GETDATE(), 
                                                @LoginId, 
                                                GETDATE())";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(HolisticPassportMangViewModel vm, UserInfo LoginUser)
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
            parameters.Add("@ActVerify", vm.EditModel.ActVerify);
            parameters.Add("@ActVerifyMemo", vm.EditModel.ActVerifyMemo);
            parameters.Add("@LoginId", LoginUser.LoginId);
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
                                            ActVerify = @ActVerify, 
                                            ActVerifyMemo = @ActVerifyMemo, 
                                            LastModifier = @LoginId, 
                                            LastModified = GETDATE()
                                     WHERE ID = @ID";

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
            parameters.Add("@ID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM HolisticPassportMang WHERE ID = @ID";

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
