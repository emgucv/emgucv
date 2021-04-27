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

public class Ocr : MonoBehaviour
{
    private TesseractModel _ocrModel;

    // Use this for initialization
    void Start()
    {
        _ocrModel = new TesseractModel();
        StartCoroutine(workflow());
    }

    private IEnumerator workflow()
    {
        yield return _ocrModel.Init(DownloadManager_OnDownloadProgressChanged, null);
        Debug.Log("OCR engine loaded.");
        yield return performOCR();
    }

    private static String ByteToSizeStr(long byteCount)
    {
        if (byteCount < 1024)
        {
            return String.Format("{0} B", byteCount);
        }
        else if (byteCount < 1024 * 1024)
        {
            return String.Format("{0} KB", byteCount / 1024);
        }
        else
        {
            return String.Format("{0} MB", byteCount / (1024 * 1024));
        }
    }

    protected void DownloadManager_OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
    {
        String msg;
        if (e.TotalBytesToReceive > 0)
            msg = String.Format("{0} of {1} downloaded ({2}%)", ByteToSizeStr(e.BytesReceived), ByteToSizeStr(e.TotalBytesToReceive), e.ProgressPercentage);
        else
            msg = String.Format("{0} downloaded", ByteToSizeStr(e.BytesReceived));
        updateTextureWithString(msg);
    }
    private IEnumerator performOCR()
    {
        Mat img = new Mat(new Size(480, 200), DepthType.Cv8U, 3);
        img.SetTo(new MCvScalar());
        String message = "Hello, World";
        CvInvoke.PutText(img, message, new Point(50, 100), Emgu.CV.CvEnum.FontFace.HersheyPlain, 2.0, new MCvScalar(255, 255, 255), 2);
        Mat imgOut = new Mat();
        String outMessage = _ocrModel.ProcessAndRender(img, imgOut);
        
        Debug.Log(outMessage);
        //updateTextureWithString(outMessage);

        Texture2D texture = TextureConvert.InputArrayToTexture2D(imgOut, FlipType.Vertical);

        RenderTexture(texture);
        ResizeTexture(texture);
        yield return null;
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
