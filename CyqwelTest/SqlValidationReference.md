# SqlValidationReference

examples come from SqlValidationReferenceTests.cs

```
 var result = SqlValidator.Validate(
            "SELECT 1",
            ValidationTestCatalog.Create(withRelationship: true),
            options: new SqlSchemaValidationOptions { CheckReferences = true });

```

Invalid_foreign_key_targets_are_reported
SqlValidationCodes.InvalidForeignKeyReference

```
var catalog = new SqlSchemaCatalog(
    new SqlTableSchema(
        "orders",
        [
            new(
                "user_id",
                "integer",
                References: new SqlColumnReference("missing_users", "id")),
        ]));

var result = SqlValidator.Validate(
    "SELECT 1",
    catalog,
    options: new SqlSchemaValidationOptions { CheckReferences = true });

```

Invalid_foreign_keys_warn_in_non_strict_mode

```
var catalog = new SqlSchemaCatalog(
            new SqlTableSchema(
                "orders",
                [
                    new(
                        "user_id",
                        "integer",
                        References: new SqlColumnReference("missing_users", "id")),
                ]));

var result = SqlValidator.Validate(
    "SELECT 1",
    catalog,
    options: new SqlSchemaValidationOptions
    {
        CheckReferences = true,
        Strict = false,
    });
```

Ambiguous_unqualified_columns_are_reported

```
 var result = SqlValidator.Validate(
            "SELECT id FROM users JOIN orders ON users.id = orders.user_id",
            ValidationTestCatalog.Create(),
            options: new SqlSchemaValidationOptions { CheckReferences = true });
```

Cartesian_joins_warn

```
var result = SqlValidator.Validate(
            "SELECT users.id FROM users CROSS JOIN orders",
            ValidationTestCatalog.Create(),
            options: new SqlSchemaValidationOptions { CheckReferences = true });
```





