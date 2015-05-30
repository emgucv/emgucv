//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Diagnostics;

namespace TrafficSignRecognition
{
   public partial class TrafficSignRecognitionForm : Form
   {
      private StopSignDetector _stopSignDetector;

      public TrafficSignRecognitionForm()
      {
         InitializeComponent();
         using (Image<Bgr, Byte> stopSignModel = new Image<Bgr, Byte>("stop-sign-model.png"))
         {
            Mat image = CvInvoke.Imread("stop-sign.jpg", LoadImageType.Color);

            _stopSignDetector = new StopSignDetector(stopSignModel);
            ProcessImage(image);
         }
      }

      private void ProcessImage(Mat image)
      {
         Stopwatch watch = Stopwatch.StartNew(); // time the detection process

         List<Mat> stopSignList = new List<Mat>();
         List<Rectangle> stopSignBoxList = new List<Rectangle>();
         _stopSignDetector.DetectStopSign(image, stopSignList, stopSignBoxList);

         watch.Stop(); //stop the timer
         processTimeLabel.Text = String.Format("Stop Sign Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds);

         panel1.Controls.Clear();
         Point startPoint = new Point(10, 10);

         for (int i = 0; i < stopSignList.Count; i++)
         {
            Rectangle rect = stopSignBoxList[i];
            AddLabelAndImage(
               ref startPoint,
               String.Format("Stop Sign [{0},{1}]:", rect.Location.Y + rect.Width / 2, rect.Location.Y + rect.Height / 2),
               stopSignList[i]);
            CvInvoke.Rectangle(image, rect, new Bgr(Color.Aquamarine).MCvScalar, 2);

         }

         imageBox1.Image = image;
      }

      private void AddLabelAndImage(ref Point startPoint, String labelText, IImage image)
      {
         Label label = new Label();
         panel1.Controls.Add(label);
         label.Text = labelText;
         label.Width = 100;
         label.Height = 30;
         label.Location = startPoint;
         startPoint.Y += label.Height;

         ImageBox box = new ImageBox();
         panel1.Controls.Add(box);
         box.ClientSize = image.Size;
         box.Image = image;
         box.Location = startPoint;
         startPoint.Y += box.Height + 10;
      }

      private void button1_Click(object sender, EventArgs e)
      {
         DialogResult result = openFileDialog1.ShowDialog();
         if (result == DialogResult.OK)
         {
            Mat img;
            try
            {
               img = CvInvoke.Imread(openFileDialog1.FileName, LoadImageType.Color);
            }
            catch
            {
               MessageBox.Show("Invalide file format");
               return;
            }

            ProcessImage(img);
         }
      }
   }

}