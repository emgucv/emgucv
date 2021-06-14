//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.Dnn;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace BuildInfo.NetCore.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(CvInvoke.BuildInformation);
			
			System.Console.WriteLine(GetRuntimeInfo() + System.Environment.NewLine);
			
			System.Console.WriteLine(GetVideoIOInfo() + System.Environment.NewLine);
        }


        private static String GetVideoIOInfo()
        {
            String outText = String.Empty;
			int width = 640; 
			int height = 480;
            using (VideoWriter writer = new VideoWriter("out.avi", VideoWriter.Fourcc('M', 'J', 'P', 'G'), 5, new Size(width, height), true))
			{
				for (int i = 0; i < 10; i++)
				{
					using (Mat m = new Mat (width, height, DepthType.Cv8U, 3))
					{
						writer.Write(m);
					}
				}
				outText += "Video written to out.avi" + Environment.NewLine;
			}
			using (VideoCapture capture = new VideoCapture("out.avi"))
			{
				Mat img2 = capture.QueryFrame();
                int count = 0;
                while (img2 != null && !img2.IsEmpty)
                {
					img2.Dispose();
                    img2 = capture.QueryFrame();
                    count++;
                }
				outText += "Video read from out.avi" + Environment.NewLine;
			}
            
            return outText;
        }

        private static String GetRuntimeInfo()
        {
            StringBuilder runtimeStringBuilder = new StringBuilder();
            runtimeStringBuilder.Append(
                String.Format("Runtime info: {0}", System.Environment.NewLine));
            runtimeStringBuilder.Append(
                String.Format("OS Architecture: {0}{1}", RuntimeInformation.OSArchitecture, System.Environment.NewLine));
            runtimeStringBuilder.Append(
                String.Format("Framework Description: {0}{1}", RuntimeInformation.FrameworkDescription, System.Environment.NewLine));
            runtimeStringBuilder.Append(
                String.Format("Process Architecture: {0}{1}", RuntimeInformation.ProcessArchitecture, System.Environment.NewLine));
            runtimeStringBuilder.Append(
                String.Format("OS Description: {0}{1}", RuntimeInformation.OSDescription, System.Environment.NewLine));
            return runtimeStringBuilder.ToString();
        }
    }
}
