namespace SqlSchemaDiff.Models;

public class TableDefinition
{
    public string Schema { get; set; } = "dbo";

    public string Name { get; set; } = string.Empty;

    /// <summary>The full CREATE TABLE statement exactly as it appears in the script.</summary>
    public string CreateStatement { get; set; } = string.Empty;

    public List<ColumnDefinition> Columns { get; } = new();

    /// <summary>Form used with OBJECT_ID / COL_LENGTH, e.g. dbo.Settings.</summary>
    public string QualifiedName => $"{Schema}.{Name}";

    /// <summary>Bracketed form used in DDL, e.g. [dbo].[Settings].</summary>
    public string FullName => $"[{Schema}].[{Name}]";
}
