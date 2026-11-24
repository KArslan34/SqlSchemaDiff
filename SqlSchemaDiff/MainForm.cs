using System.ComponentModel;
using System.Text;
using NebulaUI.Animation;
using NebulaUI.Controls;
using NebulaUI.Dialogs;
using NebulaUI.Forms;
using NebulaUI.Theming;
using SqlSchemaDiff.Helpers;
using SqlSchemaDiff.Models;
using SqlSchemaDiff.Services;

namespace SqlSchemaDiff;

/// <summary>Ekran Designer'da (MainForm.Designer.cs); burada sadece iş mantığı ve ekran davranışı.</summary>
public partial class MainForm : NebulaForm
{
    private const string SqlFileFilter = "SQL betikleri (*.sql)|*.sql|Tüm dosyalar (*.*)|*.*";

    private readonly SqlSchemaParser _parser = new();
    private readonly SchemaComparer _comparer = new();
    private readonly UpgradeScriptBuilder _scriptBuilder = new();
    private readonly BindingList<SchemaDifference> _differences = new();

    private string _targetDatabaseName = string.Empty;
    private int _scriptChangeCount;

    public MainForm()
    {
        InitializeComponent();
        // NOT: Designer'daki olaylar burada tekrar bağlanmıyor.

        gridDifferences.DataSource = _differences;

        // .sql sürükle-bırak: başlık ve kaynak kartının tamamı (iç TextBox'lar dahil)
        EnableFileDrop(this, includeChildren: false);
        EnableFileDrop(pnlHeader);
        EnableFileDrop(cardSources);

        SetStatus("İki betik dosyasını seçip “Karşılaştır”a bas veya dosyaları pencereye sürükle.", NebulaVariant.Secondary, LedState.Off);
        UpdateCommandStates();
    }

    /// <summary>F5 = karşılaştır, Ctrl+G = script oluştur, Ctrl+S = kaydet, Ctrl+Shift+C = kopyala.</summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.F5:
                btnCompare.PerformClick();
                return true;
            case Keys.Control | Keys.G when btnGenerate.Enabled:
                btnGenerate.PerformClick();
                return true;
            case Keys.Control | Keys.S when btnSave.Enabled:
                btnSave.PerformClick();
                return true;
            case Keys.Control | Keys.Shift | Keys.C when btnCopy.Enabled:
                btnCopy.PerformClick();
                return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    // ---------- Designer olayları ----------

    private void btnBrowseSource_Click(object? sender, EventArgs e) => BrowseForFile(txtSourcePath);

    private void btnBrowseTarget_Click(object? sender, EventArgs e) => BrowseForFile(txtTargetPath);

    private void btnSelectAll_Click(object? sender, EventArgs e) => SetAllSelected(true);

    private void btnClearSelection_Click(object? sender, EventArgs e) => SetAllSelected(false);

