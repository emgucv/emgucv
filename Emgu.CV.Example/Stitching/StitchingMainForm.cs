//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;
using Emgu.CV.UI;

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

                Image<Bgr, byte>[] sourceImages = new Image<Bgr, byte>[dlg.FileNames.Length];

                for (int i = 0; i < sourceImages.Length; i++)
                {
                    sourceImages[i] = new Image<Bgr, byte>(dlg.FileNames[i]);

                    using (Image<Bgr, byte> thumbnail = sourceImages[i].Resize(200, 200, Emgu.CV.CvEnum.Inter.Cubic, true))
                    {
                        DataGridViewRow row = sourceImageDataGridView.Rows[sourceImageDataGridView.Rows.Add()];
                        row.Cells["FileNameColumn"].Value = dlg.FileNames[i];
                        row.Cells["ThumbnailColumn"].Value = thumbnail.ToBitmap();
                        row.Height = 200;
                    }
                }
                try
                {
                    //only use GPU if you have build the native binary from code and enabled "NON_FREE"
                    using (Stitcher stitcher = new Stitcher())
                    using (Emgu.CV.Features2D.AKAZE finder = new Emgu.CV.Features2D.AKAZE())
                    using (Emgu.CV.Stitching.WarperCreator warper = new SphericalWarper())
                    {
                        stitcher.SetFeaturesFinder(finder);
                        stitcher.SetWarper(warper);
                        using (VectorOfMat vm = new VectorOfMat())
                        {
                            Mat result = new Mat();
                            vm.Push(sourceImages);

                            Stopwatch watch = Stopwatch.StartNew();

                            this.Text = "Stitching";
                            Stitcher.Status stitchStatus = stitcher.Stitch(vm, result);
                            watch.Stop();

                            if (stitchStatus == Stitcher.Status.Ok)
                            {
                                resultImageBox.Image = result;
                                this.Text = String.Format("Stitched in {0} milliseconds.", watch.ElapsedMilliseconds);
                            }
                            else
                            {
                                MessageBox.Show(this, String.Format("Stiching Error: {0}", stitchStatus));
                                resultImageBox.Image = null;
                            }
                        }
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
