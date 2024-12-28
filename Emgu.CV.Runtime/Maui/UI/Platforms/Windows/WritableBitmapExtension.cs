#if WINDOWS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Platform.Maui.UI
{
    /// <summary>
    /// Extension methods for WritableBitmap
    /// </summary>
    public static class WriteableBitmapExtension
    {
        /*
        public static void ToArray(this Microsoft.UI.Xaml.Media.Imaging.WriteableBitmap writeableBitmap, IOutputArray outputArray)
        {
            byte[] data = new byte[writeableBitmap.PixelWidth * writeableBitmap.PixelHeight * 4];
            writeableBitmap.PixelBuffer.CopyTo(data);

            GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                using (Mat image = new Mat(
                    new System.Drawing.Size(writeableBitmap.PixelWidth, writeableBitmap.PixelHeight),
                    DepthType.Cv8U,
                    4,
                    dataHandle.AddrOfPinnedObject(),
                    writeableBitmap.PixelWidth * 4
                    ))
                {
                    CvInvoke.CvtColor(image, outputArray, ColorConversion.Bgra2Bgr);
                }
            }
            finally
            {
                dataHandle.Free();
            }
        }*/

        /// <summary>
        /// Convert an InputArray to WritableBitmap
        /// </summary>
        /// <param name="array">The InputArray to be converted to WritableBitmap</param>
        /// <returns>The writable bitmap</returns>
        /// <exception cref="NotImplementedException">Exception will be thrown if the specific input array cannot be converted to WritableBitmap</exception>
        public static Microsoft.UI.Xaml.Media.Imaging.WriteableBitmap ToWritableBitmap(this IInputArray array)
        {
            using (InputArray ia = array.GetInputArray())
            { 
                System.Drawing.Size size = ia.GetSize();

                int channels = ia.GetChannels();
                //size.Width = size.Width / channels;

                Microsoft.UI.Xaml.Media.Imaging.WriteableBitmap bmp = new Microsoft.UI.Xaml.Media.Imaging.WriteableBitmap(size.Width, size.Height);
                byte[] buffer = new byte[bmp.PixelWidth * bmp.PixelHeight * 4];
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                using (Mat resultImage = new Mat(
                    new System.Drawing.Size(bmp.PixelWidth, bmp.PixelHeight),
                    DepthType.Cv8U,
                    4,
                    handle.AddrOfPinnedObject(),
                    bmp.PixelWidth * 4))
                {
                    //int channels = ia.GetChannels();
                    switch (channels)
                    {
                        case 1:
                            CvInvoke.CvtColor(array, resultImage, ColorConversion.Gray2Bgra);
                            break;
                        case 3:
                            CvInvoke.CvtColor(array, resultImage, ColorConversion.Bgr2Bgra);
                            break;
                        case 4:
                            using (Mat m = ia.GetMat())
                                m.CopyTo(resultImage);
                            break;
                        default:
                            throw new NotImplementedException(String.Format(
                                "Conversion from {0} channel IInputArray to WritableBitmap is not supported",
                                channels));
                    }
                }
                handle.Free();
                using (Stream resultStream = System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeBufferExtensions.AsStream(bmp.PixelBuffer))
                //using (Stream resultStream = bmp.PixelBuffer.AsStream())
                {
                    resultStream.Write(buffer, 0, buffer.Length);
                }

                return bmp;
            }
        }


    }

}
#endif