//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Simlpe3DReconstruction
{
   public partial class Simple3DReconstruction : Form
   {
      bool _glLoaded = false;
      private Image<Bgr, Byte> _left;
      private Image<Bgr, byte> _right;
      private static double _angle = 0.0;
      private static double _angleIncrement = 0.2;
      private MCvPoint3D32f[] _points;

      public Simple3DReconstruction()
      {
         InitializeComponent();
         _left = new Image<Bgr, byte>("left.jpg");
         _right = new Image<Bgr, byte>("right.jpg");
         Image<Gray, short> disparityMap;

         Stopwatch watch = Stopwatch.StartNew();
         Computer3DPointsFromStereoPair(_left.Convert<Gray, Byte>(), _right.Convert<Gray, Byte>(), out disparityMap, out _points);
         watch.Stop();
         long disparityComputationTime = watch.ElapsedMilliseconds;

         //Display the disparity map
         imageBox1.Image = disparityMap;

         watch.Reset(); watch.Start();


         /*
          Osg.Geode geode = new Osg.Geode();
          Osg.Geometry geometry = new Osg.Geometry();

          int textureSize = 256;
          //create and setup the texture
          SetTexture(left.Resize(textureSize, textureSize, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC), geode);

          #region setup the vertices, the primitive and the texture
          Osg.Vec3Array vertices = new Osg.Vec3Array();
          Osg.DrawElementsUInt draw = new Osg.DrawElementsUInt(
             (uint)Osg.PrimitiveSet.Mode.POINTS, 0);
          Osg.Vec2Array textureCoor = new Osg.Vec2Array();
          uint verticesCount = 0;
          foreach (MCvPoint3D32f p in _points)
          {
             //skip the depth outliers
             if (Math.Abs(p.z) < 1000)
             {
                vertices.Add(new Osg.Vec3(-p.x, p.y, p.z));
                draw.Add(verticesCount);
                textureCoor.Add(new Osg.Vec2(p.x / left.Width + 0.5f, p.y / left.Height + 0.5f));
                verticesCount++;
             }
          }
          geometry.setVertexArray(vertices);
          geometry.addPrimitiveSet(draw);
          geometry.setTexCoordArray(0, textureCoor);
          #endregion

          geode.addDrawable(geometry);

          #region apply the rotation on the scene
          Osg.MatrixTransform transform = new Osg.MatrixTransform(
             Osg.Matrix.rotate(90.0 / 180.0 * Math.PI, new Osg.Vec3d(1.0, 0.0, 0.0)) *
             Osg.Matrix.rotate(180.0 / 180.0 * Math.PI, new Osg.Vec3d(0.0, 1.0, 0.0)));
          transform.addChild(geode);
          #endregion         

          watch.Stop();

          Text = String.Format("Disparity and 3D points computed in {0} millseconds. 3D model created in {1} milliseconds", 
             disparityComputationTime, watch.ElapsedMilliseconds);

          viewer3D.Viewer.setSceneData(transform);
          viewer3D.Viewer.realize();*/
      }

      /// <summary>
      /// Given the left and right image, computer the disparity map and the 3D point cloud.
      /// </summary>
      /// <param name="left">The left image</param>
      /// <param name="right">The right image</param>
      /// <param name="disparityMap">The left disparity map</param>
      /// <param name="points">The 3D point cloud within a [-0.5, 0.5] cube</param>
      private static void Computer3DPointsFromStereoPair(Image<Gray, Byte> left, Image<Gray, Byte> right, out Image<Gray, short> disparityMap, out MCvPoint3D32f[] points)
      {
         Size size = left.Size;

         disparityMap = new Image<Gray, short>(size);
         //using (StereoSGBM stereoSolver = new StereoSGBM(5, 64, 0, 0, 0, 0, 0, 0, 0, 0, false))
         using (StereoBM stereoSolver = new StereoBM(Emgu.CV.CvEnum.STEREO_BM_TYPE.BASIC, 0))
         {
            stereoSolver.FindStereoCorrespondence(left, right, disparityMap);

            float scale = Math.Max(size.Width, size.Height);

            //Construct a simple Q matrix, if you have a matrix from cvStereoRectify, you should use that instead
            using (Matrix<double> q = new Matrix<double>(
               new double[,] {
                  {1.0, 0.0, 0.0, -size.Width/2}, //shift the x origin to image center
                  {0.0, 1.0, 0.0, -size.Height/2}, //shift the y origin to image center
                  {0.0, 0.0, 1.0, 0.0}, //Multiply the z value by 1.0, 
                  {0.0, 0.0, 0.0, scale}}))
               points = PointCollection.ReprojectImageTo3D(disparityMap, q);
         }
      }

      /*
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
      }*/

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

            //viewer3D.Dispose();
         }
         base.Dispose(disposing);
      }

      private void View3DGlControl_Load(object sender, EventArgs e)
      {
         _glLoaded = true;

         GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
         GL.MatrixMode(MatrixMode.Projection);
         GL.LoadIdentity();
         GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);

         #region Create texture for the 3D Point clouds
         int repeat = (int)OpenTK.Graphics.OpenGL.All.Repeat;
         int linear = (int)OpenTK.Graphics.OpenGL.All.Linear;
         GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ref repeat);
         GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ref repeat);
         GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref linear);
         GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref linear);
         GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)OpenTK.Graphics.OpenGL.All.Decal);
         GL.ShadeModel(ShadingModel.Smooth);
         GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
         GL.BindTexture(TextureTarget.Texture2D, 1);

         Size size = _left.Size;
         int maxDim = Math.Max(size.Width, size.Height);
         using (Image<Bgr, Byte> squareImg = new Image<Bgr, byte>(maxDim, maxDim))
         {
            Rectangle roi = new Rectangle(maxDim / 2 - size.Width / 2, maxDim / 2 - size.Height / 2, size.Width, size.Height);
            squareImg.ROI = roi;
            CvInvoke.cvCopy(_left, squareImg, IntPtr.Zero);
            squareImg.ROI = Rectangle.Empty;
            using (Image<Bgr, Byte> texture = squareImg.Resize(256, 256, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true))
            {
               texture._Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);
               GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, texture.Width, texture.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, texture.MIplImage.imageData);
               //Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, texture.Width, texture.Height, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, texture.MIplImage.imageData);
            }
         }
         /*
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
         }*/
         #endregion
      }

      private void View3DGlControl_Paint(object sender, PaintEventArgs e)
      {
         if (!_glLoaded)
            return;

         GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         //GL.Enable(EnableCap.Texture2D);
         GL.PushMatrix();
         GL.Rotate(10.0, 0.0, 1.0, 0.0);
         GL.Rotate(_angle, 0.0, 1.0, 0.0);
         GL.Begin(BeginMode.Points);
         GL.Color4(1.0, 1.0, 1.0, 1.0);
         foreach (MCvPoint3D32f p in _points)
         {
           // GL.TexCoord2(p.x + 0.5f, p.y + 0.5f);
            GL.Vertex3(p.x, -p.y, p.z);
            
         }
         GL.End();
         GL.PopMatrix();
         if (_angle >= 30.0 || _angle <= -30.0)
            _angleIncrement = -_angleIncrement;
         _angle += _angleIncrement;
         GraphicsContext.CurrentContext.SwapBuffers();
         View3DGlControl.Invalidate();
         /*
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
         simpleOpenGlControl1.Invalidate();*/
      }
   }
}
