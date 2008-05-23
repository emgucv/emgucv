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

            //create a frame grabbing thread and let it run in the background
            _frameGrabber = 
                new Thread( delegate() { while (true) imageBox1.Image = GrapFrame(); });
            _frameGrabber.Start();
            
        }

        /// <summary>
        /// Function used to grab a frame 
        /// (Change this to suite your need, for example, capture an image from the camera and do face detection)
        /// </summary>
        /// <returns>Grabed image</returns>
        public Image<Bgr, Byte>  GrapFrame()
        {
            //Create an image of 300x200 with white background
            Image<Bgr, Byte> img = new Image<Bgr, byte>( 300, 200,  new Bgr(255, 255, 255)); 

            img.Draw(
                DateTime.Now.Ticks.ToString(), //draw the system clock tick 
                new Emgu.CV.Font(FONT.CV_FONT_HERSHEY_PLAIN, 1.0, 1.0), //using the specific font
                new Point2D<int>(50, 150), //The bottom left posistion of the first character
                new Bgr(0, 0, 0)); //The color of the font
            return img;            
        }

        private void ReleaseResource()
        {
        }
    }
}