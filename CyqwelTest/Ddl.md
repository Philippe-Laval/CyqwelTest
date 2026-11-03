
examples are from TSqlDdlCoreTests.cs

## Create table

```
   
var column = new ColumnDefinition(new("id"), new SqlDataType("INT"), Identity: IdentityGeneration.ByDefault);
var create = new CreateTableStatement(new("t"), [column]);
create.ToSql(SqlDialects.TSql));
create.ToSql(SqlDialects.PostgreSql)
```

## Alter table

```
var column = new ColumnDefinition(new("id"), new SqlDataType("INT"), Identity: IdentityGeneration.ByDefault);
var alter = new AlterTableStatement(new("t"), [new AddColumnAction(column)]);
alter.ToSql(SqlDialects.TSql));
```

## Functions

```
 var function = Sql.CreateInlineFunction("dbo.f").Parameter("id", new SqlDataType("INT"))
            .Returns(Sql.SelectItems(new SelectItem(Sql.Lit(1), "value")).Build()).Build();
function.ToSql(SqlDialects.TSql)
```

## Validate with a schema catalog

```
var catalog = new SqlSchemaCatalog();
        var options = new SqlSchemaValidationOptions { CheckTypes = true };
        var valid = SqlValidator.Validate(
            "CREATE FUNCTION f(@id INT) RETURNS TABLE AS RETURN SELECT @id + 1 AS id", catalog, SqlDialects.TSql, options);
```




