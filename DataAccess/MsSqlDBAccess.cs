using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Dapper;

namespace DataAccess
{
    public class MsSqlDBAccess : DataAccessBase, IDBAccess
    {
        /// <summary> 是否使用 Event handle 的方式 </summary>
        public bool UsingEvent = false;

        public MsSqlDBAccess() : base() {; }

        public MsSqlDBAccess(string ConnectionString) : base(ConnectionString) {; }

        private void DbaRunException(DbExecuteInfo ExceptionInfo, DBAccessException ExHandle)
        {
            //要處理 exception
            if (UsingEvent)
            {
                if (ExHandle != null)
                    ExHandle(ExceptionInfo);
                else
                {
                    throw ExceptionInfo.SystemResult;
                }
            }
        }

        /// <summary>
        /// 執行 ExecuteQuery
        /// </summary>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="ResultDS">Result DataSet</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <returns>DbExecuteInfo</returns>
        public virtual DbExecuteInfo DbaExecuteQuery(string commandText, DBAParameter Parameters, DataSet ResultDS, bool exceptionHandle, DBAccessException ExHandle)
        {
            return DbaExecuteQuery(commandText, Parameters, ResultDS, exceptionHandle, ExHandle, null);
        }

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
        public virtual DbExecuteInfo DbaExecuteQuery(string commandText, DBAParameter Parameters, DataSet ResultDS, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            UsingEvent = exceptionHandle;

            try
            {
                IDataReader reader = this._connection.ExecuteReader(commandText, Parameters.GetInstance(), Transaction, CommandTimeOut);

                ConvertDataReaderToDataSet(reader, ResultDS);

                if (ResultDS == null)
                {
                    ExecuteResult.isSuccess = false;
                    ExecuteResult.ErrorCode = dbErrorCode._EC_NotMatchData;
                }
                else
                {
                    ExecuteResult.isSuccess = true;
                    ExecuteResult.ErrorCode = 0;
                    foreach (DataTable dt in ResultDS.Tables)
                    {
                        ExecuteResult.AffectRowCount += dt.Rows.Count;
                    }
                }
            }
            catch (DbException dbE)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = dbE;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (Exception ex)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = ex;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }

            return ExecuteResult;
        }


        /// <summary>
        /// 執行 ExecuteQuery
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <returns>DbExecuteInfo</returns>
        public virtual (DbExecuteInfo, IEnumerable<T>) DbaExecuteQuery<T>(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle)
        {
            return DbaExecuteQuery<T>(commandText, Parameters, exceptionHandle, ExHandle, null);
        }

        /// <summary>
        /// 執行 ExecuteQuery
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <param name="CommandTimeOut">timeout 秒</param>
        /// <returns>DbExecuteInfo</returns>
        public virtual (DbExecuteInfo, IEnumerable<T>) DbaExecuteQuery<T>(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            UsingEvent = exceptionHandle;

            try
            {
                IEnumerable<T> entitys = this._connection.Query<T>(commandText.Trim(), Parameters.GetInstance(), Transaction, true, CommandTimeOut);

                if (entitys == null)
                {
                    ExecuteResult.isSuccess = false;
                    ExecuteResult.ErrorCode = dbErrorCode._EC_NotMatchData;
                }
                else
                {
                    ExecuteResult.isSuccess = true;
                    ExecuteResult.ErrorCode = 0;
                }

                return (ExecuteResult, entitys);
            }
            catch (DbException dbE)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = dbE;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (Exception ex)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = ex;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }

