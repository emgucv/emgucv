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
   public partial class MainForm : Form
   {
      public MainForm()
      {
         InitializeComponent();

         Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg"); //Read the files as an 8-bit Bgr image  
         Image<Gray, Byte> gray = image.Convert<Gray, Byte>(); //Convert it to Grayscale

         //normalizes brightness and increases contrast of the image
         gray._EqualizeHist();

         //Read the HaarCascade object
         HaarCascade face = new HaarCascade("haarcascade_frontalface_alt2.xml");

         //Detect the faces  from the gray scale image and store the locations as rectangle
         //The first dimensional is the channel
         //The second dimension is the index of the rectangle in the specific channel
         Rectangle<double>[][] facesDetected = gray.DetectHaarCascade(face);

         foreach (Rectangle<double> f in facesDetected[0])
         {
            //draw all the faces detected in the 0th (gray) channel with blue color
            image.Draw(f, new Bgr(255, 0, 0), 2);
         }

         //display the image 
         imageBox1.Image = image;

      }
   }
}