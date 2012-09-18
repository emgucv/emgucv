using System;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;

using Android.Views;
using Android.Util;
using Android.Content;

using Emgu.CV.Structure;

namespace Emgu.CV
{
   public class GLImageView : AndroidGameView
   {
      float prevx, prevy;
      float xangle, yangle;
      int[] textureIds;
      //int cur_texture;
      int _viewPortWidth, _viewPortHeight;
      Context context;

      private System.Drawing.Size _imageSize = System.Drawing.Size.Empty;

      public GLImageView(Context context)
         : base(context)
      {
         Initialize();
      }

      public GLImageView(Context context, IAttributeSet attrs) :
         base(context, attrs)
      {
         Initialize();
      }

      public GLImageView(IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
         : base(handle, transfer)
      {
         Initialize();
      }

      private void Initialize()
      {
         textureIds = new int[1];
         context = Context;
         //xangle = 45;
         //yangle = 45;

         Resize += delegate
         {
            //_viewPortHeight = Height;
            //_viewPortWidth = Width;
            SetupCamera();
            RenderCube();
         };
      }

      // This method is called everytime the context needs
      // to be recreated. Use it to set any egl-specific settings
      // prior to context creation
      protected override void CreateFrameBuffer()
      {
         ContextRenderingApi = GLVersion.ES1;

         // the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
         try
         {
            Report.Debug(context.PackageName, "Loading with GL Image View with default settings");

            // if you don't call this, the context won't be created
            base.CreateFrameBuffer();
            return;
         }
         catch (Exception ex)
         {
            Report.Error(context.PackageName , ex.Message);
         }

         // Fallback modes
         // If the first attempt at initializing the surface with a default graphics
         // mode fails, then the app can try different configurations. Devices will
         // support different modes, and what is valid for one might not be valid for
         // another. If all options fail, you can set all values to 0, which will
         // ask for the first available configuration the device has without any
         // filtering.
         // After a successful call to base.CreateFrameBuffer(), the GraphicsMode
         // object will have its values filled with the actual values that the
         // device returned.


         // This is a setting that asks for any available 16-bit color mode with no
         // other filters. It passes 0 to the buffers parameter, which is an invalid
         // setting in the default OpenTK implementation but is valid in some
         // Android implementations, so the AndroidGraphicsMode object allows it.
         try
         {
            Report.Debug(context.PackageName, "Loading with custom Android settings (low mode)");
            GraphicsMode = new AndroidGraphicsMode(16, 0, 0, 0, 0, false);

            // if you don't call this, the context won't be created
            base.CreateFrameBuffer();
            return;
         }
         catch (Exception ex)
         {
            Report.Error(context.PackageName, ex.Message);
         }

         // this is a setting that doesn't specify any color values. Certain devices
         // return invalid graphics modes when any color level is requested, and in
         // those cases, the only way to get a valid mode is to not specify anything,
         // even requesting a default value of 0 would return an invalid mode.
         try
         {
            Report.Debug(context.PackageName, "Loading with no Android settings");
            GraphicsMode = new AndroidGraphicsMode(0, 4, 0, 0, 0, false);

            // if you don't call this, the context won't be created
            base.CreateFrameBuffer();
            return;
         }
         catch (Exception ex)
         {
            Report.Error(context.PackageName, ex.Message);
         }
         throw new Exception("Can't load egl, aborting");
      }

      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);

         GL.ShadeModel(All.Smooth);
         GL.ClearColor(0, 0, 0, 1);

         GL.ClearDepth(1.0f);
         GL.Enable(All.DepthTest);
         GL.DepthFunc(All.Lequal);

         GL.Enable(All.CullFace);
         GL.CullFace(All.Back);

         GL.Hint(All.PerspectiveCorrectionHint, All.Nicest);

         // create texture ids
         GL.Enable(All.Texture2D);
         GL.GenTextures(1, textureIds);

         using (Image<Bgra, byte> image = new Image<Bgra, byte>(4, 4, new Bgra(0, 0, 255, 255)))
         {
            LoadTextureBGRA(image.Size, image.MIplImage.imageData);
         }
         SetupCamera();
         RenderCube();
      }

