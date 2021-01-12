//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Freetype;
using Emgu.Models;

namespace Emgu.CV.XamarinForms
{
    public class FreetypePage : ButtonTextImagePage
    {
        public FreetypePage()
            : base()
        {
            var button = this.GetButton();
            button.Text = "Draw Freetype Text";
            button.Clicked += async (sender, args) =>
            {
                
                await InitFreetype();
                Mat img = new Mat(new Size(640, 480), DepthType.Cv8U, 3);
                img.SetTo(new MCvScalar(0, 0, 0, 0));
                _freetype2.PutText(img, "Hello", new Point(100, 50), 36, new MCvScalar(255, 255, 0), 1, LineType.EightConnected, false);
                _freetype2.PutText(img, "您好", new Point(100, 100), 36, new MCvScalar(255, 255, 0), 1, LineType.EightConnected, false);
                _freetype2.PutText(img, "こんにちは", new Point(100, 150), 36, new MCvScalar(255, 255, 0), 1, LineType.EightConnected, false);
                _freetype2.PutText(img, "여보세요", new Point(100, 200), 36, new MCvScalar(255, 255, 0), 1, LineType.EightConnected, false);
                _freetype2.PutText(img, "Здравствуйте", new Point(100, 250), 36, new MCvScalar(255, 255, 0), 1, LineType.EightConnected, false);
                SetImage(img);
            };
        }

        private Freetype.Freetype2 _freetype2 = null;
        private async Task InitFreetype()
        {
            if (_freetype2 == null)
            {
                FileDownloadManager manager = new FileDownloadManager();


                manager.AddFile(
                    "https://github.com/emgucv/emgucv/raw/4.4.0/Emgu.CV.Test/NotoSansCJK-Regular.ttc",
                    "Freetype");


                manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;
                await manager.Download();

                _freetype2 = new Freetype2();
                _freetype2.LoadFontData(manager.Files[0].LocalFile, 0);
                
            }
        }

        private void DownloadManager_OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive <= 0)
                SetMessage(String.Format("{0} bytes downloaded.", e.BytesReceived));
            else
                SetMessage(String.Format("{0} of {1} bytes downloaded ({2}%)", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage));
        }
    }
}