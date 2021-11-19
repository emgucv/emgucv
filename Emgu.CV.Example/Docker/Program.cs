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
	    CvInvoke.LogLevel = LogLevel.Verbose;
	    
            System.Console.WriteLine(CvInvoke.BuildInformation);

	    System.Console.WriteLine(GetDnnInfo() + System.Environment.NewLine);
	    
	    System.Console.WriteLine(GetRuntimeInfo() + System.Environment.NewLine);
	    
	    System.Console.WriteLine(GetVideoIOInfo() + System.Environment.NewLine);
        }

	private static String GetDnnInfo()
	{
	    var openCVConfigDict = CvInvoke.ConfigDict;
	    bool haveDNN = (openCVConfigDict["HAVE_OPENCV_DNN"] != 0);
	    String dnnText;
	    if (haveDNN)
	    {
		var dnnBackends = DnnInvoke.AvailableBackends;
		List<String> dnnBackendsText = new List<string>();
		foreach (var dnnBackend in dnnBackends)
		{
		    dnnBackendsText.Add(String.Format("{0} - {1}", dnnBackend.Backend, dnnBackend.Target));
		}
		
		dnnText = String.Format("DNN backends: {0}{1}", System.Environment.NewLine, String.Join(System.Environment.NewLine, dnnBackendsText.ToArray()));
	    }
	    else
	    {
		dnnText = "DNN module not available.";
	    }
	    
	    return dnnText;
	}

	private static String GetBackendInfo(Emgu.CV.Backend[] backends)
	{
	    List<String> backendsText = new List<string>();
	    foreach (var backend in backends)
	    {
		backendsText.Add(String.Format("{0} - {1}", backend.ID, backend.Name));
	    }

	    return String.Join(System.Environment.NewLine, backendsText.ToArray());
	}
	
        private static String GetVideoIOInfo()
        {
            String outText = String.Empty;
	    
            outText += String.Format("{0}{0}Stream Backends (VideoCapture from file/Stream): {0}{1}", System.Environment.NewLine, GetBackendInfo(CvInvoke.StreamBackends));
	    
	    outText += String.Format("{0}{0}VideoWriter backends: {0}{1}{0}", Environment.NewLine,
				     GetBackendInfo(CvInvoke.WriterBackends));
	    if (Emgu.Util.Platform.OperationSystem == Platform.OS.Windows)
	    {
		int width = 640; 
		int height = 480;
		
		try
		{
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
		} catch
		{
		    outText += "Failed to write video to out.avi" + Environment.NewLine;
		    return outText;
		}
		
		try
		{
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
		} catch
		{
		    outText += "Failed to read from out.avi" + Environment.NewLine;
		}
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
