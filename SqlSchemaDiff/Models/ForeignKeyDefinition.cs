namespace SqlSchemaDiff.Models;

public class ForeignKeyDefinition
{
    public string Name { get; set; } = string.Empty;

    public string TableFullName { get; set; } = string.Empty;

    /// <summary>The "ALTER TABLE ... ADD CONSTRAINT ... FOREIGN KEY ..." statement.</summary>
    public string AddStatement { get; set; } = string.Empty;
}
