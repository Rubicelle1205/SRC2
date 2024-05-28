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

    public class FResourceBorrowDataAccess : BaseAccess
    {
        /// <summary> 查詢結果 </summary>

        public List<FResourceBorrowResultModel> GetSearchResult(FResourceBorrowConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@SDate", model.SDate.HasValue ? model.SDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);

            #endregion

            CommandText = $@"SELECT A.BorrowMainID, A.ApplyUnitName 
                               FROM BorrowMain A
                              WHERE 1 = 1 
{(model.SDate.HasValue ? " AND @SDate BETWEEN A.TakeSDate AND A.TakeEDate " : " ")}
";

            (DbExecuteInfo info, IEnumerable<FResourceBorrowResultModel> entitys) dbResult = DbaExecuteQuery<FResourceBorrowResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FResourceBorrowResultModel>();
        }


        public List<SelectListItem> GetBorrowSecondClassMang()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT A.ID AS VALUE, B.Text + '-' + A.Text AS TEXT
                              FROM BorrowSecondClassMang A
                         LEFT JOIN BorrowMainClassMang B ON B.ID = A.MainClass ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public FResourceBorrowResultDetailModel GetResultDetail(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ApplyUnitType, B.Text AS ApplyUnitTypeText, A.ApplyUnitName, A.ApplyMan, A.ApplyPurpose, 
                                    A.TakeSDate, A.TakeEDate, A.Created
                              FROM BorrowMain A
                         LEFT JOIN CODE B ON B.Code = A.ApplyUnitType AND B.Type = 'ApplyUnitType'
                             WHERE 1 = 1
AND (A.BorrowMainID = @ID) ";


            (DbExecuteInfo info, IEnumerable<FResourceBorrowResultDetailModel> entitys) dbResult = DbaExecuteQuery<FResourceBorrowResultDetailModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public List<FResourceBorrowResultDetailResourceModel> GetResultResourceDetail(string borrowMainID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@BorrowMainID", borrowMainID);
            #endregion

            CommandText = $@"SELECT '(' + B.Text + ')' + C.MainResourceName AS ResourceName, SUM(A.BorrowAmt) AS Amt
                              FROM BorrowDevice A
                         LEFT JOIN BorrowMainClassMang B ON B.ID = A.MainClassID
                         LEFT JOIN BorrowMainResourceMang C ON c.MainResourceID = A.MainResourceID
                        WHERE A.BorrowMainID = @BorrowMainID
                     GROUP BY B.Text, C.MainResourceName, A.BorrowAmt ";

            (DbExecuteInfo info, IEnumerable<FResourceBorrowResultDetailResourceModel> entitys) dbResult = DbaExecuteQuery<FResourceBorrowResultDetailResourceModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FResourceBorrowResultDetailResourceModel>();
        }
    }
}
