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
                    id: "DebuggerVisualizer.cffe27be-9cce-48fa-829b-a6a446aaa28b",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "Emgu Corporation",
                    displayName: "Emgu CV DebuggerVisualizer",
                    description: "Debugger Visualizer for Emgu CV"),
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.
        }
    }
}
