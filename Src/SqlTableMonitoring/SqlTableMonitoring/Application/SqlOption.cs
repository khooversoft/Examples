using System;
using System.Collections.Generic;
using System.Text;

namespace SqlTableMonitoring.Application
{
    public class SqlOption
    {
        public string ConnectionString { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
