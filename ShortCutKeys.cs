using System.Windows.Forms;

namespace SimpleImageViewer
{
    internal class ShortCutKeys
    {
        internal static void AsignarTeclasAccesoRapido(ToolStripMenuItem toolStripMenuItem, Keys shortcutKeys)
        {
            toolStripMenuItem.ShortcutKeys = shortcutKeys;
            toolStripMenuItem.ShowShortcutKeys = true;
        }

        internal static void QuitarTeclasAccesoRapido(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.ShortcutKeys = Keys.None;
            toolStripMenuItem.ShowShortcutKeys = true;
        }
    }
}
