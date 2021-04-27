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
            tex.ToOutputArray(imgs[i]);
            
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

        Texture2D texture = result.ToTexture2D();

        RenderTexture(texture);
        ResizeTexture(texture);
    }

    private void updateTextureWithString(String text)
    {
        Mat img = new Mat(new Size(640, 240), DepthType.Cv8U, 3);
        CvInvoke.PutText(img, text, new System.Drawing.Point(10, 60), Emgu.CV.CvEnum.FontFace.HersheyDuplex,
                         1.0, new MCvScalar(0, 255, 0));

        Texture2D texture = img.ToTexture2D();

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
