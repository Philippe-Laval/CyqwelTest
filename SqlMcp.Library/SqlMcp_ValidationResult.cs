using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SqlMcp.Library
{
    public enum SqlMcp_ValidationSeverity
    {
        [JsonPropertyName("warning")]
        Warning,

        [JsonPropertyName("error")]
        Error
    }

    public class SqlMcp_SqlTextSpan
    {
        [JsonPropertyName("start")]
        public int Start { get; }

        [JsonPropertyName("length")]
        public int Length { get; }

        [JsonPropertyName("isEmpty")]
        public bool IsEmpty => Start < 0;


        public SqlMcp_SqlTextSpan(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public static SqlMcp_SqlTextSpan None => new SqlMcp_SqlTextSpan(-1, 0);
    }

    public class SqlMcp_ValidationLocation
    {
        [JsonPropertyName("span")]
        public SqlMcp_SqlTextSpan Span { get; }

        [JsonPropertyName("line")]
        public int Line { get; }

        [JsonPropertyName("column")]
        public int Column { get; }

        public SqlMcp_ValidationLocation(SqlMcp_SqlTextSpan span, int line, int column)
        {
            Span = span;
            Line = line;
            Column = column;
        }
    }

    public class SqlMcp_ValidationDiagnostic
    {
        [JsonPropertyName("severity")]
        public SqlMcp_ValidationSeverity Severity { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("location")]
        public SqlMcp_ValidationLocation? Location { get; set; }

        public SqlMcp_ValidationDiagnostic(SqlMcp_ValidationSeverity severity, string code, string message, SqlMcp_ValidationLocation? location = null)
        {
            Severity = severity;
            Code = code;
            Message = message;
            Location = location;
        }
    }

    public sealed class SqlMcp_ValidationResult
    {
        [JsonPropertyName("isValid")]
        public bool IsValid => Diagnostics.All((SqlMcp_ValidationDiagnostic diagnostic) => diagnostic.Severity != SqlMcp_ValidationSeverity.Error);

        [JsonPropertyName("diagnostics")]
        public IList<SqlMcp_ValidationDiagnostic> Diagnostics { get; set; }

        public SqlMcp_ValidationResult(IList<SqlMcp_ValidationDiagnostic> diagnostics)
        {
            Diagnostics = diagnostics;
        }
    }
}
