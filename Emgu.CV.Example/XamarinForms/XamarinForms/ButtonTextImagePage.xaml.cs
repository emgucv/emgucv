using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace Emgu.CV.XamarinForms
{
	public partial class ButtonTextImagePage : ContentPage
	{
		public ButtonTextImagePage ()
		{
			InitializeComponent ();
		}

      public Mat LoadImage(String imageName)
      {
#if __ANDROID__
          return new Mat(Android.App.Application.Context.Assets, imageName);
#else
         return CvInvoke.Imread(imageName, LoadImageType.AnyColor);
#endif
      }

      public void SetImage(Emgu.CV.Mat image)
	   {
	      if (image == null)
	      {
	         this.DisplayImage.Source = null;
	         return;
	      }
	      using (VectorOfByte vb = new VectorOfByte())
	      {
	         CvInvoke.Imencode(".jpg", image, vb);
	         byte[] rawData = vb.ToArray();
	         
	            this.DisplayImage.Source = ImageSource.FromStream(() => new MemoryStream(rawData));
	      }
	   }

	   public Label GetLabel()
	   {
	      return MessageLabel;
	   }
	   public Button GetButton()
	   {
	      return TopButton;
	   }
	}
}
