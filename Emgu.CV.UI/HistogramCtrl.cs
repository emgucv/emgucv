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
      private Graphics _graphic;

      /// <summary>
      /// Construct a histogram control
      /// </summary>
      public HistogramCtrl()
      {
         InitializeComponent();

         #region Setup the graph
         // First, clear out any old GraphPane's from the MasterPane collection
         MasterPane master = zedGraphControl1.MasterPane;
         master.PaneList.Clear();

         // Display the MasterPane Title, and set the outer margin to 10 points
         master.Title.IsVisible = true;
         master.Title.Text = "Histogram";
         master.Margin.All = 10;
         #endregion

         // Layout the GraphPanes using a default Pane Layout
         _graphic = this.CreateGraphics();
         
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
      /// Add a plot of the histogram. You should call the Refresh() function to update the control after all modification is complete.
      /// </summary>
      /// <param name="name">The name of the histogram</param>
      /// <param name="color">The drawing color</param>
      /// <param name="values">The points on the histogram</param>
      public void AddHistogram(String name, System.Drawing.Color color, IEnumerable<Point2D<int>> values)
      {
         GraphPane pane = new GraphPane();

         // Set the Title
         pane.Title.Text = name;
         pane.XAxis.Title.Text = "Color Intensity";
         pane.YAxis.Title.Text = "Pixel Count";

         PointPairList list1 = new PointPairList();

         foreach (Point2D<int> point in values)
            //if (point.Y != 0)
            list1.Add(point.X, point.Y);

         pane.AddCurve(name, list1, color);

         zedGraphControl1.MasterPane.Add(pane);
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
