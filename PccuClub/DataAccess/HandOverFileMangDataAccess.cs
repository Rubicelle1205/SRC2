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
    
    public class HandOverFileMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<HandOverFileMangResultModel> GetSearchResult(HandOverFileMangConditionModel model)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@ClubID", model?.ClubID);
            parameters.Add("@ClubCName", model?.ClubCName);
            parameters.Add("@FromDate", model.From_ReleaseDate.HasValue ? model.From_ReleaseDate.Value.ToString("yyyy/MM/dd 00:00:00") : null);
            parameters.Add("@ToDate", model.To_ReleaseDate.HasValue ? model.To_ReleaseDate.Value.ToString("yyyy/MM/dd 23:59:59") : null);

            #endregion

            CommandText = $@"SELECT A.HoDetailID, A.HoID, A.HandOverClass, C.Text AS HandOverClassText, A.ActVerify, E.Text AS ActVerifyText,
                                    A.Created, B.ClubID, B.SchoolYear, D.ClubCName
                               FROM HandOverFileDetail A
                          LEFT JOIN HandOverMain B ON B.HoID = A.HoID
                          LEFT JOIN Code C ON C.Code = A.HandOverClass AND C.Type = 'HandOverClass'
                          LEFT JOIN ClubMang D ON D.ClubID = B.ClubID
                          LEFT JOIN Code E ON E.Code = A.ActVerify AND E.Type = 'ActVerify'
                              WHERE 1 = 1
AND (A.DataEnable = '01')
{(model.From_ReleaseDate.HasValue && model.To_ReleaseDate.HasValue ? " AND A.Created BETWEEN @FromDate AND @ToDate" : " ")}
AND (@SchoolYear IS NULL OR B.SchoolYear = @SchoolYear)
AND (@ActVerify IS NULL OR A.ActVerify = @ActVerify)
AND (@ClubID IS NULL OR B.ClubID LIKE '%' + @ClubID + '%') 
AND (@ClubCName IS NULL OR D.ClubCName LIKE '%' + @ClubCName + '%') ";


            (DbExecuteInfo info, IEnumerable<HandOverFileMangResultModel> entitys) dbResult = DbaExecuteQuery<HandOverFileMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HandOverFileMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HandOverFileMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@HoDetailID", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.HoID, A.HoDetailID, A.HoID, A.HandOverClass, C.Text AS HandOverClassText, A.ActVerify, A.VerifyMemo, A.DataEnable, 
                                    A.Created, B.ClubID, B.SchoolYear, D.ClubCName
                               FROM HandOverFileDetail A
                          LEFT JOIN HandOverMain B ON B.HoID = A.HoID
                          LEFT JOIN Code C ON C.Code = A.HandOverClass AND C.Type = 'HandOverClass'
                          LEFT JOIN ClubMang D ON D.ClubID = B.ClubID
                              WHERE 1 = 1
                                AND (A.DataEnable = '01') 
                                AND (A.HoDetailID = @HoDetailID) ";


            (DbExecuteInfo info, IEnumerable<HandOverFileMangEditModel> entitys) dbResult = DbaExecuteQuery<HandOverFileMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        public List<HandOverFileMangFileModel> GetFiles(string? HoDetailID)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@HoDetailID", HoDetailID);

            #endregion

            CommandText = $@"SELECT A.FilePath
                               FROM HandOverFile A
                              WHERE 1 = 1 AND (A.HoDetailID = @HoDetailID )";


            (DbExecuteInfo info, IEnumerable<HandOverFileMangFileModel> entitys) dbResult = DbaExecuteQuery<HandOverFileMangFileModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<HandOverFileMangFileModel>();
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(HandOverFileMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            
                parameters.Add("@ActVerify", vm.EditModel.ActVerify);
            parameters.Add("@HoDetailID", vm.EditModel.HoDetailID);
            parameters.Add("@VerifyMemo", vm.EditModel.VerifyMemo);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion 參數設定

            string CommendText = $@" UPDATE HandOverFileDetail 
                                        SET ActVerify = @ActVerify, 
                                            VerifyMemo = @VerifyMemo, 
                                            LastModifier = @LoginId, 
                                            LastModified = GETDATE() 
                                     WHERE HoDetailID = @HoDetailID";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }

        public DbExecuteInfo UpdateFile(HandOverFileMangViewModel vm, UserInfo LoginUser)
        {
            DataSet ds = new DataSet();
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            List<HandOverFileMangFileModel> dataList = vm.EditModel.LstFile;
            string HoID = vm.EditModel.HoID;
            string HoDetailID = vm.EditModel.HoDetailID;

            #endregion 參數設定

            string CommendText = $@"INSERT INTO HandOverFile
                                               (HoID, 
                                                HoDetailID,
                                                FilePath, 
                                                Creator, 
                                                Created, 
                                                LastModifier, 
                                                LastModified)
                                         VALUES
                                               ('{HoID}', 
                                                '{HoDetailID}',
                                                @FilePath, 
                                                '{LoginUser.LoginId}', 
                                                GETDATE(), 
                                                '{LoginUser.LoginId}', 
                                                GETDATE())";

            ExecuteResult = DbaExecuteNonQueryWithBulk(CommendText, dataList, false, DBAccessException, null);

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
            parameters.Add("@HoID", ser);
            #endregion 參數設定

            string CommendText = $@"DELETE FROM HandOverFileDetail WHERE HoID = @HoID ";

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
        public List<SelectListItem> GetAllActVerify()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'ActVerify' AND Code <> '05'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }


        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            for (int i = 108; i <= 130; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
        }

        
    }
}
