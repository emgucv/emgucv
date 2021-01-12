//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Emgu.CV.UI
{
   /// <summary>
   /// A view for histogram
   /// </summary>
   public partial class HistogramViewer : Form
   {
      /// <summary>
      /// A histogram viewer
      /// </summary>
      public HistogramViewer()
      {
         InitializeComponent();
      }

      /// <summary>
      /// Display the histograms of the specific image
      /// </summary>
      /// <param name="image">The image to retrieve histogram from</param>
      public static void Show(IInputArray image)
      {
         Show(image, 256);
      }

      /// <summary>
      /// Display the histograms of the specific image
      /// </summary>
      /// <param name="image">The image to retrieve histogram from</param>
      /// <param name="numberOfBins">The number of bins in the histogram</param>
      public static void Show(IInputArray image, int numberOfBins)
      {
         HistogramViewer viewer = new HistogramViewer();
         viewer.HistogramCtrl.GenerateHistograms(image, numberOfBins);
         viewer.HistogramCtrl.Refresh();
         viewer.ShowDialog();
      }

      /*
      /// <summary>
      /// Display the specific histogram
      /// </summary>
      /// <param name="hist">The 1 dimension histogram to be displayed</param>
      /// <param name="title">The name of the histogram</param>
      public static void Show(DenseHistogram hist, string title)
      {
         HistogramViewer viewer = new HistogramViewer();
         if (hist.Dimension == 1)
            viewer.HistogramCtrl.AddHistogram(title, Color.Black, hist);
         
         viewer.HistogramCtrl.Refresh();
         viewer.Show();
      }*/

      /// <summary>
      /// Get the histogram control of this viewer
      /// </summary>
      public HistogramBox HistogramCtrl
      {
         get
         {
            return histogramCtrl1;
         }
      }
   }
}
