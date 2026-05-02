using DataAccess;
using PccuClub.WebAuth;
using System.Data;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{

    public class ClubSelectDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<ClubSelectResultModel> GetSearchResult(ClubSelectConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = $@"SELECT MenuBoardId, MenuBoardCode, Header, ShortDesc, IconPath, IsEnable
                               FROM MenuBoardMang";


            (DbExecuteInfo info, IEnumerable<ClubSelectResultModel> entitys) dbResult = DbaExecuteQuery<ClubSelectResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubSelectResultModel>();
        }

        public List<ClubSelectResultModel> GetSearchResult()
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

            (DbExecuteInfo info, IEnumerable<ClubSelectResultModel> entitys) dbResult = DbaExecuteQuery<ClubSelectResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubSelectResultModel>();
        }
    }
}
