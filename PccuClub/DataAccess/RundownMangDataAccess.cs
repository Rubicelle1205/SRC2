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
using static NPOI.HSSF.Util.HSSFColor;

namespace WebPccuClub.DataAccess
{
    
    public class RundownMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<RundownMangResultModel> GetSearchResult(RundownMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ActType", model?.ActType);
            parameters.Add("@SDGs", model?.SDGs);
            parameters.Add("@PlaceSource", model?.PlaceSource);
            parameters.Add("@LifeClass", model?.LifeClass);
            parameters.Add("@RundownStatus", model?.RundownStatus);
            parameters.Add("@ActName", model?.ActName);

            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.ActRundownID, A.ActID, C.SchoolYear, C.ActName, C.Capacity, C.ActType, E.ActTypeName AS ActTypeText, 
                                    A.PlaceSource, F.Text AS PlaceSourceText, A.ActPlaceText,
                                    C.SDGs, C.BrrowUnit AS ClubiD, D.ClubCName, D.LifeClass, H.Text AS LifeClassText,
                                    B.ActVerify, G.Text AS ActVerifyText, A.RundownStatus,
                                    A.Date, A.STime, A.ETime, A.Created
                               FROM ActRundown A
                          LEFT JOIN ActMain B ON B.ActID = A.ActID
                          LEFT JOIN ActDetail C ON C.ActID = A.ActID
                          LEFT JOIN ClubMang D ON D.ClubId = C.BrrowUnit
                          LEFT JOIN ActTypeMang E ON E.ActTypeID = C.ActType
                          LEFT JOIN Code F ON F.Code = A.PlaceSource AND F.Type = 'PlaceSource'
                          LEFT JOIN Code G ON G.Code = B.ActVerify AND G.Type = 'ActVerify'
                          LEFT JOIN Code H ON H.Code = D.LifeClass AND H.Type = 'LifeClass'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR C.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR B.ActVerify = @ActVerify)
AND (@ActType IS NULL OR C.ActType = @ActType)
AND (@PlaceSource IS NULL OR A.PlaceSource = @PlaceSource)
AND (@LifeClass IS NULL OR D.LifeClass = @LifeClass)
AND (@RundownStatus IS NULL OR A.RundownStatus = @RundownStatus)

AND (@SDGs IS NULL OR C.SDGs LIKE '%' + @SDGs + '%') 
AND (@ClubID IS NULL OR C.BrrowUnit LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR D.ClubCName LIKE '%' + @ClubCName + '%') 
AND(@ActName IS NULL OR C.ActName LIKE '%' + @ActName + '%') ";


            (DbExecuteInfo info, IEnumerable<RundownMangResultModel> entitys) dbResult = DbaExecuteQuery<RundownMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<RundownMangResultModel>();
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateRundownData(RundownMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ActRundownID", vm.EditModel.ActRundownID);
            parameters.Add("@RundownStatus", vm.EditModel.RundownStatus);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@" UPDATE ActRundown 
                                        SET RundownStatus = @RundownStatus,
                                            LastModifier = @LoginId,
                                            LastModified = GETDATE()
                                     WHERE ActRundownID = @ActRundownID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<RundownMangExcelResultModel> GetExportResult(RundownMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ActType", model?.ActType);
            parameters.Add("@SDGs", model?.SDGs);
            parameters.Add("@PlaceSource", model?.PlaceSource);
            parameters.Add("@LifeClass", model?.LifeClass);
            parameters.Add("@RundownStatus", model?.RundownStatus);
            parameters.Add("@ActName", model?.ActName);

            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.ActRundownID, A.ActID, C.SchoolYear, C.ActName, C.Capacity, C.ActType, E.ActTypeName AS ActTypeText, 
                                    A.PlaceSource, F.Text AS PlaceSourceText, A.ActPlaceText,
                                    C.SDGs, C.BrrowUnit AS ClubiD, D.ClubCName, D.LifeClass, H.Text AS LifeClassText,
                                    B.ActVerify, G.Text AS ActVerifyText, A.RundownStatus,
                                    A.Date, A.STime, A.ETime, A.Created
                               FROM ActRundown A
                          LEFT JOIN ActMain B ON B.ActID = A.ActID
                          LEFT JOIN ActDetail C ON C.ActID = A.ActID
                          LEFT JOIN ClubMang D ON D.ClubId = C.BrrowUnit
                          LEFT JOIN ActTypeMang E ON E.ActTypeID = C.ActType
                          LEFT JOIN Code F ON F.Code = A.PlaceSource AND F.Type = 'PlaceSource'
                          LEFT JOIN Code G ON G.Code = B.ActVerify AND G.Type = 'ActVerify'
                          LEFT JOIN Code H ON H.Code = D.LifeClass AND H.Type = 'LifeClass'
                              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR C.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR B.ActVerify = @ActVerify)
AND (@ActType IS NULL OR C.ActType = @ActType)
AND (@PlaceSource IS NULL OR A.PlaceSource = @PlaceSource)
AND (@LifeClass IS NULL OR D.LifeClass = @LifeClass)
AND (@RundownStatus IS NULL OR A.RundownStatus = @RundownStatus)

AND (@SDGs IS NULL OR C.SDGs LIKE '%' + @SDGs + '%') 
AND (@ClubID IS NULL OR C.BrrowUnit LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR D.ClubCName LIKE '%' + @ClubCName + '%') 
AND(@ActName IS NULL OR C.ActName LIKE '%' + @ActName + '%') ";


            (DbExecuteInfo info, IEnumerable<RundownMangExcelResultModel> entitys) dbResult = DbaExecuteQuery<RundownMangExcelResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<RundownMangExcelResultModel>();
        }



        public List<SelectListItem> GetAllActVerify()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'ActVerify'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            for (int i = 108; i <= 130; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
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

        public List<SelectListItem> GetAllPlaceSource()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'PlaceSource'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
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

        public List<SelectListItem> GetAllRundownStatus()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, TEXT AS TEXT FROM Code WHERE Type = 'RundownStatus'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
