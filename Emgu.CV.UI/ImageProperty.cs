//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util.TypeEnum;
using Emgu.CV.Structure;
using System.Text;

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
         set
         {
            _imageBox = value;
            if (_imageBox != null)
            {
               //update control base on current funtional mode
               HandleFunctionalModeChange(this, new EventArgs());

               //register event such that feature change in functional mode will also trigger an update to the control
               _imageBox.OnFunctionalModeChanged += HandleFunctionalModeChange;
            }
         }
      }

      private void HandleFunctionalModeChange(Object sender, EventArgs args)
      {
         bool zoom = (_imageBox.FunctionalMode & ImageBox.FunctionalModeOption.PanAndZoom) == ImageBox.FunctionalModeOption.PanAndZoom;
         zoomLabel.Visible = zoomLevelComboBox.Visible = zoom;
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

         #region check if image is a subclass of CvArr type
         Type imageType = image.GetType();
         isCvArray = true;
         Type cvArrayType = typeof(CvArray<>);
         while (cvArrayType != imageType)
         {
            if (imageType.IsGenericType && imageType.GetGenericTypeDefinition() == cvArrayType)
            {
               break;
            }
            imageType = imageType.BaseType;
            if (imageType == null)
            {
               isCvArray = false;
               break;
            }
         }
         #endregion

         UpdateHistogram();
         UpdateZoomScale();
      }

      private bool isCvArray = true;

      /// <summary>
      /// A buffer used by SetMousePositionOnImage function
      /// </summary>
      private double[] _buffer = new double[4];

      /// <summary>
      /// Set the mouse position over the image. 
      /// It also set the color intensity of the pixel on the image where is mouse is at
      /// </summary>
      /// <param name="location">The location of the mouse on the image</param>
      public void SetMousePositionOnImage(Point location)
      {
         mousePositionTextbox.Text = location.ToString();

         IImage img = _imageBox.DisplayedImage;
         Size size = img.Size;
         location.X = Math.Min(location.X, size.Width - 1);
         location.Y = Math.Min(location.Y, size.Height - 1);

         if (isCvArray)
         {
            MCvScalar scalar = CvInvoke.cvGet2D(img.Ptr, location.Y, location.X);
            _buffer[0] = scalar.v0; _buffer[1] = scalar.v1; _buffer[2] = scalar.v2; _buffer[3] = scalar.v3;

            StringBuilder sb = new StringBuilder(String.Format("[{0}", _buffer[0]));
            for (int i = 1; i < img.NumberOfChannels; i++)
               sb.AppendFormat(",{0}", _buffer[i]);
            sb.Append("]");

            colorIntensityTextbox.Text = sb.ToString();
         }
         else
         {
            colorIntensityTextbox.Text = String.Empty;
         }
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

      public void UpdateZoomScale()
      {
         zoomLevelComboBox.Text = String.Format("{0}%", ImageBox.ZoomScale * 100);
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
            _imageBox.SetZoomScale(ImageBox.ZoomLevels[zoomLevelComboBox.SelectedIndex], Point.Empty);
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

            _imageBox.OnFunctionalModeChanged -= HandleFunctionalModeChange;
         }
         base.Dispose(disposing);
      }
   }
}
