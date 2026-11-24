namespace SqlSchemaDiff.Models;

/// <summary>Everything the tool understands about one SQL script.</summary>
public class SchemaSnapshot
{
    public string DatabaseName { get; set; } = string.Empty;

    public List<TableDefinition> Tables { get; } = new();

    public List<ViewDefinition> Views { get; } = new();

    public List<ForeignKeyDefinition> ForeignKeys { get; } = new();
}
