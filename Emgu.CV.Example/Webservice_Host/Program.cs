using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Webservice_Host
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Uri uri = new Uri("http://localhost:8080/ImageService");
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.TransferMode = TransferMode.Streamed;
            */
            
            Uri uri = new Uri("net.tcp://localhost:8082/ImageService");
            NetTcpBinding binding = new NetTcpBinding();
            

            binding.MaxReceivedMessageSize = 5000000;
            /*
            binding.SendTimeout = new TimeSpan(1, 0, 0);
            binding.ReceiveTimeout = new TimeSpan(1, 0, 0);
            binding.Security.Mode = SecurityMode.None;
            binding.TransferMode = TransferMode.Streamed;
            */

            ServiceHost host = new ServiceHost(typeof(ImageService));

            ServiceBehaviorAttribute serviceBehavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehavior.IncludeExceptionDetailInFaults = true;

            /*
            #region defines the ServiceMetadataBehavior for this host
            ServiceMetadataBehavior metadataBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                host.Description.Behaviors.Add(metadataBehavior);
            }
            metadataBehavior.HttpGetEnabled = true;
            #endregion
            */

            // Create endpoint
            host.AddServiceEndpoint(typeof(IImageService), binding, uri);

            host.Open();
            Console.WriteLine("Service is ready, press any key to terminate.");
            Console.ReadKey();
            host.Close();
        }


    }
}
