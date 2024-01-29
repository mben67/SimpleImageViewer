using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleImageViewer
{
    internal class ImageSelectionControl : Control
    {
        private PictureBox pictureBox;
        private Rectangle selectionRectangle;
        private Point dragStart;
        private bool isDragging;

        public ImageSelectionControl(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.Transparent;

            this.MouseDown += ImageSelectionControl_MouseDown;
            this.MouseMove += ImageSelectionControl_MouseMove;
            this.MouseUp += ImageSelectionControl_MouseUp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var p = new Pen(Color.FromArgb(255, 255, 255), 4))
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                // Dibuja la línea punteada usando los límites de la selección
                e.Graphics.DrawRectangle(p, selectionRectangle);
            }
        }

        private void ImageSelectionControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStart = e.Location;
                selectionRectangle = new Rectangle(dragStart, new Size(0, 0));
            }
        }

        private void ImageSelectionControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int width = e.X - dragStart.X;
                int height = e.Y - dragStart.Y;

                // Ajusta los límites de la selección para que no salgan del PictureBox
                width = Math.Min(width, pictureBox.Width - selectionRectangle.Left);
                height = Math.Min(height, pictureBox.Height - selectionRectangle.Top);

                selectionRectangle.Size = new Size(width, height);
                Invalidate();
            }
        }

        private void ImageSelectionControl_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void LimitSelectionToBounds()
        {
            // Limitamos la selección a los bordes de la imagen
            int maxX = pictureBox.Width - selectionRectangle.Width;
            int maxY = pictureBox.Height - selectionRectangle.Height;

            selectionRectangle.X = Math.Max(0, Math.Min(selectionRectangle.X, maxX));
            selectionRectangle.Y = Math.Max(0, Math.Min(selectionRectangle.Y, maxY));
        }
       /* public void HandleMouseDown(MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        public void HandleMouseMove(MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        public void HandleMouseUp(MouseEventArgs e)
        {
            OnMouseUp(e);
        }*/
    }
}