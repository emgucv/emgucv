//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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

public class CameraTexture : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    private Texture2D resultTexture;
    private Color32[] data;
    private byte[] bytes;
    private WebCamDevice[] devices;
    public int cameraCount = 0;
    private bool _textureResized = false;
    private Quaternion baseRotation;

    // Use this for initialization
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        int cameraCount = devices.Length;

        if (cameraCount == 0)
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(640, 240);
            CvInvoke.PutText(img, String.Format("{0} camera found", devices.Length), new System.Drawing.Point(10, 60),
               Emgu.CV.CvEnum.FontFace.HersheyDuplex,
               1.0, new MCvScalar(0, 255, 0));
            Texture2D texture = TextureConvert.ImageToTexture2D(img, FlipType.Vertical);

            RenderTexture(texture);
            ResizeTexture(texture);
            //this.GetComponent<GUITexture>().texture = texture;
            //this.GetComponent<GUITexture>().pixelInset = new Rect(-img.Width/2, -img.Height/2, img.Width, img.Height);
        }
        else
        {
            webcamTexture = new WebCamTexture(devices[0].name);

            baseRotation = transform.rotation;
            webcamTexture.Play();
            //data = new Color32[webcamTexture.width * webcamTexture.height];
            CvInvoke.CheckLibraryLoaded();
        }
    }

    private FlipType flip = FlipType.None;
    // Update is called once per frame
    void Update()
    {
        if (webcamTexture != null && webcamTexture.didUpdateThisFrame)
        {
            if (data == null || (data.Length != webcamTexture.width * webcamTexture.height))
            {
                data = new Color32[webcamTexture.width * webcamTexture.height];
            }
            webcamTexture.GetPixels32(data);

            if (bytes == null || bytes.Length != data.Length * 3)
            {
                bytes = new byte[data.Length * 3];
            }
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            GCHandle resultHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            using (Mat bgra = new Mat(new Size(webcamTexture.width, webcamTexture.height), DepthType.Cv8U, 4, handle.AddrOfPinnedObject(), webcamTexture.width * 4))
            using (Mat bgr = new Mat(webcamTexture.height, webcamTexture.width, DepthType.Cv8U, 3, resultHandle.AddrOfPinnedObject(), webcamTexture.width * 3))
            {
                CvInvoke.CvtColor(bgra, bgr, ColorConversion.Bgra2Bgr);

                #region do some image processing here

                CvInvoke.BitwiseNot(bgr, bgr);

                #endregion

                if (flip != FlipType.None)
                    CvInvoke.Flip(bgr, bgr, flip);
            }
            handle.Free();
            resultHandle.Free();
            if (resultTexture == null || resultTexture.width != webcamTexture.width ||
                resultTexture.height != webcamTexture.height)
            {
                resultTexture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);
            }

            resultTexture.LoadRawTextureData(bytes);
            resultTexture.Apply();

            if (!_textureResized)
            {
                //this.GetComponent<GUITexture>().pixelInset = new Rect(-webcamTexture.width / 2, -webcamTexture.height / 2, webcamTexture.width, webcamTexture.height);
                ResizeTexture(resultTexture);
                _textureResized = true;
            }
            transform.rotation = baseRotation * Quaternion.AngleAxis(webcamTexture.videoRotationAngle, Vector3.up);
            //this.GetComponent<GUITexture>().texture = resultTexture;
            RenderTexture(resultTexture);
            //count++;

        }
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

}
