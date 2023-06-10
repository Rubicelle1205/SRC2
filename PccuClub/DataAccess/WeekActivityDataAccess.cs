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

namespace WebPccuClub.DataAccess
{

    public class WeekActivityDataAccess : BaseAccess
    {

        public List<PlaceData> GetPlaceData(string? buildID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@Buildid", buildID);

            #endregion

            CommandText = $@"SELECT PlaceID, PlaceName
                               FROM PlaceSchoolMang
                              WHERE (@Buildid IS NULL OR Buildid = @Buildid)";

            (DbExecuteInfo info, IEnumerable<PlaceData> entitys) dbResult = DbaExecuteQuery<PlaceData>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PlaceData>();
        }

        /// <summary> 查詢結果 </summary>

        public List<WeekActClubData> GetSearchResult(WeekActivityConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SDate", model.SDate);
            parameters.Add("@EDate", DateTime.Parse(model.SDate).AddDays(6));

            #endregion

            CommandText = $@"SELECT A.ActID, MIN(A.STime) AS STime, MAX(A.ETime) AS ETime,
                                    A.ActPlaceID, A.ActPlaceText, A.[Date], C.ActName, C.BrrowUnit AS ClubID, D.ClubCName AS ClubCName
                               FROM ActRundown A
                          LEFT JOIN PlaceSchoolMang B on B.PlaceID = A.ActPlaceID
                          LEFT JOIN ActDetail C ON C.ActDetailId = A.ActDetailId
                          LEFT JOIN ClubMang D ON D.ClubId = C.BrrowUnit
                              WHERE [date] between @SDate AND @EDate
                           GROUP BY A.ActID,A.ActPlaceID, A.ActPlaceText, A.[Date], C.ActName, C.BrrowUnit, D.ClubCName";

            (DbExecuteInfo info, IEnumerable<WeekActClubData> entitys) dbResult = DbaExecuteQuery<WeekActClubData>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<WeekActClubData>();
        }


        public List<SelectListItem> GetAllBuild()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT BuildID AS VALUE, BuildName AS TEXT FROM BuildMang ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
