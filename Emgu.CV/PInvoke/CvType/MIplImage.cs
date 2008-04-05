using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to IplImage
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MIplImage
    {
        /// <summary>
        /// sizeof(IplImage) 
        /// </summary>
        public int nSize;
        /// <summary>
        /// version (=0)
        /// </summary>
        public int ID;
        /// <summary>
        /// Most of OpenCV functions support 1,2,3 or 4 channels 
        /// </summary>
        public int nChannels;
        /// <summary>
        /// ignored by OpenCV 
        /// </summary>
        public int alphaChannel;
        /// <summary>
        /// pixel depth in bits: IPL_DEPTH_8U, IPL_DEPTH_8S, IPL_DEPTH_16U, IPL_DEPTH_16S, IPL_DEPTH_32S, IPL_DEPTH_32F and IPL_DEPTH_64F are supported 
        /// </summary>
        public int depth;
        /// <summary>
        /// ignored by OpenCV 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] colorModel;
        /// <summary>
        /// ditto
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channelSeq;
        /// <summary>
        /// 0 - interleaved color channels, 1 - separate color channels.
        /// cvCreateImage can only create interleaved images 
        /// </summary>
        public int dataOrder;
        /// <summary>
        /// 0 - top-left origin,
        /// 1 - bottom-left origin (Windows bitmaps style)
        /// </summary>
        public int origin;
        /// <summary>
        /// Alignment of image rows (4 or 8).
        /// OpenCV ignores it and uses widthStep instead 
        /// </summary>
        public int align;
        /// <summary>
        /// image width in pixels 
        /// </summary>
        public int width;
        /// <summary>
        /// image height in pixels 
        /// </summary>
        public int height;
        /// <summary>
        /// image ROI. when it is not NULL, this specifies image region to process 
        /// </summary>
        public IntPtr roi;
        /// <summary>
        /// must be NULL in OpenCV 
        /// </summary>
        public IntPtr maskROI;
        /// <summary>
        /// ditto
        /// </summary>
        public IntPtr imageId;
        /// <summary>
        /// ditto 
        /// </summary>
        public IntPtr tileInfo;
        /// <summary>
        /// image data size in bytes
        /// (=image->height*image->widthStep in case of interleaved data)
        /// </summary>
        public int imageSize;
        /// <summary>
        /// pointer to aligned image data 
        /// </summary>
        public IntPtr imageData;
        /// <summary>
        /// size of aligned image row in bytes 
        /// </summary>
        public int widthStep;
        /// <summary>
        /// border completion mode, ignored by OpenCV 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] BorderMode;
        /// <summary>
        /// ditto
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] BorderConst;
        /// <summary>
        ///  pointer to a very origin of image data (not necessarily aligned) - it is needed for correct image deallocation 
        /// </summary>
        public IntPtr imageDataOrigin;
    }

    /// <summary>
    /// Offset in bytes for different fields of IplImage
    /// </summary>
    public static class IplImageOffset
    {
        /// <summary>
        /// offset of nSize
        /// </summary>
        public const int nSize = 0;
        /// <summary>
        /// Offset of ID
        /// </summary>
        public const int ID = nSize + sizeof(int);
        /// <summary>
        /// Offset of nchannels
        /// </summary>
        public const int nChannels = ID + sizeof(int);
        /// <summary>
        /// Offset of alpha Channel
        /// </summary>
        public const int alphaChannel = nChannels + sizeof(int);
        /// <summary>
        /// Offset of depth
        /// </summary>
        public const int depth = alphaChannel + sizeof(int);
        /// <summary>
        /// Offset of colorModel
        /// </summary>
        public const int colorModel = depth + sizeof(int);
        /// <summary>
        /// Offset of channelSeq
        /// </summary>
        public const int channelSeq = colorModel + 4 * sizeof(Byte);
        /// <summary>
        /// Offset of dataOrder
        /// </summary>
        public const int dataOrder = channelSeq + 4 * sizeof(Byte);
        /// <summary>
        /// Offset of origin
        /// </summary>
        public const int origin = dataOrder + sizeof(int);
        /// <summary>
        /// Offset of align
        /// </summary>
        public const int align = origin + sizeof(int);
        /// <summary>
        /// Offset of width
        /// </summary>
        public const int width = align + sizeof(int);
        /// <summary>
        /// Offset of height
        /// </summary>
        public const int height = width + sizeof(int);
        /// <summary>
        /// Offset of roi
        /// </summary>
        public const int roi = height + sizeof(int);
    }
}
