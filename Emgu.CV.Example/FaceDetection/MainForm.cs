using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

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

         //Read the HaarCascade objects
         HaarCascade face = new HaarCascade("haarcascade_frontalface_alt_tree.xml");
         HaarCascade eye = new HaarCascade("haarcascade_eye.xml");

         //Detect the faces  from the gray scale image and store the locations as rectangle
         //The first dimensional is the channel
         //The second dimension is the index of the rectangle in the specific channel
         MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(face, 1.1, 1, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

         foreach (MCvAvgComp f in facesDetected[0])
         {
            //Set the region of interest on the faces
            gray.ROI = f.rect;
            MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(eye, 1.1, 1, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            gray.ROI = Rectangle.Empty;

            //if there is no eye in the specific region, the region shouldn't contains a face
            //note that we might not be able to recoginize a person who ware glass in this case 
            if (eyesDetected[0].Length == 0) continue;

            //draw the face detected in the 0th (gray) channel with blue color
            image.Draw(f.rect, new Bgr(Color.Blue), 2);

            foreach (MCvAvgComp e in eyesDetected[0])
            {
               Rectangle eyeRect = e.rect;
               eyeRect.Offset(f.rect.X, f.rect.Y);
               image.Draw(eyeRect, new Bgr(Color.Red), 2);
            }
         }

         //display the image 
         imageBox1.Image = image;

      }
   }
}
