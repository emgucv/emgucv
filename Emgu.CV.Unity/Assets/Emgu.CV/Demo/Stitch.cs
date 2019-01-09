using System.IO;
using System.Linq;
using Emgu.CV.CvEnum;
using UnityEngine;
using UnityEngine.UI;
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
        String[] textureNames = new string[] { "stitch1", "stitch2", "stitch3", "stitch4" };
        Mat[] imgs = new Mat[textureNames.Length];
        Mat tmp = new Mat();
        for (int i = 0; i < textureNames.Length; i++)
        {
            Texture2D tex = Resources.Load<Texture2D>(textureNames[i]);
            imgs[i] = new Mat();
            TextureConvert.Texture2dToOutputArray(tex, tmp);
            CvInvoke.Flip(tmp, tmp, FlipType.Vertical);
            CvInvoke.CvtColor(tmp, imgs[i], ColorConversion.Bgra2Bgr);
            if (imgs[i].IsEmpty)
                Debug.Log("Image " + i + " is empty");
            else
                Debug.Log("Image " + i + " is " + imgs[i].NumberOfChannels + " channels " + imgs[i].Width + "x" + imgs[i].Height);
        }
        Emgu.CV.Stitching.Stitcher stitcher = new Emgu.CV.Stitching.Stitcher();
        Mat result = new Mat();
        using (VectorOfMat vms = new VectorOfMat(imgs))
            stitcher.Stitch(vms, result);
        //CvInvoke.Flip(result, result, FlipType.Vertical);

        Texture2D texture = TextureConvert.InputArrayToTexture2D(result, FlipType.Vertical);

        RenderTexture(texture);
        ResizeTexture(texture);
    }

    private void updateTextureWithString(String text)
    {
        Image<Bgr, Byte> img = new Image<Bgr, byte>(640, 240);
        CvInvoke.PutText(img, text, new System.Drawing.Point(10, 60), Emgu.CV.CvEnum.FontFace.HersheyDuplex,
                         1.0, new MCvScalar(0, 255, 0));

        Texture2D texture = TextureConvert.ImageToTexture2D(img);

        RenderTexture(texture);
        ResizeTexture(texture);
        
    }
    private void RenderTexture(Texture2D texture)
    {
        Image image = this.GetComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void ResizeTexture(Texture2D texture)
    {
        Image image = this.GetComponent<Image>();
        var transform = image.rectTransform;
        transform.sizeDelta = new Vector2(texture.width, texture.height);
        transform.position = new Vector3(-texture.width / 2, -texture.height / 2);
        transform.anchoredPosition = new Vector2(0, 0);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
