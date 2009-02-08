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
   public partial class ImageProperty : UserControl
   {
      private ImageBox _imageBox;

      /// <summary>
      /// The parent imagebox for this property panel
      /// </summary>
      public ImageBox ImageBox
      {
         get { return _imageBox; }
         set { _imageBox = value; }
      }

      /// <summary>
      /// Create a ImageProperty control
      /// </summary>
      public ImageProperty()
      {
         InitializeComponent();
         cSharpOperationStackView.Language = ProgrammingLanguage.CSharp;
         cPlusPlusoperationStackView.Language = ProgrammingLanguage.CPlusPlus;
      }

      /// <summary>
      /// Set the width of the image
      /// </summary>
      public int ImageWidth
      {
         set
         {
            widthTextbox.Text = value.ToString();
         }
      }

      /// <summary>
      /// Set the height of the image
      /// </summary>
      public int ImageHeight
      {
         set
         {
            heightTextBox.Text = value.ToString();
         }
      }

      /// <summary>
      /// Set the Type of the color
      /// </summary>
      public Type TypeOfColor
      {
         set
         {
            Object[] colorAttributes = value.GetCustomAttributes(typeof(ColorInfoAttribute), true);
            if (colorAttributes.Length > 0)
            {
               ColorInfoAttribute info = (ColorInfoAttribute)colorAttributes[0];
               typeOfColorTexbox.Text = info.ConversionCodename;
            }
            else
            {
               typeOfColorTexbox.Text = "Unknown";
            }
         }
      }

      /// <summary>
      /// Set the mouse position over the image
      /// </summary>
      public System.Drawing.Point MousePositionOnImage
      {
         set
         {
            mousePositionTextbox.Text = String.Format("[{0}, {1}]", value.X, value.Y);
         }
      }

      /// <summary>
      /// Set the color intensity of the pixel on the image where is mouse is at
      /// </summary>
      public IColor ColorIntensity
      {
         set
         {
            colorIntensityTextbox.Text = String.Format("[{0}]",
                value == null ? String.Empty : value.ToString());
         }
      }

      /// <summary>
      /// Set the Depth of the image
      /// </summary>
      public Type TypeOfDepth
      {
         set
         {
            typeOfDepthTextBox.Text = value.Name;
         }
      }

      /// <summary>
      /// Set the description of the operation stack
      /// </summary>
      public void SetOperationStack(Stack<Operation> stack)
      {
         cSharpOperationStackView.DisplayOperationStack(stack);
         cPlusPlusoperationStackView.DisplayOperationStack(stack);
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

      private void clearStackBtn_Click(object sender, EventArgs e)
      {
         _imageBox.ClearOperation();
      }

      private void popStackButton_Click(object sender, EventArgs e)
      {
         _imageBox.PopOperation();
      }

      private void showHistogramButton_Click(object sender, EventArgs e)
      {
         IImage image = _imageBox.DisplayedImage;

         if (image == null)
         {
            MessageBox.Show("Please load an image first");
            return;
         }

         IImage[] channels = image.Split();
         Type imageType = Toolbox.GetBaseType(image.GetType(), "Image`2");
         IColor typeOfColor = Activator.CreateInstance(imageType.GetGenericArguments()[0]) as IColor;
         String[] channelNames = Reflection.ReflectColorType.GetNamesOfChannels(typeOfColor);
         System.Drawing.Color[] colors = Reflection.ReflectColorType.GetDisplayColorOfChannels(typeOfColor);

         HistogramViewer hviewer = new HistogramViewer();

         float minVal, maxVal;
         #region Get the maximum and minimum color intensity values
         System.Type typeOfDepth = imageType.GetGenericArguments()[1];
         if (typeOfDepth == typeof(Byte))
         {
            minVal = 0.0f;
            maxVal = 256.0f;
         }
         else
         {
            #region obtain the maximum and minimum color value
            double[] minValues, maxValues;
            System.Drawing.Point[] minLocations, maxLocations;
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

            double min = minValues[0], max = maxValues[0];
            for (int i = 1; i < minValues.Length; i++)
            {
               if (minValues[i] < min) min = minValues[i];
               if (maxValues[i] > max) max = maxValues[i];
            }
            #endregion

            minVal = (float)min;
            maxVal = (float)max;
         }
         #endregion

         for (int i = 0; i < channels.Length; i++)
            using (Histogram hist = new Histogram(256, new RangeF(minVal, maxVal)))
            {
               hist.Accumulate(new IImage[1] { channels[i] });
               hviewer.HistogramCtrl.AddHistogram(channelNames[i], colors[i], hist);
            }

         hviewer.HistogramCtrl.Refresh();

         hviewer.Show();
      }
   }
}
