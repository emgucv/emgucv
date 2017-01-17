//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


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
using Emgu.CV.OCR;

public class Ocr : MonoBehaviour
{
   private Tesseract _ocr;

   // Use this for initialization
   void Start()
   {
      String[] names = new string[] {"eng.cube.bigrams", "eng.cube.fold", "eng.cube.lm", "eng.cube.nn", "eng.cube.params", "eng.cube.size", "eng.cube.word-freq", "eng.tesseract_cube.nn", "eng.traineddata"};
      String outputPath = Path.Combine(Application.persistentDataPath, "tessdata");
      if (!Directory.Exists(outputPath))
         Directory.CreateDirectory(outputPath);
      
      foreach (String n in names)
      {
         TextAsset textAsset = Resources.Load<TextAsset>(Path.Combine("tessdata", n));  
         String filePath = Path.Combine(outputPath, n);
#if UNITY_METRO
         UnityEngine.Windows.File.WriteAllBytes(filePath, textAsset.bytes);
#else
         if (!File.Exists(filePath))
            File.WriteAllBytes(filePath, textAsset.bytes);
#endif
      }

      _ocr = new Tesseract(outputPath, "eng", OcrEngineMode.TesseractLstmCombined);

      Debug.Log("OCR engine loaded.");

      Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 200);
      
      String message = "Hello, World";
      CvInvoke.PutText(img, message, new Point(50, 100), Emgu.CV.CvEnum.FontFace.HersheySimplex, 1.0, new MCvScalar(255, 255, 255));

      _ocr.Recognize(img);


      Tesseract.Character[] characters = _ocr.GetCharacters();
      foreach (Tesseract.Character c in characters)
      {
         CvInvoke.Rectangle(img, c.Region, new MCvScalar(255, 0, 0));
      }

      String messageOcr = _ocr.GetText().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text   
      Debug.Log("Detected text: "+ message);

      Texture2D texture = TextureConvert.InputArrayToTexture2D(img, FlipType.Vertical);

      this.GetComponent<GUITexture>().texture = texture;
      this.GetComponent<GUITexture>().pixelInset = new Rect(-img.Width / 2, -img.Height / 2, img.Width, img.Height);
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
