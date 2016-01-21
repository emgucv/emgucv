//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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
         InitOcr("", "eng", OcrEngineMode.TesseractCubeCombined);
        
      }

      private void InitOcr(String path, String lang, OcrEngineMode mode)
      {
         try
         {
            if (_ocr != null)
            {
               _ocr.Dispose();
               _ocr = null;
            }
            _ocr = new Tesseract(path, lang, mode);
            languageNameLabel.Text = String.Format("{0} : {1}", lang, mode.ToString());
         }
         catch (Exception e)
         {
            _ocr = null;
            MessageBox.Show(e.Message, "Failed to initialize tesseract OCR engine", MessageBoxButtons.OK);
            languageNameLabel.Text = "Failed to initialize tesseract OCR engine";
         }
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
                  Tesseract.Character[] characters = _ocr.GetCharacters();
                  foreach (Tesseract.Character c in characters)
                  {
                     image.Draw(c.Region, drawColor, 1);
                  }

                  imageBox1.Image = image;

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
            string path = Path.GetDirectoryName(openLanguageFileDialog.FileName);
            string lang =  Path.GetFileNameWithoutExtension(openLanguageFileDialog.FileName).Split('.')[0];

            InitOcr(path, lang, OcrEngineMode.Default);
            
         }
      }
   }
}
