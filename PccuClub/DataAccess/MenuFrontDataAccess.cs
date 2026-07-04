using DataAccess;
using PccuClub.WebAuth;
using System.Data;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{

    public class MenuFrontDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<MenuFrontResultModel> GetSearchResult(MenuFrontConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = $@"SELECT MenuBoardId, MenuBoardCode, Header, ShortDesc, IconPath, IsEnable
                               FROM MenuBoardMang";


            (DbExecuteInfo info, IEnumerable<MenuFrontResultModel> entitys) dbResult = DbaExecuteQuery<MenuFrontResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<MenuFrontResultModel>();
        }

        public List<MenuFrontResultModel> GetSearchResult()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = $@"SELECT MenuBoardId, MenuBoardCode, Header, ShortDesc, IconPath, IsEnable
                               FROM MenuBoardMang 
                              ORDER BY MenuBoardId
";

            (DbExecuteInfo info, IEnumerable<MenuFrontResultModel> entitys) dbResult = DbaExecuteQuery<MenuFrontResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<MenuFrontResultModel>();
        }
    }
}
