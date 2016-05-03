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

      public virtual async void LoadImages(String[] imageNames, String[] labels = null)
      {
#if NETFX_CORE
         Mat[] mats = new Mat[imageNames.Length];
         for (int i = 0; i < mats.Length; i++)
            mats[i] = CvInvoke.Imread(imageNames[i], ImreadModes.Color);
         InvokeOnImagesLoaded(mats);
#else
         if (_mediaPicker == null)
         {
#if __ANDROID__
            _mediaPicker = new MediaPicker(Forms.Context);
#else
            _mediaPicker = new MediaPicker();
#endif
         }
         
         Mat[] mats = new Mat[imageNames.Length];
         for (int i = 0; i < mats.Length; i++)
         {
         String pickImgString = "Use Image from";
         if (labels != null && labels.Length > i)
            pickImgString = labels[i];
         var action = await (_mediaPicker.IsCameraAvailable? 
            DisplayActionSheet(pickImgString, "Cancel", null, "Default", "Photo Library", "Camera") 
            : DisplayActionSheet(pickImgString, "Cancel", null, "Default", "Photo Library")); 

         if (action.Equals("Default"))
         {
#if __ANDROID__
            mats[i] = new Mat(Forms.Context.Assets, imageNames[i]) ;
            
#else
            mats[i] = CvInvoke.Imread(imageNames[i], ImreadModes.AnyColor);
            
#endif

         }
         else if (action.Equals("Photo Library"))
         {
#if __ANDROID__
            Android.Content.Intent intent = _mediaPicker.GetPickPhotoUI();
            Android.App.Activity activity = Forms.Context as Android.App.Activity;
            activity.StartActivityForResult(intent, PickImageRequestCode);
            //once the image was picked, the MainActivity.OnActivityResult function will handle the remaining work flow
            Task t = new Task( () =>
               {_waitHandle.WaitOne();});
            t.Start();
            await t;
            if (MatHandle == null) //Cancelled
               return;
            mats[i] = MatHandle; 
#else
            var file = await _mediaPicker.PickPhotoAsync();
            using (Stream s = file.GetStream())
            using (MemoryStream ms = new MemoryStream())
            {
              s.CopyTo(ms);
               byte[] data = ms.ToArray();
               Mat m = new Mat();
               CvInvoke.Imdecode(data, ImreadModes.Color, m );
               mats[i] = m;      
         
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
           Task t = new Task( () =>
               {_waitHandle.WaitOne();});
            t.Start();
            await t;
            if (MatHandle == null) //Cancelled
               return;
            mats[i] = MatHandle; 
#else
            var file = await _mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions());
            using (Stream s = file.GetStream())
            using (MemoryStream ms = new MemoryStream())
            {
               s.CopyTo(ms);
               byte[] data = ms.ToArray();
               Mat m = new Mat();
               CvInvoke.Imdecode(data, ImreadModes.Color, m);
               mats[i] = m;
               
            }
#endif
         }
}
         InvokeOnImagesLoaded(mats);
#endif
      }

#if __ANDROID__
      private readonly System.Threading.EventWaitHandle _waitHandle = new System.Threading.AutoResetEvent(false);
      public void Continute()
      {
         _waitHandle.Set();
      }
      public Mat MatHandle;
#endif

      public void InvokeOnImagesLoaded(Mat[] images)
	   {
	      if (OnImagesLoaded != null)
	         OnImagesLoaded(this, images);
	   }

	   public event EventHandler<Mat[]> OnImagesLoaded; 

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
