using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    public class FBorrowIndexDataAccess : BaseAccess
    {
        public List<FBorrowIndexResultModel> GetSearchResult()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();
            #region 參數設定


            #endregion

            CommandText = $@"SELECT ID, Text, CoverPath 
                               FROM BorrowMainClassMang
";

            (DbExecuteInfo info, IEnumerable<FBorrowIndexResultModel> entitys) dbResult = DbaExecuteQuery<FBorrowIndexResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FBorrowIndexResultModel>();
        }

        public FBorrowIndexDetailModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT ID, Text, BorrowSDate, BorrowEDate, BorrowRule, BorrowFee, ReserveRule
FROM BorrowMainClassMang
WHERE 1 = 1 
AND (ID = @ID) ";


            (DbExecuteInfo info, IEnumerable<FBorrowIndexDetailModel> entitys) dbResult = DbaExecuteQuery<FBorrowIndexDetailModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

    }
}
