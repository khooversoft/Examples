using Microsoft.Extensions.Logging;
using SqlTableMonitoring.Application;
using SqlTableMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTableMonitoring.Services
{
    public class SqlService
    {
        private readonly IOption _option;
        private readonly ILogger<SqlService> _logger;
        private readonly SqlConnection _sqlConnection;

        public SqlService(IOption option, ILogger<SqlService> logger)
        {
            _option = option;
            _logger = logger;

            _sqlConnection = new SqlConnection(_option.SqlOption.GetResolvedConnectionString());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public async Task<IReadOnlyList<T>> Select<T>(string query, Func<SqlDataReader, T> create, IEnumerable<SqlParameter>? sqlParameters = null)
            where T : IModel, new()
        {
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
            using SqlCommand command = new SqlCommand();

            command.Connection = _sqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            command.Parameters.AddRange(sqlParameters?.ToArray() ?? Array.Empty<SqlParameter>());

            await command.ExecuteNonQueryAsync(_option.CancellationTokenSource.Token);
        }
    }
}