    private void btnCompare_Click(object? sender, EventArgs e)
    {
        if (!TryGetExistingPaths(out var sourcePath, out var targetPath))
        {
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;
            SetStatus("Betikler okunuyor…", NebulaVariant.Accent, LedState.Blink);
            Application.DoEvents();   // LED/metin hemen görünsün (parse senkron)

            var source = _parser.ParseFile(sourcePath);
            var target = _parser.ParseFile(targetPath);

            _targetDatabaseName = target.DatabaseName;
            ShowDifferences(_comparer.Compare(source, target));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            SetStatus("Karşılaştırma başarısız.", NebulaVariant.Danger, LedState.On);
            ShowError($"Betikler okunamadı:\n{ex.Message}");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnGenerate_Click(object? sender, EventArgs e)
    {
        gridDifferences.EndEdit();

        var selected = SelectedDifferences().ToList();
        if (selected.Count == 0)
        {
            ShowError("Script oluşturmak için en az bir fark seçin.");
            return;
        }

        _scriptChangeCount = selected.Count;
        rtbScript.Text = _scriptBuilder.Build(
            selected,
            _targetDatabaseName,
            Path.GetFileName(txtSourcePath.Text),
            Path.GetFileName(txtTargetPath.Text));

        SqlSyntaxHighlighter.Apply(rtbScript);
        tabBottom.SelectedTab = tabScript;
        rtbScript.Select(0, 0);
        rtbScript.ScrollToCaret();

        // ALTER COLUMN seçiliyse potansiyel veri etkisini hatırlat (scriptin başlığında da yazıyor)
        int risky = selected.Count(d => d.Kind == DifferenceKind.ChangedColumn);
        if (risky > 0)
        {
            SetStatus($"{selected.Count} değişiklik için script oluşturuldu — {risky} ALTER COLUMN mevcut veriyi etkileyebilir.",
                NebulaVariant.Warning, LedState.On);
            NebulaToast.Show(this, $"{risky} kolon tanımı değişikliği mevcut veriyi etkileyebilir. Önce yedek alıp test ortamında dene.",
                "Potansiyel veri etkisi", NebulaToastKind.Warning, 6000);
            return;
        }

        SetStatus($"{selected.Count} değişiklik için script oluşturuldu.", NebulaVariant.Success, LedState.On);
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (!HasScript())
        {
            return;
        }

        using var dialog = new NebulaSaveFileDialog
        {
            Filter = SqlFileFilter,
            FileName = SuggestedFileName(),
            Title = "Scripti kaydet"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        // UTF-8 BOM: SSMS Türkçe karakterleri doğru göstersin
        File.WriteAllText(dialog.FileName, GetScriptText(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

        SetStatus($"Kaydedildi: {dialog.FileName}", NebulaVariant.Success, LedState.On);
        NebulaToast.Show(this, Path.GetFileName(dialog.FileName), "Script kaydedildi", NebulaToastKind.Success);
    }

    private void btnCopy_Click(object? sender, EventArgs e)
    {
        if (!HasScript())
        {
            return;
        }

        Clipboard.SetText(GetScriptText());
        SetStatus("Script panoya kopyalandı.", NebulaVariant.Success, LedState.On);
        NebulaToast.Show(this, "SSMS'e yapıştırabilirsin.", "Panoya kopyalandı", NebulaToastKind.Info, 2500);
    }

    /// <summary>Checkbox tıklaması anında BindingList'e yazılsın, sayaç güncellensin.</summary>
    private void gridDifferences_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (!gridDifferences.IsCurrentCellDirty)
        {
            return;
        }

        gridDifferences.CommitEdit(DataGridViewDataErrorContexts.Commit);
        UpdateCommandStates();
    }

    /// <summary>Bilgi satırlarının (scripti olmayan) checkbox'ı tıklanamaz.</summary>
    private void gridDifferences_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow row in gridDifferences.Rows)
        {
            if (row.DataBoundItem is SchemaDifference { IsApplicable: false })
            {
                row.Cells[colApply.Index].ReadOnly = true;
            }
        }
    }

    /// <summary>Tür sütunu türüne göre renkli; bilgi satırları komple soluk.</summary>
    private void gridDifferences_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.CellStyle is null
            || gridDifferences.Rows[e.RowIndex].DataBoundItem is not SchemaDifference difference)
        {
            return;
        }

        var theme = ThemeManager.Current;
        if (difference.Kind.IsInformational())
        {
            e.CellStyle.ForeColor = theme.TextMuted;
            e.CellStyle.SelectionForeColor = theme.TextMuted;
        }
        else if (e.ColumnIndex == colKind.Index)
        {
            var color = theme.GetVariantColor(difference.Kind.GetVariant());
            e.CellStyle.ForeColor = color;
            e.CellStyle.SelectionForeColor = color;
        }
    }

    /// <summary>Seçili farkın şema diff'ini güncelle (klavyeyle gezerken de).</summary>
    private void gridDifferences_SelectionChanged(object? sender, EventArgs e) => ShowSelectedDiff();