      public void SetupCamera()
      {
         _viewPortWidth = Width;
         _viewPortHeight = Height;

         GL.Viewport(0, 0, _viewPortWidth, _viewPortHeight);
         
         // setup projection matrix
         GL.MatrixMode(All.Projection);
         GL.LoadIdentity();
         
         // setup the projection matrix to compensate for the aspect ratio of the imageSize;
         if (_imageSize.IsEmpty || _viewPortHeight == 0)
            GL.Ortho(-1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f);
         else
         {
            double viewPortWidthHeight = (double)_viewPortWidth / (double)_viewPortHeight;
            double imageWidthHeight = (double)_imageSize.Width / (double)_imageSize.Height;

            if (viewPortWidthHeight > imageWidthHeight)
            {
               //view port is too wide
               float compensation = (float)((viewPortWidthHeight - imageWidthHeight) / 2);
               GL.Ortho(-1.0f - compensation, 1.0f + compensation, -1.0f, 1.0f, -1.0f, 1.0f);
            }
            else
            {
               float compensation = (float)((1.0 / viewPortWidthHeight) - (1.0 / imageWidthHeight)) / 2.0f;
               //viewport is too tall
               GL.Ortho(-1.0f, 1.0f, -1.0f - compensation, 1.0f + compensation, -1.0f, 1.0f);
            }
         }
      }

      /*
      public override bool OnTouchEvent(MotionEvent e)
      {
         base.OnTouchEvent(e);
         if (e.Action == MotionEventActions.Down)
         {
            prevx = e.GetX();
            prevy = e.GetY();
         }
         if (e.Action == MotionEventActions.Move)
         {
            float e_x = e.GetX();
            float e_y = e.GetY();

            float xdiff = (prevx - e_x);
            float ydiff = (prevy - e_y);
            xangle = xangle + ydiff;
            yangle = yangle + xdiff;
            prevx = e_x;
            prevy = e_y;
         }
         if (e.Action == MotionEventActions.Down || e.Action == MotionEventActions.Move)
            RenderCube();
         return true;
      }*/

      protected override void OnUnload(EventArgs e)
      {
         base.OnLoad(e);
         GL.DeleteTextures(1, textureIds);
      }

      public void RenderCube()
      {
         GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         GL.MatrixMode(All.Modelview);
         GL.LoadIdentity();

         // draw cube
         GL.Rotate(-xangle, 1, 0, 0);
         GL.Rotate(-yangle, 0, 1, 0);

         GL.BindTexture(All.Texture2D, textureIds[0]);
         GL.EnableClientState(All.VertexArray);
         GL.EnableClientState(All.TextureCoordArray);

         GL.VertexPointer(3, All.Float, 0, squareVertexCoords);
         GL.TexCoordPointer(2, All.Float, 0, squareTextureCoords);
         GL.DrawArrays(All.TriangleFan, 0, 4);

         GL.DisableClientState(All.VertexArray);
         GL.DisableClientState(All.TextureCoordArray);

         SwapBuffers();
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         GL.DeleteTextures(1, textureIds);
      }

      public static float ToRadians(float degrees)
      {
         //pi/180
         //FIXME: precalc pi/180
         return (float)(degrees * (System.Math.PI / 180.0));
      }


      public void LoadTextureBGRA(System.Drawing.Size imageSize, IntPtr data)
      {
         All error = GL.GetError();
         if (error != All.NoError)
         {
            Report.Error(context.PackageName, String.Format("Error before attemps to bing Texture: {0}", error));
         }

         GL.BindTexture(All.Texture2D, textureIds[0]);

         error = GL.GetError();
         if (error != All.NoError)
         {
            Report.Error(context.PackageName, String.Format("Failed to bing Texture: {0}", error));
         }

         // setup texture parameters
         GL.TexParameterx(All.Texture2D, All.TextureMagFilter, (int)All.Linear);
         GL.TexParameterx(All.Texture2D, All.TextureMinFilter, (int)All.Linear);
         GL.TexParameterx(All.Texture2D, All.TextureWrapS, (int)All.ClampToEdge);
         GL.TexParameterx(All.Texture2D, All.TextureWrapT, (int)All.ClampToEdge);
         error = GL.GetError();
         if (error != All.NoError)
         {
            Report.Error(context.PackageName, String.Format("Failed to set Texture Parameter: {0}", error));
         }

         if (!_imageSize.IsEmpty && _imageSize.Equals(imageSize))
         {
            GL.TexSubImage2D(All.Texture2D, 0, 0, 0, _imageSize.Width, _imageSize.Height, All.BgraExt, All.UnsignedByte, data);
         }
         else
         {
            _imageSize = imageSize;
            GL.TexImage2D(All.Texture2D, 0, (int)All.BgraExt, _imageSize.Width, _imageSize.Height, 0, All.BgraExt, All.UnsignedByte, data);
         }
         error = GL.GetError();
         if (error != All.NoError)
         {
            Report.Error(context.PackageName, String.Format("Failed to load Texture: {0}", error));
         }
      }

      static float[] squareVertexCoords =
         new float[] { // front
				 1, 1, 0,
				-1, 1, 0,
				-1,-1, 0,
				 1,-1, 0
		};

      static float[] squareTextureCoords =
         new float[] { // front
				1, 0,
				0, 0,
				0, 1,
				1, 1
			};

   }
}
