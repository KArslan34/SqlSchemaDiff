using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using NebulaUI.Theming;

namespace SqlSchemaDiff.Helpers;

/// <summary>Üretilen T-SQL'i RichTextBox içinde Nebula renkleriyle boyar (yorum, metin, anahtar kelime, tip, GO).</summary>
internal static partial class SqlSyntaxHighlighter
{
    private const int WM_SETREDRAW = 0x000B;

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    // Sıra önemli: yorum ve metin önce yakalanır, içlerindeki kelimeler boyanmaz
    [GeneratedRegex(
        @"(?<comment>--[^\n]*|/\*.*?\*/)" +
        @"|(?<string>N?'(?:[^']|'')*')" +
        @"|(?<go>^\s*GO\s*$)" +
        @"|(?<type>\b(?:BIGINT|INT|SMALLINT|TINYINT|BIT|DECIMAL|NUMERIC|MONEY|FLOAT|REAL|DATE|DATETIME|DATETIME2|TIME|NVARCHAR|VARCHAR|NCHAR|CHAR|NTEXT|TEXT|VARBINARY|BINARY|UNIQUEIDENTIFIER|XML|MAX)\b)" +
        @"|(?<keyword>\b(?:CREATE|ALTER|DROP|ADD|TABLE|VIEW|COLUMN|CONSTRAINT|FOREIGN|PRIMARY|KEY|REFERENCES|IF|NOT|EXISTS|NULL|BEGIN|END|EXEC|EXECUTE|SET|USE|PRINT|SELECT|FROM|WHERE|AND|OR|ON|IS|AS|WITH|CHECK|NOCHECK|DEFAULT|IDENTITY|INSERT|INTO|VALUES|UPDATE|DELETE|JOIN|INNER|LEFT|TRANSACTION|TRAN|COMMIT|ROLLBACK|TRY|CATCH|THROW|DECLARE|RETURN|ELSE|CASCADE|NOCOUNT|XACT_ABORT|OBJECT_ID|COL_LENGTH|INFORMATION_SCHEMA)\b)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline)]
    private static partial Regex TokenRegex();

    public static void Apply(RichTextBox box)
    {
        var t = ThemeManager.Current;
        var keyword = Lighten(t.Accent, 0.45f);    // koyu mavi zeminde okunmuyor → açılmış Accent
        var type = Lighten(t.Secondary, 0.35f);
        var str = t.Warning;
        var comment = t.TextMuted;
        var go = t.Primary;

        int selStart = box.SelectionStart, selLength = box.SelectionLength;
        SendMessage(box.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);   // titremeyi önle
        try
        {
            box.SelectAll();
            box.SelectionColor = t.Text;

            foreach (Match m in TokenRegex().Matches(box.Text))
            {
                box.Select(m.Index, m.Length);
                box.SelectionColor =
                    m.Groups["comment"].Success ? comment :
                    m.Groups["string"].Success ? str :
                    m.Groups["go"].Success ? go :
                    m.Groups["type"].Success ? type : keyword;
            }

            box.Select(selStart, selLength);
        }
        finally
        {
            SendMessage(box.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            box.Invalidate();
        }
    }

    private static Color Lighten(Color c, float amount) => Color.FromArgb(
        c.R + (int)((255 - c.R) * amount),
        c.G + (int)((255 - c.G) * amount),
        c.B + (int)((255 - c.B) * amount));
}
