using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using System.Threading;
using System.Diagnostics;

namespace Emgu.CV.Test
{
    public partial class TestCamera : Form
    {
        public TestCamera()
        {
            InitializeComponent();
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            _capture = true;
        }

        private bool _capture;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                (new Thread(
                delegate()
                {
                    using (Emgu.CV.Capture c = new Capture())
                    {
                        _capture = true;

                        while (_capture)
                        {
                            using (Image<Bgr, Byte> frame = c.QueryFrame())
                            using (Image<Bgr, Byte> flip = frame.Flip(CvEnum.FLIP.HORIZONTAL))
                            using (Image<Bgr, Byte> smooth = flip.GaussianSmooth(5))
                            {
                                DateTime t1 = DateTime.Now;

                                FaceDetector fd = new FaceDetector();
                                List<Face<Byte>> faces = fd.Detect(smooth);
                                if (faces.Count > 0)
                                {
                                    Face<Byte> f = fd.Detect(smooth)[0];
                                    TimeSpan ts = DateTime.Now.Subtract(t1);
                                    Trace.WriteLine(ts.TotalMilliseconds);

                                    using (Image<Bgr, Byte> res = f.RGB.Clone())
                                    {
                                        //res.Draw(f.SkinContour, new Rgb(255.0, 255.0, 255.0), new Rgb(255.0, 255.0, 255.0), 1);
                                        pictureBox2.Image = res.ToBitmap();
                                    }
                                    smooth.Draw(f.Rectangle, new Bgr(255.0, 255.0, 255.0), 1);
                                }
                                pictureBox1.Image = smooth.ToBitmap();
                            }

                        }
                    }
                })).Start();
            }
            catch (Emgu.Exception excpt)
            {
                excpt.AlertIfServere(true);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _capture = false;
        }
    }
}
