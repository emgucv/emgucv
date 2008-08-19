using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;

namespace ShapeDetection
{
   public partial class MainForm : Form
   {
      public MainForm()
      {
         InitializeComponent();

         fileNameTextBox.Text = "pic3.png";
      }

      public void PerformShapeDetection()
      {
         if (fileNameTextBox.Text != String.Empty)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(fileNameTextBox.Text).Resize(400, 400, true);
            Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();
            Gray cannyThreshold = new Gray(180);
            Gray cannyThresholdLinking = new Gray(120);
            Gray circleAccumulatorThreshold = new Gray(120);
            Image<Gray, Byte> cannyEdges = gray.Canny(cannyThreshold, cannyThresholdLinking);

            Circle<float>[] circles = gray.HughCircles(
                cannyThreshold,
                circleAccumulatorThreshold,
                5.0, //Resolution of the accumulator used to detect centers of the circles
                10.0, //min distance 
                5, //min radius
                0 //max radius
                )[0]; //Get the circles from the first channel

            LineSegment2D<int>[] lines = cannyEdges.HughLinesBinary(
                1, //Distance resolution in pixel-related units
                Math.PI / 45.0, //Angle resolution measured in radians.
                20, //threshold
                30, //min Line width
                10 //gap between lines
                )[0]; //Get the lines from the first channel

            #region Find triangles and rectangles
            List<Triangle2D<int>> triangleList = new List<Triangle2D<int>>();
            List<Box2D<int>> boxList = new List<Box2D<int>>();

            using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
               for (Contour contours = cannyEdges.FindContours(); contours != null; contours = contours.HNext)
               {
                  Contour currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

                  if (contours.Area > 250) //only consider contours with area greater than 250
                  {
                     if (currentContour.Total == 3) //The contour has 3 vertices
                     {
                        Point2D<int>[] pts = Array.ConvertAll<MCvPoint, Point2D<int>>(currentContour.ToArray(), delegate(MCvPoint p) { return new Point2D<int>(p.x, p.y); });
                        triangleList.Add(new Triangle2D<int>(pts[0], pts[1], pts[2]));
                     }
                     else if (currentContour.Total == 4) //The contour has 4 vertices.
                     {
                        #region determine if all the angles in the contour are within the range of [80, 100] degree
                        bool isRectangle = true;
                        Point2D<int>[] pts = Array.ConvertAll<MCvPoint, Point2D<int>>(currentContour.ToArray(), delegate(MCvPoint p) { return new Point2D<int>(p.x, p.y); });
                        LineSegment2D<int>[] edges = PointCollection.PolyLine<int>(pts, true);

                        for (int i = 0; i < edges.Length; i++)
                        {
                           double angle = Math.Abs(edges[(i + 1) % edges.Length].GetExteriorAngleDegree(edges[i]));
                           if (angle < 80 || angle > 100)
                           {
                              isRectangle = false;
                              break;
                           }
                        }
                        #endregion

                        if (isRectangle) boxList.Add(currentContour.GetMinAreaRect());
                     }
                  }
               }
            #endregion

            originalImageBox.Image = img;

            Image<Bgr, Byte> triangleRectangleImage = img.CopyBlank();
            foreach (Triangle2D<int> triangle in triangleList)
               triangleRectangleImage.Draw(triangle, new Bgr(Color.DarkBlue), 2);
            foreach (Box2D<int> box in boxList)
               triangleRectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);
            triangleRectangleImageBox.Image = triangleRectangleImage;

            Image<Bgr, Byte> circleImage = img.CopyBlank();
            foreach (Circle<float> circle in circles)
               circleImage.Draw(circle, new Bgr(Color.Brown), 2);
            circleImageBox.Image = circleImage;

            Image<Bgr, Byte> lineImage = img.CopyBlank();
            foreach (LineSegment2D<int> line in lines)
               lineImage.Draw(line, new Bgr(Color.Green), 2);
            lineImageBox.Image = lineImage;
         }
      }

      private void textBox1_TextChanged(object sender, EventArgs e)
      {
         PerformShapeDetection();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         DialogResult result = openFileDialog1.ShowDialog();
         if (result == DialogResult.OK || result == DialogResult.Yes)
         {
            fileNameTextBox.Text = openFileDialog1.FileName;
         }
      }
   }
}