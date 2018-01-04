//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Ocl;

public class FeatureMatching : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        Texture2D boxTexture = Resources.Load<Texture2D>("box");
        Texture2D boxInSceneTexture = Resources.Load<Texture2D>("box_in_scene");

        Mat box3Channels = new Mat();
        TextureConvert.Texture2dToOutputArray(boxTexture, box3Channels);
        Mat box = new Mat();
        CvInvoke.CvtColor(box3Channels, box, ColorConversion.Bgra2Gray);
        CvInvoke.Flip(box, box, FlipType.Vertical);

        Mat boxInScene3Channels = new Mat();
        TextureConvert.Texture2dToOutputArray(boxInSceneTexture, boxInScene3Channels);
        Mat boxInScene = new Mat();
        CvInvoke.CvtColor(boxInScene3Channels, boxInScene, ColorConversion.Bgra2Gray);
        CvInvoke.Flip(boxInScene, boxInScene, FlipType.Vertical);

        long time;
        Mat img = FeatureMatchingExample.DrawMatches.Draw(box, boxInScene, out time);
        //CvInvoke.Imwrite("c:\\tmp\\tmp.png", img);
        //Mat outImg = new Mat();
        //CvInvoke.CvtColor(img, outImg, ColorConversion.Bgr2Bgra);
        //CvInvoke.Imwrite("c:\\tmp\\tmp.png", outImg);
        Texture2D texture = TextureConvert.InputArrayToTexture2D(img);

        this.GetComponent<GUITexture>().texture = texture;
        
        this.GetComponent<GUITexture>().pixelInset = new Rect(-texture.width / 2, -texture.height / 2, texture.width, texture.height);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
