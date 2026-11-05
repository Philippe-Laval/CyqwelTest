namespace CyqwelTest
{
    public static class McpSql_SchemaCatalogExtensions
    {
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
}
