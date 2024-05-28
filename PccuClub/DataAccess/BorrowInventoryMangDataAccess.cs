using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class BorrowInventoryMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BorrowInventoryMangResultModel> GetSearchResult(BorrowInventoryMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@Text", model?.Text);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT A.ID, A.MainResourceID, B.MainResourceName AS MainResourceIDText, A.AmtReal
      ,A.AmtInventory
      ,A.BorrowType, C.Text AS BorrowTypeText, A.Creator
      ,A.Created
      ,A.LastModifier
      ,A.LastModified
FROM InventoryRecord A
LEFT JOIN BorrowMainResourceMang B ON B.MainResourceID = A.MainResourceID
LEFT JOIN Code C ON C.Code = A.BorrowType AND C.Type = 'BorrowMultType'
WHERE 1 = 1
AND (@Text IS NULL OR Text LIKE '%' + @Text + '%') ";


            (DbExecuteInfo info, IEnumerable<BorrowInventoryMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowInventoryMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowInventoryMangResultModel>();
        }

        public List<BorrowInventoryMangExcelModel> GetExportResult(string InventoryRecordID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@InventoryRecordID", InventoryRecordID);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.SecondResourceNo, A.SecondResourceName, A.ShelvesStatus, B.Text AS ShelvesStatusText, 
		                            A.BorrowStatus, C.Text AS BorrowStatusText, A.ResourceInventoryStatus, D.Text AS ResourceInventoryStatusText
                               FROM InventoryDetail A
                          LEFT JOIN Code B ON B.Code = A.ShelvesStatus AND B.Type = 'ShelvesStatus'
                          LEFT JOIN Code C ON C.Code = A.BorrowStatus AND C.Type = 'BorrowStatus'
                          LEFT JOIN Code D ON D.Code = A.ResourceInventoryStatus AND D.Type = 'InventoryStatus'
                              WHERE InventoryRecordID = @InventoryRecordID";


            (DbExecuteInfo info, IEnumerable<BorrowInventoryMangExcelModel> entitys) dbResult = DbaExecuteQuery<BorrowInventoryMangExcelModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowInventoryMangExcelModel>();
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="ser"></param>
        /// <returns></returns>
        public DbExecuteInfo DeletetData(string ser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM InventoryRecord WHERE ID = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        
    }
}
