using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlTableMonitoring.Model;
using SqlTableMonitoring.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toolbox.Extensions;

namespace SqlTableMonitoring.Workers
{
    public class TableOneWorker : BackgroundService
    {
        private readonly TableOneService _tableOneService;
        private readonly ILogger<TableOneWorker> _logger;

        public TableOneWorker(TableOneService tableOneService, ILogger<TableOneWorker> logger)
        {
            _tableOneService = tableOneService;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            stoppingToken.Register(() => tcs.SetResult(true));

            var tasks = new[]
            {
                Read(stoppingToken),
                Write(stoppingToken),
            };

            return Task.WhenAll(tasks.Append(tcs.Task));
        }

        private async Task Read(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IReadOnlyList<TableOne> results = await _tableOneService.Read();

                results
                    .ForEach(x => _logger.LogInformation($"{nameof(Read)}: Id={x.Id}, Text={x.Text}"));
            }
        }

        private async Task Write(CancellationToken stoppingToken)
        {
            int count = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                await _tableOneService.Write(count.ToString(), $"Text_{count}");

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
