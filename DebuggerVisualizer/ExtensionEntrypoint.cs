using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

namespace Emgu.CV.DebuggerVisualizer
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    internal class ExtensionEntrypoint : Extension
    {
        /// <inheritdoc/>
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            Metadata = new(
                    id: "com.emgu.cv.debuggervisualizer",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "Emgu Corporation",
                    displayName: "Debugger Visualizer for Emgu CV",
                    description: "Debugger Visualizer for Emgu CV"
                    ),
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.
        }
    }
}
