//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV.Test
{
   public class FaceDetector
   {
      private CascadeClassifier _faceCascade;

      public FaceDetector()
      {
         _faceCascade = new CascadeClassifier("haarcascade_frontalface_alt2.xml");

      }

      public List<Face> Detect(Image<Bgr, Byte> img)
      {
         using (Image<Gray, Byte> gray = img.Convert<Gray, Byte>())
         {
            Rectangle[] objects = _faceCascade.DetectMultiScale(gray, 1.1, 3, Size.Empty, Size.Empty);
            List<Face> res = new List<Face>();

            foreach (Rectangle o in objects)
            {
               img.ROI = o;
               res.Add(new Face(img.Copy(), o));
            }
            img.ROI = Rectangle.Empty;
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
      private Image<Bgr, Byte> _image;
      public Eye(Image<Bgr, Byte> img, Rectangle rect)
      {
         _image = img;
      }

      public Image<Bgr, Byte> RGB
      {
         get
         {
            return _image;
         }
      }
   }

   public class Face 
   {
      private Image<Bgr, Byte> _image;
      private Image<Gray, Byte> _imageGray;
      private Image<Hsv, Byte> _imageHSV;
      private Image<Gray, Byte> _h;
      private Image<Gray, Byte> _s;
      private Image<Gray, Byte> _v;
      //private DenseHistogram _hueHtg;
      //private Seq<MCvContour> _skinContour;
      private Rectangle _rect;
      private CascadeClassifier _eyeCascade;

      public Face(Image<Bgr, Byte> img, Rectangle rect)
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
            _image.ROI = o;
            res.Add(new Eye(_image.Copy(), o));
         }
         _image.ROI = Rectangle.Empty;
         return res;
      }

      public Rectangle Rectangle
      {
         get { return _rect; }
      }

      public Image<Bgr, Byte> Bgr
      {
         get
         {
            return _image;
         }
      }

      public Image<Gray, Byte> Gray
      {
         get
         {
            if (_imageGray == null) _imageGray = _image.Convert<Gray, Byte>();
            return _imageGray;
         }
      }

      public Image<Hsv, Byte> Hsv
      {
         get
         {
            if (_imageHSV == null) _imageHSV = _image.Convert<Hsv, Byte>();
            return _imageHSV;
         }
      }

      public Image<Gray, Byte> H
      {
         get
         {
            if (_h == null)
            {
               Image<Gray, Byte>[] imgs = Hsv.Split();
               _h = imgs[0];
               _s = imgs[1];
               _v = imgs[2];
            }
            return _h;
         }
      }
      public Image<Gray, Byte> S
      {
         get
         {
            if (_s == null)
            {
               Image<Gray, Byte>[] imgs = Hsv.Split();
               _h = imgs[0];
               _s = imgs[1];
               _v = imgs[2];
            }
            return _s;
         }
      }
      public Image<Gray, Byte> V
      {
         get
         {
            if (_h == null)
            {
               Image<Gray, Byte>[] imgs = Hsv.Split();
               _h = imgs[0];
               _s = imgs[1];
               _v = imgs[2];
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

      public Image<Gray, Byte> SkinMask
      {
         get
         {
            Image<Gray, Byte> skinMask = Gray.CopyBlank();

            //skinMask.Draw(SkinContour, new Gray(255.0), new Gray(120.0), -1);

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
