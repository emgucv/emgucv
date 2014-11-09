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
   public WebCamTexture webcamTexture;
   public Color32[] data;

   // Use this for initialization
   void Start()
   {
      webcamTexture = new WebCamTexture();
      
      webcamTexture.Play();
      data = new Color32[webcamTexture.width * webcamTexture.height];
      CvInvoke.CheckLibraryLoaded();

   }

   private bool _textureResized = false;

   // Update is called once per frame
   void Update()
   {
      
      webcamTexture.GetPixels32(data);
      GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
      using (
         Image<Bgra, byte> image = new Image<Bgra, byte>(webcamTexture.width, webcamTexture.height,
            webcamTexture.width*4, handle.AddrOfPinnedObject()))
      using (Image<Bgr, Byte> bgr = image.Convert<Bgr, Byte>())
      {
         CvInvoke.BitwiseNot(bgr, bgr);
         Texture2D texture = TextureConvert.ImageToTexture2D(bgr, Emgu.CV.CvEnum.FlipType.None);

         if (!_textureResized)
         {
            this.guiTexture.pixelInset = new Rect(-bgr.Width / 2, -bgr.Height / 2, bgr.Width, bgr.Height);
            _textureResized = true;
         }
         this.guiTexture.texture = texture;
         
      }
      handle.Free();
   }
}