    /// <summary>Satıra tıklanınca diff sekmesine geç (checkbox tıklaması hariç).</summary>
    private void gridDifferences_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex != colApply.Index)
        {
            tabBottom.SelectedTab = tabDiff;
        }
    }

    private void rtbScript_TextChanged(object? sender, EventArgs e)
    {
        UpdateScriptInfo();
        UpdateCommandStates();
    }

    // ---------- Sürükle-bırak ----------

    private void EnableFileDrop(Control root, bool includeChildren = true)
    {
        root.AllowDrop = true;
        root.DragEnter += OnFileDragEnter;
        root.DragDrop += OnFileDragDrop;

        if (!includeChildren)
        {
            return;
        }

        foreach (Control child in root.Controls)
        {
            EnableFileDrop(child);
        }
    }

    private static void OnFileDragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = GetDroppedFiles(e).Length > 0 ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void OnFileDragDrop(object? sender, DragEventArgs e)
    {
        var files = GetDroppedFiles(e);
        if (files.Length == 0)
        {
            return;
        }

        if (files.Length >= 2)
        {
            // İki dosya: sıra = kaynak, hedef
            txtSourcePath.Text = files[0];
            txtTargetPath.Text = files[1];
        }
        else
        {
            ResolveDropTarget(sender as Control).Text = files[0];
        }

        SetStatus("Dosya eklendi. Hazır olduğunda “Karşılaştır”a bas (F5).", NebulaVariant.Accent, LedState.On);
    }

    /// <summary>Tek dosya: üzerine bırakılan kutu; değilse ilk boş kutu; ikisi de doluysa kaynak.</summary>
    private NebulaTextBox ResolveDropTarget(Control? dropped)
    {
        for (var c = dropped; c is not null; c = c.Parent)
        {
            if (c == txtSourcePath) return txtSourcePath;
            if (c == txtTargetPath) return txtTargetPath;
        }

        if (string.IsNullOrWhiteSpace(txtSourcePath.Text)) return txtSourcePath;
        if (string.IsNullOrWhiteSpace(txtTargetPath.Text)) return txtTargetPath;
        return txtSourcePath;
    }

    private static string[] GetDroppedFiles(DragEventArgs e) =>
        e.Data?.GetData(DataFormats.FileDrop) is string[] paths
            ? paths.Where(File.Exists).ToArray()
            : Array.Empty<string>();

    // ---------- Yardımcılar ----------

    private void BrowseForFile(NebulaTextBox targetTextBox)
    {
        using var dialog = new NebulaOpenFileDialog { Filter = SqlFileFilter, Title = "SQL betiği seç" };

        var current = targetTextBox.Text.Trim();
        if (File.Exists(current))
        {
            dialog.InitialDirectory = Path.GetDirectoryName(current) ?? string.Empty;
        }

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            targetTextBox.Text = dialog.FileName;
        }
    }

    private bool TryGetExistingPaths(out string sourcePath, out string targetPath)
    {
        sourcePath = txtSourcePath.Text.Trim();
        targetPath = txtTargetPath.Text.Trim();

        if (File.Exists(sourcePath) && File.Exists(targetPath))
        {
            return true;
        }

        ShowError("Lütfen geçerli iki .sql dosyası seçin.");
        return false;
    }

    private void ShowDifferences(IEnumerable<SchemaDifference> differences)
    {
        _differences.Clear();
        foreach (var difference in differences)
        {
            _differences.Add(difference);
        }

        rtbScript.Clear();
        RefreshKindTags();
        tabBottom.SelectedTab = tabDiff;
        ShowSelectedDiff();
        UpdateCommandStates();

        var applicableCount = _differences.Count(d => d.IsApplicable);
        if (_differences.Count == 0)
        {
            SetStatus("Fark bulunamadı: hedef şema kaynakla aynı.", NebulaVariant.Success, LedState.On);
        }
        else
        {
            SetStatus($"{_differences.Count} fark bulundu, {applicableCount} tanesi için script üretilebilir.",
                NebulaVariant.Warning, LedState.Pulse);
        }
    }

    private void ShowSelectedDiff()
    {
        if (gridDifferences.CurrentRow?.DataBoundItem is not SchemaDifference difference)
        {
            lblDiffObject.Text = "Nesne seçilmedi";
            diffView.ShowEmpty(_differences.Count == 0
                ? "Karşılaştırma yapınca farklar burada yan yana görünür."
                : "Farkı görmek için listeden bir satır seç.");
            return;
        }

        var lines = LineDiff.Compare(difference.SourceDefinition, difference.TargetDefinition);
        if (lines.Count == 0)
        {
            lblDiffObject.Text = $"{difference.ObjectName}  ·  {difference.KindText}";
            diffView.ShowEmpty("Bu fark için gösterilecek tanım yok.");
            return;
        }

        // Kolon farklarında kolon satırına odaklan: [KolonAdı]
        var focus = difference.FocusColumn is { Length: > 0 } column ? $"[{column}]" : null;
        diffView.ShowDiff(lines, focus);

        var summary = string.IsNullOrEmpty(difference.TargetDefinition)
            ? "hedefte yok — tamamı eklenecek"
            : $"{diffView.ChangedLineCount} satır farklı";
        lblDiffObject.Text = $"{difference.ObjectName}  ·  {difference.KindText}  ·  {summary}";
    }

    /// <summary>Türe göre sayılar: "Yeni tablo · 3" etiketleri (tür rengiyle).</summary>
    private void RefreshKindTags()
    {
        flpKindTags.SuspendLayout();
        try
        {
            foreach (var tag in flpKindTags.Controls.OfType<NebulaTagLabel>().ToList())
            {
                flpKindTags.Controls.Remove(tag);
                tag.Dispose();
            }

            var groups = _differences.GroupBy(d => d.Kind).OrderBy(g => g.Key).ToList();
            lblDiffHint.Visible = groups.Count == 0;
            lblDiffHint.Text = _differences.Count == 0 && !string.IsNullOrEmpty(txtTargetPath.Text)
                ? "Fark yok — şemalar aynı."
                : "Henüz karşılaştırma yapılmadı.";

            foreach (var group in groups)
            {
                flpKindTags.Controls.Add(new NebulaTagLabel
                {
                    Text = $"{group.Key.ToDisplayText()} · {group.Count()}",
                    Variant = group.Key.GetVariant(),
                    Margin = new Padding(0, 5, 8, 0)
                });
            }
        }
        finally
        {
            flpKindTags.ResumeLayout();
        }
    }

    private void SetAllSelected(bool isSelected)
    {
        foreach (var difference in _differences.Where(d => d.IsApplicable))
        {
            difference.IsSelected = isSelected;
        }

        gridDifferences.Refresh();
        UpdateCommandStates();
    }

    private IEnumerable<SchemaDifference> SelectedDifferences() =>
        _differences.Where(d => d.IsSelected && d.IsApplicable);

    /// <summary>Butonlar sadece anlamlı olduklarında aktif; sağ alttaki seçim sayacı.</summary>
    private void UpdateCommandStates()
    {
        int applicable = _differences.Count(d => d.IsApplicable);
        int selected = SelectedDifferences().Count();
        bool hasScript = !string.IsNullOrWhiteSpace(rtbScript.Text);

        btnSelectAll.Enabled = applicable > 0;
        btnClearSelection.Enabled = selected > 0;
        btnGenerate.Enabled = selected > 0;
        btnSave.Enabled = hasScript;
        btnCopy.Enabled = hasScript;

        lblSelection.Text = _differences.Count == 0 ? string.Empty : $"Seçili: {selected} / {applicable}";
    }

    private void UpdateScriptInfo()
    {
        if (rtbScript.TextLength == 0)
        {
            lblScriptInfo.Text = "Farkları işaretleyip “Script oluştur”a bas.";
            return;
        }

        int lines = rtbScript.Lines.Length;
        double kb = Encoding.UTF8.GetByteCount(rtbScript.Text) / 1024d;
        lblScriptInfo.Text = $"{_scriptChangeCount} değişiklik  ·  {lines} satır  ·  {kb:0.0} KB";
    }

    /// <summary>Upgrade_&lt;Veritabanı&gt;_yyyyMMdd.sql — geçersiz karakterler atılır.</summary>
    private string SuggestedFileName()
    {
        var db = string.IsNullOrWhiteSpace(_targetDatabaseName) ? "Hedef" : _targetDatabaseName;
        db = string.Concat(db.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)));
        return $"Upgrade_{db}_{DateTime.Now:yyyyMMdd}.sql";
    }

    /// <summary>RichTextBox satır sonlarını \n'e çevirir; SSMS için \r\n'e geri al.</summary>
    private string GetScriptText() => rtbScript.Text.ReplaceLineEndings("\r\n");

    private bool HasScript()
    {
        if (!string.IsNullOrWhiteSpace(rtbScript.Text))
        {
            return true;
        }

        ShowError("Önce “Script oluştur” düğmesine basın.");
        return false;
    }

    private void SetStatus(string text, NebulaVariant ledVariant, LedState ledState)
    {
        lblStatus.Text = text;
        ledStatus.Variant = ledVariant;
        ledStatus.State = ledState;
    }

    private void ShowError(string message) =>
        NebulaMessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
