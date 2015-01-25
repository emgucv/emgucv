using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using System.Text;
//using System.Threading.Tasks;
using ES20 = OpenTK.Graphics.ES20;
using ES11 = OpenTK.Graphics.ES11;


#if IOS
using OpenTK.Platform.iPhoneOS;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
#elif ANDROID
using OpenTK.Platform.Android;
using Android.Views;
using Android.Util;
using Android.Content;
#else

#endif
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV.UI.GLView
{
#if IOS
   [Register("GLImageView")]
   #endif
   public class GLImageView 
#if IOS
      : iPhoneOSGameView
#elif ANDROID
      : AndroidGameView
#else
      : GLControl
#endif
   {

      private const int _glTextureDimension = 512;

      public enum OpenGLVersion
      {
         ES1,
         ES2, 
         GL3
      }

#if ANDROID || IOS
      private OpenGLVersion _glVersion = OpenGLVersion.ES1;
#else
      private OpenGLVersion _glVersion = OpenGLVersion.GL3;
#endif
      public OpenGLVersion GlVersion
      {
         get
         {
            return _glVersion;
         }
      }

      //int _viewPortWidth, _viewPortHeight;
      private String PackageName;
      private System.Drawing.Size _imageSize;

      int[] _textureIds;
      private System.Drawing.Size[] _textureSizes;
      private TextureColor[] _textureColor;
      private bool[] _textureEnabled;
     
      /// <summary>
      /// Individual texture rotations
      /// </summary>
      private float[] _textureRotations;
      private Emgu.CV.CvEnum.FlipType[] _textureFlipMode;
      private bool[] _textureRequiresReset;

      private ImageBufferFactory<Image<Bgr, Byte>> _bgrBuffers;
      private String _frameFileName;

      private int _gridLines;
      private float[] _verticalGridLines;
      private float[] _horizontalGridLines;

      #region GL_20
      private Matrix4 _projectionMatrix;
      private int _textureRenderRGBAProgramHandle;
      private int _textureRenderBGRAProgramHandle;
      private int _lineRenderProgramHandle;
      #endregion

      private static readonly float _degreeToRadian = (float)(Math.PI / 180.0);
      private RectangleF[] _rectangles;

      private const float _expectedLeft = -1.0f;
      private const float _expectedRight = 1.0f;
      private const float _expectedBottom = -1.0f;
      private const float _expectedTop = 1.0f;

      private float _left = _expectedLeft;
      private float _right = _expectedRight;
      private float _top = _expectedTop;
      private float _bottom = _expectedBottom;
      private float _zNear = 0.01f;
      private float _zFar = 1.0f;

      static float Min = -1;
      static float Max = 1;

      static float RectanglesVertexDepth = 0.1f;
      static float GridVertexDepth = 0.2f;
      static float FrameVertexDepth = 0.3f;
      static float ImageVertexDepth = 0.4f;

      static float[][] _textureVertexCoords = new float[][] 
      {
         new float[] { 
             Max, Max, ImageVertexDepth,
             Min, Max, ImageVertexDepth,
             Min, Min, ImageVertexDepth,
             Max, Min, ImageVertexDepth
         },
         new float[] { 
             Max, Max, FrameVertexDepth,
             Min, Max, FrameVertexDepth,
             Min, Min, FrameVertexDepth,
             Max, Min, FrameVertexDepth
         }
      };

      static float[] squareTextureCoords =
         new float[] { 
            1, 0,
            0, 0,
            0, 1,
            1, 1
         };

      static float[] squareTextureCoordsFlipHorizontal =
         new float[] {
            0, 0, 
            1, 0,
            1, 1, 
            0, 1
         };

      static float[] squareTextureCoordsFlipVetical =
         new float[] {
            1, 1,
            0, 1, 
            0, 0,
            1, 0
         };

      private float[] MergeArray(float[] lines, float[] newLines)
      {
         if (lines == null)
            return newLines;
         
         if (newLines == null)
            return lines;

         int oldLength = lines.Length;
         Array.Resize(ref lines, lines.Length + newLines.Length);
         Array.Copy(newLines, 0, lines, oldLength, newLines.Length);
         return lines;
      }

      public void GetGridLines(out float[] lines, out float[] colors)
      {
         float[] color = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };

         lines = MergeArray(null, _verticalGridLines);
         lines = MergeArray(lines, _horizontalGridLines);

         colors = null;
         if (lines == null)
         {
            return;
         }

         //every 3 float represent one point, each point need 4 values for color;
         colors = new float[4 * (lines.Length / 3)];
         for (int i = 0, idx = 0; i < lines.Length / 3; i++)
         {
            colors[idx++] = color[0];
            colors[idx++] = color[1];
            colors[idx++] = color[2];
            colors[idx++] = color[3];
         }
      }

      public void GetRectangleLines(out float[] lines, out float[] colors)
      {
         float[] color = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };

         colors = null;
         lines = null;
         if (_rectangles == null)
            return;

         int pointsCount = _rectangles.Length * 8;

         lines = new float[pointsCount * 3];
         int idx = 0;
         foreach (RectangleF rect in _rectangles)
         {
            lines[idx++] = rect.Location.X;
            lines[idx++] = rect.Location.Y;
            lines[idx++] = RectanglesVertexDepth;
            lines[idx++] = rect.Location.X + rect.Width;
            lines[idx++] = rect.Location.Y;
            lines[idx++] = RectanglesVertexDepth;

            lines[idx++] = rect.Location.X + rect.Width;
            lines[idx++] = rect.Location.Y;
            lines[idx++] = RectanglesVertexDepth;
            lines[idx++] = rect.Location.X + rect.Width;
            lines[idx++] = rect.Location.Y + rect.Height;
            lines[idx++] = RectanglesVertexDepth;

            lines[idx++] = rect.Location.X + rect.Width;
            lines[idx++] = rect.Location.Y + rect.Height;
            lines[idx++] = RectanglesVertexDepth;
            lines[idx++] = rect.Location.X;
            lines[idx++] = rect.Location.Y + rect.Height;
            lines[idx++] = RectanglesVertexDepth;

            lines[idx++] = rect.Location.X;
            lines[idx++] = rect.Location.Y + rect.Height;
            lines[idx++] = RectanglesVertexDepth;
            lines[idx++] = rect.Location.X;
            lines[idx++] = rect.Location.Y;
            lines[idx++] = RectanglesVertexDepth;
         }

         idx = 0;
         //change the X and Y coordinate such that they range from [-1.0, 1.0] instead of [0.0, 1.0];
         for (int i = 0; i < pointsCount; i++)
         {
            lines[idx] = lines[idx] * 2.0f -1.0f;
            idx++;
            lines[idx] = lines[idx] * 2.0f -1.0f;
            idx += 2;
         }

         colors = new float[4 * pointsCount];
         idx = 0;
         for (int i = 0; i < pointsCount; i++)
         {
            colors[idx++] = color[0];
            colors[idx++] = color[1];
            colors[idx++] = color[2];
            colors[idx++] = color[3];
         }
      }

      public int GridLines
      {
         get
         {
            return _gridLines;
         }
         set
         {
            _gridLines = value;
            if (_gridLines <= 0)
            {
               _verticalGridLines = null;
               _horizontalGridLines = null;
               _gridLines = 0;
            }
            else
            {
               _verticalGridLines = new float[GridLines * 6];
               _horizontalGridLines = new float[GridLines * 6];
               int idx = 0;

               float increment = (Max - Min) / (GridLines + 1);
               float current = Min + increment;
               for (int i = 0; i < GridLines; ++i)
               {
                  _verticalGridLines[idx] = Min;
                  _horizontalGridLines[idx] = current;
                  idx++;
                  _verticalGridLines[idx] = current;
                  _horizontalGridLines[idx] = Min;
                  idx++;
                  _verticalGridLines[idx] = GridVertexDepth;
                  _horizontalGridLines[idx] = GridVertexDepth;
                  idx++;
                  _verticalGridLines[idx] = Max;
                  _horizontalGridLines[idx] = current;
                  idx++;
                  _verticalGridLines[idx] = current;
                  _horizontalGridLines[idx] = Max;
                  idx++;
                  _verticalGridLines[idx] = GridVertexDepth;
                  _horizontalGridLines[idx] = GridVertexDepth;
                  idx++;
                  current += increment;
               }
            }
         }
      }

      /// <summary>
      /// The rectangles to be drawed. (0,0) point should be the image texture origin where (1,1) should match the opposite corner of the image.
      /// </summary>
      public RectangleF[] Rectangles
      {
         get
         {
            return _rectangles;
         }
         set
         {
            _rectangles = value;
         }
      }

      public enum TouchType
      {
         Down,
         Fling, 
         Scroll,
         SingleTapUp
      }

      private PointF ViewportPointToWorldPoint(PointF viewportPoint)
      {
         return new PointF(
            (viewportPoint.X / ViewPortWidth) * (_right - _left) + _left,
            ( (ViewPortHeight - viewportPoint.Y) / ViewPortHeight) * (_top - _bottom) + _bottom);
      }

      private PointF WorldPointToImagePoint(PointF worldPoint)
      {
         return new PointF((worldPoint.X + 1.0f) / 2.0f, (worldPoint.Y + 1.0f) / 2.0f); 
      }

      private static float Dist(PointF p1, PointF p2)
      {
         float dx = p1.X - p2.X;
         float dy = p1.Y - p2.Y;
         return (float) Math.Sqrt(dx * dx + dy * dy);
      }

      private static PointF RestrictRange(PointF point, float minX, float maxX, float minY, float maxY)
      {
         if (point.X < minX)
            point.X = minX;
         if (point.X > maxX)
            point.X = maxX;
         if (point.Y < minY)
            point.Y = minY;
         if (point.Y > maxY)
            point.Y = maxY;
         return point;
      }

      public System.Drawing.Rectangle GetRectangleInImageCoordinate(int rectangleIndex, System.Drawing.Size imageSize)
      {
         if (_rectangles == null || rectangleIndex >= _rectangles.Length)
            return System.Drawing.Rectangle.Empty;
         System.Drawing.RectangleF rect = _rectangles[rectangleIndex];
         System.Drawing.Rectangle roi = new System.Drawing.Rectangle(
               new System.Drawing.Point((int)(rect.Location.X * imageSize.Width), (int)((1.0 - rect.Location.Y) * imageSize.Height)),
               new System.Drawing.Size((int)(rect.Width * imageSize.Width), (int)(rect.Height * imageSize.Height)));
            roi.Location = new System.Drawing.Point(roi.Location.X, roi.Location.Y - roi.Height);
         return roi;
      }

      /// <summary>
      /// Update the rectangles location by a touch event
      /// </summary>
      /// <param name="type">The type of touch</param>
      /// <param name="viewportStartPoint">The start point of the touch, may be PointF.Empty if touch type is down</param>
      /// <param name="viewportEndPoint">The end point of the touch</param>
      /// <param name="rectangleIndex">The index of the rectangle when the shape is to be updated.</param>
      public void UpdateRectangleByTouch(TouchType type, PointF viewportStartPoint, PointF viewportEndPoint, int rectangleIndex)
      {
         if (_rectangles == null || rectangleIndex >= _rectangles.Length)
            return;

         //PointF startPoint = RestrictRange( WorldPointToImagePoint( ViewportPointToWorldPoint(viewportStartPoint)), 0.0f, 1.0f, 0.0f, 1.0f);
         PointF endPoint = RestrictRange( WorldPointToImagePoint( ViewportPointToWorldPoint(viewportEndPoint) ), 0.0f, 1.0f, 0.0f, 1.0f);
         
         RectangleF rect = _rectangles[rectangleIndex];

         float scale = 0.6f;
         RectangleF rectCenterRegion = new RectangleF(new PointF(rect.Location.X + (rect.Width - rect.Width * scale) / 2.0f, rect.Location.Y + (rect.Height -rect.Height * scale) / 2.0f), new SizeF(rect.Width * scale, rect.Height * scale));

         if (rectCenterRegion.Contains(endPoint))
         {  // move the center to the end point while keeping the width and height
            float newX = endPoint.X - rect.Width / 2.0f;
            if (newX <= 0)
               newX = 0;
            if (newX + rect.Width > 1.0f)
               newX = 1.0f - rect.Width;

            float newY = endPoint.Y - rect.Height / 2.0f;
            if (newY <= 0)
               newY = 0;
            if (newY + rect.Height > 1.0f)
               newY = 1.0f - rect.Height;
            rect.Location = new PointF(newX, newY);

            _rectangles[rectangleIndex] = rect;
            return;
         }

         PointF point0 = rect.Location;
         PointF point1 = new PointF(point0.X + rect.Width, point0.Y);
         PointF point2 = new PointF(point0.X + rect.Width, point0.Y + rect.Height);
         PointF point3 = new PointF(point0.X, point0.Y + rect.Height);

         float d0 = Dist(endPoint, point0);
         float d1 = Dist(endPoint, point1);
         float d2 = Dist(endPoint, point2);
         float d3 = Dist(endPoint, point3);

         float min = Math.Min(Math.Min(d0, d1), Math.Min(d2, d3));
         if (min == d0)
         {
            //PointF newLoc = endPoint;
            //if (newLoc.X < 0)
            //   newLoc.X = 0;
            //if (newLoc.X > 1.0f)
            //   newLoc.X = 1.0f;
            rect.Width = point2.X - endPoint.X;
            //rect.Location = endPoint;           

            //if (newLoc.Y < 0)
            //   newLoc.Y = 0;
            //if (newLoc.Y > 1.0f)
            //   newLoc.Y = 1.0f;
            rect.Height = point2.Y - endPoint.Y;
            rect.Location = endPoint;
         }
         else if (min == d1)
         {
            rect.Width = endPoint.X - point0.X;
            if (rect.Width <= 0)
               rect.Width = 1.0e-5f;


            rect.Height = point3.Y - endPoint.Y;
            if (rect.Height <= 0)
               rect.Height = 1.0e-5f;
            rect.Location = new PointF(rect.Location.X, endPoint.Y);

            //if (rect.Width + point0.X > 1.0f)
            //   rect.Width = 1.0f - point0.X;
         }
         else if (min == d2)
         {
            rect.Width = endPoint.X - point0.X;
            if (rect.Width <= 0)
               rect.Width = 1.0e-5f;
            //if (rect.Width + point0.X > 1.0f)
            //   rect.Width = 1.0f - point0.X;

            rect.Height = endPoint.Y - point0.Y;
            if (rect.Height <= 0)
               rect.Height = 1.0e-5f;
            //if (rect.Height + point0.Y > 1.0f)
            //   rect.Height = 1.0f - point0.Y;

         }
         else if (min == d3)
         {
            rect.Height = endPoint.Y - point0.Y;
            if (rect.Height <= 0)
               rect.Height = 1.0e-5f;

            rect.Width = point1.X - endPoint.X;
            if (rect.Width <= 0)
               rect.Width= 1.0e-5f;
            rect.Location = new PointF(endPoint.X, rect.Location.Y);
            //if (rect.Height + point0.Y > 1.0)
            //   rect.Height = 1.0f - point0.Y;
         }
         _rectangles[rectangleIndex] = rect;
      }

      public bool[] TextureEnabled
      {
         get { return _textureEnabled; }
      }

      /*
      public void SetTextureRotation(int textureIdx, int rotation)
      {
         float oldRotation = _textureRotation[textureIdx];
         if (((oldRotation == 0 || oldRotation == 180) && (rotation == 90 || rotation == 270))
            || ((oldRotation == 90 || oldRotation == 270) && (rotation == 0 || rotation == 180)))
         {
            Size s = _imageSize;
            _imageSize.Width = s.Height;
            _imageSize.Height = s.Width;
         }

         _textureRotation[textureIdx] = rotation;
      }*/

      public Emgu.CV.CvEnum.FlipType[] TextureFlipMode
      {
         get
         {
            return _textureFlipMode;
         }
      }

      /*
      public bool TextureRequiresReset
      {
         get { return _textureRequiresReset; }
         set { _textureRequiresReset = value; }
      }*/

