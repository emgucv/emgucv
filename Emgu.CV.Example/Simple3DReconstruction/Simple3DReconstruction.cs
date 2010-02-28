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
using OsgViewer;

namespace Simlpe3DReconstruction
{
   public partial class Simple3DReconstruction : Form
   {
      private MCvPoint3D32f[] _points;
      private Image<Bgr, Byte> _left;

      /// <summary>
      /// Convert an Emgu CV image to Osg Image
      /// </summary>
      /// <param name="image">An Emgu CV image</param>
      /// <returns>An Osg Image</returns>
      private static Osg.Image ConvertImage(Image<Bgr, Byte> image)
      {
         Osg.Image res = new Osg.Image();
         OsgWrapper.UnsignedCharPointer ptr = new OsgWrapper.UnsignedCharPointer();
         ptr.Ptr = image.MIplImage.imageData;

         res.setImage(image.Width, image.Height, image.NumberOfChannels,
            Gl.GL_RGB, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, ptr,
            Osg.Image.AllocationMode.USE_NEW_DELETE);
         return res;
      }

      /// <summary>
      /// Set the textuer for the specific geode
      /// </summary>
      /// <param name="textureImage">The texture</param>
      /// <param name="geode">The geode</param>
      private static void SetTexture(Image<Bgr, Byte> textureImage, Osg.Geode geode)
      {
         //Create a new StateSet with default settings
         Osg.StateSet state = new Osg.StateSet();
         Osg.Texture2D texture = new Osg.Texture2D(ConvertImage(textureImage));
         state.setTextureAttributeAndModes(0, texture, 1);
         geode.setStateSet(state);
      }

      public Simple3DReconstruction()
      {
         InitializeComponent();

         _left = new Image<Bgr, byte>("left.jpg");
         Image<Bgr, Byte> right = new Image<Bgr, byte>("right.jpg");
         Image<Gray, Int16> leftDisparityMap;
         Computer3DPointsFromImages(_left.Convert<Gray, Byte>(), right.Convert<Gray, Byte>(), out leftDisparityMap, out _points);

         //remove some depth outliers
         for (int i = 0; i < _points.Length; i++)
         {
            if (Math.Abs(_points[i].z) >= 1000) _points[i].z = 0;
         }

         //Display the disparity map
         imageBox1.Image = leftDisparityMap;

         Osg.Geode geode = new Osg.Geode();
         Osg.Geometry geometry = new Osg.Geometry();

         int textureSize = 256;
         //create and setup the texture
         SetTexture(_left.Resize(textureSize, textureSize, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC), geode);

         #region setup the vertices
         Osg.Vec3Array vertices = new Osg.Vec3Array();
         foreach (MCvPoint3D32f p in _points)
            vertices.Add(new Osg.Vec3(-p.x, p.y, p.z));
         geometry.setVertexArray(vertices);
         #endregion

         #region setup the primitive as point cloud
         Osg.DrawElementsUInt draw = new Osg.DrawElementsUInt(
            (uint)Osg.PrimitiveSet.Mode.POINTS, 0);
         for (uint i = 0; i < _points.Length; i++)
            draw.Add(i);
         geometry.addPrimitiveSet(draw);
         #endregion

         #region setup the texture coordinates for the pixels
         Osg.Vec2Array textureCoor = new Osg.Vec2Array();
         foreach (MCvPoint3D32f p in _points)
            textureCoor.Add(new Osg.Vec2(p.x / _left.Width + 0.5f, p.y / _left.Height + 0.5f));
         geometry.setTexCoordArray(0, textureCoor);
         #endregion

         geode.addDrawable(geometry);

         #region apply the rotation on the scene
         Osg.MatrixTransform transform = new Osg.MatrixTransform(
            Osg.Matrix.rotate(90.0 / 180.0 * Math.PI, new Osg.Vec3d(1.0, 0.0, 0.0)) *
            Osg.Matrix.rotate(180.0 / 180.0 * Math.PI, new Osg.Vec3d(0.0, 1.0, 0.0)));
         transform.addChild(geode);
         #endregion         

         viewer3D.Viewer.setSceneData(transform);
         viewer3D.Viewer.realize();
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
         //using (StereoSGBM stereoSolver = new StereoSGBM(5, 64, 0, 0, 0, 0, 0, 0, 0, 0))
         using (StereoGC stereoSolver = new StereoGC(16, 2))
         {
            stereoSolver.FindStereoCorrespondence(left, right, leftDisparity, rightDisparity);
            //stereoSolver.FindStereoCorrespondence(left, right, leftDisparity);

            leftDisparityMap = leftDisparity * (-16);
            //leftDisparityMap = leftDisparity.Clone();

            //Construct a simple Q matrix, if you have a matrix from cvStereoRectify, you should use that instead
            using (Matrix<double> q = new Matrix<double>(
               new double[,] {
                  {1.0, 0.0, 0.0, -size.Width/2}, //shift the x origin to image center
                  {0.0, 1.0, 0.0, -size.Height/2}, //shift the y origin to image center
                  {0.0, 0.0, -16.0, 0.0}, //Multiply the z value by -16, 
                  {0.0, 0.0, 0.0, 1.0}}))
               points = PointCollection.ReprojectImageTo3D(leftDisparity, q);
         }
      }

      /// <summary>
      /// This function draw the 3D point cloud using the left image as texture.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void viewer3D_Paint(object sender, PaintEventArgs e)
      {
         if (viewer3D.Viewer != null && !viewer3D.Viewer.done())
         {
            viewer3D.Viewer.updateTraversal();
            viewer3D.Viewer.frame();
         }

         viewer3D.Invalidate(); //this create a repaint loop
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (components != null)
               components.Dispose();

            viewer3D.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
