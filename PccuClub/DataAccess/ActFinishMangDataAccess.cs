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
using MathNet.Numerics.RootFinding;

namespace WebPccuClub.DataAccess
{
    
    public class ActFinishMangDataAccess : BaseAccess
    {
        PublicFun PublicFun = new PublicFun();

        /// <summary> 查詢結果 </summary>
        public List<ActFinishMangResultModel> GetSearchResult(ActFinishMangConditionModel model, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ClubID", model?.ClubId);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@ActID", model?.ActID);
            parameters.Add("@ActName", model?.ActName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy-MM-dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy-MM-dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.ActFinishId, A.ActID, B.ActDetailId, A.ClubId, A.ClubCName, A.ActName, B.SchoolYear, A.ActFinishVerify, D.Text AS ActVerifyText, A.Created
                               FROM ActFinish A
                          LEFT JOIN ActDetail B ON B.ActID = A.ActID AND B.ActDetailId = A.ActDetailId
                          LEFT JOIN ActMain C ON C.ActID = B.ActID
                          LEFT JOIN Code D ON D.Code = A.ActFinishVerify AND D.Type = 'ActVerify'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR B.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR A.ActFinishVerify = @ActVerify)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR A.ClubCName LIKE '%' + @ClubCName + '%') 
AND (@ActID IS NULL OR A.ActID LIKE '%' + @ActID + '%') 
AND (@ActName IS NULL OR A.ActName LIKE '%' + @ActName + '%')  ";


            (DbExecuteInfo info, IEnumerable<ActFinishMangResultModel> entitys) dbResult = DbaExecuteQuery<ActFinishMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActFinishMangResultModel>();
        }

        /// <summary>取得Excel資料</summary>
        internal List<ActFinishMangExcelModel> GetExportResult(ActFinishMangConditionModel model, UserInfo loginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ClubID", model?.ClubId);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@ActID", model?.ActID);
            parameters.Add("@ActName", model?.ActName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy-MM-dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy-MM-dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.ActFinishId, A.ActID, B.ActDetailId, A.ClubId, A.ClubCName, A.ActName, B.SchoolYear, A.ActFinishVerify, D.Text AS ActVerifyText, A.Created
                               FROM ActFinish A
                          LEFT JOIN ActDetail B ON B.ActID = A.ActID AND B.ActDetailId = A.ActDetailId
                          LEFT JOIN ActMain C ON C.ActID = B.ActID
                          LEFT JOIN Code D ON D.Code = A.ActFinishVerify AND D.Type = 'ActVerify'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR B.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR A.ActFinishVerify = @ActVerify)
AND (@ClubID IS NULL OR A.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR A.ClubCName LIKE '%' + @ClubCName + '%') 
AND (@ActID IS NULL OR A.ActID LIKE '%' + @ActID + '%') 
AND (@ActName IS NULL OR A.ActName LIKE '%' + @ActName + '%')  ";


            (DbExecuteInfo info, IEnumerable<ActFinishMangExcelModel> entitys) dbResult = DbaExecuteQuery<ActFinishMangExcelModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ActFinishMangExcelModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ActFinishMangDetailModel GetDetailData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActFinishId", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ActFinishId, A.ActID, A.ActDetailId, A.ClubId, A.ClubCName, A.Caseman, A.Email, A.Tel, A.ActDate, A.ActName, A.Course, 
                                    A.ShortInfo, A.ElseFile, A.ActFinishVerify, B.Text AS ActVerifyText
                               FROM ActFinish A
                          LEFT JOIN Code B ON B.Code = A.ActFinishVerify AND B.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (A.ActFinishId = @ActFinishId) ";


            (DbExecuteInfo info, IEnumerable<ActFinishMangDetailModel> entitys) dbResult = DbaExecuteQuery<ActFinishMangDetailModel>(CommandText, parameters, true, DBAccessException);

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
        public ActFinishMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActFinishId", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ActFinishId, A.ActID, A.ActDetailId, A.ClubId, A.ClubCName, A.Caseman, A.Email, A.Tel, A.ActDate, A.ActName, A.Course, 
                                    A.ShortInfo, A.ElseFile, A.ActFinishVerify, B.Text AS ActVerifyText
                               FROM ActFinish A
                          LEFT JOIN Code B ON B.Code = A.ActFinishVerify AND B.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (A.ActFinishId = @ActFinishId) ";


            (DbExecuteInfo info, IEnumerable<ActFinishMangEditModel> entitys) dbResult = DbaExecuteQuery<ActFinishMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 取得學號資料 </summary>
        public List<PersonModel> GetPersonData(string id)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ActFinishId", id);

            #endregion

            CommandText = $@"SELECT A.ActFinishPersonId, A.ActFinishId, A.SNO
                               FROM ActFinishPerson A
                              WHERE 1 = 1
                                AND ActFinishId = @ActFinishId
                           ORDER BY A.ActFinishPersonId";


            (DbExecuteInfo info, IEnumerable<PersonModel> entitys) dbResult = DbaExecuteQuery<PersonModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PersonModel>();
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

        public List<SelectListItem> GetActVerify()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'ActVerify' AND Code <> '05'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<PersonModel> GetExportResult(string id)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ActFinishId", id);

            #endregion

            CommandText = $@"SELECT A.SNO
                               FROM ActFinishPerson A
                              WHERE 1 = 1
                                AND ActFinishId = @ActFinishId
                           ORDER BY A.ActFinishPersonId";


            (DbExecuteInfo info, IEnumerable<PersonModel> entitys) dbResult = DbaExecuteQuery<PersonModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PersonModel>();
        }

        public List<ALLPersonModel> GetExportResult(string year, string verify, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@SchoolYear", year);
            parameters.Add("@ActVerify", verify);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion

            CommandText = $@"SELECT C.SchoolYear, B.ActID, B.ActName, A.SNO
                               FROM ActFinishPerson A
                          LEFT JOIN ActFinish B ON B.ActFinishId = A.ActFinishId
						  LEFT JOIN ActDetail C ON C.ActID = B.ActID AND C.ActDetailId = B.ActDetailId
						  LEFT JOIN Code D ON D.Code = B.ActFinishVerify AND D.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (@SchoolYear IS NULL OR C.SchoolYear = @SchoolYear)
                                AND (@LoginId IS NULL OR C.BrrowUnit = @LoginId)
                                AND (@ActVerify IS NULL OR D.Code = @ActVerify) 
                           ORDER BY B.Created, A.SNO ASC";


            (DbExecuteInfo info, IEnumerable<ALLPersonModel> entitys) dbResult = DbaExecuteQuery<ALLPersonModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ALLPersonModel>();
        }

        
    }
}
