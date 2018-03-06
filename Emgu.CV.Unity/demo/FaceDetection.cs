//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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

      UMat img = new UMat();
      TextureConvert.Texture2dToOutputArray(lenaTexture, img);
      CvInvoke.Flip(img, img, FlipType.Vertical);

      //String fileName = "haarcascade_frontalface_default";
      //String fileName = "lbpcascade_frontalface";
      String fileName = "haarcascade_frontalface_alt2";
      String filePath = Path.Combine(Application.persistentDataPath, fileName + ".xml");
      //if (!File.Exists(filePath))
      {
         TextAsset cascadeModel = Resources.Load<TextAsset>(fileName);
         
#if UNITY_METRO
         UnityEngine.Windows.File.WriteAllBytes(filePath, cascadeModel.bytes);
#else
         File.WriteAllBytes(filePath, cascadeModel.bytes);
#endif
      }

      using (CascadeClassifier classifier = new CascadeClassifier(filePath))
      using (UMat gray = new UMat())
      {
         CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

         Rectangle[] faces = null;
         try
         {
            faces = classifier.DetectMultiScale(gray);

            foreach (Rectangle face in faces)
            {
               CvInvoke.Rectangle(img, face, new MCvScalar(0, 255, 0));
            }
         }
         catch (Exception e)
         {
            Debug.Log(e.Message);
            
            return;
         }
      }

      Texture2D texture = TextureConvert.InputArrayToTexture2D(img, FlipType.Vertical);

      this.GetComponent<GUITexture>().texture = texture;
      Size s = img.Size;
      this.GetComponent<GUITexture>().pixelInset = new Rect(-s.Width / 2, -s.Height / 2, s.Width, s.Height);
   }


   // Update is called once per frame
   void Update()
   {

   }
}