#if IOS
      public GLImageView(RectangleF frame)
         : base(frame)
      {
         Initialize();
      }

      /*
      [Export("initWithCoder:")]
      public GLImageView(NSCoder coder)
         : base(coder)
      {
         Initialize();
      }*/

      [Export ("layerClass")]
      public static Class LayerClass()
      {
         return iPhoneOSGameView.GetLayerClass();
      }
#elif ANDROID
      public GLImageView(Context context, GLVersion glVersion)
         : base(context)
      {
         _glVersion = glVersion;
         Initialize();
      }

      public GLImageView(Context context, IAttributeSet attrs, GLVersion glVersion) :
         base(context, attrs)
      {
         _glVersion = glVersion;
         Initialize();
      }

      public GLImageView(IntPtr handle, Android.Runtime.JniHandleOwnership transfer, GLVersion glVersion)
         : base(handle, transfer)
      {
         _glVersion = glVersion;
         Initialize();
      }
#else
      public GLImageView()
         :base()
      {
         Initialize();
      }
#endif

#if IOS || ANDROID
      public override void Pause()
      {
         base.Pause();
         DeleteTextures();
      }

      public override void Resume()
      {
         base.Resume();
         GenTextures();
      }
#endif

      private void Initialize()
      {
#if IOS
         LayerRetainsBacking = true;
         LayerColorFormat = MonoTouch.OpenGLES.EAGLColorFormat.RGBA8;
         CreateFrameBuffer();
#elif ANDROID
         PackageName = Context.PackageName;
#else
#endif

         _textureSizes = new System.Drawing.Size[2];
         _textureColor = new TextureColor[2] {
            TextureColor.NotSupported,
            TextureColor.NotSupported
         };
         _textureEnabled = new bool[2];
         _textureRotations = new float[2];

         _textureFlipMode = new Emgu.CV.CvEnum.FlipType[2];
         for (int i = 0; i < _textureFlipMode.Length; i++)
         {
            _textureFlipMode[i] = Emgu.CV.CvEnum.FlipType.None;
         }

         _textureRequiresReset = new bool[2];
         for (int i = 0; i < _textureRequiresReset.Length; i++)
            _textureRequiresReset[i] = true;

         if (_glVersion == OpenGLVersion.ES1)
            _bgrBuffers = new ImageBufferFactory<Image<Bgr, byte>>(s => new Image<Bgr, byte>(s));
          
         _textureIds = new int[2];
         //context = Context;
         //xangle = 45;
         //yangle = 45;

#if IOS
         OnLoad(null);
#endif

         Resize += delegate
         {
            //_viewPortHeight = Height;
            //_viewPortWidth = Width;
            //SetupCamera();
            PerformRenderAll();
         };
         
      }

