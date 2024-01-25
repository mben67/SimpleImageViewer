using ImageViewer;
using Imazen.WebP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace SimpleImageViewer
{
    public partial class Form1 : Form
    {
        int NuevoIndex;
        int numberPart;
        int currentIndice;
        int i = 0;
        //int s = 100;

        string siguienteImagen = null;
        string prevImagen = null;
        string nombreImagen;
        string myKey;
        string[] files;
        string archivoInicial;
        string folderName = null;// aqui se guarda la variable para el path del folder seleccionado

        Bitmap bmp;
        
        ToolTip ttip = new ToolTip();

        //se declara la lista vacia que se llenara al leer la carpeta de imagenes
        Dictionary<string, string> imagelist = new Dictionary<string, string>();

        private bool isDragging = false;
        private int currentX;
        private int currentY;

        //Clases externas propias
       
        readonly EstadosMenus estadosMenus = new();
        readonly ModifImagen modifImagen = new();

        Version version = Assembly.GetExecutingAssembly().GetName().Version;


        public Form1(string recibido)
        {
            archivoInicial = recibido;
            InitializeComponent();

            /*
             * se llama a la clase style para setear el estado inicial y las dimensiones del form y su color de fondo
             */
            
            Style.EstadoInicial(this);
            Style.Dimensiones(this,1280,720);//default (1280,720)
            Style.ColorDeFondo(this, 238, 243, 250);//default (238,243,250)
           
            //Clase Style para backcolor  of the picturebox
            Style.ColorFondoPictureBox(pictureBox1, 238, 243, 250);//default(238,243,250)
            
            //Class style para back color de menu superior y remover el area gris de los items del menu superior
            Style.RemueveAreaGrisSubmenus(menuStrip1);
            Style.ColorFondoMenuStrip(menuStrip1,216,229,242);//default (216,229,242)
            


            //se setean las propiedades del opendilaogo para seleccionar file
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog1.Title = "Select Image";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "All Type Supported (*.jpg; *.jpeg; *.png; *.jfif; *.bmp; *.webp)|*.jpg; *.jpeg; *.png; *.jfif; *.bmp; *.webp |"+
                                     "Jpeg Image|*.jpeg|" +
                                     "Jpg Image|*.jpg|" +
                                     "Png Image|*.png|" +
                                     "Webp Image|*.webp|" +
                                     "Bitmap Image|*.bmp|" +
                                     "Gif Image|*.gif|" +
                                     "Tiff Image|*.tiff|" +
                                     "Jfif Image|*.jfif";


            /*
             * Si no hay imagen cargada se habilitan y deshabilitan los siguientes botones del menu top
             * se usa clase EstadosMenus
             */

            //Menu File
            estadosMenus.EstadoMenu(abrirOpenDialogFile, true);
            estadosMenus.EstadoMenu(closeStripMenuItem, false);
            estadosMenus.EstadoMenu(exitStripMenuItem, true);
           

            //Menu View
            estadosMenus.EstadoMenu(viewFullSizeStripMenuItem, false);
            estadosMenus.EstadoMenu(fullScreenStripMenuItem, false);
            estadosMenus.EstadoMenu(openFileLocationMenuItem, false);
            
            //Menu Modify
            estadosMenus.EstadoMenu(flipHMenuItem, false);
            estadosMenus.EstadoMenu(flipVMenuItem, false);
            estadosMenus.EstadoMenu(saveImageStripMenuItem, false);
            estadosMenus.EstadoMenu(selectToolMenuItem, false);
            estadosMenus.EstadoMenu(saveSelectionMenuItem,false);


            /*
             * se asignan funciones a los botones del menu top
             */

            EventHandlers.AsignarAccionesBotonesMenuTopAlIniciar(this);

            /*
             * se asignan ShorcutsKeys a los menus del top menu
             */
            ShortCutKeys.AsignarTeclasAccesoRapido(abrirOpenDialogFile, Keys.Control | Keys.O);//abrir archivo
            ShortCutKeys.AsignarTeclasAccesoRapido(exitStripMenuItem, Keys.Control | Keys.E);//cerrar programa


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            //se actualiza la barra de titulo sin ombre de archivo
            Style.NombreImgTitulo(this,null);

            //es el que permite que el form registre los eventos de keyboard
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyEvent);

            if (archivoInicial != null)
            {
                folderName = Path.GetDirectoryName(archivoInicial);
                CargarImg(archivoInicial);
            }
            else
            {
                return;
            }
        }


        internal void SelectToolStart(object sender, EventArgs e)
        {
            var s = 100;
            var c = new FrameControl();
            c.Size = new Size(s, s);
            c.Location = new Point((pictureBox1.Width - s) / 2, (pictureBox1.Height - s) / 2);
            pictureBox1.Controls.Add(c);

            pictureBox1.Invalidate();
        }

        
        //para la seleccion
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.HasChildren == true)
            {
                e.Graphics.ExcludeClip(pictureBox1.Controls[0].Bounds);
                using (var b = new SolidBrush(Color.FromArgb(100, Color.FromArgb(128, 72, 145, 220))))//default(128,72,145,220)
               
                {
                    e.Graphics.FillRectangle(b, pictureBox1.ClientRectangle);
                }
               
            }
        }

        internal void SalirApp(object sender, EventArgs e)
        {
            Application.Exit();
        }


        internal void AbrirOpenDialogFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = null;
            openFileDialog1.FileName = "";
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }

                nombreImagen = openFileDialog1.FileName;
                folderName = Path.GetDirectoryName(nombreImagen);

                CargarImg(nombreImagen);
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        public void CargarImg(string nombreImagen)
        {

            //se invca a la clase Style para mostrar la ruta de la imagen en la barra de titulo
            Style.NombreImgTitulo(this, nombreImagen);



            string soloNombre = Path.GetFileName(nombreImagen);
            string extension = Path.GetExtension(soloNombre);

            if (extension == ".webp")
            {
                /*se inicia deco de webp image */
                var webpDecoder = new SimpleDecoder();

                using (Stream inputStream = File.Open(nombreImagen, FileMode.Open))
                {
                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }

                        var bytes = ms.ToArray();
                        bmp = webpDecoder.DecodeFromBytes(bytes, bytes.LongLength);
                    }
                }/* termina webp decode */

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                else
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Tag = nombreImagen;
                    ttip.SetToolTip(pictureBox1, nombreImagen);
                    
                }
            }// se acaba el if de si es imagen .webp
            else
            {
                /* 
                 * Si la imagen no es Webp
                 * inicia carga de Imagen normal 
                 */
               // this.Text = "Image Viewer - Ver: "+version.ToString()+" - "+nombreImagen;
               // CargarImagen.NombreImgTitulo(this, nombreImagen);

                using (Image imgFromFile = Image.FromFile(nombreImagen))
                {
                    var destRect = new Rectangle(0, 0, 0, 0);
                    bmp = new Bitmap(imgFromFile, imgFromFile.Width, imgFromFile.Height);
                    bmp.SetResolution(imgFromFile.HorizontalResolution, imgFromFile.VerticalResolution);

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CompositingMode = CompositingMode.SourceCopy;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.DrawImage(imgFromFile, 0, 0);
                    }

                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                    }
                    else
                    {
                        pictureBox1.Tag = nombreImagen;
                        ttip.SetToolTip(pictureBox1, nombreImagen);
                        
                    }
                }
            }

            ManipularImagen.ImagenesADirectorio(this,pictureBox1,files,imagelist,folderName,i);

           /*string[] extensions = { "jpg", "jpeg", "png", "tiff", "jfif", "webp" };
            files = Directory.EnumerateFiles(folderName, "*.*")
           .OrderBy(f =>  f).
           Where(f => extensions.Contains(f.Split('.').Last()
           .ToLower())).ToArray();

            foreach (string file in files)
            {
                pictureBox1.Name = i.ToString();
                ++i;
                imagelist.Add("index" + i, file.ToString());
            }*/



            /* se usa la ruta de la imagen como valor para buscar el indice
             que ocupa la iagen en el listado de imagenes del directorio
            para extraerle datos como el indice que ocupa */

            /*se busca la imagen*/
            myKey = imagelist.FirstOrDefault(x => x.Value == nombreImagen).Key;

            /*se divide la parte numerica y alfabetica del index[i]*/
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(myKey);

            string alphaPart = result.Groups[1].Value;
            numberPart = Int32.Parse(result.Groups[2].Value);

            pictureBox1.Name = numberPart.ToString();

            //se usa el indice de la imagen que se encontro como indice actual
            currentIndice = numberPart;

            //se carga la imagen en el picturebox
            pictureBox1.Image = bmp;
            
            /*
             * por último se habilitan y dehabilitan los estados de los menus
             * cuando hay imagen cargada con la clase EstadosMenus y la funcion estadosMenu
             */

            
            /*
             * Menu File
             */

            //se deshabilita el boton de abrir imagen
            estadosMenus.EstadoMenu(abrirOpenDialogFile, false);
            ShortCutKeys.QuitarTeclasAccesoRapido(abrirOpenDialogFile);

            //se habilita el boton de cerrar imagen
            estadosMenus.EstadoMenu(closeStripMenuItem, true);
            ShortCutKeys.AsignarTeclasAccesoRapido(closeStripMenuItem, Keys.Control | Keys.Q);

           
            /*
             * Menu View
             */

            //se habilita el boton y shortcut para ver la imagen en tamaño real sin entrar a full screen
            estadosMenus.EstadoMenu(viewFullSizeStripMenuItem, true);
            ShortCutKeys.AsignarTeclasAccesoRapido(viewFullSizeStripMenuItem, Keys.Control | Keys.F);//ver tamaño real

            //se habilita el boton y shortcut para ver la imagen en full screen
            estadosMenus.EstadoMenu(fullScreenStripMenuItem, true);
            ShortCutKeys.AsignarTeclasAccesoRapido(fullScreenStripMenuItem, Keys.Alt | Keys.F);

            //se habilita el boton y shortcut para ver la imagen en el explorador de archivos
            estadosMenus.EstadoMenu(openFileLocationMenuItem, true);
            ShortCutKeys.AsignarTeclasAccesoRapido(openFileLocationMenuItem, Keys.Control | Keys.E);


            /*
             * Menu Modify
             */

            //se habilita el boton y shortcut para girar horizontalmente la imagen
            estadosMenus.EstadoMenu(flipHMenuItem,true);
            ShortCutKeys.AsignarTeclasAccesoRapido(flipHMenuItem, Keys.Control | Keys.H);
            
            //se habilita el boton para girar verticalmente la imagen
            estadosMenus.EstadoMenu(flipVMenuItem,true);
            ShortCutKeys.AsignarTeclasAccesoRapido(flipVMenuItem, Keys.Control | Keys.V);
            
            //se habilita el botn para salvar imagen como
            estadosMenus.EstadoMenu(saveImageStripMenuItem,true);
            ShortCutKeys.AsignarTeclasAccesoRapido(saveImageStripMenuItem, Keys.Control | Keys.S);

            //se habilita el boton de herramienta de seleccion
            estadosMenus.EstadoMenu(selectToolMenuItem, true);
            ShortCutKeys.AsignarTeclasAccesoRapido(selectToolMenuItem, Keys.Control | Keys.T);

            //se habilita el boton de Salvar seleccion
            estadosMenus.EstadoMenu(saveSelectionMenuItem, false);
            ShortCutKeys.AsignarTeclasAccesoRapido(saveSelectionMenuItem, Keys.None);

        
        }


        


        internal void EnterFullScreen(object sender, EventArgs e) 
        {

            Form2 f2 = new Form2(pictureBox1.Tag.ToString(), pictureBox1.Name, imagelist, currentIndice);
            f2.Show();
        }


        public void KeyEvent(object sender, KeyEventArgs e) //Keyup Event 
        {
            int totalImagenes = imagelist.Count;


            /* si no esta vacia el picturebox y si esta en modo libre se activa los shortcut */
            if (pictureBox1.Image != null & pictureBox1.Dock != DockStyle.None)
            {
                if (e.KeyCode == Keys.Right)
                {
                    //se vacia el tooltip
                    ttip.SetToolTip(pictureBox1, null);

                    //se quita la imagen del pictureBox para cargar la nueva
                    pictureBox1.Image.Dispose();

                    //se actualiza el current indice
                    NuevoIndex = currentIndice + 1;
                    currentIndice = NuevoIndex;


                    /*
                     * si se llega al final del total de imagenes se repite desde el inicio
                     * reseteando el NuevoIndex
                     */

                    if (NuevoIndex > totalImagenes)
                    {
                        //reset Nuevo Index para volver al principio
                        currentIndice = 1;


                        siguienteImagen = null;
                        siguienteImagen = BuscaImage2(currentIndice.ToString(), imagelist);
                        pictureBox1.Image = null;

                        //se actualiza el nombre de archivo en la barra de titulo 
                        Style.NombreImgTitulo(this,siguienteImagen);


                        /*inicia codigo de webp image*/

                        string soloNombre = Path.GetFileName(siguienteImagen);
                        string extension = Path.GetExtension(soloNombre);


                        if (extension == ".webp")
                        {
                            CargarWebp(siguienteImagen);
                        }
                        else
                        { 
                            /*acaba codigo de webp image*/



                            /*inicia codigo de prueba*/

                            using (Image imgFromFile = Image.FromFile(siguienteImagen))
                            {
                                bmp = new Bitmap(siguienteImagen);
                                bmp.SetResolution(imgFromFile.HorizontalResolution, imgFromFile.VerticalResolution);

                                using (Graphics g = Graphics.FromImage(bmp))
                                {
                                    g.CompositingMode = CompositingMode.SourceCopy;
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                                    g.DrawImage(imgFromFile, 0, 0);
                                }

                                if (pictureBox1.Image != null)
                                {
                                    pictureBox1.Image.Dispose();
                                }
                                else
                                {
                                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    pictureBox1.Tag = siguienteImagen;

                                    pictureBox1.Name = null;
                                    pictureBox1.Name = currentIndice.ToString();

                                    ttip.SetToolTip(pictureBox1, siguienteImagen);

                                    pictureBox1.Image = bmp;
                                }

                            }
                        }
                        /*termina codigo de prueba*/
                    }
                    else
                    {

                        //MessageBox.Show(string.Join(Environment.NewLine, imagelist));
                        siguienteImagen = null;
                        siguienteImagen = BuscaImage2(currentIndice.ToString(), imagelist);
                        pictureBox1.Image = null;

                        //se actualiza el nombre del archivo en la barra de titulo
                        Style.NombreImgTitulo(this,siguienteImagen);
                        

                        string soloNombre = Path.GetFileName(siguienteImagen);
                        string extension = Path.GetExtension(soloNombre);
                        
                        if (extension == ".webp")
                        {
                            CargarWebp(siguienteImagen);
                        }
                        else
                        {
                            /* inicia carga de Imagen */
                            using (Image imgFromFile = Image.FromFile(siguienteImagen))
                            {

                                bmp = new Bitmap(siguienteImagen);
                                bmp.SetResolution(imgFromFile.HorizontalResolution, imgFromFile.VerticalResolution);

                                using (Graphics g = Graphics.FromImage(bmp))
                                {
                                    g.CompositingMode = CompositingMode.SourceCopy;
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                                    g.DrawImage(imgFromFile, 0, 0);
                                }

                                if (pictureBox1.Image != null)
                                {
                                    pictureBox1.Image.Dispose();
                                }
                                else
                                {
                                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    pictureBox1.Tag = siguienteImagen;

                                    ttip.SetToolTip(pictureBox1, siguienteImagen);

                                    pictureBox1.Name = null;
                                    pictureBox1.Name = currentIndice.ToString();
                                    pictureBox1.Image = bmp;
                                }
                            } /* termina carga de Imagen */
                        }

                    }
                }

                //se pulsa la tecla izquierda y se va a la imagen anterior en el diccionario
                if (e.KeyCode == Keys.Left)
                {

                    ttip.SetToolTip(pictureBox1, null);

                    NuevoIndex = currentIndice - 1;
                    currentIndice = NuevoIndex;

                    pictureBox1.Image.Dispose();

                    /*
                     * si se regresa hasta la primera imagen se sigue con la ultima
                     * para que no se rompa la navegación
                     */
                    if (currentIndice == 0)
                    {
                        /*
                         * se asigna el nuevo index para poder cargar la ultima imagen 
                         * cuando se regresa hasta la primera
                         */

                        currentIndice = totalImagenes;

                        // se busca la imagen en el dictionary y se actualiza la variable
                        prevImagen = null;
                        prevImagen = BuscaImage2(currentIndice.ToString(), imagelist);
                        pictureBox1.Image = null;


                        //se actualiz el nombre del archivo en la barra de titulo
                        Style.NombreImgTitulo(this,prevImagen);
                        
                        /*
                         * se carga la imagen en el picturebox
                         */
                        string soloNombre = Path.GetFileName(prevImagen);
                        string extension = Path.GetExtension(soloNombre);


                        if (extension == ".webp")
                        {
                            CargarWebp(prevImagen);
                        }
                        else
                        {
                            /* si no es webp image*/
                            using (Image imgFromFile = Image.FromFile(prevImagen))
                            {

                                bmp = new Bitmap(prevImagen);
                                bmp.SetResolution(imgFromFile.HorizontalResolution, imgFromFile.VerticalResolution);
                                //MessageBox.Show("Siguiente Imagen" + imgFromFile.HorizontalResolution.ToString() + "/" + imgFromFile.VerticalResolution.ToString());

                                using (Graphics g = Graphics.FromImage(bmp))
                                {
                                    g.CompositingMode = CompositingMode.SourceCopy;
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                                    g.DrawImage(imgFromFile, 0, 0);
                                }

                                if (pictureBox1.Image != null)
                                {
                                    pictureBox1.Image.Dispose();
                                }
                                else
                                {
                                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    pictureBox1.Tag = prevImagen;
                                    pictureBox1.Name = null;
                                    pictureBox1.Name = currentIndice.ToString();
                                    pictureBox1.Image = bmp;

                                    //se actualiza el tooltip
                                    ttip.SetToolTip(pictureBox1, prevImagen);
                                }
                            } /* termina código de cargar  imagen */
                        }/* termina if de webp */
                    }/* termina if de currentIndice*/
                    else
                    {

                        prevImagen = null;
                        prevImagen = BuscaImage2(currentIndice.ToString(), imagelist);
                        pictureBox1.Image = null;

                        //se actualiza el nombre del archivo en la barra de titulo
                        Style.NombreImgTitulo(this,prevImagen);

                        string soloNombre = Path.GetFileName(prevImagen);
                        string extension = Path.GetExtension(soloNombre);


                        if (extension == ".webp")
                        {
                            CargarWebp(prevImagen);
                        }
                        else
                        {
                            /* inicia carga de imagen */
                            using (Image imgFromFile = Image.FromFile(prevImagen))
                            {

                                bmp = new Bitmap(prevImagen);
                                bmp.SetResolution(imgFromFile.HorizontalResolution, imgFromFile.VerticalResolution);


                                using (Graphics g = Graphics.FromImage(bmp))
                                {
                                    g.CompositingMode = CompositingMode.SourceCopy;
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                                    g.DrawImage(imgFromFile, 0, 0);
                                }

                                if (pictureBox1.Image != null)
                                {
                                    pictureBox1.Image.Dispose();
                                }
                                else
                                {
                                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    pictureBox1.Tag = prevImagen;
                                    pictureBox1.Name = null;
                                    pictureBox1.Name = currentIndice.ToString();
                                    pictureBox1.Image = bmp;

                                    ttip.SetToolTip(pictureBox1, prevImagen);
                                }
                            } /* termina carga de imagen */
                        }//if de webp image 
                    }
                }
            }// termina if picturebox no esta vacio
        }
        public string BuscaImage2(string indexBusca, Dictionary<string, string> newlist)
        {
            if (newlist.ContainsKey("index" + indexBusca))
            {
                return newlist["index" + indexBusca];
            }
            else
            {
                return indexBusca;
            }
        }

        private void CargarWebp(string nombreImagen)
        {

            string soloNombre = Path.GetFileName(nombreImagen);
            string extension = Path.GetExtension(soloNombre);

            /*se inicia deco de webp image */
            var webpDecoder = new SimpleDecoder();

            using (Stream inputStream = File.Open(nombreImagen, FileMode.Open))
            {
                byte[] buffer = new byte[16 * 1024];

                using (MemoryStream ms = new MemoryStream())
                {
                    int read;

                    while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }

                    var bytes = ms.ToArray();
                    bmp = webpDecoder.DecodeFromBytes(bytes, bytes.LongLength);
                }
            } /* termina webp decode */

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Tag = nombreImagen;
                ttip.SetToolTip(pictureBox1, nombreImagen);
                pictureBox1.Name = null;
                pictureBox1.Name = currentIndice.ToString();
                pictureBox1.Image = bmp;
            }
        }

        internal void FullSizeImage(object sender, EventArgs e)
        {
            viewFullSizeStripMenuItem.Enabled = false;
            viewFullSizeStripMenuItem.Visible = false;

            exitFullSizeStripMenuItem.Visible = true;
            exitFullSizeStripMenuItem.Enabled = true;

            pictureBox1.Cursor = new Cursor(GetType(), "cursorOpen.cur");
            menuStrip1.BringToFront();


            pictureBox1.Dock = DockStyle.None;
            pictureBox1.Anchor = AnchorStyles.None;

            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = bmp.Height;

            // se centra la imagen
            pictureBox1.Location = new Point(this.ClientSize.Width / 2 - pictureBox1.Width / 2, this.ClientSize.Height / 2 - pictureBox1.Height / 2);

            EventHandlers.AccionesMouseFullSizeImagen(this,pictureBox1);

            //se asigna la funcion de salir fullscreen al menu child de sair de full size top y se asigna el shotcut
            EventHandlers.AsignarAccionBtnMenu(exitFullSizeStripMenuItem, SalirFullSize);
            ShortCutKeys.AsignarTeclasAccesoRapido(exitFullSizeStripMenuItem, Keys.Control | Keys.Space);

            
            
        }

        internal void SalirFullSize(object sender, EventArgs e)
        {
            isDragging = false;
            pictureBox1.BringToFront();
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Cursor = Cursors.Default;

            //se quita la funcion de drag del mouse al picture box
            pictureBox1.MouseDown -= IniciaDrag;
            pictureBox1.MouseMove -= MueveDrag;
            pictureBox1.MouseUp -= UpDrag;

            //se quita la funcion de zoom al mouse wheel
            pictureBox1.MouseWheel -= PictureBox1_MouseWheel;

            exitFullSizeStripMenuItem.Click -= SalirFullSize;

            viewFullSizeStripMenuItem.Enabled = true;
            viewFullSizeStripMenuItem.Visible = true;

            exitFullSizeStripMenuItem.Visible = false;
            exitFullSizeStripMenuItem.Enabled = false;
        }


        //Para hacer zoom con el scroll
        internal void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;
            int ow = pictureBox1.Width;
            int oh = pictureBox1.Height;
            int VX, VY;

            if (e.Delta > 0)
            {
                pictureBox1.Width = pictureBox1.Width += 100;
                pictureBox1.Height = pictureBox1.Height += 100;
            }
            else
            {
                //if (pictureBox1.Width < bmp.Width / 10)
                if (pictureBox1.Width <= bmp.Width / 5 || pictureBox1.Height <= bmp.Height / 5)
                {
                    return;
                }

                pictureBox1.Width = pictureBox1.Width -= 100;
                pictureBox1.Height = pictureBox1.Height -= 100;
            }

            VX = (int)((double)x * (ow - pictureBox1.Width) / ow);
            VY = (int)((double)y * (oh - pictureBox1.Height) / oh);

            pictureBox1.Location = new Point(pictureBox1.Location.X + VX, pictureBox1.Location.Y + VY);
        }


        /* drag and move*/


        internal void IniciaDrag(object sender, MouseEventArgs e)
        {
            isDragging = true;

            currentX = e.X;
            currentY = e.Y;

            pictureBox1.Cursor = new Cursor(GetType(), "cursorGrab.cur");

        }

        internal void MueveDrag(object sender, MouseEventArgs e)
        {

            pictureBox1.Cursor = new Cursor(GetType(), "cursorOpen.cur");

            if (isDragging)
            {
                pictureBox1.Cursor = new Cursor(GetType(), "cursorGrab.cur");

                pictureBox1.Top = pictureBox1.Top + (e.Y - currentY);
                pictureBox1.Left = pictureBox1.Left + (e.X - currentX);
            }
        }

        internal void UpDrag(object sender, MouseEventArgs e)
        {

            pictureBox1.Cursor = new Cursor(GetType(), "cursorOpen.cur");
            isDragging = false;
        }

        /* end drag and move*/
        internal void CerrarImagen(object sender, EventArgs e)
        {
            //se actualiza la barra de titulo sin el nombre del archivo 
            Style.NombreImgTitulo(this,null);
            
            files = null;
            nombreImagen = "";
            myKey = "";
            imagelist.Clear();
            currentIndice = 0;
            i = 0;

            ttip.SetToolTip(pictureBox1, null);

            if (pictureBox1.Dock == DockStyle.None) {
                SalirFullSize(sender, e);
            }

            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;


            //se deshabilitan los botones de cerrar imagen y de imprimir
            abrirOpenDialogFile.Enabled = true;
            closeStripMenuItem.Enabled = false;
            flipHMenuItem.Enabled = false;
            flipVMenuItem.Enabled = false;

            viewFullSizeStripMenuItem.Enabled = false;
            fullScreenStripMenuItem.Enabled = false;
            openFileLocationMenuItem.Enabled = false;
            viewFullSizeStripMenuItem.Visible = true;
            viewFullSizeStripMenuItem.Enabled = false;
            exitFullSizeStripMenuItem.Visible = false;
            exitFullSizeStripMenuItem.Enabled = false;



            //btnPrint.Enabled = false;
        }

        internal void abrirEnExplorer(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", string.Format("/select,\"{0}\"", pictureBox1.Tag.ToString()));
        }

        internal void flipHImagen(object senders, EventArgs e)
        {
            modifImagen.FlipHorizontal( pictureBox1);
        }

        internal void flipVImagen(object senders, EventArgs e)
        {
            modifImagen.FlipVertical(pictureBox1);
        }


        internal void savePictureBox(object senders, EventArgs e)
        {
            string soloNombreImagen = Path.GetFileName(pictureBox1.Tag.ToString());
            string extensionImagen = Path.GetExtension(soloNombreImagen);

            SaveFileDialog dialog = new SaveFileDialog();
            
            dialog.Title = "Save New Image";
            dialog.DefaultExt = System.IO.Path.GetExtension(pictureBox1.Tag.ToString());
            dialog.AddExtension = true;
            dialog.OverwritePrompt = true;
            dialog.Filter = "Jpg Image|*.jpg|Jpeg Image|*.jpeg|Png Image|*.png|Webp Image|*.webp|Bitmap Image|*.bmp|Gif Image|*.gif|Tiff Image|*.tiff|Jfif Image|*.jfif";

            //se hace un switch en la extension de la imagen
            //para setera el filter index
            switch (extensionImagen)
            {
                case ".jpg":
                    dialog.FilterIndex = 1;
                    break;

                case ".jpeg":
                    dialog.FilterIndex = 2;
                    break;

                case ".png":
                    dialog.FilterIndex = 3;
                    break;

                case ".webp":
                    dialog.FilterIndex = 4;
                    break;

                case ".bmp":
                    dialog.FilterIndex = 5;
                    break;

                case ".gif":
                    dialog.FilterIndex = 6;
                    break;
                
                case ".tiff":
                    dialog.FilterIndex = 7;
                    break;

                case ".jfif":
                    dialog.FilterIndex = 8;
                    break;
            }
            
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string NewExtension = System.IO.Path.GetExtension(dialog.FileName);
                
                //si la nueva imagen va a ser webp
                if (NewExtension == ".webp")
                {
                    var webpFileName = dialog.FileName;

                    using (Bitmap bitmap = new Bitmap(pictureBox1.Image))
                    {
                        bitmap.SetResolution(pictureBox1.Image.HorizontalResolution, pictureBox1.Image.VerticalResolution);

                        using (var saveImageStream = System.IO.File.Open(webpFileName, FileMode.Create))
                        {
                            var encoder = new SimpleEncoder();
                            encoder.Encode(bitmap, saveImageStream, 90);
                        }
                    }
                }
                //si la nueva imagen va a tener el mismo formato que la anterior y no es webp
                else if (NewExtension == extensionImagen & NewExtension != ".webp")
                {
                    MessageBox.Show("sende guarda con el mismo formato");

                    using (Image image = Image.FromFile(pictureBox1.Tag.ToString()))
                    {
                        var rawFormat = image.RawFormat;
                        bmp.Save(dialog.FileName, rawFormat);
                    }
                }
                //si la nueva imagen va a ser con nuevo formato y no es extension webp
                else if (NewExtension != extensionImagen & NewExtension != ".webp")
                {


                    /*
                    System.Drawing.Imaging.Encoder myEncoder;
                    ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                    myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 83L);
                   bmp.Save(dialog.FileName, jpegCodec, encoderParameters);
                    
                    image/bmp
                    image/jpeg
                    image/x-png
                    image/png
                    image/gif
                     
                     */
                    switch (NewExtension)
                    {
                        case ".jpg":
                            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                            EncoderParameters encoderParameters = new EncoderParameters(1);
                            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 83L);
                            bmp.Save(dialog.FileName, jpegCodec, encoderParameters);


                            //bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                            break;

                        case ".jpeg":
                            ImageCodecInfo jpegCodec2 = GetEncoderInfo("image/jpeg");
                            EncoderParameters encoderParameters2 = new EncoderParameters(1);
                            encoderParameters2.Param[0] = new EncoderParameter(Encoder.Quality, 83L);
                            bmp.Save(dialog.FileName, jpegCodec2, encoderParameters2);

                            //bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                            break;

                        case ".png":
                            bmp.Save(dialog.FileName, ImageFormat.Png);
                            break;

                        case ".bmp":
                            ImageCodecInfo jpegCodec3 = GetEncoderInfo("image/bmp");
                            EncoderParameters encoderParameters3 = new EncoderParameters(1);
                            encoderParameters3.Param[0] = new EncoderParameter(Encoder.Quality, 83L);
                            bmp.Save(dialog.FileName, jpegCodec3, encoderParameters3);
                            //bmp.Save(dialog.FileName, ImageFormat.Bmp);
                            break;

                        case ".gif":
                            ImageCodecInfo jpegCodec4 = GetEncoderInfo("image/gif");
                            EncoderParameters encoderParameters4 = new EncoderParameters(1);
                            encoderParameters4.Param[0] = new EncoderParameter(Encoder.Quality, 83L);
                            bmp.Save(dialog.FileName, jpegCodec4, encoderParameters4);
                            //bmp.Save(dialog.FileName, ImageFormat.Gif);
                            break;

                        case ".tiff":
                            bmp.Save(dialog.FileName, ImageFormat.Tiff);
                            break;

                        case ".jfif":
                            bmp.Save(dialog.FileName, ImageFormat.Exif);
                            break;

                    }
                    
                    
                    //bmp.Save(dialog.FileName, ImageFormat.Jfif);
                }/*en if tipos de imagen a guardar*/



                //int width = Convert.ToInt32(drawImage.Width);
                //int height = Convert.ToInt32(drawImage.Height);
                //Bitmap bmp = new Bitmap(width, height);
                //drawImage.DrawToBitmap(bmp, new Rectangle(0, 0, width, height);
                //bmp.Save(dialog.FileName, ImageFormat.Tiff);

                /*System.IO.FileStream fs = (System.IO.FileStream)dialog.OpenFile();

                switch (dialog.FilterIndex)
                {
                    case 1:
                        this.pictureBox1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        this.pictureBox1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        this.pictureBox1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Png);
                        break;

                    case 4:
                        this.pictureBox1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;

                    case 5:
                        this.pictureBox1.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                }
                fs.Close();*/

            }
            dialog.Dispose();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            /* int j;
             ImageCodecInfo[] encoders;
             encoders = ImageCodecInfo.GetImageEncoders();
             for (j = 0; j < encoders.Length; ++j)
             {
                 if (encoders[j].MimeType == mimeType)
                     return encoders[j];
             }
             return null;*/

            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                if (codec.MimeType == mimeType)
                    return codec;

            return null;
        }
        /*************************AREA DE PRUEBA SELECCTION **********************************/
        
        
        

        // Start Rectangle
        //
        

        // Draw Rectangle
        //
        

        /*private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Rect.Contains(e.Location))
                {
                    MessageBox.Show("Right click dentro");
                }
                else
                {
                    MessageBox.Show("Right click fuera");
                }
            }
        }*/


        /************************/
        /*protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return cp;
            }
        }
        */

        public void SaveSelection(object sender, EventArgs e)
        {
            SaveFileDialog saveSelect = new SaveFileDialog();

            if(saveSelect.ShowDialog() == DialogResult.OK)
            {
                /*MessageBox.Show(pictureBox1.Controls[0].Bounds.Width.ToString()+" / "+
                pictureBox1.Controls[0].Bounds.Height.ToString());*/
                
                 int Xpos = pictureBox1.Controls[0].Bounds.Location.X;
                 int Ypos = pictureBox1.Controls[0].Bounds.Location.Y;
                 int ancho = pictureBox1.Controls[0].Bounds.Width;
                 int alto = pictureBox1.Controls[0].Bounds.Height;
               

                /*solucion original viene de aca
                 * https://stackoverflow.com/questions/53800328/translate-rectangle-position-in-zoom-mode-picturebox
                 */
                var selectedRectangle = new Rectangle(Xpos,Ypos, ancho,alto);
                var result = GetRectangeOnImage(pictureBox1, selectedRectangle);
                using (var bm = new Bitmap((int)result.Width, (int)result.Height))
                {
                    bm.SetResolution(pictureBox1.Image.HorizontalResolution, pictureBox1.Image.VerticalResolution);
                    using (var g = Graphics.FromImage(bm))
                    {
                        g.DrawImage(pictureBox1.Image, 0, 0, result, GraphicsUnit.Pixel);
                    }
                    bm.Save(saveSelect.FileName, ImageFormat.Jpeg);
                }
                
                pictureBox1.Controls[0].Dispose();
                pictureBox1.Invalidate();
            }
        }


        public RectangleF GetRectangeOnImage(PictureBox p, Rectangle selectionRect)
        {
            var method = typeof(PictureBox).GetMethod("ImageRectangleFromSizeMode",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var imageRect = (Rectangle)method.Invoke(p, new object[] { p.SizeMode });
            if (p.Image == null)
                return selectionRect;
            var cx = (float)p.Image.Width / (float)imageRect.Width;
            var cy = (float)p.Image.Height / (float)imageRect.Height;
            var r2 = Rectangle.Intersect(imageRect, selectionRect);
            r2.Offset(-imageRect.X, -imageRect.Y);
            return new RectangleF(r2.X * cx, r2.Y * cy, r2.Width * cx, r2.Height * cy);
        }
    }
}