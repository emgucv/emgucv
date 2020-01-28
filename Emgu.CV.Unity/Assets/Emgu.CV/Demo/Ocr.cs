//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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

using Emgu.CV.OCR;

public class Ocr : MonoBehaviour
{
    private Tesseract _ocr;

    private static IEnumerator TesseractDownloadLangFile(String folder, String lang)
    {
        String subfolderName = "tessdata";
        String folderName = System.IO.Path.Combine(folder, subfolderName);
        if (!System.IO.Directory.Exists(folderName))
        {
            System.IO.Directory.CreateDirectory(folderName);
        }
        String dest = System.IO.Path.Combine(folderName, String.Format("{0}.traineddata", lang));


        if (!System.IO.File.Exists(dest) || !(new FileInfo(dest).Length > 0))
        {
            String source = Tesseract.GetLangFileUrl(lang);
            using (UnityEngine.Networking.UnityWebRequest webclient = new UnityEngine.Networking.UnityWebRequest(source))
            {
                Debug.Log(String.Format("Downloading file from '{0}' to '{1}'", source, dest));

                webclient.downloadHandler = new UnityEngine.Networking.DownloadHandlerFile(dest);
                yield return webclient.SendWebRequest();
                if (webclient.isNetworkError || webclient.isHttpError)
                {
                    Debug.LogError(webclient.error);
                }

                if (!System.IO.File.Exists(dest) || !(new FileInfo(dest).Length > 0))
                {
                    Debug.LogError(String.Format("File {0} is empty, failed to download file.", dest));
                }

                Debug.Log("File successfully downloaded and saved to " + dest);
                //Debug.Log(String.Format("Download completed"));
            }
        }
    }
//#endif

    // Use this for initialization
    void Start()
    {
        StartCoroutine(workflow());
    }

    private IEnumerator workflow()
    {
        yield return TesseractDownloadLangFile(Application.persistentDataPath, "eng");
        yield return TesseractDownloadLangFile(Application.persistentDataPath, "osd"); //script orientation detection
        yield return performOCR();
    }

    private IEnumerator performOCR()
    {
        _ocr = new Tesseract(Path.Combine(Application.persistentDataPath, "tessdata"), "eng", OcrEngineMode.TesseractOnly);

        Debug.Log("OCR engine loaded.");

        Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 200);

        String message = "Hello, World";
        CvInvoke.PutText(img, message, new Point(50, 100), Emgu.CV.CvEnum.FontFace.HersheySimplex, 1.0, new MCvScalar(255, 255, 255));

        _ocr.SetImage(img);
        _ocr.Recognize();

        Tesseract.Character[] characters = _ocr.GetCharacters();
        foreach (Tesseract.Character c in characters)
        {
            CvInvoke.Rectangle(img, c.Region, new MCvScalar(255, 0, 0));
        }

        String messageOcr = _ocr.GetUTF8Text().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text   
        Debug.Log("Detected text: " + message);

        Texture2D texture = TextureConvert.InputArrayToTexture2D(img, FlipType.Vertical);

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
