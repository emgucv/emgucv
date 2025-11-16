//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace Emgu.CV.Test
{
   public class FaceDetector
   {
      private CascadeClassifier _faceCascade;

      public FaceDetector()
      {
         _faceCascade = new CascadeClassifier("haarcascade_frontalface_alt2.xml");

      }

      public List<Face> Detect(Mat img)
      {
         using (Mat gray = new Mat() )
         {
             CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
                Rectangle[] objects = _faceCascade.DetectMultiScale(gray, 1.1, 3, Size.Empty, Size.Empty);
            List<Face> res = new List<Face>();

            foreach (Rectangle o in objects)
            {
               res.Add(new Face(new Mat(gray, o), o));
            }
            return res;
         }
      }

      public void Dispose()
      {
         _faceCascade.Dispose();
      }
   }

   public class Eye
   {
      private Mat _image;
      public Eye(Mat img)
      {
         _image = img;
      }

      public Mat RGB
      {
         get
         {
            return _image;
         }
      }
   }

   public class Face 
   {
      private Mat _image;
      private Mat _imageGray;
      private Mat _imageHSV;
      private Mat _h;
      private Mat _s;
      private Mat _v;
      //private DenseHistogram _hueHtg;
      //private Seq<MCvContour> _skinContour;
      private Rectangle _rect;
      private CascadeClassifier _eyeCascade;

      public Face(Mat img, Rectangle rect)
      {
         _image = img;
         _rect = rect;
         _eyeCascade = new CascadeClassifier("haarcascade_eye_tree_eyeglasses.xml");
      }

      public List<Eye> DetectEye()
      {
         Rectangle[] objects = _eyeCascade.DetectMultiScale(Gray, 1.1, 3, Size.Empty, Size.Empty);
         List<Eye> res = new List<Eye>();

         foreach (Rectangle o in objects)
         {
            
            res.Add(new Eye(new Mat(_image, o)));
         }
         
         return res;
      }

      public Rectangle Rectangle
      {
         get { return _rect; }
      }

      public Mat Bgr
      {
         get
         {
            return _image;
         }
      }

      public Mat Gray
      {
         get
         {
             if (_imageGray == null)
             {
                 _imageGray = new Mat();
                 CvInvoke.CvtColor(_image, _imageGray, ColorConversion.Bgr2Gray);
             }
            return _imageGray;
         }
      }

      public Mat Hsv
      {
         get
         {
             if (_imageHSV == null)
             {
                 _imageHSV = new Mat();
                 CvInvoke.CvtColor(_image, _imageHSV, ColorConversion.Bgr2Hsv);
             }
            return _imageHSV;
         }
      }

      public Mat H
      {
         get
         {
            if (_h == null)
            {
               _h = new Mat();
               _s = new Mat();
               _v = new Mat();
               using (VectorOfMat vm = new VectorOfMat(_h, _s, _v))
                CvInvoke.Split(Hsv, vm);
            }
            return _h;
         }
      }
      public Mat S
      {
         get
         {
            if (_s == null)
            {
                _h = new Mat();
                _s = new Mat();
                _v = new Mat();
                using (VectorOfMat vm = new VectorOfMat(_h, _s, _v))
                    CvInvoke.Split(Hsv, vm);
            }
                return _s;
         }
      }
      public Mat V
      {
         get
         {
            if (_h == null)
            {
                _h = new Mat();
                _s = new Mat();
                _v = new Mat();
                using (VectorOfMat vm = new VectorOfMat(_h, _s, _v))
                    CvInvoke.Split(Hsv, vm);
            }
                return _v;
         }
      }

      /*
      public DenseHistogram HueHistogram
      {
         get
         {
            if (_hueHtg == null)
            {
               int size = 60;
               _hueHtg = new DenseHistogram(size ,  new RangeF(0, 180) );
            }
            return _hueHtg;
         }
      }*/

      public Mat SkinMask
      {
         get
         {
             Mat skinMask = new Mat();
             Gray.CopyTo(skinMask);

            

            return skinMask;
         }
      }


      /*
      public Seq<MCvContour> SkinContour
      {
          get
          {
              if (_skinContour == null)
              {
                  Histogram htg = HueHistogram;

                  htg.Accumulate(new Image<Gray, Byte>[1] { H });

                  double[] arr = new double[htg.BinSize[0]];
                  for (int i = 0; i < htg.BinSize[0]; i++)
                      arr[i] = htg.Query(new int[1] { i });
                  System.Array.Sort<double>(arr);
                  System.Array.Reverse(arr);
                  htg.Threshold(arr[2]);

                  using (Image<Gray, Byte> bpj = htg.BackProject(new Image<Gray, Byte>[1] { H }))
                  {
                      Seq<MCvContour> cList = bpj.FindContours( CvEnum.CHAIN_APPROX_METHOD.CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.LIST, new MemStorage());
                      Seq<MCvContour> maxAreaContour = cList;
                      foreach (Seq<MCvContour> ct in cList)
                      {
                          if (ct.Area > maxAreaContour.Area)
                              maxAreaContour = ct;
                      }
                      _skinContour = GRAY.Snake(maxAreaContour, 1.0f, 1.0f, 1.0f, new Point2D<int>(5, 5), new Emgu.CV.MCvTermCriteria(20, 1.0), new MemStorage());

                  }
              }
              return _skinContour;
          }
      }*/

      public void Dispose()
      {
         _image.Dispose();
         if (_imageGray != null) _imageGray.Dispose();
         if (_imageHSV != null) _imageHSV.Dispose();
         if (_h != null) _h.Dispose();
         if (_s != null) _s.Dispose();
         if (_v != null) _v.Dispose();
         //if (_skinContour != null) _skinContour.Dispose();
      }
   }
}
