using NebulaUI.Theming;
using SqlSchemaDiff.Models;

namespace SqlSchemaDiff.Helpers;

/// <summary>Fark türünün ekrandaki rengi: yeni = güvenli, değişmiş = riskli, bilgi = soluk.</summary>
internal static class DifferenceKindVisuals
{
    public static NebulaVariant GetVariant(this DifferenceKind kind) => kind switch
    {
        DifferenceKind.NewTable or DifferenceKind.NewColumn or DifferenceKind.NewForeignKey or DifferenceKind.NewView
            => NebulaVariant.Success,
        DifferenceKind.ChangedColumn or DifferenceKind.ChangedView
            => NebulaVariant.Warning,
        _ => NebulaVariant.Secondary
    };

    public static bool IsInformational(this DifferenceKind kind) => kind == DifferenceKind.ExtraColumn;
}
