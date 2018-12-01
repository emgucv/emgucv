//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !NETFX_CORE

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Preferences;
#endif

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Xamarin.Forms;
using Emgu.CV.Dnn;
using Emgu.CV.Util;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace Emgu.CV.XamarinForms
{
    public class DnnPage : ButtonTextImagePage
    {

        private static String DnnDownloadFile(String folder, String fileName)
        {
            String folderName = folder;
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }
            String dest = System.IO.Path.Combine(folderName, fileName);
            if (!System.IO.File.Exists(dest))
                using (System.Net.WebClient webclient = new System.Net.WebClient())
                {
                    String source = "https://github.com/emgucv/models/raw/master/mask_rcnn_inception_v2_coco_2018_01_28/" + fileName;
                    
                    Console.WriteLine(String.Format("Downloading file from '{0}' to '{1}'", source, dest));
                    webclient.DownloadFile(source, dest);
                    Console.WriteLine(String.Format("Download completed"));
                }
            return dest;
        }


        public DnnPage()
         : base()
        {
            var button = this.GetButton();
            button.Text = "Perform Dnn Detection";
            button.Clicked += OnButtonClicked;

            OnImagesLoaded += async (sender, image) =>
            {
                if (image == null || image[0] == null)
                    return;
                SetMessage("Please wait...");
                SetImage(null);

                Task<Tuple<Mat, String, long>> t = new Task<Tuple<Mat, String, long>>(
                  () =>
                  {

                      
#if __ANDROID__
                      String path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                             Android.OS.Environment.DirectoryDownloads, "dnn_data");
#else
                      String path = "./dnn_data/";
#endif

                      String graphFile = DnnDownloadFile(path, "frozen_inference_graph.pb");
                      String lookupFile = DnnDownloadFile(path, "coco-labels-paper.txt");
                      String configFile = "mask_rcnn_inception_v2_coco_2018_01_28.pbtxt";
                      Emgu.CV.Dnn.Net net = Emgu.CV.Dnn.DnnInvoke.ReadNetFromTensorflow(graphFile, configFile);
                      Mat blob = DnnInvoke.BlobFromImage(image[0]);
                      
                      net.SetInput(blob, "image_tensor");
                      using (VectorOfMat tensors = new VectorOfMat())
                      {
                          net.Forward(tensors, new string[]  {"detection_out_final", "detection_masks" });
                          using (Mat boxes = tensors[0])
                          using (Mat masks = tensors[1])
                          {
                              System.Drawing.Size imgSize = image[0].Size;
                              float[,,,] boxesData = boxes.GetData(true) as float[,,,];
                              float[,,,] masksData = masks.GetData(true) as float[,,,];
                              int numDetections = boxesData.GetLength(2);
                              for (int i = 0; i < numDetections; i++)
                              {
                                  float score = boxesData[0, 0, i, 2];
                                  float left = boxesData[0, 0, i, 3] * imgSize.Width;
                                  float top = boxesData[0, 0, i, 4] * imgSize.Height;
                                  float right = boxesData[0, 0, i, 5] * imgSize.Width;
                                  float bottom = boxesData[0, 0, i, 6] * imgSize.Height;
                                  //left = Math.Max(0, Math.Min(left, imgSize.Width - 1));
                                  //top = Math.Max(0, Math.Min(top, imgSize.Height - 1));
                                  //right = Math.Max(0, Math.Min(right, imgSize.Width - 1));
                                  //bottom = Math.Max(0, Math.Min(bottom, imgSize.Height - 1));

                                  RectangleF rectF = new RectangleF(left, top, right - left, bottom - top);
                                  Rectangle rect = Rectangle.Round(rectF);
                                  Rectangle rect = Rectangle.Round(rectF);
                                  rect.
                                      CvInvoke.Rectangle(image[0], rect, new MCvScalar(0,0,0,0), 1);
                              }
                              //int numClasses = masks.SizeOfDimemsion[]
                              //int numDetections = boxes.SizeOfDimemsion[]

                          }
                      }
                      long time = 0;

                      return new Tuple<Mat, String, long>(image[0], null, time);
                  });
                t.Start();

                var result = await t;
                SetImage(t.Result.Item1);
                String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
                
                SetMessage(t.Result.Item2);
            };
        }

        private void OnButtonClicked(Object sender, EventArgs args)
        {
            LoadImages(new string[] { "dog416.png" });
        }

    }
}

#endif