using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using Emgu.CV;

namespace Emgu.CV.UI
{
   /// <summary>
   /// The histogram control
   /// </summary>
   public partial class HistogramCtrl : UserControl
   {
      /// <summary>
      /// Construct a histogram control
      /// </summary>
      public HistogramCtrl()
      {
         InitializeComponent();

         #region Setup the graph
         // get a reference to the GraphPane
         GraphPane myPane = zedGraphControl1.GraphPane;

         // Set the Titles
         myPane.Title.Text = "Histogram";
         myPane.XAxis.Title.Text = "Color Intensity";
         myPane.YAxis.Title.Text = "Pixel Count";
         #endregion

         // Size the control to fill the form with a margin
         SetSize();

      }

      private void HistogramViewer_Load(object sender, EventArgs e)
      {

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
      /// Add a plot of the histogram
      /// </summary>
      /// <param name="name">The name of the histogram</param>
      /// <param name="color"></param>
      /// <param name="values"></param>
      public void AddHistogram(String name, System.Drawing.Color color, IEnumerable<Point2D<int>> values)
      {
         PointPairList list1 = new PointPairList();

         foreach (Point2D<int> point in values)
            //if (point.Y != 0)
            list1.Add(point.X, point.Y);

         // Generate a curve of color with diamond
         // symbols, and name in the legend
         zedGraphControl1.GraphPane.AddBar(name, list1, color);

         // Tell ZedGraph to refigure the
         // axes since the data have changed
         zedGraphControl1.AxisChange();
      }
   }
}
