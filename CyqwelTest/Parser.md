# Parser

Examples comes from ParserTests.cs

```
const string sql = """
            SELECT u.id, COUNT(*) AS total
            FROM users u
            LEFT JOIN orders o ON u.id = o.user_id
            WHERE u.active = TRUE
            GROUP BY u.id
            HAVING COUNT(*) > 1
            ORDER BY total DESC
            LIMIT 10 OFFSET 5
            """;

var document = SqlParser.Parse(sql);
```

- select.Projections
- select.From
- select.Where
- select.GroupBy
- select.Having
- select.OrderBy
- select.Limit
- select.Offset

```
const string sql = """
            WITH active AS (SELECT id FROM users WHERE active = TRUE)
            SELECT id FROM active
            UNION ALL
            SELECT id FROM archived
            ORDER BY id
            LIMIT 5
            """;
var document = SqlParser.Parse(sql);
```

## SqlDialects.TSql.Parse

```
 var document = SqlDialects.TSql.Parse(
            "SELECT GETDATE() FROM users ORDER BY id OFFSET 20 ROWS FETCH NEXT 10 ROWS ONLY");
``` 


```
var document = SqlParser.Parse("SELECT 'it''s valid' FROM users WHERE NOT age = 18");
```

```
const string sql = """
            SELECT -1, +2, ~3, 5 % 2, flags & 1, flags ^ 2, flags | 4,
                   CASE WHEN score NOT BETWEEN 1 AND 10
                        THEN CAST(score AS DECIMAL(10, 2))
                   END
            FROM users
            WHERE id NOT IN (SELECT id FROM archived)
              AND name NOT LIKE 'x%'
              AND deleted IS NOT NULL
            """;
 var document = SqlParser.Parse(sql);

var unaryOperators = document.FindAll<UnaryExpression>()
```

- UnaryExpression
- BinaryExpression
- BetweenExpression
- InExpression
- IsNullExpression
- CastExpression
- CaseExpression

## Joins

```
var joins = SqlParser.Parse("""
            SELECT *
            FROM a
            JOIN b ON a.id = b.id
            INNER JOIN c ON b.id = c.id
            RIGHT OUTER JOIN d ON c.id = d.id
            FULL OUTER JOIN e ON d.id = e.id
            CROSS JOIN f
            """);
        var joinKinds = joins.FindAll<JoinTable>()
            .Select(join => join.Kind)
            .ToArray();

        Assert.Contains(JoinKind.Inner, joinKinds);
        Assert.Contains(JoinKind.Right, joinKinds);
        Assert.Contains(JoinKind.Full, joinKinds);
        Assert.Contains(JoinKind.Cross, joinKinds);
```

- JoinKind.Inner
- JoinKind.Right
- JoinKind.Full
- JoinKind.Cross


