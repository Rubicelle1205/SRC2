using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{

    public abstract class DataAccessCaseBase
    {
        #region 屬性
        private DbExecuteInfo _ExecuteResult { get; set; }
        internal DbConnection _connection { get; set; }

        internal DbTransaction _transaction;

        //使用 Transction ( true : using transction )
        internal bool _iniTransction = false;
        //使用 Transction 發生錯誤( true : Exception )
        internal bool _isTransctionFail = false;

        internal string _ConnectionString { get; set; }

        internal DbTransaction Transaction
        {
            set { _transaction = value; }
            get { return _transaction; }
        }

        #endregion 屬性

        public DataAccessCaseBase()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            IConfiguration config = builder.Build();
            this._ConnectionString = config.GetValue<string>("ConnectionString:CaseDatabase");
            this._connection = new SqlConnection(this._ConnectionString);
        }

        public DataAccessCaseBase(string ConnectionString)
        {
            this._ConnectionString = ConnectionString;
            this._connection = new SqlConnection(this._ConnectionString);
        }

        #region Transction

        /// <summary> 啟用 Transction 機制 </summary>
        internal DbExecuteInfo InitialTransactionConnection()
        {
            _ExecuteResult = new DbExecuteInfo();

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                if (_transaction == null || _transaction.Connection == null)
                    _transaction = _connection.BeginTransaction();
                _ExecuteResult.isSuccess = true;
                _iniTransction = true;
            }
            catch (DbException dbE)
            {
                _ExecuteResult.SystemResult = dbE;
                _ExecuteResult.isSuccess = false;
            }
            catch (Exception ex)
            {
                _ExecuteResult.SystemResult = ex;
                _ExecuteResult.isSuccess = false;
            }
            return _ExecuteResult;
        }

        /// <summary> Transction CommitExt </summary>
        internal DbExecuteInfo CommitExt()
        {
            _ExecuteResult = new DbExecuteInfo();

            try
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    _iniTransction = false;
                    _isTransctionFail = false;
                }

                if (_connection.State == ConnectionState.Open)
                    _connection.Close();

                _ExecuteResult.isSuccess = true;
            }
            catch (DbException dbE)
            {
                _ExecuteResult.SystemResult = dbE;
                _ExecuteResult.isSuccess = false;
            }
            catch (Exception ex)
            {

                _ExecuteResult.SystemResult = ex;
                _ExecuteResult.isSuccess = false;
            }
            return _ExecuteResult;
        }

        /// <summary> Transction RollbackExt </summary>
        internal DbExecuteInfo RollbackExt()
        {
            _ExecuteResult = new DbExecuteInfo();

            try
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _iniTransction = false;
                    _isTransctionFail = false;
                }

                _ExecuteResult.isSuccess = true;
            }
            catch (DbException dbE)
            {
                _ExecuteResult.SystemResult = dbE;
                _ExecuteResult.isSuccess = false;
            }
            catch (Exception ex)
            {

                _ExecuteResult.SystemResult = ex;
                _ExecuteResult.isSuccess = false;
            }
            return _ExecuteResult;
        }

        /// <summary> 啟用 Transction 機制 (Exception 會自動 RollBack) </summary>
        public DbExecuteInfo DbaInitialTransaction()
        {
            return this.InitialTransactionConnection();
        }

        /// <summary>  Transction Commit </summary>
        public DbExecuteInfo DbaCommit()
        {
            return this.CommitExt();
        }

        /// <summary> Transction RollBack </summary>
        public DbExecuteInfo DbaRollBack()
        {
            return this.RollbackExt();
        }

        #endregion Transction

        /// <summary> Convert IDataReader to DataSet </summary>
        /// <param name="data"></param>
        /// <param name="ResultDS">ref DataSet</param>
        /// <returns></returns>
        internal DataSet ConvertDataReaderToDataSet(IDataReader data, DataSet ResultDS)
        {
            if (ResultDS == null)
            { ResultDS = new DataSet(); }

            int tableIdx = ResultDS.Tables.Count == 0 ? 0 : ResultDS.Tables.Count;
            while (!data.IsClosed)
            {
                ResultDS.Tables.Add("Table" + (tableIdx + 1));
                ResultDS.EnforceConstraints = false;
                ResultDS.Tables[tableIdx].Load(data);
                tableIdx++;
            }

            return ResultDS;
        }
    }
}
