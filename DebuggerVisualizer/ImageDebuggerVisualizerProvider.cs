using Microsoft.VisualStudio.Extensibility.DebuggerVisualizers;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Extensibility.UI;
using Microsoft.VisualStudio.RpcContracts.DebuggerVisualizers;
using Microsoft.Extensions.DependencyInjection;

using System.Drawing;
using System.Windows.Media.Imaging;

namespace Emgu.CV.DebuggerVisualizer
{
    /// <summary>
    /// Debugger visualizer provider class for <see cref="System.String"/>.
    /// </summary>
    [VisualStudioContribution]
    internal class ImageDebuggerVisualizerProvider : DebuggerVisualizerProvider
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageDebuggerVisualizerProvider"/> class.
        /// </summary>
        /// <param name="extension">Extension instance.</param>
        /// <param name="extensibility">Extensibility object.</param>
        public ImageDebuggerVisualizerProvider(ExtensionEntrypoint extension, VisualStudioExtensibility extensibility)
            : base(extension, extensibility)
        {
        }

        /// <inheritdoc/>
        public override DebuggerVisualizerProviderConfiguration DebuggerVisualizerProviderConfiguration => new("Mat Visualizer", typeof(Emgu.CV.Mat));

        /// <inheritdoc/>
        public override async Task<IRemoteUserControl> CreateVisualizerAsync(VisualizerTarget visualizerTarget, CancellationToken cancellationToken)
        {
            Mat targetObjectValue = await visualizerTarget.ObjectSource.RequestDataAsync<Mat>(jsonSerializer: null, cancellationToken);
            //BitmapSource bs = targetObjectValue.ToBitmapSource();
            var data = CvInvoke.Imencode(".png", targetObjectValue);
            
            //Bitmap bs = targetObjectValue.ToBitmap();
            return new Emgu.CV.DebuggerVisualizer.ImageVisualizerDialog(
                String.Format("Size: {0}x{1}", targetObjectValue.Width, targetObjectValue.Width),
                Convert.ToBase64String(data)
                );
        }
    }
}
