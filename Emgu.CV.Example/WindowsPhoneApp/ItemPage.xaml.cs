using System.Drawing;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPhoneApp.Common;
using Emgu.CV.Windows.Phone.App.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Emgu.CV.Windows.Phone.App
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class ItemPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ItemPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        } 

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

       private async Task<Mat> LoadImage(String imageUri)
       {

          StorageFile file =
             await Package.Current.InstalledLocation.GetFileAsync(imageUri);
          using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
          
          {
             BitmapImage bmpImage = new BitmapImage();
             bmpImage.SetSource(fileStream);
             
             WriteableBitmap bmp = new WriteableBitmap(bmpImage.PixelWidth, bmpImage.PixelHeight);
             bmp.SetSource(await file.OpenAsync(FileAccessMode.Read));

             Mat img = new Mat(bmp);
            
             return img;

          }
       }

       /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var item = await SampleDataSource.GetItemAsync((string)e.NavigationParameter);
            this.DefaultViewModel["Item"] = item;
           if (item.Title.Equals("Run Hello World"))
           {
              Mat img = new Mat(200, 400, DepthType.Cv8U, 3);
              img.SetTo(new MCvScalar(255, 0, 0));
              CvInvoke.PutText(img, "Hello world.", new System.Drawing.Point(10, 80), FontFace.HersheyComplex, 1.0, new MCvScalar(0, 255, 0));

              ImageView.Source = img.ToWritableBitmap();
           } else if (item.Title.Equals("Run Planar Subdivision"))
           {
              Mat img = PlanarSubdivisionExample.DrawSubdivision.Draw(400, 30);
              ImageView.Source = img.ToWritableBitmap();
           } else if (item.Title.Equals("Run Face Detection"))
           {
              Mat img = await LoadImage(@"Assets\Images\lena.jpg");

              List<Rectangle> faces = new List<Rectangle>();
              List<Rectangle> eyes = new List<Rectangle>();
              long detectionTime;
              FaceDetection.DetectFace.Detect(img, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
                 faces, eyes, false, false, out detectionTime);

              foreach (Rectangle face in faces)
                 CvInvoke.Rectangle(img, face, new Bgr(0, 0, 255).MCvScalar, 2);
              foreach (Rectangle eye in eyes)
                 CvInvoke.Rectangle(img, eye, new Bgr(255, 0, 0).MCvScalar, 2);
              ImageView.Source = img.ToWritableBitmap();
            
           } else if (item.Title.Equals("Run Pedestrian Detection"))
           {
              Mat img = await LoadImage(@"Assets\Images\pedestrian.png");
              Mat gray = new Mat();
              CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
              long detectionTime;
              Rectangle[] pedestrians = PedestrianDetection.FindPedestrian.Find(gray, false, false, out detectionTime);
              foreach (Rectangle pedestrian in pedestrians)
              {
                 CvInvoke.Rectangle(img, pedestrian, new MCvScalar(0, 0, 255) );
              }
              ImageView.Source = img.ToWritableBitmap();
           }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
