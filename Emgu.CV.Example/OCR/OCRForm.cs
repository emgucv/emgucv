//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;

namespace OCR
{
   public partial class OCRForm : Form
   {
      private Tesseract _ocr;
      public OCRForm()
      {
         InitializeComponent();
         _ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED);
         languageNameLabel.Text = "eng : tesseract + cube";
      }

      private void loadImageButton_Click(object sender, EventArgs e)
      {
         if (openImageFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            Bgr drawColor = new Bgr(Color.Blue);
            try
            {
               Image<Bgr, Byte> image = new Image<Bgr, byte>(openImageFileDialog.FileName);

               using (Image<Gray, byte> gray = image.Convert<Gray, Byte>())
               {
                  _ocr.Recognize(gray);
                  Tesseract.Charactor[] charactors = _ocr.GetCharactors();
                  foreach (Tesseract.Charactor c in charactors)
                  {
                     image.Draw(c.Region, drawColor, 1);
                  }

                  imageBox1.Image = image;

                  //String text = String.Concat( Array.ConvertAll(charactors, delegate(Tesseract.Charactor t) { return t.Text; }) );
                  String text = _ocr.GetText();
                  ocrTextBox.Text = text;
               }
            }
            catch (Exception exception)
            {
               MessageBox.Show(exception.Message);
            }
         }
      }

      private void loadLanguageToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (openLanguageFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            _ocr.Dispose();
            string path = Path.GetDirectoryName(openLanguageFileDialog.FileName);
            string lang =  Path.GetFileNameWithoutExtension(openLanguageFileDialog.FileName).Split('.')[0];
            _ocr = new Tesseract(path, lang, Tesseract.OcrEngineMode.OEM_DEFAULT);
            languageNameLabel.Text = String.Format("{0} : tesseract", lang);
         }
      }
   }
}
