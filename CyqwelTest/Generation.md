# Generation

Examples come from the file GenerationTests.cs


```csharp
var query = Sql.Select("u.id", "u.name")
    .From("users", "u")
    .Where(Sql.Col("u.age").GreaterThan(Sql.Lit(18)))
    .OrderBy(Sql.Col("u.name"))
    .Limit(10)
    .Build();
query.ToSql();
```

```sql
SELECT u.id, u.name FROM users AS u WHERE u.age > 18 ORDER BY u.name ASC LIMIT 10
```

Generates culture invariant_numeric literals

```csharp
var sql = Sql.Select(Sql.Lit(1234.5m), Sql.Lit(6.25d), Sql.Lit(7.75f)).ToSql();
```

```
var dialect = SqlDialectRegistry.Get(dialectName);

var sql = $"SELECT created_at + INTERVAL {value} DAY FROM events";
            var document = SqlDialects.MySql.Parse(sql);
            var literal = Assert.Single(document.FindAll<LiteralExpression>());
            var generated = document.ToSql(dialect);
```

```
dialect.Parse(generated).ToSql(dialect)
```

## Top and offset fetch

```
var top = Sql.Select("id").From("users").Limit(10).Build();
        var paged = Sql.Select("id")
            .From("users")
            .OrderBy(Sql.Col("id"))
            .Limit(10)
            .Offset(20)
            .Build();
```


```csharp
 var query = Sql.Select("id")
            .From("users")
            .Limit(10)
            .Offset(20)
            .Build();
```

## Transpile

```csharp
SqlDialects.TSql.Transpile(
                "SELECT TOP 10 id FROM users",
                SqlDialects.PostgreSql))
```


## DML


```
 Sql.InsertInto("users")
                .Columns("id", "name")
                .Values(1, "Ada")
                .Values(2, "Grace")
                .ToSql());

Sql.Update("users")
                .Set("name", "Ada")
                .Where(Sql.Col("id").EqualTo(Sql.Lit(1)))
                .ToSql());

Sql.DeleteFrom("users")
                .Where(Sql.Col("id").EqualTo(Sql.Lit(1)))
                .ToSql());

```

