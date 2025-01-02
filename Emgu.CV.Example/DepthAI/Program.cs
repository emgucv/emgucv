//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dai;
using Emgu.CV.ML;
//using Emgu.CV.Models.DepthAI;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Newtonsoft.Json;


namespace DepthAI
{

    class Program
    {
        private static bool _is_running = true;
        private static Mat _render = new Mat();

        /*
        private static void onDownloadProgressChanged(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            if (totalBytesToReceive == null)
                System.Console.WriteLine(String.Format("{0} bytes downloaded", bytesReceived));
            else
                System.Console.WriteLine(String.Format("{0} of {1} bytes downloaded ({2}%)", bytesReceived, totalBytesToReceive, progressPercentage));
        }
        */

        static async Task Main(string[] args)
        {
            CvInvoke.Init();

            if (!DaiInvoke.HaveDepthAI)
            {
                Console.WriteLine("The native binary is built without Depth AI support.");
                return;
            }

            String win1 = "Depth AI (Press 'q' to quit)"; //The name of the window

            CvInvoke.NamedWindow(win1); //Create the window using the specific name

            using (Emgu.CV.Dai.Pipeline pipeline = new Pipeline())
            using (Emgu.CV.Dai.ColorCamera colorCamera = pipeline.CreateColorCamera())
            using (Emgu.CV.Dai.XLinkOut xLinkOut = pipeline.CreateXLinkOut())
            {
                xLinkOut.StreamName = "preview";
                colorCamera.Interleaved = true;
                using (NodeOutput cameraPreview = colorCamera.GetPreview())
                using (Emgu.CV.Dai.NodeInput xLinkOutInput = xLinkOut.GetInput())
                {
                    cameraPreview.Link(xLinkOutInput);

                    using (Emgu.CV.Dai.Device device = new Device(pipeline, true))
                    using (DataOutputQueue preview = device.GetDataOutputQueue("preview"))
                    {

                        while (_is_running)
                        {
                            using (ImgFrame imgFrame = preview.GetImgFrame())
                            using (Mat m = new Mat(
                                      (int)imgFrame.Height,
                                      (int)imgFrame.Width,
                                      DepthType.Cv8U,
                                      3,
                                      imgFrame.GetDataPtr(),
                                      (int)imgFrame.Width * 3))
                            {
                                CvInvoke.Imshow("preview", m);
                            }

                            if (CvInvoke.WaitKey(1) > 0) //Press any key to stop
                            {
                                _is_running = false;
                            }
                        }
                    }
                }
            }

            CvInvoke.DestroyAllWindows(); //Destroy all windows if key is pressed

        }
    }
}