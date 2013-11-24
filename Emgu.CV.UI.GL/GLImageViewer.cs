using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV.UI.GLView
{
   public partial class GLImageViewer : Form
   {
      public GLImageViewer()
      {
         InitializeComponent();
         
      }

      public void LoadImage(Image<Bgr, Byte> image)
      {
         this.glImageView.SetImage(image, new GeometricChange());
      }

      private void loadImageButton_Click(object sender, EventArgs e)
      {
         OpenFileDialog ofd = new OpenFileDialog();
         ofd.Multiselect = false;
         if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            String f = ofd.FileName;
            Image<Bgr, Byte> image = new Image<Bgr, byte>(f);
            
            this.glImageView.Rectangles = new RectangleF[] { new RectangleF(PointF.Empty, new SizeF(0.4f, 0.9f)) };
            this.glImageView.GridLines = 2;

            RectangleF rectangle = glImageView.GetRectangleInImageCoordinate(0, image.Size);
            LoadImage(image);
         }
      }
   }
}
