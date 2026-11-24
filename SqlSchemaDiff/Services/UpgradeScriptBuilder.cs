using System.Text;
using SqlSchemaDiff.Models;

namespace SqlSchemaDiff.Services;

/// <summary>Assembles the selected differences into one runnable .sql file.</summary>
public class UpgradeScriptBuilder
{
    public string Build(
        IEnumerable<SchemaDifference> selectedDifferences,
        string databaseName,
        string sourceFileName,
        string targetFileName)
    {
        var builder = new StringBuilder();
        var differences = selectedDifferences.ToList();   // iki kez dolaşılıyor (uyarı + bölümler)

        AppendHeader(builder, sourceFileName, targetFileName, differences);
        AppendSessionSettings(builder, databaseName);
        AppendSections(builder, differences);
        AppendBatch(builder, "PRINT N'Guncelleme betigi tamamlandi.'");

        return NormalizeLineEndings(builder.ToString());
    }

    private static void AppendHeader(StringBuilder builder, string sourceFileName, string targetFileName,
        IReadOnlyCollection<SchemaDifference> differences)
    {
        builder.AppendLine("/* =====================================================================");
        builder.AppendLine("   Veritabanı Güncelleme Betiği (SQL Schema Diff ile oluşturuldu)");
        builder.AppendLine($"   Tarih     : {DateTime.Now:dd.MM.yyyy HH:mm}");
        builder.AppendLine($"   Kaynak    : {sourceFileName}");
        builder.AppendLine($"   Hedef     : {targetFileName}");
        builder.AppendLine("   Betik tekrar çalıştırılabilir.");
        AppendDataRiskWarning(builder, differences);
        builder.AppendLine("   Çalıştırmadan önce veritabanının yedeğini alın.");
        builder.AppendLine("   ===================================================================== */");
        builder.AppendLine();
    }

    /// <summary>
    /// ALTER COLUMN içeren seçimlerde potansiyel veri etkisi uyarısı; diğer değişiklikler mevcut veriye dokunmaz.
    /// </summary>
    private static void AppendDataRiskWarning(StringBuilder builder, IReadOnlyCollection<SchemaDifference> differences)
    {
        var risky = differences.Where(d => d.Kind == DifferenceKind.ChangedColumn).Select(d => d.ObjectName).ToList();
        if (risky.Count == 0)
        {
            builder.AppendLine("   Yalnızca ekleme yapar (tablo/kolon/FK/view); mevcut veriye dokunmaz.");
            return;
        }

        builder.AppendLine();
        builder.AppendLine($"   UYARI (potansiyel): {risky.Count} kolon tanımı değişikliği (ALTER COLUMN) içeriyor.");
        builder.AppendLine("   Bu değişiklikler mevcut veriyi etkileyebilir (ör. uzunluk kısalırsa kırpma/hata,");
        builder.AppendLine("   tip dönüşümü başarısız olabilir, NULL → NOT NULL dolu tabloda hata verebilir):");
        foreach (var name in risky)
        {
            builder.AppendLine($"     - {name}");
        }
        builder.AppendLine();
    }

    private static void AppendSessionSettings(StringBuilder builder, string databaseName)
    {
        if (!string.IsNullOrWhiteSpace(databaseName))
        {
            AppendBatch(builder, $"USE [{databaseName}]");
        }

        AppendBatch(builder, "SET ANSI_NULLS ON");
        AppendBatch(builder, "SET QUOTED_IDENTIFIER ON");
    }

    private static void AppendSections(StringBuilder builder, IEnumerable<SchemaDifference> differences)
    {
        var sections = differences
            .Where(d => d.IsApplicable)
            .GroupBy(d => d.Kind)
            .OrderBy(group => group.Key);

        foreach (var section in sections)
        {
            builder.AppendLine($"-- ---------- {section.Key.ToDisplayText()} ({section.Count()}) ----------");
            builder.AppendLine();

            foreach (var difference in section)
            {
                builder.AppendLine($"-- {difference.ObjectName}");
                AppendBatch(builder, difference.Script);
            }
        }
    }

    private static void AppendBatch(StringBuilder builder, string statement)
    {
        builder.AppendLine(statement);
        builder.AppendLine("GO");
        builder.AppendLine();
    }

    private static string NormalizeLineEndings(string text) =>
        text.Replace("\r\n", "\n").Replace("\n", "\r\n");
}
