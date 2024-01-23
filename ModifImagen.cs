using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleImageViewer
{
    internal class ModifImagen
    {
        public void FlipHorizontal(PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox.Invalidate();
            }
        }

        public void FlipVertical(PictureBox pictureBox) 
        {  
            if (pictureBox.Image != null) 
            {
                pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                pictureBox.Invalidate();
            } 
        }
    }
}
