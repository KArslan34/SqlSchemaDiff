using System.Text.RegularExpressions;
using SqlSchemaDiff.Models;

namespace SqlSchemaDiff.Services;

/// <summary>Compares the source (Main) schema with the target schema.</summary>
public class SchemaComparer
{
    private static readonly Regex WhiteSpace = new(@"\s+");

    public List<SchemaDifference> Compare(SchemaSnapshot source, SchemaSnapshot target)
    {
        var differences = new List<SchemaDifference>();
        differences.AddRange(CompareTables(source, target));
        differences.AddRange(CompareForeignKeys(source, target));
        differences.AddRange(CompareViews(source, target));
        return differences;
    }

    // ---------- Tables and columns ----------

    private static IEnumerable<SchemaDifference> CompareTables(SchemaSnapshot source, SchemaSnapshot target)
    {
        var targetTables = target.Tables.ToDictionary(t => t.QualifiedName, StringComparer.OrdinalIgnoreCase);

        foreach (var sourceTable in source.Tables)
        {
            if (!targetTables.TryGetValue(sourceTable.QualifiedName, out var targetTable))
            {
                yield return CreateNewTableDifference(sourceTable);
                continue;
            }

            foreach (var difference in CompareColumns(sourceTable, targetTable))
            {
                yield return difference;
            }
        }
    }

    private static SchemaDifference CreateNewTableDifference(TableDefinition table) => new()
    {
        Kind = DifferenceKind.NewTable,
        ObjectName = table.QualifiedName,
        Detail = $"{table.Columns.Count} kolon",
        Script = SqlScriptTemplates.CreateTableIfMissing(table),
        IsSelected = true,
        SourceDefinition = table.CreateStatement
    };

    private static IEnumerable<SchemaDifference> CompareColumns(TableDefinition sourceTable, TableDefinition targetTable)
    {
        var targetColumns = targetTable.Columns.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);
        var sourceColumnNames = sourceTable.Columns.Select(c => c.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var sourceColumn in sourceTable.Columns)
        {
            if (!targetColumns.TryGetValue(sourceColumn.Name, out var targetColumn))
            {
                yield return WithTables(CreateNewColumnDifference(sourceTable, sourceColumn), sourceTable, targetTable, sourceColumn.Name);
            }
            else if (!AreEqual(sourceColumn.Definition, targetColumn.Definition))
            {
                yield return WithTables(CreateChangedColumnDifference(sourceTable, sourceColumn, targetColumn), sourceTable, targetTable, sourceColumn.Name);
            }
        }

        foreach (var extraColumn in targetTable.Columns.Where(c => !sourceColumnNames.Contains(c.Name)))
        {
            yield return new SchemaDifference
            {
                Kind = DifferenceKind.ExtraColumn,
                ObjectName = $"{targetTable.QualifiedName}.{extraColumn.Name}",
                Detail = extraColumn.Definition,
                SourceDefinition = sourceTable.CreateStatement,
                TargetDefinition = targetTable.CreateStatement,
                FocusColumn = extraColumn.Name
            };
        }
    }

    /// <summary>Kolon farklarında diff için iki tablonun tamamı gösterilir, kolon satırı odaklanır.</summary>
    private static SchemaDifference WithTables(SchemaDifference difference, TableDefinition sourceTable, TableDefinition targetTable, string column)
    {
        difference.SourceDefinition = sourceTable.CreateStatement;
        difference.TargetDefinition = targetTable.CreateStatement;
        difference.FocusColumn = column;
        return difference;
    }

    private static SchemaDifference CreateNewColumnDifference(TableDefinition table, ColumnDefinition column) => new()
    {
        Kind = DifferenceKind.NewColumn,
        ObjectName = $"{table.QualifiedName}.{column.Name}",
        Detail = BuildNewColumnDetail(column),
        Script = SqlScriptTemplates.AddColumnIfMissing(table, column),
        IsSelected = true
    };

    private static string BuildNewColumnDetail(ColumnDefinition column)
    {
        var requiresDefault = column.Definition.Contains("NOT NULL", StringComparison.OrdinalIgnoreCase)
                              && !column.Definition.Contains("IDENTITY", StringComparison.OrdinalIgnoreCase);

        return requiresDefault
            ? $"{column.Definition}  (NOT NULL: dolu tabloda DEFAULT gerekebilir)"
            : column.Definition;
    }

    private static SchemaDifference CreateChangedColumnDifference(
        TableDefinition table, ColumnDefinition sourceColumn, ColumnDefinition targetColumn) => new()
    {
        Kind = DifferenceKind.ChangedColumn,
        ObjectName = $"{table.QualifiedName}.{sourceColumn.Name}",
        Detail = $"Hedef: {targetColumn.Definition}  →  Kaynak: {sourceColumn.Definition}",
        Script = SqlScriptTemplates.AlterColumn(table, sourceColumn),
        IsSelected = false // risky: the user must opt in
    };

    // ---------- Foreign keys ----------

    private static IEnumerable<SchemaDifference> CompareForeignKeys(SchemaSnapshot source, SchemaSnapshot target)
    {
        var targetKeyNames = target.ForeignKeys.Select(k => k.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var foreignKey in source.ForeignKeys.Where(k => !targetKeyNames.Contains(k.Name)))
        {
            yield return new SchemaDifference
            {
                Kind = DifferenceKind.NewForeignKey,
                ObjectName = foreignKey.Name,
                Detail = foreignKey.TableFullName,
                Script = SqlScriptTemplates.AddForeignKeyIfMissing(foreignKey),
                IsSelected = true,
                SourceDefinition = foreignKey.AddStatement
            };
        }
    }

    // ---------- Views ----------

    private static IEnumerable<SchemaDifference> CompareViews(SchemaSnapshot source, SchemaSnapshot target)
    {
        var targetViews = target.Views.ToDictionary(v => v.QualifiedName, StringComparer.OrdinalIgnoreCase);

        foreach (var sourceView in source.Views)
        {
            if (!targetViews.TryGetValue(sourceView.QualifiedName, out var targetView))
            {
                yield return CreateViewDifference(DifferenceKind.NewView, sourceView, null, "Hedefte yok");
            }
            else if (!AreViewsEqual(sourceView, targetView))
            {
                yield return CreateViewDifference(DifferenceKind.ChangedView, sourceView, targetView, "İçerik farklı");
            }
        }
    }

    private static SchemaDifference CreateViewDifference(DifferenceKind kind, ViewDefinition view, ViewDefinition? targetView, string detail) => new()
    {
        Kind = kind,
        ObjectName = view.QualifiedName,
        Detail = detail,
        Script = SqlScriptTemplates.CreateOrAlterView(view),
        IsSelected = true,
        SourceDefinition = view.CreateStatement,
        TargetDefinition = targetView?.CreateStatement ?? string.Empty
    };

    // ---------- Helpers ----------

    private static bool AreEqual(string first, string second) =>
        string.Equals(CollapseWhiteSpace(first), CollapseWhiteSpace(second), StringComparison.OrdinalIgnoreCase);

    // Views are compared without any whitespace so that reformatting alone is not reported.
    private static bool AreViewsEqual(ViewDefinition first, ViewDefinition second) =>
        string.Equals(
            WhiteSpace.Replace(first.CreateStatement, string.Empty),
            WhiteSpace.Replace(second.CreateStatement, string.Empty),
            StringComparison.OrdinalIgnoreCase);

    private static string CollapseWhiteSpace(string value) => WhiteSpace.Replace(value, " ").Trim();
}
