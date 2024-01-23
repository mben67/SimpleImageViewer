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
    }
}
