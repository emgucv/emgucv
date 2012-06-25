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
      private Image<Bgr, Byte> _texture;
      private static double _angle = 0.0;
      private static double _angleIncrement = 0.2;
      private MCvPoint3D32f[] _points;
      private int[] _textures;

      public Simple3DReconstruction()
      {
         InitializeComponent();
         _left = new Image<Bgr, byte>("imL.png");
         _right = new Image<Bgr, byte>("imR.png");
         Image<Gray, short> disparityMap;

         Stopwatch watch = Stopwatch.StartNew();
         Computer3DPointsFromStereoPair(_left.Convert<Gray, Byte>(), _right.Convert<Gray, Byte>(), out disparityMap, out _points);
         watch.Stop();
         long disparityComputationTime = watch.ElapsedMilliseconds;

         //Display the disparity map
         imageBox1.Image = disparityMap;

         watch.Reset(); watch.Start();
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
                  {0.0, -1.0, 0.0, size.Height/2}, //shift the y origin to image center and flip it upside down
                  {0.0, 0.0, -1.0, 0.0}, //Multiply the z value by -1.0, 
                  {0.0, 0.0, 0.0, scale}})) //scale the object's corrdinate to within a [-0.5, 0.5] cube
               points = PointCollection.ReprojectImageTo3D(disparityMap, q);
         }
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

            View3DGlControl.Dispose();
         }
         base.Dispose(disposing);
      }

      private void View3DGlControl_Load(object sender, EventArgs e)
      {
         _glLoaded = true;

         GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
         SetupViewport();

         #region Create texture for the 3D Point clouds
         int repeat = (int)OpenTK.Graphics.OpenGL.All.Repeat;
         int linear = (int)OpenTK.Graphics.OpenGL.All.Linear;
         GL.Enable(EnableCap.CullFace);
         GL.Enable(EnableCap.Texture2D);
         _textures = new int[1];
         GL.GenTextures(1, _textures);
         GL.BindTexture(TextureTarget.Texture2D, _textures[0]);
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
            _texture = squareImg.Resize(256, 256, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true);
               _texture._Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);
               GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, _texture.Width, _texture.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, _texture.MIplImage.imageData);
         }
         #endregion
         
      }

      private void View3DGlControl_Paint(object sender, PaintEventArgs e)
      {
         if (!_glLoaded)
            return;

         GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         GL.Enable(EnableCap.Texture2D);
         GL.PushMatrix();
         GL.Rotate(10.0, 0.0, 1.0, 0.0);
         GL.Rotate(_angle, 0.0, 1.0, 0.0);
         GL.Begin(BeginMode.Points);
         GL.Color4(1.0, 1.0, 1.0, 1.0);
         foreach (MCvPoint3D32f p in _points)
         {
            GL.TexCoord2(p.x + 0.5f, p.y + 0.5f);
            GL.Vertex3(p.x, p.y, p.z);
         }
         GL.End();
         GL.PopMatrix();
         if (_angle >= 30.0 || _angle <= -30.0)
            _angleIncrement = -_angleIncrement;
         _angle += _angleIncrement;
         GraphicsContext.CurrentContext.SwapBuffers();
         View3DGlControl.Invalidate();
      }

      private void SetupViewport()
      {
         int w = View3DGlControl.Width;
         int h = View3DGlControl.Height;
         GL.MatrixMode(MatrixMode.Projection);
         GL.LoadIdentity();
         GL.Ortho(-1.5, 1.5, -1.5, 1.5, -1.5, 1.5);
         GL.Viewport(0, 0, w, h);
      }

      private void View3DGlControl_Resize(object sender, EventArgs e)
      {
         if (!_glLoaded)
            return;
         SetupViewport();
      }
   }
}
