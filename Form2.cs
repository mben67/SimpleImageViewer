using Imazen.WebP;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ImageViewer
{
    public partial class Form2 : Form
    {
        Dictionary<string, string> imageList = new Dictionary<string, string>();

        int currentIndice;
        int NuevoIndex;
        string siguienteImagen = null;
        string prevImagen = null;
        string actualImagen;

        ToolTip ttip = new ToolTip();
        ContextMenuStrip cm = new ContextMenuStrip();
        ToolStripItem item1;

        Bitmap bmp;

        public Form2(string img, string indice, Dictionary<string, string> newlist, int currentIndex)
        {
            InitializeComponent();


            this.Text = img;

            /* se carga la imagen */
            cargaImagen(img);



            //se añade el tooltip
            ttip.SetToolTip(pictureBoxFull, img);

            actualImagen = img;
            currentIndice = 1;
            imageList = newlist;

            string indiceActual = "index" + currentIndex.ToString();

            if (imageList.ContainsKey(indiceActual))
            {
                NuevoIndex = currentIndex;
            }
            else
            {
                MessageBox.Show("Imagen No Encontrada");
            }
            
        }

        /* 
         * función que permite buscar la imágen en el diccionario que 
         * se recibe desde el form1
         */
        public string BuscaImage(string indexBusca, Dictionary<string, string> newlist)
        {
            if (newlist.ContainsKey("index" + indexBusca))
            {
                //muestra el value del key index1, index2, etc
                return newlist["index" + indexBusca];
            }
            else
            {
                return indexBusca;
            }
        }


        public void KeyEvent(object sender, KeyEventArgs e) //Keyup Event 
        {
            int totalImagenes = imageList.Count;


            if (e.KeyCode == Keys.Right)
            {
                //se vacia el tooltip
                ttip.SetToolTip(pictureBoxFull, null);

                //se quita la imagen del pictureBox para cargar la nueva
                pictureBoxFull.Image.Dispose();

                //se actualiza el current indice
                currentIndice = NuevoIndex;
                NuevoIndex = currentIndice + 1;

                /*
                 * se remueve el elemento 0 del menu contextual para volver a crearlo 
                 * y evitar que se acumulen cuando se navega con las flechas de direccion
                 */
                cm.Items.RemoveAt(0);


                /*
                 * si se llega al final del total de imagenes se repite desde el inicio
                 * reseteando el NuevoIndex
                 */

                if (NuevoIndex > totalImagenes)
                {
                    //reset Nuevo Index para volver al principio
                    NuevoIndex = 1;

                    //se busca la siguiente imagen en el diccionario
                    siguienteImagen = BuscaImage(NuevoIndex.ToString(), imageList);

                    //se quita la imagen para cargar la nueva
                    pictureBoxFull.Image = null;

                    this.Text = siguienteImagen;

                    //se carga la nueva imagen
                    cargaImagen(siguienteImagen);

                    //se lena el tooltip con la nueva info
                    ttip.SetToolTip(pictureBoxFull, siguienteImagen);


                    /* se crea el nuevo elemento index 0 del menu contextual y 
                     * se le asigna funcion con nueva variable
                     */
                    item1 = cm.Items.Add("Open file in explorer");
                    item1.Click += (s, f) => functionOpen(s, e, siguienteImagen);

                }
                else
                {
                    siguienteImagen = BuscaImage(NuevoIndex.ToString(), imageList);
                    pictureBoxFull.Image = null;


                    this.Text = siguienteImagen;

                    //se carga la imagen
                    cargaImagen(siguienteImagen);

                    ttip.SetToolTip(pictureBoxFull, siguienteImagen);

                    item1 = cm.Items.Add("Open file in explorer");
                    item1.Click += (s, f) => functionOpen(s, e, siguienteImagen);
                }
            }

            //se pulsa la tecla izquierda y se va a la imagen anterior en el diccionario
            if (e.KeyCode == Keys.Left)
            {
                ttip.SetToolTip(pictureBoxFull, null);
                pictureBoxFull.Image.Dispose();
                currentIndice = NuevoIndex;
                NuevoIndex = currentIndice - 1;
                cm.Items.RemoveAt(0);

                /*
                 * si se regresa hasta la primera imagen se sigue con la ultima
                 * para que no se rompa la navegación
                 */
                if (NuevoIndex == 0)
                {
                    /*
                     * se asigna el nuevo index para poder cargar la ultima imagen 
                     * cuando se regresa hasta la primera
                     */

                    NuevoIndex = totalImagenes;

                    // se busca la imagen en el dictionary
                    prevImagen = BuscaImage(NuevoIndex.ToString(), imageList);

                    //se remueve la imagen anterior para cargar la nueva
                    pictureBoxFull.Image = null;

                    this.Text = prevImagen;


                    //pictureBoxFull.ImageLocation = prevImagen;
                    cargaImagen(prevImagen);

                    //se actualiza el tooltip
                    ttip.SetToolTip(pictureBoxFull, prevImagen);

                    //se actualiza el context menu item
                    item1 = cm.Items.Add("Open file in explorer");
                    item1.Click += (s, f) => functionOpen(s, e, prevImagen);
                }
                else
                {
                    prevImagen = BuscaImage(NuevoIndex.ToString(), imageList);

                    //se quita la imagen que estaba para reemplazarla por la nueva
                    pictureBoxFull.Image = null;

                    this.Text = prevImagen;

                    //se carga la imagen
                    cargaImagen(prevImagen);

                    // se actualiza el tooltip 
                    ttip.SetToolTip(pictureBoxFull, prevImagen);

                    //se actualiza el context menu item
                    item1 = cm.Items.Add("Open file in explorer");
                    item1.Click += (s, f) => functionOpen(s, e, prevImagen);
                }
            }
        }

        // asigna el cerrar el fullscreen con la tecla esc
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                pictureBoxFull.Image.Dispose();
                pictureBoxFull.Dispose();
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        public void Form2_Load(object sender, EventArgs e)
        {
            //es el que permite que el form registre los eventos de keyboard
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyEvent);

            // se añade el elemento 0 al menu contextual
            item1 = cm.Items.Add("Open file in explorer");

            //se añade la funcion al elemento 0 del menu contextual
            //item1.Click += new EventHandler((s, f) => functionOpen(s, e, actualImagen));
            item1.Click += (s, f) => functionOpen(s, e, actualImagen);

            //se añade el menu contextual al picturebox
            pictureBoxFull.ContextMenuStrip = cm;
        }

        protected void functionOpen(object sender, EventArgs s, string valueItem)
        {
            /* 
             * se recibe el valueItem desde el evento click 
             * y contiene la ruta de la imagen a abrir en el explorer
             */

            Process.Start("explorer.exe", string.Format("/select,\"{0}\"", valueItem.ToString()));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Opacity == 1)
            {
                timer1.Stop();
            }
            Opacity += .2;
        }

        public void timer2_Tick(object sender, EventArgs e)
        {
            Opacity -= .2;
        }

        /* funcion que permite cargar la imagen en el pictureboxfull */
        private void cargaImagen(string sourceImg)
        {
            string soloNombre = Path.GetFileName(sourceImg);
            string extension = Path.GetExtension(soloNombre);

            if (extension == ".webp")
            {
                var bytes = File.ReadAllBytes(sourceImg);
                SimpleDecoder decoder = new SimpleDecoder();
                //var bytes = File.ReadAllBytes(nombreImagen);
                bmp = decoder.DecodeFromBytes(bytes, bytes.Length);

                if (pictureBoxFull.Image != null)
                {
                    pictureBoxFull.Image.Dispose();
                }
                else
                {
                    pictureBoxFull.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBoxFull.Tag = siguienteImagen;
                    pictureBoxFull.Name = null;
                    pictureBoxFull.Name = currentIndice.ToString();
                    ttip.SetToolTip(pictureBoxFull, sourceImg);
                    pictureBoxFull.Image = bmp;
                }
            }//fin de .webp
            else
            {
                using (Image imgFromFile = Image.FromFile(sourceImg))
                {
                    bmp = new Bitmap(sourceImg);

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawImage(imgFromFile, 0, 0);
                    }

                    if (pictureBoxFull.Image != null)
                    {
                        pictureBoxFull.Image.Dispose();
                    }
                    else
                    {
                        pictureBoxFull.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBoxFull.Tag = siguienteImagen;
                        pictureBoxFull.Name = null;
                        pictureBoxFull.Name = currentIndice.ToString();
                        ttip.SetToolTip(pictureBoxFull, sourceImg);
                        pictureBoxFull.Image = bmp;
                    }

                }//fin using
            }
        }//fin function


    }
}