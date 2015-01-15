//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV.CvEnum;
using ZedGraph;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.UI
{
   /// <summary>
   /// The control that is used to display histogram
   /// </summary>
   public partial class HistogramBox : UserControl
   {
      private Graphics _graphic;

      /// <summary>
      /// Construct a histogram control
      /// </summary>
      public HistogramBox()
      {
         InitializeComponent();

         #region Setup the graph
         // First, clear out any old GraphPane's from the MasterPane collection
         MasterPane master = zedGraphControl1.MasterPane;
         master.PaneList.Clear();

         // Display the MasterPane Title, and set the outer margin to 10 points
         master.Title.IsVisible = true;
         master.Title.Text = Properties.StringTable.DefaultHistogramTitle;
         master.Margin.All = 10;
         #endregion

         // Layout the GraphPanes using a default Pane Layout
         _graphic = CreateGraphics();
         
         // Size the control to fill the form with a margin
         SetSize();
      }

      private void HistogramViewer_Resize(object sender, EventArgs e)
      {
         SetSize();
      }

      // SetSize() is separate from Resize() so we can 
      // call it independently from the Form1_Load() method
      // This leaves a 10 px margin around the outside of the control
      // Customize this to fit your needs
      private void SetSize()
      {
         zedGraphControl1.Location = new Point(10, 10);
         // Leave a small margin around the outside of the control
         zedGraphControl1.Size = new Size(ClientRectangle.Width - 20,
                                 ClientRectangle.Height - 20);
      }

      /// <summary>
      /// Get the zedgraph control from this histogram control
      /// </summary>
      public ZedGraphControl ZedGraphControl
      {
         get
         {
            return zedGraphControl1;
         }
      }

      /// <summary>
      /// Add a plot of the 1D histogram. You should call the Refresh() function to update the control after all modification is complete.
      /// </summary>
      /// <param name="name">The name of the histogram</param>
      /// <param name="color">The drawing color</param>
      /// <param name="histogram">The 1D histogram to be drawn</param>
      /// <param name="binSize">The size of the bin</param>
      /// <param name="ranges">The ranges</param>
      public void AddHistogram(String name, Color color, Mat histogram, int binSize, float[] ranges)
      {
         //Debug.Assert(histogram.Dimension == 1, Properties.StringTable.Only1DHistogramSupported);

         GraphPane pane = new GraphPane();
         // Set the Title
         pane.Title.Text = name;
         pane.XAxis.Title.Text = Properties.StringTable.Value;
         pane.YAxis.Title.Text = Properties.StringTable.Count;

         #region draw the histogram
         RangeF range = new RangeF(ranges[0], ranges[1]);
         
         float step = (range.Max - range.Min) / binSize;
         float start = range.Min;
         double[] bin = new double[binSize];
         for (int binIndex = 0; binIndex < binSize; binIndex++)
         {
            bin[binIndex] = start;
            start += step;
         }

         double[] binVal = new double[histogram.Size.Height];
         GCHandle handle = GCHandle.Alloc(binVal, GCHandleType.Pinned);
         using (Matrix<double> m = new Matrix<double>(binVal.Length, 1, handle.AddrOfPinnedObject(), sizeof(double)))
         {
            histogram.ConvertTo(m, DepthType.Cv64F);
            PointPairList pointList = new PointPairList(
               bin,
               binVal);

            pane.AddCurve(name, pointList, color);
         }
         handle.Free();
         
         #endregion

         zedGraphControl1.MasterPane.Add(pane);
      }

      /// <summary>
      /// Generate histograms for the image. One histogram is generated for each color channel.
      /// You will need to call the Refresh function to do the painting afterward.
      /// </summary>
      /// <param name="image">The image to generate histogram from</param>
      /// <param name="numberOfBins">The number of bins for each histogram</param>
      public void GenerateHistograms(IImage image, int numberOfBins)
      {
         Mat[] channels = new Mat[image.NumberOfChannels];
         Type imageType; 
         if ((imageType = Toolbox.GetBaseType(image.GetType(), "Image`2")) != null 
            || (imageType = Toolbox.GetBaseType(image.GetType(), "Mat")) != null)
         {
            for (int i = 0; i < image.NumberOfChannels; i++)
            {
               Mat channel = new Mat();
               CvInvoke.ExtractChannel(image, channel, i);
               channels[i] = channel;
            }
             
         }
         else if ((imageType = Toolbox.GetBaseType(image.GetType(), "CudaImage`2")) != null)
         {
            IImage img = imageType.GetMethod("ToImage").Invoke(image, null) as IImage;
            for (int i = 0; i < img.NumberOfChannels; i++)
            {
               Mat channel = new Mat();
               CvInvoke.ExtractChannel(img, channel, i);
               channels[i] = channel;
            }
         }
         else
         {
            throw new ArgumentException(String.Format("The input image type of {0} is not supported", image.GetType().ToString()));
         }

         Type[] genericArguments = imageType.GetGenericArguments();
         String[] channelNames;
         Color[] colors;
         Type typeOfDepth;
         if (genericArguments.Length > 0)
         {
            IColor typeOfColor = Activator.CreateInstance(genericArguments[0]) as IColor;
            channelNames = Reflection.ReflectColorType.GetNamesOfChannels(typeOfColor);
            colors = Reflection.ReflectColorType.GetDisplayColorOfChannels(typeOfColor);
            typeOfDepth = imageType.GetGenericArguments()[1];
         }
         else
         {
            channelNames = new String[image.NumberOfChannels];
            colors = new Color[image.NumberOfChannels];
            for (int i = 0; i < image.NumberOfChannels; i++)
            {
               channelNames[i] = String.Format("Channel {0}", i);
               colors[i] = Color.Red;
            }

            if (image is Mat)
            {
               typeOfDepth = CvInvoke.GetDepthType(((Mat)image).Depth);
            }
            else
            {
               throw new ArgumentException(String.Format("Unable to get the type of depth from image of type {0}", image.GetType().ToString()));
            }
            
         }
         
         float minVal, maxVal;
         #region Get the maximum and minimum color intensity values
         
         if (typeOfDepth == typeof(Byte))
         {
            minVal = 0.0f;
            maxVal = 256.0f;
         }
         else
         {
            #region obtain the maximum and minimum color value
            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
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
         {
            
            //using (DenseHistogram hist = new DenseHistogram(numberOfBins, new RangeF(minVal, maxVal)))
            using (Mat hist = new Mat())
            using (Util.VectorOfMat vm = new Util.VectorOfMat())
            {
               vm.Push(channels[i]);

               float[] ranges = new float[] { minVal, maxVal };
               CvInvoke.CalcHist(vm, new int[] { 0 }, null, hist, new int[] { numberOfBins }, ranges, false);
               //hist.Calculate(new IImage[1] { channels[i] }, true, null);
               AddHistogram(channelNames[i], colors[i], hist, numberOfBins, ranges );
            }
         }
      }

      /// <summary>
      /// Remove all the histogram from the control. You should call the Refresh() function to update the control after all modification is complete.
      /// </summary>
      public void ClearHistogram()
      {
         zedGraphControl1.MasterPane.PaneList.Clear();
      }

      /// <summary>
      /// Paint the histogram
      /// </summary>
      public new void Refresh()
      {
         zedGraphControl1.MasterPane.AxisChange(_graphic);
         zedGraphControl1.MasterPane.SetLayout(_graphic, PaneLayout.SingleColumn);
         base.Refresh();
      }
   }
}