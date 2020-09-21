using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SqlTableMonitoring.Model
{
    public class TableOne : IModel
    {
        public string? Id { get; set; }

        public string? Text { get; set; }

        public static TableOne Read(SqlDataReader sqlDataReader)
        {
            return new TableOne
            {
                Id = (string)sqlDataReader[nameof(Id)],
                Text = (string)sqlDataReader[nameof(Text)],
            };
        }
    }
}
