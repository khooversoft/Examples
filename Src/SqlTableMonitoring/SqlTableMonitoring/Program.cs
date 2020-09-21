using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlTableMonitoring.Application;
using SqlTableMonitoring.Services;
using SqlTableMonitoring.Workers;
using Toolbox.Tools;

namespace SqlTableMonitoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IOption option = new OptionBuilder()
                .SetArgs(args)
                .Build();

            CreateHostBuilder(args, option)
                .Build()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, IOption option) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(option);
                    services.AddSingleton<SqlService>();
                    services.AddSingleton<TableOneService>();

                    services.AddHostedService<Worker>();
                    services.AddHostedService<TableOneWorker>();
                })
                .ConfigureLogging(logger =>
                {
                    if( option.RunEnvironment == RunEnvironment.Dev)
                    {
                        logger
                            .AddConsole()
                            .AddDebug()
                            .AddFilter(x => true);
                    }
                });
    }
}