#if IOS || ANDROID
      // This method is called everytime the context needs
      // to be recreated. Use it to set any egl-specific settings
      // prior to context creation
      protected override void CreateFrameBuffer()
      {
#if IOS
         ContextRenderingApi = MonoTouch.OpenGLES.EAGLRenderingAPI.OpenGLES1;
#elif ANDROID
         ContextRenderingApi = _glVersion;
#endif
         // the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
         try
         {
            Report.Debug(PackageName, "Loading with GL Image View with default settings");
            //if (GraphicsContext == null)
               // if you don't call this, the context won't be created
               base.CreateFrameBuffer();

            return;
         } catch (Exception ex)
         {
            Report.Error(PackageName, ex.Message);
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
#if IOS
#elif ANDROID
            Report.Debug(PackageName, "Loading with custom Android settings (low mode)");
            GraphicsMode = new AndroidGraphicsMode(16, 0, 0, 0, 0, false);
#endif
            //if (GraphicsContext == null)
               // if you don't call this, the context won't be created
               base.CreateFrameBuffer();
            return;
         } catch (Exception ex)
         {
            Report.Error(PackageName, ex.Message);
         }

         // this is a setting that doesn't specify any color values. Certain devices
         // return invalid graphics modes when any color level is requested, and in
         // those cases, the only way to get a valid mode is to not specify anything,
         // even requesting a default value of 0 would return an invalid mode.
         try
         {
#if IOS
#elif ANDROID
            Report.Debug(PackageName, "Loading with no Android settings");
            GraphicsMode = new AndroidGraphicsMode(0, 4, 0, 0, 0, false);
#endif 
            //if (GraphicsContext == null)
               // if you don't call this, the context won't be created
               base.CreateFrameBuffer();
            return;
         } catch (Exception ex)
         {
            Report.Error(PackageName, ex.Message);
         }
         throw new Exception("Can't load egl, aborting");
      }
#endif

      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);

#if ANDROID || IOS
         if (_glVersion == OpenGLVersion.ES2)
         {
            ES20.All error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error before onLoad: {0}", error));
            }
            _textureRenderRGBAProgramHandle = GetRenderRGBATextureProgram();
            _textureRenderBGRAProgramHandle = GetRenderBGRATextureProgram();

            _lineRenderProgramHandle = GetRenderLineProgram();
            error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error during onLoad 0: {0}", error));
            }

            GenTextures();

            error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error during onLoad 2: {0}", error));
            }
            for (int i = 0; i < _textureRequiresReset.Length; i++)
               _textureRequiresReset[i] = true;

            if (_textureEnabled[1])
               LoadFrame();

            /*
            if (_textureEnabled[0])
            {
               Image<Bgr, Byte> img = _bgrBuffers.GetBuffer(0);
               if (img != null)
               {
                  LoadTexture(img.Size, img.MIplImage.imageData, TextureColor.RGB, 0);
               }
            }*/

            error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error during onLoad 3: {0}", error));
            }

            ES20.GL.BlendFunc(ES20.All.SrcAlpha, ES20.All.OneMinusSrcAlpha);
            ES20.GL.Enable(ES20.All.Blend);

            error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error during onLoad 4: {0}", error));
            }
         }
         else
         {
            ES11.All error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error before onLoad: {0}", error));
            }
            ES11.GL.ShadeModel(ES11.All.Smooth);
            ES11.GL.ClearColor(0, 0, 0, 1);

            ES11.GL.ClearDepth(1.0f);
            ES11.GL.Enable(ES11.All.DepthTest);
            ES11.GL.DepthFunc(ES11.All.Lequal);

            ES11.GL.Enable(ES11.All.CullFace);
            ES11.GL.CullFace(ES11.All.Back);

            error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error during onLoad 0: {0}", error));
            }

            ES11.GL.Hint(ES11.All.PerspectiveCorrectionHint, ES11.All.Nicest);
            // create texture ids
            ES11.GL.Enable(ES11.All.Texture2D);

            GenTextures();

            error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error during onLoad 2: {0}", error));
            }
            for (int i = 0; i < _textureRequiresReset.Length; i++)
               _textureRequiresReset[i] = true;

            if (_textureEnabled[1])
               LoadFrame();

            if (_textureEnabled[0])
            {
               Image<Bgr, Byte> img = _bgrBuffers.GetBuffer(0);
               if (img != null)
               {
                  LoadTexture(img.Size, img.MIplImage.imageData, TextureColor.RGB, 0);
               }
            }

            error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error during onLoad 3: {0}", error));
            }

            ES11.GL.BlendFunc(ES11.All.SrcAlpha, ES11.All.OneMinusSrcAlpha);
            ES11.GL.Enable(ES11.All.Blend);

            error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("GL Error during onLoad 4: {0}", error));
            }
         }
#else
         if (_glVersion == OpenGLVersion.GL3)
         {

            _textureRenderRGBAProgramHandle = GetRenderRGBATextureProgram();
            _textureRenderBGRAProgramHandle = GetRenderBGRATextureProgram();

            _lineRenderProgramHandle = GetRenderLineProgram();


            GenTextures();

          
            for (int i = 0; i < _textureRequiresReset.Length; i++)
               _textureRequiresReset[i] = true;

            if (_textureEnabled[1])
               LoadFrame();

            /*
            if (_textureEnabled[0])
            {
               Image<Bgr, Byte> img = _bgrBuffers.GetBuffer(0);
               if (img != null)
               {
                  LoadTexture(img.Size, img.MIplImage.imageData, TextureColor.RGB, 0);
               }
            }*/
            
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
         }

#endif
         PerformRenderAll();
      }

      private void PerformRenderAll()
      {
#if IOS
         PerformSelector(new Selector("RenderAll"), null, 0.2f);
#else
         RenderAll();
#endif
      }

      #region ES20
      int ES2LoadShader(ES20.All type, string source)
      {
         int shader = ES20.GL.CreateShader(type);
         if (shader == 0)
         {
            throw new InvalidOperationException(String.Format("Unable to create shader {0}", ES20.GL.GetError()));
         }

         int length = 0;
         ES20.GL.ShaderSource(shader, 1, new string[] { source }, (int[])null);
         ES20.GL.CompileShader(shader);

         int compiled = 0;
         ES20.GL.GetShader(shader, ES20.All.CompileStatus, out compiled);
         if (compiled == 0)
         {
            length = 0;
            ES20.GL.GetShader(shader, ES20.All.InfoLogLength, out length);
            if (length > 0)
            {
               var log = new StringBuilder(length);
               ES20.GL.GetShaderInfoLog(shader, length, out length, log);
               Report.Debug("GL2", "Couldn't compile shader: " + log.ToString());
            }

            ES20.GL.DeleteShader(shader);
            throw new InvalidOperationException("Unable to compile shader of type : " + type.ToString());
         }

         return shader;
      }
      #endregion

      int GLLoadShader(ShaderType type, string source)
      {
         int shader = GL.CreateShader(type);
         if (shader == 0)
         {
            throw new InvalidOperationException(String.Format("Unable to create shader {0}", GL.GetError()));
         }

         int length = 0;
         GL.ShaderSource(shader, source);
         GL.CompileShader(shader);

         int compiled = 0;
         GL.GetShader(shader,  ShaderParameter.CompileStatus, out compiled);
         if (compiled == 0)
         {
            length = 0;
            GL.GetShader(shader, ShaderParameter.InfoLogLength, out length);
            if (length > 0)
            {
               var log = new StringBuilder(length);
               GL.GetShaderInfoLog(shader, length, out length, log);
               Report.Debug("GL", "Couldn't compile shader: " + log.ToString());
            }

            GL.DeleteShader(shader);
            throw new InvalidOperationException("Unable to compile shader of type : " + type.ToString());
         }

         return shader;
      }

      private int ViewPortWidth
      {
         get
         {
#if IOS
            return Size.Width;
#else
            return Width;
#endif
         }
      }

      private int ViewPortHeight
      {
         get
         {
#if IOS
            return Size.Height;
#else
            return Height;
#endif
         }
      }

      /*
      public void SetupCamera()
      {


      }*/
      
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
            RenderTexture();
         return true;
      }*/

