using Cyqwel;
using Cyqwel.Ast;
using Cyqwel.Dialects;
using Cyqwel.Generation;
using Cyqwel.Parsing;
using Cyqwel.Validation;
using Cyqwel.Visitors;

namespace CyqwelTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MegaBuilder megaBuilder = new MegaBuilder();
            var sql = megaBuilder.Test();

            SqlSchemaCatalogGenerator generator = new SqlSchemaCatalogGenerator();
            var catalog = SqlSchemaCatalogGenerator.CreateCatalog();
            var json = catalog.SerializeToJson();
            Console.WriteLine(json);

            ParseSql();
            InspectAndTransformSQL();
            GenerateTranspileAndFormatSQL();
            ValidateSQL();
            BuildSQL();
            BuildStoredProcedures();
            ExtendDialects();
        }

        private static void ParseSql()
        {
            var document1 = SqlDialects.TSql.Parse(
                "SELECT TOP 10 [display name] FROM [users]");

            var statement = document1.Statements[0];


            if (!SqlDialects.PostgreSql.TryParse(
                "SELECT * FROM users",
                out var document2,
                out var error))
            {
                Console.WriteLine($"{error!.Code}: {error.Message}");
            }
        }


        private static void InspectAndTransformSQL()
        {
            var source = SqlDialects.PostgreSql.Parse(
                "SELECT u.id FROM users AS u");

            var tables = source.GetTableNames();   // ["users"]
            var columns = source.GetColumnNames(); // ["u.id"]

            var transformed = source
                .RenameTable("users", "accounts")
                .RenameColumn("id", "account_id");

            var sql = transformed.ToSql(SqlDialects.PostgreSql);
            // SELECT u.account_id FROM accounts AS u
        }

        private static void GenerateTranspileAndFormatSQL()
        {
            var postgres = SqlDialects.TSql.Transpile(
                "SELECT TOP 10 [id] FROM [users]",
                SqlDialects.PostgreSql);

            // SELECT "id" FROM "users" LIMIT 10

            var document = SqlParser.Parse(
                    "select id, name from users where active = true");

            var formatted = document.ToSql(options: new SqlGenerationOptions
            {
                PrettyPrint = true,
                IndentSize = 2,
            });

            Console.WriteLine(formatted);
        }

        private static void ValidateSQL()
        {
            var result = SqlValidator.Validate(
                 "SELECT * FROM users LIMIT 10",
                 SqlDialects.PostgreSql,
                 new SqlValidationOptions
                 {
                     StrictSyntax = true,
                     Semantic = true,
                 });

            foreach (var diagnostic in result.Diagnostics)
            {
                Console.WriteLine($"{diagnostic.Code}: {diagnostic.Message}");
            }


            var catalog = new SqlSchemaCatalog(
                new SqlTableSchema(
                    "users",
                    [
                        new("id", "integer", IsPrimaryKey: true),
                        new("age", "integer"),
                    ],
                    PrimaryKey: ["id"]));

            var result2 = SqlValidator.Validate(
                "SELECT id FROM users WHERE age > 18",
                catalog,
                options: new SqlSchemaValidationOptions
                {
                    CheckTypes = true,
                    CheckReferences = true,
                });

            if (!result2.IsValid)
            {
                // Handle validation errors.
                foreach (var diagnostic in result2.Diagnostics)
                {
                    Console.WriteLine($"{diagnostic.Code}: {diagnostic.Message}");
                }
            }

            // Schema findings are errors by default. Set SqlSchemaValidationOptions.Strict to false to report them as warnings.

            var result3 = SqlValidator.Validate(
                "SELECT id FROM users WHERE price > 18",
                catalog,
                options: new SqlSchemaValidationOptions
                {
                    CheckTypes = true,
                    CheckReferences = true,
                });

            if (!result3.IsValid)
            {
                // Handle validation errors.
                foreach (var diagnostic in result3.Diagnostics)
                {
                    Console.WriteLine($"{diagnostic.Code}: {diagnostic.Message}");
                }
            }

        }

        private static void BuildSQL()
        {
            var query = Sql.Select("u.id", "u.name")
                .From("users", "u")
                .Where(Sql.Col("u.age").GreaterThan(Sql.Param("minimumAge")))
                .OrderBy(Sql.Col("u.name"))
                .Limit(10)
                .Build();

            var sql = query.ToSql(SqlDialects.PostgreSql);
            // SELECT u.id, u.name FROM users AS u
            // WHERE u.age > @minimumAge ORDER BY u.name ASC LIMIT 10
            Console.WriteLine(sql);


            var ranked = Sql.SelectItems(
                new SelectItem(
                    Sql.Func("ROW_NUMBER").Over(
                        partitionBy: [Sql.Col("region")],
                        orderBy: [Sql.Order(Sql.Col("amount"), OrderDirection.Descending)]),
                    "position"))
            .From("sales")
            .With("active_regions", Sql.Select("id").From("regions").Build())
            .Qualify(Sql.Col("position").LessThanOrEqualTo(Sql.Lit(3)))
            .Build();

            var combined = ranked
                .ToSql(SqlDialects.Sqlite);

            Console.WriteLine(combined);

            var query2 = Sql.SelectItems(new SelectItem(Sql.CurrentTimestamp())).Build();

            sql = query2.ToSql(SqlDialects.TSql);       // SELECT GETDATE()
            Console.WriteLine(sql);
            sql = query2.ToSql(SqlDialects.PostgreSql); // SELECT CURRENT_TIMESTAMP
            Console.WriteLine(sql);
            sql = query2.ToSql(SqlDialects.Oracle);     // SELECT CURRENT_TIMESTAMP
            Console.WriteLine(sql);

            var query3 = Sql.SelectItems(new SelectItem(Sql.CurrentTimestamp(CurrentTimestampKind.Utc))).Build();

            sql = query3.ToSql(SqlDialects.TSql);       // SELECT GETDATE()
            Console.WriteLine(sql);
            sql = query3.ToSql(SqlDialects.PostgreSql); // SELECT CURRENT_TIMESTAMP
            Console.WriteLine(sql);
            sql = query3.ToSql(SqlDialects.Oracle);     // SELECT CURRENT_TIMESTAMP
            Console.WriteLine(sql);

        }

        private static void BuildStoredProcedures()
        {
            var procedure = Sql.CreateProcedure("app.activate_user")
                .Parameter("user_id", "INTEGER")
                .Variable("changed", "INTEGER", Sql.Lit(0))
                .Statement(Sql.Update("users")
                    .Set("active", true)
                    .Where(Sql.Col("id").EqualTo(Sql.Local("user_id")))
                    .Build())
                .While(
                    Sql.Local("changed").GreaterThan(Sql.Lit(0)),
                    [
                        Sql.If(
                            Sql.Local("changed").EqualTo(Sql.Lit(1)),
                            [Sql.Break()]),
                        Sql.Continue(),
                    ])
                .Return()
                .Build();

            var createSql = procedure.ToSql(SqlDialects.PostgreSql);
            var callSql = Sql.CallProcedure("app.activate_user")
                .Argument(42)
                .ToSql(SqlDialects.PostgreSql);

            Console.WriteLine(createSql);
            Console.WriteLine(callSql);


            var block = Sql.Block()
                .Variable("attempt", "INTEGER", Sql.Lit(0))
                .While(
                    Sql.Local("attempt").LessThan(Sql.Lit(3)),
                    [Sql.If(Sql.Local("attempt").EqualTo(Sql.Lit(2)), [Sql.Break()])])
                .Return()
                .Build();

            var sql = block.ToSql(SqlDialects.PostgreSql);
            Console.WriteLine(sql);
        }

        private static void ExtendDialects()
        {
            var warehouse = SqlDialectBuilder.Create("warehouse")
                .BasedOn(SqlDialects.PostgreSql)
                .WithFunctionNameTransform(name =>
                    name.Equals("LEN", StringComparison.OrdinalIgnoreCase)
                        ? "LENGTH"
                        : name)
                .Register();

            var sql = warehouse.Generate(Sql.Func("LEN", Sql.Col("name")));
            // LENGTH(name)
            Console.WriteLine(sql);
        }


        public static SqlSchemaCatalog Create(bool withRelationship = false) =>
        new SqlSchemaCatalog(
            new SqlTableSchema(
                "users",
                [
                    new SqlColumnSchema("id", "integer", IsPrimaryKey: true),
                    new SqlColumnSchema("name", "varchar"),
                    new SqlColumnSchema("email", "varchar"),
                    new SqlColumnSchema("age", "integer"),
                    new SqlColumnSchema("active", "boolean"),
                ],
                PrimaryKey: ["id"]),
            new SqlTableSchema(
                "orders",
                [
                    new SqlColumnSchema("id", "integer", IsPrimaryKey: true),
                    new SqlColumnSchema(
                        "user_id",
                        "integer",
                        References: withRelationship
                            ? new SqlColumnReference("users", "id")
                            : null),
                    new SqlColumnSchema("total", "decimal"),
                    new SqlColumnSchema("description", "varchar"),
                ],
                PrimaryKey: ["id"]));

    }
}
