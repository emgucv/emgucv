//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.XFeatures2D;

using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

using FeatureMatchingExample;

namespace MauiDemoApp
{
    public class FeatureMatchingPage : SimpleDemoPage
    {
        private static readonly Color Green = Color.FromArgb("#2BA84A");
        // Built-in Emgu sample images (file, friendly name, glyph) for either slot.
        private static readonly (string file, string name, string glyph)[] Samples =
        {
            ("box.png", "Box (object)", MaskRcnnPage.GlyphImage),
            ("box_in_scene.png", "Box on a desk", MaskRcnnPage.GlyphImage),
            ("lena.jpg", "Portrait", MaskRcnnPage.GlyphImage),
            ("dog416.png", "Dog & bike", MaskRcnnPage.GlyphDog),
            ("pedestrian.png", "Street scene", MaskRcnnPage.GlyphWalk),
        };

        private readonly Image _image = new Image { Aspect = Aspect.AspectFit, HeightRequest = 220, HorizontalOptions = LayoutOptions.Fill };
        private readonly Label _matchCount = new Label { Text = "—", FontFamily = MaskRcnnPage.TitleFont, FontSize = 22, TextColor = MaskRcnnPage.PrimaryText };
        private readonly Label _located = new Label { Text = "—", FontFamily = MaskRcnnPage.TitleFont, FontSize = 22, TextColor = MaskRcnnPage.SecondaryText };

        private readonly Image _modelThumb = new Image { WidthRequest = 46, HeightRequest = 46, Aspect = Aspect.AspectFill };
        private readonly Image _sceneThumb = new Image { WidthRequest = 46, HeightRequest = 46, Aspect = Aspect.AspectFill };
        private readonly Label _modelName = new Label { Text = "Box (object)", FontFamily = MaskRcnnPage.BodyFont, FontSize = 14, TextColor = MaskRcnnPage.SecondaryText, VerticalOptions = LayoutOptions.Center };
        private readonly Label _sceneName = new Label { Text = "Box on a desk", FontFamily = MaskRcnnPage.BodyFont, FontSize = 14, TextColor = MaskRcnnPage.SecondaryText, VerticalOptions = LayoutOptions.Center };

        private Mat _modelMat;
        private Mat _sceneMat;
        private int _feature;   // 0 = KAZE, 1 = SIFT
        private bool _busy;
        private bool _ran;

