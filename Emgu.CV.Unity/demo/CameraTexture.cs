using Emgu.CV.CvEnum;
using UnityEngine;
using System;
using System.Drawing;
using System.Collections;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;

public class CameraTexture : MonoBehaviour
{
   private WebCamTexture webcamTexture;
   private Color32[] data;
   private Mat bgr;
   private WebCamDevice[] devices;
   public int cameraCount = 0;
   private bool _textureResized = false;

   // Use this for initialization
   void Start()
   {
      WebCamDevice[] devices = WebCamTexture.devices;
      int cameraCount = devices.Length;

      if (cameraCount == 0)
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(640, 240);
         CvInvoke.PutText(img, String.Format("{0} camera found", devices.Length), new System.Drawing.Point(10, 60), Emgu.CV.CvEnum.FontFace.HersheyDuplex,
              1.0, new MCvScalar(0, 255, 0));
         Texture2D texture = TextureConvert.ImageToTexture2D(img, FlipType.Vertical);

         this.guiTexture.texture = texture;
         this.guiTexture.pixelInset = new Rect(-img.Width / 2, -img.Height / 2, img.Width, img.Height);
      }
      else
      {
         webcamTexture = new WebCamTexture(devices[0].name);


         webcamTexture.Play();
         //data = new Color32[webcamTexture.width * webcamTexture.height];
         CvInvoke.CheckLibraryLoaded();
      }
      bgr = new Mat();
   }


   // Update is called once per frame
   void Update()
   {
      if (webcamTexture != null && webcamTexture.didUpdateThisFrame)
      {

         if (data == null || (data.Length != webcamTexture.width * webcamTexture.height))
         {
            data = new Color32[webcamTexture.width * webcamTexture.height];
         }
         webcamTexture.GetPixels32(data);
         GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
         using (Image<Bgra, byte> image = new Image<Bgra, byte>(webcamTexture.width, webcamTexture.height, webcamTexture.width * 4, handle.AddrOfPinnedObject()))
         {
            CvInvoke.CvtColor(image, bgr, ColorConversion.Bgra2Bgr);
            CvInvoke.BitwiseNot(bgr, bgr);
            Texture2D texture = TextureConvert.ToTexture2D(bgr, false);

            if (!_textureResized)
            {
               this.guiTexture.pixelInset = new Rect(-bgr.Width / 2, -bgr.Height / 2, bgr.Width, bgr.Height);
               _textureResized = true;
            }
            this.guiTexture.texture = texture;

         }
         handle.Free();
         //count++;

      }
   }
}
