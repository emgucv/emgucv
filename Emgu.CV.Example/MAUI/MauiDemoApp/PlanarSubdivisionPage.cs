//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

using Emgu.CV;

using Microsoft.Maui.Controls.Shapes;

using PlanarSubdivisionExample;

namespace MauiDemoApp
{
    public class PlanarSubdivisionPage : SimpleDemoPage
    {
        private const int MaxValue = 600;

        private readonly Image _image = new Image { Aspect = Aspect.AspectFit, HeightRequest = 300, HorizontalOptions = LayoutOptions.Fill };
        private readonly Label _badge = new Label { Text = "30 points", FontFamily = MaskRcnnPage.TitleFont, FontSize = 13, TextColor = MaskRcnnPage.Accent };
        private readonly Label _sliderValue = new Label { Text = "30 points", FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = MaskRcnnPage.Accent, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
        private int _pointCount = 30;

        public PlanarSubdivisionPage(string glyph)
            : base("Planar Subdivision", "Delaunay triangulation & Voronoi diagram", glyph)
        {
            var badgeBorder = new Border
            {
                BackgroundColor = MaskRcnnPage.TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(11) },
                Padding = new Thickness(12, 6),
                VerticalOptions = LayoutOptions.Center,
                Content = _badge
            };

            AddCard(Card(new VerticalStackLayout
            {
                Spacing = 14,
                Children =
                {
                    SectionHeader("Subdivision", badgeBorder),
                    ImageFrame(_image),
                    Caption("Colored Voronoi cells with the Delaunay triangulation overlaid. Each dot is a cell centre.")
                }
            }));

            AddCard(PrimaryButton("New Points", MaskRcnnPage.GlyphPlay, (s, e) => Generate()));

            // ---- Point-count slider card ----
            var slider = new Slider { Minimum = 10, Maximum = 100, Value = 30, MinimumTrackColor = MaskRcnnPage.Accent, ThumbColor = MaskRcnnPage.Accent, HorizontalOptions = LayoutOptions.Fill };
            slider.ValueChanged += (s, e) =>
            {
                _pointCount = (int)Math.Round(e.NewValue);
                _sliderValue.Text = $"{_pointCount} points";
            };
            slider.DragCompleted += (s, e) => Generate();

            var sliderRow = new Grid { ColumnSpacing = 12, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            sliderRow.Add(new Label { Text = "10", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            sliderRow.Add(slider, 1, 0);
            sliderRow.Add(new Label { Text = "100", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText, VerticalOptions = LayoutOptions.Center }, 2, 0);

            AddCard(Card(new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    SectionHeader("Point Count", _sliderValue),
                    sliderRow,
                    Caption("Adjust the number of random points.")
                }
            }));

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
