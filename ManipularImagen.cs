using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using ImageViewer;
using System.Text.RegularExpressions;

namespace SimpleImageViewer
{
    internal class ManipularImagen
    {
        

        internal static void FlipHorizontal(PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox.Invalidate();
            }
        }

        internal static void FlipVertical(PictureBox pictureBox) 
        {  
            if (pictureBox.Image != null) 
            {
                pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                pictureBox.Invalidate();
            }
        }

        //se declaran las extensione que se aceptan de imagenes
        private static readonly string[] DefaultExtensions = { "jpg", "jpeg", "png", "tiff", "jfif", "webp" };


        internal static void ImagenesADirectorio(Form form ,Control c, string[] ruta, Dictionary<string, string> dictionary, string folderPath,int actulIndice)
        {

           

            ruta = EnumerateFilesWithDefaultExtensions(folderPath);

            foreach (string file in ruta)
            {
                c.Name = actulIndice.ToString();
                ++actulIndice;
                dictionary.Add("index" + actulIndice, file.ToString());
            }

            //MessageBox.Show(actulIndice.ToString());

        }



        internal static string[] EnumerateFilesWithDefaultExtensions(string folderPath)
        {

            return EnumerateFilesWithExtensions(folderPath, DefaultExtensions);
        }

        /*
         * Se crea el directorio con el diccionario de la imagenes ordenadas que contiene la carpeta
         */
        internal static string[] EnumerateFilesWithExtensions(string folderPath, params string[] extensions)
        {

            try
            {
                //por nombre y fecha de modificacion
                return Directory.EnumerateFiles(folderPath, "*.*")
                                .OrderBy(f => PadNumbers( f))
                                //ThenBy(f => new FileInfo(f).LastWriteTime)//fecha de modificacion
                                .Where(f => extensions.Contains(f.Split('.').Last().ToLower()))
                                .ToArray();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones 
                MessageBox.Show(ex.Message);
                return Array.Empty<string>(); // o null, dependiendo de tu lógica
            }
        }



        internal static void ManipularNombreImagen(string myLlave, int numeroParte,PictureBox pictureBox, int indiceActual) 
        {
            //se divide la parte numerica y alfabetica del index[i]
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(myLlave);

            //string alphaPart = result.Groups[1].Value;
            numeroParte = Int32.Parse(result.Groups[2].Value);
            pictureBox.Name = numeroParte.ToString();

            //se usa el indice de la imagen que se encontro como indice actual
            indiceActual = numeroParte;
        }

        //metodo que ordena las imagenes muy similar a como las ordena el explorador de windows
        internal static string PadNumbers(string input)
        {
            return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));
        }

        internal static void PantallaCompleta(PictureBox pictureBox, Dictionary<string, string> dictionary, int indice)
        {
            Form2 f2 = new Form2(pictureBox.Tag.ToString(), pictureBox.Name, dictionary, indice);
            f2.Show();
        }
    }
}
