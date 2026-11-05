using Cyqwel.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
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
        public IReadOnlyList<string> Columns { get; set; }

        [JsonPropertyName("schema")]
        public string? Schema { get; set; }

        public McpSql_TableReference(string table, IList<string> columns, string? schema = null)
        {
            Table = table;
            Columns = new List<string>(columns);
            Schema = schema;
        }
    }

    public sealed class McpSql_ForeignKey
    {
        [JsonPropertyName("columns")]
        List<string> Columns { get; set; }

        [JsonPropertyName("references")]
        McpSql_TableReference References { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public McpSql_ForeignKey(
            IList<string> columns,
            McpSql_TableReference references,
            string? name = null)
        {
            Columns = new List<string>(columns);
            References = references;
            Name = name;
        }
    }

    

    public sealed class McpSql_TableSchema
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("columns")]
        public IList<McpSql_ColumnSchema> Columns { get; set; }

        [JsonPropertyName("schema")]
        public string? Schema { get; set; } = null;

        [JsonPropertyName("aliases")]
        public IList<string>? Aliases { get; set; } = null;

        [JsonPropertyName("primaryKey")]
        public IList<string>? PrimaryKey { get; set; } = null;

        [JsonPropertyName("uniqueKeys")]
        public IList<IList<string>>? UniqueKeys { get; set; } = null;

        [JsonPropertyName("foreignKeys")]
        public IList<McpSql_ForeignKey>? ForeignKeys { get; set; } = null;

        public McpSql_TableSchema(
            string name,
            IList<McpSql_ColumnSchema> columns)
        {
            Name = name;
            Columns = columns;
        }

        public McpSql_TableSchema(
            string name,
            IList<McpSql_ColumnSchema> columns,
            string? schema = null,
            IList<string>? aliases = null,
            IList<string>? primaryKey = null,
            IList<IList<string>>? uniqueKeys = null,
            IList<McpSql_ForeignKey>? foreignKeys = null)
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
        public IList<McpSql_TableSchema> Tables { get; set; }

        public McpSql_SchemaCatalog(IList<McpSql_TableSchema> tables)
        {
            Tables = tables;
        }

        public McpSql_SchemaCatalog(params McpSql_TableSchema[] tables)
            : this((IList<McpSql_TableSchema>)tables)
        {
        }
    }

    public static class McpSql_SchemaCatalogExtensions
    {
        public static void AddTable(this McpSql_SchemaCatalog catalog, McpSql_TableSchema table)
        {
            catalog.Tables.Add(table);
        }

        public static string SerializeToJson(this McpSql_SchemaCatalog catalog)
        {
            return System.Text.Json.JsonSerializer.Serialize(catalog, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        public static McpSql_SchemaCatalog DeserializeFromJson(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<McpSql_SchemaCatalog>(json) ?? new McpSql_SchemaCatalog(new List<McpSql_TableSchema>());
        }

    }


    public sealed class SqlSchemaCatalogGenerator
    {
        public McpSql_SchemaCatalog Generate()
        {
            var catalog = new McpSql_SchemaCatalog(new List<McpSql_TableSchema>());

            return catalog;
        }

        public McpSql_SchemaCatalog GenerateSqlTableSchema()
        {
            IList<McpSql_TableSchema> tables = new List<McpSql_TableSchema>();
            

            var SqlTableSchema = new McpSql_SchemaCatalog(tables);

            return SqlTableSchema;
        }


        public static McpSql_SchemaCatalog CreateCatalog(bool withRelationship = false) =>
        new McpSql_SchemaCatalog(
            new McpSql_TableSchema(
                "users",
                new List<McpSql_ColumnSchema>
                {
                    new McpSql_ColumnSchema("id", "integer", isPrimaryKey: true),
                    new McpSql_ColumnSchema("name", "varchar"),
                    new McpSql_ColumnSchema("email", "varchar"),
                    new McpSql_ColumnSchema("age", "integer"),
                    new McpSql_ColumnSchema("active", "boolean"),
                },
                primaryKey: new List<string> { "id" }),
            new McpSql_TableSchema(
                "orders",
                new List<McpSql_ColumnSchema>
                {
                    new McpSql_ColumnSchema("id", "integer", isPrimaryKey: true),
                    new McpSql_ColumnSchema(
                        "user_id",
                        "integer",
                        references : withRelationship ? new McpSql_ColumnReference("users", "id") : null),
                    new McpSql_ColumnSchema("total", "decimal"),
                    new McpSql_ColumnSchema("description", "varchar"),
                },
                primaryKey: new List<string> { "id" }));
    }
}
