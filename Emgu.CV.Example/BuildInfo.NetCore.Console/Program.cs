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

            System.Console.WriteLine(GetOpenCLInfo() + System.Environment.NewLine);

            System.Console.WriteLine(GetCudaInfo() + System.Environment.NewLine);

            System.Console.WriteLine(GetCaptureInfo() + System.Environment.NewLine);

            System.Console.WriteLine(GetRuntimeInfo() + System.Environment.NewLine);

            System.Console.WriteLine(GetParallelBackendInfo() + System.Environment.NewLine);
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

        private static String GetOpenCLInfo()
        {
            String openCLText = String.Format("Has OpenCL: {0}", CvInvoke.HaveOpenCL);

            String lineBreak = System.Environment.NewLine;
            if (CvInvoke.HaveOpenCL)
            {
                openCLText = String.Format("{0}{1}Use OpenCL: {2}{1}{3}{1}",
                    openCLText, lineBreak,
                    CvInvoke.UseOpenCL,
                    CvInvoke.OclGetPlatformsSummary());
            }

            return openCLText;
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

        private static String GetVideoWriterFFMPEGInfo()
        {
            if (Emgu.Util.Platform.OperationSystem == Platform.OS.Windows)
            {
                Emgu.CV.Backend[] backends = CvInvoke.WriterBackends;
                int backend_idx = 0; //any backend;
                String backendName = String.Empty;
                foreach (Emgu.CV.Backend be in backends)
                {
                    if (be.Name.Equals("FFMPEG"))
                        //if (be.Name.Equals("INTEL_MFX"))
                    {
                        backend_idx = be.ID;
                        backendName = be.Name;
                        break;
                    }
                }

                if (backend_idx > 0) //FFMPEG backend is available
                {
                    try
                    {
                        using (VideoWriter writer = new VideoWriter(
                            "tmp.avi",
                            backend_idx,
                            VideoWriter.Fourcc('X', 'V', 'I', 'D'),
                            25,
                            new Size(640, 480),
                            new Tuple<VideoWriter.WriterProperty, int>[]
                            {
                                new Tuple<VideoWriter.WriterProperty, int>(VideoWriter.WriterProperty.IsColor, 1),
                                new Tuple<VideoWriter.WriterProperty, int>(VideoWriter.WriterProperty.HwAcceleration, (int) VideoAccelerationType.Any)
                            }))
                        {

                            VideoAccelerationType hwAcceleration =
                                (VideoAccelerationType)writer.Get(VideoWriter.WriterProperty.HwAcceleration);
                            return String.Format("{0}VideoWriter successfully created with backend: {1} (hw acceleration: {2})", System.Environment.NewLine, backendName, hwAcceleration);
                        }
                    }
                    catch (Exception e)
                    {
                        //System.Console.WriteLine(e);
                        return Environment.NewLine + "Failed to create VideoWriter with FFMPEG backend.";
                    }
                }
                else
                {
                    return Environment.NewLine + "FFMPEG backend not found.";
                }
            }

            return String.Empty;
        }

        private static String GetVideoWriterIntelMfxInfo()
        {
            if (Emgu.Util.Platform.OperationSystem == Platform.OS.Windows)
            {
                Emgu.CV.Backend[] backends = CvInvoke.WriterBackends;
                int backend_idx = 0; //any backend;
                String backendName = String.Empty;
                foreach (Emgu.CV.Backend be in backends)
                {
                    if (be.Name.Equals("INTEL_MFX"))
                    {
                        backend_idx = be.ID;
                        backendName = be.Name;
                        break;
                    }
                }

                if (backend_idx > 0) //Intel MFX backend is available
                {
                    try
                    {
                        using (VideoWriter writer = new VideoWriter(
                            "tmp.avi",
                            backend_idx,
                            VideoWriter.Fourcc('H', '2', '6', '4'),
                            25,
                            new Size(640, 480),
                            new Tuple<VideoWriter.WriterProperty, int>[]
                            {
                                new Tuple<VideoWriter.WriterProperty, int>(VideoWriter.WriterProperty.IsColor, 1),
                                new Tuple<VideoWriter.WriterProperty, int>(VideoWriter.WriterProperty.HwAcceleration, (int) VideoAccelerationType.Any)
                            }))
                        {

                            VideoAccelerationType hwAcceleration =
                                (VideoAccelerationType)writer.Get(VideoWriter.WriterProperty.HwAcceleration);
                            return String.Format("{0}VideoWriter successfully created with backend: {1} (hw acceleration: {2})", System.Environment.NewLine, backendName, hwAcceleration);
                        }
                    }
                    catch (Exception e)
                    {
                        //System.Console.WriteLine(e);
                        return Environment.NewLine + "Failed to create VideoWriter with Intel MFX backend.";
                    }
                }
                else
                {
                    return Environment.NewLine + "Intel MFX backend not found.";
                }
            }
            return String.Empty;
        }

        private static String GetCaptureInfo()
        {
            String captureText = String.Format("Capture Backends (VideoCapture from device): {0}{1}", System.Environment.NewLine, GetBackendInfo(CvInvoke.Backends));

	    /*
            //We don't want to create VideoCapture on Mac OS unless we have requested camera permission
            if (Emgu.Util.Platform.OperationSystem != Platform.OS.MacOS)
            {
	        try {
                  using (VideoCapture cap = new VideoCapture(0, VideoCapture.API.Any,
                      new Tuple<CapProp, int>(CapProp.HwAcceleration, (int)VideoAccelerationType.Any)))
                  {
                      if (cap.IsOpened)
                      {
                          String backendName = cap.BackendName;
                          VideoAccelerationType hwAcceleration = (VideoAccelerationType)cap.Get(CapProp.HwAcceleration);
                          captureText +=
                              String.Format(
                                  "{0}VideoCapture device successfully opened with default backend: {1} (hw acceleration: {2})",
                                  System.Environment.NewLine, backendName, hwAcceleration);
                      } else
                      {
                          captureText +=
                              String.Format(
                                  "{0}VideoCapture device failed to opened with default backend: {1}",
                                  System.Environment.NewLine, cap.BackendName);
                      }
                  }
		} catch (Emgu.CV.Util.CvException e)
		{
		        //System.Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>");
			captureText += String.Format("{0}No capture device found.", System.Environment.NewLine);
		}
            }*/
	    
            captureText += String.Format("{0}{0}Stream Backends (VideoCapture from file/Stream): {0}{1}", System.Environment.NewLine, GetBackendInfo(CvInvoke.StreamBackends));

            captureText += String.Format("{0}{0}VideoWriter backends: {0}{1}{0}", Environment.NewLine,
                GetBackendInfo(CvInvoke.WriterBackends));

            captureText += GetVideoWriterFFMPEGInfo();
            //captureText += GetVideoWriterIntelMfxInfo();

            return captureText;
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

        private static String GetCudaInfo()
        {
            StringBuilder cudaStringBuilder = new StringBuilder();

            cudaStringBuilder.Append(
                String.Format("Has Cuda: {0}{1}", CudaInvoke.HasCuda, System.Environment.NewLine));
            if (CudaInvoke.HasCuda)
            {
                cudaStringBuilder.Append(CudaInvoke.GetCudaDevicesSummary());
            }

            return cudaStringBuilder.ToString();
        }

        private static String GetParallelBackendInfo()
        {
            StringBuilder parallelBackendStringBuilder = new StringBuilder();

            String[] availableParallelBackends = CvInvoke.AvailableParallelBackends;
            parallelBackendStringBuilder.Append(String.Format("Available Parallel backends:{0}", Environment.NewLine));
            parallelBackendStringBuilder.Append(String.Join(Environment.NewLine, availableParallelBackends));

            return parallelBackendStringBuilder.ToString();
        }
    }
}
