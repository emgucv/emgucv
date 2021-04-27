//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using UnityEngine.UI;

public class FeatureMatching : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Texture2D boxTexture = Resources.Load<Texture2D>("box");
        Texture2D boxInSceneTexture = Resources.Load<Texture2D>("box_in_scene");

        Mat box = new Mat();
        boxTexture.ToOutputArray(box, FlipType.Vertical, typeof(Gray));

        Mat boxInScene = new Mat();
        boxInSceneTexture.ToOutputArray(boxInScene, FlipType.Vertical, typeof(Gray));
        
        long time;
        Mat img = FeatureMatchingExample.DrawMatches.Draw(box, boxInScene, out time);
        
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
