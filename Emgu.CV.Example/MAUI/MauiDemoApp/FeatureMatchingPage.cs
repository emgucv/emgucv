//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;

using FeatureMatchingExample;

namespace MauiDemoApp
{
    /// <summary>
    /// Finds a known object (box.png) in a scene with KAZE or SIFT features on the
    /// <see cref="StagePage"/> layout. The scene is the stage's input: a sample, or
    /// any photo the user picks or drops.
    /// </summary>
    public class FeatureMatchingPage : StagePage
    {
        private const string ModelFile = "box.png";
        private static readonly string[] Detectors = { "KAZE", "SIFT" };

        // Drawn into the result image: sky match lines, a lime outline, grey
        // unmatched keypoints. All read on both light and dark photos.
        private static readonly MCvScalar MatchColor = new MCvScalar(252, 211, 125);   // #7DD3FC, BGR
        private static readonly MCvScalar OutlineColor = new MCvScalar(58, 255, 212);  // #D4FF3A, BGR
        private static readonly MCvScalar PointColor = new MCvScalar(150, 150, 150);

        private Mat _model;
        private SegmentedControl _detectorControl;
        private volatile int _detector;   // 0 = KAZE, 1 = SIFT

        public FeatureMatchingPage() : base(new StageConfig
        {
            Title = "feature matching",
            Category = "Image & 3D",
            Module = "features module",
            Description = "Find a known object in a scene by matching keypoints.",
            About = "Finds the box from box.png inside a cluttered scene. A detector (KAZE or SIFT) picks out distinctive keypoints in both images and describes each one; the descriptors are matched, matches that aren't clearly better than their runner-up or that disagree on size and rotation are dropped, and a homography is fitted to what remains.\n\nThe homography maps the box's corners into the scene, drawn as the lime outline. The same technique underlies panorama stitching, object tracking and augmented reality.\n\nPick another sample, or add your own photo, to try a scene with or without the box.",
            Samples = new[]
            {
                new StageSample("box_in_scene.png", "box_in_scene.png"),
                new StageSample("lena.jpg", "lena.jpg"),
            },
            HasCamera = false,
            AutoRun = true,
            ShowResultCount = false,
            RunLabel = "Match",
            OptionsTitle = "Detector",
            ResultsTitle = "Matching",
            ItemSingular = "stat",
            ItemPlural = "stats",
            EmptyCaption = "match to see keypoints and matches here",
            MaxImageSize = 1280,
        })
        {
        }

        protected override View BuildOptions()
        {
            _detectorControl = new SegmentedControl(Detectors, _detector);
            _detectorControl.SelectionChanged += (s, e) =>
            {
                _detector = _detectorControl.SelectedIndex;
                SetOptionsValue(Detectors[_detector]);
                _ = RunAsync();   // generated demo: refresh right away
            };
            return _detectorControl;
        }

        protected override void OnStatusInit()
        {
            SetOptionsValue(Detectors[_detector]);
            SetStatus("Looking for " + ModelFile);
        }

        protected override async Task<bool> PrepareAsync()
        {
            if (_model != null)
                return true;
            try
            {
                byte[] bytes = await MaskRcnnPage.ReadAppFileAsync(ModelFile);
                var m = new Mat();
                CvInvoke.Imdecode(bytes, ImreadModes.ColorBgr, m);
                if (m.IsEmpty)
                {
                    m.Dispose();
                    SetStatus("Could not load " + ModelFile, DemoTheme.MutedText);
                    return false;
                }
                _model = m;
                return true;
            }
            catch (Exception)
            {
                SetStatus("Could not load " + ModelFile, DemoTheme.MutedText);
                return false;
            }
        }

        protected override StageResult RunCore(Mat input)
        {
            Mat model = _model;
            int detector = _detector;

            var match = Stopwatch.StartNew();
            using Feature2D fx = detector == 1 ? new SIFT() : new KAZE();
            using VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
            DrawMatches.FindMatch(model, input, fx, out _,
                out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint sceneKeyPoints,
                matches, out Mat mask, out Mat homography);
            match.Stop();

            try
            {
                var draw = Stopwatch.StartNew();
                int good = mask == null ? 0 : CvInvoke.CountNonZero(mask);
                bool found = homography != null;
                using Mat result = DrawMatches.Draw(model, modelKeyPoints, input, sceneKeyPoints, matches, mask, homography,
                    MatchColor, PointColor, OutlineColor, out _);
                byte[] png = MaskRcnnPage.Encode(result);
                draw.Stop();

                return new StageResult
                {
                    OutputPng = png,
                    OutputWidth = result.Width,
                    OutputHeight = result.Height,
                    Rows = new[]
                    {
                        new StageRow("Keypoints · box", modelKeyPoints.Size.ToString()),
                        new StageRow("Keypoints · scene", sceneKeyPoints.Size.ToString()),
                        new StageRow("Good matches", good.ToString(), null, DemoTheme.BoxColors[1]),
                        new StageRow("Homography", found ? "Found" : "Not found", null, found ? DemoTheme.Lime : DemoTheme.MutedText),
                    },
                    Note = found ? null : "Not enough consistent matches to locate the box in this scene.",
                    Summary = $"{(found ? "box found" : "not found")} · {match.Elapsed.TotalMilliseconds:0} ms",
                    InferenceMs = match.Elapsed.TotalMilliseconds,
                    PrePostMs = draw.Elapsed.TotalMilliseconds,
                };
            }
            finally
            {
                modelKeyPoints.Dispose();
                sceneKeyPoints.Dispose();
                mask?.Dispose();
                homography?.Dispose();
            }
        }

        protected override string CodeSnippet => $@"using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.CV.Util;
{(_detector == 0 ? "using Emgu.CV.XFeatures2D;\n" : "")}
using Mat box = CvInvoke.Imread(""box.png"", ImreadModes.ColorBgr);
using Mat scene = CvInvoke.Imread(""box_in_scene.png"", ImreadModes.ColorBgr);
using Feature2D detector = new {Detectors[_detector]}();

// Keypoints + descriptors for both images
using var boxPoints = new VectorOfKeyPoint();
using var scenePoints = new VectorOfKeyPoint();
using Mat boxDesc = new Mat(), sceneDesc = new Mat();
detector.DetectAndCompute(box, null, boxPoints, boxDesc, false);
detector.DetectAndCompute(scene, null, scenePoints, sceneDesc, false);

// Two nearest neighbours per scene descriptor
using var matches = new VectorOfVectorOfDMatch();
using var matcher = new FlannBasedMatcher(
    new LinearIndexParams(), new SearchParams());
matcher.Add(boxDesc);
matcher.KnnMatch(sceneDesc, matches, 2, null);

// Keep distinctive, consistent matches, then fit a homography
using Mat mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
mask.SetTo(new MCvScalar(255));
FeaturesToolbox.VoteForUniqueness(matches, 0.8, mask);
FeaturesToolbox.VoteForSizeAndOrientation(
    boxPoints, scenePoints, matches, mask, 1.5, 20);
Mat homography = FeaturesToolbox.GetHomographyMatrixFromMatchedFeatures(
    boxPoints, scenePoints, matches, mask, 2);
Console.WriteLine(homography != null ? ""box found"" : ""not found"");";

        protected override void OnClosed()
        {
            _model?.Dispose();
            _model = null;
        }
    }
}
