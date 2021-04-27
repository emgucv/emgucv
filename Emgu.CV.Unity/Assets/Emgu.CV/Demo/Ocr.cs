//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System.IO;
using Emgu.CV.CvEnum;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Drawing;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Models;
using Emgu.CV.OCR;

public class Ocr : ProcessAndRenderModelBehaviour
{
    
    // Use this for initialization
    void Start()
    {
        _model = new TesseractModel();
        StartCoroutine(Workflow());
    }
    
    /// <summary>
    /// Get the input image
    /// </summary>
    /// <returns>The input image</returns>
    public override Mat GetInputMat()
    {
        Mat img = new Mat(new Size(480, 200), DepthType.Cv8U, 3);
        img.SetTo(new MCvScalar());
        String message = "Hello, World";
        CvInvoke.PutText(img, message, new Point(50, 100), Emgu.CV.CvEnum.FontFace.HersheyPlain, 2.0, new MCvScalar(255, 255, 255), 2);
        
        return img;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