#if IOS || ANDROID
      protected override void OnUnload(EventArgs e)
      {
         base.OnUnload(e);
         DeleteTextures();
      }
#endif

      private void DeleteTextures()
      {
         for (int i = 0; i < _textureSizes.Length; i++)
         {
            _textureSizes[i] = Size.Empty;
            _textureRequiresReset[i] = true;
            _textureEnabled[i] = false;
         }
         if (_glVersion == OpenGLVersion.ES2)
         {
            ES20.GL.DeleteTextures(2, _textureIds);
         }
         else if (_glVersion == OpenGLVersion.ES1)
         {
            ES11.GL.DeleteTextures(2, _textureIds);
         } else
         {
            GL.DeleteTextures(2, _textureIds);
         }
      }

      private void GenTextures()
      {
         for (int i = 0; i < _textureSizes.Length; i++)
         {
            _textureSizes[i] = Size.Empty;
            _textureRequiresReset[i] = true;
            _textureEnabled[i] = false;
         }

         if (_glVersion == OpenGLVersion.ES2)
         {
            ES20.GL.GenTextures(_textureIds.Length, _textureIds);
         }
         else if (_glVersion == OpenGLVersion.ES1)
         {
            ES11.GL.GenTextures(_textureIds.Length, _textureIds);
         } else
         {
            GL.GenTextures(_textureIds.Length, _textureIds);
         }
      }


      private float _rotation;

      /// <summary>
      /// Get or set the rotation of the whole GLImageView in degree
      /// </summary>
      public float Rotation
      {
         get
         {
            return _rotation;
         }
         set
         {
            _rotation = value;
         }
      }

      private void ResetScreen()
      {
         // setup the projection matrix to compensate for the aspect ratio of the imageSize;
         float widthScale = 1.0f;
         float heightScale = 1.0f;
         int viewPortWidth = ViewPortWidth;
         int viewPortHeight = ViewPortHeight;

         if (!(_textureSizes[0].IsEmpty || viewPortHeight == 0))
         {
            double viewPortWidthHeight = (double)viewPortWidth / (double)viewPortHeight;
            double imageWidthHeight = (double)_imageSize.Width / (double)_imageSize.Height;
            if (viewPortWidthHeight > imageWidthHeight)
            {
               //view port is too wide
               widthScale = (float)(viewPortWidthHeight / imageWidthHeight);
               float compensation = (float)(viewPortWidthHeight - imageWidthHeight) / 2.0f;
               _left = _expectedLeft - compensation;
               _right = _expectedRight + compensation;
               _top = _expectedTop;
               _bottom = _expectedBottom;

            }
            else
            {
               heightScale = 1.0f / (float)(viewPortWidthHeight / imageWidthHeight);
               float compensation = (float)((1.0 / viewPortWidthHeight) - (1.0 / imageWidthHeight)) / 2.0f;
               //viewport is too tall
               _bottom = _expectedBottom - compensation;
               _top = _expectedTop + compensation;
               _left = _expectedLeft;
               _right = _expectedRight;
            }
         }
         else
         {
            _bottom = _expectedBottom;
            _top = _expectedTop;
            _left = _expectedLeft;
            _right = _expectedRight;
         }

#if ANDROID || IOS
         if (_glVersion == OpenGLVersion.ES2)
         {
            //_zNear = -10.0f;
            ES20.GL.Viewport(0, 0, viewPortWidth, viewPortHeight);
            _projectionMatrix = OpenTK.Matrix4.Scale(1.0f / widthScale, 1.0f / heightScale, 1.0f);
            //_projectionMatrix = OpenTK.Matrix4.CreateOrthographicOffCenter(_left, _right, _bottom, _top, _zNear, _zFar);
            //_projectionMatrix = OpenTK.Matrix4.Frustum(_left, _right, _bottom, _top, _zNear, _zFar);
            //_projectionMatrix = OpenTK.Matrix4.Frustum(-1, 1, -1, 1, 3, 7);

            ES20.All glError = ES20.GL.GetError();
            if (glError != ES20.All.NoError)
            {
               Report.Warn(PackageName, String.Format("Error after setup camera: {0}", glError));
            }
         }
         else
         {
            ES11.GL.Viewport(0, 0, viewPortWidth, viewPortHeight);

            ES11.All glError = ES11.GL.GetError();
            if (glError != ES11.All.NoError)
            {
               Report.Warn(PackageName, String.Format("Error after setup camera: {0}", glError));
            }
         }

         if (_glVersion == OpenGLVersion.ES2)
         {
            ES20.All glError = ES20.GL.GetError();
            if (glError != ES20.All.NoError)
            {
               Report.Warn(PackageName, String.Format("Error before clearing screen: {0}", glError));
            }

#if DEBUG
            ES20.GL.ClearColor(1.0f, 0.0f, 0.0f, 1);
#else
            ES20.GL.ClearColor(0.0f, 0.0f, 0.0f, 1);
#endif       
            ES20.GL.Clear(
#if IOS
            (int)
#endif
(ES20.ClearBufferMask.ColorBufferBit | ES20.ClearBufferMask.DepthBufferBit));
         }
         else //ES 1.1
         {
            ES11.All glError = ES11.GL.GetError();
            if (glError != ES11.All.NoError)
            {
               Report.Warn(PackageName, String.Format("Error before clearing screen: {0}", glError));
            }
#if DEBUG
            ES11.GL.ClearColor(1.0f, 0.0f, 0.0f, 1);
#else
            ES11.GL.ClearColor(0.0f, 0.0f, 0.0f, 1);
#endif
            ES11.GL.Clear(
#if IOS
            (int)
#endif
(ES11.ClearBufferMask.ColorBufferBit | ES11.ClearBufferMask.DepthBufferBit));
            
            ES11.GL.MatrixMode(ES11.All.Projection);
            ES11.GL.LoadIdentity();

            //ES11.GL.Ortho(_expectedLeft, _expectedRight, _expectedBottom, _expectedTop, _zNear, _zFar);
            //ES11.GL.Scale(1.0f / widthScale, 1.0f / heightScale, 1.0f);
            _zNear = -10.0f;
            ES11.GL.Ortho(_left, _right, _bottom, _top, _zNear, _zFar);
         }
#else
         if (_glVersion == OpenGLVersion.GL3)
         {
            //_zNear = -10.0f;
            GL.Viewport(0, 0, viewPortWidth, viewPortHeight);
            _projectionMatrix = OpenTK.Matrix4.Scale(1.0f / widthScale, 1.0f / heightScale, 1.0f);
            //_projectionMatrix = OpenTK.Matrix4.CreateOrthographicOffCenter(_left, _right, _bottom, _top, _zNear, _zFar);
            //_projectionMatrix = OpenTK.Matrix4.Frustum(_left, _right, _bottom, _top, _zNear, _zFar);
            //_projectionMatrix = OpenTK.Matrix4.Frustum(-1, 1, -1, 1, 3, 7);
            

#if DEBUG
            GL.ClearColor(1.0f, 0.0f, 0.0f, 1);
#else
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1);
#endif
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         }
#endif
      }

      private void RenderLines(float[] lines, float[] colors, float lineWidth)
      {
         if (lines == null)
            return;
#if (ANDROID || IOS)
         if (_glVersion == OpenGLVersion.ES2)
         {
            ES20.GL.UseProgram(_lineRenderProgramHandle);
            // get handle to vertex shader's vPosition member
            int linePositionHandle = ES20.GL.GetAttribLocation(_lineRenderProgramHandle, new StringBuilder("vPosition"));

            // Enable a handle to the triangle vertices
            ES20.GL.EnableVertexAttribArray(linePositionHandle);

            // Prepare the line data
            ES20.GL.VertexAttribPointer(linePositionHandle, 3, ES20.All.Float, false, 0, lines);

            // get handle to fragment shader's vColor member
            int colorHandle = ES20.GL.GetAttribLocation(_lineRenderProgramHandle, new StringBuilder("vColor"));
            ES20.GL.EnableVertexAttribArray(colorHandle);
            ES20.GL.VertexAttribPointer(colorHandle, 4, ES20.All.Float, false, 0, colors);

            // get handle to shape's transformation matrix
            int lineMatrixHandle = ES20.GL.GetUniformLocation(_lineRenderProgramHandle, new StringBuilder("uMVPMatrix"));

            // Apply the projection and view transformation
            Matrix4 projectionMatrix = Matrix4.Mult(_projectionMatrix, Matrix4.CreateRotationZ(_rotation * _degreeToRadian));
            
            ES20.GL.UniformMatrix4(lineMatrixHandle, false, ref projectionMatrix);
            

            ES20.GL.LineWidth(lineWidth);
            ES20.GL.DrawArrays(ES20.All.Lines, 0, lines.Length / 3);

            ES20.All glError = ES20.GL.GetError();
            if (glError != ES20.All.NoError)
            {
               Report.Warn(PackageName, String.Format("Error after drawing lines: {0}", glError));
            }

            ES20.GL.DisableVertexAttribArray(linePositionHandle);
            ES20.GL.DisableVertexAttribArray(colorHandle);
         }
         else //ES1.1
         {
            ES11.GL.Disable(ES11.All.Texture2D);
            ES11.GL.EnableClientState(ES11.All.VertexArray);
            ES11.GL.EnableClientState(ES11.All.ColorArray);

            ES11.GL.PushMatrix();
            ES11.GL.Rotate(_rotation, 0, 0, 1);

            //ES11.GL.LineWidth(lineWidth);
            ES11.GL.VertexPointer(3, ES11.All.Float, 0, lines);
            ES11.GL.ColorPointer(4, ES11.All.Float, 0, colors);
            ES11.GL.DrawArrays(ES11.All.Lines, 0, lines.Length / 3);

            ES11.GL.DisableClientState(ES11.All.VertexArray);
            ES11.GL.DisableClientState(ES11.All.ColorArray);
            //GL.Color4(0, 0, 0, 0);

            ES11.GL.PopMatrix();

            ES11.GL.Enable(ES11.All.Texture2D);

            ES11.All glError = ES11.GL.GetError();
            if (glError != ES11.All.NoError)
            {
               Report.Warn(PackageName, String.Format("Error after drawing lines: {0}", glError));
            }

         }
#else
         if (_glVersion == OpenGLVersion.GL3)
         {
            GL.UseProgram(_lineRenderProgramHandle);
            // get handle to vertex shader's vPosition member
            int linePositionHandle = GL.GetAttribLocation(_lineRenderProgramHandle, "vPosition");

            // Enable a handle to the triangle vertices
            GL.EnableVertexAttribArray(linePositionHandle);

            // Prepare the line data
            GL.VertexAttribPointer(linePositionHandle, 3, VertexAttribPointerType.Float, false, 0, lines);

            // get handle to fragment shader's vColor member
            int colorHandle = GL.GetAttribLocation(_lineRenderProgramHandle, "vColor");
            GL.EnableVertexAttribArray(colorHandle);
            GL.VertexAttribPointer(colorHandle, 4, VertexAttribPointerType.Float, false, 0, colors);

            // get handle to shape's transformation matrix
            int lineMatrixHandle = GL.GetUniformLocation(_lineRenderProgramHandle, "uMVPMatrix");

            // Apply the projection and view transformation
            Matrix4 projectionMatrix = Matrix4.Mult(_projectionMatrix, Matrix4.CreateRotationZ(_rotation * _degreeToRadian));

            GL.UniformMatrix4(lineMatrixHandle, false, ref projectionMatrix);


            GL.LineWidth(lineWidth);
            GL.DrawArrays(BeginMode.Lines, 0, lines.Length / 3);

            GL.DisableVertexAttribArray(linePositionHandle);
            GL.DisableVertexAttribArray(colorHandle);
         }
#endif

      }

      private void RenderTextures()
      {
#if ANDROID || IOS
         if (_glVersion == OpenGLVersion.ES2)
         {
            Matrix4 projectionMatrix = Matrix4.Mult(_projectionMatrix, Matrix4.CreateRotationZ(_rotation * _degreeToRadian));

            for (int i = 0; i <= 1; i++)
            {
               if (_textureEnabled[i] && ES20.GL.IsTexture(_textureIds[i]))
               {
                  if (_textureColor[i] == TextureColor.RGB || _textureColor[i] == TextureColor.RGBA)
                  {
                     ES20.GL.UseProgram(_textureRenderRGBAProgramHandle);
                  }
                  else if (_textureColor[i] == TextureColor.BGR || _textureColor[i] == TextureColor.BGRA)
                  {
                     ES20.GL.UseProgram(_textureRenderBGRAProgramHandle);
                  }
                  else
                  {
                     continue;
                  }
                  // get handle to vertex shader's vPosition member
                  int positionHandle = ES20.GL.GetAttribLocation(_textureRenderRGBAProgramHandle, new StringBuilder("vPosition"));
                  // Enable a handle to the rectangle vertices
                  ES20.GL.EnableVertexAttribArray(positionHandle);

                  int textureCoordHandle = ES20.GL.GetAttribLocation(_textureRenderRGBAProgramHandle, new StringBuilder("TexCoordIn"));
                  ES20.GL.EnableVertexAttribArray(textureCoordHandle);

                  // Prepare the rectangle coordinate data
                  ES20.GL.VertexAttribPointer(positionHandle, 3, ES20.All.Float, false, 0, _textureVertexCoords[i]);

                  // get handle to fragment shader's vColor member
                  //int colorHandle = GL.GetUniformLocation(_programHandle, new StringBuilder("vColor"));

                  // Set color with red, green, blue and alpha (opacity) values
                  //float[] color = new float[] { 0.63671875f, 0.76953125f, 0.22265625f, 1.0f };

                  // Set color for drawing the triangle
                  //GL.Uniform4(colorHandle, 1, color);

                  ES20.All glError = ES20.GL.GetError();
                  if (glError != ES20.All.NoError)
                  {
                     Report.Warn(PackageName, String.Format("Error after setting texture position: {0}", glError));
                  }

                  if (_textureFlipMode[i] == Emgu.CV.CvEnum.FLIP.None)
                  {
                     ES20.GL.VertexAttribPointer(textureCoordHandle, 2, ES20.All.Float, false, 0, squareTextureCoords);
                  }
                  else if (_textureFlipMode[i] == Emgu.CV.CvEnum.FLIP.HORIZONTAL)
                  {
                     ES20.GL.VertexAttribPointer(textureCoordHandle, 2, ES20.All.Float, false, 0, squareTextureCoordsFlipHorizontal);
                  }
                  else
                  {
                     throw new NotImplementedException(String.Format("Flip mode of {0} in GLImageView is not implemented", _textureFlipMode[i].ToString()));
                  }

                  glError = ES20.GL.GetError();
                  if (glError != ES20.All.NoError)
                  {
                     Report.Warn(PackageName, String.Format("Error after setting texture vertex: {0}", glError));
                  }

                  int textureHandle = ES20.GL.GetUniformLocation(_textureRenderRGBAProgramHandle, new StringBuilder("Texture"));
                  ES20.GL.ActiveTexture(ES20.All.Texture0);
                  ES20.GL.BindTexture(ES20.All.Texture2D, _textureIds[i]);
                  ES20.GL.Uniform1(textureHandle, 0);

                  glError = ES20.GL.GetError();
                  if (glError != ES20.All.NoError)
                  {
                     Report.Warn(PackageName, String.Format("Error after setting texture: {0}", glError));
                  }

                  // get handle to shape's transformation matrix
                  int MVPMatrixHandle = ES20.GL.GetUniformLocation(_textureRenderRGBAProgramHandle, new StringBuilder("uMVPMatrix"));

                  if (_textureRotations[i] != 0)
                  {
                     Matrix4 rotationMat = Matrix4.Mult(Matrix4.CreateRotationZ(_textureRotations[i] * _degreeToRadian), projectionMatrix);
                     ES20.GL.UniformMatrix4(MVPMatrixHandle, false, ref rotationMat);
                  }
                  else
                  {
                     // Apply the projection and view transformation
                     ES20.GL.UniformMatrix4(MVPMatrixHandle, false, ref projectionMatrix);
                  }

                  glError = ES20.GL.GetError();
                  if (glError != ES20.All.NoError)
                  {
                     Report.Warn(PackageName, String.Format("Error after setting transformation: {0}", glError));
                  }

                  ES20.GL.DrawArrays(ES20.All.TriangleFan, 0, 4);

                  glError = ES20.GL.GetError();
                  if (glError != ES20.All.NoError)
                  {
                     Report.Warn(PackageName, String.Format("Error after draw array: {0}", glError));
                  }

                  ES20.GL.DisableVertexAttribArray(positionHandle);
                  ES20.GL.DisableVertexAttribArray(textureCoordHandle);
               }
            }
         }
         else //GLES 1.1
         {
            //ES11.GL.PushMatrix();
            //ES11.GL.Rotate(_rotation, 0, 0, 1);

            ES11.GL.EnableClientState(ES11.All.VertexArray);
            ES11.GL.EnableClientState(ES11.All.TextureCoordArray);

            for (int i = 0; i <= 1; i++)
            {
               if (_textureEnabled[i] && ES11.GL.IsTexture(_textureIds[i]))
               {
                  //if (_textureRotations[i] != 0)
                  {
                     ES11.GL.PushMatrix();
                     ES11.GL.Rotate(_rotation + _textureRotations[i], 0, 0, 1.0f);
                  }
                  ES11.GL.BindTexture(ES11.All.Texture2D, _textureIds[i]);
                  ES11.GL.VertexPointer(3, ES11.All.Float, 0, _textureVertexCoords[i]);

                  if (_textureFlipMode[i] == Emgu.CV.CvEnum.FLIP.None)
                  {
                     ES11.GL.TexCoordPointer(2, ES11.All.Float, 0, squareTextureCoords);
                  }
                  else if (_textureFlipMode[i] == Emgu.CV.CvEnum.FLIP.HORIZONTAL)
                  {
                     ES11.GL.TexCoordPointer(2, ES11.All.Float, 0, squareTextureCoordsFlipHorizontal);
                  }
                  else
                  {
                     throw new NotImplementedException(String.Format("Flip mode of {0} in GLImageView is not implemented", _textureFlipMode[i].ToString()));
                  }

                  ES11.GL.DrawArrays(ES11.All.TriangleFan, 0, 4);
                  //if (_textureRotations[i] != 0)
                  {
                     ES11.GL.PopMatrix();
                  }
               }
            }

            ES11.GL.DisableClientState(ES11.All.VertexArray);
            ES11.GL.DisableClientState(ES11.All.TextureCoordArray);
            //ES11.GL.PopMatrix();
         }
#else
         if (_glVersion == OpenGLVersion.GL3)
         {
            Matrix4 projectionMatrix = Matrix4.Mult(_projectionMatrix, Matrix4.CreateRotationZ(_rotation * _degreeToRadian));

            for (int i = 0; i <= 1; i++)
            {
               if (_textureEnabled[i] && GL.IsTexture(_textureIds[i]))
               {
                  if (_textureColor[i] == TextureColor.RGB || _textureColor[i] == TextureColor.RGBA)
                  {
                     GL.UseProgram(_textureRenderRGBAProgramHandle);
                  }
                  else if (_textureColor[i] == TextureColor.BGR || _textureColor[i] == TextureColor.BGRA)
                  {
                     GL.UseProgram(_textureRenderBGRAProgramHandle);
                  }
                  else
                  {
                     continue;
                  }
                  // get handle to vertex shader's vPosition member
                  int positionHandle = GL.GetAttribLocation(_textureRenderRGBAProgramHandle, "vPosition");
                  // Enable a handle to the rectangle vertices
                  GL.EnableVertexAttribArray(positionHandle);

                  int textureCoordHandle = GL.GetAttribLocation(_textureRenderRGBAProgramHandle, "TexCoordIn");
                  GL.EnableVertexAttribArray(textureCoordHandle);

                  // Prepare the rectangle coordinate data
                  GL.VertexAttribPointer(positionHandle, 3, VertexAttribPointerType.Float, false, 0, _textureVertexCoords[i]);

                  // get handle to fragment shader's vColor member
                  //int colorHandle = GL.GetUniformLocation(_programHandle, new StringBuilder("vColor"));

                  // Set color with red, green, blue and alpha (opacity) values
                  //float[] color = new float[] { 0.63671875f, 0.76953125f, 0.22265625f, 1.0f };

                  // Set color for drawing the triangle
                  //GL.Uniform4(colorHandle, 1, color);

                  if (_textureFlipMode[i] == Emgu.CV.CvEnum.FlipType.None)
                  {
                     GL.VertexAttribPointer(textureCoordHandle, 2, VertexAttribPointerType.Float, false, 0, squareTextureCoords);
                  }
                  else if (_textureFlipMode[i] == Emgu.CV.CvEnum.FlipType.Horizontal)
                  {
                     GL.VertexAttribPointer(textureCoordHandle, 2, VertexAttribPointerType.Float, false, 0, squareTextureCoordsFlipHorizontal);
                  }
                  else
                  {
                     throw new NotImplementedException(String.Format("Flip mode of {0} in GLImageView is not implemented", _textureFlipMode[i].ToString()));
                  }

                  int textureHandle = GL.GetUniformLocation(_textureRenderRGBAProgramHandle, "Texture");
                  GL.ActiveTexture(TextureUnit.Texture0);
                  GL.BindTexture(TextureTarget.Texture2D, _textureIds[i]);
                  GL.Uniform1(textureHandle, 0);


                  // get handle to shape's transformation matrix
                  int MVPMatrixHandle = GL.GetUniformLocation(_textureRenderRGBAProgramHandle, "uMVPMatrix");

                  if (_textureRotations[i] != 0)
                  {
                     Matrix4 rotationMat = Matrix4.Mult(Matrix4.CreateRotationZ(_textureRotations[i] * _degreeToRadian), projectionMatrix);
                     GL.UniformMatrix4(MVPMatrixHandle, false, ref rotationMat);
                  }
                  else
                  {
                     // Apply the projection and view transformation
                     GL.UniformMatrix4(MVPMatrixHandle, false, ref projectionMatrix);
                  }

                  GL.DrawArrays(BeginMode.TriangleFan, 0, 4);

                  GL.DisableVertexAttribArray(positionHandle);
                  GL.DisableVertexAttribArray(textureCoordHandle);
               }
            }
         }
#endif
      }

