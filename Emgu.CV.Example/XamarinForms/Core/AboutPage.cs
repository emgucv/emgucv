//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Cuda;
using Emgu.CV.Dnn;
using Emgu.CV.ML;
using Xamarin.Forms;

namespace Emgu.CV.XamarinForms
{
    public class AboutPage : ContentPage
    {
        public AboutPage()
        {

            String lineBreak = "<br/>";

            String openclTxt = String.Format("Has OpenCL: {0}", CvInvoke.HaveOpenCL);
            if (CvInvoke.HaveOpenCL)
            {
                openclTxt = String.Format("{0}{1}Use OpenCL: {2}{1}<textarea rows=\"5\">{3}</textarea>{1}",
                   openclTxt, lineBreak,
                   CvInvoke.UseOpenCL,
                   CvInvoke.OclGetPlatformsSummary());
            }

            String cudaTxt = String.Format("Has CUDA: {0}", CudaInvoke.HasCuda);
            if (CudaInvoke.HasCuda)
            {
                cudaTxt = String.Format("{0}{1}<textarea rows=\"5\">{2}</textarea>{1}",
                    cudaTxt, 
                    lineBreak,
                    CudaInvoke.GetCudaDevicesSummary());
            }

            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveDNN = (openCVConfigDict["HAVE_OPENCV_DNN"] != 0);
            String dnnText;
            if (haveDNN)
            {
                var dnnBackends = DnnInvoke.AvailableBackends;
                List<String> dnnBackendsText = new List<string>();
                foreach (var dnnBackend in dnnBackends)
                {
                    dnnBackendsText.Add(String.Format("<p>{0} - {1}</p>", dnnBackend.Backend, dnnBackend.Target));
                }

                dnnText = String.Join("", dnnBackendsText.ToArray());
            }
            else
            {
                dnnText = "DNN not available on this platform";
            }

            bool haveVideoio = (openCVConfigDict["HAVE_OPENCV_VIDEOIO"] != 0);

            String osDescription = Emgu.Util.Platform.OperationSystem.ToString();

            String parallelText;
            List<String> parallelBackendText = new List<string>();
            foreach (var parallelBackend in CvInvoke.AvailableParallelBackends)
            {
                parallelBackendText.Add(String.Format("<p>{0}</p>", parallelBackend));
            }

            parallelText = String.Join("", parallelBackendText.ToArray());

            String tesseractText;
            String tesseractVersion = Emgu.CV.OCR.Tesseract.VersionString;
            if (tesseractVersion.Length == 0)
            {
                tesseractText = "Not Built";
            }
            else
            {
                tesseractText = String.Format("Version: {0}", tesseractVersion);
            }

            Content =
                  new WebView()
                  {
                      WidthRequest = 1000,
                      HeightRequest = 1000,
                      Source = new HtmlWebViewSource()
                      {
                          Html =
                        @"<html>
<head>
<style>body { background-color: #EEEEEE; }</style>
<style type=""text/css"">
textarea { width: 100%; margin: 0; padding: 0; border - width: 0; }
</style>
</head>
<body>
<H2> Emgu CV Examples </H2>
<a href=http://www.emgu.com>Visit our website</a> <br/><br/>
<a href=mailto:support@emgu.com>Email Support</a> <br/><br/>
<H4> OpenCL Info </H4>
" + openclTxt + @"
<H4> Cuda Info </H4>
" + cudaTxt + @"
<H4> OS: </H4>
" + osDescription + @"
<H4> OS Architecture: </H4>
" + RuntimeInformation.OSArchitecture + @"
<H4> Framework Description: </H4>
" + RuntimeInformation.FrameworkDescription + @"
<H4> Process Architecture: </H4>
" + RuntimeInformation.ProcessArchitecture + @"
<H4> Available Parallel Backends: </H4>
" + parallelText + @"
<H4> Dnn Backends: </H4>
" + dnnText + @"
<H4> Capture Backends (VideoCapture from device): </H4>
" + (haveVideoio ? GetBackendInfo(CvInvoke.Backends) : "Videoio backend not supported.") + @"
<H4> Stream Backends (VideoCapture from file/Stream): </H4>
" + (haveVideoio ? GetBackendInfo(CvInvoke.StreamBackends) : "Videoio backend not supported.") + @"
<H4> VideoWriter Backends: </H4>
" + (haveVideoio ? GetBackendInfo(CvInvoke.WriterBackends) : "Videoio backend not supported.") + @"
<H4> Tesseract OCR: </H4>
" + tesseractText + @"
<H4> Build Info </H4>
<textarea rows=""30"">"
                        + CvInvoke.BuildInformation + @"
</textarea>
</body>
</html>"
                      }


                  };
        }

        private static String GetBackendInfo(Backend[] backends)
        {
            List<String> backendsText = new List<string>();
            foreach (var backend in backends)
            {
                backendsText.Add(String.Format("<p>{0} - {1}</p>", backend.ID, backend.Name));
            }

            return String.Join("", backendsText.ToArray());
        }
    }
}
