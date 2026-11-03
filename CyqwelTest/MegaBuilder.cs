using Cyqwel;
using Cyqwel.Ast;
using Cyqwel.Dialects;
using Cyqwel.Generation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyqwelTest
{
    public class MegaBuilder
    {
        public string Test()
        {
            SqlDialect sqlDialect = SqlDialects.TSql;
            SqlGenerationOptions sqlGenerationOptions = new SqlGenerationOptions { PrettyPrint = true };
            var sql = Sql.CreateSchema("dbo").ToSql(sqlDialect, sqlGenerationOptions);
            // "CREATE SCHEMA dbo"

            //SqlIdentifier sqlIdentifier = new SqlIdentifier()
            //ColumnDefinition c1 = new ColumnDefinition("id", SqlDataType.)

            //sql = Sql.CreateTable("user").ToSql(sqlDialect, sqlGenerationOptions);

            var createTable = Sql.CreateTable("users")
             .Temporary()
             .IfNotExists()
             .Column(Sql.DefineColumn(
                 "id",
                 new SqlDataType("INT"),
                 Nullability.NotNull,
                 identity: IdentityGeneration.ByDefault,
                 primaryKey: true))
             .Column("name", "VARCHAR", 100)
             .Constraint(Sql.Unique(["name"], "uq_users_name"))
             .Build();
            sql = createTable.ToSql(sqlDialect, sqlGenerationOptions);

            var alterTable = Sql.AlterTable("users")
                .AddColumn("email", "VARCHAR", 200)
                .AlterColumn("name", nullability: Nullability.NotNull)
                .AddConstraint(Sql.Check(Sql.Col("id").GreaterThan(Sql.Lit(0)), "ck_users_id"))
                .RenameColumn("email", "email_address")
                .Build();

            var drop = Sql.DropTable("users").And("archived_users").IfExists().Cascade().Build();

            var createTable2 = Sql.CreateTable("children")
            .Column("id", "INT")
            .Column("parent_id", "INT")
            .Constraint(Sql.ForeignKey(
                ["parent_id"],
                "parents",
                ["id"],
                ReferentialAction.SetNull,
                ReferentialAction.Cascade,
                "fk_children_parent"))
            .Build();

            return sql;
        }
    }
}
