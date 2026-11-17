//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

using Emgu.CV;

using PlanarSubdivisionExample;

namespace MauiDemoApp
{
    public class PlanarSubdivisionPage : SimpleDemoPage
    {
        private const int MaxValue = 600;

        private readonly Image _image = new Image { Aspect = Aspect.AspectFit, HeightRequest = 300, HorizontalOptions = LayoutOptions.Fill };
        // Text of the (non-tappable) "30 points" tag pill in the Subdivision header.
        private readonly Label _badge;
        private readonly Label _sliderValue = new Label { Text = "30 points", FontFamily = DemoTheme.TitleFont, FontSize = 15, TextColor = DemoTheme.PrimaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
        private int _pointCount = 30;

        public PlanarSubdivisionPage(string glyph)
            : base("Planar Subdivision", "Delaunay triangulation & Voronoi diagram", glyph, ImageCategory)
        {
            var badgePill = DemoTheme.TagPill("30 points");
            _badge = (Label)badgePill.Content;

            AddSection(new VerticalStackLayout
            {
                Spacing = 16,
                Children =
                {
                    SectionHeader("Subdivision", badgePill),
                    DemoTheme.ImageFrame(_image),
                    Caption("Colored Voronoi cells with the Delaunay triangulation overlaid. Each dot is a cell centre.")
                }
            });

            AddContent(PrimaryButton("New Points", MaskRcnnPage.GlyphPlay, (s, e) => Generate()));

            // ---- Point-count slider card ----
            var slider = new Slider { Minimum = 10, Maximum = 100, Value = 30, MinimumTrackColor = DemoTheme.Accent, MaximumTrackColor = DemoTheme.Hairline, ThumbColor = DemoTheme.Accent, HorizontalOptions = LayoutOptions.Fill };
            slider.ValueChanged += (s, e) =>
            {
                _pointCount = (int)Math.Round(e.NewValue);
                _sliderValue.Text = $"{_pointCount} points";
            };
            slider.DragCompleted += (s, e) => Generate();

            var sliderRow = new Grid { ColumnSpacing = 12, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            sliderRow.Add(new Label { Text = "10", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            sliderRow.Add(slider, 1, 0);
            sliderRow.Add(new Label { Text = "100", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center }, 2, 0);

            AddSection(new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    SectionHeader("Point Count", _sliderValue),
                    sliderRow,
                    Caption("Adjust the number of random points.")
                }
            });

            AddAbout("Scatters random points, then builds and draws the Delaunay triangulation and Voronoi diagram between them — a demo of OpenCV's computational geometry (Subdiv2D).");

            Generate();
        }

        private void Generate()
        {
            using (Mat m = DrawSubdivision.Draw(MaxValue, _pointCount))
                _image.Source = ToImageSource(m);
            _badge.Text = $"{_pointCount} points";
        }
    }
}
