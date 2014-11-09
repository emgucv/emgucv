using System.IO;
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

public class HelloTexture : MonoBehaviour
{

   // Use this for initialization
   void Start()
   {      
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

      CvInvoke.PutText(img, String.Format("Emgu CV for Unity {0}", Emgu.Util.Platform.OperationSystem), new System.Drawing.Point(10, 60), Emgu.CV.CvEnum.FontFace.HersheyDuplex,
                       1.0, new MCvScalar(0, 255, 0));

      CvInvoke.PutText(img, String.Format("OpenCL: {0}",openclStr), new System.Drawing.Point(10, 120), Emgu.CV.CvEnum.FontFace.HersheyDuplex,
                       1.0, new MCvScalar(0, 0, 255));

      Texture2D texture = TextureConvert.ImageToTexture2D(img, FlipType.Vertical);

      this.guiTexture.texture = texture;
      this.guiTexture.pixelInset = new Rect(-img.Width / 2, -img.Height / 2, img.Width, img.Height);
      
   }

   // Update is called once per frame
   void Update()
   {

   }
}