#if IOS
      [Export("RenderAll")]
#endif
      public void RenderAll()
      {
         this.MakeCurrent();
         ResetScreen();

         RenderTextures();

         float[] lines, colors;
         GetGridLines(out lines, out colors);
         RenderLines(lines, colors, 1.0f);

         GetRectangleLines(out lines, out colors);
         RenderLines(lines, colors, 2.0f);

         SwapBuffers();
      }


      #region GL_20
      public int GetRenderLineProgram()
      {
         // Vertex and fragment shaders
         string vertexShaderSrc = "uniform mat4 uMVPMatrix;   \n" +
                       "attribute vec4 vPosition;    \n" +
                       "attribute vec4 vColor;    \n" +
                       "varying vec4 color;    \n" +
                       "void main()                  \n" +
                       "{                            \n" +
                       "   gl_Position = uMVPMatrix * vPosition;  \n" +
                       "   color = vColor;  \n" +
                       "}                            \n";

         string fragmentShaderSrc = 
                        
                        "varying vec4 color;                         \n" +
                        "void main()                                  \n" +
                        "{                                            \n" +
                        "  gl_FragColor = color;  \n" +
                        "}                                            \n";
         if (_glVersion == OpenGLVersion.ES2 || _glVersion == OpenGLVersion.ES1)
         {
            fragmentShaderSrc = "precision mediump float;             \n" + fragmentShaderSrc;
         }
         return GetRenderProgram(vertexShaderSrc, fragmentShaderSrc);
      }

      public int GetRenderProgram(String vertexShaderSrc, String fragmentShaderSrc)
      {
         if (_glVersion == OpenGLVersion.ES2)
         {
            int vertexShader = ES2LoadShader(ES20.All.VertexShader, vertexShaderSrc);
            int fragmentShader = ES2LoadShader(ES20.All.FragmentShader, fragmentShaderSrc);
            int programHandle = ES20.GL.CreateProgram();
            if (programHandle == 0)
               throw new InvalidOperationException("Unable to create program");

            ES20.GL.AttachShader(programHandle, vertexShader);
            ES20.GL.AttachShader(programHandle, fragmentShader);

            ES20.GL.BindAttribLocation(programHandle, 0, "vPosition");
            ES20.GL.LinkProgram(programHandle);

            int linked;
            ES20.GL.GetProgram(programHandle, ES20.All.LinkStatus, out linked);
            if (linked == 0)
            {
               // link failed
               int length;
               ES20.GL.GetProgram(programHandle, ES20.All.InfoLogLength, out length);
               if (length > 0)
               {
                  var log = new StringBuilder(length);
                  ES20.GL.GetProgramInfoLog(programHandle, length, out length, log);
                  Report.Debug("GL2", "Couldn't link program: " + log.ToString());
               }

               ES20.GL.DeleteProgram(programHandle);
               throw new InvalidOperationException("Unable to link program");
            }
            return programHandle;
         } else
         {
            int vertexShader = GLLoadShader(ShaderType.VertexShader, vertexShaderSrc);
            int fragmentShader = GLLoadShader(ShaderType.FragmentShader, fragmentShaderSrc);
            int programHandle = GL.CreateProgram();
            if (programHandle == 0)
               throw new InvalidOperationException("Unable to create program");

            GL.AttachShader(programHandle, vertexShader);
            GL.AttachShader(programHandle, fragmentShader);

            GL.BindAttribLocation(programHandle, 0, "vPosition");
            GL.LinkProgram(programHandle);

            int linked;
            GL.GetProgram(programHandle, ProgramParameter.LinkStatus, out linked);
            if (linked == 0)
            {
               // link failed
               int length;
               GL.GetProgram(programHandle, ProgramParameter.InfoLogLength, out length);
               if (length > 0)
               {
                  var log = new StringBuilder(length);
                  GL.GetProgramInfoLog(programHandle, length, out length, log);
                  Report.Debug("GL", "Couldn't link program: " + log.ToString());
               }

               GL.DeleteProgram(programHandle);
               throw new InvalidOperationException("Unable to link program");
            }
            return programHandle;
         }
         
      }

      public int GetRenderRGBATextureProgram()
      {
         // Vertex and fragment shaders
         string vertexShaderSrc = "uniform mat4 uMVPMatrix;   \n" +
                       "attribute vec4 vPosition;    \n" +
                       "attribute vec2 TexCoordIn;    \n" +
                       "varying vec2 TexCoordOut;    \n" +
                       "void main()                  \n" +
                       "{                            \n" +
                       "   gl_Position = uMVPMatrix * vPosition;  \n" +
                       "   TexCoordOut = TexCoordIn;  \n" +
                       "}                            \n";

         string fragmentShaderSrc = 
                        //"varying lowp vec2 TexCoordOut;                         \n" +
                        "uniform sampler2D Texture;                         \n" +
                        "void main()                                  \n" +
                        "{                                            \n" +
                        "  gl_FragColor = texture2D(Texture, TexCoordOut);  \n" +
                        "}                                            \n";
         if (_glVersion == OpenGLVersion.ES1 || _glVersion == OpenGLVersion.ES2)
         {
            fragmentShaderSrc = "precision mediump float;             \n" +
               "varying lowp vec2 TexCoordOut;                         \n" + fragmentShaderSrc;
         }
         else
         {
            fragmentShaderSrc = "in vec2 TexCoordOut;\n" + fragmentShaderSrc;
         }
         return GetRenderProgram(vertexShaderSrc, fragmentShaderSrc);
      }

      public int GetRenderBGRATextureProgram()
      {
         // Vertex and fragment shaders
         string vertexShaderSrc = "uniform mat4 uMVPMatrix;   \n" +
                       "attribute vec4 vPosition;    \n" +
                       "attribute vec2 TexCoordIn;    \n" +
                       "varying vec2 TexCoordOut;    \n" +
                       "void main()                  \n" +
                       "{                            \n" +
                       "   gl_Position = uMVPMatrix * vPosition;  \n" +
                       "   TexCoordOut = TexCoordIn;  \n" +
                       "}                            \n";

         string fragmentShaderSrc = 
                        
                        "uniform sampler2D Texture;                         \n" +
                        "void main()                                  \n" +
                        "{                                            \n" +
                        "  vec4 color = texture2D(Texture, TexCoordOut);  \n" +
                        "  gl_FragColor = vec4(color.b, color.g, color.r, color.a);  \n" +
                        "}                                            \n";
         if (_glVersion == OpenGLVersion.ES1 || _glVersion == OpenGLVersion.ES2)
         {
            fragmentShaderSrc = "precision mediump float;             \n" +
               "varying lowp vec2 TexCoordOut;                         \n" + fragmentShaderSrc;
         } else
         {
            fragmentShaderSrc = "in vec2 TexCoordOut;\n" + fragmentShaderSrc;
         }
         return GetRenderProgram(vertexShaderSrc, fragmentShaderSrc);
      }
      #endregion

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         if (_glVersion == OpenGLVersion.ES2)
         {
            ES20.GL.DeleteTextures(2, _textureIds);
         }
         else if (_glVersion == OpenGLVersion.ES1)
         {
            ES11.GL.DeleteTextures(2, _textureIds);
         } else
         {
            //GL.DeleteTextures(2, _textureIds);
         }

         if (_bgrBuffers != null)
            _bgrBuffers.Dispose();
      }

      public static float ToRadians(float degrees)
      {
         //pi/180
         //FIXME: precalc pi/180
         return (float) (degrees * (System.Math.PI / 180.0));
      }

      public enum TextureColor
      {
         NotSupported,
         BGR,
         BGRA, 
         RGBA, 
         RGB
      }

      public void SetFrame(String fileName)
      {
         _frameFileName = fileName;
         LoadFrame();
      }

      private void LoadFrame()
      {
         if (_frameFileName == null)
         {
            _textureEnabled[1] = false;
         }
         else
         {
            _textureEnabled[1] = true;

            Size textureSize = new Size(_glTextureDimension, _glTextureDimension);
            using (Image<Bgra, Byte> tmp = new Image<Bgra, byte>(_frameFileName))
            {
               if (_glVersion == OpenGLVersion.ES1)
               {
                  if (tmp.Size == textureSize)
                  {
                     CvInvoke.CvtColor(tmp, tmp, Emgu.CV.CvEnum.ColorConversion.Bgra2Rgba);
                     LoadTexture(tmp.Size, tmp.MIplImage.ImageData, GLImageView.TextureColor.RGBA, 1);
                  }
                  else
                     using (
                        Image<Bgra, Byte> resize = tmp.Resize(_glTextureDimension,
                                                              _glTextureDimension,
                                                              Emgu.CV.CvEnum.Inter.Nearest))
                     {
                        CvInvoke.CvtColor(resize, resize, Emgu.CV.CvEnum.ColorConversion.Bgra2Rgba);
                        LoadTexture(resize.Size, resize.MIplImage.ImageData, GLImageView.TextureColor.RGBA, 1);
                     }
               }
               else
               {
                  LoadTexture(tmp.Size, tmp.MIplImage.ImageData, TextureColor.BGRA, 1);
               }
            }
         }
      }

      public void SetImage(Image<Bgr, Byte> image, GeometricChange geometricChange)
      {
         SetImage(image, geometricChange, Size.Empty);
      }

      public void SetImage(Image<Bgr, Byte> image, GeometricChange geometricChange, Size imageDisplaySize)
      {
         if (_glVersion == OpenGLVersion.GL3)
         {
            //Desktop GL requires the image to be rotated 90 degree
            geometricChange.Rotation = (geometricChange.Rotation + 270) % 360;
         }
         if (image == null)
         {
            TextureEnabled[0] = false;
            _imageSize = Size.Empty;
         }
         else
         {
            if (geometricChange.Rotation == 0 || geometricChange.Rotation == 180)
            {
               _imageSize = imageDisplaySize.IsEmpty ? image.Size : imageDisplaySize;
            }
            else if (geometricChange.Rotation == 90 || geometricChange.Rotation == 270)
            {
               _imageSize = imageDisplaySize.IsEmpty ? new Size(image.Height, image.Width) : new Size(imageDisplaySize.Height, imageDisplaySize.Width);
            }
            else
            {
               throw new Exception(String.Format("Unsupported geometricChange: {0}", geometricChange)); 
            }
            _textureRotations[0] = geometricChange.Rotation;
            _textureFlipMode[0] = geometricChange.FlipMode;

            if (_glVersion == OpenGLVersion.ES1)
            {
               Image<Bgr, Byte> resized = _bgrBuffers.GetBuffer(new Size(_glTextureDimension, _glTextureDimension), 0);

               CvInvoke.Resize(image, resized, resized.Size, 0, 0, Emgu.CV.CvEnum.Inter.Nearest);
               CvInvoke.CvtColor(resized, resized, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
               LoadTexture(resized.Size, resized.MIplImage.ImageData, TextureColor.RGB, 0);
            }
            else
            {
               LoadTexture(image.Size, image.MIplImage.ImageData, TextureColor.BGR, 0);
            }
            
            TextureEnabled[0] = true;
         }
         //SetupCamera();
         PerformRenderAll();
      }

      public void LoadTexture(System.Drawing.Size imageSize, IntPtr data, TextureColor textureColor, int textureIndex)
      {
         bool reuseTexture = (!_textureSizes[textureIndex].IsEmpty && _textureSizes[textureIndex].Equals(imageSize) && _textureColor[textureIndex] == textureColor && !_textureRequiresReset[textureIndex]);
         LoadTexture(imageSize, data, textureColor, textureIndex, reuseTexture);
      }

      private void LoadTexture(System.Drawing.Size imageSize, IntPtr data, TextureColor textureColor, int textureIndex, bool reuseTexture)
      {
#if ANDROID || IOS
         if (_glVersion == OpenGLVersion.ES2)
         {
            ES20.All error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("Error before attemps to bind Texture: {0}", error));
               return;
            }

            ES20.GL.BindTexture(ES20.All.Texture2D, _textureIds[textureIndex]);

            error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("Failed to bind Texture: {0}", error));
               return;
            }
            ES20.GL.TexParameter(ES20.All.Texture2D, ES20.All.TextureMagFilter, (int)ES20.All.Linear);
            ES20.GL.TexParameter(ES20.All.Texture2D, ES20.All.TextureMinFilter, (int)ES20.All.Linear);
            ES20.GL.TexParameter(ES20.All.Texture2D, ES20.All.TextureWrapS, (int)ES20.All.ClampToEdge);
            ES20.GL.TexParameter(ES20.All.Texture2D, ES20.All.TextureWrapT, (int)ES20.All.ClampToEdge);

            error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("Failed to set Texture Parameter: {0}", error));
               return;
            }

            if (reuseTexture)
            {
               //if (textureColor == TextureColor.BGRA)
               //   ES20.GL.TexSubImage2D(ES20.All.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, ES20.All.BgraExt, ES20.All.UnsignedByte, data);
               //else 
               if (textureColor == TextureColor.RGBA || textureColor == TextureColor.BGRA)
                  ES20.GL.TexSubImage2D(ES20.All.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, ES20.All.Rgba, ES20.All.UnsignedByte, data);
               else if (textureColor == TextureColor.RGB || textureColor == TextureColor.BGR)
                  ES20.GL.TexSubImage2D(ES20.All.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, ES20.All.Rgb, ES20.All.UnsignedByte, data);
            }
            else
            {
               _textureSizes[textureIndex] = imageSize;
               _textureColor[textureIndex] = textureColor;
               //if (textureColor == TextureColor.BGRA)
                  //ES20.GL.TexImage2D(ES20.All.Texture2D, 0, (int)ES20.All.BgraExt, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, ES20.All.BgraExt, ES20.All.UnsignedByte, data);
               //else 
               if (textureColor == TextureColor.RGBA || textureColor == TextureColor.BGRA)
                  ES20.GL.TexImage2D(ES20.All.Texture2D, 0, (int)ES20.All.Rgba, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, ES20.All.Rgba, ES20.All.UnsignedByte, data);
               else if (textureColor == TextureColor.RGB || textureColor == TextureColor.BGR)
                  ES20.GL.TexImage2D(ES20.All.Texture2D, 0, (int)ES20.All.Rgb, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, ES20.All.Rgb, ES20.All.UnsignedByte, data);
               else
               {
                  throw new Exception(String.Format("The specific texture color is not supported: {0}", textureColor));
               }
               _textureRequiresReset[textureIndex] = false;
            }
            error = ES20.GL.GetError();
            if (error != ES20.All.NoError)
            {
               Report.Error(PackageName, String.Format("Failed to load Texture {0} (Texture ID {1}): {2}; reuseTexture: {3}", textureIndex, _textureIds[textureIndex], error, reuseTexture));
               if (reuseTexture)
               {
                  //try again without reusing the texture
                  LoadTexture(imageSize, data, textureColor, textureIndex, false);
               }
               return;
            }
         }
         else
         {
            ES11.All error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("Error before attemps to bind Texture: {0}", error));
               return;
            }

            ES11.GL.BindTexture(ES11.All.Texture2D, _textureIds[textureIndex]);

            error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("Failed to bing Texture: {0}", error));
               return;
            }
            // setup texture parameters
            ES11.GL.TexParameterx(ES11.All.Texture2D, ES11.All.TextureMagFilter, (int)ES11.All.Linear);
            ES11.GL.TexParameterx(ES11.All.Texture2D, ES11.All.TextureMinFilter, (int)ES11.All.Linear);
            ES11.GL.TexParameterx(ES11.All.Texture2D, ES11.All.TextureWrapS, (int)ES11.All.ClampToEdge);
            ES11.GL.TexParameterx(ES11.All.Texture2D, ES11.All.TextureWrapT, (int)ES11.All.ClampToEdge);

            error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("Failed to set Texture Parameter: {0}", error));
               return;
            }

            if (reuseTexture)
            {
               if (textureColor == TextureColor.BGRA)
                  ES11.GL.TexSubImage2D(ES11.All.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, ES11.All.BgraExt, ES11.All.UnsignedByte, data);
               else if (textureColor == TextureColor.RGBA)
                  ES11.GL.TexSubImage2D(ES11.All.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, ES11.All.Rgba, ES11.All.UnsignedByte, data);
               else if (textureColor == TextureColor.RGB)
                  ES11.GL.TexSubImage2D(ES11.All.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, ES11.All.Rgb, ES11.All.UnsignedByte, data);
            }
            else
            {
               _textureSizes[textureIndex] = imageSize;
               _textureColor[textureIndex] = textureColor;
               if (textureColor == TextureColor.BGRA)
                  ES11.GL.TexImage2D(ES11.All.Texture2D, 0, (int)ES11.All.BgraExt, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, ES11.All.BgraExt, ES11.All.UnsignedByte, data);
               else if (textureColor == TextureColor.RGBA)
                  ES11.GL.TexImage2D(ES11.All.Texture2D, 0, (int)ES11.All.Rgba, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, ES11.All.Rgba, ES11.All.UnsignedByte, data);
               else if (textureColor == TextureColor.RGB)
                  ES11.GL.TexImage2D(ES11.All.Texture2D, 0, (int)ES11.All.Rgb, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, ES11.All.Rgb, ES11.All.UnsignedByte, data);
               else
               {
                  throw new Exception(String.Format("The specific texture color is not supported: {0}", textureColor));
               }
               _textureRequiresReset[textureIndex] = false;
            }
            error = ES11.GL.GetError();
            if (error != ES11.All.NoError)
            {
               Report.Error(PackageName, String.Format("Failed to load Texture {0} (Texture ID {1}): {2}; reuseTexture: {3}", textureIndex, _textureIds[textureIndex], error, reuseTexture));
               if (reuseTexture)
               {
                  //try again without reusing the texture
                  LoadTexture(imageSize, data, textureColor, textureIndex, false);
               }
               return;
            }
         }
