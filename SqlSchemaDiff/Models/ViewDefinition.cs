namespace SqlSchemaDiff.Models;

public class ViewDefinition
{
    public string Schema { get; set; } = "dbo";

    public string Name { get; set; } = string.Empty;

    /// <summary>The full CREATE VIEW statement, starting at the CREATE keyword.</summary>
    public string CreateStatement { get; set; } = string.Empty;

    public string QualifiedName => $"{Schema}.{Name}";
}
