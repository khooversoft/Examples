using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Toolbox.Tools;

namespace SqlTableMonitoring.Application
{
    internal class Option : IOption
    {
        public string? ConfigFile { get; set; }
        public string? SecretId { get; set; }
        public string Environment { get; set; } = "dev";
        public RunEnvironment RunEnvironment { get; set; } = RunEnvironment.Unknown;

        public SqlOption SqlOption { get; set; } = null!;

        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
    }
}
