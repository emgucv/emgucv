using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;

namespace FaceDetection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg")) //Read the image as a Bgr 8-bit image  
            using (Image<Gray, Byte> gray = image.Convert<Gray, Byte>()) //Convert it to Grayscale
            {
                //Read the HaarCascade object
                HaarCascade face = new HaarCascade("haarcascade_frontalface_alt2.xml");

                //Detect the faces and store the locations as rectangle
                Emgu.CV.Rectangle<double>[][] facesDetected = image.DetectHaarCascade(face);

                foreach (Rectangle<double> f in facesDetected[0])
                {
                    //draw all the faces detected in the 0th (gray) channel with blue color
                    image.Draw(f, new Bgr(255, 0, 0), 2);
                }

                //display the image
                pictureBox1.Image = image.ToBitmap();
            }
        }
    }
}