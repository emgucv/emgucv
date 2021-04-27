//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Models;


public class FaceDetection : ProcessAndRenderModelBehaviour
{
    // Use this for initialization
    void Start()
    {
        _model = new FaceAndLandmarkDetector();
        StartCoroutine(Workflow());
    }

    /// <summary>
    /// Get the input image
    /// </summary>
    /// <returns>The input image</returns>
    public override Mat GetInputMat()
    {
        Texture2D lenaTexture = Resources.Load<Texture2D>("lena");
        Mat imgBgr = new Mat();
        lenaTexture.ToOutputArray(imgBgr);

        return imgBgr;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
