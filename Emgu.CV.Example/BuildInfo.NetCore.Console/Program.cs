//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.Dnn;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Cuda;
using Emgu.Util;

namespace BuildInfo.NetCore.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(CvInvoke.BuildInformation);

            System.Console.WriteLine(GetDnnInfo() + System.Environment.NewLine);

            System.Console.WriteLine(GetOpenCLInfo() + System.Environment.NewLine);

            System.Console.WriteLine(GetCudaInfo() + System.Environment.NewLine);

            System.Console.WriteLine(GetCaptureInfo() + System.Environment.NewLine);

            System.Console.WriteLine(GetRuntimeInfo() + System.Environment.NewLine);
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

        private static String GetCaptureInfo()
        {
            var captureBackends = CvInvoke.Backends;
            List<String> captureBackendsText = new List<string>();
            foreach (var captureBackend in captureBackends)
            {
                captureBackendsText.Add(String.Format("{0} - {1}", captureBackend.ID, captureBackend.Name));
            }

            String captureText = String.Format("Capture backends: {0}{1}", System.Environment.NewLine, String.Join(System.Environment.NewLine, captureBackendsText.ToArray()));
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
    }
}
