using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV
{
    /// <summary>
    /// Utilities class
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// The ColorPalette of Grayscale for Bitmap Format8bppIndexed
        /// </summary>
        public static readonly System.Drawing.Imaging.ColorPalette GrayscalePalette = GenerateGrayscalePalette();

        private static System.Drawing.Imaging.ColorPalette GenerateGrayscalePalette()
        {
            using (Bitmap image = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
            {
                System.Drawing.Imaging.ColorPalette palette = image.Palette;
                for (int i = 0; i < 256; i++)
                {
                    palette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
                }
                return palette;
            }
        }

        /*
        public static void GetModuleInfo()
        {
                        IntPtr version = IntPtr.Zero;
        IntPtr plugin_info = IntPtr.Zero;
        CvInvoke.cvGetModuleInfo(0, ref version, ref plugin_info);
        return (plugin_info != 0 && strstr(plugin_info, "ipp") != 0);
        }
        */ 

        /// <summary>
        /// Enable or diable IPL optimization for opencv
        /// </summary>
        /// <param name="enable">true to enable optimization, false to disable</param>
        public static void OptimizeCV(bool enable)
        {            
            CvInvoke.cvUseOptimized(enable? 1:0);
        }

        /// <summary>
        /// Determine if a small convex polygon is inside a larger convex polygon
        /// </summary>
        /// <typeparam name="T">The type of the depth</typeparam>
        /// <param name="smallPolygon">the smaller polygon</param>
        /// <param name="largePolygon">the larger polygon</param>
        /// <returns>
        /// true if the small convex polygon is inside the larger convex polygon;
        /// false otherwise.
        /// </returns>
        public static bool IsConvexPolygonInConvexPolygon<T>(IConvexPolygon<T> smallPolygon, IConvexPolygon<T> largePolygon) where T : IComparable, new()
        {
            Point2D<T>[] vertices = smallPolygon.Vertices;
            return Array.TrueForAll<Point2D<T>>(vertices, delegate(Point2D<T> v) { return v.InConvexPolygon(largePolygon); });
        }
    }
}
