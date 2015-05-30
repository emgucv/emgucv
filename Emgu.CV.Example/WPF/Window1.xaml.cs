//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

         image1.Source = BitmapSourceConvert.ToBitmapSource(image);
      }

   }
}
