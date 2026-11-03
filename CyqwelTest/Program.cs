using Cyqwel;
using Cyqwel.Dialects;
using Cyqwel.Visitors;
using Cyqwel.Generation;
using Cyqwel.Parsing;
using Cyqwel.Validation;

namespace CyqwelTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ParseSql();
            InspectAndTransformSQL();
            GenerateTranspileAndFormatSQL();
            ValidateSql();
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

        private static void ValidateSql()
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
        }

    }
}
