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
using Emgu.Util.TypeEnum;
#if !__MACOS__
using Plugin.Media;
#endif

namespace Emgu.CV.XamarinForms
{
    public class ButtonTextImagePage : Xamarin.Forms.ContentPage
    {
        private Button _topButton = new Button();
        public Button TopButton
        {
            get { return _topButton; }
        }

        private Label _messageLabel = new Label();
        public Label MessageLabel
        {
            get { return _messageLabel; }
        }

        private Image _displayImage = new Image();

        public Image DisplayImage
        {
            get { return _displayImage; }
        }

        public ButtonTextImagePage()
        {
            TopButton.Text = "Click me";
            TopButton.IsEnabled = true;
            TopButton.HorizontalOptions = LayoutOptions.Center;

            MessageLabel.Text = "";
            MessageLabel.HorizontalOptions = LayoutOptions.Center;

            StackLayout mainLayout = new StackLayout();
            mainLayout.Children.Add(TopButton);
            mainLayout.Children.Add(MessageLabel);
            mainLayout.Children.Add(DisplayImage);
            mainLayout.Padding = new Thickness( 10, 10, 10, 10);
            Content = mainLayout;
        }

        public virtual async void LoadImages(String[] imageNames, String[] labels = null)
        {
#if __MACOS__
            Mat[] mats = new Mat[imageNames.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = CvInvoke.Imread(imageNames[i], ImreadModes.Color);
            InvokeOnImagesLoaded(mats);
#else

#if __ANDROID__ || __IOS__
            await CrossMedia.Current.Initialize();
#endif

            Mat[] mats = new Mat[imageNames.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                String pickImgString = "Use Image from";
                if (labels != null && labels.Length > i)
                    pickImgString = labels[i];

                bool haveCameraOption;
                bool havePickImgOption;
                if (Emgu.Util.Platform.OperationSystem == OS.Windows)
                {
                    //CrossMedia is not implemented on Windows.
                    haveCameraOption = false;
                    havePickImgOption = true; //We will provide our implementation of pick image option
                }
                else
                {
                    haveCameraOption =
                        (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported);
                    havePickImgOption =
                        CrossMedia.Current.IsPickVideoSupported;
                }

                String action;
                if (haveCameraOption & havePickImgOption)
                {
                    action = await DisplayActionSheet(pickImgString, "Cancel", null, "Default", "Photo Library",
                        "Camera");
                    if (action == null) //user clicked outside of action sheet
                        return;
                }
                else if (havePickImgOption)
                {
                    action = await DisplayActionSheet(pickImgString, "Cancel", null, "Default", "Photo Library");
                    if (action == null) //user clicked outside of action sheet
                        return;
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
                    if (!File.Exists(imageNames[i]))
                        throw new FileNotFoundException(String.Format("File {0} do not exist.", imageNames[i]));
                    mats[i] = CvInvoke.Imread(imageNames[i], ImreadModes.AnyColor);
#endif

                }
                else if (action.Equals("Photo Library"))
                {

#if __ANDROID__ || __IOS__ || NETFX_CORE
                    var photoResult = await CrossMedia.Current.PickPhotoAsync();
                    if (photoResult == null) //canceled
                        return;
                    mats[i] = CvInvoke.Imread(photoResult.Path);

#else
                    if (Emgu.Util.Platform.OperationSystem == OS.Windows)
                    {
                        // our implementation of pick image
                        using (System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog())
                        {
                            dialog.Multiselect = false;
                            dialog.Title = "Select an Image File";
                            dialog.Filter = "Image | *.jpg;*.jpeg;*.png;*.bmp;*.gif | All Files | *";
                            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                mats[i] = CvInvoke.Imread(dialog.FileName, ImreadModes.AnyColor);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        var file = await CrossMedia.Current.PickPhotoAsync();
                        using (Stream s = file.GetStream())
                        using (MemoryStream ms = new MemoryStream())
                        {
                            s.CopyTo(ms);
                            byte[] data = ms.ToArray();
                            Mat m = new Mat();
                            CvInvoke.Imdecode(data, ImreadModes.Color, m);
                            mats[i] = m;
                        }
                    }
#endif
                }
                else if (action.Equals("Camera"))
                {
#if __ANDROID__ || __IOS__ || NETFX_CORE
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
                    var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Emgu",
                        Name = $"{DateTime.UtcNow}.jpg"
                    };
                    var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
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
#elif __IOS__
                //the following is needed for iOS due to the fact that
                //Xamarin Forms' Image object do not seems to refresh after we set the source.
                //Forcing the focus seems to force a rendering update.
                this.DisplayImage.Focus ();
#endif

            }
        }

        public Label GetLabel()
        {
            //return null;
            return this.MessageLabel;
        }

        public void SetMessage(String message)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(
                () =>
                {
                    Label label = GetLabel();
                    label.Text = message;

                    label.LineBreakMode = LineBreakMode.WordWrap;
                    label.WidthRequest = this.Width;
                }
            );
        }

        public Button GetButton()
        {
            //return null;
            return this.TopButton;
        }

    }
}
