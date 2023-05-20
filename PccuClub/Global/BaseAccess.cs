using Dapper;
using DataAccess;
using System.Data;
using System.Drawing;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using X.PagedList;

namespace WebPccuClub.Global
{
    public class BaseAccess : MsSqlDBAccess
    {
        public BaseAccess() : base()
        { }

        public BaseAccess(String ConnectionString) : base(ConnectionString)
        { }

        public StaticPagedList<T> GetPageData<T>(string SQL, DBAParameter Parameter, int? CurrentPage, int? PageSize)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DataSet ds = new DataSet();
            int thisCurrentPage = CurrentPage ?? 1;
            int thisPageSize = PageSize ?? 1;
            int TotalRow = 0;

            if (Parameter == null)
            { Parameter = new DBAParameter(); }

            Parameter.Add("Page", thisCurrentPage);
            Parameter.Add("PageSize", thisPageSize);

            ExecuteResult = DbaExecuteQuery(SQL, Parameter, ds, true, DBAccessException);

            if (ExecuteResult.isSuccess && !ds.IsNullOrEmpty())
            {
                TotalRow = int.Parse(ds.Tables[0].Rows[0]["TotalRowCount"].ToString());
                ds.Tables[0].Columns.Remove("TotalRowCount");

                IEnumerable<T> thisSource = ds.Tables[0].DataTableToEntities<T>();
                return new StaticPagedList<T>(thisSource, thisCurrentPage, thisPageSize, TotalRow);
            }
            else
            {
                List<T> empty = new List<T>();
                return new StaticPagedList<T>(empty, 1, 1, 0);
            }

        }

        public override DbExecuteInfo DbaExecuteQuery(string commandText, DBAParameter Parameters, DataSet ResultDS, bool exceptionHandle, DBAccessException ExHandle)
        {
            return base.DbaExecuteQuery(commandText, Parameters, ResultDS, exceptionHandle, ExHandle);
        }

        public override DbExecuteInfo DbaExecuteNonQuery(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle)
        {
            return base.DbaExecuteNonQuery(commandText, Parameters, exceptionHandle, ExHandle);
        }

        /// <summary> 錯誤處理 </summary>
        public void DBAccessException(DbExecuteInfo DAException)
        {
            // 直接回傳錯誤給上層處理好了...
            throw new Exception("[DAException]", DAException.SystemResult);
        }

        #region Method

        /// <summary> 取X流水號 </summary>
        public int GetXSequence()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();
            DBAParameter parameters = new DBAParameter();
            CommandText = $@"SELECT next value for　X_Sequence";

            (DbExecuteInfo info, IEnumerable<int> entitys) dbResult = DbaExecuteQuery<int>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.FirstOrDefault();

            return -1;
        }


        /// <summary>
        /// 排除特殊字元，並傳回調整後字串
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string TransSpecialChar(string Keyword)
        {
            string PaperKeyword = string.Empty;
            PaperKeyword = Keyword?.IndexOf("[") >= 0 ? Keyword.Replace("[", "[[]") : Keyword;

            return PaperKeyword;
        }


        public string QueryField(string StrTable, string StrField, string StrWhereSQL = "", string StrOrderbySQL = "")
        {
            DBAParameter parameters = new DBAParameter();
            DataSet ds = new DataSet();
            string strRtn = string.Empty;


            string CommandText = string.Format("SELECT {0} FROM {1} ", StrField, StrTable);

            if (!string.IsNullOrEmpty(StrWhereSQL))
                CommandText = CommandText + " WHERE " + StrWhereSQL;


            if (!string.IsNullOrEmpty(StrOrderbySQL))
                CommandText = CommandText + " Order By " + StrOrderbySQL;


            (DbExecuteInfo info, IEnumerable<RoleMangEditModel> entitys) dbResult = DbaExecuteQuery<RoleMangEditModel>(CommandText, parameters, true, DBAccessException);

            DbaExecuteQuery(CommandText, parameters, ds, true, DBAccessException);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRtn = ds.Tables[0].Rows[0].ToString();
                }
            }

            return strRtn;
        }

        #endregion

    }
}
