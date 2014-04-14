//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.AVFoundation;
using MonoTouch.UIKit;
using MonoTouch.CoreVideo;
using MonoTouch.CoreMedia;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreFoundation;

namespace Emgu.CV.Example.MonoTouch
{
    public class CameraDialogViewController : DialogViewController
    {
      public static UIImageView ImageView;
      AVCaptureSession session;
      OutputRecorder outputRecorder;
      DispatchQueue queue;

        public CameraDialogViewController()
         : base(new RootElement(""), true)
        {

        }

      public override void ViewDidLoad()
      {
         base.ViewDidLoad();

         RootElement root = Root;
         ImageView = new UIImageView(View.Frame);
         root.Add (new Section() { ImageView});


         if (!SetupCaptureSession ())
         {
         }
            //AddSubview (new UILabel (new RectangleF (20, 20, 200, 60)) { Text = "No input device" });


      }

bool SetupCaptureSession ()
      {
         // configure the capture session for low resolution, change this if your code
         // can cope with more data or volume
         session = new AVCaptureSession () {
            SessionPreset = AVCaptureSession.PresetMedium
         };

         // create a device input and attach it to the session
         var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType (AVMediaType.Video);
         var input = AVCaptureDeviceInput.FromDevice (captureDevice);
         if (input == null){
            Console.WriteLine ("No input device");
            return false;
         }
         session.AddInput (input);

         // create a VideoDataOutput and add it to the sesion
         var output = new AVCaptureVideoDataOutput () {
            VideoSettings = new AVVideoSettings (CVPixelFormatType.CV32BGRA),

            // If you want to cap the frame rate at a given speed, in this sample: 15 frames per second
            //MinFrameDuration = new CMTime (1, 15)
         };

         // configure the output
         queue = new DispatchQueue ("myQueue");
         outputRecorder = new OutputRecorder (ImageView);
         output.SetSampleBufferDelegateAndQueue (outputRecorder, queue);
         session.AddOutput (output);

         session.StartRunning ();
         return true;
      }

      public class OutputRecorder : AVCaptureVideoDataOutputSampleBufferDelegate 
      { 
         private UIImageView _imageView;
         public OutputRecorder(UIImageView imageView)
         {
            _imageView = imageView;
         }
         public override void DidOutputSampleBuffer (AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
         {
            try {
               UIImage image = ImageFromSampleBuffer (sampleBuffer);

               // Do something with the image, we just stuff it in our main view.
               BeginInvokeOnMainThread (delegate {
                  if (_imageView.Frame.Size != image.Size)
                     _imageView.Frame = new RectangleF(Point.Empty, image.Size);
                  _imageView.Image = image;
               });

               //
               // Although this looks innocent "Oh, he is just optimizing this case away"
               // this is incredibly important to call on this callback, because the AVFoundation
               // has a fixed number of buffers and if it runs out of free buffers, it will stop
               // delivering frames. 
               // 
               sampleBuffer.Dispose ();
            } catch (Exception e){
               Console.WriteLine (e);
            }
         }

         //private static MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0, 1.0);

         UIImage ImageFromSampleBuffer (CMSampleBuffer sampleBuffer)
         {
            // Get the CoreVideo image
            using (CVPixelBuffer pixelBuffer = sampleBuffer.GetImageBuffer () as CVPixelBuffer)
            {
               // Lock the base address
               pixelBuffer.Lock (0);
               // Get the number of bytes per row for the pixel buffer
               IntPtr baseAddress = pixelBuffer.BaseAddress;
               int bytesPerRow = pixelBuffer.BytesPerRow;
               int width = pixelBuffer.Width;
               int height = pixelBuffer.Height;

               using (Image<Bgra, byte> bgra = new Image<Bgra, byte>(width, height, bytesPerRow, baseAddress))
               using (Image<Bgr, byte> bgr = bgra.Convert<Bgr, byte>())
               using (Image<Bgr, byte> bgr2 = bgr.Rotate(90, new Bgr(0,0,0)))
               {
                  bgr2.Draw(
                     string.Format("{0} x {1}", width, height),
                     new Point(20, 20), 
                     CvEnum.FontFace.HersheySimplex,
                     1.0,
                     new Bgr(255, 0, 0));
                  //CvInvoke.cvCvtColor(bgr2, bgra, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2BGRA);
                  UIImage result = bgr2.ToUIImage();
                  pixelBuffer.Unlock(0);
                  return result;
               }

            }
         }
      }
    }
}

