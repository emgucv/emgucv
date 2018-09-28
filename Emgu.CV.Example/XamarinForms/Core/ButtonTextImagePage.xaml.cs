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
#if NETFX_CORE || (__UNIFIED__ && !__IOS__) //NETFX or Xamarin Mac
#else
using Plugin.Media;
using Plugin.Media.Abstractions;
#endif

namespace Emgu.CV.XamarinForms
{
    public partial class ButtonTextImagePage : ContentPage
    {

        public ButtonTextImagePage()
        {
            InitializeComponent();
        }

        public virtual async void LoadImages(String[] imageNames, String[] labels = null)
        {
#if NETFX_CORE || (__UNIFIED__ && !__IOS__) //NETFX or Xamarin Mac
            Mat[] mats = new Mat[imageNames.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = CvInvoke.Imread(imageNames[i], ImreadModes.Color);
            InvokeOnImagesLoaded(mats);
#else

            Mat[] mats = new Mat[imageNames.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                String pickImgString = "Use Image from";
                if (labels != null && labels.Length > i)
                    pickImgString = labels[i];
                bool haveCameraOption =
                    (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported);
                bool havePickImgOption =
                    CrossMedia.Current.IsPickVideoSupported;

                String action;
                if (haveCameraOption & havePickImgOption)
                {
                    action = await DisplayActionSheet(pickImgString, "Cancel", null, "Default", "Photo Library",
                        "Camera");
                }
                else if (havePickImgOption)
                {
                    action = await DisplayActionSheet(pickImgString, "Cancel", null, "Default", "Photo Library");
                }
                else
                {
                    action = "Default";
                }

                if (action.Equals("Default"))
                {
#if __ANDROID__
                    mats[i] = new Mat( Android.App.Application.Context.Assets, imageNames[i]);

#else
                    mats[i] = CvInvoke.Imread(imageNames[i], ImreadModes.AnyColor);
#endif

                }
                else if (action.Equals("Photo Library"))
                {
#if __ANDROID__ || __IOS__
                    var photoResult = await CrossMedia.Current.PickPhotoAsync();
                    if (photoResult == null) //cancelled
                        return;
                    mats[i] = CvInvoke.Imread(photoResult.Path);

#else
                    var file = await CrossMedia.Current.PickPhotoAsync();
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
#if __ANDROID__ || __IOS__
                    var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Emgu",
                        Name = $"{DateTime.UtcNow}.jpg"
                    };
                    var takePhotoResult = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                    if (takePhotoResult == null) //cancelled
                        return;


                    mats[i] = CvInvoke.Imread(takePhotoResult.Path);
#else
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
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

#if __MACOS__
                using (InputArray iaImage = image.GetInputArray())
                    this.DisplayImage.HeightRequest = iaImage.GetSize().Height;
#endif
            }
        }

        public Label GetLabel()
        {
            //return null;
            return this.MessageLabel;
        }

        public Button GetButton()
        {
            //return null;
            return this.TopButton;
        }

    }
}
