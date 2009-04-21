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
using tessnet2;

namespace LicensePlateRecognition
{
   public partial class LicensePlateRecognitionForm : Form
   {
      private LicensePlateDetector _licensePlateDetector;
      private StopSignDetector _stopSignDetector;

      public LicensePlateRecognitionForm()
      {
         InitializeComponent();
         _licensePlateDetector = new LicensePlateDetector();
         _stopSignDetector = new StopSignDetector();

         //ProcessImage(new Image<Bgr, byte>("stop-sign2.jpg"));
         ProcessImage(new Image<Bgr, byte>("license-plate.jpg"));
      }

      private void ProcessImage(Image<Bgr, byte> image)
      {
         List<Image<Gray, Byte>> licensePlateList = new List<Image<Gray, byte>>();
         List<Image<Gray, Byte>> filteredLicensePlateList = new List<Image<Gray, byte>>();
         List<MCvBox2D> licenseBoxList = new List<MCvBox2D>();
         List<List<Word>> words = _licensePlateDetector.DetectLicensePlate(
            image,
            licensePlateList,
            filteredLicensePlateList,
            licenseBoxList);

         List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, byte>>();
         List<Image<Gray, Byte>> filteredStopSignList = new List<Image<Gray, byte>>();
         List<MCvBox2D> stopSignBoxList = new List<MCvBox2D>();
         _stopSignDetector.DetectStopSign(image, stopSignList, filteredStopSignList, stopSignBoxList);

         panel1.Controls.Clear();

         Point startPoint = new Point(10, 10);
         ShowLicense(ref startPoint, words, licensePlateList, filteredLicensePlateList, licenseBoxList);
         foreach (MCvBox2D box in licenseBoxList)
            image.Draw(box, new Bgr(Color.Red), 2);

         ShowStopSign(ref startPoint, stopSignList, filteredStopSignList, stopSignBoxList);
         foreach (MCvBox2D box in stopSignBoxList)
            image.Draw(box, new Bgr(Color.Aquamarine), 2);

         //imageBox1.Image = image;
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

      private void ShowStopSign(ref Point startPoint, List<Image<Gray, Byte>> stopSignList, List<Image<Gray, Byte>> filteredStopSignList, List<MCvBox2D> boxList)
      {
         for (int i = 0; i < stopSignList.Count; i++)
         {
            AddLabelAndImage(
               ref startPoint, 
               String.Format("Stop Sign {0}:", boxList[i].center.ToString()),
               stopSignList[i].ConcateVertical(filteredStopSignList[i]));
         }
      }

      private void ShowLicense(ref Point startPoint, List<List<Word>> licenses, List<Image<Gray, Byte>> licensePlateList, List<Image<Gray, Byte>> filteredLicensePlateList, List<MCvBox2D> boxList)
      {
         for (int i = 0; i < licenses.Count; i++)
         {
            AddLabelAndImage(
               ref startPoint,
               "License: " + String.Join(" ", licenses[i].ConvertAll<String>(delegate(Word w) { return w.Text; }).ToArray()),
               licensePlateList[i].ConcateVertical(filteredLicensePlateList[i]));
         }
      }

      private void button1_Click(object sender, EventArgs e)
      {
         DialogResult result = openFileDialog1.ShowDialog();
         if (result == DialogResult.OK)
         {
            try
            {
               Image<Bgr, Byte> img = new Image<Bgr, byte>(openFileDialog1.FileName);
               ProcessImage(img);
            }
            catch
            {
               MessageBox.Show("Invalide file format");
            }
         }
      }
   }

}