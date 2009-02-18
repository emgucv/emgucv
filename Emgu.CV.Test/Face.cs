using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Test
{
   public class FaceDetector
   {
      private HaarCascade _faceCascade;

      public FaceDetector()
      {
         _faceCascade = new HaarCascade(".\\haarcascades\\haarcascade_frontalface_alt2.xml");

      }

      public List<Face<D>> Detect<D>(Image<Bgr, D> img)
      {
         using (Image<Gray, D> gray = img.Convert<Gray, D>())
         {
            MCvAvgComp[][] objects = gray.DetectHaarCascade(_faceCascade);
            List<Face<D>> res = new List<Face<D>>();

            foreach (MCvAvgComp o in objects[0])
            {
               img.ROI = o.rect;
               res.Add(new Face<D>(img.Copy(), o.rect));
            }
            img.ROI = System.Drawing.Rectangle.Empty;
            return res;
         }
      }

      public void Dispose()
      {
         _faceCascade.Dispose();
      }
   }

   public class Eye<D>
   {
      private Image<Bgr, D> _image;
      public Eye(Image<Bgr, D> img, System.Drawing.Rectangle rect)
      {
         _image = img;
      }

      public Image<Bgr, D> RGB
      {
         get
         {
            return _image;
         }
      }
   }

   public class Face<D>
   {
      private Image<Bgr, D> _image;
      private Image<Gray, D> _imageGray;
      private Image<Hsv, D> _imageHSV;
      private Image<Gray, D> _h;
      private Image<Gray, D> _s;
      private Image<Gray, D> _v;
      private Histogram _hueHtg;
      //private Seq<MCvContour> _skinContour;
      private System.Drawing.Rectangle _rect;
      private HaarCascade _eyeCascade;

      public Face(Image<Bgr, D> img, System.Drawing.Rectangle rect)
      {
         _image = img;
         _rect = rect;
         _eyeCascade = new HaarCascade(".\\haarcascades\\haarcascade_eye_tree_eyeglasses.xml");
      }

      public List<Eye<D>> DetectEye()
      {
         MCvAvgComp[][] objects = Gray.DetectHaarCascade(_eyeCascade);
         List<Eye<D>> res = new List<Eye<D>>();

         foreach (MCvAvgComp o in objects[0])
         {
            _image.ROI = o.rect;
            res.Add(new Eye<D>(_image.Copy(), o.rect));
         }
         _image.ROI = System.Drawing.Rectangle.Empty;
         return res;
      }

      public System.Drawing.Rectangle Rectangle
      {
         get { return _rect; }
      }

      public Image<Bgr, D> Bgr
      {
         get
         {
            return _image;
         }
      }

      public Image<Gray, D> Gray
      {
         get
         {
            if (_imageGray == null) _imageGray = _image.Convert<Gray, D>();
            return _imageGray;
         }
      }

      public Image<Hsv, D> Hsv
      {
         get
         {
            if (_imageHSV == null) _imageHSV = _image.Convert<Hsv, D>();
            return _imageHSV;
         }
      }

      public Image<Gray, D> H
      {
         get
         {
            if (_h == null)
            {
               Image<Gray, D>[] imgs = Hsv.Split();
               _h = imgs[0];
               _s = imgs[1];
               _v = imgs[2];
            }
            return _h;
         }
      }
      public Image<Gray, D> S
      {
         get
         {
            if (_s == null)
            {
               Image<Gray, D>[] imgs = Hsv.Split();
               _h = imgs[0];
               _s = imgs[1];
               _v = imgs[2];
            }
            return _s;
         }
      }
      public Image<Gray, D> V
      {
         get
         {
            if (_h == null)
            {
               Image<Gray, D>[] imgs = Hsv.Split();
               _h = imgs[0];
               _s = imgs[1];
               _v = imgs[2];
            }
            return _v;
         }
      }

      public Histogram HueHistogram
      {
         get
         {
            if (_hueHtg == null)
            {
               int size = 60;
               _hueHtg = new Histogram(size ,  new RangeF(0, 180) );
            }
            return _hueHtg;
         }
      }

      public Image<Gray, D> SkinMask
      {
         get
         {
            Image<Gray, D> skinMask = Gray.CopyBlank();

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

                  htg.Accumulate(new Image<Gray, D>[1] { H });

                  double[] arr = new double[htg.BinSize[0]];
                  for (int i = 0; i < htg.BinSize[0]; i++)
                      arr[i] = htg.Query(new int[1] { i });
                  System.Array.Sort<double>(arr);
                  System.Array.Reverse(arr);
                  htg.Threshold(arr[2]);

                  using (Image<Gray, D> bpj = htg.BackProject(new Image<Gray, D>[1] { H }))
                  {
                      Seq<MCvContour> cList = bpj.FindContours( CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, new MemStorage());
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