#else
         if (_glVersion == OpenGLVersion.GL3)
         {
            GL.BindTexture(TextureTarget.Texture2D, _textureIds[textureIndex]);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            if (reuseTexture)
            {
               //if (textureColor == TextureColor.BGRA)
               //   ES20.GL.TexSubImage2D(ES20.All.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, ES20.All.BgraExt, ES20.All.UnsignedByte, data);
               //else 
               if (textureColor == TextureColor.RGBA || textureColor == TextureColor.BGRA)
                  GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, PixelFormat.Rgba, PixelType.UnsignedByte, data);
               else if (textureColor == TextureColor.RGB || textureColor == TextureColor.BGR)
                  GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, PixelFormat.Rgb, PixelType.UnsignedByte, data);
            }
            else
            {
               _textureSizes[textureIndex] = imageSize;
               _textureColor[textureIndex] = textureColor;
               //if (textureColor == TextureColor.BGRA)
               //ES20.GL.TexImage2D(ES20.All.Texture2D, 0, (int)ES20.All.BgraExt, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, ES20.All.BgraExt, ES20.All.UnsignedByte, data);
               //else 
               if (textureColor == TextureColor.RGBA || textureColor == TextureColor.BGRA)
                  GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
               else if (textureColor == TextureColor.RGB || textureColor == TextureColor.BGR)
                  GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, _textureSizes[textureIndex].Width, _textureSizes[textureIndex].Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, data);
               else
               {
                  throw new Exception(String.Format("The specific texture color is not supported: {0}", textureColor));
               }
               _textureRequiresReset[textureIndex] = false;
            }
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
               Report.Error(PackageName, String.Format("Failed to load Texture {0} (Texture ID {1}): {2}; reuseTexture: {3}", textureIndex, _textureIds[textureIndex], error, reuseTexture));
               if (reuseTexture)
               {
                  //try again without reusing the texture
                  LoadTexture(imageSize, data, textureColor, textureIndex, false);
               }
               return;
            }
         }
#endif

         _textureEnabled[textureIndex] = true;
      }

   }
}
