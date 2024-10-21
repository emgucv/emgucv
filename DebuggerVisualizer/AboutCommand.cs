using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
//using Microsoft.VisualStudio.Extensibility.
using Microsoft.VisualStudio.Extensibility.Shell;
using System.Diagnostics;
using System.Reflection;

namespace Emgu.CV.DebuggerVisualizer
{


    /// <summary>
    /// About Command handler.
    /// </summary>
    [VisualStudioContribution]
    internal class AboutCommand : Command
    {


        private readonly TraceSource logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="traceSource">Trace source instance to utilize.</param>
        public AboutCommand(TraceSource traceSource)
        {
            // This optional TraceSource can be used for logging in the command. You can use dependency injection to access
            // other services here as well.
            this.logger = Requires.NotNull(traceSource, nameof(traceSource));
        }

        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration => new("%DebuggerVisualizer.AboutCommand.DisplayName%")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
            Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
            Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
        };

        /// <inheritdoc />
        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            // Use InitializeAsync for any one-time setup or initialization.
            return base.InitializeAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //assemblyVersion = Assembly.LoadFile("your assembly file").GetName().Version.ToString();

            String versionMsg = String.Format("Emgu CV Debugger Visualizer v{0}", assemblyVersion);
            await this.Extensibility.Shell().ShowPromptAsync(versionMsg, PromptOptions.OK, cancellationToken);
        }
    }
}
