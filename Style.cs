﻿using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SimpleImageViewer
{
    internal class Style
    {
        internal static void Dimensiones(Form1 formulario, int ancho, int alto)
        {
            formulario.Width = ancho;
            formulario.Height = alto;
        }

        internal static void EstadoInicial(Form1 form1)
        {
            form1.WindowState = FormWindowState.Normal;
        }

        internal static void NombreImgTitulo(Form1 form1, string nombreArchivo)
        {

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            form1.Text = "Image Viewer - Ver: " +version.ToString()+" - "+nombreArchivo;

        }

        internal static void ColorDeFondo(Form1 formulario, int r, int g, int b)
        {
            Color nuevoColor = Color.FromArgb(r, g, b);
            formulario.BackColor = nuevoColor;
        }

        internal static void ColorFondoPictureBox(PictureBox pictureBox, int r, int g, int b)
        {
            Color color = Color.FromArgb(r, g, b);
            pictureBox.BackColor = color;
        }

        internal static void ColorFondoMenuStrip(MenuStrip menuStrip, int r, int g, int b)
        {
            Color color = Color.FromArgb(r,g,b);
            menuStrip.BackColor = color;
        }


        internal static void RemueveAreaGrisSubmenus(MenuStrip menuStrip) 
        {

            foreach (ToolStripMenuItem menuItem in menuStrip.Items)
            {
                if (menuItem.DropDown is ToolStripDropDownMenu dropDownMenu)
                {
                    dropDownMenu.ShowImageMargin = false;
                }
            }
        }
    }
}
