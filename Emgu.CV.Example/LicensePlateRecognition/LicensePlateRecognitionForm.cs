//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using System.Diagnostics;

namespace LicensePlateRecognition
{
   public partial class LicensePlateRecognitionForm : Form
   {
      private LicensePlateDetector _licensePlateDetector;

      public LicensePlateRecognitionForm()
      {
         InitializeComponent();
         _licensePlateDetector = new LicensePlateDetector();

         ProcessImage(new Image<Bgr, byte>("license-plate.jpg"));
      }

      private void ProcessImage(Image<Bgr, byte> image)
      {
         Stopwatch watch = Stopwatch.StartNew(); // time the detection process

         List<Image<Gray, Byte>> licensePlateImagesList = new List<Image<Gray, byte>>();
         List<Image<Gray, Byte>> filteredLicensePlateImagesList = new List<Image<Gray, byte>>();
         List<MCvBox2D> licenseBoxList = new List<MCvBox2D>();
         List<string> words = _licensePlateDetector.DetectLicensePlate(
            image,
            licensePlateImagesList,
            filteredLicensePlateImagesList,
            licenseBoxList);

         watch.Stop(); //stop the timer
         processTimeLabel.Text = String.Format("License Plate Recognition time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds);

         panel1.Controls.Clear();
         Point startPoint = new Point(10, 10);
         for (int i = 0; i < words.Count; i++)
         {
            AddLabelAndImage(
               ref startPoint,
               String.Format("License: {0}", words[i]),
               licensePlateImagesList[i].ConcateVertical(filteredLicensePlateImagesList[i]));
            image.Draw(licenseBoxList[i], new Bgr(Color.Red), 2);
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
            Image<Bgr, Byte> img;
            try
            {
               img = new Image<Bgr, byte>(openFileDialog1.FileName);
            }
            catch
            {
               MessageBox.Show(String.Format("Invalide File: {0}", openFileDialog1.FileName));
               return;
            }

            ProcessImage(img);
         }
      }
   }

}