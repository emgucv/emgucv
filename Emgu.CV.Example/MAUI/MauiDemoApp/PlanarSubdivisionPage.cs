//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.Structure;

using PlanarSubdivisionExample;

namespace MauiDemoApp
{
    /// <summary>
    /// Random points, their Delaunay triangulation and Voronoi diagram, on the
    /// <see cref="StagePage"/> layout. Every run scatters a new set of points.
    /// </summary>
    public class PlanarSubdivisionPage : StagePage
    {
        private const int MaxValue = 600;
        private static readonly int[] PointCounts = { 10, 30, 60, 100 };

        private SegmentedControl _countControl;
        private volatile int _pointCount = 30;

        public PlanarSubdivisionPage() : base(new StageConfig
        {
            Title = "planar",
            Category = "Image & 3D",
            Module = "imgproc · Subdiv2D",
            Description = "Delaunay triangulation and Voronoi diagram of random points.",
            About = "Scatters random points, then builds a planar subdivision of them with OpenCV's Subdiv2D. The white lines are the Delaunay triangulation: it connects the points into triangles, avoiding long thin ones. The colored cells are the Voronoi diagram: each cell holds every place that is closer to its point (the red dot) than to any other.\n\nThe two are duals of each other, and both are used for meshes, interpolation and nearest-neighbour lookups. Each run makes a new random set.",
            HasCamera = false,
            AutoRun = true,
            ShowResultCount = false,
            RunLabel = "Generate",
            OptionsTitle = "Points",
            ResultsTitle = "Subdivision",
            ItemSingular = "stat",
            ItemPlural = "stats",
            EmptyCaption = "generate to see the counts here",
        })
        {
        }

        protected override View BuildOptions()
        {
            _countControl = new SegmentedControl(PointCounts.Select(c => c.ToString()), Array.IndexOf(PointCounts, _pointCount));
            _countControl.SelectionChanged += (s, e) =>
            {
                _pointCount = PointCounts[_countControl.SelectedIndex];
                SetOptionsValue($"{_pointCount} points");
                _ = RunAsync();   // generated demo: refresh right away
            };
            return _countControl;
        }

        protected override void OnStatusInit()
        {
            SetOptionsValue($"{_pointCount} points");
            SetStatus("New random points every run");
        }

        protected override StageResult RunCore(Mat input)
        {
            int count = _pointCount;
            var compute = Stopwatch.StartNew();
            DrawSubdivision.CreateSubdivision(MaxValue, count, out Triangle2DF[] triangles, out VoronoiFacet[] facets);
            compute.Stop();

            var draw = Stopwatch.StartNew();
            using Mat img = DrawSubdivision.Draw(MaxValue, triangles, facets);
            byte[] png = MaskRcnnPage.Encode(img);
            draw.Stop();

            return new StageResult
            {
                OutputPng = png,
                OutputWidth = img.Width,
                OutputHeight = img.Height,
                Rows = new[]
                {
                    // Swatches match what's drawn: red dots, white (here grey, so it shows
                    // in light mode) triangle edges, colored cells.
                    new StageRow("Points", count.ToString(), null, Color.FromArgb("#FF3B30")),
                    new StageRow("Delaunay triangles", triangles.Length.ToString(), null, Color.FromArgb("#BDBDBD")),
                    new StageRow("Voronoi cells", facets.Length.ToString(), null, DemoTheme.BoxColors[1]),
                },
                Summary = $"{triangles.Length} triangles · {compute.Elapsed.TotalMilliseconds:0.0} ms",
                InferenceMs = compute.Elapsed.TotalMilliseconds,
                PrePostMs = draw.Elapsed.TotalMilliseconds,
            };
        }

        protected override string CodeSnippet => $@"using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

// {_pointCount} random points in a {MaxValue} x {MaxValue} square
var rng = new Random();
PointF[] points = new PointF[{_pointCount}];
for (int i = 0; i < points.Length; i++)
    points[i] = new PointF(
        (float)rng.NextDouble() * {MaxValue},
        (float)rng.NextDouble() * {MaxValue});

using Subdiv2D subdivision = new Subdiv2D(points);
Triangle2DF[] triangles = subdivision.GetDelaunayTriangles();
VoronoiFacet[] cells = subdivision.GetVoronoiFacets();

Console.WriteLine(
    $""{{triangles.Length}} triangles, {{cells.Length}} cells"");";
    }
}
