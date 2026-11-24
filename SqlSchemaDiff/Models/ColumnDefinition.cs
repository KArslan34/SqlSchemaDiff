namespace SqlSchemaDiff.Models;

/// <summary>A single table column, e.g. Name = "Id", Definition = "[int] NOT NULL".</summary>
public class ColumnDefinition
{
    public string Name { get; set; } = string.Empty;

    public string Definition { get; set; } = string.Empty;
}
