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

            Content = new ScrollView()
            {
                Content = _mainLayout
            };
        }

        public bool HasCameraOption { get; set; }

        public virtual async Task<Mat[]> LoadImages(String[] imageNames, String[] labels = null)
        {
            Mat[] mats = new Mat[imageNames.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                String pickImgString = "Use Image from";
                if (labels != null && labels.Length > i)
                    pickImgString = labels[i];

                bool captureSupported;
                
                if (Device.RuntimePlatform == Device.WPF
                    || Device.RuntimePlatform == Device.macOS)
                {
                    //Pick image from camera is not implemented on WPF.
                    captureSupported = false;
                }
                else
                {
                    captureSupported = Xamarin.Essentials.MediaPicker.IsCaptureSupported;
                }

                String action;
                List<String> options = new List<string>();
                options.Add("Default");

                options.Add("Photo Library");

                if (captureSupported)
                    options.Add("Photo from Camera");

                if (Device.RuntimePlatform == Device.Android
                    || Device.RuntimePlatform == Device.iOS
                    || Device.RuntimePlatform == Device.UWP)
                {
                    if (this.HasCameraOption && captureSupported)
                        options.Add("Camera");
                } else if (Device.RuntimePlatform == Device.WPF)
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
                    if (
                        action == null //user clicked outside of action sheet
                        || action.Equals("Cancel") // user cancel
                    )
                        return null;
                }
                
                if (action.Equals("Default"))
                {
#if __ANDROID__
                    mats[i] = Android.App.Application.Context.Assets.GetMat( imageNames[i] );
#else
                    if (!File.Exists(imageNames[i]))
                        throw new FileNotFoundException(String.Format("File {0} do not exist.", imageNames[i]));
                    mats[i] = CvInvoke.Imread(imageNames[i], ImreadModes.Color);
#endif
                }
                else if (action.Equals("Photo Library"))
                {
                    if (Device.RuntimePlatform == Device.WPF)
                    {
#if !( __MACOS__ || __ANDROID__ || __IOS__ || NETFX_CORE )
                        Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                        dialog.Multiselect = false;
                        dialog.Title = "Select an Image File";
                        dialog.Filter = "Image | *.jpg;*.jpeg;*.png;*.bmp;*.gif | All Files | *";
                        if (dialog.ShowDialog() == false)
                            return null;
                        mats[i] = CvInvoke.Imread(dialog.FileName, ImreadModes.AnyColor);
#endif
                    }
                    else
                    {
                        var fileResult = await Xamarin.Essentials.FilePicker.PickAsync(Xamarin.Essentials.PickOptions.Images);
                        if (fileResult == null) //canceled
                            return null;
                        using (Stream s = await fileResult.OpenReadAsync())
                            mats[i] = await ReadStream(s);
                    }
                }
                else if (action.Equals("Photo from Camera"))
                {
                    var takePhotoResult = await Xamarin.Essentials.MediaPicker.CapturePhotoAsync();
                    
                    if (takePhotoResult == null) //canceled
                        return null;
                    using (Stream stream = await takePhotoResult.OpenReadAsync())
                        mats[i] = await ReadStream(stream);
                }
                else if (action.Equals("Camera"))
                {
                    mats = new Mat[0];
                }
            }

            return mats;
        }

        private static async Task<Mat> ReadStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                byte[] data = ms.ToArray();
                Mat m = new Mat();
                CvInvoke.Imdecode(data, ImreadModes.Color, m);
                return m;
            }
        }

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
