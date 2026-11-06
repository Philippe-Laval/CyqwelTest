using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMcp.Library
{
    public static class SqlMcp_SchemaCatalogExtensions
    {
        public static string SerializeToJson(this SqlMcp_SchemaCatalog catalog)
        {
            return System.Text.Json.JsonSerializer.Serialize(catalog, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        public static SqlMcp_SchemaCatalog DeserializeFromJson(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<SqlMcp_SchemaCatalog>(json) ?? new SqlMcp_SchemaCatalog(new List<SqlMcp_TableSchema>());
        }

    }
}
