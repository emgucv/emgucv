using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Tao.OpenGl;

namespace Simlpe3DReconstruction
{
   public partial class Simple3DReconstruction : Form
   {
      private MCvPoint3D32f[] _points;
      private Image<Bgr, Byte> _left;
      private static double _angle = 0.0;
      private static double _angleIncrement = 0.2;
      public Simple3DReconstruction()
      {
         InitializeComponent();

         _left = new Image<Bgr, byte>("left.jpg");
         Image<Bgr, Byte> right = new Image<Bgr, byte>("right.jpg");
         Image<Gray, Int16> leftDisparityMap;
         Computer3DPointsFromImages(_left.Convert<Gray, Byte>(), right.Convert<Gray, Byte>(), out leftDisparityMap, out _points);
         //Display the disparity map
         imageBox1.Image = leftDisparityMap;

         #region Initialize OpenGL control
         simpleOpenGlControl1.InitializeContexts();
         Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
         Gl.glMatrixMode(Gl.GL_PROJECTION);
         Gl.glLoadIdentity();
         Gl.glOrtho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);

         #region Create texture for the 3D Point clouds
         Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
         Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
         Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
         Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
         Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);
         Gl.glShadeModel(Gl.GL_SMOOTH);
         Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);

         Gl.glBindTexture(Gl.GL_TEXTURE_2D, 1);

         Size size = _left.Size;
         int maxDim = Math.Max(size.Width, size.Height);
         using (Image<Bgr, Byte> squareImg = new Image<Bgr, byte>(maxDim, maxDim))
         {
            Rectangle roi = new Rectangle(maxDim / 2 - size.Width / 2, maxDim / 2 - size.Height / 2, size.Width, size.Height);
            squareImg.ROI = roi;
            CvInvoke.cvCopy(_left, squareImg, IntPtr.Zero);
            squareImg.ROI = Rectangle.Empty;
            using (Image<Bgr, Byte> texture = squareImg.Resize(256, 256, true))
            {
               texture._Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);
               Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, texture.Width, texture.Height, 0, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, texture.MIplImage.imageData);
               Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, texture.Width, texture.Height, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, texture.MIplImage.imageData);
            }
         }
         #endregion

         #endregion
      }

      /// <summary>
      /// Given the left and right image, computer the disparity map and the 3D point cloud.
      /// </summary>
      /// <param name="left">The left image</param>
      /// <param name="right">The right image</param>
      /// <param name="leftDisparityMap">The left disparity map</param>
      /// <param name="points">The 3D point cloud within a [-0.5, 0.5] cube</param>
      private static void Computer3DPointsFromImages(Image<Gray, Byte> left, Image<Gray, Byte> right, out Image<Gray, Int16> leftDisparityMap, out MCvPoint3D32f[] points)
      {
         Size size = left.Size;

         using (Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(size))
         using (Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(size))
         using (StereoGC gc = new StereoGC(16, 2))
         {
            gc.FindStereoCorrespondence(left, right, leftDisparity, rightDisparity);

            leftDisparityMap = leftDisparity * (-16);

            float scale = Math.Max(size.Width, size.Height);

            //Construct a simple Q matrix, if you have a matrix from cvStereoRectify, you should use that instead
            using (Matrix<double> q = new Matrix<double>(
               new double[,] {
                  {1.0, 0.0, 0.0, -size.Width/2}, //shift the x origin to image center
                  {0.0, -1.0, 0.0, size.Height/2}, //shift the y origin to image center and flip it upside down
                  {0.0, 0.0, 16.0, 0.0}, //Multiply the z value by 16, 
                  {0.0, 0.0, 0.0, scale}})) //scale the object's corrdinate to within a [-0.5, 0.5] cube
               points = PointCollection.ReprojectImageTo3D(leftDisparity, q);
         }
      }

      /// <summary>
      /// This function draw the 3D point cloud using the left image as texture.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
      {
         Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

         Gl.glEnable(Gl.GL_TEXTURE_2D);

         Gl.glPushMatrix();
         Gl.glRotated(10.0, 0.0, 1.0, 0.0);
         Gl.glRotated(_angle, 0.0, 1.0, 0.0);

         Gl.glBegin(Gl.GL_POINTS);

         foreach (MCvPoint3D32f p in _points)
         {
            Gl.glTexCoord2f(p.x + 0.5f, p.y + 0.5f);
            Gl.glVertex3f(p.x, p.y, p.z);
         }

         Gl.glEnd();
         Gl.glPopMatrix();

         if (_angle >= 30.0 || _angle <= -30.0) _angleIncrement = -_angleIncrement;
         _angle += _angleIncrement;
         simpleOpenGlControl1.Invalidate();
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            simpleOpenGlControl1.DestroyContexts();
            components.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
