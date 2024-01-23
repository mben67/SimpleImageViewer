using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleImageViewer
{
    class EstadosMenus
    {
        public void EstadoMenu(ToolStripMenuItem toolStripMenuItem, Boolean estado )
        {
            toolStripMenuItem.Enabled = estado;
        }
    }




}
