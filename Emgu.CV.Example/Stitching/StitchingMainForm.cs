using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;

namespace Stitching
{
   public partial class StitchingMainForm : Form
   {
      public StitchingMainForm()
      {
         InitializeComponent();
      }

      private void selectImagesButton_Click(object sender, EventArgs e)
      {
         OpenFileDialog dlg = new OpenFileDialog();
         dlg.CheckFileExists = true;
         dlg.Multiselect = true;

         if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            sourceImageDataGridView.Rows.Clear();

            Image<Bgr, Byte>[] sourceImages = new Image<Bgr, byte>[dlg.FileNames.Length];
            
            for (int i = 0; i < sourceImages.Length; i++)
            {
               sourceImages[i] = new Image<Bgr, byte>(dlg.FileNames[i]);

               using (Image<Bgr, byte> thumbnail = sourceImages[i].Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true))
               {
                  DataGridViewRow row = sourceImageDataGridView.Rows[sourceImageDataGridView.Rows.Add()];
                  row.Cells["FileNameColumn"].Value = dlg.FileNames[i];
                  row.Cells["ThumbnailColumn"].Value = thumbnail.ToBitmap();
                  row.Height = 200;
               }
            }
            try
            {
               using (Stitcher stitcher = new Stitcher(true))
               {
                  Image<Bgr, Byte> result = stitcher.Stitch(sourceImages);
                  resultImageBox.Image = result;
               }
            }
            finally
            {
               foreach (Image<Bgr, Byte> img in sourceImages)
               {
                  img.Dispose();
               }
            }
         }
      }
   }
}
