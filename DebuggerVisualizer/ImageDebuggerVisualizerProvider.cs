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
//using System.Windows.Media.Imaging;

namespace Emgu.CV.DebuggerVisualizer
{
    /// <summary>
    /// Debugger visualizer provider class for Mat and UMat
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
        public override DebuggerVisualizerProviderConfiguration DebuggerVisualizerProviderConfiguration =>
            new DebuggerVisualizerProviderConfiguration(
                new VisualizerTargetType("Emgu CV Visualizer", typeof(Emgu.CV.Mat)),
                new VisualizerTargetType("Emgu CV Visualizer", typeof(Emgu.CV.UMat))
                );
        

        /// <inheritdoc/>
        public override async Task<IRemoteUserControl> CreateVisualizerAsync(VisualizerTarget visualizerTarget, CancellationToken cancellationToken)
        { 
            //Both Mat & UMat are JSON serialized to Mat
            Mat mat = await visualizerTarget.ObjectSource.RequestDataAsync<Mat>(jsonSerializer: null, cancellationToken);
            if (mat != null)
            {
                // Create a temporary file path
                string tempPngFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".png");

                // Write the image to temporary file
                CvInvoke.Imwrite(tempPngFilePath, mat);

                return new Emgu.CV.DebuggerVisualizer.ImageVisualizerDialog(
                    String.Format("Size: {0}x{1}", mat.Width, mat.Width),
                    tempPngFilePath
                );
            }

            /*
            UMat umat = await visualizerTarget.ObjectSource.RequestDataAsync<UMat>(jsonSerializer: null, cancellationToken);
            if (umat != null)
            {
                // Create a temporary file path
                string tempPngFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".png");

                // Write the image to temporary file
                CvInvoke.Imwrite(tempPngFilePath, umat);

                return new Emgu.CV.DebuggerVisualizer.ImageVisualizerDialog(
                    String.Format("UMat: {0}x{1}", umat.Size.Height, umat.Size.Width),
                    tempPngFilePath
                );
            }*/

            return null;
            /*
            //BitmapSource bs = targetObjectValue.ToBitmapSource();
            var data = CvInvoke.Imencode(".png", targetObjectValue);

            //Bitmap bs = targetObjectValue.ToBitmap();
            return new Emgu.CV.DebuggerVisualizer.ImageVisualizerDialog(
                String.Format("Size: {0}x{1}", targetObjectValue.Width, targetObjectValue.Width),
                Convert.ToBase64String(data)
                );
            */
        }
    }
}
