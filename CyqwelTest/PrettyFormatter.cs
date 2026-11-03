using Cyqwel.Generation;
using Cyqwel.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyqwelTest
{
    public static class PrettyFormatter
    {
        public static string Format(string sql, int indentSize = 2)
        {
            var options = new SqlGenerationOptions { PrettyPrint = true, IndentSize = indentSize };
            var formatted = SqlParser.Parse(sql).ToSql(options: options);
            return formatted;
        }
    }
}
