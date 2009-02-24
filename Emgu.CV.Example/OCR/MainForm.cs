using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using tessnet2; //OCR

namespace OCR
{
   public partial class MainForm : Form
   {
      public MainForm()
      {
         InitializeComponent();
         PerformOCR("eng.arial.g4.tif");
      }

      private void PerformOCR(string fileName)
      {
         //Read the image from file
         Image<Gray, Byte> image = new Image<Gray, byte>(fileName);

         fileNameTextBox.Text = fileName;

         //Resize the image if it is too big, display it on the image box
         int width = Math.Min(image.Width, imageBox1.Width);
         int height = Math.Min(image.Height, imageBox1.Height);
         imageBox1.Image = image.Resize(width, height, true);

         //Perform OCR
         Tesseract ocr = new Tesseract();
         //You can download more language definition data from
         //http://code.google.com/p/tesseract-ocr/downloads/list
         //Languages supported includes:
         //Dutch, Spanish, German, Italian, French and English
         ocr.Init("eng", numericalOnlyCheckBox.Checked); 
         List<tessnet2.Word> result = ocr.DoOCR(image.Bitmap, Rectangle.Empty);

         //Obtain the texts from OCR result
         String[] texts = result.ConvertAll<String>(delegate(Word w) { return w.Text; }).ToArray();

         //Display the text in the text box
         textBox1.Text = String.Join(" ", texts);
      }

      private void button1_Click(object sender, EventArgs e)
      {
         DialogResult result = openFileDialog1.ShowDialog();
         if (result == DialogResult.OK || result == DialogResult.Yes)
         {
            PerformOCR(openFileDialog1.FileName);
         }
      }
   }
}