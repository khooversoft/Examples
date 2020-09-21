using System.Threading;
using Toolbox.Tools;

namespace SqlTableMonitoring.Application
{
    public interface IOption
    {
        CancellationTokenSource CancellationTokenSource { get; }
        string? ConfigFile { get; }
        string Environment { get; }
        RunEnvironment RunEnvironment { get; }
        string? SecretId { get; }
        SqlOption SqlOption { get; }
    }
}