using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleImageViewer
{
    internal class EventHandlers
    {
       internal static void AsignarAccionesBotonesMenuTopAlIniciar(Form1 formulario)
        {
            formulario.abrirOpenDialogFile.Click += formulario.AbrirOpenDialogFile_Click;
            formulario.closeStripMenuItem.Click += formulario.CerrarImagen;
            formulario.exitStripMenuItem.Click += formulario.SalirApp;
            formulario.viewFullSizeStripMenuItem.Click += formulario.FullSizeImage;
            formulario.fullScreenStripMenuItem.Click += formulario.EnterFullScreen;
            formulario.flipHMenuItem.Click += formulario.flipHImagen;
            formulario.flipVMenuItem.Click += formulario.flipVImagen;
            formulario.saveImageStripMenuItem.Click += formulario.savePictureBox;
            formulario.saveSelectionMenuItem.Click += formulario.SaveSelection;
            formulario.selectToolMenuItem.Click += formulario.SelectToolStart;
            formulario.openFileLocationMenuItem.Click += formulario.abrirEnExplorer;
        
        }
        
        internal static void AsignarAccionBtnMenu( ToolStripMenuItem toolStripMenuItem, EventHandler accion)
        {
            toolStripMenuItem.Click += accion;
            
        }

        internal static void AccionesMouseFullSizeImagen(Form1 formulario, Control control)
        {
            control.MouseDown += formulario.IniciaDrag;
            control.MouseMove += formulario.MueveDrag;
            control.MouseUp += formulario.UpDrag;
            control.MouseWheel +=formulario.PictureBox1_MouseWheel; //se asigna la funcion de zoom al mouse wheel
        }

        internal static void RetirarAccionBtnMenu()
        {
        
        }

    }
}
