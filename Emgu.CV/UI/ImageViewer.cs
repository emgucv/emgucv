using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Emgu.CV.UI
{
    public partial class ImageViewer : Form
    {
        public ImageViewer(IImage img)
        {
            InitializeComponent();
            imageBox1.Image = img;
        }

        public ImageViewer(IImage img, string windowName)
            : this(img)
        {
            this.Text = windowName;
        }
    }
}