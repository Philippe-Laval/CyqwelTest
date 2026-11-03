# SqlValidationTypeTests

examples come from SqlValidationTypeTests.cs

Schema_types_are_classified

```
[Theory]
[InlineData("INT4", SqlTypeFamily.Integer)]
[InlineData("double precision", SqlTypeFamily.Numeric)]
[InlineData("VARCHAR(255)", SqlTypeFamily.String)]
[InlineData("Nullable(Int64)", SqlTypeFamily.Integer)]
[InlineData("Array(String)", SqlTypeFamily.Array)]
[InlineData("STRUCT<a INT>", SqlTypeFamily.Struct)]
public void Schema_types_are_classified(string dataType, SqlTypeFamily expected)
{
    Assert.Equal(expected, SqlTypeFamilies.Classify(dataType));
}
```

Incompatible_comparisons_are_reported

```
private static readonly SqlSchemaCatalog Catalog = ValidationTestCatalog.Create();

var result = SqlValidator.Validate(
            "SELECT id FROM users WHERE age = 'old'",
            Catalog,
            options: Options);
```

Invalid_arithmetic_is_reported

```
var result = SqlValidator.Validate(
            "SELECT age + name FROM users",
            Catalog,
            options: Options);
```

Non_boolean_predicates_are_reported
with diagnostic SqlValidationCodes.InvalidPredicateType

```
var result = SqlValidator.Validate(
            "SELECT id FROM users WHERE age + 1",
            Catalog,
            options: Options);
```

Function_argument_types_and_arity_are_checked
SqlValidationCodes.InvalidFunctionArgumentType
SqlValidationCodes.InvalidFunctionArity

```
 var typeResult = SqlValidator.Validate(
            "SELECT ABS(name) FROM users",
            Catalog,
            options: Options);
        var arityResult = SqlValidator.Validate(
            "SELECT ABS(age, id) FROM users",
            Catalog,
            options: Options);
```

Dml_assignment_types_are_checked
InvalidAssignmentType

```
private static readonly SqlSchemaValidationOptions Options = new() { CheckTypes = true };

var result = SqlValidator.Validate("INSERT INTO users (age) VALUES ('old')", Catalog, options: Options);
var result = SqlValidator.Validate("INSERT INTO users (age) SELECT name FROM users", Catalog, options: Options);
var result = SqlValidator.Validate("UPDATE users SET age = name", Catalog, options: Options);
```

Merge_insert_assignment_types_are_checked
InvalidAssignmentType

```
var result = SqlValidator.Validate(
            """
            MERGE INTO users u
            USING orders o
            ON u.id = o.user_id
            WHEN NOT MATCHED THEN INSERT (age) VALUES (o.description)
            """,
            Catalog,
            options: Options);
```

Intervals_are_not_assignable_to_timestamp_columns

```
var catalog = new SqlSchemaCatalog(
            new SqlTableSchema(
                "events",
                [
                    new("started_at", "timestamp"),
                    new("duration", "interval"),
                ]));

        var result = SqlValidator.Validate(
            "UPDATE events SET started_at = INTERVAL '1' DAY",
            catalog,
            options: Options);
```

Set_operation_arity_and_types_are_checked

```
 var arity = SqlValidator.Validate(
            "SELECT id FROM users UNION SELECT id, total FROM orders",
            Catalog,
            options: Options);

var types = SqlValidator.Validate(
    "SELECT age FROM users UNION SELECT name FROM users",
    Catalog,
    options: Options);
```

Non_strict_type_issues_are_warnings

```
var result = SqlValidator.Validate(
            "UPDATE users SET age = name",
            Catalog,
            options: new SqlSchemaValidationOptions
            {
                CheckTypes = true,
                Strict = false,
            });
```