            return (ExecuteResult, null);
        }

        /// <summary>
        /// 執行 ExecuteQuery
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <param name="CommandTimeOut">timeout 秒</param>
        /// <param name="commandtype">Command Type</param>
        /// <returns>DbExecuteInfo</returns>
        public virtual (DbExecuteInfo, IEnumerable<T>) DbaExecuteQueryWithSP<T>(string commandText, DynamicParameters Parameters, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut, CommandType commandtype)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            UsingEvent = exceptionHandle;

            try
            {
                IEnumerable<T> entitys = this._connection.Query<T>(commandText.Trim(), Parameters, Transaction, true, CommandTimeOut, commandtype);

                if (entitys == null)
                {
                    ExecuteResult.isSuccess = false;
                    ExecuteResult.ErrorCode = dbErrorCode._EC_NotMatchData;
                }
                else
                {
                    ExecuteResult.isSuccess = true;
                    ExecuteResult.ErrorCode = 0;
                }

                return (ExecuteResult, entitys);
            }
            catch (DbException dbE)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = dbE;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (Exception ex)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = ex;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }

            return (ExecuteResult, null);
        }

        /// <summary> 執行 NonQuery 作業 </summary>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <returns>DbExecuteInfo</returns>
        public virtual DbExecuteInfo DbaExecuteNonQuery(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle)
        {
            return DbaExecuteNonQuery(commandText, Parameters, exceptionHandle, ExHandle, null);
        }

        /// <summary> 執行 NonQuery 作業 </summary>
        /// <param name="commandText">SQL Text</param>
        /// <param name="Parameters">SQL Parameter</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <param name="CommandTimeOut">timeout 秒</param>
        /// <returns>DbExecuteInfo</returns>
        public virtual DbExecuteInfo DbaExecuteNonQuery(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            UsingEvent = exceptionHandle;
            int count = 0;

            try
            {
                count = this._connection.Execute(commandText, Parameters.GetInstance(), Transaction, CommandTimeOut);
                ExecuteResult.AffectRowCount = count;

                if (count > 0)
                {
                    //成功更新
                    ExecuteResult.isSuccess = true;
                }
                else
                {
                    //無資料更新
                    ExecuteResult.isSuccess = false;
                    ExecuteResult.ErrorCode = dbErrorCode._EC_NotAffect;
                }
            }
            catch (SqlException sqlE)
            {
                switch (sqlE.Number)
                {
                    case 2627:
                    case 2601:
                        ExecuteResult.ErrorCode = dbErrorCode._EC_UniqueViolation;
                        break;
                    default:
                        break;
                }

                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = sqlE;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (DbException dbE)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = dbE;

                //發生例外若有使用 transction 則自動 RollBack
                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (Exception ex)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = ex;

                //發生例外若有使用 transction 則自動 RollBack
                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }

            return ExecuteResult;
        }

        public virtual int DbaExecuteQuerySingle(string commandText, DBAParameter Parameters, bool exceptionHandle, DBAccessException ExHandle)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            UsingEvent = exceptionHandle;
            int count = 0;
            int identity = -1;
            try
            {
                identity = this._connection.QuerySingle<int>(commandText, Parameters.GetInstance());
                ExecuteResult.AffectRowCount = 1;

                if (count > 0)
                {
                    //成功更新
                    ExecuteResult.isSuccess = true;
                }
                else
                {
                    //無資料更新
                    ExecuteResult.isSuccess = false;
                    ExecuteResult.ErrorCode = dbErrorCode._EC_NotAffect;
                }
            }
            catch (SqlException sqlE)
            {
                switch (sqlE.Number)
                {
                    case 2627:
                    case 2601:
                        ExecuteResult.ErrorCode = dbErrorCode._EC_UniqueViolation;
                        break;
                    default:
                        break;
                }

                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = sqlE;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (DbException dbE)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = dbE;

                //發生例外若有使用 transction 則自動 RollBack
                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (Exception ex)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = ex;

                //發生例外若有使用 transction 則自動 RollBack
                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }

            return identity;
        }

        public virtual DbExecuteInfo DbaExecuteNonQueryWithBulk(string commandText, object Parameters, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            UsingEvent = exceptionHandle;
            int count = 0;

            try
            {
                count = this._connection.Execute(commandText, Parameters, Transaction, CommandTimeOut);

                ExecuteResult.AffectRowCount = count;

                if (count > 0)
                {
                    //成功更新
                    ExecuteResult.isSuccess = true;
                }
                else
                {
                    //無資料更新
                    ExecuteResult.isSuccess = false;
                    ExecuteResult.ErrorCode = dbErrorCode._EC_NotAffect;
                }
            }
            catch (SqlException sqlE)
            {
                switch (sqlE.Number)
                {
                    case 2627:
                    case 2601:
                        ExecuteResult.ErrorCode = dbErrorCode._EC_UniqueViolation;
                        break;
                    default:
                        break;
                }

                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = sqlE;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (DbException dbE)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = dbE;

                //發生例外若有使用 transction 則自動 RollBack
                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (Exception ex)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = ex;

                //發生例外若有使用 transction 則自動 RollBack
                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }

            return ExecuteResult;
        }

        /// <summary> SqlServer Bulk Insert By DataTable </summary>
        /// <param name="SourceDataTable">Bulk Insert Data</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <returns>DbExecuteInfo</returns>
        public virtual DbExecuteInfo BulkInsert(DataTable SourceDataTable, bool exceptionHandle, DBAccessException ExHandle)
        {
            return BulkInsert(SourceDataTable, exceptionHandle, ExHandle, null);
        }

        /// <summary> SqlServer Bulk Insert By DataTable </summary>
        /// <param name="SourceDataTable">Bulk Insert Data</param>
        /// <param name="exceptionHandle">是否處理錯誤(True:throw Exception，False:not throw Exception)</param>
        /// <param name="ExHandle">Exception Delegate Function</param>
        /// <param name="CommandTimeOut">timeout 秒</param>
        /// <returns>DbExecuteInfo</returns>
        public virtual DbExecuteInfo BulkInsert(DataTable SourceDataTable, bool exceptionHandle, DBAccessException ExHandle, int? CommandTimeOut)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            UsingEvent = exceptionHandle;

            try
            {
                if (SourceDataTable == null || SourceDataTable.Rows.Count == 0)
                { throw new Exception("NullReferenceException: DataTable is null or empty!"); }

                if (string.IsNullOrWhiteSpace(SourceDataTable.TableName))
                { throw new Exception("NullReferenceException: DataTable name is empty!"); }

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                SqlBulkCopy dbBulk = null;

                if (_iniTransction)
                    dbBulk = new SqlBulkCopy(base._connection as SqlConnection, SqlBulkCopyOptions.KeepIdentity, Transaction as SqlTransaction);
                else
                    dbBulk = new SqlBulkCopy(base._connection as SqlConnection);

                if (CommandTimeOut != null)
                { dbBulk.BulkCopyTimeout = (int)CommandTimeOut; }

                dbBulk.BatchSize = 100000;

                dbBulk.DestinationTableName = SourceDataTable.TableName;
                foreach (var column in SourceDataTable.Columns)
                { dbBulk.ColumnMappings.Add(column.ToString(), column.ToString()); }

                dbBulk.WriteToServer(SourceDataTable);

                ExecuteResult.AffectRowCount = SourceDataTable.Rows.Count;

                //成功更新
                ExecuteResult.isSuccess = true;
            }
            catch (SqlException sqlE)
            {
                switch (sqlE.Number)
                {
                    case 2627:
                    case 2601:
                        ExecuteResult.ErrorCode = dbErrorCode._EC_UniqueViolation;
                        break;
                    default:
                        break;
                }

                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = sqlE;

                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (DbException dbE)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = dbE;

                //發生例外若有使用 transction 則自動 RollBack
                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            catch (Exception ex)
            {
                ExecuteResult.isSuccess = false;
                ExecuteResult.SystemResult = ex;

                //發生例外若有使用 transction 則自動 RollBack
                if (_iniTransction)
                    base.RollbackExt();

                DbaRunException(ExecuteResult, ExHandle);
            }
            finally
            {
                if (this._transaction == null)
                {
                    _connection.Close();
                }
            }

            return ExecuteResult;
        }

        /// <summary> 取得 SQL 字串(將 Parameter 代入 SQL) </summary>
        public string GetSqlWithParameter(string commandText, DBAParameter Parameters)
        {
            if (Parameters == null)
            { return string.Empty; }

            foreach (var name in Parameters.GetInstance().ParameterNames)
            {
                string Value = Parameters.GetInstance().Get<dynamic>(name);

                //if (string.IsNullOrEmpty(Value))
                //{ continue; }

                if (string.IsNullOrEmpty(Value))
                {
                    commandText = Regex.Replace(commandText, string.Format(@"@\b{0}\b", name), "null");
                }
                else
                {
                    commandText = Regex.Replace(commandText, string.Format(@"@\b{0}\b", name), string.Format("'{0}'", Value));
                }
            }

            return commandText;
        }

    }
}
