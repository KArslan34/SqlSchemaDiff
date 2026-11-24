using System.ComponentModel;
using NebulaUI.Controls;
using NebulaUI.Data;
using NebulaUI.Theming;
using SqlSchemaDiff.Services;

namespace SqlSchemaDiff.Views;

/// <summary>
/// Yan yana şema diff'i: # | Kaynak | # | Hedef. Satır türüne göre renkli zemin.
/// VirtualMode ile çizilir; büyük view'lerde de hızlıdır. Designer'a toolbox'tan bırakılabilir.
/// </summary>
[ToolboxItem(true)]
[DesignerCategory("Code")]
[Description("Kaynak ve hedef tanımlarını yan yana karşılaştırır.")]
public class SchemaDiffView : UserControl
{
    private const float TintStrength = 0.22f;

    private readonly NebulaDataGridView _grid = new();
    private readonly NebulaLabel _emptyLabel = new();
    private readonly DataGridViewTextBoxColumn _colSourceNo = NumberColumn();
    private readonly DataGridViewTextBoxColumn _colSourceText = TextColumn("Kaynak");
    private readonly DataGridViewTextBoxColumn _colTargetNo = NumberColumn();
    private readonly DataGridViewTextBoxColumn _colTargetText = TextColumn("Hedef");

    private IReadOnlyList<DiffLine> _lines = Array.Empty<DiffLine>();

    public SchemaDiffView()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.VirtualMode = true;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AllowUserToResizeColumns = true;
        _grid.AutoGenerateColumns = false;
        _grid.MultiSelect = false;
        _grid.Columns.AddRange(_colSourceNo, _colSourceText, _colTargetNo, _colTargetText);
        _grid.CellValueNeeded += OnCellValueNeeded;
        _grid.CellFormatting += OnCellFormatting;

        _emptyLabel.Dock = DockStyle.Fill;
        _emptyLabel.Muted = true;
        _emptyLabel.TextAlign = ContentAlignment.MiddleCenter;
        _emptyLabel.Text = "Farkı görmek için listeden bir satır seç.";

        Controls.Add(_grid);
        Controls.Add(_emptyLabel);
        ShowEmpty(_emptyLabel.Text);
    }

    /// <summary>Gösterilen satırlardan farklı olanların sayısı.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int ChangedLineCount { get; private set; }

    /// <summary>Diff'i gösterir; <paramref name="focusText"/> içeren ilk satıra (yoksa ilk farka) kaydırır.</summary>
    public void ShowDiff(IReadOnlyList<DiffLine> lines, string? focusText = null)
    {
        _lines = lines;
        ChangedLineCount = lines.Count(l => l.Kind != DiffLineKind.Equal);

        _grid.RowCount = 0;          // VirtualMode: önbelleği temizle
        _grid.RowCount = lines.Count;
        _grid.Visible = true;
        _emptyLabel.Visible = false;

        ScrollToFocus(focusText);
    }

    /// <summary>Tabloyu gizler, ortada mesaj gösterir.</summary>
    public void ShowEmpty(string message)
    {
        _lines = Array.Empty<DiffLine>();
        ChangedLineCount = 0;
        _grid.RowCount = 0;
        _grid.Visible = false;
        _emptyLabel.Text = message;
        _emptyLabel.Visible = true;
    }

    private void ScrollToFocus(string? focusText)
    {
        if (_lines.Count == 0)
        {
            return;
        }

        int index = -1;
        if (!string.IsNullOrEmpty(focusText))
        {
            index = IndexOf(l => l.SourceText.Contains(focusText, StringComparison.OrdinalIgnoreCase)
                              || l.TargetText.Contains(focusText, StringComparison.OrdinalIgnoreCase));
        }
        if (index < 0)
        {
            index = IndexOf(l => l.Kind != DiffLineKind.Equal);
        }
        if (index < 0)
        {
            index = 0;
        }

        _grid.ClearSelection();
        _grid.Rows[index].Selected = true;
        try
        {
            _grid.FirstDisplayedScrollingRowIndex = Math.Max(0, index - 3);   // biraz bağlam bırak
        }
        catch (InvalidOperationException)
        {
            // Sekme gizliyken grid'in yüksekliği 0 olabilir; kaydırma atlanır
        }
    }

    private int IndexOf(Func<DiffLine, bool> predicate)
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            if (predicate(_lines[i])) return i;
        }
        return -1;
    }

    private void OnCellValueNeeded(object? sender, DataGridViewCellValueEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= _lines.Count)
        {
            return;
        }

        var line = _lines[e.RowIndex];
        e.Value = e.ColumnIndex switch
        {
            0 => line.SourceNo?.ToString() ?? string.Empty,
            1 => line.SourceText,
            2 => line.TargetNo?.ToString() ?? string.Empty,
            _ => line.TargetText
        };
    }

    /// <summary>Eklenecek = yeşil, farklı = turuncu, sadece hedefte = mor zemin; boş taraf koyu.</summary>
    private void OnCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= _lines.Count || e.CellStyle is null)
        {
            return;
        }

        var t = ThemeManager.Current;
        var line = _lines[e.RowIndex];
        bool sourceSide = e.ColumnIndex <= 1;
        bool isNumber = e.ColumnIndex is 0 or 2;

        Color? tint = (line.Kind, sourceSide) switch
        {
            (DiffLineKind.Modified, _) => t.Warning,
            (DiffLineKind.OnlySource, true) => t.Success,
            (DiffLineKind.OnlyTarget, false) => t.Secondary,
            (DiffLineKind.OnlySource, false) or (DiffLineKind.OnlyTarget, true) => null,   // boş taraf
            _ => (Color?)null
        };

        bool emptySide = line.Kind is DiffLineKind.OnlySource && !sourceSide
                      || line.Kind is DiffLineKind.OnlyTarget && sourceSide;

        Color back = emptySide ? t.Void : tint is { } c ? Blend(t.Surface, c, TintStrength) : t.Surface;
        e.CellStyle.BackColor = back;
        e.CellStyle.SelectionBackColor = Blend(back, t.Accent, 0.30f);
        e.CellStyle.ForeColor = isNumber ? t.TextMuted : t.Text;
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
    }

    private static Color Blend(Color from, Color to, float amount) => Color.FromArgb(
        (int)(from.R + (to.R - from.R) * amount),
        (int)(from.G + (to.G - from.G) * amount),
        (int)(from.B + (to.B - from.B) * amount));

    private static DataGridViewTextBoxColumn NumberColumn() => new()
    {
        HeaderText = "#",
        Width = 52,
        ReadOnly = true,
        SortMode = DataGridViewColumnSortMode.NotSortable,
        DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
    };

    private static DataGridViewTextBoxColumn TextColumn(string header) => new()
    {
        HeaderText = header,
        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        FillWeight = 50,
        ReadOnly = true,
        SortMode = DataGridViewColumnSortMode.NotSortable,
        DefaultCellStyle = { Font = new Font("Consolas", 9.75F), WrapMode = DataGridViewTriState.False }
    };
}
