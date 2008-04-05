using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Emgu.UI
{
    public partial class ImageViewer : Form
    {
        public ImageViewer(Bitmap img)
        {
            InitializeComponent();
            this.Width = img.Width + 8;
            this.Height = img.Height + 20;

            pictureBox1.Image = img;
        }

        public ImageViewer(Bitmap img, string windowName)
            : this(img)
        {
            this.Text = windowName;
        }
    }
}