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
using OsgTerrain;

namespace Simlpe3DReconstruction
{
   public partial class Simple3DReconstruction : Form
   {
      private MCvPoint3D32f[] _points;
      private Image<Bgr, Byte> _left;

      private Viewer _viewer;

      private Osg.Image ConvertImage(Image<Bgr, Byte> image)
      {
         Osg.Image res = new Osg.Image();
         OsgWrapper.UnsignedCharPointer ptr = new OsgWrapper.UnsignedCharPointer();
         ptr.Ptr = image.MIplImage.imageData;
         
         res.setImage(image.Width, image.Height, image.NumberOfChannels,
            (int)Gl.GL_RGB, (int)Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, ptr,
            Osg.Image.AllocationMode.USE_NEW_DELETE);
         return res;
      }

      public Simple3DReconstruction()
      {
         int textureWidth = 256;
         int textureHeight = 256;

         InitializeComponent();
         _viewer = simpleOsgControl.Viewer;

         _left = new Image<Bgr, byte>("left.jpg");
         Image<Bgr, Byte> right = new Image<Bgr, byte>("right.jpg");
         Image<Gray, Int16> leftDisparityMap;
         Computer3DPointsFromImages(_left.Convert<Gray, Byte>(), right.Convert<Gray, Byte>(), out leftDisparityMap, out _points);

         /*
         #region osg terrain
         OsgTerrain.TerrainNode terrainNode = new TerrainNode();
         OsgTerrain.Locator locator = new OsgTerrain.EllipsoidLocator(-Math.PI, -Math.PI * 0.5, 2.0 * Math.PI, Math.PI, 0.0);
         OsgTerrain.ValidDataOperator validDataOperator = new ValidDataOperator();

         Osg.Image depthMap = ConvertImage(leftDisparityMap.Min(1000).Max(-1000).Convert<Bgr, Byte>());
         OsgTerrain.ImageLayer depthLayer= new ImageLayer();
         depthLayer.setImage(depthMap);
         depthLayer.setLocator(locator);
         depthLayer.setValidDataOperator(validDataOperator);
         terrainNode.setElevationLayer(depthLayer);

         Osg.Image textureMap = ConvertImage(_left);
         OsgTerrain.ImageLayer textureLayer = new ImageLayer();
         textureLayer.setImage(textureMap);
         textureLayer.setLocator(locator);
         depthLayer.setValidDataOperator(validDataOperator);
         terrainNode.setColorLayer(0, textureLayer);

         OsgTerrain.GeometryTechnique geometryTechnique = new GeometryTechnique();
         terrainNode.setTerrainTechnique(geometryTechnique);
         #endregion
         */
           
         //remove some depth outliers
         for (int i = 0; i < _points.Length; i++)
         {
            if (Math.Abs(_points[i].z) >= 1000) _points[i].z = 0;
         }

         //Display the disparity map
         //imageBox1.Image = leftDisparityMap;

         
         Osg.Geode geode = new Osg.Geode();
         Osg.Geometry geometry = new Osg.Geometry();

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

         #region set the drawing color
         Osg.Vec4Array colors = new Osg.Vec4Array();
         colors.Add(new Osg.Vec4(1.0f, 1.0f, 1.0f, 1.0f));
         geometry.setColorArray(colors);
         geometry.setColorBinding(Osg.Geometry.AttributeBinding.BIND_OVERALL);
         #endregion

         #region setup the texture coordinates
         Osg.Vec2Array textureCoor = new Osg.Vec2Array();
         foreach (MCvPoint3D32f p in _points)
            textureCoor.Add(new Osg.Vec2( p.x / _left.Width + 0.5f , p.y / _left.Height + 0.5f));
         geometry.setTexCoordArray(0, textureCoor);
         #endregion

         #region create and setup the texture
         // Create a new StateSet with default settings
         Osg.StateSet state = new Osg.StateSet();
         Osg.Image textureImage = ConvertImage(_left.Resize(textureWidth, textureHeight));
         Osg.Texture2D texture = new Osg.Texture2D(textureImage);
         state.setTextureAttributeAndModes(0, texture, 1);
         geode.setStateSet(state);
         #endregion

         geode.addDrawable(geometry);
         
         _viewer.setSceneData(geode);
         //_viewer.setSceneData(terrainNode);
         _viewer.realize();
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
      private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
      {
         if (_viewer != null && !_viewer.done())
         {
            _viewer.updateTraversal();
            _viewer.frame();
         }
         simpleOsgControl.Invalidate();
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            simpleOsgControl.Dispose();
            components.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
