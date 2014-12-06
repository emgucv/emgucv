
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
   private Texture2D resultTexture;
   private Color32[] data;
   private byte[] bytes;
   private WebCamDevice[] devices;
   public int cameraCount = 0;
   private bool _textureResized = false;
   private Quaternion baseRotation;
   // Use this for initialization
   void Start()
   {
      WebCamDevice[] devices = WebCamTexture.devices;
      int cameraCount = devices.Length;

      if (cameraCount == 0)
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(640, 240);
         CvInvoke.PutText(img, String.Format("{0} camera found", devices.Length), new System.Drawing.Point(10, 60),
            Emgu.CV.CvEnum.FontFace.HersheyDuplex,
            1.0, new MCvScalar(0, 255, 0));
         Texture2D texture = TextureConvert.ImageToTexture2D(img, FlipType.Vertical);

         this.guiTexture.texture = texture;
         this.guiTexture.pixelInset = new Rect(-img.Width/2, -img.Height/2, img.Width, img.Height);
      }
      else
      {
         webcamTexture = new WebCamTexture(devices[0].name);

         baseRotation = transform.rotation;
         webcamTexture.Play();
         //data = new Color32[webcamTexture.width * webcamTexture.height];
         CvInvoke.CheckLibraryLoaded();
      }
   }

   private FlipType flip = FlipType.None;
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

         if (bytes == null || bytes.Length != data.Length*3)
         {
            bytes = new byte[data.Length*3];
         }
         GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
         GCHandle resultHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
         using (Image<Bgra, byte> image = new Image<Bgra, byte>(webcamTexture.width, webcamTexture.height, webcamTexture.width * 4, handle.AddrOfPinnedObject()))
         using (Mat bgr = new Mat(webcamTexture.height, webcamTexture.width, DepthType.Cv8U, 3, resultHandle.AddrOfPinnedObject(), webcamTexture.width * 3))
         {
            CvInvoke.CvtColor(image, bgr, ColorConversion.Bgra2Bgr);
            CvInvoke.BitwiseNot(bgr, bgr);
            if (flip != FlipType.None)
               CvInvoke.Flip(bgr, bgr, flip);
         }
         handle.Free();
         resultHandle.Free();
         if (resultTexture == null || resultTexture.width != webcamTexture.width ||
             resultTexture.height != webcamTexture.height)
         {
            resultTexture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);
         }
         
         resultTexture.LoadRawTextureData(bytes);
         resultTexture.Apply();

         if (!_textureResized)
         {
            this.guiTexture.pixelInset = new Rect(-webcamTexture.width / 2, -webcamTexture.height / 2, webcamTexture.width, webcamTexture.height);
            _textureResized = true;
         }
         transform.rotation = baseRotation * Quaternion.AngleAxis(webcamTexture.videoRotationAngle, Vector3.up);
         this.guiTexture.texture = resultTexture;
         //count++;

      }
   }
}
