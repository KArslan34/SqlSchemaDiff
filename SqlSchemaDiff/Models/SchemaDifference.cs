namespace SqlSchemaDiff.Models;

/// <summary>One row of the comparison result. Bound directly to the grid.</summary>
public class SchemaDifference
{
    public DifferenceKind Kind { get; set; }

    public string ObjectName { get; set; } = string.Empty;

    public string Detail { get; set; } = string.Empty;

    /// <summary>The T-SQL batch that fixes this difference. Empty for informational rows.</summary>
    public string Script { get; set; } = string.Empty;

    public bool IsSelected { get; set; }

    /// <summary>Nesnenin kaynaktaki tam tanımı (CREATE TABLE/VIEW, FK ifadesi). Yoksa boş.</summary>
    public string SourceDefinition { get; set; } = string.Empty;

    /// <summary>Nesnenin hedefteki tam tanımı. Hedefte yoksa boş.</summary>
    public string TargetDefinition { get; set; } = string.Empty;

    /// <summary>Diff'te vurgulanacak kolon adı (kolon farklarında); diğerlerinde null.</summary>
    public string? FocusColumn { get; set; }

    public string KindText => Kind.ToDisplayText();

    public bool IsApplicable => !string.IsNullOrWhiteSpace(Script);
}
