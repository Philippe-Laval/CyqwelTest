# Cyqwel code

- AST
- Builders
- Dialects
- Generation
- Parsing
- Validation
- Visitors

## Ast

## Builders

### Sql.cs

It's the entry point to create SQL statements and expressions. 
It provides a fluent API to construct SQL queries in a dialect-neutral way.

Creates dialect-neutral SQL expressions and fluent statement builders.

## Dialects

- class SqlDialectRegistry
- class SqlDialectBuilder


## Generation
## Parsing
## Validation
## Visitors


## SqlValidationModels.cs

SqlValidationCodes

SqlValidationSeverity
SqlValidationLocation

SqlValidationOptions and SqlSchemaValidationOptions

SqlSchemaCatalog, SqlTableSchema, SqlColumnSchema, SqlColumnReference, SqlForeignKey
SqlTypeFamily

SqlTypeFamilies.Classify

