using Sketech.Infrastructure.Configurations;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sketech.Dals
{
    public class SkRepositorySession : IDisposable
    {
        public bool Initialized { get; private set; }

        public int SqlCommandTimeout { get; private set; }

        private SqlConnection _currentSqlConnection;

        private SqlTransaction _currentTrasaction;

        public SqlConnection GetConnection()
        {
            if (_currentSqlConnection != null)
            {
                return _currentSqlConnection;
            }

            // get command timeout
            SqlCommandTimeout = SkConfiguration.Current.AppSettings.SqlCommandTimeout;

            // get connection string
            var connStr = SkConfiguration.Current.ConnectionStrings.Primary;

            // create sql connection and return
            _currentSqlConnection = new SqlConnection(connStr);
            return _currentSqlConnection;
        }

        public bool IsOpenedTransaction
        {
            get { return _currentTrasaction != null; }
        }

        public void OpenTransaction(IsolationLevel isolaTionLevel = IsolationLevel.ReadCommitted)
        {
            if(_currentSqlConnection == null)
            {
                return;
            }

            if(_currentTrasaction != null)
            {
                return;
            }

            if (_currentSqlConnection.State == ConnectionState.Open)
            {
                _currentSqlConnection.Close();
            }

            _currentTrasaction = _currentSqlConnection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CommitTransaction()
        {
            if(_currentTrasaction == null)
            {
                return;
            }

            _currentTrasaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (_currentTrasaction == null)
            {
                return;
            }

            _currentTrasaction.Rollback();
        }

        public void SaveTransaction(string savePointName)
        {
            if (_currentTrasaction == null)
            {
                return;
            }

            _currentTrasaction.Save(savePointName);
        }

        public void Dispose()
        {
            _currentTrasaction?.Dispose();
            _currentSqlConnection?.Dispose();
        }
    }
}