//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    
        /// <summary>
        /// Video codecs supported by VideoReader
        /// </summary>
        public enum CudaCodec
        {
            /// <summary>
            /// MPEG1
            /// </summary>
            MPEG1 = 0,
            /// <summary>
            /// MPEG2
            /// </summary>
            MPEG2,
            /// <summary>
            /// MPEG4
            /// </summary>
            MPEG4,
            /// <summary>
            /// VC1
            /// </summary>
            VC1,
            /// <summary>
            /// H264
            /// </summary>
            H264,
            /// <summary>
            /// JPEG
            /// </summary>
            JPEG,
            /// <summary>
            /// H264_SVC
            /// </summary>
            H264_SVC,
            /// <summary>
            /// H264_MVC
            /// </summary>
            H264_MVC,
            /// <summary>
            /// HEVC
            /// </summary>
            HEVC,
            /// <summary>
            /// VP8
            /// </summary>
            VP8,
            /// <summary>
            /// VP9
            /// </summary>
            VP9,
            /// <summary>
            /// Number of codecs
            /// </summary>
            NumCodecs,
            /// <summary>
            /// Y,U,V (4:2:0)
            /// </summary>
            Uncompressed_YUV420 = (('I' << 24) | ('Y' << 16) | ('U' << 8) | ('V')),
            /// <summary>
            /// Y,V,U (4:2:0)
            /// </summary>
            Uncompressed_YV12 = (('Y' << 24) | ('V' << 16) | ('1' << 8) | ('2')),
            /// <summary>
            /// Y,UV  (4:2:0)
            /// </summary>
            Uncompressed_NV12 = (('N' << 24) | ('V' << 16) | ('1' << 8) | ('2')),
            /// <summary>
            /// YUYV/YUY2 (4:2:2)
            /// </summary>
            Uncompressed_YUYV = (('Y' << 24) | ('U' << 16) | ('Y' << 8) | ('V')),
            /// <summary>
            /// UYVY (4:2:2)
            /// </summary>
            Uncompressed_UYVY = (('U' << 24) | ('Y' << 16) | ('V' << 8) | ('Y'))
        }
}

