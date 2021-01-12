//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
   }
}
