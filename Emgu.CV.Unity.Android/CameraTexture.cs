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

public class ChangeTexture : MonoBehaviour
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
      /*
      Image<Bgr, Byte> img = new Image<Bgr, byte>(640, 240);

      String openclStr = "None";
      if (CvInvoke.HaveOpenCL)
      {
         //StringBuilder builder = new StringBuilder();
         using (VectorOfOclPlatformInfo oclPlatformInfos = OclInvoke.GetPlatformInfo())
         {
            if (oclPlatformInfos.Size > 0)
            {
               OclPlatformInfo platformInfo = oclPlatformInfos[0];
               openclStr = platformInfo.ToString();
            }
         }
      }

      CvInvoke.PutText(img, "Emgu CV for Unity Android", new System.Drawing.Point(10, 60), Emgu.CV.CvEnum.FontFace.HersheyDuplex,
                       1.0, new MCvScalar(0, 255, 0));

      CvInvoke.PutText(img, String.Format("OpenCL: {0}",openclStr), new System.Drawing.Point(10, 120), Emgu.CV.CvEnum.FontFace.HersheyDuplex,
                       1.0, new MCvScalar(0, 0, 255));

      Texture2D texture = TextureConvert.ImageToTexture2D(img);

      GUITexture[] textures = GameObject.FindObjectsOfType(typeof(GUITexture)) as GUITexture[];
      foreach (GUITexture t in textures)
         t.texture = texture;*/
   }

   // Update is called once per frame
   void Update()
   {
      /*
      GUITexture[] textures = GameObject.FindObjectsOfType(typeof(GUITexture)) as GUITexture[];
      foreach (GUITexture t in textures)
         t.texture = webcamTexture;*/
      
      webcamTexture.GetPixels32(data);
      GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
      using (
         Image<Bgra, byte> image = new Image<Bgra, byte>(webcamTexture.width, webcamTexture.height,
            webcamTexture.width*4, handle.AddrOfPinnedObject()))
      using (Image<Bgr, Byte> bgr = image.Convert<Bgr, Byte>())
      {
         CvInvoke.BitwiseNot(bgr, bgr);
         Texture2D texture = TextureConvert.ImageToTexture2D(bgr);
         this.guiTexture.texture = texture;
         //GUITexture t = GameObject.Find("MainTexture") as GUITexture;
         
         //t.texture = texture;
      }
      handle.Free();
   }
}
