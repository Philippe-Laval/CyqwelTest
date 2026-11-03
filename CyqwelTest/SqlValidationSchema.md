# SqlValidator

Examples are from file SqlValidationSchemaTests.cs


```
var result = SqlValidator.Validate(
            "SELECT id, name, email FROM users",
            Catalog);
```

Warning

```
var result = SqlValidator.Validate("SELECT * FROM missing", Catalog);
```

Unknown_tables_are_errors_in_strict_mode
Unknown_columns_are_warnings_in_non_strict_mode

```
var result = SqlValidator.Validate(
            "SELECT missing FROM users",
            Catalog,
            options: new SqlSchemaValidationOptions { Strict = false });
```

```
 var result = SqlValidator.Validate(
            "SELECT u.id, o.total FROM users u JOIN orders o ON u.id = o.user_id",
            Catalog);
```

Unknown_qualifiers_are_reported

Explicit_schema_names_do_not_fall_back_to_another_schema

```
var catalog = new SqlSchemaCatalog(
            new SqlTableSchema("users", [new("id", "integer")], Schema: "public"));

        var known = SqlValidator.Validate("SELECT id FROM public.users", catalog);
        var unknown = SqlValidator.Validate("SELECT id FROM private.users", catalog);

```


```
var result = SqlValidator.Validate(
            "WITH selected AS (SELECT id AS user_id FROM users) SELECT user_id FROM selected",
            Catalog);
```


