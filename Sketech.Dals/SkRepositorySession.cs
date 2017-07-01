using Sketech.Infrastructure.Configurations;
using System;
using System.Data.SqlClient;

namespace Sketech.Dals
{
    public class SkRepositorySession : IDisposable
    {
        public bool Initialized { get; private set; }

        public int SqlCommandTimeout { get; private set; }

        private SqlConnection _currentSqlConnection;

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

        public void Dispose()
        {
            _currentSqlConnection?.Dispose();
        }
    }
}