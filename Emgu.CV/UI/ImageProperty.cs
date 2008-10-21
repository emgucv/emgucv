using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
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
               typeOfColorTexbox.Text = info.ConversionCodeName;
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
      public Point2D<int> MousePositionOnImage
      {
         set
         {
            mousePositionTextbox.Text = String.Format("[{0}, {1}]", value.X, value.Y);
         }
      }

      /// <summary>
      /// Set the color intensity of the pixel on the image where is mouse is at
      /// </summary>
      public ColorType ColorIntensity
      {
         set
         {
            colorIntensityTextbox.Text = String.Format("[{0}]",
                value == null ? "" : String.Join(",", System.Array.ConvertAll<double, String>(value.Coordinate, System.Convert.ToString)));
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
      public void SetOperationStack(Stack<Operation<IImage>> stack)
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

      private static Point2D<int>[] SingleChannelImageToHistogramPoints(IImage channel, int numberOfBins, float minVal, float maxVal)
      {
         int[] binSize = new int[1] { numberOfBins };
         float[] min = new float[1] { minVal };
         float[] max = new float[1] { maxVal };

         using (Histogram hist = new Histogram(binSize, min, max))
         {
            hist.Clear(); //this is required since the initial histogram might contains random values
            hist.Accumulate(new IImage[] { channel });

            //all the values of the histogram for the specific color channel
            Point2D<int>[] pts = new Point2D<int>[binSize[0]];
            for (int binIndex = 0; binIndex < binSize[0]; binIndex++)
               pts[binIndex] = new Point2D<int>(binIndex, (int)hist.Query(binIndex));

            return pts;
         }
      }

      private void showHistogramButton_Click(object sender, EventArgs e)
      {
         IImage image = _imageBox.DisplayedImage;

         IImage[] channels = image.Split();
         ColorType typeOfColor = ((ColorType)Activator.CreateInstance(image.TypeOfColor));
         String[] channelNames = typeOfColor.ChannelName;
         System.Drawing.Color[] colors = typeOfColor.ChannelColor;

         HistogramViewer hviewer = new HistogramViewer();

         if (image.TypeOfDepth == typeof(Byte))
         {
            for (int i = 0; i < channels.Length; i++)
            {
               Point2D<int>[] pts = SingleChannelImageToHistogramPoints(channels[i], 256, 0.0f, 255f);
               hviewer.HistogramCtrl.AddHistogram(channelNames[i], colors[i], pts);
            }
            hviewer.HistogramCtrl.Paint();
         }
         else
         {
            #region obtain the maximum and minimum color value
            double[] minValues, maxValues;
            MCvPoint[] minLocations, maxLocations;
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

            double min = minValues[0], max = maxValues[0];
            for (int i = 1; i < minValues.Length; i++)
            {
               if (minValues[i] < min) min = minValues[i];
               if (maxValues[i] > max) max = maxValues[i];
            }
            #endregion

            for (int i = 0; i < channels.Length; i++)
            {
               Point2D<int>[] pts = SingleChannelImageToHistogramPoints(channels[i], 256, (float)min, (float)max);
               hviewer.HistogramCtrl.AddHistogram(channelNames[i], colors[i], pts);
            }
         }

         hviewer.Show();
      }
   }
}
