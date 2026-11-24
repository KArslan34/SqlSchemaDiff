using System.Text.RegularExpressions;

namespace SqlSchemaDiff.Services;

public enum DiffLineKind
{
    Equal,
    OnlySource,   // kaynakta var, hedefte yok → eklenecek
    OnlyTarget,    // sadece hedefte
    Modified         // iki tarafta da var ama farklı
}

/// <summary>Yan yana diff'in bir satırı. Boş taraf için numara null, metin boştur.</summary>
public sealed record DiffLine(DiffLineKind Kind, int? SourceNo, string SourceText, int? TargetNo, string TargetText);

/// <summary>
/// Satır bazlı LCS diff. Boşluk, büyük/küçük harf ve satır sonu virgülü karşılaştırmada yok sayılır;
/// ardışık silme+ekleme blokları "Modified" olarak yan yana eşlenir.
/// </summary>
public static partial class LineDiff
{
    private const long MaxLcsCells = 4_000_000;   // ~2000 x 2000 satır üstünde basit eşlemeye düş

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhiteSpace();

    public static List<DiffLine> Compare(string source, string target)
    {
        var left = SplitLines(source);
        var right = SplitLines(target);
        var leftKeys = left.Select(Normalize).ToArray();
        var rightKeys = right.Select(Normalize).ToArray();

        var result = new List<DiffLine>(Math.Max(left.Length, right.Length));
        var pendingLeft = new List<int>();
        var pendingRight = new List<int>();

        // Bekleyen silme/ekleme bloğu: aynı ilk kelimeyle başlayanlar (örn. aynı kolon adı) "Modified" eşlenir,
        // kalanlar tek taraflı gösterilir. Böylece [New] ile [Old] gibi farklı kolonlar yanlışlıkla eşlenmez.
        void FlushPending()
        {
            var usedRight = new bool[pendingRight.Count];
            foreach (int l in pendingLeft)
            {
                string token = LeadingToken(leftKeys[l]);
                int match = -1;
                for (int k = 0; k < pendingRight.Count; k++)
                {
                    if (!usedRight[k] && LeadingToken(rightKeys[pendingRight[k]]) == token)
                    {
                        match = k;
                        break;
                    }
                }

                if (match >= 0)
                {
                    usedRight[match] = true;
                    int r = pendingRight[match];
                    result.Add(new DiffLine(DiffLineKind.Modified, l + 1, left[l], r + 1, right[r]));
                }
                else
                {
                    result.Add(new DiffLine(DiffLineKind.OnlySource, l + 1, left[l], null, string.Empty));
                }
            }

            for (int k = 0; k < pendingRight.Count; k++)
            {
                if (usedRight[k]) continue;
                int r = pendingRight[k];
                result.Add(new DiffLine(DiffLineKind.OnlyTarget, null, string.Empty, r + 1, right[r]));
            }

            pendingLeft.Clear();
            pendingRight.Clear();
        }

        if ((long)left.Length * right.Length > MaxLcsCells)
        {
            // Çok büyük nesne: sıra sıra eşle
            for (int k = 0; k < Math.Max(left.Length, right.Length); k++)
            {
                if (k < left.Length && k < right.Length && leftKeys[k] == rightKeys[k])
                {
                    FlushPending();
                    result.Add(new DiffLine(DiffLineKind.Equal, k + 1, left[k], k + 1, right[k]));
                    continue;
                }
                if (k < left.Length) pendingLeft.Add(k);
                if (k < right.Length) pendingRight.Add(k);
            }
            FlushPending();
            return result;
        }

        // lcs[i, j] = left[i..] ile right[j..] arasındaki en uzun ortak alt dizi
        var lcs = new int[left.Length + 1, right.Length + 1];
        for (int i = left.Length - 1; i >= 0; i--)
            for (int j = right.Length - 1; j >= 0; j--)
                lcs[i, j] = leftKeys[i] == rightKeys[j] ? lcs[i + 1, j + 1] + 1 : Math.Max(lcs[i + 1, j], lcs[i, j + 1]);

        int a = 0, b = 0;
        while (a < left.Length || b < right.Length)
        {
            if (a < left.Length && b < right.Length && leftKeys[a] == rightKeys[b])
            {
                FlushPending();
                result.Add(new DiffLine(DiffLineKind.Equal, a + 1, left[a], b + 1, right[b]));
                a++;
                b++;
            }
            else if (b >= right.Length || (a < left.Length && lcs[a + 1, b] >= lcs[a, b + 1]))
            {
                pendingLeft.Add(a++);
            }
            else
            {
                pendingRight.Add(b++);
            }
        }
        FlushPending();
        return result;
    }

    private static string[] SplitLines(string text) =>
        string.IsNullOrEmpty(text)
            ? Array.Empty<string>()
            : text.ReplaceLineEndings("\n").TrimEnd('\n').Split('\n');

    private static string LeadingToken(string normalizedLine)
    {
        int space = normalizedLine.IndexOf(' ');
        return space < 0 ? normalizedLine : normalizedLine[..space];
    }

    private static string Normalize(string line) =>
        WhiteSpace().Replace(line, " ").Trim().TrimEnd(',').TrimEnd().ToUpperInvariant();
}
