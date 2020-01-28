//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
using UnityEngine.UI;

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

        ResizeTexture(texture);
        RenderTexture(texture);
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
