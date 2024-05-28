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

        #region 今日借用者

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


        #endregion

        #region 資源借用狀況

        public List<FResourceBorrowResourceResultModel> GetResourceSearchResult(FResourceBorrowConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", model.BorrowSecondClassID);

            #endregion

            CommandText = $@"SELECT A.MainResourceID, A.SecondClass, A.MainClass, B.Text AS MainClassText, A.MainResourceName, A.ShortDesc, A.AmtShelves AS RemainAmt, A.AmtShelves
                               FROM BorrowMainResourceMang A
                          LEFT JOIN BorrowMainClassMang B on B.ID = A.MainClass
                              WHERE 1 =1
                                AND (@MainResourceID IS NULL OR A.MainResourceID = @MainResourceID) 
";

            (DbExecuteInfo info, IEnumerable<FResourceBorrowResourceResultModel> entitys) dbResult = DbaExecuteQuery<FResourceBorrowResourceResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FResourceBorrowResourceResultModel>();
        }

        public List<FResourceBorrowResourceBorrowedModel> GetBorrowedResult(FResourceBorrowConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", model.BorrowSecondClassID);
            parameters.Add("@SDate", model.SDate.HasValue ? model.SDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            #endregion

            CommandText = $@"SELECT A.BorrowMainID, A.TakeSDate, B.MainResourceID, B.BorrowAmt
                               FROM BorrowMain A
                          LEFT JOIN BorrowDevice B on B.BorrowMainID = A.BorrowMainID
                              WHERE 1 = 1
                                AND A.ActVerify IN ('02','04')
                                AND B.MainResourceID = @MainResourceID
{(model.SDate.HasValue ? " AND @SDate BETWEEN A.TakeSDate AND A.TakeEDate " : " ")}
";

            (DbExecuteInfo info, IEnumerable<FResourceBorrowResourceBorrowedModel> entitys) dbResult = DbaExecuteQuery<FResourceBorrowResourceBorrowedModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FResourceBorrowResourceBorrowedModel>();
        }

        #endregion

        public FResourceBorrowEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MainResourceID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.MainResourceID, A.MainResourceName, A.MainClass, B.Text AS MainClassText, A.SecondClass, C.Text AS SecondClassText,
									A.ShortDesc, A.BorrowRule,
                                    A.ResourceImg1, A.ResourceImg2, A.ResourceImg3, A.ResourceImg4
                               FROM BorrowMainResourceMang A
							   LEFT JOIN BorrowMainClassMang B ON B.ID = A.MainClass
							   LEFT JOIN BorrowSecondClassMang C ON C.ID = A.SecondClass
WHERE 1 = 1
AND (A.MainResourceID = @MainResourceID) ";


            (DbExecuteInfo info, IEnumerable<FResourceBorrowEditModel> entitys) dbResult = DbaExecuteQuery<FResourceBorrowEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }


        public List<FResourceBorrowEditDetailModel> GetEditDetailData(string submitBtn)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MainResourceID", submitBtn);

            #endregion

            CommandText = $@"SELECT A.MainResourceID, C.MainResourceName, A.SecondResourceNo, A.SecondResourceName, A.BorrowStatus, B.Text as BorrowStatusText, A.Memo
                               FROM BorrowSecondResourceMang A
                          LEFT JOIN code B on B.Code = A.BorrowStatus AND B.Type = 'BorrowStatus'
						  LEFT JOIN BorrowMainResourceMang C on C.MainResourceID = A.MainResourceID
                              WHERE 1 = 1
                                AND A.MainResourceID = @MainResourceID
                                AND A.ShelvesStatus = '01'
";

            (DbExecuteInfo info, IEnumerable<FResourceBorrowEditDetailModel> entitys) dbResult = DbaExecuteQuery<FResourceBorrowEditDetailModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<FResourceBorrowEditDetailModel>();
        }

        public List<SelectListItem> GetBorrowSecondClassMang()
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
    