using Cyqwel.Validation;
using SqlMcp.Library;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace CyqwelTest
{
    


    public sealed class SqlSchemaCatalogGenerator
    {
        public SqlMcp_SchemaCatalog Generate()
        {
            var catalog = new SqlMcp_SchemaCatalog(new List<SqlMcp_TableSchema>());

            return catalog;
        }

        public SqlMcp_SchemaCatalog GenerateSqlTableSchema()
        {
            List<SqlMcp_TableSchema> tables = new List<SqlMcp_TableSchema>();
            

            var SqlTableSchema = new SqlMcp_SchemaCatalog(tables);

            return SqlTableSchema;
        }


        public static SqlMcp_SchemaCatalog CreateCatalog(bool withRelationship = false) =>
        new SqlMcp_SchemaCatalog(
            new SqlMcp_TableSchema(
                "users",
                new List<SqlMcp_ColumnSchema>
                {
                    new SqlMcp_ColumnSchema("id", "integer", isPrimaryKey: true),
                    new SqlMcp_ColumnSchema("name", "varchar"),
                    new SqlMcp_ColumnSchema("email", "varchar"),
                    new SqlMcp_ColumnSchema("age", "integer"),
                    new SqlMcp_ColumnSchema("active", "boolean"),
                },
                primaryKey: new List<string> { "id" }),
            new SqlMcp_TableSchema(
                "orders",
                new List<SqlMcp_ColumnSchema>
                {
                    new SqlMcp_ColumnSchema("id", "integer", isPrimaryKey: true),
                    new SqlMcp_ColumnSchema(
                        "user_id",
                        "integer",
                        references : withRelationship ? new SqlMcp_ColumnReference("users", "id") : null),
                    new SqlMcp_ColumnSchema("total", "decimal"),
                    new SqlMcp_ColumnSchema("description", "varchar"),
                },
                primaryKey: new List<string> { "id" }));
    }
}
