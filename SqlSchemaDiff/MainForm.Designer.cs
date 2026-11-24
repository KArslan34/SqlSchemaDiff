namespace SqlSchemaDiff
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tlpRoot = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblTitle = new NebulaUI.Controls.NebulaLabel();
            lblSubtitle = new NebulaUI.Controls.NebulaLabel();
            cardSources = new NebulaUI.Containers.NebulaCard();
            tlpFiles = new TableLayoutPanel();
            lblSource = new NebulaUI.Controls.NebulaLabel();
            txtSourcePath = new NebulaUI.Controls.NebulaTextBox();
            btnBrowseSource = new NebulaUI.Controls.NebulaButton();
            lblTarget = new NebulaUI.Controls.NebulaLabel();
            txtTargetPath = new NebulaUI.Controls.NebulaTextBox();
            btnBrowseTarget = new NebulaUI.Controls.NebulaButton();
            btnCompare = new NebulaUI.Controls.NebulaButton();
            splitMain = new NebulaUI.Data.NebulaSplitContainer();
            cardDifferences = new NebulaUI.Containers.NebulaCard();
            gridDifferences = new NebulaUI.Data.NebulaDataGridView();
            colApply = new DataGridViewCheckBoxColumn();
            colKind = new DataGridViewTextBoxColumn();
            colObject = new DataGridViewTextBoxColumn();
            colDetail = new DataGridViewTextBoxColumn();
            pnlDiffToolbar = new Panel();
            flpKindTags = new FlowLayoutPanel();
            lblDiffHint = new NebulaUI.Controls.NebulaLabel();
            flpSelectionButtons = new FlowLayoutPanel();
            btnSelectAll = new NebulaUI.Controls.NebulaButton();
            btnClearSelection = new NebulaUI.Controls.NebulaButton();
            tabBottom = new NebulaUI.Containers.NebulaTabControl();
            tabDiff = new TabPage();
            diffView = new SqlSchemaDiff.Views.SchemaDiffView();
            pnlDiffHeader = new Panel();
            lblDiffObject = new NebulaUI.Controls.NebulaLabel();
            flpLegend = new FlowLayoutPanel();
            tagLegendAdd = new NebulaUI.Controls.NebulaTagLabel();
            tagLegendChanged = new NebulaUI.Controls.NebulaTagLabel();
            tagLegendTarget = new NebulaUI.Controls.NebulaTagLabel();
            tabScript = new TabPage();
            rtbScript = new NebulaUI.Controls.NebulaRichTextBox();
            pnlScriptToolbar = new Panel();
            lblScriptInfo = new NebulaUI.Controls.NebulaLabel();
            flpScriptButtons = new FlowLayoutPanel();
            btnGenerate = new NebulaUI.Controls.NebulaButton();
            btnCopy = new NebulaUI.Controls.NebulaButton();
            btnSave = new NebulaUI.Controls.NebulaButton();
            pnlStatus = new Panel();
            flpStatus = new FlowLayoutPanel();
            ledStatus = new NebulaUI.Controls.NebulaLed();
            lblStatus = new NebulaUI.Controls.NebulaLabel();
            lblSelection = new NebulaUI.Controls.NebulaLabel();
            tlpRoot.SuspendLayout();
            pnlHeader.SuspendLayout();
            cardSources.SuspendLayout();
            tlpFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            cardDifferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridDifferences).BeginInit();
            pnlDiffToolbar.SuspendLayout();
            flpKindTags.SuspendLayout();
            flpSelectionButtons.SuspendLayout();
            tabBottom.SuspendLayout();
            tabDiff.SuspendLayout();
            pnlDiffHeader.SuspendLayout();
            flpLegend.SuspendLayout();
            tabScript.SuspendLayout();
            pnlScriptToolbar.SuspendLayout();
            flpScriptButtons.SuspendLayout();
            pnlStatus.SuspendLayout();
            flpStatus.SuspendLayout();
            SuspendLayout();
            //
            // tlpRoot
            //
            tlpRoot.ColumnCount = 1;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(pnlHeader, 0, 0);
            tlpRoot.Controls.Add(cardSources, 0, 1);
            tlpRoot.Controls.Add(splitMain, 0, 2);
            tlpRoot.Controls.Add(pnlStatus, 0, 3);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(16, 40);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.RowCount = 4;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tlpRoot.Size = new Size(1248, 772);
            tlpRoot.TabIndex = 0;
            //
            // pnlHeader
            //
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.TabIndex = 0;
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            lblTitle.Glow = true;
            lblTitle.Location = new Point(0, 4);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 0;
            lblTitle.Text = "SQL Schema Diff";
            //
            // lblSubtitle
            //
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(2, 38);
            lblSubtitle.Muted = true;
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Kaynak ve hedef şemayı karşılaştır; hedefi kaynakla eşitleyen, tekrar çalıştırılabilir güncelleme scripti üret.  (.sql dosyalarını buraya sürükleyebilirsin)";
            //
            // cardSources
            //
            cardSources.Controls.Add(tlpFiles);
            cardSources.Dock = DockStyle.Fill;
            cardSources.Margin = new Padding(0, 0, 0, 8);
            cardSources.Name = "cardSources";
            cardSources.Padding = new Padding(12, 6, 12, 10);
            cardSources.TabIndex = 1;
            cardSources.Title = "Kaynak betikler";
            //
            // tlpFiles
            //
            tlpFiles.ColumnCount = 4;
            tlpFiles.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tlpFiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpFiles.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112F));
            tlpFiles.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tlpFiles.Controls.Add(lblSource, 0, 0);
            tlpFiles.Controls.Add(txtSourcePath, 1, 0);
            tlpFiles.Controls.Add(btnBrowseSource, 2, 0);
            tlpFiles.Controls.Add(lblTarget, 0, 1);
            tlpFiles.Controls.Add(txtTargetPath, 1, 1);
            tlpFiles.Controls.Add(btnBrowseTarget, 2, 1);
            tlpFiles.Controls.Add(btnCompare, 3, 0);
            tlpFiles.Dock = DockStyle.Fill;
            tlpFiles.Name = "tlpFiles";
            tlpFiles.RowCount = 2;
            tlpFiles.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpFiles.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpFiles.TabIndex = 0;
            tlpFiles.SetRowSpan(btnCompare, 2);
            //
            // lblSource
            //
            lblSource.Anchor = AnchorStyles.Left;
            lblSource.AutoSize = true;
            lblSource.Muted = true;
            lblSource.Name = "lblSource";
            lblSource.TabIndex = 0;
            lblSource.Text = "Kaynak şema (olması gereken)";
            //
            // txtSourcePath
            //
            txtSourcePath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSourcePath.Name = "txtSourcePath";
            txtSourcePath.PlaceholderText = "Kaynak .sql betiği — örn. v2.5_Schema.sql";
            txtSourcePath.Size = new Size(700, 36);
            txtSourcePath.TabIndex = 1;
            //
            // btnBrowseSource
            //
            btnBrowseSource.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnBrowseSource.Name = "btnBrowseSource";
            btnBrowseSource.Size = new Size(104, 36);
            btnBrowseSource.TabIndex = 2;
            btnBrowseSource.Text = "Gözat…";
            btnBrowseSource.Variant = NebulaUI.Theming.NebulaVariant.Secondary;
            btnBrowseSource.Click += btnBrowseSource_Click;
            //
            // lblTarget
            //
            lblTarget.Anchor = AnchorStyles.Left;
            lblTarget.AutoSize = true;
            lblTarget.Muted = true;
            lblTarget.Name = "lblTarget";
            lblTarget.TabIndex = 3;
            lblTarget.Text = "Hedef şema (güncellenecek)";
            //
            // txtTargetPath
            //
            txtTargetPath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTargetPath.Name = "txtTargetPath";
            txtTargetPath.PlaceholderText = "Hedef .sql betiği — örn. Production_Schema.sql";
            txtTargetPath.Size = new Size(700, 36);
            txtTargetPath.TabIndex = 4;
            //
            // btnBrowseTarget
            //
            btnBrowseTarget.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnBrowseTarget.Name = "btnBrowseTarget";
            btnBrowseTarget.Size = new Size(104, 36);
            btnBrowseTarget.TabIndex = 5;
            btnBrowseTarget.Text = "Gözat…";
            btnBrowseTarget.Variant = NebulaUI.Theming.NebulaVariant.Secondary;
            btnBrowseTarget.Click += btnBrowseTarget_Click;
            //
            // btnCompare
            //
            btnCompare.Dock = DockStyle.Fill;
            btnCompare.Margin = new Padding(8, 4, 0, 4);
            btnCompare.Name = "btnCompare";
            btnCompare.TabIndex = 6;
            btnCompare.Text = "Karşılaştır  (F5)";
            btnCompare.Variant = NebulaUI.Theming.NebulaVariant.Primary;
            btnCompare.Click += btnCompare_Click;
            //
            // splitMain
            //
            splitMain.Dock = DockStyle.Fill;
            splitMain.Margin = new Padding(0);
            splitMain.Name = "splitMain";
            splitMain.Orientation = Orientation.Horizontal;
            //
            // splitMain.Panel1
            //
            splitMain.Panel1.Controls.Add(cardDifferences);
            splitMain.Panel1MinSize = 160;
            //
            // splitMain.Panel2
            //
            splitMain.Panel2.Controls.Add(tabBottom);
            splitMain.Panel2MinSize = 140;
            splitMain.Size = new Size(1248, 518);
            splitMain.SplitterDistance = 290;
            splitMain.TabIndex = 2;
            splitMain.Variant = NebulaUI.Theming.NebulaVariant.Accent;
            //
            // cardDifferences
            //
            cardDifferences.Controls.Add(gridDifferences);
            cardDifferences.Controls.Add(pnlDiffToolbar);
            cardDifferences.Dock = DockStyle.Fill;
            cardDifferences.Name = "cardDifferences";
            cardDifferences.Padding = new Padding(12, 4, 12, 10);
            cardDifferences.ShowAccent = true;
            cardDifferences.TabIndex = 0;
            cardDifferences.Title = "Farklar";
            //
            // gridDifferences
            //
            gridDifferences.AllowUserToAddRows = false;
            gridDifferences.AllowUserToDeleteRows = false;
            gridDifferences.AutoGenerateColumns = false;
            gridDifferences.Columns.AddRange(new DataGridViewColumn[] { colApply, colKind, colObject, colDetail });
            gridDifferences.Dock = DockStyle.Fill;
            gridDifferences.Name = "gridDifferences";
            gridDifferences.TabIndex = 1;
            gridDifferences.Variant = NebulaUI.Theming.NebulaVariant.Primary;
            gridDifferences.CellClick += gridDifferences_CellClick;
            gridDifferences.CellFormatting += gridDifferences_CellFormatting;
            gridDifferences.CurrentCellDirtyStateChanged += gridDifferences_CurrentCellDirtyStateChanged;
            gridDifferences.DataBindingComplete += gridDifferences_DataBindingComplete;
            gridDifferences.SelectionChanged += gridDifferences_SelectionChanged;
            //
            // colApply
            //
            colApply.DataPropertyName = "IsSelected";
            colApply.HeaderText = "Uygula";
            colApply.Name = "colApply";
            colApply.Width = 72;
            //
            // colKind
            //
            colKind.DataPropertyName = "KindText";
            colKind.HeaderText = "Tür";
            colKind.Name = "colKind";
            colKind.ReadOnly = true;
            colKind.Width = 190;
            //
            // colObject
            //
            colObject.DataPropertyName = "ObjectName";
            colObject.HeaderText = "Nesne";
            colObject.Name = "colObject";
            colObject.ReadOnly = true;
            colObject.Width = 320;
            //
            // colDetail
            //
            colDetail.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDetail.DataPropertyName = "Detail";
            colDetail.HeaderText = "Ayrıntı";
            colDetail.Name = "colDetail";
            colDetail.ReadOnly = true;
            //
            // pnlDiffToolbar
            //
            pnlDiffToolbar.Controls.Add(flpKindTags);
            pnlDiffToolbar.Controls.Add(flpSelectionButtons);
            pnlDiffToolbar.Dock = DockStyle.Top;
            pnlDiffToolbar.Name = "pnlDiffToolbar";
            pnlDiffToolbar.Padding = new Padding(0, 0, 0, 8);
            pnlDiffToolbar.Size = new Size(1224, 46);
            pnlDiffToolbar.TabIndex = 0;
            //
            // flpKindTags
            //
            flpKindTags.Controls.Add(lblDiffHint);
            flpKindTags.Dock = DockStyle.Fill;
            flpKindTags.Name = "flpKindTags";
            flpKindTags.TabIndex = 0;
            flpKindTags.WrapContents = false;
            //
            // lblDiffHint
            //
            lblDiffHint.AutoSize = true;
            lblDiffHint.Margin = new Padding(0, 10, 0, 0);
            lblDiffHint.Muted = true;
            lblDiffHint.Name = "lblDiffHint";
            lblDiffHint.TabIndex = 0;
            lblDiffHint.Text = "Henüz karşılaştırma yapılmadı.";
            //
            // flpSelectionButtons
            //
            flpSelectionButtons.AutoSize = true;
            flpSelectionButtons.Controls.Add(btnSelectAll);
            flpSelectionButtons.Controls.Add(btnClearSelection);
            flpSelectionButtons.Dock = DockStyle.Right;
            flpSelectionButtons.Name = "flpSelectionButtons";
            flpSelectionButtons.TabIndex = 1;
            flpSelectionButtons.WrapContents = false;
            //
            // btnSelectAll
            //
            btnSelectAll.Margin = new Padding(8, 0, 0, 0);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(124, 34);
            btnSelectAll.TabIndex = 0;
            btnSelectAll.Text = "Tümünü seç";
            btnSelectAll.Variant = NebulaUI.Theming.NebulaVariant.Secondary;
            btnSelectAll.Click += btnSelectAll_Click;
            //
            // btnClearSelection
            //
            btnClearSelection.Margin = new Padding(8, 0, 0, 0);
            btnClearSelection.Name = "btnClearSelection";
            btnClearSelection.Size = new Size(124, 34);
            btnClearSelection.TabIndex = 1;
            btnClearSelection.Text = "Seçimi kaldır";
            btnClearSelection.Variant = NebulaUI.Theming.NebulaVariant.Secondary;
            btnClearSelection.Click += btnClearSelection_Click;
            //
            // tabBottom
            //
            tabBottom.Controls.Add(tabDiff);
            tabBottom.Controls.Add(tabScript);
            tabBottom.Dock = DockStyle.Fill;
            tabBottom.Name = "tabBottom";
            tabBottom.SelectedIndex = 0;
            tabBottom.TabIndex = 0;
            tabBottom.Variant = NebulaUI.Theming.NebulaVariant.Accent;
            //
            // tabDiff
            //
            tabDiff.Controls.Add(diffView);
            tabDiff.Controls.Add(pnlDiffHeader);
            tabDiff.Name = "tabDiff";
            tabDiff.Padding = new Padding(12, 8, 12, 10);
            tabDiff.TabIndex = 0;
            tabDiff.Text = "Şema farkı";
            //
            // diffView
            //
            diffView.Dock = DockStyle.Fill;
            diffView.Name = "diffView";
            diffView.TabIndex = 1;
            //
            // pnlDiffHeader
            //
            pnlDiffHeader.Controls.Add(lblDiffObject);
            pnlDiffHeader.Controls.Add(flpLegend);
            pnlDiffHeader.Dock = DockStyle.Top;
            pnlDiffHeader.Name = "pnlDiffHeader";
            pnlDiffHeader.Padding = new Padding(0, 0, 0, 8);
            pnlDiffHeader.Size = new Size(1200, 42);
            pnlDiffHeader.TabIndex = 0;
            //
            // lblDiffObject
            //
            lblDiffObject.Dock = DockStyle.Fill;
            lblDiffObject.Name = "lblDiffObject";
            lblDiffObject.TabIndex = 0;
            lblDiffObject.Text = "Nesne seçilmedi";
            lblDiffObject.TextAlign = ContentAlignment.MiddleLeft;
            //
            // flpLegend
            //
            flpLegend.AutoSize = true;
            flpLegend.Controls.Add(tagLegendAdd);
            flpLegend.Controls.Add(tagLegendChanged);
            flpLegend.Controls.Add(tagLegendTarget);
            flpLegend.Dock = DockStyle.Right;
            flpLegend.Name = "flpLegend";
            flpLegend.TabIndex = 1;
            flpLegend.WrapContents = false;
            //
            // tagLegendAdd
            //
            tagLegendAdd.Margin = new Padding(8, 4, 0, 0);
            tagLegendAdd.Name = "tagLegendAdd";
            tagLegendAdd.Text = "+ Hedefte yok";
            tagLegendAdd.Variant = NebulaUI.Theming.NebulaVariant.Success;
            //
            // tagLegendChanged
            //
            tagLegendChanged.Margin = new Padding(8, 4, 0, 0);
            tagLegendChanged.Name = "tagLegendChanged";
            tagLegendChanged.Text = "~ Farklı";
            tagLegendChanged.Variant = NebulaUI.Theming.NebulaVariant.Warning;
            //
            // tagLegendTarget
            //
            tagLegendTarget.Margin = new Padding(8, 4, 0, 0);
            tagLegendTarget.Name = "tagLegendTarget";
            tagLegendTarget.Text = "− Sadece hedefte";
            tagLegendTarget.Variant = NebulaUI.Theming.NebulaVariant.Secondary;
            //
            // tabScript
            //
            tabScript.Controls.Add(rtbScript);
            tabScript.Controls.Add(pnlScriptToolbar);
            tabScript.Name = "tabScript";
            tabScript.Padding = new Padding(12, 8, 12, 10);
            tabScript.TabIndex = 1;
            tabScript.Text = "Güncelleme scripti";
            //
            // rtbScript
            //
            rtbScript.BorderStyle = BorderStyle.None;
            rtbScript.DetectUrls = false;
            rtbScript.Dock = DockStyle.Fill;
            rtbScript.Font = new Font("Consolas", 10F);
            rtbScript.Name = "rtbScript";
            rtbScript.ScrollBars = RichTextBoxScrollBars.Both;
            rtbScript.TabIndex = 1;
            rtbScript.Text = "";
            rtbScript.WordWrap = false;
            rtbScript.TextChanged += rtbScript_TextChanged;
            //
            // pnlScriptToolbar
            //
            pnlScriptToolbar.Controls.Add(lblScriptInfo);
            pnlScriptToolbar.Controls.Add(flpScriptButtons);
            pnlScriptToolbar.Dock = DockStyle.Top;
            pnlScriptToolbar.Name = "pnlScriptToolbar";
            pnlScriptToolbar.Padding = new Padding(0, 0, 0, 8);
            pnlScriptToolbar.Size = new Size(1224, 46);
            pnlScriptToolbar.TabIndex = 0;
            //
            // lblScriptInfo
            //
            lblScriptInfo.Dock = DockStyle.Fill;
            lblScriptInfo.Muted = true;
            lblScriptInfo.Name = "lblScriptInfo";
            lblScriptInfo.TabIndex = 0;
            lblScriptInfo.Text = "Farkları işaretleyip “Script oluştur”a bas.";
            lblScriptInfo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // flpScriptButtons
            //
            flpScriptButtons.AutoSize = true;
            flpScriptButtons.Controls.Add(btnGenerate);
            flpScriptButtons.Controls.Add(btnCopy);
            flpScriptButtons.Controls.Add(btnSave);
            flpScriptButtons.Dock = DockStyle.Right;
            flpScriptButtons.Name = "flpScriptButtons";
            flpScriptButtons.TabIndex = 1;
            flpScriptButtons.WrapContents = false;
            //
            // btnGenerate
            //
            btnGenerate.Margin = new Padding(8, 0, 0, 0);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(160, 34);
            btnGenerate.TabIndex = 0;
            btnGenerate.Text = "Script oluştur  (Ctrl+G)";
            btnGenerate.Variant = NebulaUI.Theming.NebulaVariant.Accent;
            btnGenerate.Click += btnGenerate_Click;
            //
            // btnCopy
            //
            btnCopy.Margin = new Padding(8, 0, 0, 0);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(124, 34);
            btnCopy.TabIndex = 1;
            btnCopy.Text = "Kopyala";
            btnCopy.Variant = NebulaUI.Theming.NebulaVariant.Secondary;
            btnCopy.Click += btnCopy_Click;
            //
            // btnSave
            //
            btnSave.Margin = new Padding(8, 0, 0, 0);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(140, 34);
            btnSave.TabIndex = 2;
            btnSave.Text = "Kaydet  (Ctrl+S)";
            btnSave.Variant = NebulaUI.Theming.NebulaVariant.Primary;
            btnSave.Click += btnSave_Click;
            //
            // pnlStatus
            //
            pnlStatus.Controls.Add(flpStatus);
            pnlStatus.Controls.Add(lblSelection);
            pnlStatus.Dock = DockStyle.Fill;
            pnlStatus.Margin = new Padding(0, 4, 0, 0);
            pnlStatus.Name = "pnlStatus";
            pnlStatus.TabIndex = 3;
            //
            // flpStatus
            //
            flpStatus.Controls.Add(ledStatus);
            flpStatus.Controls.Add(lblStatus);
            flpStatus.Dock = DockStyle.Fill;
            flpStatus.Name = "flpStatus";
            flpStatus.TabIndex = 0;
            flpStatus.WrapContents = false;
            //
            // ledStatus
            //
            ledStatus.Margin = new Padding(0, 4, 8, 0);
            ledStatus.Name = "ledStatus";
            ledStatus.Size = new Size(16, 16);
            ledStatus.TabIndex = 0;
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Margin = new Padding(0, 4, 0, 0);
            lblStatus.Muted = true;
            lblStatus.Name = "lblStatus";
            lblStatus.TabIndex = 1;
            lblStatus.Text = "-";
            //
            // lblSelection
            //
            lblSelection.AutoSize = true;
            lblSelection.Dock = DockStyle.Right;
            lblSelection.Muted = true;
            lblSelection.Name = "lblSelection";
            lblSelection.Padding = new Padding(0, 4, 0, 0);
            lblSelection.TabIndex = 1;
            lblSelection.Text = "";
            //
            // MainForm
            //
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 820);
            Controls.Add(tlpRoot);
            KeyPreview = true;
            MinimumSize = new Size(1000, 680);
            Name = "MainForm";
            Padding = new Padding(16, 4, 16, 10);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SQL Schema Diff";
            tlpRoot.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            cardSources.ResumeLayout(false);
            tlpFiles.ResumeLayout(false);
            tlpFiles.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            cardDifferences.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridDifferences).EndInit();
            pnlDiffToolbar.ResumeLayout(false);
            pnlDiffToolbar.PerformLayout();
            flpKindTags.ResumeLayout(false);
            flpKindTags.PerformLayout();
            flpSelectionButtons.ResumeLayout(false);
            tabBottom.ResumeLayout(false);
            tabDiff.ResumeLayout(false);
            pnlDiffHeader.ResumeLayout(false);
            pnlDiffHeader.PerformLayout();
            flpLegend.ResumeLayout(false);
            flpLegend.PerformLayout();
            tabScript.ResumeLayout(false);
            pnlScriptToolbar.ResumeLayout(false);
            pnlScriptToolbar.PerformLayout();
            flpScriptButtons.ResumeLayout(false);
            pnlStatus.ResumeLayout(false);
            pnlStatus.PerformLayout();
            flpStatus.ResumeLayout(false);
            flpStatus.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRoot;
        private Panel pnlHeader;
        private NebulaUI.Controls.NebulaLabel lblTitle;
        private NebulaUI.Controls.NebulaLabel lblSubtitle;
        private NebulaUI.Containers.NebulaCard cardSources;
        private TableLayoutPanel tlpFiles;
        private NebulaUI.Controls.NebulaLabel lblSource;
        private NebulaUI.Controls.NebulaTextBox txtSourcePath;
        private NebulaUI.Controls.NebulaButton btnBrowseSource;
        private NebulaUI.Controls.NebulaLabel lblTarget;
        private NebulaUI.Controls.NebulaTextBox txtTargetPath;
        private NebulaUI.Controls.NebulaButton btnBrowseTarget;
        private NebulaUI.Controls.NebulaButton btnCompare;
        private NebulaUI.Data.NebulaSplitContainer splitMain;
        private NebulaUI.Containers.NebulaCard cardDifferences;
        private NebulaUI.Data.NebulaDataGridView gridDifferences;
        private DataGridViewCheckBoxColumn colApply;
        private DataGridViewTextBoxColumn colKind;
        private DataGridViewTextBoxColumn colObject;
        private DataGridViewTextBoxColumn colDetail;
        private Panel pnlDiffToolbar;
        private FlowLayoutPanel flpKindTags;
        private NebulaUI.Controls.NebulaLabel lblDiffHint;
        private FlowLayoutPanel flpSelectionButtons;
        private NebulaUI.Controls.NebulaButton btnSelectAll;
        private NebulaUI.Controls.NebulaButton btnClearSelection;
        private NebulaUI.Containers.NebulaTabControl tabBottom;
        private TabPage tabDiff;
        private SqlSchemaDiff.Views.SchemaDiffView diffView;
        private Panel pnlDiffHeader;
        private NebulaUI.Controls.NebulaLabel lblDiffObject;
        private FlowLayoutPanel flpLegend;
        private NebulaUI.Controls.NebulaTagLabel tagLegendAdd;
        private NebulaUI.Controls.NebulaTagLabel tagLegendChanged;
        private NebulaUI.Controls.NebulaTagLabel tagLegendTarget;
        private TabPage tabScript;
        private NebulaUI.Controls.NebulaRichTextBox rtbScript;
        private Panel pnlScriptToolbar;
        private NebulaUI.Controls.NebulaLabel lblScriptInfo;
        private FlowLayoutPanel flpScriptButtons;
        private NebulaUI.Controls.NebulaButton btnGenerate;
        private NebulaUI.Controls.NebulaButton btnCopy;
        private NebulaUI.Controls.NebulaButton btnSave;
        private Panel pnlStatus;
        private FlowLayoutPanel flpStatus;
        private NebulaUI.Controls.NebulaLed ledStatus;
        private NebulaUI.Controls.NebulaLabel lblStatus;
        private NebulaUI.Controls.NebulaLabel lblSelection;
    }
}
