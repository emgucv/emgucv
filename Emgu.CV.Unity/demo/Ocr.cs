//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System.IO;
using Emgu.CV.CvEnum;
using UnityEngine;
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

#if NETFX_CORE && (!UNITY_EDITOR) 
using System.Threading.Tasks;
using System.Net.Http;
#else
using System.Security.Cryptography.X509Certificates;
#endif
using Emgu.CV.OCR;

public class Ocr : MonoBehaviour
{
    private Tesseract _ocr;

#if NETFX_CORE && (!UNITY_EDITOR)
    public static async Task DownloadAsync(Uri requestUri, string filename)
    {
        if (filename == null)
            throw new ArgumentNullException("filename");

        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                using (
                    Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
                    stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 2048, true))
                {
                    await contentStream.CopyToAsync(stream);
                }
            }
        }
    }

    private static void TesseractDownloadLangFile(String folder, String lang)
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
            String source =
                    String.Format("https://github.com/tesseract-ocr/tessdata/blob/4592b8d453889181e01982d22328b5846765eaad/{0}.traineddata?raw=true", lang);

            Task t = DownloadAsync(new Uri(source), dest);
            t.Wait();
        }
    }
#else
    private static void TesseractDownloadLangFile(String folder, String lang)
    {
        String subfolderName = "tessdata";
        String folderName = System.IO.Path.Combine(folder, subfolderName);
        if (!System.IO.Directory.Exists(folderName))
        {
            System.IO.Directory.CreateDirectory(folderName);
        }
        String dest = System.IO.Path.Combine(folderName, String.Format("{0}.traineddata", lang));

        if (!System.IO.File.Exists(dest) || !(new FileInfo(dest).Length > 0))
            using (System.Net.WebClient webclient = new System.Net.WebClient())
            {
                String source =
                    String.Format("https://github.com/tesseract-ocr/tessdata/blob/4592b8d453889181e01982d22328b5846765eaad/{0}.traineddata?raw=true", lang);

                Debug.Log(String.Format("Downloading file from '{0}' to '{1}'", source, dest));
                webclient.DownloadFile(source, dest);
                Debug.Log(String.Format("Download completed"));
            }
    }
#endif

    // Use this for initialization
    void Start()
    {
#if !( NETFX_CORE && (!UNITY_EDITOR) )
        //Warning: The following code is used to get around a https certification issue for downloading tesseract language files from Github
        //Do not use this code in a production environment. Please make sure you understand the security implication from the following code before using it
        ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            HttpWebRequest webRequest = sender as HttpWebRequest;
            if (webRequest != null)
            {
                String requestStr = webRequest.Address.AbsoluteUri;
                if (requestStr.StartsWith(@"https://github.com/") || requestStr.StartsWith(@"https://raw.githubusercontent.com/"))
                    return true;
            }
            return false;
        };
#endif
        TesseractDownloadLangFile(Application.persistentDataPath, "eng");
        TesseractDownloadLangFile(Application.persistentDataPath, "osd"); //script orientation detection


        _ocr = new Tesseract(Path.Combine(Application.persistentDataPath, "tessdata"), "eng", OcrEngineMode.TesseractLstmCombined);

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

        this.GetComponent<GUITexture>().texture = texture;
        this.GetComponent<GUITexture>().pixelInset = new Rect(-img.Width / 2, -img.Height / 2, img.Width, img.Height);
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
