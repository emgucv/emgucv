//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// BarcodeDetector model
    /// </summary>
    public class BarcodeDetectorModel : DisposableObject, IProcessAndRenderModel
    {
        private String _modelFolderName = "wechat";

        private BarcodeDetector _barcodeDetector = null;

        /// <summary>
        /// Download and initialize the BarcodeDetector model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <returns>Asyn task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_barcodeDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://github.com/WeChatCV/opencv_3rdparty/raw/wechat_qrcode/sr.prototxt",
                    _modelFolderName,
                    "8AE41ACBA97E8B4A8E741EE350481E49B8E01D787193F470A4C95EE1C02D5B61");

                manager.AddFile(
                    "https://github.com/WeChatCV/opencv_3rdparty/raw/wechat_qrcode/sr.caffemodel",
                    _modelFolderName,
                    "E5D36889D8E6EF2F1C1F515F807CEC03979320AC81792CD8FB927C31FD658AE3");

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _barcodeDetector = new BarcodeDetector(
                        manager.Files[0].LocalFile, 
                        manager.Files[1].LocalFile
                        );
                }
            }
        }
        
        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_barcodeDetector != null)
            {
                _barcodeDetector.Dispose();
                _barcodeDetector = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this WeChatQRCode detector.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }


        /// <summary>
        /// Download and initialize the WeChatQRCode model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <param name="initOptions">A string, not used right now.</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#endif
        {

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
            yield return Init(onDownloadProgressChanged);
#else
            await Init(onDownloadProgressChanged);
#endif
        }

        private static Point[] MatToPoints(Mat m)
        {
            PointF[] points = new PointF[ m.Width * m.Height / 2 ];
            GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
            Emgu.CV.Util.CvToolbox.Memcpy( handle.AddrOfPinnedObject(), m.DataPointer,points.Length * Marshal.SizeOf<PointF>());
            handle.Free();
            return Array.ConvertAll(points, Point.Round);
        }

        private MCvScalar _renderColor = new MCvScalar(255, 0, 0);

        /// <summary>
        /// Get or Set the color used in rendering.
        /// </summary>
        public MCvScalar RenderColor
        {
            get
            {
                return _renderColor;
            }
            set
            {
                _renderColor = value;
            }
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">
        /// The output image, can be the same as <paramref name="imageIn"/>, in which case we will render directly into the input image.
        /// Note that if no bar codes are detected, <paramref name="imageOut"/> will remain unchanged.
        /// If bar codes are detected, we will draw the code and (rectangle) regions on top of the existing pixels of <paramref name="imageOut"/>.
        /// If the <paramref name="imageOut"/> is not the same object as <paramref name="imageIn"/>, it is a good idea to copy the pixels over from the input image before passing it to this function.
        /// </param>
        /// <returns>The messages that we want to display.</returns>
        public String ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            using (VectorOfMat points = new VectorOfMat())
            {
                Stopwatch watch = Stopwatch.StartNew();
                var barcodesFound = _barcodeDetector.DetectAndDecode(imageIn);
                watch.Stop();

                for (int i = 0; i < barcodesFound.Length; i++)
                {
                    Point[] contour = Array.ConvertAll(barcodesFound[i].Points, Point.Round);

                    using (VectorOfVectorOfPoint vpp = new VectorOfVectorOfPoint(new Point[][] { contour }))
                    {
                        CvInvoke.DrawContours(imageOut, vpp, -1, RenderColor);
                    }

                    CvInvoke.PutText(
                        imageOut, 
                        barcodesFound[i].DecodedInfo, 
                        Point.Round( barcodesFound[i].Points[0]),
                        FontFace.HersheySimplex,
                        1.0,
                        RenderColor
                        );
                }


                if (barcodesFound.Length == 0)
                {
                    return String.Format("No barcodes found (in {0} milliseconds)", watch.ElapsedMilliseconds);
                } 

                String[] barcodesTexts = Array.ConvertAll(barcodesFound,
                    delegate(BarcodeDetector.Barcode input) { return input.DecodedInfo; });
                String allBarcodeText = String.Join(";", barcodesTexts);
                return String.Format(
                    "Barcodes found (in {1} milliseconds): {0}", 
                    allBarcodeText, 
                    watch.ElapsedMilliseconds);
            }
        }
    }
}
