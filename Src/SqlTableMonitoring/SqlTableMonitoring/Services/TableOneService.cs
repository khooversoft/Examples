using Microsoft.Extensions.Logging;
using SqlTableMonitoring.Application;
using SqlTableMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SqlTableMonitoring.Services
{
    public class TableOneService
    {
        private readonly SqlService _sqlService;
        private readonly IOption _option;
        private readonly ILogger<TableOneService> _logger;

        public TableOneService(SqlService sqlService, IOption option, ILogger<TableOneService> logger)
        {
            _sqlService = sqlService;
            _option = option;
            _logger = logger;
        }

        public async Task<IReadOnlyList<TableOne>> Read() => await _sqlService.Select<TableOne>("select * from dbo.TableOne", TableOne.Read);

        public async Task Write(string id, string text) => await _sqlService.Exceute($"insert into dbo.TableOne (Id, Text) values ({id}, {text}");
    }
}
