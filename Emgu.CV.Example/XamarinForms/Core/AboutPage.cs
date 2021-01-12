//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Dnn;
using Emgu.CV.ML;
using Xamarin.Forms;

namespace Emgu.CV.XamarinForms
{
    public class AboutPage : ContentPage
    {
        public AboutPage()
        {

            String openclTxt = String.Format("Has OpenCL: {0}", CvInvoke.HaveOpenCL);

            String lineBreak = "<br/>";
            if (CvInvoke.HaveOpenCL)
            {
                openclTxt = String.Format("{0}{1}Use OpenCL: {2}{1}<textarea rows=\"5\">{3}</textarea>{1}",
                   openclTxt, lineBreak,
                   CvInvoke.UseOpenCL,
                   CvInvoke.OclGetPlatformsSummary());
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

            String osDescription = Emgu.Util.Platform.OperationSystem.ToString();

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
<H4> OS: </H4>
" + osDescription + @"
<H4> OS Architecture: </H4>
" + RuntimeInformation.OSArchitecture + @"
<H4> Framework Description: </H4>
" + RuntimeInformation.FrameworkDescription + @"
<H4> Process Architecture: </H4>
" + RuntimeInformation.ProcessArchitecture + @"
<H4> Dnn Backends: </H4>
" + dnnText + @"
<H4> Capture Backends: </H4>
" + GetCaptureInfo() + @"
<H4> Build Info </H4>
<textarea rows=""30"">"
                        + CvInvoke.BuildInformation + @"
</textarea>
</body>
</html>"
                      }


                  };
        }

        private static String GetCaptureInfo()
        {
            var captureBackends = CvInvoke.Backends;
            List<String> captureBackendsText = new List<string>();
            foreach (var captureBackend in captureBackends)
            {
                captureBackendsText.Add(String.Format("<p>{0} - {1}</p>", captureBackend.ID, captureBackend.Name));
            }

            String captureText = String.Join("", captureBackendsText.ToArray());
            return captureText;
        }
    }
}
