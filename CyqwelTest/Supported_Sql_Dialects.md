# Supported SQL dialects

le fichier DialectIdentityFixtureTests.cs permet de verifier que le dialect ne modifie pas le SQL.
```
var generated = dialect.Parse(sql).ToSql(dialect);
var regenerated = dialect.Parse(generated).ToSql(dialect);
```

On peut voir le code sql supporté par chaque dialecte dans les fichiers de test suivants:

| File | Dialect |
|----|----|
| DialectIdentityFixtureTests.Generic.cs | Generic |
| DialectIdentityFixtureTests.MySql.cs | MySql |
| DialectIdentityFixtureTests.Oracle.cs | Oracle |
| DialectIdentityFixtureTests.PostgreSql.cs | PostgreSql |
| DialectIdentityFixtureTests.Sqlite.cs | Sqlite |
| DialectIdentityFixtureTests.TSql.cs | TSql |

