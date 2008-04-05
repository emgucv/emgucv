using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Threading;

namespace WindowsForm
{
    public partial class Form1 : Form
    {
        public Thread _frameGrabber;

        public Form1()
        {
            InitializeComponent();

            #region create a frame grabbing thread and let it run in the background
            _frameGrabber = new Thread(
                delegate() {
                    while (true)
                        pictureBox1.Image = GrapFrame(); 
                });
            _frameGrabber.Start();
            #endregion
        }

        /// <summary>
        /// Function used to grab a frame 
        /// (Change this to suite your need, for example, capture an image from the camera and do face detection)
        /// </summary>
        /// <returns>Grabed image</returns>
        public Bitmap GrapFrame()
        {
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 300, new Bgr(255, 255, 255)))
            {
                Emgu.CV.Font f = new Emgu.CV.Font(FONT.CV_FONT_HERSHEY_PLAIN, 1.0, 1.0);
                String str = DateTime.Now.Ticks.ToString();
                img.Draw(str, f, new Point2D<int>(50, 150), new Bgr(0, 0, 0));
                return img.ToBitmap( pictureBox1.Width, pictureBox1.Height );
            }
        }

        private void ReleaseResource()
        {
        }
    }
}