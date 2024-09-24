using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace Emgu.CV.DebuggerVisualizer;

[DataContract]
internal class ImageViewModel : INotifyPropertyChanged
{
    public ImageViewModel(String label, String imageFile)
    {
        Label = label;
        //ImageBase64 = base64Image;
        ImageFile = imageFile;

    }

    //private String _imageSource;
    private String _label;
    //private BitmapSource _bitmap;
    private String _imageFile;

    [DataMember]
    public String? Label
    {
        get
        {
            return _label; 
        }
        set
        {
            _label = value;
            OnPropertyChanged();
        }
    }

    [DataMember]
    public String? ImageFile
    {
        get
        {
            return _imageFile;
        }
        set
        {
            _imageFile = value;
            OnPropertyChanged();
        }
    }



    /*
    [DataMember]
    public String? ImageBase64
    {
        get
        {
            return _imageSource;
        }
        set
        {
            _imageSource = value;
            OnPropertyChanged();
            Image = ConvertBase64ToBitmapSource(value);
        }
    }
    
    
    //[DataMember]
    public BitmapSource Image
    {
        get
        {
            return _bitmap;
        }
        set
        {
            _bitmap = value;
            OnPropertyChanged();
        }
    }
    
    public static BitmapSource ConvertBase64ToBitmapSource(string base64Png)
    {
        // Step 1: Convert the Base64 string to a byte array
        byte[] imageBytes = Convert.FromBase64String(base64Png);

        // Step 2: Load the byte array into a MemoryStream
        using (MemoryStream ms = new MemoryStream(imageBytes))
        {
            // Step 3: Create a BitmapImage from the stream
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze(); // Freeze to make it cross-thread safe if needed

            return bitmapImage;
        }
    }*/

    public event PropertyChangedEventHandler PropertyChanged;
        
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}