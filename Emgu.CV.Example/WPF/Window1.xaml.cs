//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
            Image<Bgr, Byte> image = new Image<Bgr, byte>(400, 100, new Bgr(255, 255, 255));
            MCvFont f = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 3.0, 3.0);
            image.Draw("Hello, world", ref f, new System.Drawing.Point(10, 50), new Bgr(255.0, 0.0, 0.0));

            image1.Source = BitmapSourceConvert.ToBitmapSource(image);
        }

    }
}
