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
            //set { _displayImage = value; }
        }

        private StackLayout _mainLayout = new StackLayout();
        public StackLayout MainLayout
        {
            get { return _mainLayout; }
        }
        public ButtonTextImagePage()
        {
            TopButton.Text = "Click me";
            TopButton.IsEnabled = true;
            TopButton.HorizontalOptions = LayoutOptions.Center;

            MessageLabel.Text = "";
            MessageLabel.HorizontalOptions = LayoutOptions.Center;

            
            //DisplayImage.HeightRequest = 100;
            //DisplayImage.WidthRequest = 100;
            //DisplayImage.MinimumHeightRequest = 10;
            
            //StackLayout mainLayout = new StackLayout();
            _mainLayout.Children.Add(TopButton);
            _mainLayout.Children.Add(MessageLabel);
            _mainLayout.Children.Add(DisplayImage);
            _mainLayout.Children.Add(MessageLabel);
            _mainLayout.Padding = new Thickness( 10, 10, 10, 10);

            //MessageLabel.BackgroundColor = Color.AliceBlue;
            //DisplayImage.BackgroundColor = Color.Aqua;
            //_mainLayout.BackgroundColor = Color.Blue;
            
            Content = _mainLayout;
        }

        public bool HasCameraOption { get; set; }

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
                if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
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
                List<String> options = new List<string>();
                options.Add("Default");
                if (havePickImgOption)
                    options.Add("Photo Library");
                if (haveCameraOption)
                    options.Add("Photo from Camera");

#if __IOS__ || __ANDROID__
                if (this.HasCameraOption && haveCameraOption)
                    options.Add("Camera");
#endif
                if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows && Emgu.Util.Platform.ClrType != Emgu.Util.Platform.Clr.NetFxCore)
                {
                    options.Add("Camera");
                }

                if (options.Count == 1)
                {
                    action = "Default";
                }
                else
                {
                    action = await DisplayActionSheet(pickImgString, "Cancel", null, options.ToArray());
                    if (action == null) //user clicked outside of action sheet
                        return;
                }
                /*
                if (haveCameraOption & havePickImgOption)
                {
                    action = await DisplayActionSheet(pickImgString, "Cancel", null, "Default", "Photo Library",
                        "Photo from Camera");
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
                }*/

                if (action.Equals("Default"))
                {
#if __ANDROID__
                    mats[i] = Android.App.Application.Context.Assets.GetMat( imageNames[i] );
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
                    if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
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
                else if (action.Equals("Photo from Camera"))
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
                else if (action.Equals("Camera"))
                {
                    mats = new Mat[0];
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

        //private byte[] _imageData;
        //private MemoryStream _imageStream;
        public virtual void SetImage(IInputArray image)
        {
            if (image == null)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(
                    () =>
                    {
                        this.DisplayImage.Source = null;
                        this.DisplayImage.IsVisible = false;
                    });
                return;
            }
            
            int width = 0;
            int height = 0;
            using (InputArray iaImage = image.GetInputArray())
            {
                System.Drawing.Size s = iaImage.GetSize();
                width = s.Width;
                height = s.Height;
            }

            using (VectorOfByte vb = new VectorOfByte())
            {
                //CvInvoke.Imencode(".jpg", image, vb);
                CvInvoke.Imencode(".png", image, vb);
                byte[] rawData = vb.ToArray();
                //_imageData = vb.ToArray();
                //_imageStream = new MemoryStream(_imageData);
                Xamarin.Forms.Device.BeginInvokeOnMainThread(
                    () =>
                    {
                        this.DisplayImage.IsVisible = true;
                        this.DisplayImage.Source = ImageSource.FromStream(() => new MemoryStream(rawData));
                        
                        this.DisplayImage.WidthRequest = Math.Min(this.Width, width);
                        this.DisplayImage.HeightRequest = height;
                        //this.MainLayout.ForceLayout();
                        //this.ForceLayout();
                        //var bounds = this.DisplayImage.Bounds;
                    });
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
                    //label.WidthRequest = this.Width;
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
