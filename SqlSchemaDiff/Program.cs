using NebulaUI.Dialogs;
using NebulaUI.Theming;

namespace SqlSchemaDiff;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // NebulaUI: menüler temalı, mesaj kutusu butonları Türkçe
        NebulaToolStripRenderer.ApplyGlobally();
        NebulaMessageBox.Texts = NebulaMessageBoxTexts.Turkish;

        Application.Run(new MainForm());
    }
}
