//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV.CvEnum;
//using ZedGraph;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;
using System.Linq;
using Emgu.CV.Plot;
using Emgu.CV.Util;

namespace Emgu.CV.UI
{
    /// <summary>
    /// The control that is used to display histogram
    /// </summary>
    public partial class HistogramBox : ImageBox
    {
        /// <summary>
        /// Construct a histogram control
        /// </summary>
        public HistogramBox()
        : base()
        {

        }

        /// <summary>
        /// Add a plot of the 1D histogram. 
        /// </summary>
        /// <param name="name">The name of the histogram</param>
        /// <param name="color">The drawing color</param>
        /// <param name="histogram">The 1D histogram to be drawn</param>
        /// <param name="binSize">The size of the bin</param>
        /// <param name="ranges">The ranges</param>
        /// <returns>The image of the histogram</returns>
        public Mat GenerateHistogram(String name, Color color, Mat histogram, int binSize, float[] ranges)
        {
            //Debug.Assert(histogram.Dimension == 1, Properties.StringTable.Only1DHistogramSupported);

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

            using (Matrix<double> m = new Matrix<double>(binVal.Length, 1, handle.AddrOfPinnedObject(),
                sizeof(double)))
            {
                histogram.ConvertTo(m, DepthType.Cv64F);
            }
            handle.Free();

            using (VectorOfDouble x = new VectorOfDouble(bin))
            using (VectorOfDouble y = new VectorOfDouble(binVal))
            {
                using (Emgu.CV.Plot.Plot2d plot = new Plot2d(x, y))
                {
                    plot.SetShowText(false);
                    plot.SetPlotBackgroundColor(new MCvScalar(255, 255, 255));
                    plot.SetPlotLineColor(new MCvScalar(0, 0, 0));
                    plot.SetPlotGridColor(new MCvScalar(220, 220, 220));
                    plot.SetGridLinesNumber(255);
                    plot.SetPlotSize(512, 200);
                    plot.SetMinX(0);
                    plot.SetMaxX(256);
                    plot.SetMinY(-1);
                    plot.SetMaxY(binVal.Max());
                    plot.SetInvertOrientation(true);

                    Mat render = new Mat();
                    plot.Render(render);
                    CvInvoke.PutText(render, name, new Point(20, 30), FontFace.HersheyComplex, 0.8,
                        new MCvScalar(0, 0, 255));
                    return render;
                }
            }
            #endregion

        }

        /// <summary>
        /// Generate histograms for the image. One histogram is generated for each color channel.
        /// You will need to call the Refresh function to do the painting afterward.
        /// </summary>
        /// <param name="image">The image to generate histogram from</param>
        /// <param name="numberOfBins">The number of bins for each histogram</param>
        public void GenerateHistograms(IInputArray image, int numberOfBins)
        {
            using (InputArray iaImage = image.GetInputArray())
            {
                int channelCount = iaImage.GetChannels();
                Mat[] channels = new Mat[channelCount];
                Type imageType;
                if ((imageType = Toolbox.GetBaseType(image.GetType(), "Image`2")) != null
                    || (imageType = Toolbox.GetBaseType(image.GetType(), "Mat")) != null
                    || (imageType = Toolbox.GetBaseType(image.GetType(), "UMat")) != null)
                {
                    for (int i = 0; i < channelCount; i++)
                    {
                        Mat channel = new Mat();
                        CvInvoke.ExtractChannel(image, channel, i);
                        channels[i] = channel;
                    }

                }
                else if ((imageType = Toolbox.GetBaseType(image.GetType(), "CudaImage`2")) != null)
                {
                    using (Mat img = imageType.GetMethod("ToMat").Invoke(image, null) as Mat)
                        for (int i = 0; i < channelCount; i++)
                        {
                            Mat channel = new Mat();
                            CvInvoke.ExtractChannel(img, channel, i);
                            channels[i] = channel;
                        }
                }
                else
                {
                    throw new ArgumentException(String.Format("The input image type of {0} is not supported",
                        image.GetType().ToString()));
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

                    channelNames = new String[channelCount];
                    colors = new Color[channelCount];
                    for (int i = 0; i < channelCount; i++)
                    {
                        channelNames[i] = String.Format("Channel {0}", i);
                        colors[i] = Color.Red;
                    }

                    if (image is Mat)
                    {
                        typeOfDepth = CvInvoke.GetDepthType(((Mat)image).Depth);
                    }
                    else if (image is UMat)
                    {
                        typeOfDepth = CvInvoke.GetDepthType(((UMat)image).Depth);
                    }
                    else
                    {
                        throw new ArgumentException(String.Format(
                            "Unable to get the type of depth from image of type {0}", image.GetType().ToString()));
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
                    using (InputArray ia = image.GetInputArray())
                    using (Mat m = ia.GetMat())
                    {
                        m.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                        double min = minValues[0], max = maxValues[0];
                        for (int i = 1; i < minValues.Length; i++)
                        {
                            if (minValues[i] < min) min = minValues[i];
                            if (maxValues[i] > max) max = maxValues[i];
                        }

                        minVal = (float)min;
                        maxVal = (float)max;
                    }
                    #endregion
                }
                #endregion

                Mat[] histograms = new Mat[channels.Length];
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
                        histograms[i] = GenerateHistogram(channelNames[i], colors[i], hist, numberOfBins, ranges);
                    }
                }

                if (histograms.Length == 1)
                {
                    this.Image = histograms[0];
                }
                else
                {
                    int maxWidth = 0;
                    int totalHeight = 0;
                    for (int i = 0; i < histograms.Length; i++)
                    {
                        maxWidth = Math.Max(maxWidth, histograms[i].Width);
                        totalHeight += histograms[i].Height;
                    }
                    Mat concated = new Mat(new Size(maxWidth, totalHeight), histograms[0].Depth, histograms[0].NumberOfChannels);

                    int currentY = 0;
                    for (int i = 0; i < histograms.Length; i++)
                    {
                        using (Mat roi = new Mat(concated, new Rectangle(new Point(0, currentY), histograms[i].Size)))
                        {
                            histograms[i].CopyTo(roi);
                        }
                        currentY += histograms[i].Height;
                        histograms[i].Dispose();
                    }

                    this.Image = concated;
                }

            }
        }

        /// <summary>
        /// Remove all the histogram from the control. You should call the Refresh() function to update the control after all modification is complete.
        /// </summary>
        public void ClearHistogram()
        {
            this.Image = null;
        }

    }
}