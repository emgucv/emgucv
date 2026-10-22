//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>
    /// Lists the open source components the demo app is built from, with the full
    /// license text of each one. The texts are not copied into this project: the
    /// csproj bundles each component's own license file straight from its source
    /// tree (opencv/, opencv_contrib/, the tesseract/leptonica/eigen/harfbuzz/hdf5/
    /// freetype2 submodules) as a MauiAsset under "licenses/", so they always
    /// match the sources the native runtime was compiled from.
    /// </summary>
    public class LicensePage : SimpleDemoPage
    {
        // The bundled Material Symbols font is a small subset (no gavel / expand_less),
        // so an expanded row shows expand_more rotated by 180 degrees.
        private const string GlyphExpandMore = "";   // expand_more

        // Shared license texts, for components whose own license file isn't in
        // this source tree (fetched at build time, or a NuGet package / font).
        private const string Apache2 = "licenses/Apache-2.0.txt";
        private const string Ofl = "licenses/OFL-1.1.txt";
        private const string OflStart = "This Font Software is licensed under the SIL Open Font License";

        private const string MitText =
            "Permission is hereby granted, free of charge, to any person obtaining a copy of this software and " +
            "associated documentation files (the \"Software\"), to deal in the Software without restriction, " +
            "including without limitation the rights to use, copy, modify, merge, publish, distribute, " +
            "sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is " +
            "furnished to do so, subject to the following conditions:\n\n" +
            "The above copyright notice and this permission notice shall be included in all copies or " +
            "substantial portions of the Software.\n\n" +
            "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING " +
            "BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND " +
            "NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, " +
            "DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, " +
            "OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

        private sealed class Component
        {
            public string Name;
            public string License;
            public string Url;
            /// <summary>Copyright notice shown above a shared license text.</summary>
            public string Notice;
            /// <summary>License files (MauiAsset logical names), shown in order.</summary>
            public string[] Files = Array.Empty<string>();
            /// <summary>Inline license text, for licenses with no file to bundle.</summary>
            public string Text;
            /// <summary>Show only the leading /* ... */ comment of each file (license in a source header).</summary>
            public bool HeaderComment;
            /// <summary>Skip each file's text before this marker (drops another project's copyright line).</summary>
            public string StartAt;
        }

        private static readonly Component[] EmguComponents =
        {
            new Component { Name = "Emgu CV", License = "GPL v3 or Emgu CV commercial license", Url = "https://www.emgu.com", Files = new[] { "licenses/emgucv.txt" } },
        };

        private static readonly Component[] NativeComponents =
        {
            new Component { Name = "OpenCV", License = "Apache-2.0", Url = "https://opencv.org", Files = new[] { "licenses/opencv-copyright.txt", Apache2 } },
            new Component { Name = "OpenCV contrib modules", License = "Apache-2.0", Url = "https://github.com/opencv/opencv_contrib", Files = new[] { Apache2 } },
            new Component { Name = "Tesseract OCR", License = "Apache-2.0", Url = "https://github.com/tesseract-ocr/tesseract", Files = new[] { Apache2 } },
            new Component { Name = "Leptonica", License = "BSD-2-Clause", Url = "http://www.leptonica.org", Files = new[] { "licenses/leptonica.txt" } },
            new Component { Name = "Eigen", License = "MPL-2.0", Url = "https://eigen.tuxfamily.org", Files = new[] { "licenses/eigen.txt" } },
            new Component { Name = "libjpeg-turbo", License = "IJG, BSD-3-Clause, Zlib", Url = "https://libjpeg-turbo.org", Files = new[] { "licenses/libjpeg-turbo.txt", "licenses/libjpeg-turbo-ijg.txt" } },
            new Component { Name = "libpng", License = "PNG Reference Library License v2", Url = "http://www.libpng.org", Files = new[] { "licenses/libpng.txt" } },
            new Component { Name = "zlib", License = "Zlib", Url = "https://zlib.net", Files = new[] { "licenses/zlib.txt" } },
            new Component { Name = "LibTIFF", License = "libtiff", Url = "https://libtiff.gitlab.io/libtiff/", Files = new[] { "licenses/libtiff.txt" } },
            new Component { Name = "libwebp", License = "BSD-3-Clause", Url = "https://chromium.googlesource.com/webm/libwebp", Files = new[] { "licenses/libwebp.txt" } },
            new Component { Name = "OpenJPEG", License = "BSD-2-Clause", Url = "https://www.openjpeg.org", Files = new[] { "licenses/openjpeg.txt" } },
            new Component { Name = "Protocol Buffers", License = "BSD-3-Clause", Url = "https://github.com/protocolbuffers/protobuf", Files = new[] { "licenses/protobuf.txt" } },
            new Component { Name = "FlatBuffers", License = "Apache-2.0", Url = "https://github.com/google/flatbuffers", Notice = "Copyright 2014 Google Inc. All rights reserved.", Files = new[] { Apache2 } },
            new Component { Name = "MLAS (ONNX Runtime)", License = "MIT", Url = "https://github.com/microsoft/onnxruntime", Files = new[] { "licenses/mlas.txt" } },
            new Component { Name = "Intel ITT API (ittnotify)", License = "BSD-3-Clause", Url = "https://github.com/intel/ittapi", Files = new[] { "licenses/ittnotify.txt" } },
            new Component { Name = "Berkeley SoftFloat", License = "BSD-3-Clause", Url = "http://www.jhauser.us/arithmetic/SoftFloat.html", Files = new[] { "licenses/softfloat.txt" } },
            new Component { Name = "Annoy", License = "Apache-2.0", Url = "https://github.com/spotify/annoy", Files = new[] { "licenses/annoy.txt", Apache2 } },
            new Component { Name = "MSCR chi-square table", License = "BSD-3-Clause", Url = "https://github.com/opencv/opencv", Files = new[] { "licenses/mscr.txt" } },
            new Component { Name = "KAZE / AKAZE", License = "BSD-3-Clause", Url = "https://github.com/pablofdezalc/akaze", Files = new[] { "licenses/kaze.txt", "licenses/akaze.txt" } },
            new Component { Name = "Carotene (NVIDIA)", License = "BSD-3-Clause", Url = "https://github.com/opencv/opencv/tree/5.x/hal/carotene", Files = new[] { "licenses/carotene.txt" }, HeaderComment = true },
            new Component { Name = "KleidiCV (Arm)", License = "Apache-2.0", Url = "https://gitlab.arm.com/kleidi/kleidicv", Notice = "Copyright 2023 - 2026 Arm Limited and/or its affiliates <open-source-office@arm.com>", Files = new[] { Apache2 } },
            new Component { Name = "ADE Framework", License = "Apache-2.0", Url = "https://github.com/opencv/ade", Notice = "Copyright (C) 2018 Intel Corporation", Files = new[] { Apache2 } },
            new Component { Name = "Rubik font", License = "OFL-1.1", Url = "https://github.com/googlefonts/rubik", Files = new[] { Ofl } },
            new Component { Name = "FreeType", License = "FreeType License (FTL)", Url = "https://freetype.org", Files = new[] { "licenses/freetype.txt" } },
            new Component { Name = "HarfBuzz", License = "Old MIT", Url = "https://harfbuzz.github.io", Files = new[] { "licenses/harfbuzz.txt" } },
            new Component { Name = "HDF5", License = "BSD-3-Clause", Url = "https://www.hdfgroup.org", Files = new[] { "licenses/hdf5.txt" } },
            new Component { Name = "cpufeatures (Android)", License = "Apache-2.0", Url = "https://android.googlesource.com/platform/ndk", Files = new[] { "licenses/cpufeatures.txt", Apache2 } },
        };

        private static readonly Component[] AppComponents =
        {
            new Component { Name = ".NET and .NET MAUI", License = "MIT", Url = "https://github.com/dotnet/maui", Notice = "Copyright (c) .NET Foundation and Contributors", Text = MitText },
            new Component { Name = "Inter font", License = "OFL-1.1", Url = "https://github.com/rsms/inter", Notice = "Copyright 2016 The Inter Project Authors (https://github.com/rsms/inter)", Files = new[] { Ofl }, StartAt = OflStart },
            new Component { Name = "Open Sans font", License = "Apache-2.0", Url = "https://github.com/googlefonts/opensans", Notice = "Digitized data copyright © 2010-2011, Google Corporation.", Files = new[] { Apache2 } },
            new Component { Name = "Material Symbols", License = "Apache-2.0", Url = "https://github.com/google/material-design-icons", Notice = "Copyright 2026 Google LLC. All Rights Reserved.", Files = new[] { Apache2 } },
        };

        public LicensePage()
            : base("Licenses", "Open source components", MaskRcnnPage.GlyphInfo)
        {
            AddCard(BuildSection("Emgu CV", null, EmguComponents));
            AddCard(BuildSection("Native runtime",
                "Compiled into the Emgu CV native library (cvextern). The exact set depends on the platform and build configuration.",
                NativeComponents));
            AddCard(BuildSection("App framework & fonts", null, AppComponents));
            AddCard(Card(Caption("Models that individual demos download at run time are not part of the app; each is distributed under its publisher's own license.")));
        }

        private static View BuildSection(string title, string caption, Component[] components)
        {
            var stack = new VerticalStackLayout { Spacing = 4 };
            stack.Children.Add(SectionHeader(title));
            if (caption != null)
                stack.Children.Add(Caption(caption));
            var rows = new VerticalStackLayout { Spacing = 0, Margin = new Thickness(0, 6, 0, 0) };
            for (int i = 0; i < components.Length; i++)
                rows.Children.Add(BuildRow(components[i], i + 1 < components.Length));
            stack.Children.Add(rows);
            return Card(stack);
        }

        private static View BuildRow(Component c, bool divider)
        {
            var chevron = MaskRcnnPage.MakeIcon(GlyphExpandMore, MaskRcnnPage.SecondaryText, 22);
            var header = new Grid
            {
                Padding = new Thickness(0, 12),
                ColumnSpacing = 12,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            header.Add(new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    new Label { Text = c.Name, FontFamily = MaskRcnnPage.TitleFont, FontSize = 16, TextColor = MaskRcnnPage.PrimaryText },
                    new Label { Text = c.License, FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.Accent }
                }
            }, 0, 0);
            chevron.VerticalOptions = LayoutOptions.Center;
            header.Add(chevron, 1, 0);

            // The license text is only loaded and laid out on first expand: some
            // (GPL v3, MPL-2.0) are tens of KB, and most rows are never opened.
            var details = new VerticalStackLayout { Spacing = 10, IsVisible = false, Padding = new Thickness(0, 0, 0, 14) };
            bool loaded = false;

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                details.IsVisible = !details.IsVisible;
                chevron.Rotation = details.IsVisible ? 180 : 0;
                if (details.IsVisible && !loaded)
                {
                    loaded = true;
                    await FillDetails(details, c);
                }
            };
            header.GestureRecognizers.Add(tap);

            var stack = new VerticalStackLayout { Children = { header, details } };
            if (divider)
                stack.Children.Add(new BoxView { HeightRequest = 1, Color = MaskRcnnPage.RowBorder });
            return stack;
        }

        private static async Task FillDetails(VerticalStackLayout details, Component c)
        {
            var link = new Label { Text = c.Url, FontFamily = MaskRcnnPage.BodyFont, FontSize = 14, TextColor = MaskRcnnPage.Accent, TextDecorations = TextDecorations.Underline };
            var linkTap = new TapGestureRecognizer();
            linkTap.Tapped += async (s, e) =>
            {
                try { await Launcher.Default.OpenAsync(new Uri(c.Url)); } catch { }
            };
            link.GestureRecognizers.Add(linkTap);
            details.Children.Add(link);

            if (c.Notice != null)
                details.Children.Add(new Label { Text = c.Notice, FontFamily = MaskRcnnPage.TitleFont, FontSize = 13, TextColor = MaskRcnnPage.PrimaryText });

            var texts = new List<string>();
            if (c.Text != null)
                texts.Add(c.Text);
            foreach (string file in c.Files)
                texts.Add(await ReadLicense(file, c));

            foreach (string text in texts)
                details.Children.Add(LicenseText(text));
        }

        private static Border LicenseText(string text) => new Border
        {
            BackgroundColor = MaskRcnnPage.ImageBackground,
            Stroke = MaskRcnnPage.RowBorder,
            StrokeThickness = 1,
            Padding = new Thickness(12),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
            Content = new Label { Text = text, FontSize = 12, TextColor = MaskRcnnPage.PrimaryText, LineBreakMode = LineBreakMode.WordWrap }
        };

        private static async Task<string> ReadLicense(string file, Component c)
        {
            string text;
            try
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync(file);
                using var reader = new StreamReader(stream);
                text = await reader.ReadToEndAsync();
            }
            catch (Exception)
            {
                // The csproj only bundles license files that exist in the source tree.
                return String.Format("The license text was not bundled with this build. See {0}", c.Url);
            }

            text = text.Replace("\r\n", "\n");
            if (c.StartAt != null)
            {
                int start = text.IndexOf(c.StartAt, StringComparison.Ordinal);
                if (start > 0)
                    text = text.Substring(start);
            }
            if (c.HeaderComment)
                text = ExtractHeaderComment(text);
            return text.Trim();
        }

        /// <summary>The first /* ... */ block of a source file, without the leading " * " of each line.</summary>
        private static string ExtractHeaderComment(string source)
        {
            int start = source.IndexOf("/*", StringComparison.Ordinal);
            int end = start < 0 ? -1 : source.IndexOf("*/", start + 2, StringComparison.Ordinal);
            if (end < 0)
                return source;
            var lines = source.Substring(start + 2, end - start - 2).Split('\n');
            var sb = new System.Text.StringBuilder();
            foreach (string line in lines)
            {
                string l = line.TrimStart();
                if (l.StartsWith("*"))
                    l = l.Substring(1);
                if (l.StartsWith(" "))
                    l = l.Substring(1);
                sb.Append(l.TrimEnd()).Append('\n');
            }
            return sb.ToString();
        }
    }
}
