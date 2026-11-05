using Cyqwel.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace CyqwelTest
{
    


    public sealed class SqlSchemaCatalogGenerator
    {
        public McpSql_SchemaCatalog Generate()
        {
            var catalog = new McpSql_SchemaCatalog(new List<McpSql_TableSchema>());

            return catalog;
        }

        public McpSql_SchemaCatalog GenerateSqlTableSchema()
        {
            List<McpSql_TableSchema> tables = new List<McpSql_TableSchema>();
            

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
