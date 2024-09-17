using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace Emgu.CV.DebuggerVisualizer;

[DataContract]
internal class ImageViewModel : INotifyPropertyChanged
{
    private String _imageSource;
    private String _label;
        
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
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
        
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}