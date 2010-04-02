using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV;
using System.Drawing;
using System.Diagnostics;

namespace FaceDetection
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Run();
      }

      static void Run()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg"); //Read the files as an 8-bit Bgr image  
         Image<Gray, Byte> gray = image.Convert<Gray, Byte>(); //Convert it to Grayscale

         Stopwatch watch = Stopwatch.StartNew();
         //normalizes brightness and increases contrast of the image
         gray._EqualizeHist();

         //Read the HaarCascade objects
         HaarCascade face = new HaarCascade("haarcascade_frontalface_alt_tree.xml");
         HaarCascade eye = new HaarCascade("haarcascade_eye.xml");

         //Detect the faces  from the gray scale image and store the locations as rectangle
         //The first dimensional is the channel
         //The second dimension is the index of the rectangle in the specific channel
         MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
            face, 
            1.1, 
            10, 
            Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, 
            new Size(20, 20));

         foreach (MCvAvgComp f in facesDetected[0])
         {
            //draw the face detected in the 0th (gray) channel with blue color
            image.Draw(f.rect, new Bgr(Color.Blue), 2);

            //Set the region of interest on the faces
            gray.ROI = f.rect;
            MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
               eye, 
               1.1, 
               10, 
               Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, 
               new Size(20, 20));
            gray.ROI = Rectangle.Empty;

            foreach (MCvAvgComp e in eyesDetected[0])
            {
               Rectangle eyeRect = e.rect;
               eyeRect.Offset(f.rect.X, f.rect.Y);
               image.Draw(eyeRect, new Bgr(Color.Red), 2);
            }
         }

         watch.Stop();
         //display the image 
         ImageViewer.Show(image, String.Format("Perform face and eye detection in {0} milliseconds", watch.ElapsedMilliseconds));
      }
   }
}