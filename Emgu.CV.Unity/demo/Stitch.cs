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

public class Stitch : MonoBehaviour
{

   // Use this for initialization
   void Start()
   {  
		String[] textureNames = new string[] { "stitch1", "stitch2", "stitch3", "stitch4"};
		Mat[] imgs = new Mat[textureNames.Length];
		Mat tmp = new Mat ();
		for (int i = 0; i < textureNames.Length; i++) {
			Texture2D tex = Resources.Load<Texture2D>(textureNames[i]);
			imgs [i] = new Mat ();
			TextureConvert.Texture2dToOutputArray(tex, tmp);
			CvInvoke.Flip(tmp, tmp, FlipType.Vertical);
			CvInvoke.CvtColor (tmp, imgs [i], ColorConversion.Bgra2Bgr);
			if (imgs [i].IsEmpty)
				Debug.Log ("Image " + i + " is empty");
			else
				Debug.Log ("Image " + i + " is " + imgs[i].NumberOfChannels + " channels "  + imgs [i].Width + "x" + imgs [i].Height);
		}
		Emgu.CV.Stitching.Stitcher stitcher = new Emgu.CV.Stitching.Stitcher (false);
		Mat result = new Mat ();
		using (VectorOfMat vms = new VectorOfMat (imgs))
			stitcher.Stitch (vms, result);
		//CvInvoke.Flip(result, result, FlipType.Vertical);

		Texture2D texture = TextureConvert.InputArrayToTexture2D(result, FlipType.Vertical);

		this.GetComponent<GUITexture>().texture = texture;
		Size s = result.Size;
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
