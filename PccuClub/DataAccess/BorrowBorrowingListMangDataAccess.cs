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

    public class BorrowBorrowingListMangDataAccess : BaseAccess
    {

        public List<BorrowBorrowingUnitData> GetResurceData(string? MainResourceID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", MainResourceID);

            #endregion

            CommandText = $@"SELECT A.MainResourceID, A.ID, A.SecondResourceName 
                               FROM BorrowSecondResourceMang A
                              WHERE (@MainResourceID IS NULL OR A.MainResourceID = @MainResourceID)";

            (DbExecuteInfo info, IEnumerable<BorrowBorrowingUnitData> entitys) dbResult = DbaExecuteQuery<BorrowBorrowingUnitData>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowBorrowingUnitData>();
        }

        /// <summary> 查詢結果 </summary>

        public List<BorrowUnitData> GetSearchResult(BorrowBorrowingListMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SDate", model.SDate);
            parameters.Add("@EDate", DateTime.Parse(model.SDate).AddDays(6));

            #endregion

            CommandText = $@"
WITH DateGenerator AS (
    SELECT A.ID, A.BorrowMainID, A.MainClassID, B.ActName, A.MainResourceID, C.MainResourceName, B.TakeSDate, B.TakeEDate, B.ApplyUnitName,
           CAST(B.TakeSDate AS DATE) AS BorrowDate
    FROM BorrowDevice A
    LEFT JOIN BorrowMain B ON B.BorrowMainID = A.BorrowMainID
	LEFT JOIN BorrowMainResourceMang C ON C.MainResourceID = A.MainResourceID
    WHERE B.TakeSDate IS NOT NULL 
    
    UNION ALL
    
    SELECT ID, BorrowMainID, MainClassID, ActName, MainResourceID, MainResourceName, TakeSDate, TakeEDate, ApplyUnitName,
           CAST(DATEADD(day, 1, BorrowDate) AS DATE)
    FROM DateGenerator
    WHERE DATEADD(day, 1, BorrowDate) <= CAST(TakeEDate AS DATE)
)
SELECT ID, BorrowMainID, MainClassID, ActName, MainResourceID, MainResourceName, TakeSDate, TakeEDate, ApplyUnitName, BorrowDate AS Date
FROM DateGenerator
ORDER BY ID, Date
OPTION (MAXRECURSION 0);
";

            (DbExecuteInfo info, IEnumerable<BorrowUnitData> entitys) dbResult = DbaExecuteQuery<BorrowUnitData>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowUnitData>();
        }


        public List<SelectListItem> GetAllMainResourceID()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT MainResourceID AS VALUE, MainResourceName AS TEXT
                              FROM BorrowMainResourceMang ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

    }
}
