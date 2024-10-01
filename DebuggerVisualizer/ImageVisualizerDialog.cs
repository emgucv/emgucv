using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.Extensibility.UI;

namespace Emgu.CV.DebuggerVisualizer
{

    internal sealed class ImageVisualizerDialog :
        RemoteUserControl
    {
        
        public ImageVisualizerDialog(String label, String imageFile)
            : base(dataContext: new ImageViewModel(label, imageFile))
        {
        }

        /*
        public class Base64ToImageConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is string base64String && !string.IsNullOrEmpty(base64String))
                {
                    try
                    {
                        // Remove any data URL prefix (e.g., "data:image/png;base64,")
                        if (base64String.Contains(","))
                        {
                            base64String = base64String.Substring(base64String.IndexOf(",") + 1);
                        }

                        byte[] imageBytes = System.Convert.FromBase64String(base64String);
                        BitmapImage bitmap = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(imageBytes))
                        {
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.StreamSource = stream;
                            bitmap.EndInit();
                        }
                        return bitmap;
                    }
                    catch (Exception)
                    {
                        // Handle conversion error (optional)
                        return null;
                    }
                }
                return null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
        */
    }

}