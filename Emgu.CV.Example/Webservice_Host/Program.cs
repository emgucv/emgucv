//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Emgu.CV;

namespace Webservice_Host
{
   class Program
   {
      static void Main(string[] args)
      {
         if (!IsPlaformCompatable()) return;

         Uri uri = new Uri("net.tcp://localhost:8082/ImageService");
         NetTcpBinding binding = new NetTcpBinding();

         ServiceHost host = new ServiceHost(typeof(ImageService));

         ServiceBehaviorAttribute serviceBehavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
         serviceBehavior.IncludeExceptionDetailInFaults = true;

         // Create endpoint
         host.AddServiceEndpoint(typeof(IImageService), binding, uri);

         host.Open();
         Console.WriteLine("Service is ready, press any key to terminate.");
         Console.ReadKey();
         host.Close();
      }

      /// <summary>
      /// Check if both the managed and unmanaged code are compiled for the same architecture
      /// </summary>
      /// <returns>Returns true if both the managed and unmanaged code are compiled for the same architecture</returns>
      static bool IsPlaformCompatable()
      {
         int clrBitness = Marshal.SizeOf(typeof(IntPtr)) * 8;
         if (clrBitness != CvInvoke.UnmanagedCodeBitness)
         {
            Console.WriteLine(String.Format("Platform mismatched: CLR is {0} bit, C++ code is {1} bit."
               + " Please consider recompiling the executable with the same platform target as C++ code.",
               clrBitness, CvInvoke.UnmanagedCodeBitness));
            Console.ReadKey();
            return false;
         }
         return true;
      }
   }
}
