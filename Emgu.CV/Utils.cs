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
        /// Enable or diable IPL optimization of opencv
        /// </summary>
        /// <param name="flag">true to enable optimization, false to disable</param>
        public static void OptimizeCV(bool flag)
        {            
            CvInvoke.cvUseOptimized(flag? 1:0);
        }
    }
}
