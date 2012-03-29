//
// Camera preview based on monodroid API-samples
// https://github.com/xamarin/monodroid-samples/blob/master/ApiDemo/Graphics/CameraPreview.cs
//

using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Paint = Android.Graphics.Paint;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MonoAndroidCamera
{
   [Activity(Label = "Emgu CV: MonoAndroidCamera", MainLauncher = true, Icon = "@drawable/icon")]
   public class CameraPreview : Activity
   {
      private Preview preview;
      private TopLayer _topLayer;

      private IMenuItem _menuCanny;
      private IMenuItem _menuFacedetect;
      private IMenuItem _menuPreview;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         _topLayer = new TopLayer(this);

         preview = new Preview(this, _topLayer);
         SetContentView(preview);
         AddContentView(_topLayer, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent ));

         SetRequestedOrientation(Android.Content.PM.ScreenOrientation.Landscape);
      }

      public override bool OnCreateOptionsMenu(IMenu menu)
      {
         _menuPreview = menu.Add("Preview");
         _menuCanny = menu.Add("Canny");
         _menuFacedetect = menu.Add("Face Detect");
         return base.OnCreateOptionsMenu(menu);
      }

      public override bool OnOptionsItemSelected(IMenuItem item)
      {
         if (item == _menuCanny)
         {
            _topLayer.Mode = ViewMode.Canny;
         }
         else if (item == _menuFacedetect)
         {
            _topLayer.Mode = ViewMode.FaceDetect;
         } else
         {
            _topLayer.Mode = ViewMode.Preview;
         }
         return base.OnOptionsItemSelected(item);
      }
   }

   enum ViewMode
   {
      Preview,
      Canny, 
      FaceDetect
   }

   class TopLayer : View, Camera.IPreviewCallback
   {
      private Stopwatch _watch;
      Paint _paint;
      HaarCascade _faceDetector;
      Android.Graphics.Bitmap _bmp;
      int[] _bgraData;
      byte[] _bgrData;

      public ViewMode Mode = ViewMode.Preview;

      public TopLayer(Context context)
         : base(context)
      {
         _paint = new Paint();
         _paint.SetStyle(Paint.Style.Stroke);
         _paint.SetARGB(255, 255, 0, 0);
         _paint.TextSize = 25;

         Image<Bgr, Byte> img = new Image<Bgr, byte>(4, 8);

         System.IO.Stream iStream = context.Assets.Open("haarcascade_frontalface_default.xml"); ;
         Java.IO.File dir = context.GetDir("cascade", FileCreationMode.Private);
         Java.IO.File file = new Java.IO.File(dir, "cascade.xml");
         using (System.IO.Stream os = System.IO.File.OpenWrite(file.AbsolutePath))
         {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = iStream.Read(buffer, 0, buffer.Length)) > 0)
               os.Write(buffer, 0, len);
         }
         _faceDetector = new HaarCascade(file.AbsolutePath);
         file.Delete();
         dir.Delete();

         _watch = Stopwatch.StartNew();
      }

      protected override void OnDraw(Android.Graphics.Canvas canvas)
      {
         base.OnDraw(canvas);

         lock (this)
         {
            if (_bmp != null && canvas != null)
            {
               Stopwatch w = Stopwatch.StartNew();
               canvas.DrawBitmap(_bmp, 0, 0, null);
               w.Stop();
               _watch.Stop();
               canvas.DrawText(String.Format("{0:F2} FPS; Render Time: {1} ms", 1.0 / _watch.ElapsedMilliseconds * 1000, w.ElapsedMilliseconds), 20, 20, _paint);
               _watch.Reset();
               _watch.Start();
            }
         }
      }

      private bool _busy;

      public void OnPreviewFrame(byte[] data, Camera camera)
      {
         if (!_busy)
            try
            {
               _busy = true;
               Camera.Size size = camera.GetParameters().PreviewSize;
               GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

               if (Mode == ViewMode.Canny)
               {
                  if (_bgrData == null || _bgrData.Length < size.Width * size.Height)
                     _bgrData = new byte[size.Width * size.Height * 3];
                  GCHandle bgrHandle = GCHandle.Alloc(_bgrData, GCHandleType.Pinned);
                  using (Image<Gray, Byte> grey = new Image<Gray, byte>(size.Width, size.Height, size.Width, handle.AddrOfPinnedObject()))
                  using (Image<Gray, Byte> canny = new Image<Gray,byte>(size.Width, size.Height, size.Width, bgrHandle.AddrOfPinnedObject()))
                  { 
                     CvInvoke.cvCanny(grey, canny, 100, 60, 3);

                     if (_bgraData == null || _bgraData.Length < size.Width * size.Height)
                        _bgraData = new int[size.Width * size.Height];
                     GCHandle bgraHandle = GCHandle.Alloc(_bgraData, GCHandleType.Pinned);
                     using (Image<Bgra, Byte> bgra = new Image<Bgra, byte>(size.Width, size.Height, size.Width * 4, bgraHandle.AddrOfPinnedObject()))
                        CvInvoke.cvCvtColor(canny, bgra, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGRA);
                     bgraHandle.Free();

                     lock (this)
                     {
                        if (_bmp != null && (_bmp.Width != size.Width || _bmp.Height != size.Height))
                        {
                           _bmp.Dispose();
                           _bmp = null;
                        }
                        if (_bmp == null)
                           _bmp = Android.Graphics.Bitmap.CreateBitmap(size.Width, size.Height, Android.Graphics.Bitmap.Config.Argb8888);

                        _bmp.SetPixels(_bgraData, 0, size.Width, 0, 0, size.Width, size.Height);
                     }
                  }
               }
               else if (Mode == ViewMode.Preview)
               {
                  if (_bgraData == null || _bgraData.Length < size.Width * size.Height)
                     _bgraData = new int[size.Width * size.Height];
                  GCHandle bgraHandle = GCHandle.Alloc(_bgraData, GCHandleType.Pinned);
                  using (Image<Gray, Byte> tmp = new Image<Gray, byte>(size.Width, (size.Height >> 1) * 3, size.Width, handle.AddrOfPinnedObject()))
                  using (Image<Bgra, Byte> bgra = new Image<Bgra, byte>(size.Width, size.Height, size.Width * 4, bgraHandle.AddrOfPinnedObject()))
                     CvInvoke.cvCvtColor(tmp, bgra, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_YUV420sp2BGRA);
                  bgraHandle.Free();

                  lock (this)
                  {
                     if (_bmp != null && (_bmp.Width != size.Width || _bmp.Height != size.Height))
                     {
                        _bmp.Dispose();
                        _bmp = null;
                     }
                     if (_bmp == null)
                        _bmp = Android.Graphics.Bitmap.CreateBitmap(size.Width, size.Height, Android.Graphics.Bitmap.Config.Argb8888);

                     _bmp.SetPixels(_bgraData, 0, size.Width, 0, 0, size.Width, size.Height);
                  }
               }
               else
               {  //face detection
                  MCvAvgComp[] faces;
                  using (Image<Gray, Byte> grey = new Image<Gray, byte>(size.Width, size.Height, size.Width, handle.AddrOfPinnedObject()))
                     faces = _faceDetector.Detect(grey);

                  if (_bgraData == null || _bgraData.Length < size.Width * size.Height)
                     _bgraData = new int[size.Width * size.Height];
                  if (_bgrData == null || _bgrData.Length < size.Width * size.Height * 3)
                     _bgrData = new byte[size.Width * size.Height * 3];

                  GCHandle bgraHandle = GCHandle.Alloc(_bgraData, GCHandleType.Pinned);
                  GCHandle bgrHandle = GCHandle.Alloc(_bgrData, GCHandleType.Pinned);
                  using (Image<Gray, Byte> tmp = new Image<Gray, byte>(size.Width, (size.Height >> 1) * 3, size.Width, handle.AddrOfPinnedObject()))
                  using (Image<Bgr, Byte> bgr = new Image<Bgr, byte>(size.Width, size.Height, size.Width * 3, bgrHandle.AddrOfPinnedObject()))
                  using (Image<Bgra, Byte> bgra = new Image<Bgra, byte>(size.Width, size.Height, size.Width * 4, bgraHandle.AddrOfPinnedObject()))
                  {
                     CvInvoke.cvCvtColor(tmp, bgr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_YUV420sp2BGR);
                     foreach (MCvAvgComp face in faces)
                        bgr.Draw(face.rect, new Bgr(255, 0, 0), 2);
                     CvInvoke.cvCvtColor(bgr, bgra, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2BGRA);
                  }
                  bgraHandle.Free();
                  bgrHandle.Free();
                  lock (this)
                  {
                     if (_bmp != null && (_bmp.Width != size.Width || _bmp.Height != size.Height))
                     {
                        _bmp.Dispose();
                        _bmp = null;
                     }
                     if (_bmp == null)
                        _bmp = Android.Graphics.Bitmap.CreateBitmap(size.Width, size.Height, Android.Graphics.Bitmap.Config.Argb8888);

                     _bmp.SetPixels(_bgraData, 0, size.Width, 0, 0, size.Width, size.Height);
                  }
               }

               handle.Free();

               this.Invalidate();
            }
            finally
            {
               _busy = false;
            }
         camera.AddCallbackBuffer(data);
      }
   }

   class Preview : SurfaceView, ISurfaceHolderCallback
   {
      ISurfaceHolder surface_holder;
      Camera camera;
      TopLayer _topLayer;

      public Preview(Context context, TopLayer topLayer)
         : base(context)
      {
         // Install a SurfaceHolder.Callback so we get notified when the
         // underlying surface is created and destroyed.
         surface_holder = Holder;
         surface_holder.AddCallback(this);
         //surface_holder.SetType(SurfaceType.PushBuffers);

         _topLayer = topLayer;
      }

      public void SurfaceCreated(ISurfaceHolder holder)
      {
         // The Surface has been created, acquire the camera and tell it where
         // to draw.
         camera = Camera.Open();
         
         try
         {
            camera.SetPreviewDisplay(holder);
            //camera.SetPreviewDisplay(null);
         }
         catch (Exception)
         {
            camera.Release();
            camera = null;
            // TODO: add more exception handling logic here
         }
      }

      public void SurfaceDestroyed(ISurfaceHolder holder)
      {
         // Surface will be destroyed when we return, so stop the preview.
         // Because the CameraDevice object is not a shared resource, it's very
         // important to release it when the activity is paused.
         camera.StopPreview();
         camera.SetPreviewCallbackWithBuffer(null);
         camera.Release();
         camera = null;
      }

      private Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h)
      {
         const double ASPECT_TOLERANCE = 0.05;
         double targetRatio = (double)w / h;

         if (sizes == null)
            return null;

         Camera.Size optimalSize = null;
         double minDiff = Double.MaxValue;

         int targetHeight = h;

         // Try to find an size match aspect ratio and size
         for (int i = 0; i < sizes.Count; i++)
         {
            Camera.Size size = sizes[i];
            double ratio = (double)size.Width / size.Height;

            if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE)
               continue;

            if (Math.Abs(size.Height - targetHeight) < minDiff)
            {
               optimalSize = size;
               minDiff = Math.Abs(size.Height - targetHeight);
            }
         }

         // Cannot find the one match the aspect ratio, ignore the requirement
         if (optimalSize == null)
         {
            minDiff = Double.MaxValue;
            for (int i = 0; i < sizes.Count; i++)
            {
               Camera.Size size = sizes[i];

               if (Math.Abs(size.Height - targetHeight) < minDiff)
               {
                  optimalSize = size;
                  minDiff = Math.Abs(size.Height - targetHeight);
               }
            }
         }

         return optimalSize;
      }

      public void SurfaceChanged(ISurfaceHolder holder, int format, int w, int h)
      {
         // Now that the size is known, set up the camera parameters and begin
         // the preview.
         Camera.Parameters parameters = camera.GetParameters();

         IList<Camera.Size> sizes = parameters.SupportedPreviewSizes;
         Camera.Size optimalSize = GetOptimalPreviewSize(sizes, w, h);

         parameters.SetPreviewSize(optimalSize.Width, optimalSize.Height);
         
         camera.SetParameters(parameters);

         //set for protrait mode
         //camera.SetDisplayOrientation(90);

         //camera.SetPreviewCallback(_topLayer);
         camera.SetPreviewCallbackWithBuffer(_topLayer);
         int bufferSize = optimalSize.Width * (optimalSize.Height >> 1) * 3;
         for (int i = 0; i < 1; ++i)
            camera.AddCallbackBuffer(new byte[bufferSize]);
         camera.StartPreview();

         Layout(0, 0, optimalSize.Width, optimalSize.Height);
      }
   }
}

