//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
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

        private String _modelFolderName = "dnn_data";
        private String _path = null;
        private void InitPath()
        {
            if (_path == null)
            {
#if __ANDROID__
                _path = System.IO.Path.Combine(
                    Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                    Android.OS.Environment.DirectoryDownloads, 
                    _modelFolderName);
#else
                _path = String.Format("./{0}/", _modelFolderName);
#endif
            }
        }

        public static String DnnDownloadFile(String url, String fileName, String folder)
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
                    String source = url + fileName;

                    Console.WriteLine(String.Format("Downloading file from '{0}' to '{1}'", source, dest));
                    webclient.DownloadFile(source, dest);
                    Console.WriteLine(String.Format("Download completed"));
                }
            FileInfo fi = new FileInfo(dest);
            return fi.FullName;
        }


        public DnnPage()
         : base()
        {
            var button = this.GetButton();
            button.Text = "Perform Mask-rcnn Detection";
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
                      InitPath();


                      String url =
                          "https://github.com/emgucv/models/raw/master/mask_rcnn_inception_v2_coco_2018_01_28/";
                      String graphFile = DnnDownloadFile(url, "frozen_inference_graph.pb", _path);
                      String lookupFile = DnnDownloadFile(url, "coco-labels-paper.txt", _path);

                      String configFile = DnnDownloadFile(
                          "https://github.com/opencv/opencv_extra/raw/4.0.1/testdata/dnn/",
                          "mask_rcnn_inception_v2_coco_2018_01_28.pbtxt",
                          _path);

                      string[] labels = File.ReadAllLines(lookupFile);
                      Emgu.CV.Dnn.Net net = Emgu.CV.Dnn.DnnInvoke.ReadNetFromTensorflow(graphFile, configFile);


                      Mat blob = DnnInvoke.BlobFromImage(image[0]);

                      net.SetInput(blob, "image_tensor");
                      using (VectorOfMat tensors = new VectorOfMat())
                      {
                          net.Forward(tensors, new string[] { "detection_out_final", "detection_masks" });
                          using (Mat boxes = tensors[0])
                          using (Mat masks = tensors[1])
                          {
                              System.Drawing.Size imgSize = image[0].Size;
                              float[,,,] boxesData = boxes.GetData(true) as float[,,,];
                              //float[,,,] masksData = masks.GetData(true) as float[,,,];
                              int numDetections = boxesData.GetLength(2);
                              for (int i = 0; i < numDetections; i++)
                              {

                                  float score = boxesData[0, 0, i, 2];

                                  if (score > 0.5)
                                  {
                                      int classId = (int)boxesData[0, 0, i, 1];
                                      String label = labels[classId];

                                      float left = boxesData[0, 0, i, 3] * imgSize.Width;
                                      float top = boxesData[0, 0, i, 4] * imgSize.Height;
                                      float right = boxesData[0, 0, i, 5] * imgSize.Width;
                                      float bottom = boxesData[0, 0, i, 6] * imgSize.Height;

                                      RectangleF rectF = new RectangleF(left, top, right - left, bottom - top);
                                      Rectangle rect = Rectangle.Round(rectF);
                                      rect.Intersect(new Rectangle(Point.Empty, imgSize));
                                      CvInvoke.Rectangle(image[0], rect, new MCvScalar(0, 0, 0, 0), 1);
                                      CvInvoke.PutText(image[0], label, rect.Location, FontFace.HersheyComplex, 1.0,
                                          new MCvScalar(0, 0, 255), 2);

                                      int[] masksDim = masks.SizeOfDimension;
                                      using (Mat mask = new Mat(
                                          masksDim[2],
                                          masksDim[3],
                                          DepthType.Cv32F,
                                          1,
                                          //masks.DataPointer +
                                          //(i * masksDim[1] + classId ) 
                                          //* masksDim[2] * masksDim[3] * masks.ElementSize,
                                          masks.GetDataPointer(i, classId),
                                          masksDim[3] * masks.ElementSize))
                                      using (Mat maskLarge = new Mat())
                                      using (Mat maskLargeInv = new Mat())
                                      using (Mat subRegion = new Mat(image[0], rect))
                                      using (Mat largeColor = new Mat(subRegion.Size, Emgu.CV.CvEnum.DepthType.Cv8U, 3))
                                      {
                                          CvInvoke.Resize(mask, maskLarge, rect.Size);

                                          //give the mask at least 30% transparency
                                          using (ScalarArray sa = new ScalarArray(0.7))
                                              CvInvoke.Min(sa, maskLarge, maskLarge);

                                          //Create the inverse mask for the original image
                                          using (ScalarArray sa = new ScalarArray(1.0))
                                              CvInvoke.Subtract(sa, maskLarge, maskLargeInv);

                                          //The mask color
                                          largeColor.SetTo(new Emgu.CV.Structure.MCvScalar(255, 0, 0));
                                          if (subRegion.NumberOfChannels == 4)
                                          {
                                              using (Mat bgrSubRegion = new Mat())
                                              {
                                                  CvInvoke.CvtColor(subRegion, bgrSubRegion, ColorConversion.Bgra2Bgr);
                                                  CvInvoke.BlendLinear(largeColor, bgrSubRegion, maskLarge, maskLargeInv, bgrSubRegion);
                                                  CvInvoke.CvtColor(bgrSubRegion, subRegion, ColorConversion.Bgr2Bgra);
                                              }

                                          }
                                          else
                                              CvInvoke.BlendLinear(largeColor, subRegion, maskLarge, maskLargeInv, subRegion);
                                      }

                                  }
                              }


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