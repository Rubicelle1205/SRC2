using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebPccuClub.DataAccess
{
    
    public class BorrowMailTemplateMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<BorrowMailTemplateMangResultModel> GetSearchResult(BorrowMailTemplateMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();


            #region 參數設定

            parameters.Add("@BorrowMainClassID", model?.BorrowMainClassID);
            parameters.Add("@TemplateName", model?.TemplateName);
            parameters.Add("@IsEnable", model?.IsEnable);
            parameters.Add("@FromDate", model?.From_ReleaseDate?.Date);
            parameters.Add("@ToDatePlusOne", model?.To_ReleaseDate?.Date.AddDays(1));

            #endregion

            CommandText = $@"SELECT A.TemplateId, A.TemplateName, A.SubjectTemplate, 
                                    A.BorrowMainClassID, B.Text AS BorrowMainClassIDText, 
                                    A.BodyTemplate, A.IsEnable, C.Text AS IsEnableText, A.Created
				               FROM BorrowMailTemplate A
				          LEFT JOIN BorrowMainClassMang B ON B.ID = A.BorrowMainClassID
				          LEFT JOIN Code C ON C.Code = A.IsEnable AND C.Type = 'Enable'
				              WHERE 1 = 1
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created >= @FromDate AND A.Created < @ToDatePlusOne" : "")}

AND (@TemplateName IS NULL OR A.TemplateName LIKE '%' + @TemplateName + '%') 
AND (@BorrowMainClassID IS NULL OR A.BorrowMainClassID = @BorrowMainClassID)
AND (@IsEnable IS NULL OR A.IsEnable = @IsEnable)
";


            (DbExecuteInfo info, IEnumerable<BorrowMailTemplateMangResultModel> entitys) dbResult = DbaExecuteQuery<BorrowMailTemplateMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<BorrowMailTemplateMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BorrowMailTemplateMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT TemplateId, TemplateName, BorrowMainClassID, SubjectTemplate, BodyTemplate, IsEnable, Creator, Created, 
                   LastModifier, LastModified
FROM BorrowMailTemplate
WHERE 1 = 1
AND (TemplateId = @ID) ";


            (DbExecuteInfo info, IEnumerable<BorrowMailTemplateMangEditModel> entitys) dbResult = DbaExecuteQuery<BorrowMailTemplateMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 新增資料 </summary>
        public DbExecuteInfo InsertData(BorrowMailTemplateMangViewModel vm, UserInfo LoginUser)
        {

            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@TemplateName", vm.CreateModel.TemplateName);
            parameters.Add("@BorrowMainClassID", vm.CreateModel.BorrowMainClassID);
            parameters.Add("@SubjectTemplate", vm.CreateModel.SubjectTemplate);
            parameters.Add("@BodyTemplate", vm.CreateModel.BodyTemplate); 
            parameters.Add("@IsEnable", vm.CreateModel.IsEnable);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"INSERT INTO BorrowMailTemplate
                                               (
TemplateName, 
BorrowMainClassID, 
SubjectTemplate, 
BodyTemplate, 
IsEnable, 
Creator, 
Created,
LastModifier, 
LastModified
)
                                         VALUES
                                               (
@TemplateName,
@BorrowMainClassID, 
@SubjectTemplate, 
@BodyTemplate, 
@IsEnable, 
@LoginId,
GETDATE(),
@LoginId,
GETDATE()
)";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(BorrowMailTemplateMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@ID", vm.EditModel.TemplateId);

            parameters.Add("@TemplateName", vm.EditModel.TemplateName);
            parameters.Add("@BorrowMainClassID", vm.EditModel.BorrowMainClassID);
            parameters.Add("@SubjectTemplate", vm.EditModel.SubjectTemplate);
            parameters.Add("@BodyTemplate", vm.EditModel.BodyTemplate);
            parameters.Add("@IsEnable", vm.EditModel.IsEnable);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@"UPDATE BorrowMailTemplate 
                                       SET 
TemplateName = @TemplateName, 
BorrowMainClassID = @BorrowMainClassID, 
SubjectTemplate = @SubjectTemplate, 
BodyTemplate = @BodyTemplate, 
IsEnable = @IsEnable, 
LastModifier = @LoginId, 
LastModified = GETDATE()
                                     WHERE TemplateId = @ID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
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

            string CommendText = $@"DELETE FROM BorrowMailTemplate WHERE TemplateId = @ID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public List<SelectListItem> GetddlEnable()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS Value, Text AS Text FROM Code WHERE Type = 'Enable'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<SelectListItem> GetddlMainClass()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT ID AS Value, Text AS Text FROM BorrowMainClassMang";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }
    }
}
