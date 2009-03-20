using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.Util.TypeEnum;

namespace Emgu.CV.UI
{
   internal partial class ImageProperty : UserControl
   {
      private HistogramViewer _histogramViewer;

      /// <summary>
      /// Create a ImageProperty control
      /// </summary>
      public ImageProperty()
      {
         InitializeComponent();
         cSharpOperationView.Language = ProgrammingLanguage.CSharp;
         cPlusPlusOperationView.Language = ProgrammingLanguage.CPlusPlus;

         for (int i = 0; i < ImageBox.ZoomLevels.Length; i++)
         {
            zoomLevelComboBox.Items.Add(String.Format("{0}%", (int)(ImageBox.ZoomLevels[i] * 100)));
         }    
      }

      private ImageBox _imageBox;

      /// <summary>
      /// The parent imagebox for this property panel
      /// </summary>
      public ImageBox ImageBox
      {
         get { return _imageBox; }
         set { _imageBox = value; }
      }

      public void SetImage(IImage image)
      {
         #region display the size of the image
         Size size = image.Size;
         widthTextbox.Text = size.Width.ToString();
         heightTextBox.Text = size.Height.ToString();
         #endregion

         #region display the color type of the image
         Type colorType = Reflection.ReflectIImage.GetTypeOfColor(image);
         Object[] colorAttributes = colorType.GetCustomAttributes(typeof(ColorInfoAttribute), true);
         if (colorAttributes.Length > 0)
         {
            ColorInfoAttribute info = (ColorInfoAttribute)colorAttributes[0];
            typeOfColorTexbox.Text = info.ConversionCodename;
         }
         else
         {
            typeOfColorTexbox.Text = Properties.StringTable.Unknown;
         }

         Type colorDepth = Reflection.ReflectIImage.GetTypeOfDepth(image);
         typeOfDepthTextBox.Text = colorDepth.Name;
         #endregion

         UpdateHistogram();
      }

      /// <summary>
      /// Set the mouse position over the image. 
      /// It also set the color intensity of the pixel on the image where is mouse is at
      /// </summary>
      /// <param name="location">The location of the mouse on the image</param>
      public void SetMousePositionOnImage(System.Drawing.Point location)
      {
         mousePositionTextbox.Text = location.ToString();

         IImage img = _imageBox.DisplayedImage;
         IColor pixelColor =
            img == null ?
            null :
            Reflection.ReflectIImage.GetPixelColor(img, location);
         colorIntensityTextbox.Text =
                pixelColor == null ? String.Empty : pixelColor.ToString();
      }

      /// <summary>
      /// Set the description of the operation view
      /// </summary>
      public void SetOperations(List<Operation> operations)
      {
         cSharpOperationView.DisplayOperations(operations);
         cPlusPlusOperationView.DisplayOperations(operations);
      }

      /// <summary>
      /// Set the frame rate
      /// </summary>
      public int FramesPerSecondText
      {
         set
         {
            fpsTextBox.Text = value.ToString();
         }
      }

      private void clearOperationBtn_Click(object sender, EventArgs e)
      {
         _imageBox.ClearOperation();
      }

      private void popOperationButton_Click(object sender, EventArgs e)
      {
         _imageBox.PopOperation();
      }

      private void showHistogramButton_Click(object sender, EventArgs e)
      {
         if (_imageBox.DisplayedImage == null)
         {
            MessageBox.Show(Properties.StringTable.PleaseLoadAnImageFirst);
            return;
         }

         if (_histogramViewer != null && _histogramViewer.Visible == false)
         {
            _histogramViewer.Dispose();
            _histogramViewer = null;
         }

         if (_histogramViewer == null)
         {
            _histogramViewer = new HistogramViewer();
            _histogramViewer.Show();
         }

         UpdateHistogram();
      }

      private void UpdateHistogram()
      {
         if (_histogramViewer != null && _histogramViewer.Visible)
         {
            IImage image = _imageBox.DisplayedImage;

            if (image != null)
            {
               _histogramViewer.HistogramCtrl.ClearHistogram();
               _histogramViewer.HistogramCtrl.GenerateHistograms(image, 256);
               _histogramViewer.HistogramCtrl.Refresh();
            }
         }
      }

      private void zoomLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (_imageBox != null)
         {
            _imageBox.ZoomScale = ImageBox.ZoomLevels[zoomLevelComboBox.SelectedIndex];;
         }
      }

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
            if (_histogramViewer != null)
            {
               _histogramViewer.Dispose();
               _histogramViewer = null;
            }
         }
         base.Dispose(disposing);
      }
   }
}
