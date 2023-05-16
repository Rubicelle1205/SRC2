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

    public class ClubListDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ClubListResultModel> GetSearchResult(ClubListConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ClubClass", model.ClubClass);
            
            #endregion

            CommandText = $@"SELECT A.ClubId, A.ClubCName, A.ClubClass, B.Text AS ClubClassText, A.LogoPath
                               FROM ClubMang A
                          LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass' 
                              WHERE 1 = 1 
                                AND (@ClubClass IS NULL OR A.ClubClass = @ClubClass)";

            (DbExecuteInfo info, IEnumerable<ClubListResultModel> entitys) dbResult = DbaExecuteQuery<ClubListResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubListResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClubListEditModel GetEditData(string ClubId)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ClubId", ClubId);

            #region 參數設定
            #endregion

            CommandText = $@"
                            SELECT A.ClubId, A.ClubCName, A.ClubEName, A.SchoolYear, A.LifeClass, C.Text AS LifeClassText, A.ClubClass, B.Text AS ClubClassText, A.Address, A.EMail, A.Tel, 
                                   A.Social1, A.Social2, A.Social3, A.LogoPath, A.ActImgPath, A.ShortInfo, A.Memo, A.Created, A.LastModified, D.RoleId
                               FROM ClubMang A
							   LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass'
							   LEFT JOIN Code C ON C.Code = A.LifeClass AND C.Type = 'LifeClass'
                               LEFT JOIN UserRole D ON D.LoginId = A.ClubId
                              WHERE 1 = 1
                               AND A.ClubId = @ClubId";


            (DbExecuteInfo info, IEnumerable<ClubListEditModel> entitys) dbResult = DbaExecuteQuery<ClubListEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public List<SelectListItem> GetAllClubClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'ClubClass' ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
