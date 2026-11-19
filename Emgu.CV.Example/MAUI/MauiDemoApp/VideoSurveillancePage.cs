//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using Emgu.CV;
using Emgu.CV.Models;

namespace MauiDemoApp
{
    /// <summary>
    /// Video surveillance (background subtraction + blob tracking) is camera-only:
    /// the stage shows an example scene and Run opens the live camera full screen.
    /// </summary>
    public class VideoSurveillancePage : StagePage
    {
        public VideoSurveillancePage() : base(new StageConfig
        {
            Title = "video surveillance",
            Category = "Video",
            Module = "video · bgsegm",
            Description = "Track moving objects from the camera, live.",
            About = "Learns what the static background looks like from the camera, then flags anything that moves and follows each moving blob from frame to frame (background subtraction + blob tracking). No neural network involved.\n\n" +
                    "Point the camera at a scene and keep the device still for a few seconds so it can learn the background. People or objects that then move through the scene get a box and an id.",
            Samples = new[] { new StageSample("pedestrian.png", "example scene") },
            HasCamera = true,
            RunLabel = "Open camera",
            RunOpensCamera = true,
            OptionsTitle = "Source",
            ResultsTitle = "How it works",
            EmptyTitle = "camera only",
            EmptyCaption = "this demo runs on the live camera; press Open camera",
        })
        {
        }

        protected override void OnStatusInit() => SetStatus("Uses the camera · nothing is recorded or uploaded");

        protected override string BackendName => "CPU · OpenCV";

        // Never called: Run opens the camera instead (RunOpensCamera).
        protected override StageResult RunCore(Mat input) => new StageResult();

        protected override async Task OpenLiveCameraAsync()
        {
#if __IOS__ || __MACCATALYST__
            await Navigation.PushModalAsync(new ModelShowcaseLivePage("Video Surveillance", () => new VideoSurveillanceModel(), null));
#else
            await Navigation.PushAsync(new Emgu.CV.Platform.Maui.UI.ProcessAndRenderPage(
                new VideoSurveillanceModel(), "Video Surveillance (Live)", null, "Track moving objects from the camera."));
#endif
        }

        protected override string CodeSnippet => @"using Emgu.CV;
using Emgu.CV.Models;

// VideoSurveillanceModel is in this demo app: background
// subtraction plus blob tracking over camera frames.
using var model = new VideoSurveillanceModel();
await model.Init(null, null);

using var capture = new VideoCapture(0);
using Mat frame = new Mat(), render = new Mat();
while (capture.Read(frame) && !frame.IsEmpty)
{
    frame.CopyTo(render);
    model.ProcessAndRender(frame, render);
    // show render…
}";
    }
}
