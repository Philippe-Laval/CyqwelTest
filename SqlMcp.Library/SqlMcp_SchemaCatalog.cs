using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SqlMcp.Library
{
    public sealed class SqlMcp_ColumnReference
    {
        [JsonPropertyName("table")]
        public string Table { get; }

        [JsonPropertyName("column")]
        public string Column { get; }

        [JsonPropertyName("schema")]
        public string? Schema { get; }

        public SqlMcp_ColumnReference(string table, string column, string? schema = null)
        {
            Table = table;
            Column = column;
            Schema = schema;
        }
    }

    public sealed class SqlMcp_ColumnSchema
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
        public SqlMcp_ColumnReference? References { get; set; } = null;

        public SqlMcp_ColumnSchema(
            string name,
            string dataType)
        {
            Name = name;
            DataType = dataType;
        }

        public SqlMcp_ColumnSchema(
            string name,
            string dataType,
            bool? isNullable = null,
            bool isPrimaryKey = false,
            bool isUnique = false,
            SqlMcp_ColumnReference? references = null)
        {
            Name = name;
            DataType = dataType;
            IsNullable = isNullable;
            IsPrimaryKey = isPrimaryKey;
            IsUnique = isUnique;
            References = references;
        }
    }

    public sealed class SqlMcp_TableReference
    {
        [JsonPropertyName("table")]
        public string Table { get; set; }

        [JsonPropertyName("columns")]
        public List<string> Columns { get; set; }

        [JsonPropertyName("schema")]
        public string? Schema { get; set; }

        public SqlMcp_TableReference(string table, List<string> columns, string? schema = null)
        {
            Table = table;
            Columns = columns;
            Schema = schema;
        }
    }

    public sealed class SqlMcp_ForeignKey
    {
        [JsonPropertyName("columns")]
        public List<string> Columns { get; set; }

        [JsonPropertyName("references")]
        public SqlMcp_TableReference References { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public SqlMcp_ForeignKey(
            List<string> columns,
            SqlMcp_TableReference references,
            string? name = null)
        {
            Columns = columns;
            References = references;
            Name = name;
        }
    }



    public sealed class SqlMcp_TableSchema
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("columns")]
        public List<SqlMcp_ColumnSchema> Columns { get; set; }

        [JsonPropertyName("schema")]
        public string? Schema { get; set; } = null;

        [JsonPropertyName("aliases")]
        public List<string>? Aliases { get; set; } = null;

        [JsonPropertyName("primaryKey")]
        public List<string>? PrimaryKey { get; set; } = null;

        [JsonPropertyName("uniqueKeys")]
        public List<List<string>>? UniqueKeys { get; set; } = null;

        [JsonPropertyName("foreignKeys")]
        public List<SqlMcp_ForeignKey>? ForeignKeys { get; set; } = null;

        public SqlMcp_TableSchema(
            string name,
            List<SqlMcp_ColumnSchema> columns)
        {
            Name = name;
            Columns = columns;
        }

        public SqlMcp_TableSchema(
            string name,
            List<SqlMcp_ColumnSchema> columns,
            string? schema = null,
            List<string>? aliases = null,
            List<string>? primaryKey = null,
            List<List<string>>? uniqueKeys = null,
            List<SqlMcp_ForeignKey>? foreignKeys = null)
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

    public sealed class SqlMcp_SchemaCatalog
    {
        [JsonPropertyName("tables")]
        public List<SqlMcp_TableSchema> Tables { get; set; }

        public SqlMcp_SchemaCatalog(List<SqlMcp_TableSchema> tables)
        {
            Tables = tables;
        }

        public SqlMcp_SchemaCatalog(params SqlMcp_TableSchema[] tables)
            : this(new List<SqlMcp_TableSchema>(tables))
        {
        }
    }
}
