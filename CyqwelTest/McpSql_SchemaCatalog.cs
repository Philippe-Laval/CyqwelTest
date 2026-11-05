using System.Text.Json.Serialization;

namespace CyqwelTest
{
    public sealed class McpSql_ColumnReference
    {
        [JsonPropertyName("table")]
        public string Table { get; }

        [JsonPropertyName("column")]
        public string Column { get; }

        [JsonPropertyName("schema")]
        public string? Schema { get; }

        public McpSql_ColumnReference(string table, string column, string? schema = null)
        {
            Table = table;
            Column = column;
            Schema = schema;
        }
    }

    public sealed class McpSql_ColumnSchema
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("dataType")]
        public string DataType { get; set; }

        [JsonPropertyName("isNullable")]
        public bool? IsNullable { get; set; } = null;

        [JsonPropertyName("isPrimaryKey")]
        public bool IsPrimaryKey { get; set; } = false;

        [JsonPropertyName("isUnique")]
        public bool IsUnique { get; set; } = false;

        [JsonPropertyName("references")]
        public McpSql_ColumnReference? References { get; set; } = null;

        public McpSql_ColumnSchema(
            string name,
            string dataType)
        {
            Name = name;
            DataType = dataType;
        }

        public McpSql_ColumnSchema(
            string name,
            string dataType,
            bool? isNullable = null,
            bool isPrimaryKey = false,
            bool isUnique = false,
            McpSql_ColumnReference? references = null)
        {
            Name = name;
            DataType = dataType;
            IsNullable = isNullable;
            IsPrimaryKey = isPrimaryKey;
            IsUnique = isUnique;
            References = references;
        }
    }

    public sealed class McpSql_TableReference
    {
        [JsonPropertyName("table")]
        public string Table { get; set; }

        [JsonPropertyName("columns")]
        public List<string> Columns { get; set; }

        [JsonPropertyName("schema")]
        public string? Schema { get; set; }

        public McpSql_TableReference(string table, List<string> columns, string? schema = null)
        {
            Table = table;
            Columns = columns;
            Schema = schema;
        }
    }

    public sealed class McpSql_ForeignKey
    {
        [JsonPropertyName("columns")]
        public List<string> Columns { get; set; }

        [JsonPropertyName("references")]
        public McpSql_TableReference References { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public McpSql_ForeignKey(
            List<string> columns,
            McpSql_TableReference references,
            string? name = null)
        {
            Columns = columns;
            References = references;
            Name = name;
        }
    }



    public sealed class McpSql_TableSchema
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("columns")]
        public List<McpSql_ColumnSchema> Columns { get; set; }

        [JsonPropertyName("schema")]
        public string? Schema { get; set; } = null;

        [JsonPropertyName("aliases")]
        public List<string>? Aliases { get; set; } = null;

        [JsonPropertyName("primaryKey")]
        public List<string>? PrimaryKey { get; set; } = null;

        [JsonPropertyName("uniqueKeys")]
        public List<List<string>>? UniqueKeys { get; set; } = null;

        [JsonPropertyName("foreignKeys")]
        public List<McpSql_ForeignKey>? ForeignKeys { get; set; } = null;

        public McpSql_TableSchema(
            string name,
            List<McpSql_ColumnSchema> columns)
        {
            Name = name;
            Columns = columns;
        }

        public McpSql_TableSchema(
            string name,
            List<McpSql_ColumnSchema> columns,
            string? schema = null,
            List<string>? aliases = null,
            List<string>? primaryKey = null,
            List<List<string>>? uniqueKeys = null,
            List<McpSql_ForeignKey>? foreignKeys = null)
        {
            Name = name;
            Columns = columns;
            Schema = schema;
            Aliases = aliases;
            PrimaryKey = primaryKey;
            UniqueKeys = uniqueKeys;
            ForeignKeys = foreignKeys;
        }
    }

    public sealed class McpSql_SchemaCatalog
    {
        [JsonPropertyName("tables")]
        public List<McpSql_TableSchema> Tables { get; set; }

        public McpSql_SchemaCatalog(List<McpSql_TableSchema> tables)
        {
            Tables = tables;
        }

        public McpSql_SchemaCatalog(params McpSql_TableSchema[] tables)
            : this(new List<McpSql_TableSchema>(tables))
        {
        }
    }
}
