// Based on the following Vuforia tutorial
// https://library.vuforia.com/articles/Solution/How-To-Access-the-Camera-Image-in-Unity
// Modified to convert Vuforia.Image to Emgu CV Mat by Canming Huang 08/30/2016

using System;
using UnityEngine;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using Vuforia;
using Emgu.CV;
using Emgu.CV.CvEnum;

public class CameraImageAccessBehaviour : MonoBehaviour
{
   //either use RGB888 or RGBA8888, depends on your camera device.
   //private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGB888;
   private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGBA8888;

   private bool m_RegisteredFormat = false;

   void Start()
   {
      VuforiaBehaviour vuforiaBehaviour = (VuforiaBehaviour)FindObjectOfType(typeof(VuforiaBehaviour));
      if (vuforiaBehaviour)
      {
         vuforiaBehaviour.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
      }
   }

   public void OnTrackablesUpdated()
   {
      if (!m_RegisteredFormat)
      {
         if (CameraDevice.Instance.SetFrameFormat(m_PixelFormat, true))
         {
            m_RegisteredFormat = true;
         }

      }

      CameraDevice cam = CameraDevice.Instance;
      Image image = cam.GetCameraImage(m_PixelFormat);

      if (image == null)
      {
         Debug.Log("Image is not available yet");
      }
      else
      {


         byte[] pixels = image.Pixels;
         GCHandle pixelHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
         try
         {
            if (m_PixelFormat == Image.PIXEL_FORMAT.RGBA8888)
            {
               using (
                  Mat m = new Mat(new Size(image.Width, image.Height), DepthType.Cv8U, 4,
                     pixelHandle.AddrOfPinnedObject(), image.Stride))
               using (Mat alphaChannel = new Mat())
               {
                  //process the image (RGBA) here, replace the following with your code
                  CvInvoke.ExtractChannel(m, alphaChannel, 3);//extract the alphaChannel
                  CvInvoke.BitwiseNot(m, m); //simple inversion, invert all channels including alpha
                  CvInvoke.InsertChannel(alphaChannel, m, 3); //put the alphaChannel back
               }
            }
            else if (m_PixelFormat == Image.PIXEL_FORMAT.RGB888)
            {
               using (
                  Mat m = new Mat(new Size(image.Width, image.Height), DepthType.Cv8U, 3,
                     pixelHandle.AddrOfPinnedObject(), image.Stride))
               {
                  //process the image (RGB) here, replace the following with your code.
                  CvInvoke.BitwiseNot(m, m);
               }
            }
            else
            {
               string s = String.Format("Image type {0} is not supported\n", m_PixelFormat);
               s += "  size: " + image.Width + "x" + image.Height + "\n";
               s += "  bufferSize: " + image.BufferWidth + "x" + image.BufferHeight + "\n";
               s += "  stride: " + image.Stride;
               Debug.Log(s);
            }
         }
         finally
         {
            pixelHandle.Free();
         }


      }
   }
}
