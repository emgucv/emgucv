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
using Emgu.CV.Structure;
#if __ANDROID__ || __IOS__
using Xamarin.Media;
#endif

namespace Emgu.CV.XamarinForms
{
	public partial class ButtonTextImagePage : ContentPage
	{
		public ButtonTextImagePage ()
		{
			InitializeComponent ();
		}

#if __ANDROID__ || __IOS__
      private MediaPicker _mediaPicker;
#endif

#if __ANDROID__
      public const int PickImageRequestCode = 1000;
#endif

      public async void LoadImage(String imageName)
      {
#if NETFX_CORE
         Mat m = CvInvoke.Imread(imageName, LoadImageType.AnyColor);
         InvokeOnImageLoaded(m);
#else
         if (_mediaPicker == null)
         {
#if __ANDROID__
            _mediaPicker = new MediaPicker(Forms.Context);
#else
            _mediaPicker = new MediaPicker();
#endif
         }
         
         var action = await (_mediaPicker.IsCameraAvailable? 
            DisplayActionSheet("Use Image from", "Cancel", null, "Default", "Photo Library", "Camera") 
            : DisplayActionSheet("Use Image from", "Cancel", null, "Default", "Photo Library")); 

         if (action.Equals("Default"))
         {
#if __ANDROID__
            InvokeOnImageLoaded(new Mat(Forms.Context.Assets, imageName));
#else
            Mat m = CvInvoke.Imread(imageName, LoadImageType.AnyColor);
            InvokeOnImageLoaded(m);
#endif

         }
         else if (action.Equals("Photo Library"))
         {
#if __ANDROID__
            Android.Content.Intent intent = _mediaPicker.GetPickPhotoUI();
            Android.App.Activity activity = Forms.Context as Android.App.Activity;
            activity.StartActivityForResult(intent, PickImageRequestCode);
            //once the image was picked, the MainActivity.OnActivityResult function will handle the remaining work flow
#else
            var file = await _mediaPicker.PickPhotoAsync();
            using (Stream s = file.GetStream())
            using (MemoryStream ms = new MemoryStream())
            {
              s.CopyTo(ms);
               byte[] data = ms.ToArray();
               Mat m = new Mat();
               CvInvoke.Imdecode(data, LoadImageType.Color, m );
               InvokeOnImageLoaded(m);
            }
#endif
         }
         else if (action.Equals("Camera"))
         {
#if __ANDROID__
            Android.Content.Intent intent = _mediaPicker.GetTakePhotoUI(new StoreCameraMediaOptions());
            Android.App.Activity activity = Forms.Context as Android.App.Activity;
            activity.StartActivityForResult(intent, PickImageRequestCode);
            //once the image was picked, the MainActivity.OnActivityResult function will handle the remaining work flow
#else
            var file = await _mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions());
            using (Stream s = file.GetStream())
            using (MemoryStream ms = new MemoryStream())
            {
               s.CopyTo(ms);
               byte[] data = ms.ToArray();
               Mat m = new Mat();
               CvInvoke.Imdecode(data, LoadImageType.Color, m);
               InvokeOnImageLoaded(m);
            }
#endif
         }
#endif
      }

	   public void InvokeOnImageLoaded(Mat image)
	   {
	      if (OnImageLoaded != null)
	         OnImageLoaded(this, image);
	   }

	   public event EventHandler<Mat> OnImageLoaded; 

      public void SetImage(IInputArray image)
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
