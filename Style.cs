using System.Drawing;
using System.Windows.Forms;

namespace SimpleImageViewer
{
    internal class Style
    {
        public void Dimensiones(Form1 formulario, int ancho, int alto)
        {
            formulario.Width = ancho;
            formulario.Height = alto;
        }

        public void EstadoInicial(Form1 form1)
        {
            form1.WindowState = FormWindowState.Normal;
        }

        public void ColorDeFondo(Form1 formulario, int r, int g, int b)
        {
            Color nuevoColor = Color.FromArgb(r, g, b);
            formulario.BackColor = nuevoColor;
        }

        public void ColorFondoPictureBox(PictureBox pictureBox, int r, int g, int b)
        {
            Color color = Color.FromArgb(r, g, b);
            pictureBox.BackColor = color;


        }

        public void ColorFondoMenuStrip(MenuStrip menuStrip, int r, int g, int b)
        {
            Color color = Color.FromArgb(r,g,b);
            menuStrip.BackColor = color;
        }


        public void RemueveAreaGrisSubmenus(MenuStrip menuStrip) 
        {

            foreach (ToolStripMenuItem menuItem in menuStrip.Items)
            {
                if (menuItem.DropDown is ToolStripDropDownMenu dropDownMenu)
                {
                    dropDownMenu.ShowImageMargin = false;
                }
            }

        }

        //public void Propiedades
    }
}
