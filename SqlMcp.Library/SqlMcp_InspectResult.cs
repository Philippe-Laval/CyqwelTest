using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SqlMcp.Library
{
    public class SqlMcp_InspectResult
    {
        [JsonPropertyName("parseError")]
        public SqlMcp_ParseError? ParseError { get; init; }

        [JsonPropertyName("tables")]
        public IReadOnlyList<string> Tables { get; init; }

        [JsonPropertyName("columns")]
        public IReadOnlyList<string> Columns { get; init; }

        [JsonPropertyName("isValid")]
        public bool IsValid { get; init; }

        public SqlMcp_InspectResult(bool isValid, IReadOnlyList<string> tables, IReadOnlyList<string> columns, SqlMcp_ParseError? parseError)
        {
            IsValid = isValid;
            Tables = tables;
            Columns = columns;
            ParseError = parseError;
        }
    }
}
