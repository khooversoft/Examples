using Microsoft.Extensions.Logging;
using SqlTableMonitoring.Application;
using SqlTableMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlTableMonitoring.Services
{
    public class SqlService : IDisposable
    {
        private readonly IOption _option;
        private readonly ILogger<SqlService> _logger;
        private SqlConnection _sqlConnection;

        public SqlService(IOption option, ILogger<SqlService> logger)
        {
            _option = option;
            _logger = logger;

            _sqlConnection = new SqlConnection(_option.SqlOption.GetResolvedConnectionString());
            _sqlConnection.Open();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public async Task<IReadOnlyList<T>> Select<T>(string query, Func<SqlDataReader, T> create, IEnumerable<SqlParameter>? sqlParameters = null)
            where T : IModel, new()
        {
            _logger.LogInformation($"{nameof(Select)}: executing and reading: {query}");
            using SqlCommand command = new SqlCommand();

            command.Connection = _sqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            command.Parameters.AddRange(sqlParameters?.ToArray() ?? Array.Empty<SqlParameter>());

            using SqlDataReader reader = await command.ExecuteReaderAsync(_option.CancellationTokenSource.Token);

            var list = new List<T>();
            while (await reader.ReadAsync(_option.CancellationTokenSource.Token))
            {
                list.Add(create(reader));
            }

            return list;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public async Task Exceute(string query, IEnumerable<SqlParameter>? sqlParameters = null)
        {
            _logger.LogInformation($"{nameof(Exceute)}: executing: {query}");

            using SqlCommand command = new SqlCommand();

            command.Connection = _sqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            command.Parameters.AddRange(sqlParameters?.ToArray() ?? Array.Empty<SqlParameter>());

            await command.ExecuteNonQueryAsync(_option.CancellationTokenSource.Token);
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _sqlConnection, null!)?.Close();
        }
    }
}