        public FeatureMatchingPage(string glyph)
            : base("Feature Matching", "Find an object in a scene using features", glyph)
        {
            var matchesStat = new VerticalStackLayout { Spacing = 2, Children = { new Label { Text = "Matches Found", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText }, _matchCount } };
            var locatedStat = new VerticalStackLayout { Spacing = 2, Children = { new Label { Text = "Object Located", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText }, _located } };
            var statInner = new Grid { ColumnSpacing = 16, ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) } };
            statInner.Add(matchesStat, 0, 0);
            statInner.Add(new BoxView { WidthRequest = 1, Color = MaskRcnnPage.RowBorder }, 1, 0);
            statInner.Add(locatedStat, 2, 0);
            var statCard = new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                Padding = new Thickness(16, 12),
                Content = statInner
            };

            var detectorRow = new Grid { ColumnSpacing = 14, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) } };
            detectorRow.Add(new Label { Text = "Detector", FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = MaskRcnnPage.PrimaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            detectorRow.Add(SegmentedToggle(new[] { "KAZE", "SIFT" }, 0, i => _feature = i), 1, 0);

            AddCard(Card(new VerticalStackLayout
            {
                Spacing = 14,
                Children =
                {
                    SectionHeader("Match Result", PillButton(MaskRcnnPage.GlyphImage, "Change Images", ChangeImages)),
                    ImageFrame(_image),
                    statCard,
                    detectorRow,
                    PrimaryButton("Match", MaskRcnnPage.GlyphPlay, (s, e) => Match())
                }
            }));

            // ---- Images card: pick model + scene from samples or the photo library ----
            AddCard(Card(new VerticalStackLayout
            {
                Spacing = 4,
                Children =
                {
                    SectionHeader("Images"),
                    ChooserRow("Model Image", _modelThumb, _modelName, () => PickImage(true)),
                    new BoxView { HeightRequest = 1, Color = MaskRcnnPage.RowBorder, Margin = new Thickness(0, 4) },
                    ChooserRow("Scene Image", _sceneThumb, _sceneName, () => PickImage(false))
                }
            }));

            AddAbout("Finds a known object inside a cluttered scene: it detects keypoints (KAZE or SIFT), matches them between the two images, and projects the object's outline where it is found — the basis of panorama stitching, object tracking, and AR.");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_ran)
                return;
            _ran = true;
            _modelMat = await LoadPackageImage("box.png");
            _sceneMat = await LoadPackageImage("box_in_scene.png");
            if (_modelMat != null) _modelThumb.Source = ToImageSource(_modelMat);
            if (_sceneMat != null) _sceneThumb.Source = ToImageSource(_sceneMat);
            Match();
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            if (!Navigation.NavigationStack.Contains(this) && !Navigation.ModalStack.Contains(this))
            {
                _modelMat?.Dispose(); _modelMat = null;
                _sceneMat?.Dispose(); _sceneMat = null;
            }
        }

        // A tappable "thumbnail + name + chevron" row.
        private View ChooserRow(string title, Image thumb, Label nameLabel, Action onTap)
        {
            var thumbFrame = new Border
            {
                WidthRequest = 46,
                HeightRequest = 46,
                BackgroundColor = MaskRcnnPage.ImageBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                Content = thumb
            };
            var texts = new VerticalStackLayout { Spacing = 1, VerticalOptions = LayoutOptions.Center, Children = { new Label { Text = title, FontFamily = MaskRcnnPage.TitleFont, FontSize = 16, TextColor = MaskRcnnPage.PrimaryText }, nameLabel } };
            var grid = new Grid { Padding = new Thickness(2, 10), ColumnSpacing = 14, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            grid.Add(thumbFrame, 0, 0);
            grid.Add(texts, 1, 0);
            grid.Add(MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphChevronRight, MaskRcnnPage.SecondaryText, 22), 2, 0);
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => onTap();
            grid.GestureRecognizers.Add(tap);
            return grid;
        }

        // The "Change Images" pill: pick which slot, then the image.
        private async void ChangeImages()
        {
            var slots = new System.Collections.Generic.List<(string glyph, string label, string value)>
            {
                (null, "IMAGE TO CHANGE", null),
                (MaskRcnnPage.GlyphImage, "Model Image", "model"),
                (MaskRcnnPage.GlyphImage, "Scene Image", "scene"),
            };
            string slot = await ShowChooser("Change Image", slots);
            if (slot == "model") PickImage(true);
            else if (slot == "scene") PickImage(false);
        }

        private async void PickImage(bool isModel)
        {
            var options = new System.Collections.Generic.List<(string glyph, string label, string value)>();
            options.Add((null, "SAMPLE IMAGES", null));
            foreach (var s in Samples)
                options.Add((s.glyph, s.name, "sample:" + s.file));
            options.Add((null, "YOUR PHOTOS", null));
            options.Add((MaskRcnnPage.GlyphCamera, "Photo Library", "library"));

            string choice = await ShowChooser(isModel ? "Model Image" : "Scene Image", options);
            if (string.IsNullOrEmpty(choice))
                return;

            Mat m;
            string name;
            if (choice == "library")
            {
                FileResult file = await MediaPicker.Default.PickPhotoAsync();
                if (file == null)
                    return;
                SetBusy(true, "Loading photo…");
                m = await LoadFileImage(file);
                SetBusy(false);
                name = "Library photo";
            }
            else
            {
                string file = choice.Substring("sample:".Length);
                m = await LoadPackageImage(file);
                name = file;
                foreach (var s in Samples)
                    if (s.file == file) { name = s.name; break; }
            }
            if (m == null)
            {
                await DisplayAlert("Feature Matching", "That image could not be loaded.", "OK");
                return;
            }

            if (isModel)
            {
                _modelMat?.Dispose(); _modelMat = m; _modelName.Text = name; _modelThumb.Source = ToImageSource(m);
            }
            else
            {
                _sceneMat?.Dispose(); _sceneMat = m; _sceneName.Text = name; _sceneThumb.Source = ToImageSource(m);
            }
            Match();
        }

        private async void Match()
        {
            if (_busy || _modelMat == null || _sceneMat == null)
                return;
            _busy = true;
            SetBusy(true, "Matching…");
            try
            {
                Mat model = _modelMat, scene = _sceneMat;
                int feature = _feature;
                var result = await Task.Run(() =>
                {
                    Feature2D fx = feature == 1 ? (Feature2D)new SIFT() : new KAZE();
                    Mat r = DrawMatches.Draw(model, scene, fx, out long time, out int count, out bool located);
                    fx.Dispose();
                    return (image: r, count, located);
                });

                _image.Source = ToImageSource(result.image);
                result.image.Dispose();

                _matchCount.Text = result.count.ToString();
                _matchCount.TextColor = MaskRcnnPage.PrimaryText;
                _located.Text = result.located ? "Yes" : "No";
                _located.TextColor = result.located ? Green : MaskRcnnPage.SecondaryText;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Feature Matching", ex.Message, "OK");
            }
            finally
            {
                _busy = false;
                SetBusy(false);
            }
        }

        private static async Task<Mat> LoadPackageImage(string name)
        {
            try
            {
                using Stream s = await FileSystem.OpenAppPackageFileAsync(name);
                using MemoryStream ms = new MemoryStream();
                await s.CopyToAsync(ms);
                return Decode(ms.ToArray());
            }
            catch { return null; }
        }

        private static async Task<Mat> LoadFileImage(FileResult file)
        {
            try
            {
                using Stream s = await file.OpenReadAsync();
                using MemoryStream ms = new MemoryStream();
                await s.CopyToAsync(ms);
                return Decode(ms.ToArray());
            }
            catch { return null; }
        }

        private static Mat Decode(byte[] bytes)
        {
            Mat m = new Mat();
            CvInvoke.Imdecode(bytes, ImreadModes.ColorBgr, m);
            if (m.IsEmpty) { m.Dispose(); return null; }
            return m;
        }
    }
}
