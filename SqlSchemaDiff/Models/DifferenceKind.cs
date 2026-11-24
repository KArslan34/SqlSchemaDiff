namespace SqlSchemaDiff.Models;

/// <summary>
/// The order of the members is also the order of the sections in the generated script.
/// </summary>
public enum DifferenceKind
{
    NewTable,
    NewColumn,
    ChangedColumn,
    NewForeignKey,
    NewView,
    ChangedView,
    ExtraColumn
}

public static class DifferenceKindExtensions
{
    public static string ToDisplayText(this DifferenceKind kind) => kind switch
    {
        DifferenceKind.NewTable => "Yeni tablo",
        DifferenceKind.NewColumn => "Yeni kolon",
        DifferenceKind.ChangedColumn => "Kolon tanımı farklı",
        DifferenceKind.NewForeignKey => "Yeni foreign key",
        DifferenceKind.NewView => "Yeni view",
        DifferenceKind.ChangedView => "View değişmiş",
        DifferenceKind.ExtraColumn => "Sadece hedefte (bilgi)",
        _ => kind.ToString()
    };
}
