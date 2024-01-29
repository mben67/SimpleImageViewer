using System;
using System.Windows.Forms;

namespace SimpleImageViewer
{
    class EstadosMenus
    {
        public static void MenuHabilitado(ToolStripMenuItem toolStripMenuItem, Boolean estado)
        {
            toolStripMenuItem.Enabled = estado;
        }

        public static void VisibilidadMenu(ToolStripMenuItem toolStripMenuItem, Boolean estado)
        {
            toolStripMenuItem.Visible = estado;
        }
    }




}
