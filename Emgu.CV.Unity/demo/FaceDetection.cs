using System.IO;
using System.Linq;
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

public class FaceDetection : MonoBehaviour
{

   // Use this for initialization
   void Start()
   {  
      Texture2D lenaTexture = Resources.Load<Texture2D>("lena");    

      //updateTextureWithString("load lena ok");
      UMat img = new UMat();
      TextureConvert.Texture2dToOutputArray(lenaTexture, img);
      CvInvoke.Flip(img, img, FlipType.Vertical);

      //updateTextureWithString("convert to image ok");

      //String fileName = "haarcascade_frontalface_default";
      //String fileName = "lbpcascade_frontalface";
      String fileName = "haarcascade_frontalface_alt2";
      String filePath = Path.Combine(Application.persistentDataPath, fileName + ".xml");
      //if (!File.Exists(filePath))
      {
         //updateTextureWithString("start move cascade xml");
         TextAsset cascadeModel = Resources.Load<TextAsset>(fileName);
         
#if UNITY_METRO
         UnityEngine.Windows.File.WriteAllBytes(filePath, cascadeModel.bytes);
#else
         File.WriteAllBytes(filePath, cascadeModel.bytes);
#endif
         //updateTextureWithString("File size: " + new FileInfo(filePath).Length);
      }

      
      using (CascadeClassifier classifier = new CascadeClassifier(filePath))
      using (UMat gray = new UMat())
      {
         CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
         //updateTextureWithString("classifier create ok");

         Rectangle[] faces = null;
         try
         {
            faces = classifier.DetectMultiScale(gray);

            //updateTextureWithString("face detected");
            foreach (Rectangle face in faces)
            {
               CvInvoke.Rectangle(img, face, new MCvScalar(0, 255, 0));
            }
         }
         catch (Exception e)
         {
            
            //updateTextureWithString(e.Message);
            return;
         }
         
         //updateTextureWithString(String.Format("{0} face found on image of {1} x {2}", faces.Length, img.Width, img.Height));
      }

      Texture2D texture = TextureConvert.InputArrayToTexture2D(img, FlipType.Vertical);

      this.GetComponent<GUITexture>().texture = texture;
      Size s = img.Size;
      this.GetComponent<GUITexture>().pixelInset = new Rect(-s.Width / 2, -s.Height / 2, s.Width, s.Height);
   }

   private void updateTextureWithString(String text)
   {
      Image<Bgr, Byte> img = new Image<Bgr, byte>(640, 240);
      CvInvoke.PutText(img, text, new System.Drawing.Point(10, 60), Emgu.CV.CvEnum.FontFace.HersheyDuplex,
                       1.0, new MCvScalar(0, 255, 0));

      Texture2D texture = TextureConvert.ImageToTexture2D(img);

      this.GetComponent<GUITexture>().texture = texture;
   }

   // Update is called once per frame
   void Update()
   {

   }
}
