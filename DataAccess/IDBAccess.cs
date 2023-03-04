using System.Data;

namespace DataAccess
{
    public interface IDBAccess
    {
        /// <summary> 啟用 Transction 機制 (Exception 會自動 RollBack) </summary>
        DbExecuteInfo DbaInitialTransaction();

        /// <summary> Transction Commit </summary>
        DbExecuteInfo DbaCommit();

        /// <summary> Transction RollBack </summary>
        DbExecuteInfo DbaRollBack();

        /// <summary> 取得 SQL 字串 </summary>
        string GetSqlWithParameter(string commandText, DBAParameter Parameters);

        /// <summary>
        /// 執行 ExecuteQuery
        /// </summary>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="ResultDS">Result DataSet</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <returns>DbExecuteInfo</returns>
        DbExecuteInfo DbaExecuteQuery(string commandText, DBAParameter Parameters, DataSet ResultDS, bool exceptionHandle, DBAccessException ExHandle);

        /// <summary>
        /// 執行 ExecuteQuery
        /// </summary>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="ResultDS">Result DataSet</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <param name="CommandTimeOut">timeout 秒</param>
        /// <returns>DbExecuteInfo</returns>
        DbExecuteInfo DbaExecuteQuery(string commandText, DBAParameter Parameters, DataSet ResultDS, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut);

        /// <summary>
        ///  執行 ExecuteQuery
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <returns>DbExecuteInfo, IEnumerable Object</returns>
        (DbExecuteInfo, IEnumerable<T>) DbaExecuteQuery<T>(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle);

        /// <summary>
        ///  執行 ExecuteQuery
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <param name="CommandTimeOut">timeout 秒</param>
        /// <returns>DbExecuteInfo, IEnumerable Object</returns>
        (DbExecuteInfo, IEnumerable<T>) DbaExecuteQuery<T>(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut);

        /// <summary> 執行 NonQuery 作業 </summary>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <returns>DbExecuteInfo</returns>
        DbExecuteInfo DbaExecuteNonQuery(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle);

        /// <summary> 執行 NonQuery 作業 </summary>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <param name="CommandTimeOut">timeout 秒</param>
        /// <returns>DbExecuteInfo</returns>
        DbExecuteInfo DbaExecuteNonQuery(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut);

        /// <summary> SqlServer Bulk Insert By DataTable </summary>
        /// <param name="SourceDataTable">Bulk Insert Data</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <returns>DbExecuteInfo</returns>
        DbExecuteInfo BulkInsert(DataTable SourceDataTable, bool exceptionHandle, DBAccessException ExHandle);

        /// <summary> SqlServer Bulk Insert By DataTable </summary>
        /// <param name="SourceDataTable">Bulk Insert Data</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <param name="CommandTimeOut">timeout 秒</param>
        /// <returns>DbExecuteInfo</returns>
        DbExecuteInfo BulkInsert(DataTable SourceDataTable, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut);
    }
}
