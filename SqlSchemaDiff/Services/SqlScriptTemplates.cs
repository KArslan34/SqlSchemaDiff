using System.Text.RegularExpressions;
using SqlSchemaDiff.Models;

namespace SqlSchemaDiff.Services;

/// <summary>
/// The T-SQL snippets the tool can generate. Every snippet is one complete batch
/// (the caller adds the GO separator) and can safely be executed more than once.
/// </summary>
internal static class SqlScriptTemplates
{
    private static readonly Regex CreateViewKeyword =
        new(@"\ACREATE\s+VIEW", RegexOptions.IgnoreCase);

    public static string CreateTableIfMissing(TableDefinition table) =>
        $"IF OBJECT_ID(N'{table.QualifiedName}', N'U') IS NULL\nBEGIN\n{table.CreateStatement}\nEND";

    public static string AddColumnIfMissing(TableDefinition table, ColumnDefinition column) =>
        $"IF COL_LENGTH(N'{table.QualifiedName}', N'{column.Name}') IS NULL\n" +
        $"    ALTER TABLE {table.FullName} ADD [{column.Name}] {column.Definition};";

    public static string AlterColumn(TableDefinition table, ColumnDefinition column) =>
        "-- DİKKAT: Mevcut veriyle uyumsuz olabilir; index/default kısıtı varsa önce onları kaldırmak gerekir.\n" +
        $"ALTER TABLE {table.FullName} ALTER COLUMN [{column.Name}] {column.Definition};";

    public static string AddForeignKeyIfMissing(ForeignKeyDefinition foreignKey) =>
        $"IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'{foreignKey.Name}')\nBEGIN\n" +
        $"{foreignKey.AddStatement}\n" +
        $"ALTER TABLE {foreignKey.TableFullName} CHECK CONSTRAINT [{foreignKey.Name}]\nEND";

    public static string CreateOrAlterView(ViewDefinition view) =>
        CreateViewKeyword.Replace(view.CreateStatement, "CREATE OR ALTER VIEW", 1);
}
