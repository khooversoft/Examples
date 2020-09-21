using System;
using System.Collections.Generic;
using System.Text;

namespace Toolbox.Tools
{
    public enum RunEnvironment
    {
        Unknown,
        Dev,
        Acpt,
        Prod
    }

    public static class RunEnvironmentExtensions
    {
        public static RunEnvironment ConvertToEnvironment(this string subject)
        {
            Enum.TryParse(subject, true, out RunEnvironment enviornment)
                .VerifyAssert(x => x == true, $"Invalid environment {subject}");

            return enviornment;
        }
    }
}
