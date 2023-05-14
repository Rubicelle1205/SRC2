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
            parameters.Add("@RoleId", model.ClubClass);
            
            #endregion

            CommandText = $@"SELECT A.ClubId, A.ClubCName, A.ClubClass, B.Text AS ClubClassText, A.LogoPath
                               FROM ClubMang A
                          LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass' ";

            (DbExecuteInfo info, IEnumerable<ClubListResultModel> entitys) dbResult = DbaExecuteQuery<ClubListResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubListResultModel>();
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
