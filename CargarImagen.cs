using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SimpleImageViewer
{
    internal class CargarImagen
    {
        public CargarImagen(Form1 form1, string nombreArchivo)
        {
            NombreImgTitulo(form1, nombreArchivo);

           


        }

        public void NombreImgTitulo(Form1 form1, string nombreArchivo)
        {
            form1.Text = "Image Viewer: " + nombreArchivo;

        }

        

    }
}
