using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Services;
using Toolbox.Tools;

namespace SqlTableMonitoring.Application
{
    internal static class OptionExtensions
    {
        private const string baseId = "SqlTableMonitoring.Configs";

        public static void Verify(this IOption option)
        {
            option.VerifyNotNull(nameof(option));

            option.Environment
                .VerifyNotEmpty("Environment is required")
                .VerifyAssert(x => x.ConvertToEnvironment() != RunEnvironment.Unknown, "Invalid environment");

            option.SqlOption.Verify();
        }

        public static void Verify(this SqlOption? sqlOption)
        {
            sqlOption.VerifyNotNull(nameof(sqlOption));

            sqlOption.ConnectionString.VerifyNotEmpty("SQL Option - Connection string is required");
            sqlOption.Password.VerifyNotEmpty("SQL Option - password is required");
        }

        public static string ConvertToResourceId(this RunEnvironment subject) => subject switch
        {
            RunEnvironment.Dev => $"{baseId}.dev-config.json",
            RunEnvironment.Acpt => $"{baseId}.acpt-config.json",
            RunEnvironment.Prod => $"{baseId}.prod-config.json",

            _ => throw new InvalidOperationException(),
        };

        public static string GetResolvedConnectionString(this SqlOption sqlOption)
        {
            sqlOption.Verify();

            return sqlOption.Resolve(sqlOption.ConnectionString);
        }

        /// <summary>
        /// Get property values for resolving properties
        /// </summary>
        /// <returns></returns>
        private static IReadOnlyList<KeyValuePair<string, string>> GetProperties(this SqlOption sqlOption) => new[]
        {
            new KeyValuePair<string, string>(nameof(sqlOption.Password), sqlOption.Password),
        };

        /// <summary>
        /// Resolve string with option's property values
        /// </summary>
        /// <param name="value">subject to resolve</param>
        /// <returns></returns>
        private static string Resolve(this SqlOption sqlOption, string value) => new PropertyResolver(sqlOption.GetProperties()).Resolve(value);
    }
}
