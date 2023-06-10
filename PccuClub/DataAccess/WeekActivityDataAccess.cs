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

        /// <summary> 查詢結果 </summary>

        public List<WeekActivityResultModel> GetSearchResult(WeekActivityConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            //parameters.Add("@ClubClass", model.ClubClass);
            
            #endregion

            CommandText = $@"SELECT A.ClubId, A.ClubCName, A.ClubClass, B.Text AS ClubClassText, A.LogoPath
                               FROM ClubMang A
                          LEFT JOIN Code B ON B.Code = A.ClubClass AND B.Type = 'ClubClass' 
                              WHERE 1 = 1 
                                ";

            (DbExecuteInfo info, IEnumerable<WeekActivityResultModel> entitys) dbResult = DbaExecuteQuery<WeekActivityResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<WeekActivityResultModel>();
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
