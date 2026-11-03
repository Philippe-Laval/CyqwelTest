# SqlValidationSemanticTests

examples come from SqlValidationSemanticTests.cs

Semantic_checks_warn_for_select_star

```
var result = SqlValidator.Validate(
            "SELECT * FROM users",
            options: new SqlValidationOptions { Semantic = true });
```

Mixed_aggregate_projections_without_group_by_warn

```
var result = SqlValidator.Validate(
            "SELECT user_id, SUM(total) FROM orders",
            options: new SqlValidationOptions { Semantic = true });

var result = SqlValidator.Validate(
            "SELECT user_id, SUM(total) FROM orders GROUP BY user_id",
            options: new SqlValidationOptions { Semantic = true });
```

```
var result = SqlValidator.Validate(
            "SELECT DISTINCT name FROM users ORDER BY age",
            options: new SqlValidationOptions { Semantic = true });

var result = SqlValidator.Validate(
            "SELECT DISTINCT name AS display_name FROM users ORDER BY display_name",
            options: new SqlValidationOptions { Semantic = true });
```

Non_deterministic_row_limiting_warns


```
var result = SqlValidator.Validate(
            "SELECT id FROM users LIMIT 10",
            options: new SqlValidationOptions { Semantic = true });

var result = SqlValidator.Validate(
            "SELECT id FROM users OFFSET 10",
            options: new SqlValidationOptions { Semantic = true });

var result = SqlValidator.Validate(
            "SELECT id FROM users ORDER BY id LIMIT 10",
            options: new SqlValidationOptions { Semantic = true });
```

