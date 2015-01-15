//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
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

      private bool IsSubTypeOf(Type objType, Type typeToCheck)
      {
         while (objType != typeToCheck)
         {
            if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeToCheck)
            {
               return true;
            }
            objType = objType.BaseType;
            if (objType == null)
            {
               return false;
            }
         }
         return true;
      }

      public void SetImage(IImage image)
      {

         #region display the size of the image
         if (image != null)
         {
            Size size = image.Size;
            widthTextbox.Text = size.Width.ToString();
            heightTextBox.Text = size.Height.ToString();
         }
         else
         {
            widthTextbox.Text = String.Empty;
            heightTextBox.Text = string.Empty;
         }
         #endregion

         #region display the color type of the image
         if (image != null)
         {
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
         }
         else
         {
            typeOfColorTexbox.Text = string.Empty;
            typeOfDepthTextBox.Text = string.Empty;
         }
         #endregion

         #region check if image is a subclass of CvArr type
         if (image != null)
         {
            Type imgType = image.GetType();
            if (IsSubTypeOf(imgType, typeof (CvArray<>)))
               _imageType = typeof (CvArray<>);
            else if (IsSubTypeOf(imgType, typeof (Mat)))
               _imageType = typeof (Mat);
            else
            {
               _imageType = null;
            }
         }
         else
         {
            _imageType = null;
            
         }
         #endregion

         UpdateHistogram();
         UpdateZoomScale();
      }

      private Type _imageType = null;
      //private bool isCvArray = true;

      private String BufferToString<T>(T[] data, int numberOfChannels)
      {
         StringBuilder sb = new StringBuilder(String.Format("[{0}", data[0]));
         for (int i = 1; i < numberOfChannels; i++)
            sb.AppendFormat(",{0}", data[i]);
         sb.Append("]");
         return sb.ToString();
      }

      /// <summary>
      /// Set the mouse position over the image. 
      /// It also set the color intensity of the pixel on the image where is mouse is at
      /// </summary>
      /// <param name="location">The location of the mouse on the image</param>
      public void SetMousePositionOnImage(Point location)
      {
         IImage img = _imageBox.DisplayedImage;
         Size size = img.Size;
         location.X = Math.Max( Math.Min(location.X, size.Width - 1), 0);
         location.Y = Math.Max( Math.Min(location.Y, size.Height - 1), 0);

         mousePositionTextbox.Text = location.ToString();

         if (_imageType == typeof(CvArray<>))
         {
            MCvScalar scalar = CvInvoke.cvGet2D(img.Ptr, location.Y, location.X);
            
            colorIntensityTextbox.Text = BufferToString(scalar.ToArray(), img.NumberOfChannels);
         }
         else if (_imageType == typeof (Mat))
         {
            Mat mat = img as Mat;
            byte[] raw =  mat.GetData(location.Y, location.X);

            if (mat.Depth == DepthType.Cv8U)
            {
               colorIntensityTextbox.Text = BufferToString(raw, img.NumberOfChannels);
            }
            else if (mat.Depth == DepthType.Cv8S)
            {
               sbyte[] data = new sbyte[img.NumberOfChannels];
               GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
               Marshal.Copy(raw, 0, handle.AddrOfPinnedObject(), mat.ElementSize);
               handle.Free();
               colorIntensityTextbox.Text = BufferToString(data, data.Length);
            }
            else if (mat.Depth == DepthType.Cv16S)
            {
               GCHandle handle = GCHandle.Alloc(raw, GCHandleType.Pinned);
               short[] data = new short[img.NumberOfChannels];
               Marshal.Copy(handle.AddrOfPinnedObject(), data, 0, data.Length);
               handle.Free();
               colorIntensityTextbox.Text = BufferToString(data, data.Length);
            }
            else if (mat.Depth == DepthType.Cv16U)
            {
               UInt16[] data = new UInt16[img.NumberOfChannels];
               GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
               Marshal.Copy(raw, 0, handle.AddrOfPinnedObject(), mat.ElementSize);
               handle.Free();
               colorIntensityTextbox.Text = BufferToString(data, data.Length);
            }
            else if (mat.Depth == DepthType.Cv32F)
            {
               GCHandle handle = GCHandle.Alloc(raw, GCHandleType.Pinned);
               float[] floatData = new float[img.NumberOfChannels];
               Marshal.Copy(handle.AddrOfPinnedObject(), floatData, 0, floatData.Length);
               handle.Free();
               colorIntensityTextbox.Text = BufferToString(floatData, floatData.Length);
            }
            else if (mat.Depth == DepthType.Cv32S)
            {
               int[] data = new int[img.NumberOfChannels];
               GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
               Marshal.Copy(raw, 0, handle.AddrOfPinnedObject(), mat.ElementSize);
               handle.Free();
               colorIntensityTextbox.Text = BufferToString(data, data.Length);
            }
            else if (mat.Depth == DepthType.Cv64F)
            {
               GCHandle handle = GCHandle.Alloc(raw, GCHandleType.Pinned);
               double[] doubleData = new double[img.NumberOfChannels];
               Marshal.Copy(handle.AddrOfPinnedObject(), doubleData, 0, doubleData.Length);
               handle.Free();
               colorIntensityTextbox.Text = BufferToString(doubleData, doubleData.Length);
            }
            else
            {
               colorIntensityTextbox.Text = String.Empty;
            }
         } else
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
