//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Windows;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


namespace Emgu.CV.WPF
{
   /// <summary>
   /// Interaction logic for Window1.xaml
   /// </summary>
   public partial class Window1 : Window
   {
      public Window1()
      {
         InitializeComponent();
      }

      private void image1_Initialized(object sender, EventArgs e)
      {
         Mat image = new Mat(100, 400, DepthType.Cv8U, 3);
         image.SetTo(new Bgr(255, 255, 255).MCvScalar);
         CvInvoke.PutText(image, "Hello, world", new System.Drawing.Point(10, 50), Emgu.CV.CvEnum.FontFace.HersheyPlain, 3.0, new Bgr(255.0, 0.0, 0.0).MCvScalar);

         image1.Source = image.ToBitmapSource();
      }

   }
}
