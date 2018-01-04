//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.CvEnum;
using MonoTouch.Dialog;
using Foundation;
using AVFoundation;
using UIKit;
using CoreVideo;
using CoreMedia;
using CoreGraphics;
using CoreFoundation;

namespace Example.iOS
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


         CheckVideoPermissionAndStart();

      }

        private void RenderImageMessage(String message)
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(512, 512, new Bgr(255, 255, 255));
            CvInvoke.PutText(
               img,
               message,
               new Point(10, 200),
               FontFace.HersheyComplex,
               1,
               new MCvScalar(),
               2);
            ImageView.Image = img.ToUIImage();
        }

        private void CheckVideoPermissionAndStart()
        {
            AVFoundation.AVAuthorizationStatus authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            switch (authorizationStatus)
            {
                case AVAuthorizationStatus.NotDetermined:
                    AVCaptureDevice.RequestAccessForMediaType(AVMediaType.Video, delegate (bool granted)
                    {
                        if (granted)
                        {
                            SetupCaptureSession();
                        }
                        else
                        {
                            RenderImageMessage("Please grant Video Capture permission");
                        }
                    });
                    break;
                case AVAuthorizationStatus.Authorized:
                    SetupCaptureSession();
                    break;
                case AVAuthorizationStatus.Denied:
                case AVAuthorizationStatus.Restricted:
                    RenderImageMessage("Please grant Video Capture permission");
                    break;
                default:

                    break;
                    //do nothing
            }
        }

        private void SetupCaptureSession()
        {
            // configure the capture session for low resolution, change this if your code
            // can cope with more data or volume
            session = new AVCaptureSession()
            {
                SessionPreset = AVCaptureSession.PresetMedium
            };



            // create a device input and attach it to the session
            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            if (captureDevice == null)
            {
                RenderImageMessage("Capture device not found.");

                return;
            }
            var input = AVCaptureDeviceInput.FromDevice(captureDevice);
            if (input == null)
            {
                RenderImageMessage("No input device");

                return;
            }
            session.AddInput(input);

            // create a VideoDataOutput and add it to the sesion
            AVVideoSettingsUncompressed settingUncomp = new AVVideoSettingsUncompressed();
            settingUncomp.PixelFormatType = CVPixelFormatType.CV32BGRA;
            var output = new AVCaptureVideoDataOutput()
            {
                UncompressedVideoSetting = settingUncomp,

                // If you want to cap the frame rate at a given speed, in this sample: 15 frames per second
                //MinFrameDuration = new CMTime (1, 15)
            };


            // configure the output
            queue = new DispatchQueue("myQueue");
            outputRecorder = new OutputRecorder(ImageView);
            output.SetSampleBufferDelegateQueue(outputRecorder, queue);
            session.AddOutput(output);

            session.StartRunning();

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
                     _imageView.Frame = new CGRect(CGPoint.Empty, image.Size);
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
               pixelBuffer.Lock (CVPixelBufferLock.ReadOnly);
               // Get the number of bytes per row for the pixel buffer
               IntPtr baseAddress = pixelBuffer.BaseAddress;
               int bytesPerRow = (int)pixelBuffer.BytesPerRow;
               int width = (int)pixelBuffer.Width;
               int height = (int)pixelBuffer.Height;

               using (Image<Bgra, byte> bgra = new Image<Bgra, byte>(width, height, bytesPerRow, baseAddress))
               using (Image<Bgr, byte> bgr = bgra.Convert<Bgr, byte>())
               using (Image<Bgr, byte> bgr2 = bgr.Rotate(90, new Bgr(0,0,0)))
               {
                  bgr2.Draw(
                     string.Format("{0} x {1}", width, height),
                     new Point(20, 20), 
                     FontFace.HersheySimplex,
                     1.0,
                     new Bgr(255, 0, 0));
                  //CvInvoke.cvCvtColor(bgr2, bgra, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2BGRA);
                  UIImage result = bgr2.ToUIImage();
                  pixelBuffer.Unlock(CVPixelBufferLock.ReadOnly);
                  return result;
               }

            }
         }
      }
    }
}

