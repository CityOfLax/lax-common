﻿using System;
using System.Data.SqlClient;

namespace Lax.Data.Dapper {

    public abstract class DapperContext : IDapperContext, IDisposable {

        private readonly SqlConnection _sqlConnection;

        protected DapperContext(string connectionString) {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
        }

        public SqlConnection GetSqlConnection() =>
            _sqlConnection;

        public void Dispose() {
            _sqlConnection?.Close();
        }

    }

}
