using System.Text.Json.Serialization;

namespace SqlMcp.Library
{
    public enum SqlMcp_ParseErrorCode
    {
        [JsonPropertyName("syntax")]
        Syntax,

        [JsonPropertyName("dialectIncompatible")]
        DialectIncompatible,

        [JsonPropertyName("inputTooLarge")]
        InputTooLarge,

        [JsonPropertyName("astTooLarge")]
        AstTooLarge
    }

    public class SqlMcp_ParseError
    {
        [JsonPropertyName("message")]
        public string Message { get; init; }

        [JsonPropertyName("offset")]
        public int Offset { get; init; }
       
        [JsonPropertyName("line")]
        public int Line { get; init; }
        
        [JsonPropertyName("column")]
        public int Column { get; init; }
        
        [JsonPropertyName("code")]
        public SqlMcp_ParseErrorCode Code { get; init; }
        
        public SqlMcp_ParseError(string message, int offset, int line, int column, SqlMcp_ParseErrorCode code)
        {
            Message = message;
            Offset = offset;
            Line = line;
            Column = column;
            Code = code;
        }
    }
}
