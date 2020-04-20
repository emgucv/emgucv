//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

#if __UNIFIED__
using CoreGraphics;
#elif NETSTANDARD || UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE || UNITY_METRO || UNITY_EDITOR
#else
using System.Drawing.Imaging;
#endif

using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Emgu.CV.Features2D;
using Emgu.CV.Reflection;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// An Image is a wrapper to IplImage of OpenCV. 
    /// </summary>
    /// <typeparam name="TColor">Color type of this image (either Gray, Bgr, Bgra, Hsv, Hls, Lab, Luv, Xyz, Ycc, Rgb or Rbga)</typeparam>
    /// <typeparam name="TDepth">Depth of this image (either Byte, SByte, Single, double, UInt16, Int16 or Int32)</typeparam>
    [Serializable]
    public partial class Image<TColor, TDepth>
       : CvArray<TDepth>, IEquatable<Image<TColor, TDepth>>, IInputOutputArray
        where TColor : struct, IColor
        where TDepth : new()
    {
        private ImageDataReleaseMode _imageDataReleaseMode;

        private TDepth[,,] _array;

        /// <summary>
        /// The dimension of color
        /// </summary>
        private static readonly int _numberOfChannels = new TColor().Dimension;

        #region constructors
        /// <summary>
        /// Create an empty Image
        /// </summary>
        protected Image()
        {
        }

        /// <summary>
        /// Create image from the specific multi-dimensional data, where the 1st dimesion is # of rows (height), the 2nd dimension is # cols (width) and the 3rd dimension is the channel
        /// </summary>
        /// <param name="data">The multi-dimensional data where the 1st dimension is # of rows (height), the 2nd dimension is # cols (width) and the 3rd dimension is the channel </param>
        public Image(TDepth[,,] data)
        {
            Data = data;
        }

        /// <summary>
        /// Create an Image from unmanaged data. 
        /// </summary>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        /// <param name="stride">Size of aligned image row in bytes</param>
        /// <param name="scan0">Pointer to aligned image data, <b>where each row should be 4-align</b> </param>
        /// <remarks>The caller is responsible for allocating and freeing the block of memory specified by the scan0 parameter, however, the memory should not be released until the related Image is released. </remarks>
        public Image(int width, int height, int stride, IntPtr scan0)
        {
            MapDataToImage(width, height, stride, scan0);
        }

        /// <summary>
        /// Let this Image object use the specific Image data.
        /// </summary>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        /// <param name="stride">The data stride (bytes per row)</param>
        /// <param name="scan0">The origin of the data</param>
        protected void MapDataToImage(int width, int height, int stride, IntPtr scan0)
        {
            _ptr = CvInvoke.cvCreateImageHeader(new Size(width, height), CvDepth, NumberOfChannels);

            _imageDataReleaseMode = ImageDataReleaseMode.ReleaseHeaderOnly;
            GC.AddMemoryPressure(StructSize.MIplImage);

            MIplImage iplImage = MIplImage;
            iplImage.ImageData = scan0;
            iplImage.WidthStep = stride;
            Marshal.StructureToPtr(iplImage, _ptr, false);
        }

        /// <summary>
        /// Allocate the image from the image header. 
        /// </summary>
        /// <param name="ptr">This should be only a header to the image. When the image is disposed, the cvReleaseImageHeader will be called on the pointer.</param>
        internal Image(IntPtr ptr)
        {
            _ptr = ptr;
        }


        /// <summary>
        /// Read image from a file
        /// </summary>
        /// <param name="fileName">the name of the file that contains the image</param>
        public Image(String fileName)
        {
            try
            {
                using (Mat m = CvInvoke.Imread(fileName, CvEnum.ImreadModes.AnyColor | CvEnum.ImreadModes.AnyDepth))
                {
                    if (m.IsEmpty)
                        throw new NullReferenceException(String.Format("Unable to load image from file \"{0}\".", fileName));
                    LoadImageFromMat(m);
                }
            }
            catch (TypeInitializationException e)
            {
                //possibly Exception in CvInvoke's static constructor.
                throw e;
            }
            catch (Exception)
            {
                throw new ArgumentException(String.Format("Unable to decode file: {0}", fileName));
            }
        }

        /// <summary>
        /// Create a blank Image of the specified width, height and color.
        /// </summary>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        /// <param name="value">The initial color of the image</param>
        public Image(int width, int height, TColor value)
           : this(width, height)
        {
            //int n1 = MIplImage.nSize;
            SetValue(value);
            //int n2 = MIplImage.nSize;
            //int nDiff = n2 - n1;
        }

        /// <summary>
        /// Create a blank Image of the specified width and height. 
        /// </summary>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        public Image(int width, int height)
        {
            AllocateData(height, width, NumberOfChannels);
        }

        /// <summary>
        /// Create a blank Image of the specific size
        /// </summary>
        /// <param name="size">The size of the image</param>
        public Image(Size size)
           : this(size.Width, size.Height)
        {
        }

        /// <summary>
        /// Get or Set the data for this matrix. The Get function has O(1) complexity. The Set function make a copy of the data
        /// </summary>
        /// <remarks>
        /// If the image contains Byte and width is not a multiple of 4. The second dimension of the array might be larger than the Width of this image.  
        /// This is necessary since the length of a row need to be 4 align for OpenCV optimization. 
        /// The Set function always make a copy of the specific value. If the image contains Byte and width is not a multiple of 4. The second dimension of the array created might be larger than the Width of this image.  
        /// </remarks>
        public TDepth[,,] Data
        {
            get
            {
                return _array;
            }
            set
            {
                Debug.Assert(value != null, "Data cannot be set to null");
                Debug.Assert(value.GetLength(2) == NumberOfChannels, "The number of channels must equal");
                AllocateData(value.GetLength(0), value.GetLength(1), NumberOfChannels);
                int rows = value.GetLength(0);
                int valueRowLength = value.GetLength(1) * value.GetLength(2);
                int arrayRowLength = _array.GetLength(1) * _array.GetLength(2);
                for (int i = 0; i < rows; i++)
                    Array.Copy(value, i * valueRowLength, _array, i * arrayRowLength, valueRowLength);
            }
        }

        /// <summary>
        /// Re-allocate data for the array
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of columns</param>
        /// <param name="numberOfChannels">The number of channels of this image</param>
        protected override void AllocateData(int rows, int cols, int numberOfChannels)
        {
            DisposeObject();
            Debug.Assert(!_dataHandle.IsAllocated, "Handle should be free");

            _ptr = CvInvoke.cvCreateImageHeader(new Size(cols, rows), CvDepth, numberOfChannels);
            _imageDataReleaseMode = ImageDataReleaseMode.ReleaseHeaderOnly;

            GC.AddMemoryPressure(StructSize.MIplImage);

            Debug.Assert(MIplImage.Align == 4, "Only 4 align is supported at this moment");

            if (typeof(TDepth) == typeof(Byte) && (cols & 3) != 0 && (numberOfChannels & 3) != 0)
            {   //if the managed data isn't 4 aligned, make it so
                _array = new TDepth[rows, (cols & (~3)) + 4, numberOfChannels];
            }
            else
            {
                _array = new TDepth[rows, cols, numberOfChannels];
            }

            _dataHandle = GCHandle.Alloc(_array, GCHandleType.Pinned);
            //int n1 = MIplImage.nSize;
            CvInvoke.cvSetData(_ptr, _dataHandle.AddrOfPinnedObject(), _array.GetLength(1) * _array.GetLength(2) * SizeOfElement);
            //int n2 = MIplImage.nSize;
            //int nDiff = n2 - n1;
        }

        /// <summary>
        /// Create a multi-channel image from multiple gray scale images
        /// </summary>
        /// <param name="channels">The image channels to be merged into a single image</param>
        public Image(Image<Gray, TDepth>[] channels)
        {
            Debug.Assert(NumberOfChannels == channels.Length);
            AllocateData(channels[0].Height, channels[0].Width, NumberOfChannels);

            if (NumberOfChannels == 1)
            {
                //if this image only have a single channel
                CvInvoke.cvCopy(channels[0].Ptr, Ptr, IntPtr.Zero);
            }
            else
            {
                using (VectorOfMat mv = new VectorOfMat())
                {
                    for (int i = 0; i < channels.Length; i++)
                    {
                        mv.Push(channels[i].Mat);
                    }
                    CvInvoke.Merge(mv, this);
                }
            }
        }
#endregion

#region Implement ISerializable interface
        /// <summary>
        /// Constructor used to deserialize runtime serialized object
        /// </summary>
        /// <param name="info">The serialization info</param>
        /// <param name="context">The streaming context</param>
        public Image(SerializationInfo info, StreamingContext context)
        {
            DeserializeObjectData(info, context);
            ROI = (Rectangle)info.GetValue("Roi", typeof(Rectangle));
        }

        /// <summary>
        /// A function used for runtime serialization of the object
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (IsROISet)
            {
                Rectangle roi = ROI;
                ROI = Rectangle.Empty;
                base.GetObjectData(info, context);
                ROI = roi;
                info.AddValue("Roi", roi);
            }
            else
            {
                base.GetObjectData(info, context);
                info.AddValue("Roi", ROI);
            }
        }
#endregion

#region Image Properties

        /// <summary>
        /// The IplImage structure
        /// </summary>
        public MIplImage MIplImage
        {
            get
            {
                return (MIplImage)Marshal.PtrToStructure(Ptr, typeof(MIplImage));
            }
        }

        /// <summary> 
        /// Get or Set the region of interest for this image. To clear the ROI, set it to System.Drawing.Rectangle.Empty
        /// </summary>
        public Rectangle ROI
        {
            set
            {
                if (value.Equals(Rectangle.Empty))
                {
                    //reset the image ROI
                    CvInvoke.cvResetImageROI(Ptr);

                }
                else
                {   //set the image ROI to the specific value
                    CvInvoke.cvSetImageROI(Ptr, value);
                }

                if (_cvMat != null)
                {
                    _cvMat.Dispose();
                    _cvMat = null;
                }

                _cvMat = CvInvoke.CvArrToMat(Ptr);

            }
            get
            {
                //return the image ROI
                return CvInvoke.cvGetImageROI(Ptr);
            }
        }

        /// <summary>
        /// Get the number of channels for this image
        /// </summary>
        public override int NumberOfChannels
        {
            get
            {
                return _numberOfChannels;
            }
        }

        /// <summary>
        /// Get the underneath managed array
        /// </summary>
        public override Array ManagedArray
        {
            get { return _array; }
            set
            {
                TDepth[,,] data = value as TDepth[,,];
                if (data == null)
                    throw new InvalidCastException(String.Format("Cannot convert ManagedArray to type of {0}[,,].", typeof(TDepth).ToString()));
                Data = data;
            }
        }

        /// <summary>
        /// Get the equivalent opencv depth type for this image
        /// </summary>
        public static CvEnum.IplDepth CvDepth
        {
            get
            {
                Type typeOfDepth = typeof(TDepth);

                if (typeOfDepth == typeof(Single))
                    return CvEnum.IplDepth.IplDepth32F;
                else if (typeOfDepth == typeof(Byte))
                    return CvEnum.IplDepth.IplDepth_8U;
                else if (typeOfDepth == typeof(Double))
                    return CvEnum.IplDepth.IplDepth64F;
                else if (typeOfDepth == typeof(SByte))
                    return Emgu.CV.CvEnum.IplDepth.IplDepth_8S;
                else if (typeOfDepth == typeof(UInt16))
                    return Emgu.CV.CvEnum.IplDepth.IplDepth16U;
                else if (typeOfDepth == typeof(Int16))
                    return Emgu.CV.CvEnum.IplDepth.IplDepth16S;
                else if (typeOfDepth == typeof(Int32))
                    return Emgu.CV.CvEnum.IplDepth.IplDepth32S;
                else
                    throw new NotImplementedException("Unsupported image depth");
            }
        }

        /// <summary> 
        /// Indicates if the region of interest has been set
        /// </summary> 
        public bool IsROISet
        {
            get
            {
                return Marshal.ReadIntPtr(Ptr, ImageConstants.RoiOffset) != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Get the average value on this image
        /// </summary>
        /// <returns>The average color of the image</returns>
        public TColor GetAverage()
        {
            return GetAverage(null);
        }

        /// <summary>
        /// Get the average value on this image, using the specific mask
        /// </summary>
        /// <param name="mask">The mask for find the average value</param>
        /// <returns>The average color of the masked area</returns>
        public TColor GetAverage(Image<Gray, Byte> mask)
        {
            TColor res = new TColor();
            res.MCvScalar = CvInvoke.Mean(this, mask);
            return res;
        }

        /// <summary>Get the sum for each color channel </summary>
        /// <returns>The sum for each color channel</returns>
        public TColor GetSum()
        {
            TColor res = new TColor();
            res.MCvScalar = CvInvoke.Sum(this);
            return res;
        }
#endregion

#region Coping and Filling
        /// <summary>
        /// Set every pixel of the image to the specific color 
        /// </summary>
        /// <param name="color">The color to be set</param>
        public void SetValue(TColor color)
        {
            SetValue(color.MCvScalar);
        }

        /// <summary>
        /// Set every pixel of the image to the specific color, using a mask
        /// </summary>
        /// <param name="color">The color to be set</param>
        /// <param name="mask">The mask for setting color</param>
        public void SetValue(TColor color, Image<Gray, Byte> mask)
        {
            SetValue(color.MCvScalar, mask);
        }

        /// <summary>
        /// Copy the masked area of this image to destination
        /// </summary>
        /// <param name="dest">the destination to copy to</param>
        /// <param name="mask">the mask for copy</param>
        public void Copy(Image<TColor, TDepth> dest, Image<Gray, Byte> mask)
        {
            CvInvoke.cvCopy(Ptr, dest.Ptr, mask == null ? IntPtr.Zero : mask.Ptr);
        }

        /// <summary> 
        /// Make a copy of the image using a mask, if ROI is set, only copy the ROI 
        /// </summary> 
        /// <param name="mask">the mask for coping</param>
        /// <returns> A copy of the image</returns>
        public Image<TColor, TDepth> Copy(Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
            Copy(res, mask);
            return res;
        }

        /// <summary>
        /// Make a copy of the specific ROI (Region of Interest) from the image
        /// </summary>
        /// <param name="roi">The roi to be copied</param>
        /// <returns>The region of interest</returns>
        public Image<TColor, TDepth> Copy(Rectangle roi)
        {
            /*
            Rectangle currentRoi = ROI; //cache the current roi
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(roi.Size);
            ROI = roi;
            CvInvoke.cvCopy(Ptr, res.Ptr, IntPtr.Zero);
            ROI = currentRoi; //reset the roi
            return res;*/
            using (Image<TColor, TDepth> subrect = GetSubRect(roi))
            {
                return subrect.Copy();
            }
        }

        /// <summary>
        /// Get a copy of the boxed region of the image
        /// </summary>
        /// <param name="box">The boxed region of the image</param>
        /// <returns>A copy of the boxed region of the image</returns>
        public Image<TColor, TDepth> Copy(RotatedRect box)
        {
            PointF[] srcCorners = box.GetVertices();

            PointF[] destCorners = new PointF[] {
            new PointF(0, box.Size.Height - 1),
            new PointF(0, 0),
            new PointF(box.Size.Width - 1, 0),
            new PointF(box.Size.Width - 1, box.Size.Height - 1)};

            using (Mat rot = CvInvoke.GetAffineTransform(srcCorners, destCorners))
            {
                Image<TColor, TDepth> res = new Image<TColor, TDepth>((int)box.Size.Width, (int)box.Size.Height);
                CvInvoke.WarpAffine(this, res, rot, res.Size);
                return res;
            }
        }

        /// <summary> Make a copy of the image, if ROI is set, only copy the ROI</summary>
        /// <returns> A copy of the image</returns>
        public Image<TColor, TDepth> Copy()
        {
            return Copy(null);
        }

        /// <summary> 
        /// Create an image of the same size
        /// </summary>
        /// <remarks>The initial pixel in the image equals zero</remarks>
        /// <returns> The image of the same size</returns>
        public Image<TColor, TDepth> CopyBlank()
        {
            return new Image<TColor, TDepth>(Size);
        }

        /// <summary>
        /// Make a clone of the current image. All image data as well as the COI and ROI are cloned
        /// </summary>
        /// <returns>A clone of the current image. All image data as well as the COI and ROI are cloned</returns>
        public Image<TColor, TDepth> Clone()
        {
            int coi = CvInvoke.cvGetImageCOI(Ptr); //get the COI for current image
            Rectangle roi = ROI; //get the ROI for current image

            CvInvoke.cvSetImageCOI(Ptr, 0); //clear COI for current image
            ROI = Rectangle.Empty; // clear ROI for current image

#region create a clone of the current image with the same COI and ROI
            Image<TColor, TDepth> res = Copy();
            CvInvoke.cvSetImageCOI(res.Ptr, coi);
            res.ROI = roi;
#endregion

            CvInvoke.cvSetImageCOI(Ptr, coi); //reset the COI for the current image
            ROI = roi; // reset the ROI for the current image

            return res;
        }

        /// <summary>
        /// Get a subimage which image data is shared with the current image.
        /// </summary>
        /// <param name="rect">The rectangle area of the sub-image</param>
        /// <returns>A subimage which image data is shared with the current image</returns>
        public Image<TColor, TDepth> GetSubRect(Rectangle rect)
        {
            Image<TColor, TDepth> subRect = new Image<TColor, TDepth>();
            subRect._array = _array;

            GC.AddMemoryPressure(StructSize.MIplImage); //This pressure will be released once the result image is disposed. 

            subRect._ptr = CvInvoke.cvGetImageSubRect(_ptr, ref rect);
            return subRect;
        }

#endregion


#region Drawing functions
        /// <summary>Draw an Rectangle of the specific color and thickness </summary>
        /// <param name="rect">The rectangle to be drawn</param>
        /// <param name="color">The color of the rectangle </param>
        /// <param name="thickness">If thickness is less than 1, the rectangle is filled up </param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public virtual void Draw(Rectangle rect, TColor color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            CvInvoke.Rectangle(this, rect, color.MCvScalar, thickness, lineType, shift);
        }

        /// <summary>Draw a 2D Cross using the specific color and thickness </summary>
        /// <param name="cross">The 2D Cross to be drawn</param>
        /// <param name="color">The color of the cross </param>
        /// <param name="thickness">Must be &gt; 0 </param>
        public void Draw(Cross2DF cross, TColor color, int thickness)
        {
            Debug.Assert(thickness > 0, "Thickness should be > 0");

            if (thickness > 0)
            {
                Draw(cross.Horizontal, color, thickness);
                Draw(cross.Vertical, color, thickness);
            }
        }
        /// <summary>Draw a line segment using the specific color and thickness </summary>
        /// <param name="line">The line segment to be drawn</param>
        /// <param name="color">The color of the line segment </param>
        /// <param name="thickness">The thickness of the line segment </param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public virtual void Draw(LineSegment2DF line, TColor color, int thickness, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            Debug.Assert(thickness > 0, "Thickness should be > 0");
            if (thickness > 0)
                CvInvoke.Line(
                    this,
                    Point.Round(line.P1),
                    Point.Round(line.P2),
                    color.MCvScalar,
                    thickness,
                    lineType,
                    shift);
        }

        /// <summary> Draw a line segment using the specific color and thickness </summary>
        /// <param name="line"> The line segment to be drawn</param>
        /// <param name="color"> The color of the line segment </param>
        /// <param name="thickness"> The thickness of the line segment </param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public virtual void Draw(LineSegment2D line, TColor color, int thickness, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            Debug.Assert(thickness > 0, "Thickness should be > 0");
            if (thickness > 0)
                CvInvoke.Line(
                    this,
                    line.P1,
                    line.P2,
                    color.MCvScalar,
                    thickness,
                    lineType,
                    shift);
        }

        /// <summary> Draw a convex polygon using the specific color and thickness </summary>
        /// <param name="polygon"> The convex polygon to be drawn</param>
        /// <param name="color"> The color of the triangle </param>
        /// <param name="thickness"> If thickness is less than 1, the triangle is filled up </param>
        public virtual void Draw(IConvexPolygonF polygon, TColor color, int thickness)
        {
            PointF[] polygonVertices = polygon.GetVertices();

            Point[] vertices = new Point[polygonVertices.Length];
            for (int i = 0; i < polygonVertices.Length; i++)
                vertices[i] = Point.Round(polygonVertices[i]);

            if (thickness > 0)
                DrawPolyline(vertices, true, color, thickness);
            else
            {
                FillConvexPoly(vertices, color);
            }
        }

        /// <summary>
        /// Fill the convex polygon with the specific color
        /// </summary>
        /// <param name="pts">The array of points that define the convex polygon</param>
        /// <param name="color">The color to fill the polygon with</param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public void FillConvexPoly(Point[] pts, TColor color, Emgu.CV.CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            using (VectorOfPoint vp = new VectorOfPoint(pts))
                CvInvoke.FillConvexPoly(this, vp, color.MCvScalar, lineType, shift);
        }

        /// <summary>
        /// Draw the polyline defined by the array of 2D points
        /// </summary>
        /// <param name="pts">A polyline defined by its point</param>
        /// <param name="isClosed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
        /// <param name="color">the color used for drawing</param>
        /// <param name="thickness">the thinkness of the line</param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public void DrawPolyline(Point[] pts, bool isClosed, TColor color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            DrawPolyline(new Point[][] { pts }, isClosed, color, thickness, lineType, shift);
        }

        /// <summary>
        /// Draw the polylines defined by the array of array of 2D points
        /// </summary>
        /// <param name="pts">An array of polylines each represented by an array of points</param>
        /// <param name="isClosed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
        /// <param name="color">the color used for drawing</param>
        /// <param name="thickness">the thickness of the line</param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public void DrawPolyline(Point[][] pts, bool isClosed, TColor color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            if (thickness > 0)
            {
                using (VectorOfVectorOfPoint vvp = new VectorOfVectorOfPoint(pts))
                {
                    CvInvoke.Polylines(this, vvp, isClosed, color.MCvScalar, thickness, lineType, shift);
                }
            }
        }

        /// <summary> Draw a Circle of the specific color and thickness </summary>
        /// <param name="circle"> The circle to be drawn</param>
        /// <param name="color"> The color of the circle </param>
        /// <param name="thickness"> If thickness is less than 1, the circle is filled up </param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public virtual void Draw(CircleF circle, TColor color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            CvInvoke.Circle(
             this,
             Point.Round(circle.Center),
             (int)circle.Radius,
             color.MCvScalar,
             (thickness <= 0) ? -1 : thickness,
             lineType,
             shift);
        }

        /// <summary> Draw a Ellipse of the specific color and thickness </summary>
        /// <param name="ellipse"> The ellipse to be draw</param>
        /// <param name="color"> The color of the ellipse </param>
        /// <param name="thickness"> If thickness is less than 1, the ellipse is filled up </param>
        /// <param name="lineType">Line type</param>
        /// <param name="shift">Number of fractional bits in the center coordinates and radius value</param>
        public void Draw(Ellipse ellipse, TColor color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, int shift = 0)
        {
            CvInvoke.Ellipse(this, ellipse.RotatedRect, color.MCvScalar, thickness, lineType, shift);
        }

        /// <summary>
        /// Draw the text using the specific font on the image
        /// </summary>
        /// <param name="message">The text message to be draw</param>
        /// <param name="fontFace">Font type.</param>
        /// <param name="fontScale">Font scale factor that is multiplied by the font-specific base size.</param>
        /// <param name="bottomLeft">The location of the bottom left corner of the font</param>
        /// <param name="color">The color of the text</param>
        /// <param name="thickness">Thickness of the lines used to draw a text.</param>
        /// <param name="lineType">Line type</param>
        /// <param name="bottomLeftOrigin">When true, the image data origin is at the bottom-left corner. Otherwise, it is at the top-left corner.</param>
        public virtual void Draw(String message, Point bottomLeft, CvEnum.FontFace fontFace, double fontScale, TColor color, int thickness = 1, CvEnum.LineType lineType = CvEnum.LineType.EightConnected, bool bottomLeftOrigin = false)
        {
            CvInvoke.PutText(this,
             message,
             bottomLeft,
             fontFace,
             fontScale,
             color.MCvScalar,
             thickness,
             lineType,
             bottomLeftOrigin);
        }

        /// <summary>
        /// Draws contour outlines in the image if thickness&gt;=0 or fills area bounded by the contours if thickness&lt;0
        /// </summary>
        /// <param name="contours">All the input contours. Each contour is stored as a point vector.</param>
        /// <param name="contourIdx">Parameter indicating a contour to draw. If it is negative, all the contours are drawn.</param>
        /// <param name="color">Color of the contours </param>
        /// <param name="maxLevel">Maximal level for drawn contours. If 0, only contour is drawn. If 1, the contour and all contours after it on the same level are drawn. If 2, all contours after and all contours one level below the contours are drawn, etc. If the value is negative, the function does not draw the contours following after contour but draws child contours of contour up to abs(maxLevel)-1 level. </param>
        /// <param name="thickness">Thickness of lines the contours are drawn with. If it is negative the contour interiors are drawn</param>
        /// <param name="lineType">Type of the contour segments</param>
        /// <param name="hierarchy">Optional information about hierarchy. It is only needed if you want to draw only some of the contours</param>
        /// <param name="offset">Shift all the point coordinates by the specified value. It is useful in case if the contours retrieved in some image ROI and then the ROI offset needs to be taken into account during the rendering. </param>
        public void Draw(
            IInputArrayOfArrays contours,
            int contourIdx,
            TColor color,
            int thickness = 1,
            CvEnum.LineType lineType = CvEnum.LineType.EightConnected,
            IInputArray hierarchy = null,
            int maxLevel = int.MaxValue,
            Point offset = new Point())
        {
            CvInvoke.DrawContours(
          this,
          contours,
          contourIdx,
          color.MCvScalar,
          thickness,
          lineType,
          hierarchy,
          maxLevel,
          offset);
        }

        /// <summary>
        /// Draws contour outlines in the image if thickness&gt;=0 or fills area bounded by the contours if thickness&lt;0
        /// </summary>
        /// <param name="contours">The input contour stored as a point vector.</param>
        /// <param name="color">Color of the contours </param>
        /// <param name="thickness">Thickness of lines the contours are drawn with. If it is negative the contour interiors are drawn</param>
        /// <param name="lineType">Type of the contour segments</param>
        /// <param name="offset">Shift all the point coordinates by the specified value. It is useful in case if the contours retrieved in some image ROI and then the ROI offset needs to be taken into account during the rendering. </param>
        public void Draw(
           Point[] contours,
           TColor color,
           int thickness = 1,
           CvEnum.LineType lineType = CvEnum.LineType.EightConnected,
           Point offset = new Point())
        {
            using (VectorOfPoint vp = new VectorOfPoint(contours))
            using (VectorOfVectorOfPoint vvp = new VectorOfVectorOfPoint())
            {
                vvp.Push(vp);
                Draw(vvp, 0, color, thickness, lineType, null, int.MaxValue);
            }
        }

#endregion

#region Hough line and circles
        /// <summary>
        /// Apply Probabilistic Hough transform to find line segments.
        /// The current image must be a binary image (eg. the edges as a result of the Canny edge detector)
        /// </summary>
        /// <param name="rhoResolution">Distance resolution in pixel-related units.</param>
        /// <param name="thetaResolution">Angle resolution measured in radians</param>
        /// <param name="threshold">A line is returned by the function if the corresponding accumulator value is greater than threshold</param>
        /// <param name="minLineWidth">Minimum width of a line</param>
        /// <param name="gapBetweenLines">Minimum gap between lines</param>
        /// <returns>The line segments detected for each of the channels</returns>
        public LineSegment2D[][] HoughLinesBinary(double rhoResolution, double thetaResolution, int threshold, double minLineWidth, double gapBetweenLines)
        {
            return this.ForEachDuplicateChannel<LineSegment2D[]>(
            delegate (IInputArray img, int channel)
            {
                return CvInvoke.HoughLinesP(this, rhoResolution, thetaResolution, threshold, minLineWidth, gapBetweenLines);
            });
        }

        /// <summary>
        /// Apply Canny Edge Detector follows by Probabilistic Hough transform to find line segments in the image
        /// </summary>
        /// <param name="cannyThreshold">The threshold to find initial segments of strong edges</param>
        /// <param name="cannyThresholdLinking">The threshold used for edge Linking</param>
        /// <param name="rhoResolution">Distance resolution in pixel-related units.</param>
        /// <param name="thetaResolution">Angle resolution measured in radians</param>
        /// <param name="threshold">A line is returned by the function if the corresponding accumulator value is greater than threshold</param>
        /// <param name="minLineWidth">Minimum width of a line</param>
        /// <param name="gapBetweenLines">Minimum gap between lines</param>
        /// <returns>The line segments detected for each of the channels</returns>
        public LineSegment2D[][] HoughLines(double cannyThreshold, double cannyThresholdLinking, double rhoResolution, double thetaResolution, int threshold, double minLineWidth, double gapBetweenLines)
        {
            using (Image<Gray, Byte> canny = Canny(cannyThreshold, cannyThresholdLinking))
            {
                return canny.HoughLinesBinary(
                   rhoResolution,
                   thetaResolution,
                   threshold,
                   minLineWidth,
                   gapBetweenLines);
            }
        }

        /// <summary>
        /// First apply Canny Edge Detector on the current image,
        /// then apply Hough transform to find circles
        /// </summary>
        /// <param name="cannyThreshold">The higher threshold of the two passed to Canny edge detector (the lower one will be twice smaller).</param>
        /// <param name="accumulatorThreshold">Accumulator threshold at the center detection stage. The smaller it is, the more false circles may be detected. Circles, corresponding to the larger accumulator values, will be returned first</param>
        /// <param name="dp">Resolution of the accumulator used to detect centers of the circles. For example, if it is 1, the accumulator will have the same resolution as the input image, if it is 2 - accumulator will have twice smaller width and height, etc</param>
        /// <param name="minRadius">Minimal radius of the circles to search for</param>
        /// <param name="maxRadius">Maximal radius of the circles to search for</param>
        /// <param name="minDist">Minimum distance between centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed</param>
        /// <returns>The circle detected for each of the channels</returns>
        public CircleF[][] HoughCircles(TColor cannyThreshold, TColor accumulatorThreshold, double dp, double minDist, int minRadius = 0, int maxRadius = 0)
        {
            double[] cannyThresh = cannyThreshold.MCvScalar.ToArray();
            double[] accumulatorThresh = accumulatorThreshold.MCvScalar.ToArray();
            return this.ForEachDuplicateChannel(
               delegate (IInputArray img, int channel)
               {
                   return CvInvoke.HoughCircles(img, CvEnum.HoughModes.Gradient, dp, minDist, cannyThresh[channel], accumulatorThresh[channel], minRadius, maxRadius);
               });
        }
#endregion

#region Indexer
        /// <summary>
        /// Get or Set the specific channel of the current image.
        /// For Get operation, a copy of the specific channel is returned.
        /// For Set operation, the specific channel is copied to this image.
        /// </summary>
        /// <param name="channel">The channel to get from the current image, zero based index</param>
        /// <returns>The specific channel of the current image</returns>
        public Image<Gray, TDepth> this[int channel]
        {
            get
            {
                Image<Gray, TDepth> imageChannel = new Image<Gray, TDepth>(Size);
                CvInvoke.MixChannels(this, imageChannel, new int[] { channel, 0 });
                return imageChannel;
            }
            set
            {
                CvInvoke.MixChannels(value, this, new int[] { 0, channel });
            }
        }

        /// <summary>
        /// Get or Set the color in the <paramref name="row"/>th row (y direction) and <paramref name="column"/>th column (x direction)
        /// </summary>
        /// <param name="row">The zero-based row (y direction) of the pixel </param>
        /// <param name="column">The zero-based column (x direction) of the pixel</param>
        /// <returns>The color in the specific <paramref name="row"/> and <paramref name="column"/></returns>
        public TColor this[int row, int column]
        {
            get
            {
                TColor res = new TColor();
                res.MCvScalar = CvInvoke.cvGet2D(Ptr, row, column);
                return res;
            }
            set
            {
                CvInvoke.cvSet2D(Ptr, row, column, value.MCvScalar);
            }
        }

        /// <summary>
        /// Get or Set the color in the <paramref name="location"/>
        /// </summary>
        /// <param name="location">the location of the pixel </param>
        /// <returns>the color in the <paramref name="location"/></returns>
        public TColor this[Point location]
        {
            get
            {
                return this[location.Y, location.X];
            }
            set
            {
                this[location.Y, location.X] = value;
            }
        }
#endregion

#region utilities
        /// <summary>
        /// Return parameters based on ROI
        /// </summary>
        /// <param name="ptr">The Pointer to the IplImage</param>
        /// <param name="start">The address of the pointer that point to the start of the Bytes taken into consideration ROI</param>
        /// <param name="elementCount">ROI.Width * ColorType.Dimension</param>
        /// <param name="byteWidth">The number of bytes in a row taken into consideration ROI</param>
        /// <param name="rows">The number of rows taken into consideration ROI</param>
        /// <param name="widthStep">The width step required to jump to the next row</param>
        protected static void RoiParam(IntPtr ptr, out Int64 start, out int rows, out int elementCount, out int byteWidth, out int widthStep)
        {
            MIplImage ipl = (MIplImage)Marshal.PtrToStructure(ptr, typeof(MIplImage));
            start = ipl.ImageData.ToInt64();
            widthStep = ipl.WidthStep;

            if (ipl.Roi != IntPtr.Zero)
            {
                Rectangle rec = CvInvoke.cvGetImageROI(ptr);
                elementCount = rec.Width * ipl.NChannels;
                byteWidth = ((int)ipl.Depth >> 3) * elementCount;

                start += rec.Y * widthStep
                    + ((int)ipl.Depth >> 3) * rec.X;
                rows = rec.Height;
            }
            else
            {
                byteWidth = widthStep;
                elementCount = ipl.Width * ipl.NChannels;
                rows = ipl.Height;
            }
        }

        /// <summary>
        /// Apply convertor and compute result for each channel of the image.
        /// </summary>
        /// <remarks>
        /// For single channel image, apply converter directly.
        /// For multiple channel image, set the COI for the specific channel before appling the convertor
        /// </remarks>
        /// <typeparam name="TResult">The return type</typeparam>
        /// <param name="conv">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
        /// <returns>An array which contains result for each channel</returns>
        private TResult[] ForEachChannel<TResult>(Func<IntPtr, int, TResult> conv)
        {
            TResult[] res = new TResult[NumberOfChannels];
            if (NumberOfChannels == 1)
                res[0] = conv(Ptr, 0);
            else
            {
                for (int i = 0; i < NumberOfChannels; i++)
                {
                    CvInvoke.cvSetImageCOI(Ptr, i + 1);
                    res[i] = conv(Ptr, i);
                }
                CvInvoke.cvSetImageCOI(Ptr, 0);
            }
            return res;
        }

        /// <summary>
        /// If the image has only one channel, apply the action directly on the IntPtr of this image and <paramref name="dest"/>,
        /// otherwise, make copy each channel of this image to a temperary one, apply action on it and another temperory image and copy the resulting image back to image2
        /// </summary>
        /// <typeparam name="TOtherDepth">The type of the depth of the <paramref name="dest"/> image</typeparam>
        /// <param name="act">The function which acepts the src IntPtr, dest IntPtr and index of the channel as input</param>
        /// <param name="dest">The destination image</param>
        private void ForEachDuplicateChannel<TOtherDepth>(Action<IInputArray, IOutputArray, int> act, Image<TColor, TOtherDepth> dest)
            where TOtherDepth : new()
        {
            if (NumberOfChannels == 1)
                act(this, dest, 0);
            else
            {
                using (Mat tmp1 = new Mat())
                using (Mat tmp2 = new Mat())
                {
                    for (int i = 0; i < NumberOfChannels; i++)
                    {
                        CvInvoke.ExtractChannel(this, tmp1, i);
                        act(tmp1, tmp2, i);
                        CvInvoke.InsertChannel(tmp2, dest, i);
                    }
                }
            }
        }
#endregion

#region Gradient, Edges and Features
        /// <summary>
        /// Calculates the image derivative by convolving the image with the appropriate kernel
        /// The Sobel operators combine Gaussian smoothing and differentiation so the result is more or less robust to the noise. Most often, the function is called with (xorder=1, yorder=0, aperture_size=3) or (xorder=0, yorder=1, aperture_size=3) to calculate first x- or y- image derivative.
        /// </summary>
        /// <param name="xorder">Order of the derivative x</param>
        /// <param name="yorder">Order of the derivative y</param>
        /// <param name="apertureSize">Size of the extended Sobel kernel, must be 1, 3, 5 or 7. In all cases except 1, aperture_size xaperture_size separable kernel will be used to calculate the derivative.</param>
        /// <returns>The result of the sobel edge detector</returns>
        [ExposableMethod(Exposable = true, Category = "Gradients, Edges")]
        public Image<TColor, Single> Sobel(int xorder, int yorder, int apertureSize)
        {
            Image<TColor, Single> res = new Image<TColor, float>(Size);
            CvInvoke.Sobel(this, res, CvInvoke.GetDepthType(typeof(Single)), xorder, yorder, apertureSize, 1.0, 0.0, CvEnum.BorderType.Default);
            return res;
        }

        /// <summary>
        /// Calculates Laplacian of the source image by summing second x- and y- derivatives calculated using Sobel operator.
        /// Specifying aperture_size=1 gives the fastest variant that is equal to convolving the image with the following kernel:
        ///
        /// |0  1  0|
        /// |1 -4  1|
        /// |0  1  0|
        /// </summary>
        /// <param name="apertureSize">Aperture size </param>
        /// <returns>The Laplacian of the image</returns>
        [ExposableMethod(Exposable = true, Category = "Gradients, Edges")]
        public Image<TColor, Single> Laplace(int apertureSize)
        {
            Image<TColor, Single> res = new Image<TColor, float>(Size);
            CvInvoke.Laplacian(this, res, CvInvoke.GetDepthType(typeof(Single)), apertureSize, 1.0, 0.0, CvEnum.BorderType.Default);
            return res;
        }

        /// <summary> Find the edges on this image and marked them in the returned image.</summary>
        /// <param name="thresh"> The threshhold to find initial segments of strong edges</param>
        /// <param name="threshLinking"> The threshold used for edge Linking</param>
        /// <returns> The edges found by the Canny edge detector</returns>
        [ExposableMethod(Exposable = true, Category = "Gradients, Edges")]
        public Image<Gray, Byte> Canny(double thresh, double threshLinking)
        {
            return Canny(thresh, threshLinking, 3, false);
        }

        /// <summary> Find the edges on this image and marked them in the returned image.</summary>
        /// <param name="thresh"> The threshhold to find initial segments of strong edges</param>
        /// <param name="threshLinking"> The threshold used for edge Linking</param>
        /// <param name="apertureSize">The aperture size, use 3 for default</param>
        /// <param name="l2Gradient">a flag, indicating whether a more accurate norm should be used to calculate the image gradient magnitude ( L2gradient=true ), or whether the default norm is enough ( L2gradient=false ).</param>
        /// <returns> The edges found by the Canny edge detector</returns>
        public Image<Gray, Byte> Canny(double thresh, double threshLinking, int apertureSize, bool l2Gradient)
        {
            Image<Gray, Byte> res = new Image<Gray, Byte>(Size);
            CvInvoke.Canny(this, res, thresh, threshLinking, apertureSize, l2Gradient);
            return res;
        }

        /// <summary>
        /// Iterates to find the sub-pixel accurate location of corners, or radial saddle points
        /// </summary>
        /// <param name="corners">Coordinates of the input corners, the values will be modified by this function call</param>
        /// <param name="win">Half sizes of the search window. For example, if win=(5,5) then 5*2+1 x 5*2+1 = 11 x 11 search window is used</param>
        /// <param name="zeroZone">Half size of the dead region in the middle of the search zone over which the summation in formulae below is not done. It is used sometimes to avoid possible singularities of the autocorrelation matrix. The value of (-1,-1) indicates that there is no such size</param>
        /// <param name="criteria">Criteria for termination of the iterative process of corner refinement. That is, the process of corner position refinement stops either after certain number of iteration or when a required accuracy is achieved. The criteria may specify either of or both the maximum number of iteration and the required accuracy</param>
        /// <returns>Refined corner coordinates</returns>
        public void FindCornerSubPix(
           PointF[][] corners,
           Size win,
           Size zeroZone,
           MCvTermCriteria criteria)
        {
            this.ForEachDuplicateChannel(delegate (IInputArray img, int channel)
                {
                    using (VectorOfPointF vec = new VectorOfPointF())
                    {
                        vec.Push(corners[channel]);
                        CvInvoke.CornerSubPix(
                        img,
                        vec,
                        win,
                        zeroZone,
                        criteria);
                        Array.Copy(vec.ToArray(), corners[channel], corners[channel].Length);
                    }
                });
        }

#endregion

#region Matching
        /// <summary>
        /// The function slides through image, compares overlapped patches of size wxh with templ using the specified method and return the comparison results 
        /// </summary>
        /// <param name="template">Searched template; must be not greater than the source image and the same data type as the image</param>
        /// <param name="method">Specifies the way the template must be compared with image regions </param>
        /// <returns>The comparison result: width = this.Width - template.Width + 1; height = this.Height - template.Height + 1 </returns>
        public Image<Gray, Single> MatchTemplate(Image<TColor, TDepth> template, CvEnum.TemplateMatchingType method)
        {
            Image<Gray, Single> res = new Image<Gray, Single>(Width - template.Width + 1, Height - template.Height + 1);
            CvInvoke.MatchTemplate(this, template, res, method);
            return res;
        }
#endregion

#region Logic
#region And Methods
        /// <summary> Perform an elementwise AND operation with another image and return the result</summary>
        /// <param name="img2">The second image for the AND operation</param>
        /// <returns> The result of the AND operation</returns>
        public Image<TColor, TDepth> And(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
            CvInvoke.BitwiseAnd(this, img2, res, null);
            return res;
        }

        /// <summary> 
        /// Perform an elementwise AND operation with another image, using a mask, and return the result
        /// </summary>
        /// <param name="img2">The second image for the AND operation</param>
        /// <param name="mask">The mask for the AND operation</param>
        /// <returns> The result of the AND operation</returns>
        public Image<TColor, TDepth> And(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
            CvInvoke.BitwiseAnd(this, img2, res, mask);
            return res;
        }

        /// <summary> Perform an binary AND operation with some color</summary>
        /// <param name="val">The color for the AND operation</param>
        /// <returns> The result of the AND operation</returns>
        public Image<TColor, TDepth> And(TColor val)
        {
            return And(val, null);
        }

        /// <summary> Perform an binary AND operation with some color using a mask</summary>
        /// <param name="val">The color for the AND operation</param>
        /// <param name="mask">The mask for the AND operation</param>
        /// <returns> The result of the AND operation</returns>
        public Image<TColor, TDepth> And(TColor val, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
            using (ScalarArray ia = new ScalarArray(val.MCvScalar))
            {
                CvInvoke.BitwiseAnd(this, ia, res, mask);
            }
            return res;
        }
#endregion

#region Or Methods
        /// <summary> Perform an elementwise OR operation with another image and return the result</summary>
        /// <param name="img2">The second image for the OR operation</param>
        /// <returns> The result of the OR operation</returns>
        public Image<TColor, TDepth> Or(Image<TColor, TDepth> img2)
        {
            return Or(img2, null);
        }
        /// <summary> Perform an elementwise OR operation with another image, using a mask, and return the result</summary>
        /// <param name="img2">The second image for the OR operation</param>
        /// <param name="mask">The mask for the OR operation</param>
        /// <returns> The result of the OR operation</returns>
        public Image<TColor, TDepth> Or(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.BitwiseOr(this, img2, res, mask);
            return res;
        }

        /// <summary> Perform an elementwise OR operation with some color</summary>
        /// <param name="val">The value for the OR operation</param>
        /// <returns> The result of the OR operation</returns>
        [ExposableMethod(Exposable = true, Category = "Logic")]
        public Image<TColor, TDepth> Or(TColor val)
        {
            return Or(val, null);
        }
        /// <summary> Perform an elementwise OR operation with some color using a mask</summary>
        /// <param name="val">The color for the OR operation</param>
        /// <param name="mask">The mask for the OR operation</param>
        /// <returns> The result of the OR operation</returns>
        public Image<TColor, TDepth> Or(TColor val, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(val.MCvScalar))
            {
                CvInvoke.BitwiseOr(this, ia, res, mask);
            }
            return res;
        }
#endregion

#region Xor Methods
        /// <summary> Perform an elementwise XOR operation with another image and return the result</summary>
        /// <param name="img2">The second image for the XOR operation</param>
        /// <returns> The result of the XOR operation</returns>
        public Image<TColor, TDepth> Xor(Image<TColor, TDepth> img2)
        {
            return Xor(img2, null);
        }

        /// <summary>
        /// Perform an elementwise XOR operation with another image, using a mask, and return the result
        /// </summary>
        /// <param name="img2">The second image for the XOR operation</param>
        /// <param name="mask">The mask for the XOR operation</param>
        /// <returns>The result of the XOR operation</returns>
        public Image<TColor, TDepth> Xor(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.BitwiseXor(this, img2, res, mask);
            return res;
        }

        /// <summary> 
        /// Perform an binary XOR operation with some color
        /// </summary>
        /// <param name="val">The value for the XOR operation</param>
        /// <returns> The result of the XOR operation</returns>
        [ExposableMethod(Exposable = true, Category = "Logic")]
        public Image<TColor, TDepth> Xor(TColor val)
        {
            return Xor(val, null);
        }

        /// <summary>
        /// Perform an binary XOR operation with some color using a mask
        /// </summary>
        /// <param name="val">The color for the XOR operation</param>
        /// <param name="mask">The mask for the XOR operation</param>
        /// <returns> The result of the XOR operation</returns>
        public Image<TColor, TDepth> Xor(TColor val, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(val.MCvScalar))
            {
                CvInvoke.BitwiseXor(this, ia, res, mask);
            }
            return res;
        }
#endregion

        /// <summary> 
        /// Compute the complement image
        /// </summary>
        /// <returns> The complement image</returns>
        public Image<TColor, TDepth> Not()
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.BitwiseNot(this, res, null);
            return res;
        }
#endregion

#region Comparison
        /// <summary> Find the elementwise maximum value </summary>
        /// <param name="img2">The second image for the Max operation</param>
        /// <returns> An image where each pixel is the maximum of <i>this</i> image and the parameter image</returns>
        public Image<TColor, TDepth> Max(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Max(this, img2, res);
            return res;
        }

        /// <summary> Find the elementwise maximum value </summary>
        /// <param name="value">The value to compare with</param>
        /// <returns> An image where each pixel is the maximum of <i>this</i> image and <paramref name="value"/></returns>
        public Image<TColor, TDepth> Max(double value)
        {
            Image<TColor, TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(value))
            {
                CvInvoke.Max(this, ia, res);
            }
            return res;
        }

        /// <summary> Find the elementwise minimum value </summary>
        /// <param name="img2">The second image for the Min operation</param>
        /// <returns> An image where each pixel is the minimum of <i>this</i> image and the parameter image</returns>
        public Image<TColor, TDepth> Min(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Min(this, img2, res);
            return res;
        }

        /// <summary> Find the elementwise minimum value </summary>
        /// <param name="value">The value to compare with</param>
        /// <returns> An image where each pixel is the minimum of <i>this</i> image and <paramref name="value"/></returns>
        public Image<TColor, TDepth> Min(double value)
        {
            Image<TColor, TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(value))
            {
                CvInvoke.Min(this, ia, res);
            }
            return res;
        }

        /// <summary>Checks that image elements lie between two scalars</summary>
        /// <param name="lower"> The inclusive lower limit of color value</param>
        /// <param name="higher"> The inclusive upper limit of color value</param>
        /// <returns> res[i,j] = 255 if <paramref name="lower"/> &lt;= this[i,j] &lt;= <paramref name="higher"/>, 0 otherwise</returns>
        [ExposableMethod(Exposable = true, Category = "Logic")]
        public Image<Gray, Byte> InRange(TColor lower, TColor higher)
        {
            Image<Gray, Byte> res = new Image<Gray, Byte>(Size);
            using (ScalarArray ialower = new ScalarArray(lower.MCvScalar))
            using (ScalarArray iaupper = new ScalarArray(higher.MCvScalar))
                CvInvoke.InRange(this, ialower, iaupper, res);
            return res;
        }

        /// <summary>Checks that image elements lie between values defined by two images of same size and type</summary>
        /// <param name="lower"> The inclusive lower limit of color value</param>
        /// <param name="higher"> The inclusive upper limit of color value</param>
        /// <returns> res[i,j] = 255 if <paramref name="lower"/>[i,j] &lt;= this[i,j] &lt;= <paramref name="higher"/>[i,j], 0 otherwise</returns>
        public Image<Gray, Byte> InRange(Image<TColor, TDepth> lower, Image<TColor, TDepth> higher)
        {
            Image<Gray, Byte> res = new Image<Gray, Byte>(Size);
            CvInvoke.InRange(this, lower, higher, res);
            return res;
        }

        /// <summary>
        /// Compare the current image with <paramref name="img2"/> and returns the comparison mask
        /// </summary>
        /// <param name="img2">The other image to compare with</param>
        /// <param name="cmpType">The comparison type</param>
        /// <returns>The result of the comparison as a mask</returns>
        public Image<TColor, Byte> Cmp(Image<TColor, TDepth> img2, CvEnum.CmpType cmpType)
        {
            Size size = Size;
            Image<TColor, Byte> res = new Image<TColor, byte>(size);

            if (NumberOfChannels == 1)
            {
                CvInvoke.Compare(this, img2, res, cmpType);
            }
            else
            {
                using (Image<Gray, TDepth> src1 = new Image<Gray, TDepth>(size))
                using (Image<Gray, TDepth> src2 = new Image<Gray, TDepth>(size))
                using (Image<Gray, Byte> dest = new Image<Gray, Byte>(size))
                    for (int i = 0; i < NumberOfChannels; i++)
                    {
                        CvInvoke.cvSetImageCOI(Ptr, i + 1);
                        CvInvoke.cvSetImageCOI(img2.Ptr, i + 1);
                        CvInvoke.cvCopy(Ptr, src1.Ptr, IntPtr.Zero);
                        CvInvoke.cvCopy(img2.Ptr, src2.Ptr, IntPtr.Zero);

                        CvInvoke.Compare(src1, src2, dest, cmpType);

                        CvInvoke.cvSetImageCOI(res.Ptr, i + 1);
                        CvInvoke.cvCopy(dest.Ptr, res.Ptr, IntPtr.Zero);
                    }
                CvInvoke.cvSetImageCOI(Ptr, 0);
                CvInvoke.cvSetImageCOI(img2.Ptr, 0);
                CvInvoke.cvSetImageCOI(res.Ptr, 0);
            }

            return res;
        }

        /// <summary>
        /// Compare the current image with <paramref name="value"/> and returns the comparison mask
        /// </summary>
        /// <param name="value">The value to compare with</param>
        /// <param name="comparisonType">The comparison type</param>
        /// <returns>The result of the comparison as a mask</returns>
        [ExposableMethod(Exposable = true, Category = "Logic")]
        public Image<TColor, Byte> Cmp(double value, CvEnum.CmpType comparisonType)
        {
            Size size = Size;
            Image<TColor, Byte> res = new Image<TColor, byte>(size);

            using (ScalarArray ia = new ScalarArray(value))
            {
                if (NumberOfChannels == 1)
                {
                    CvInvoke.Compare(this, ia, res, comparisonType);
                }
                else
                {
                    this.ForEachDuplicateChannel<Byte>(
                       delegate (IInputArray img1, IOutputArray img2, int channel)
                       {
                           CvInvoke.Compare(img1, ia, img2, comparisonType);
                       },
                       res);
                }
            }

            return res;
        }

        /// <summary>
        /// Compare two images, returns true if the each of the pixels are equal, false otherwise
        /// </summary>
        /// <param name="img2">The other image to compare with</param>
        /// <returns>true if the each of the pixels for the two images are equal, false otherwise</returns>
        public bool Equals(Image<TColor, TDepth> img2)
        {
            //true if the references are equal
            if (Object.ReferenceEquals(this, img2)) return true;

            //false if size are not equal
            if (!Size.Equals(img2.Size)) return false;

            using (Image<TColor, TDepth> neqMask = new Image<TColor, TDepth>(Size))
            {
                CvInvoke.BitwiseXor(this, img2, neqMask, null);
                if (NumberOfChannels == 1)
                    return CvInvoke.CountNonZero(neqMask) == 0;
                else
                {
                    IntPtr singleChannel = Marshal.AllocHGlobal(StructSize.MCvMat);
                    try
                    {
                        CvInvoke.cvReshape(neqMask, singleChannel, 1, 0);
                        using (Mat m = CvInvoke.CvArrToMat(singleChannel))
                        {
                            return CvInvoke.CountNonZero(m) == 0;
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(singleChannel);
                    }
                }
            }
        }
#endregion

#region Segmentation
        /// <summary>
        /// Use grabcut to perform background foreground segmentation.
        /// </summary>
        /// <param name="rect">The initial rectangle region for the foreground</param>
        /// <param name="iteration">The number of iterations to run GrabCut</param>
        /// <returns>The background foreground mask where 2 indicates background and 3 indicates foreground</returns>
        public Image<Gray, Byte> GrabCut(Rectangle rect, int iteration)
        {
            Image<Gray, Byte> mask = new Image<Gray, byte>(Size);
            using (Matrix<double> bgdModel = new Matrix<double>(1, 13 * 5))
            using (Matrix<double> fgdModel = new Matrix<double>(1, 13 * 5))
            {
                CvInvoke.GrabCut(this, mask, rect, bgdModel, fgdModel, 0, Emgu.CV.CvEnum.GrabcutInitType.InitWithRect);
                CvInvoke.GrabCut(this, mask, rect, bgdModel, fgdModel, iteration, Emgu.CV.CvEnum.GrabcutInitType.Eval);
            }
            return mask;
        }
#endregion

#region Arithmatic
#region Subtraction methods
        /// <summary> Elementwise subtract another image from the current image </summary>
        /// <param name="img2">The second image to be subtracted from the current image</param>
        /// <returns> The result of elementwise subtracting img2 from the current image</returns>
        public Image<TColor, TDepth> Sub(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Subtract(this, img2, res, null, CvInvoke.GetDepthType(typeof(TDepth)));
            return res;
        }

        /// <summary> Elementwise subtract another image from the current image, using a mask</summary>
        /// <param name="img2">The image to be subtracted from the current image</param>
        /// <param name="mask">The mask for the subtract operation</param>
        /// <returns> The result of elementwise subtracting img2 from the current image, using the specific mask</returns>
        public Image<TColor, TDepth> Sub(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Subtract(this, img2, res, mask, CvInvoke.GetDepthType(typeof(TDepth)));
            return res;
        }

        /// <summary> Elementwise subtract a color from the current image</summary>
        /// <param name="val">The color value to be subtracted from the current image</param>
        /// <returns> The result of elementwise subtracting color 'val' from the current image</returns>
        [ExposableMethod(Exposable = true, Category = "Math")]
        public Image<TColor, TDepth> Sub(TColor val)
        {
            Image<TColor, TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(val.MCvScalar))
            {
                CvInvoke.Subtract(this, ia, res, null, CvInvoke.GetDepthType(typeof(TDepth)));
            }
            return res;
        }

        /// <summary>
        /// result = val - this
        /// </summary>
        /// <param name="val">the value which subtract this image</param>
        /// <returns>val - this</returns>
        [ExposableMethod(Exposable = true, Category = "Math")]
        public Image<TColor, TDepth> SubR(TColor val)
        {
            return SubR(val, null);
        }

        /// <summary>
        /// result = val - this, using a mask
        /// </summary>
        /// <param name="val">The value which subtract this image</param>
        /// <param name="mask">The mask for subtraction</param>
        /// <returns>val - this, with mask</returns>
        public Image<TColor, TDepth> SubR(TColor val, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(val.MCvScalar))
            {
                CvInvoke.Subtract(ia, this, res, mask, CvInvoke.GetDepthType(typeof(TDepth)));
            }
            return res;
        }
#endregion

#region Addition methods
        /// <summary> Elementwise add another image with the current image </summary>
        /// <param name="img2">The image to be added to the current image</param>
        /// <returns> The result of elementwise adding img2 to the current image</returns>
        public Image<TColor, TDepth> Add(Image<TColor, TDepth> img2)
        {
            return Add(img2, null);
        }

        /// <summary> Elementwise add <paramref name="img2"/> with the current image, using a mask</summary>
        /// <param name="img2">The image to be added to the current image</param>
        /// <param name="mask">The mask for the add operation</param>
        /// <returns> The result of elementwise adding img2 to the current image, using the specific mask</returns>
        public Image<TColor, TDepth> Add(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Add(this, img2, res, mask, CvInvoke.GetDepthType(typeof(TDepth)));
            return res;
        }

        /// <summary> Elementwise add a color <paramref name="val"/> to the current image</summary>
        /// <param name="val">The color value to be added to the current image</param>
        /// <returns> The result of elementwise adding color <paramref name="val"/> from the current image</returns>
        [ExposableMethod(Exposable = true, Category = "Math")]
        public Image<TColor, TDepth> Add(TColor val)
        {
            Image<TColor, TDepth> res = CopyBlank();
            using (ScalarArray ia = new ScalarArray(val.MCvScalar))
            {
                CvInvoke.Add(this, ia, res, null, CvInvoke.GetDepthType(typeof(TDepth)));
            }
            return res;
        }
#endregion

#region Multiplication methods
        /// <summary> Elementwise multiply another image with the current image and the <paramref name="scale"/></summary>
        /// <param name="img2">The image to be elementwise multiplied to the current image</param>
        /// <param name="scale">The scale to be multiplied</param>
        /// <returns> this .* img2 * scale </returns>
        public Image<TColor, TDepth> Mul(Image<TColor, TDepth> img2, double scale)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Multiply(this, img2, res, scale, CvInvoke.GetDepthType(typeof(TDepth)));
            return res;
        }

        /// <summary> Elementwise multiply <paramref name="img2"/> with the current image</summary>
        /// <param name="img2">The image to be elementwise multiplied to the current image</param>
        /// <returns> this .* img2 </returns>
        public Image<TColor, TDepth> Mul(Image<TColor, TDepth> img2)
        {
            return Mul(img2, 1.0);
        }

        /// <summary> Elementwise multiply the current image with <paramref name="scale"/></summary>
        /// <param name="scale">The scale to be multiplied</param>
        /// <returns> The scaled image </returns>
        [ExposableMethod(Exposable = true, Category = "Math")]
        public Image<TColor, TDepth> Mul(double scale)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.cvConvertScale(Ptr, res.Ptr, scale, 0.0);
            return res;
        }
#endregion

        /// <summary>
        /// Accumulate <paramref name="img2"/> to the current image using the specific mask
        /// </summary>
        /// <param name="img2">The image to be added to the current image</param>
        /// <param name="mask">the mask</param>
        public void Accumulate(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            CvInvoke.Accumulate(img2, this, mask);
        }

        /// <summary>
        /// Accumulate <paramref name="img2"/> to the current image using the specific mask
        /// </summary>
        /// <param name="img2">The image to be added to the current image</param>
        public void Accumulate(Image<TColor, TDepth> img2)
        {
            CvInvoke.Accumulate(img2, this, null);
        }

        /// <summary> 
        /// Return the weighted sum such that: res = this * alpha + img2 * beta + gamma
        /// </summary>
        /// <param name="img2">img2 in: res = this * alpha + img2 * beta + gamma </param>
        /// <param name="alpha">alpha in: res = this * alpha + img2 * beta + gamma</param>
        /// <param name="beta">beta in: res = this * alpha + img2 * beta + gamma</param>
        /// <param name="gamma">gamma in: res = this * alpha + img2 * beta + gamma</param>
        /// <returns>this * alpha + img2 * beta + gamma</returns>
        public Image<TColor, TDepth> AddWeighted(Image<TColor, TDepth> img2, double alpha, double beta, double gamma)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.AddWeighted(this, alpha, img2, beta, gamma, res, CvInvoke.GetDepthType(typeof(TDepth)));
            return res;
        }

        /// <summary> 
        /// Update Running Average. <i>this</i> = (1-alpha)*<i>this</i> + alpha*img
        /// </summary>
        /// <param name="img">Input image, 1- or 3-channel, Byte or Single (each channel of multi-channel image is processed independently). </param>
        /// <param name="alpha">the weight of <paramref name="img"/></param>
        public void AccumulateWeighted(Image<TColor, TDepth> img, double alpha)
        {
            AccumulateWeighted(img, alpha, null);
        }

        /// <summary> 
        /// Update Running Average. <i>this</i> = (1-alpha)*<i>this</i> + alpha*img, using the mask
        /// </summary>
        /// <param name="img">Input image, 1- or 3-channel, Byte or Single (each channel of multi-channel image is processed independently). </param>
        /// <param name="alpha">The weight of <paramref name="img"/></param>
        /// <param name="mask">The mask for the running average</param>
        public void AccumulateWeighted(Image<TColor, TDepth> img, double alpha, Image<Gray, Byte> mask)
        {
            CvInvoke.AccumulateWeighted(img, this, alpha, mask);
        }

        /// <summary> 
        /// Computes absolute different between <i>this</i> image and the other image
        /// </summary>
        /// <param name="img2">The other image to compute absolute different with</param>
        /// <returns> The image that contains the absolute different value</returns>
        public Image<TColor, TDepth> AbsDiff(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.AbsDiff(this, img2, res);
            return res;
        }

        /// <summary> 
        /// Computes absolute different between <i>this</i> image and the specific color
        /// </summary>
        /// <param name="color">The color to compute absolute different with</param>
        /// <returns> The image that contains the absolute different value</returns>
        [ExposableMethod(Exposable = true, Category = "Math")]
        public Image<TColor, TDepth> AbsDiff(TColor color)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Size);
            using (ScalarArray ia = new ScalarArray(color.MCvScalar))
            {
                CvInvoke.AbsDiff(this, ia, res);
            }
            return res;
        }
#endregion

#region Math Functions
        /// <summary>
        /// Raises every element of input array to p
        /// dst(I)=src(I)^p, if p is integer
        /// dst(I)=abs(src(I))^p, otherwise
        /// </summary>
        /// <param name="power">The exponent of power</param>
        /// <returns>The power image</returns>
        [ExposableMethod(Exposable = true, Category = "Math")]
        public Image<TColor, TDepth> Pow(double power)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Pow(this, power, res);
            return res;
        }

        /// <summary>
        /// Calculates exponent of every element of input array:
        /// dst(I)=exp(src(I))
        /// </summary>
        /// <remarks>Maximum relative error is ~7e-6. Currently, the function converts denormalized values to zeros on output.</remarks>
        /// <returns>The exponent image</returns>
        [ExposableMethod(Exposable = true, Category = "Math")]
        public Image<TColor, TDepth> Exp()
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Exp(this, res);
            return res;
        }

        /// <summary>
        /// Calculates natural logarithm of absolute value of every element of input array
        /// </summary>
        /// <returns>Natural logarithm of absolute value of every element of input array</returns>
        [ExposableMethod(Exposable = true, Category = "Math")]
        public Image<TColor, TDepth> Log()
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Log(this, res);
            return res;
        }
#endregion

#region Sampling, Interpolation and Geometrical Transforms
        /*
        ///<summary> Sample the pixel values on the specific line segment </summary>
        ///<param name="line"> The line to obtain samples</param>
        ///<returns>The values on the (Eight-connected) line </returns>
        public TDepth[,] Sample(LineSegment2D line)
        {
            return Sample(line, Emgu.CV.CvEnum.Connectivity.EightConnected);
        }

        /// <summary>
        /// Sample the pixel values on the specific line segment
        /// </summary>
        /// <param name="line">The line to obtain samples</param>
        /// <param name="type">The sampling type</param>
        /// <returns>The values on the line, the first dimension is the index of the point, the second dimension is the index of color channel</returns>
        public TDepth[,] Sample(LineSegment2D line, CvEnum.Connectivity type)
        {
            int size = type == Emgu.CV.CvEnum.Connectivity.EightConnected ?
               Math.Max(Math.Abs(line.P2.X - line.P1.X), Math.Abs(line.P2.Y - line.P1.Y))
               : Math.Abs(line.P2.X - line.P1.X) + Math.Abs(line.P2.Y - line.P1.Y);

            TDepth[,] data = new TDepth[size, NumberOfChannels];
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            Point p1 = line.P1;
            Point p2 = line.P2;
            CvInvoke.cvSampleLine(
                Ptr,
                ref p1,
                ref p2,
                handle.AddrOfPinnedObject(),
                type);
            handle.Free();
            return data;
        }*/

        /// <summary>
        /// Scale the image to the specific size 
        /// </summary>
        /// <param name="width">The width of the returned image.</param>
        /// <param name="height">The height of the returned image.</param>
        /// <param name="interpolationType">The type of interpolation</param>
        /// <returns>The resized image</returns>
        [ExposableMethod(Exposable = true)]
        public Image<TColor, TDepth> Resize(int width, int height, CvEnum.Inter interpolationType)
        {
            Image<TColor, TDepth> imgScale = new Image<TColor, TDepth>(width, height);
            CvInvoke.Resize(this, imgScale, new Size(width, height), 0, 0, interpolationType);
            return imgScale;
        }

        /// <summary>
        /// Scale the image to the specific size
        /// </summary>
        /// <param name="width">The width of the returned image.</param>
        /// <param name="height">The height of the returned image.</param>
        /// <param name="interpolationType">The type of interpolation</param>
        /// <param name="preserveScale">if true, the scale is preservered and the resulting image has maximum width(height) possible that is &lt;= <paramref name="width"/> (<paramref name="height"/>), if false, this function is equaivalent to Resize(int width, int height)</param>
        /// <returns>The resized image</returns>
        public Image<TColor, TDepth> Resize(int width, int height, CvEnum.Inter interpolationType, bool preserveScale)
        {
            return preserveScale ?
               Resize(Math.Min((double)width / Width, (double)height / Height), interpolationType)
               : Resize(width, height, interpolationType);
        }

        /// <summary>
        /// Scale the image to the specific size: width *= scale; height *= scale  
        /// </summary>
        /// <param name="scale">The scale to resize</param>
        /// <param name="interpolationType">The type of interpolation</param>
        /// <returns>The scaled image</returns>
        [ExposableMethod(Exposable = true)]
        public Image<TColor, TDepth> Resize(double scale, CvEnum.Inter interpolationType)
        {
            return Resize(
                (int)(Width * scale),
                (int)(Height * scale),
                interpolationType);
        }

        /// <summary>
        /// Rotate the image the specified angle cropping the result to the original size
        /// </summary>
        /// <param name="angle">The angle of rotation in degrees.</param>
        /// <param name="background">The color with which to fill the background</param>   
        /// <returns>The image rotates by the specific angle</returns>
        public Image<TColor, TDepth> Rotate(double angle, TColor background)
        {
            return Rotate(angle, background, true);
        }

        /// <summary>
        /// Transforms source image using the specified matrix
        /// </summary>
        /// <param name="mapMatrix">2x3 transformation matrix</param>
        /// <param name="interpolationType">Interpolation type</param>
        /// <param name="warpType">Warp type</param>
        /// <param name="borderMode">Pixel extrapolation method</param>
        /// <param name="backgroundColor">A value used to fill outliers</param>
        /// <returns>The result of the transformation</returns>
        public Image<TColor, TDepth> WarpAffine(Mat mapMatrix, CvEnum.Inter interpolationType, CvEnum.Warp warpType, CvEnum.BorderType borderMode, TColor backgroundColor)
        {
            return WarpAffine(mapMatrix, Width, Height, interpolationType, warpType, borderMode, backgroundColor);
        }

        /// <summary>
        /// Transforms source image using the specified matrix
        /// </summary>
        /// <param name="mapMatrix">2x3 transformation matrix</param>
        /// <param name="width">The width of the resulting image</param>
        /// <param name="height">the height of the resulting image</param>
        /// <param name="interpolationType">Interpolation type</param>
        /// <param name="warpType">Warp type</param>
        /// <param name="borderMode">Pixel extrapolation method</param>
        /// <param name="backgroundColor">A value used to fill outliers</param>
        /// <returns>The result of the transformation</returns>
        public Image<TColor, TDepth> WarpAffine(Mat mapMatrix, int width, int height, CvEnum.Inter interpolationType, CvEnum.Warp warpType, CvEnum.BorderType borderMode, TColor backgroundColor)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(width, height);
            CvInvoke.WarpAffine(this, res, mapMatrix, res.Size, interpolationType, warpType, borderMode, backgroundColor.MCvScalar);
            return res;
        }

        /// <summary>
        /// Transforms source image using the specified matrix
        /// </summary>
        /// <param name="mapMatrix">3x3 transformation matrix</param>
        /// <param name="interpolationType">Interpolation type</param>
        /// <param name="warpType">Warp type</param>
        /// <param name="borderMode">Pixel extrapolation method</param>
        /// <param name="backgroundColor">A value used to fill outliers</param>
        /// <typeparam name="TMapDepth">The depth type of <paramref name="mapMatrix"/>, should be either float or double</typeparam>
        /// <returns>The result of the transformation</returns>
        public Image<TColor, TDepth> WarpPerspective<TMapDepth>(Matrix<TMapDepth> mapMatrix, CvEnum.Inter interpolationType, CvEnum.Warp warpType, CvEnum.BorderType borderMode, TColor backgroundColor)
           where TMapDepth : new()
        {
            return WarpPerspective(mapMatrix, Width, Height, interpolationType, warpType, borderMode, backgroundColor);
        }

        /// <summary>
        /// Transforms source image using the specified matrix
        /// </summary>
        /// <param name="mapMatrix">3x3 transformation matrix</param>
        /// <param name="width">The width of the resulting image</param>
        /// <param name="height">the height of the resulting image</param>
        /// <param name="interpolationType">Interpolation type</param>
        /// <param name="warpType">Warp type</param>
        /// <param name="borderType">Border type</param>
        /// <param name="backgroundColor">A value used to fill outliers</param>
        /// <typeparam name="TMapDepth">The depth type of <paramref name="mapMatrix"/>, should be either float or double</typeparam>
        /// <returns>The result of the transformation</returns>
        public Image<TColor, TDepth> WarpPerspective<TMapDepth>(
           Matrix<TMapDepth> mapMatrix,
           int width, int height,
           CvEnum.Inter interpolationType,
           CvEnum.Warp warpType,
           CvEnum.BorderType borderType,
           TColor backgroundColor)
           where TMapDepth : new()
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(width, height);
            CvInvoke.WarpPerspective(this, res, mapMatrix, res.Size, interpolationType, warpType, borderType, backgroundColor.MCvScalar);
            return res;
        }

        /// <summary>
        /// Rotate this image the specified <paramref name="angle"/>
        /// </summary>
        /// <param name="angle">The angle of rotation in degrees.</param>
        /// <param name="background">The color with which to fill the background</param>
        /// <param name="crop">If set to true the image is cropped to its original size, possibly losing corners information. If set to false the result image has different size than original and all rotation information is preserved</param>
        /// <returns>The rotated image</returns>
        [ExposableMethod(Exposable = true, Category = "Transform")]
        public Image<TColor, TDepth> Rotate(double angle, TColor background, bool crop)
        {
            Size size = Size;
            PointF center = new PointF(size.Width * 0.5f, size.Height * 0.5f);
            return Rotate(angle, center, CvEnum.Inter.Cubic, background, crop);
        }

        /// <summary>
        /// Rotate this image the specified <paramref name="angle"/>
        /// </summary>
        /// <param name="angle">The angle of rotation in degrees. Positive means clockwise.</param>
        /// <param name="background">The color with with to fill the background</param>
        /// <param name="crop">If set to true the image is cropped to its original size, possibly losing corners information. If set to false the result image has different size than original and all rotation information is preserved</param>
        /// <param name="center">The center of rotation</param>
        /// <param name="interpolationMethod">The interpolation method</param>
        /// <returns>The rotated image</returns>
        public Image<TColor, TDepth> Rotate(double angle, PointF center, CvEnum.Inter interpolationMethod, TColor background, bool crop)
        {
            if (crop)
            {
                using (Mat rotationMatrix = new Mat())
                {
                    CvInvoke.GetRotationMatrix2D(center, -angle, 1, rotationMatrix);

                    return WarpAffine(rotationMatrix, interpolationMethod, Emgu.CV.CvEnum.Warp.FillOutliers,
                       CvEnum.BorderType.Constant, background);
                }
            }
            else
            {
                Size dstImgSize;
                using (Mat rotationMatrix = RotationMatrix2D.CreateRotationMatrix(center, -angle, Size, out dstImgSize))
                {
                    //CvInvoke.GetRotationMatrix2D(center, -angle, 1.0, rotationMatrix);
                    return WarpAffine(rotationMatrix, dstImgSize.Width, dstImgSize.Height, interpolationMethod, Emgu.CV.CvEnum.Warp.FillOutliers, CvEnum.BorderType.Constant, background);
                }
            }
        }

#if !(UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE || UNITY_METRO)
        /// <summary>
        /// Convert the image to log polar, simulating the human foveal vision
        /// </summary>
        /// <param name="center">The transformation center, where the output precision is maximal</param>
        /// <param name="magnitude">Magnitude scale parameter</param>
        /// <param name="interpolationType">interpolation type</param>
        /// <param name="warpType">Warp type</param>
        /// <returns>The converted image</returns>
        [ExposableMethod(Exposable = true, Category = "Transform")]
        public Image<TColor, TDepth> LogPolar(
           PointF center,
           double magnitude,
           CvEnum.Inter interpolationType = CvEnum.Inter.Linear,
           CvEnum.Warp warpType = CvEnum.Warp.FillOutliers)
        {
            Image<TColor, TDepth> imgPolar = CopyBlank();
            CvInvoke.LogPolar(this, imgPolar, center, magnitude, interpolationType, warpType);
            return imgPolar;
        }
#endif
#endregion

#region Image color and depth conversion
        /// <summary> Convert the current image to the specific color and depth </summary>
        /// <typeparam name="TOtherColor"> The type of color to be converted to </typeparam>
        /// <typeparam name="TOtherDepth"> The type of pixel depth to be converted to </typeparam>
        /// <returns> Image of the specific color and depth </returns>
        [ExposableMethod(
           Exposable = true,
           Category = "Conversion",
           GenericParametersOptions = new Type[] {
            typeof(Bgr), typeof(Bgra), typeof(Gray), typeof(Hsv), typeof(Hls), typeof(Lab), typeof(Luv), typeof(Xyz), typeof(Ycc),
            typeof(Single), typeof(Byte), typeof(Double)},
           GenericParametersOptionSizes = new int[] { 9, 3 }
           )]
        public Image<TOtherColor, TOtherDepth> Convert<TOtherColor, TOtherDepth>()
           where TOtherColor : struct, IColor
           where TOtherDepth : new()
        {
            Image<TOtherColor, TOtherDepth> res = new Image<TOtherColor, TOtherDepth>(Size);
            res.ConvertFrom(this);
            return res;
        }

        /// <summary>
        /// Convert the source image to the current image, if the size are different, the current image will be a resized version of the srcImage. 
        /// </summary>
        /// <typeparam name="TSrcColor">The color type of the source image</typeparam>
        /// <typeparam name="TSrcDepth">The color depth of the source image</typeparam>
        /// <param name="srcImage">The sourceImage</param>
        public void ConvertFrom<TSrcColor, TSrcDepth>(Image<TSrcColor, TSrcDepth> srcImage)
           where TSrcColor : struct, IColor
           where TSrcDepth : new()
        {
            if (!Size.Equals(srcImage.Size))
            {  //if the size of the source image do not match the size of the current image
                using (Image<TSrcColor, TSrcDepth> tmp = new Image<TSrcColor, TSrcDepth>(this.Size))
                {
                    CvInvoke.Resize(srcImage, tmp, this.Size);
                    ConvertFrom(tmp);
                    return;
                }
            }

            if (typeof(TColor) == typeof(TSrcColor))
            {
#region same color
                if (typeof(TDepth) == typeof(TSrcDepth))
                {   //same depth
                    srcImage.Mat.CopyTo(this);
                    //CvInvoke.cvCopy(srcImage.Ptr, Ptr, IntPtr.Zero);
                }
                else
                {
                    //different depth
                    //int channelCount = NumberOfChannels;
                    {
                        if (typeof(TDepth) == typeof(Byte) && typeof(TSrcDepth) != typeof(Byte))
                        {
                            double[] minVal, maxVal;
                            Point[] minLoc, maxLoc;
                            srcImage.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
                            double min = minVal[0];
                            double max = maxVal[0];
                            for (int i = 1; i < minVal.Length; i++)
                            {
                                min = Math.Min(min, minVal[i]);
                                max = Math.Max(max, maxVal[i]);
                            }
                            double scale = 1.0, shift = 0.0;
                            if (max > 255.0 || min < 0)
                            {
                                scale = (max.Equals(min)) ? 0.0 : 255.0 / (max - min);
                                shift = (scale.Equals(0)) ? min : -min * scale;
                            }

                            CvInvoke.ConvertScaleAbs(srcImage, this, scale, shift);
                        }
                        else
                        {
                            srcImage.Mat.ConvertTo(this, this.Mat.Depth, 1.0, 0.0);
                            //CvInvoke.cvConvertScale(srcImage, this, 1.0, 0.0);
                        }
                    }
                }
#endregion
            }
            else
            {
#region different color
                if (typeof(TDepth) == typeof(TSrcDepth))
                {   //same depth
                    CvInvoke.CvtColor(srcImage, this, typeof(TSrcColor), typeof(TColor));
                }
                else
                {   //different depth
                    if (typeof(TSrcDepth) == typeof(Byte))
                    {   //Do color conversion first, then depth conversion
                        using (Image< TColor, TSrcDepth > tmp = srcImage.Convert<TColor, TSrcDepth>())
                        {
                            this.ConvertFrom(tmp);
                        }
                    }
                    else
                    {   //Do depth conversion first, then color conversion
                        using (Image<TSrcColor, TDepth> tmp = srcImage.Convert<TSrcColor, TDepth>()) //convert depth
                            //using (Mat tmp = new CV.Mat())
                        {
                            //srcImage.Mat.ConvertTo(tmp, CvInvoke.GetDepthType(typeof(TDepth), 
                            CvInvoke.CvtColor(tmp, this, typeof(TSrcColor), typeof(TColor));
                        }
                    }
                }
#endregion
            }
        }


        /// <summary>
        /// Convert the source image to the current image, if the size are different, the current image will be a resized version of the srcImage. 
        /// </summary>
        /// <param name="srcImage">The sourceImage</param>
        public void ConvertFrom(IInputArray srcImage)
        {
            using (InputArray iaSrcImage = srcImage.GetInputArray())
            {
                Size srcImageSize = iaSrcImage.GetSize();
                if (!Size.Equals(srcImageSize))
                {
                    //if the size of the source image do not match the size of the current image
                    using (Mat tmp = new Mat())
                    {
                        CvInvoke.Resize(srcImage, tmp, this.Size);
                        ConvertFrom(tmp);
                        return;
                    }
                }

                int srcImageNumberOfChannels = iaSrcImage.GetChannels();
                if (NumberOfChannels == srcImageNumberOfChannels)
                {
#region same color

                    DepthType srcImageDepth = iaSrcImage.GetDepth();
                    if (CvInvoke.GetDepthType(typeof(TDepth)) == srcImageDepth)
                    {
                        //same depth
                        iaSrcImage.CopyTo(this);
                        //srcImage.CopyTo(this);
                    }
                    else
                    {
                        //different depth
                        //int channelCount = NumberOfChannels;
                        {
                            if (typeof(TDepth) == typeof(Byte) && DepthType.Cv8U != CvInvoke.GetDepthType(typeof(Byte)))
                            {
                                double[] minVal, maxVal;
                                Point[] minLoc, maxLoc;
                                CvInvoke.MinMax(srcImage, out minVal, out maxVal, out minLoc, out maxLoc);

                                double min = minVal[0];
                                double max = maxVal[0];
                                for (int i = 1; i < minVal.Length; i++)
                                {
                                    min = Math.Min(min, minVal[i]);
                                    max = Math.Max(max, maxVal[i]);
                                }
                                double scale = 1.0, shift = 0.0;
                                if (max > 255.0 || min < 0)
                                {
                                    scale = max.Equals(min) ? 0.0 : 255.0 / (max - min);
                                    shift = scale.Equals(0) ? min : -min * scale;
                                }

                                CvInvoke.ConvertScaleAbs(srcImage, this, scale, shift);
                            }
                            else
                            {
                                using (Mat srcMat = iaSrcImage.GetMat())
                                {
                                    srcMat.ConvertTo(this, this.Mat.Depth, 1.0, 0.0);
                                }
                            }
                        }
                    }

#endregion
                }
                else
                {
                    if (!(srcImageNumberOfChannels == 1 || srcImageNumberOfChannels == 3 || srcImageNumberOfChannels == 4))
                        throw new Exception("Color conversion not suppported");
                    Type srcColorType =
                       srcImageNumberOfChannels == 1
                          ? typeof(Gray)
                          : srcImageNumberOfChannels == 3
                             ? typeof(Bgr)
                             : typeof(Bgra);

#region different color
                    DepthType srcImageDepth = iaSrcImage.GetDepth();
                    if (CvInvoke.GetDepthType(typeof(TDepth)) == srcImageDepth)
                    {
                        //same depth
                        CvInvoke.CvtColor(srcImage, this, srcColorType, typeof(TColor));
                    }
                    else
                    {
                        //different depth
                        //using (Image<TSrcColor, TDepth> tmp = srcImage.Convert<TSrcColor, TDepth>()) //convert depth
                        using (Mat tmp = new Mat())
                        using (Mat srcMat = iaSrcImage.GetMat())
                        {
                            srcMat.ConvertTo(tmp, CvInvoke.GetDepthType(typeof(TDepth)));
                            //srcImage.Mat.ConvertTo(tmp, CvInvoke.GetDepthType(typeof(TDepth), 
                            CvInvoke.CvtColor(tmp, this, srcColorType, typeof(TColor));
                        }
                    }

#endregion
                }
            }
        }

        /// <summary> Convert the current image to the specific depth, at the same time scale and shift the values of the pixel</summary>
        /// <param name="scale"> The value to be multiplied with the pixel </param>
        /// <param name="shift"> The value to be added to the pixel</param>
        /// <typeparam name="TOtherDepth"> The type of depth to convert to</typeparam>
        /// <returns> Image of the specific depth, val = val * scale + shift </returns>
        public Image<TColor, TOtherDepth> ConvertScale<TOtherDepth>(double scale, double shift)
                    where TOtherDepth : new()
        {
            Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Width, Height);

            if (typeof(TOtherDepth) == typeof(Byte))
                CvInvoke.ConvertScaleAbs(this, res, scale, shift);
            else
                CvInvoke.cvConvertScale(this, res, scale, shift);

            return res;
        }
#endregion

#region Pyramids
        /// <summary>
        /// Performs downsampling step of Gaussian pyramid decomposition. 
        /// First it convolves <i>this</i> image with the specified filter and then downsamples the image 
        /// by rejecting even rows and columns.
        /// </summary>
        /// <returns> The downsampled image</returns>
        [ExposableMethod(Exposable = true, Category = "Pyramids")]
        public Image<TColor, TDepth> PyrDown()
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width >> 1, Height >> 1);
            CvInvoke.PyrDown(this, res, CvEnum.BorderType.Default);
            return res;
        }

        /// <summary>
        /// Performs up-sampling step of Gaussian pyramid decomposition. 
        /// First it upsamples <i>this</i> image by injecting even zero rows and columns and then convolves 
        /// result with the specified filter multiplied by 4 for interpolation. 
        /// So the resulting image is four times larger than the source image.
        /// </summary>
        /// <returns> The upsampled image</returns>
        [ExposableMethod(Exposable = true, Category = "Pyramids")]
        public Image<TColor, TDepth> PyrUp()
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width << 1, Height << 1);
            CvInvoke.PyrUp(this, res, CvEnum.BorderType.Default);
            return res;
        }

        /// <summary>
        /// Compute the image pyramid
        /// </summary>
        /// <param name="maxLevel">The number of level's for the pyramid; Level 0 referes to the current image, level n is computed by calling the PyrDown() function on level n-1</param>
        /// <returns>The image pyramid</returns>
        public Image<TColor, TDepth>[] BuildPyramid(int maxLevel)
        {
            Debug.Assert(maxLevel >= 0, "The pyramid should have at lease maxLevel of 0");
            Image<TColor, TDepth>[] pyr = new Image<TColor, TDepth>[maxLevel + 1];
            pyr[0] = this;

            for (int i = 1; i <= maxLevel; i++)
                pyr[i] = pyr[i - 1].PyrDown();

            return pyr;
        }
#endregion

#region Special Image Transforms
        /// <summary> Use inpaint to recover the intensity of the pixels which location defined by <paramref name="mask"/> on <i>this</i> image </summary>
        /// <param name="mask">The inpainting mask. Non-zero pixels indicate the area that needs to be inpainted</param>
        /// <param name="radius">The radius of circular neighborhood of each point inpainted that is considered by the algorithm</param>
        /// <returns> The inpainted image </returns>
        public Image<TColor, TDepth> InPaint(Image<Gray, Byte> mask, double radius)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Inpaint(this, mask, res, radius, CvEnum.InpaintType.Telea);
            return res;
        }
#endregion

#region Morphological Operations
        /// <summary>
        /// Perform advanced morphological transformations using erosion and dilation as basic operations.
        /// </summary>
        /// <param name="kernel">Structuring element</param>
        /// <param name="anchor">Anchor position with the kernel. Negative values mean that the anchor is at the kernel center.</param>
        /// <param name="operation">Type of morphological operation</param>
        /// <param name="iterations">Number of times erosion and dilation are applied</param>
        /// <param name="borderType">Border type</param>
        /// <param name="borderValue">Border value</param>
        /// <returns>The result of the morphological operation</returns>
        public Image<TColor, TDepth> MorphologyEx(CvEnum.MorphOp operation, IInputArray kernel, Point anchor, int iterations, CvEnum.BorderType borderType, MCvScalar borderValue)
        {
            Image<TColor, TDepth> res = CopyBlank();

            CvInvoke.MorphologyEx(
               this, res, operation,
               kernel, anchor, iterations, borderType, borderValue);
            return res;
        }

        /// <summary>
        /// Perform inplace advanced morphological transformations using erosion and dilation as basic operations.
        /// </summary>
        /// <param name="kernel">Structuring element</param>
        /// <param name="anchor">Anchor position with the kernel. Negative values mean that the anchor is at the kernel center.</param>
        /// <param name="operation">Type of morphological operation</param>
        /// <param name="iterations">Number of times erosion and dilation are applied</param>
        /// <param name="borderType">Border type</param>
        /// <param name="borderValue">Border value</param>
        public void _MorphologyEx(CvEnum.MorphOp operation, IInputArray kernel, Point anchor, int iterations, CvEnum.BorderType borderType, MCvScalar borderValue)
        {
            CvInvoke.MorphologyEx(
               this, this, operation,
               kernel, anchor, iterations, borderType, borderValue);
        }

        /// <summary>
        /// Erodes <i>this</i> image using a 3x3 rectangular structuring element.
        /// Erosion are applied several (iterations) times
        /// </summary>
        /// <param name="iterations">The number of erode iterations</param>
        /// <returns> The eroded image</returns>
        public Image<TColor, TDepth> Erode(int iterations)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Erode(this, res, null, new Point(-1, -1), iterations, CvEnum.BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            return res;
        }

        /// <summary>
        /// Dilates <i>this</i> image using a 3x3 rectangular structuring element.
        /// Dilation are applied several (iterations) times
        /// </summary>
        /// <param name="iterations">The number of dilate iterations</param>
        /// <returns> The dilated image</returns>
        public Image<TColor, TDepth> Dilate(int iterations)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Dilate(this, res, null, new Point(-1, -1), iterations, CvEnum.BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            return res;
        }

        /// <summary>
        /// Erodes <i>this</i> image inplace using a 3x3 rectangular structuring element.
        /// Erosion are applied several (iterations) times
        /// </summary>
        /// <param name="iterations">The number of erode iterations</param>
        [ExposableMethod(Exposable = true, Category = "Morphology")]
        public void _Erode(int iterations)
        {
            CvInvoke.Erode(this, this, null, new Point(-1, -1), iterations, CvEnum.BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
        }

        /// <summary>
        /// Dilates <i>this</i> image inplace using a 3x3 rectangular structuring element.
        /// Dilation are applied several (iterations) times
        /// </summary>
        /// <param name="iterations">The number of dilate iterations</param>
        [ExposableMethod(Exposable = true, Category = "Morphology")]
        public void _Dilate(int iterations)
        {
            CvInvoke.Dilate(this, this, null, new Point(-1, -1), iterations, CvEnum.BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
        }
#endregion

#region generic operations
        /// <summary> 
        /// perform an generic action based on each element of the image
        /// </summary>
        /// <param name="action">The action to be applied to each element of the image</param>
        public void Action(Action<TDepth> action)
        {
            int cols1 = Width * new TColor().Dimension;

            int step1;
            IntPtr start;
            Size roiSize;
            CvInvoke.cvGetRawData(Ptr, out start, out step1, out roiSize);
            Int64 data1 = start.ToInt64();
            int width1 = SizeOfElement * cols1;

            using (PinnedArray<TDepth> row1 = new PinnedArray<TDepth>(cols1))
                for (int row = 0; row < Height; row++, data1 += step1)
                {
                    CvToolbox.Memcpy(row1.AddrOfPinnedObject(), new IntPtr(data1), width1);
                    foreach (TDepth v in row1.Array)
                        action(v);
                }
        }

        /// <summary>
        /// Perform an generic operation based on the elements of the two images
        /// </summary>
        /// <typeparam name="TOtherDepth">The depth of the second image</typeparam>
        /// <param name="img2">The second image to perform action on</param>
        /// <param name="action">An action such that the first parameter is the a single channel of a pixel from the first image, the second parameter is the corresponding channel of the correspondind pixel from the second image </param>
        public void Action<TOtherDepth>(Image<TColor, TOtherDepth> img2, Action<TDepth, TOtherDepth> action)
                    where TOtherDepth : new()
        {
            Debug.Assert(Size.Equals(img2.Size));

            Int64 data1;
            int height1, cols1, width1, step1;
            RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

            Int64 data2;
            int height2, cols2, width2, step2;
            RoiParam(img2.Ptr, out data2, out height2, out cols2, out width2, out step2);

            TDepth[] row1 = new TDepth[cols1];
            TOtherDepth[] row2 = new TOtherDepth[cols1];
            GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);

            for (int row = 0; row < height1; row++, data1 += step1, data2 += step2)
            {
                CvToolbox.Memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                CvToolbox.Memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
                for (int col = 0; col < cols1; action(row1[col], row2[col]), col++) ;
            }
            handle1.Free();
            handle2.Free();
        }

        /// <summary> 
        /// Compute the element of a new image based on the value as well as the x and y positions of each pixel on the image
        /// </summary>
        /// <param name="converter">The function to be applied to the image pixels</param>
        /// <returns>The result image</returns>
        public Image<TColor, TOtherDepth> Convert<TOtherDepth>(Func<TDepth, int, int, TOtherDepth> converter)
           where TOtherDepth : new()
        {
            Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Width, Height);

            int nchannel = MIplImage.NChannels;

            Int64 data1;
            int height1, cols1, width1, step1;
            RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

            Int64 data2;
            int height2, cols2, width2, step2;
            RoiParam(res.Ptr, out data2, out height2, out cols2, out width2, out step2);

            TDepth[] row1 = new TDepth[cols1];
            TOtherDepth[] row2 = new TOtherDepth[cols1];
            GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);

            for (int row = 0; row < height1; row++, data1 += step1, data2 += step2)
            {
                CvToolbox.Memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                for (int col = 0; col < cols1; row2[col] = converter(row1[col], row, col / nchannel), col++) ;
                CvToolbox.Memcpy((IntPtr)data2, handle2.AddrOfPinnedObject(), width2);
            }
            handle1.Free();
            handle2.Free();
            return res;
        }

        /// <summary> Compute the element of the new image based on element of this image</summary>
        /// <typeparam name="TOtherDepth">The depth type of the result image</typeparam>
        /// <param name="converter">The function to be applied to the image pixels</param>
        /// <returns>The result image</returns>
        public Image<TColor, TOtherDepth> Convert<TOtherDepth>(Func<TDepth, TOtherDepth> converter)
           where TOtherDepth : new()
        {
            Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Size);

            Int64 data1;
            int height1, cols1, width1, step1;
            RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

            Int64 data2;
            int height2, cols2, width2, step2;
            RoiParam(res.Ptr, out data2, out height2, out cols2, out width2, out step2);

            TDepth[] row1 = new TDepth[cols1];
            TOtherDepth[] row2 = new TOtherDepth[cols1];

            GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);
            for (int row = 0; row < height1; row++, data1 += step1, data2 += step2)
            {
                CvToolbox.Memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                for (int col = 0; col < cols1; row2[col] = converter(row1[col]), col++) ;
                CvToolbox.Memcpy((IntPtr)data2, handle2.AddrOfPinnedObject(), width2);
            }
            handle1.Free();
            handle2.Free();
            return res;
        }

        /// <summary> Compute the element of the new image based on the elements of the two image</summary>
        /// <typeparam name="TDepth2">The depth type of img2</typeparam>
        /// <typeparam name="TDepth3">The depth type of the result image</typeparam>
        /// <param name="img2">The second image</param>
        /// <param name="converter">The function to be applied to the image pixels</param>
        /// <returns>The result image</returns>
        public Image<TColor, TDepth3> Convert<TDepth2, TDepth3>(Image<TColor, TDepth2> img2, Func<TDepth, TDepth2, TDepth3> converter)
           where TDepth2 : new()
           where TDepth3 : new()
        {
            Debug.Assert(Size.Equals(img2.Size), "Image size do not match");

            Image<TColor, TDepth3> res = new Image<TColor, TDepth3>(Width, Height);

            Int64 data1;
            int height1, cols1, width1, step1;
            RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

            Int64 data2;
            int height2, cols2, width2, step2;
            RoiParam(img2.Ptr, out data2, out height2, out cols2, out width2, out step2);

            Int64 data3;
            int height3, cols3, width3, step3;
            RoiParam(res.Ptr, out data3, out height3, out cols3, out width3, out step3);

            TDepth[] row1 = new TDepth[cols1];
            TDepth2[] row2 = new TDepth2[cols1];
            TDepth3[] row3 = new TDepth3[cols1];
            GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);
            GCHandle handle3 = GCHandle.Alloc(row3, GCHandleType.Pinned);

            for (int row = 0; row < height1; row++, data1 += step1, data2 += step2, data3 += step3)
            {
                CvToolbox.Memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                CvToolbox.Memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
                for (int col = 0; col < cols1; row3[col] = converter(row1[col], row2[col]), col++) ;
                CvToolbox.Memcpy((IntPtr)data3, handle3.AddrOfPinnedObject(), width3);
            }

            handle1.Free();
            handle2.Free();
            handle3.Free();

            return res;
        }

        /// <summary> Compute the element of the new image based on the elements of the three image</summary>
        /// <typeparam name="TDepth2">The depth type of img2</typeparam>
        /// <typeparam name="TDepth3">The depth type of img3</typeparam>
        /// <typeparam name="TDepth4">The depth type of the result image</typeparam>
        /// <param name="img2">The second image</param>
        /// <param name="img3">The third image</param>
        /// <param name="converter">The function to be applied to the image pixels</param>
        /// <returns>The result image</returns>
        public Image<TColor, TDepth4> Convert<TDepth2, TDepth3, TDepth4>(Image<TColor, TDepth2> img2, Image<TColor, TDepth3> img3, Func<TDepth, TDepth2, TDepth3, TDepth4> converter)
           where TDepth2 : new()
           where TDepth3 : new()
           where TDepth4 : new()
        {
            Debug.Assert(Size.Equals(img2.Size) && Size.Equals(img3.Size), "Image size do not match");

            Image<TColor, TDepth4> res = new Image<TColor, TDepth4>(Width, Height);

            Int64 data1;
            int height1, cols1, width1, step1;
            RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

            Int64 data2;
            int height2, cols2, width2, step2;
            RoiParam(img2.Ptr, out data2, out height2, out cols2, out width2, out step2);

            Int64 data3;
            int height3, cols3, width3, step3;
            RoiParam(img3.Ptr, out data3, out height3, out cols3, out width3, out step3);

            Int64 data4;
            int height4, cols4, width4, step4;
            RoiParam(res.Ptr, out data4, out height4, out cols4, out width4, out step4);

            TDepth[] row1 = new TDepth[cols1];
            TDepth2[] row2 = new TDepth2[cols1];
            TDepth3[] row3 = new TDepth3[cols1];
            TDepth4[] row4 = new TDepth4[cols1];
            GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);
            GCHandle handle3 = GCHandle.Alloc(row3, GCHandleType.Pinned);
            GCHandle handle4 = GCHandle.Alloc(row4, GCHandleType.Pinned);

            for (int row = 0; row < height1; row++, data1 += step1, data2 += step2, data3 += step3, data4 += step4)
            {
                CvToolbox.Memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                CvToolbox.Memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
                CvToolbox.Memcpy(handle3.AddrOfPinnedObject(), (IntPtr)data3, width3);

                for (int col = 0; col < cols1; row4[col] = converter(row1[col], row2[col], row3[col]), col++) ;

                CvToolbox.Memcpy((IntPtr)data4, handle4.AddrOfPinnedObject(), width4);
            }
            handle1.Free();
            handle2.Free();
            handle3.Free();
            handle4.Free();

            return res;
        }

        /// <summary> Compute the element of the new image based on the elements of the four image</summary>
        /// <typeparam name="TDepth2">The depth type of img2</typeparam>
        /// <typeparam name="TDepth3">The depth type of img3</typeparam>
        /// <typeparam name="TDepth4">The depth type of img4</typeparam>
        /// <typeparam name="TDepth5">The depth type of the result image</typeparam>
        /// <param name="img2">The second image</param>
        /// <param name="img3">The third image</param>
        /// <param name="img4">The fourth image</param>
        /// <param name="converter">The function to be applied to the image pixels</param>
        /// <returns>The result image</returns>
        public Image<TColor, TDepth5> Convert<TDepth2, TDepth3, TDepth4, TDepth5>(Image<TColor, TDepth2> img2, Image<TColor, TDepth3> img3, Image<TColor, TDepth4> img4, Func<TDepth, TDepth2, TDepth3, TDepth4, TDepth5> converter)
           where TDepth2 : new()
           where TDepth3 : new()
           where TDepth4 : new()
           where TDepth5 : new()
        {
            Debug.Assert(Size.Equals(img2.Size) && Size.Equals(img3.Size) && Size.Equals(img4.Size), "Image size do not match");

            Image<TColor, TDepth5> res = new Image<TColor, TDepth5>(Width, Height);

            Int64 data1;
            int height1, cols1, width1, step1;
            RoiParam(Ptr, out data1, out height1, out cols1, out width1, out step1);

            Int64 data2;
            int height2, cols2, width2, step2;
            RoiParam(img2.Ptr, out data2, out height2, out cols2, out width2, out step2);

            Int64 data3;
            int height3, cols3, width3, step3;
            RoiParam(img3.Ptr, out data3, out height3, out cols3, out width3, out step3);

            Int64 data4;
            int height4, cols4, width4, step4;
            RoiParam(img4.Ptr, out data4, out height4, out cols4, out width4, out step4);

            Int64 data5;
            int height5, cols5, width5, step5;
            RoiParam(res.Ptr, out data5, out height5, out cols5, out width5, out step5);

            TDepth[] row1 = new TDepth[cols1];
            TDepth2[] row2 = new TDepth2[cols1];
            TDepth3[] row3 = new TDepth3[cols1];
            TDepth4[] row4 = new TDepth4[cols1];
            TDepth5[] row5 = new TDepth5[cols1];
            GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(row2, GCHandleType.Pinned);
            GCHandle handle3 = GCHandle.Alloc(row3, GCHandleType.Pinned);
            GCHandle handle4 = GCHandle.Alloc(row4, GCHandleType.Pinned);
            GCHandle handle5 = GCHandle.Alloc(row5, GCHandleType.Pinned);

            for (int row = 0; row < height1; row++, data1 += step1, data2 += step2, data3 += step3, data4 += step4, data5 += step5)
            {
                CvToolbox.Memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                CvToolbox.Memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
                CvToolbox.Memcpy(handle3.AddrOfPinnedObject(), (IntPtr)data3, width3);
                CvToolbox.Memcpy(handle4.AddrOfPinnedObject(), (IntPtr)data4, width4);

                for (int col = 0; col < cols1; row5[col] = converter(row1[col], row2[col], row3[col], row4[col]), col++) ;
                CvToolbox.Memcpy((IntPtr)data5, handle5.AddrOfPinnedObject(), width5);
            }
            handle1.Free();
            handle2.Free();
            handle3.Free();
            handle4.Free();
            handle5.Free();

            return res;
        }
#endregion

#region Implment UnmanagedObject
        /// <summary>
        /// Release all unmanaged memory associate with the image
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();

            if (_ptr != IntPtr.Zero)
            {
                if (_imageDataReleaseMode == ImageDataReleaseMode.ReleaseHeaderOnly)
                {
                    CvInvoke.cvReleaseImageHeader(ref _ptr);
                    GC.RemoveMemoryPressure(StructSize.MIplImage);
                }
                else //ImageDataReleaseMode.ReleaseIplImage
                {
                    CvInvoke.cvReleaseImage(ref _ptr);
                }
                Debug.Assert(_ptr == IntPtr.Zero);
            }

            _array = null;
        }
#endregion

#region Operator overload

        /// <summary>
        /// Perform an element wise AND operation on the two images
        /// </summary>
        /// <param name="img1">The first image to AND</param>
        /// <param name="img2">The second image to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Image<TColor, TDepth> operator &(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
        {
            return img1.And(img2);
        }

        /// <summary>
        /// Perform an element wise AND operation using an images and a color
        /// </summary>
        /// <param name="img1">The first image to AND</param>
        /// <param name="val">The color to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Image<TColor, TDepth> operator &(Image<TColor, TDepth> img1, double val)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(val, val, val, val);
            return img1.And(color);
        }

        /// <summary>
        /// Perform an element wise AND operation using an images and a color
        /// </summary>
        /// <param name="img1">The first image to AND</param>
        /// <param name="val">The color to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Image<TColor, TDepth> operator &(double val, Image<TColor, TDepth> img1)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(val, val, val, val);
            return img1.And(color);
        }

        /// <summary>
        /// Perform an element wise AND operation using an images and a color
        /// </summary>
        /// <param name="img1">The first image to AND</param>
        /// <param name="val">The color to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Image<TColor, TDepth> operator &(Image<TColor, TDepth> img1, TColor val)
        {
            return img1.And(val);
        }

        /// <summary>
        /// Perform an element wise AND operation using an images and a color
        /// </summary>
        /// <param name="img1">The first image to AND</param>
        /// <param name="val">The color to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Image<TColor, TDepth> operator &(TColor val, Image<TColor, TDepth> img1)
        {
            return img1.And(val);
        }

        /// <summary> Perform an element wise OR operation with another image and return the result</summary>
        /// <param name="img1">The first image to apply bitwise OR operation</param>
        /// <param name="img2">The second image to apply bitwise OR operation</param>
        /// <returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
        {
            return img1.Or(img2);
        }

        /// <summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        /// <param name="img1">The image to OR</param>
        /// <param name="val"> The color to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, double val)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(val, val, val, val);
            return img1.Or(color);
        }

        /// <summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        /// <param name="img1">The image to OR</param>
        /// <param name="val"> The color to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(double val, Image<TColor, TDepth> img1)
        {
            return img1 | val;
        }

        /// <summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        /// <param name="img1">The image to OR</param>
        /// <param name="val"> The color to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, TColor val)
        {
            return img1.Or(val);
        }

        /// <summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        /// <param name="img1">The image to OR</param>
        /// <param name="val"> The color to OR</param>
        /// <returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(TColor val, Image<TColor, TDepth> img1)
        {
            return img1.Or(val);
        }

        /// <summary>Compute the complement image</summary>
        /// <param name="image">The image to be inverted</param>
        /// <returns>The complement image</returns>
        public static Image<TColor, TDepth> operator ~(Image<TColor, TDepth> image)
        {
            return image.Not();
        }

        /// <summary>
        /// Element wise add <paramref name="img1"/> with <paramref name="img2"/>
        /// </summary>
        /// <param name="img1">The first image to be added</param>
        /// <param name="img2">The second image to be added</param>
        /// <returns>The sum of the two images</returns>
        public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
        {
            return img1.Add(img2);
        }

        /// <summary>
        /// Element wise add <paramref name="img1"/> with <paramref name="val"/>
        /// </summary>
        /// <param name="img1">The image to be added</param>
        /// <param name="val">The value to be added</param>
        /// <returns>The images plus the color</returns>
        public static Image<TColor, TDepth> operator +(double val, Image<TColor, TDepth> img1)
        {
            return img1 + val;
        }

        /// <summary>
        /// Element wise add <paramref name="image"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="image">The image to be added</param>
        /// <param name="value">The value to be added</param>
        /// <returns>The images plus the color</returns>
        public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> image, double value)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(value, value, value, value);
            return image.Add(color);
        }

        /// <summary>
        /// Element wise add <paramref name="image"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="image">The image to be added</param>
        /// <param name="value">The color to be added</param>
        /// <returns>The images plus the color</returns>
        public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> image, TColor value)
        {
            return image.Add(value);
        }

        /// <summary>
        /// Element wise add <paramref name="image"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="image">The image to be added</param>
        /// <param name="value">The color to be added</param>
        /// <returns>The images plus the color</returns>
        public static Image<TColor, TDepth> operator +(TColor value, Image<TColor, TDepth> image)
        {
            return image.Add(value);
        }

        /// <summary>
        /// Element wise subtract another image from the current image
        /// </summary>
        /// <param name="image1">The image to be subtracted</param>
        /// <param name="image2">The second image to be subtracted from <paramref name="image1"/></param>
        /// <returns> The result of element wise subtracting img2 from <paramref name="image1"/> </returns>
        public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> image1, Image<TColor, TDepth> image2)
        {
            return image1.Sub(image2);
        }

        /// <summary>
        /// Element wise subtract another image from the current image
        /// </summary>
        /// <param name="image">The image to be subtracted</param>
        /// <param name="value">The color to be subtracted</param>
        /// <returns> The result of element wise subtracting <paramref name="value"/> from <paramref name="image"/> </returns>
        public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> image, TColor value)
        {
            return image.Sub(value);
        }

        /// <summary>
        /// Element wise subtract another image from the current image
        /// </summary>
        /// <param name="image">The image to be subtracted</param>
        /// <param name="value">The color to be subtracted</param>
        /// <returns> <paramref name="value"/> - <paramref name="image"/> </returns>
        public static Image<TColor, TDepth> operator -(TColor value, Image<TColor, TDepth> image)
        {
            return image.SubR(value);
        }

        /// <summary>
        /// <paramref name="value"/> - <paramref name="image"/>
        /// </summary>
        /// <param name="image">The image to be subtracted</param>
        /// <param name="value">The value to be subtracted</param>
        /// <returns> <paramref name="value"/> - <paramref name="image"/> </returns>
        public static Image<TColor, TDepth> operator -(double value, Image<TColor, TDepth> image)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(value, value, value, value);
            return image.SubR(color);
        }

        /// <summary>
        /// Element wise subtract another image from the current image
        /// </summary>
        /// <param name="image">The image to be subtracted</param>
        /// <param name="value">The value to be subtracted</param>
        /// <returns> <paramref name="image"/> - <paramref name="value"/>   </returns>
        public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> image, double value)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(value, value, value, value);
            return image.Sub(color);
        }

        /// <summary>
        ///  <paramref name="image"/> * <paramref name="scale"/>
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="scale">The multiplication scale</param>
        /// <returns><paramref name="image"/> * <paramref name="scale"/></returns>
        public static Image<TColor, TDepth> operator *(Image<TColor, TDepth> image, double scale)
        {
            return image.Mul(scale);
        }

        /// <summary>
        ///   <paramref name="scale"/>*<paramref name="image"/>
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="scale">The multiplication scale</param>
        /// <returns><paramref name="scale"/>*<paramref name="image"/></returns>
        public static Image<TColor, TDepth> operator *(double scale, Image<TColor, TDepth> image)
        {
            return image.Mul(scale);
        }

        /// <summary>
        /// Perform the convolution with <paramref name="kernel"/> on <paramref name="image"/>
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="kernel">The kernel</param>
        /// <returns>Result of the convolution</returns>
        public static Image<TColor, Single> operator *(Image<TColor, TDepth> image, ConvolutionKernelF kernel)
        {
            return image.Convolution(kernel);
        }

        /// <summary>
        ///  <paramref name="image"/> / <paramref name="scale"/>
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="scale">The division scale</param>
        /// <returns><paramref name="image"/> / <paramref name="scale"/></returns>
        public static Image<TColor, TDepth> operator /(Image<TColor, TDepth> image, double scale)
        {
            return image.Mul(1.0 / scale);
        }

        /// <summary>
        ///   <paramref name="scale"/> / <paramref name="image"/>
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="scale">The scale</param>
        /// <returns><paramref name="scale"/> / <paramref name="image"/></returns>
        public static Image<TColor, TDepth> operator /(double scale, Image<TColor, TDepth> image)
        {
            Image<TColor, TDepth> res = image.CopyBlank();
            using (ScalarArray ia = new ScalarArray(scale))
            {
                CvInvoke.Divide(ia, image, res, 1.0, CvInvoke.GetDepthType(typeof(TDepth)));
            }
            return res;
        }

#endregion

#region Filters
        /// <summary>
        /// Summation over a pixel param1 x param2 neighborhood with subsequent scaling by 1/(param1 x param2)
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">The height of the window</param>
        /// <returns>The result of blur</returns>
        public Image<TColor, TDepth> SmoothBlur(int width, int height)
        {
            return SmoothBlur(width, height, true);
        }

        /// <summary>
        /// Summation over a pixel param1 x param2 neighborhood. If scale is true, the result is subsequent scaled by 1/(param1 x param2)
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">The height of the window</param>
        /// <param name="scale">If true, the result is subsequent scaled by 1/(param1 x param2)</param>
        /// <returns>The result of blur</returns>
        [ExposableMethod(Exposable = true, Category = "Smoothing")]
        public Image<TColor, TDepth> SmoothBlur(int width, int height, bool scale)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.BoxFilter(this, res, CvInvoke.GetDepthType(typeof(TDepth)), new Size(width, height), new Point(-1, -1), scale);
            return res;
        }

        /// <summary>
        /// Finding median of <paramref name="size"/>x<paramref name="size"/> neighborhood 
        /// </summary>
        /// <param name="size">The size (width &amp; height) of the window</param>
        /// <returns>The result of median smooth</returns>
        [ExposableMethod(Exposable = true, Category = "Smoothing")]
        public Image<TColor, TDepth> SmoothMedian(int size)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.MedianBlur(this, res, size);
            return res;
        }

        /// <summary>
        /// Applying bilateral 3x3 filtering
        /// </summary>
        /// <param name="colorSigma">Color sigma</param>
        /// <param name="spaceSigma">Space sigma</param>
        /// <param name="kernelSize">The size of the bilateral kernel</param>
        /// <returns>The result of bilateral smooth</returns>
        [ExposableMethod(Exposable = true, Category = "Smoothing")]
        public Image<TColor, TDepth> SmoothBilateral(int kernelSize, int colorSigma, int spaceSigma)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.BilateralFilter(this, res, kernelSize, colorSigma, spaceSigma);
            return res;
        }

#region Gaussian Smooth
        /// <summary> Perform Gaussian Smoothing in the current image and return the result </summary>
        /// <param name="kernelSize"> The size of the Gaussian kernel (<paramref name="kernelSize"/> x <paramref name="kernelSize"/>)</param>
        /// <returns> The smoothed image</returns>
        public Image<TColor, TDepth> SmoothGaussian(int kernelSize)
        {
            return SmoothGaussian(kernelSize, kernelSize, 0, 0);
        }

        /// <summary> Perform Gaussian Smoothing in the current image and return the result </summary>
        /// <param name="kernelWidth"> The width of the Gaussian kernel</param>
        /// <param name="kernelHeight"> The height of the Gaussian kernel</param>
        /// <param name="sigma1"> The standard deviation of the Gaussian kernel in the horizontal dimension</param>
        /// <param name="sigma2"> The standard deviation of the Gaussian kernel in the vertical dimension</param>
        /// <returns> The smoothed image</returns>
        [ExposableMethod(Exposable = true, Category = "Smoothing")]
        public Image<TColor, TDepth> SmoothGaussian(int kernelWidth, int kernelHeight, double sigma1, double sigma2)
        {
            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.GaussianBlur(this, res, new Size(kernelWidth, kernelHeight), sigma1, sigma2);
            return res;
        }

        /// <summary> Perform Gaussian Smoothing inplace for the current image </summary>
        /// <param name="kernelSize"> The size of the Gaussian kernel (<paramref name="kernelSize"/> x <paramref name="kernelSize"/>)</param>
        public void _SmoothGaussian(int kernelSize)
        {
            _SmoothGaussian(kernelSize, kernelSize, 0, 0);
        }

        /// <summary> Perform Gaussian Smoothing inplace for the current image </summary>
        /// <param name="kernelWidth"> The width of the Gaussian kernel</param>
        /// <param name="kernelHeight"> The height of the Gaussian kernel</param>
        /// <param name="sigma1"> The standard deviation of the Gaussian kernel in the horizontal dimension</param>
        /// <param name="sigma2"> The standard deviation of the Gaussian kernel in the vertical dimension</param>
        public void _SmoothGaussian(int kernelWidth, int kernelHeight, double sigma1, double sigma2)
        {
            CvInvoke.GaussianBlur(this, this, new Size(kernelWidth, kernelHeight), sigma1, sigma2);
        }

        /// <summary> 
        /// Performs a convolution using the specific <paramref name="kernel"/> 
        /// </summary>
        /// <param name="kernel">The convolution kernel</param>
        /// <param name="delta">The optional value added to the filtered pixels before storing them in dst</param>
        /// <param name="borderType">The pixel extrapolation method.</param>
        /// <returns>The result of the convolution</returns>
        public Image<TColor, Single> Convolution(ConvolutionKernelF kernel, double delta = 0,
            Emgu.CV.CvEnum.BorderType borderType = CvEnum.BorderType.Default)
        {
            Image<TColor, Single> floatImage =
               (typeof(TDepth) == typeof(Single)) ?
               this as Image<TColor, Single>
               : Convert<TColor, Single>();
            try
            {
                Size s = Size;
                Image<TColor, Single> res = new Image<TColor, Single>(s);
                int numberOfChannels = NumberOfChannels;
                if (numberOfChannels == 1)
                    CvInvoke.Filter2D(floatImage, res, kernel, kernel.Center, delta, borderType);
                else
                {
                    using (Mat m1 = new Mat(s, DepthType.Cv32F, 1))
                    using (Mat m2 = new Mat(s, DepthType.Cv32F, 1))
                    {
                        for (int i = 0; i < numberOfChannels; i++)
                        {
                            CvInvoke.ExtractChannel(floatImage, m1, i);
                            CvInvoke.Filter2D(m1, m2, kernel, kernel.Center, delta, borderType);
                            CvInvoke.InsertChannel(m2, res, i);
                        }
                    }
                }
                return res;
            }
            finally
            {

                if (!object.ReferenceEquals(floatImage, this))
                    floatImage.Dispose();
            }

        }

        /// <summary>
        /// Calculates integral images for the source image
        /// </summary>
        /// <returns>The integral image</returns>
        public Image<TColor, double> Integral()
        {
            Image<TColor, double> sum = new Image<TColor, double>(Width + 1, Height + 1);
            CvInvoke.Integral(this, sum, null, null, CvEnum.DepthType.Cv64F);
            return sum;
        }

        /// <summary>
        /// Calculates integral images for the source image
        /// </summary>
        /// <param name="sum">The integral image</param>
        /// <param name="squareSum">The integral image for squared pixel values</param>
        /// <returns>The integral image</returns>
        public void Integral(out Image<TColor, double> sum, out Image<TColor, double> squareSum)
        {
            sum = new Image<TColor, double>(Width + 1, Height + 1);
            squareSum = new Image<TColor, double>(Width + 1, Height + 1);
            CvInvoke.Integral(this, sum, squareSum, null, CvEnum.DepthType.Cv64F);
        }

        /// <summary>
        /// Calculates one or more integral images for the source image
        /// </summary>
        /// <param name="sum">The integral image</param>
        /// <param name="squareSum">The integral image for squared pixel values</param>
        /// <param name="titledSum">The integral for the image rotated by 45 degrees</param>
        public void Integral(out Image<TColor, double> sum, out Image<TColor, double> squareSum, out Image<TColor, double> titledSum)
        {
            sum = new Image<TColor, double>(Width + 1, Height + 1);
            squareSum = new Image<TColor, double>(Width + 1, Height + 1);
            titledSum = new Image<TColor, double>(Width + 1, Height + 1);
            CvInvoke.Integral(this, sum, squareSum, titledSum, CvEnum.DepthType.Cv64F);
        }
#endregion

#region Threshold methods
        /// <summary>
        /// Transforms grayscale image to binary image. 
        /// Threshold calculated individually for each pixel. 
        /// For the method CV_ADAPTIVE_THRESH_MEAN_C it is a mean of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel
        /// neighborhood, subtracted by param1. 
        /// For the method CV_ADAPTIVE_THRESH_GAUSSIAN_C it is a weighted sum (gaussian) of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel neighborhood, subtracted by param1.
        /// </summary>
        /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
        /// <param name="adaptiveType">Adaptive_method </param>
        /// <param name="thresholdType">Thresholding type. must be one of CV_THRESH_BINARY, CV_THRESH_BINARY_INV  </param>
        /// <param name="blockSize">The size of a pixel neighborhood that is used to calculate a threshold value for the pixel: 3, 5, 7, ... </param>
        /// <param name="param1">Constant subtracted from mean or weighted mean. It may be negative. </param>
        /// <returns>The result of the adaptive threshold</returns>
        [ExposableMethod(Exposable = true, Category = "Threshold")]
        public Image<TColor, TDepth> ThresholdAdaptive(
           TColor maxValue,
           CvEnum.AdaptiveThresholdType adaptiveType,
           CvEnum.ThresholdType thresholdType,
           int blockSize,
           TColor param1)
        {
            double[] max = maxValue.MCvScalar.ToArray();
            double[] p1 = param1.MCvScalar.ToArray();
            Image<TColor, TDepth> result = CopyBlank();
            ForEachDuplicateChannel<TDepth>(
               delegate (IInputArray src, IOutputArray dst, int channel)
               {
                   CvInvoke.AdaptiveThreshold(src, dst, max[channel], adaptiveType, thresholdType, blockSize, p1[channel]);
               },
               result);
            return result;
        }

        /// <summary> 
        /// The base threshold method shared by public threshold functions 
        /// </summary>
        private void ThresholdBase(Image<TColor, TDepth> dest, TColor threshold, TColor maxValue, CvEnum.ThresholdType threshType)
        {
            double[] t = threshold.MCvScalar.ToArray();
            double[] m = maxValue.MCvScalar.ToArray();
            ForEachDuplicateChannel<TDepth>(
               delegate (IInputArray src, IOutputArray dst, int channel)
               {
                   CvInvoke.Threshold(src, dst, t[channel], m[channel], threshType);
               },
               dest);
        }

        /// <summary> Threshold the image such that: dst(x,y) = src(x,y), if src(x,y)&gt;threshold;  0, otherwise </summary>
        /// <param name="threshold">The threshold value</param>
        /// <returns> dst(x,y) = src(x,y), if src(x,y)&gt;threshold;  0, otherwise </returns>
        public Image<TColor, TDepth> ThresholdToZero(TColor threshold)
        {
            Image<TColor, TDepth> res = CopyBlank();
            ThresholdBase(res, threshold, new TColor(), CvEnum.ThresholdType.ToZero);
            return res;
        }

        /// <summary> 
        /// Threshold the image such that: dst(x,y) = 0, if src(x,y)&gt;threshold;  src(x,y), otherwise 
        /// </summary>
        /// <param name="threshold">The threshold value</param>
        /// <returns>The image such that: dst(x,y) = 0, if src(x,y)&gt;threshold;  src(x,y), otherwise</returns>
        public Image<TColor, TDepth> ThresholdToZeroInv(TColor threshold)
        {
            Image<TColor, TDepth> res = CopyBlank();
            ThresholdBase(res, threshold, new TColor(), CvEnum.ThresholdType.ToZeroInv);
            return res;
        }

        /// <summary>
        /// Threshold the image such that: dst(x,y) = threshold, if src(x,y)&gt;threshold; src(x,y), otherwise 
        /// </summary>
        /// <param name="threshold">The threshold value</param>
        /// <returns>The image such that: dst(x,y) = threshold, if src(x,y)&gt;threshold; src(x,y), otherwise</returns>
        public Image<TColor, TDepth> ThresholdTrunc(TColor threshold)
        {
            Image<TColor, TDepth> res = CopyBlank();
            ThresholdBase(res, threshold, new TColor(), CvEnum.ThresholdType.Trunc);
            return res;
        }

        /// <summary> 
        /// Threshold the image such that: dst(x,y) = max_value, if src(x,y)&gt;threshold; 0, otherwise 
        /// </summary>
        /// <param name="threshold">The threshold value</param>
        /// <param name="maxValue">The maximum value of the pixel on the result</param>
        /// <returns>The image such that: dst(x,y) = max_value, if src(x,y)&gt;threshold; 0, otherwise </returns>
        public Image<TColor, TDepth> ThresholdBinary(TColor threshold, TColor maxValue)
        {
            Image<TColor, TDepth> res = CopyBlank();
            ThresholdBase(res, threshold, maxValue, CvEnum.ThresholdType.Binary);
            return res;
        }

        /// <summary> Threshold the image such that: dst(x,y) = 0, if src(x,y)&gt;threshold;  max_value, otherwise </summary>
        /// <param name="threshold">The threshold value</param>
        /// <param name="maxValue">The maximum value of the pixel on the result</param>
        /// <returns>The image such that: dst(x,y) = 0, if src(x,y)&gt;threshold;  max_value, otherwise</returns>
        public Image<TColor, TDepth> ThresholdBinaryInv(TColor threshold, TColor maxValue)
        {
            Image<TColor, TDepth> res = CopyBlank();
            ThresholdBase(res, threshold, maxValue, CvEnum.ThresholdType.BinaryInv);
            return res;
        }

        /// <summary> Threshold the image inplace such that: dst(x,y) = src(x,y), if src(x,y)&gt;threshold;  0, otherwise </summary>
        /// <param name="threshold">The threshold value</param>
        [ExposableMethod(Exposable = true, Category = "Threshold")]
        public void _ThresholdToZero(TColor threshold)
        {
            ThresholdBase(this, threshold, new TColor(), CvEnum.ThresholdType.ToZero);
        }

        /// <summary> Threshold the image inplace such that: dst(x,y) = 0, if src(x,y)&gt;threshold;  src(x,y), otherwise </summary>
        /// <param name="threshold">The threshold value</param>
        [ExposableMethod(Exposable = true, Category = "Threshold")]
        public void _ThresholdToZeroInv(TColor threshold)
        {
            ThresholdBase(this, threshold, new TColor(), CvEnum.ThresholdType.ToZeroInv);
        }

        /// <summary> Threshold the image inplace such that: dst(x,y) = threshold, if src(x,y)&gt;threshold; src(x,y), otherwise </summary>
        /// <param name="threshold">The threshold value</param>
        [ExposableMethod(Exposable = true, Category = "Threshold")]
        public void _ThresholdTrunc(TColor threshold)
        {
            ThresholdBase(this, threshold, new TColor(), CvEnum.ThresholdType.Trunc);
        }

        /// <summary> Threshold the image inplace such that: dst(x,y) = max_value, if src(x,y)&gt;threshold; 0, otherwise </summary>
        /// <param name="threshold">The threshold value</param>
        /// <param name="maxValue">The maximum value of the pixel on the result</param>
        [ExposableMethod(Exposable = true, Category = "Threshold")]
        public void _ThresholdBinary(TColor threshold, TColor maxValue)
        {
            ThresholdBase(this, threshold, maxValue, CvEnum.ThresholdType.Binary);
        }

        /// <summary> Threshold the image inplace such that: dst(x,y) = 0, if src(x,y)&gt;threshold;  max_value, otherwise </summary>
        /// <param name="threshold">The threshold value</param>
        /// <param name="maxValue">The maximum value of the pixel on the result</param>
        [ExposableMethod(Exposable = true, Category = "Threshold")]
        public void _ThresholdBinaryInv(TColor threshold, TColor maxValue)
        {
            ThresholdBase(this, threshold, maxValue, CvEnum.ThresholdType.BinaryInv);
        }
#endregion
#endregion

#region Statistic
        /// <summary>
        /// Calculates the average value and standard deviation of array elements, independently for each channel
        /// </summary>
        /// <param name="average">The avg color</param>
        /// <param name="sdv">The standard deviation for each channel</param>
        /// <param name="mask">The operation mask</param>
        public void AvgSdv(out TColor average, out MCvScalar sdv, Image<Gray, Byte> mask)
        {
            average = new TColor();
            MCvScalar avgScalar = new MCvScalar();
            sdv = new MCvScalar();

            CvInvoke.MeanStdDev(this, ref avgScalar, ref sdv, mask);
            average.MCvScalar = avgScalar;
        }

        /// <summary>
        /// Calculates the average value and standard deviation of array elements, independently for each channel
        /// </summary>
        /// <param name="avg">The avg color</param>
        /// <param name="sdv">The standard deviation for each channel</param>
        public void AvgSdv(out TColor avg, out MCvScalar sdv)
        {
            AvgSdv(out avg, out sdv, null);
        }

        /// <summary>
        /// Count the non Zero elements for each channel
        /// </summary>
        /// <returns>Count the non Zero elements for each channel</returns>
        public int[] CountNonzero()
        {
            using (Mat m = CvInvoke.CvArrToMat(this))
            {
                if (NumberOfChannels == 1)
                    return new int[] { CvInvoke.CountNonZero(m) };
                else
                {
                    int[] result = new int[NumberOfChannels];
                    using (Mat tmp = new Mat())
                        for (int i = 0; i < result.Length; i++)
                        {
                            CvInvoke.ExtractChannel(m, tmp, i);
                            result[i] = CvInvoke.CountNonZero(tmp);
                        }
                    return result;
                }
            }
        }

        /// <summary>
        /// Returns the min / max location and values for the image
        /// </summary>
        /// <param name="maxLocations">The maximum locations for each channel </param>
        /// <param name="maxValues">The maximum values for each channel</param>
        /// <param name="minLocations">The minimum locations for each channel</param>
        /// <param name="minValues">The minimum values for each channel</param>
        public void MinMax(out double[] minValues, out double[] maxValues, out Point[] minLocations, out Point[] maxLocations)
        {
            Mat.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
        }
#endregion

#region Image Flipping

        /// <summary> Return a flipped copy of the current image</summary>
        /// <param name="flipType">The type of the flipping</param>
        /// <returns> The flipped copy of <i>this</i> image </returns>
        public Image<TColor, TDepth> Flip(CvEnum.FlipType flipType)
        {
            if (flipType == Emgu.CV.CvEnum.FlipType.None) return Copy();

            Image<TColor, TDepth> res = CopyBlank();
            CvInvoke.Flip(this, res, flipType);
            return res;
        }

        /// <summary> Inplace flip the image</summary>
        /// <param name="flipType">The type of the flipping</param>
        /// <returns> The flipped copy of <i>this</i> image </returns>
        [ExposableMethod(Exposable = true, Category = "Transform")]
        public void _Flip(CvEnum.FlipType flipType)
        {
            if (flipType != Emgu.CV.CvEnum.FlipType.None)
            {
                CvInvoke.Flip(
                   this,
                   this,
                   flipType);
            }
        }
#endregion

#region various

        /// <summary>
        /// Concate the current image with another image vertically.
        /// </summary>
        /// <param name="otherImage">The other image to concate</param>
        /// <returns>A new image that is the vertical concatening of this image and <paramref name="otherImage"/></returns>
        public Image<TColor, TDepth> ConcateVertical(Image<TColor, TDepth> otherImage)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Math.Max(Width, otherImage.Width), Height + otherImage.Height);
            res.ROI = ROI;
            CvInvoke.cvCopy(Ptr, res.Ptr, IntPtr.Zero);
            Rectangle rect = otherImage.ROI;
            rect.Y += Height;
            res.ROI = rect;
            CvInvoke.cvCopy(otherImage.Ptr, res.Ptr, IntPtr.Zero);
            res.ROI = Rectangle.Empty;
            return res;
        }

        /// <summary>
        /// Concate the current image with another image horizontally. 
        /// </summary>
        /// <param name="otherImage">The other image to concate</param>
        /// <returns>A new image that is the horizontal concatening of this image and <paramref name="otherImage"/></returns>
        public Image<TColor, TDepth> ConcateHorizontal(Image<TColor, TDepth> otherImage)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width + otherImage.Width, Math.Max(Height, otherImage.Height));
            res.ROI = ROI;
            CvInvoke.cvCopy(Ptr, res.Ptr, IntPtr.Zero);
            Rectangle rect = otherImage.ROI;
            rect.X += Width;
            res.ROI = rect;
            CvInvoke.cvCopy(otherImage.Ptr, res.Ptr, IntPtr.Zero);
            res.ROI = Rectangle.Empty;
            return res;
        }


        /// <summary>
        /// Calculates spatial and central moments up to the third order and writes them to moments. The moments may be used then to calculate gravity center of the shape, its area, main axises and various shape characteristics including 7 Hu invariants.
        /// </summary>
        /// <param name="binary">If the flag is true, all the zero pixel values are treated as zeroes, all the others are treated as 1's</param>
        /// <returns>spatial and central moments up to the third order</returns>
        public Emgu.CV.Moments GetMoments(bool binary)
        {
            return CvInvoke.Moments(this, binary);
        }

        /// <summary>
        /// Gamma corrects this image inplace. The image must have a depth type of Byte.
        /// </summary>
        /// <param name="gamma">The gamma value</param>
        [ExposableMethod(Exposable = true)]
        public void _GammaCorrect(double gamma)
        {
            Image<TColor, Byte> img = this as Image<TColor, Byte>;
            if (img == null)
                throw new NotImplementedException("Gamma correction only implemented for Image of Byte as Depth");

            Byte[,] gammaLUT = new Byte[256, 1];
            for (int i = 0; i < 256; i++)
                gammaLUT[i, 0] = (Byte)(Math.Pow(i / 255.0, gamma) * 255.0);

            using (Matrix<Byte> lut = new Matrix<byte>(gammaLUT))
            {
                Matrix<Byte> lookupTable;
                if (lut.NumberOfChannels == 1)
                    lookupTable = lut;
                else
                {
                    lookupTable = new Matrix<byte>(lut.Rows, lut.Cols, NumberOfChannels);
                    using (VectorOfMat mv = new VectorOfMat())
                    {
                        for (int i = 0; i < NumberOfChannels; i++)
                            mv.Push(lut.Mat);
                        CvInvoke.Merge(mv, lookupTable);
                    }
                    /*
                    CvInvoke.cvMerge(
                       lut.Ptr,
                       NumberOfChannels > 1 ? lut.Ptr : IntPtr.Zero,
                       NumberOfChannels > 2 ? lut.Ptr : IntPtr.Zero,
                       NumberOfChannels > 3 ? lut.Ptr : IntPtr.Zero,
                       lookupTable.Ptr);
                     */
                }

                CvInvoke.LUT(this, lookupTable, this);

                if (!object.ReferenceEquals(lut, lookupTable))
                    lookupTable.Dispose();
            }
        }

        /// <summary> 
        /// Split current Image into an array of gray scale images where each element 
        /// in the array represent a single color channel of the original image
        /// </summary>
        /// <returns> 
        /// An array of gray scale images where each element  
        /// in the array represent a single color channel of the original image 
        /// </returns>
        public Image<Gray, TDepth>[] Split()
        {
            //If single channel, return a copy
            if (NumberOfChannels == 1) return new Image<Gray, TDepth>[] { Copy() as Image<Gray, TDepth> };

            //handle multiple channels
            Image<Gray, TDepth>[] res = new Image<Gray, TDepth>[NumberOfChannels];
            using (Util.VectorOfMat vm = new VectorOfMat())
            {
                Size size = Size;
                for (int i = 0; i < NumberOfChannels; i++)
                {
                    res[i] = new Image<Gray, TDepth>(size);
                    vm.Push(res[i].Mat);
                }

                CvInvoke.Split(this, vm);
            }
            return res;
        }

        /// <summary>
        /// Save this image to the specific file. 
        /// </summary>
        /// <param name="fileName">The name of the file to be saved to</param>
        /// <remarks>The image format is chosen depending on the filename extension, see cvLoadImage. Only 8-bit single-channel or 3-channel (with 'BGR' channel order) images can be saved using this function. If the format, depth or channel order is different, use cvCvtScale and cvCvtColor to convert it before saving, or use universal cvSave to save the image to XML or YAML format.</remarks>
        public override void Save(String fileName)
        {

            if (NumberOfChannels == 3 && typeof(TColor) != typeof(Bgr))
            {
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(this, tmp, typeof(TColor), typeof(Bgr));
                    tmp.Save(fileName);
                }
            }
            else if (NumberOfChannels == 4 && typeof(TColor) != typeof(Bgra))
            {
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(this, tmp, typeof(TColor), typeof(Bgra));
                    tmp.Save(fileName);
                }
            }
            else
            {
                Mat.Save(fileName);
            }
        }

        /// <summary>
        /// The algorithm inplace normalizes brightness and increases contrast of the image.
        /// For color images, a HSV representation of the image is first obtained and the V (value) channel is histogram normalized
        /// </summary>
        [ExposableMethod(Exposable = true)]
        public void _EqualizeHist()
        {
            if (NumberOfChannels == 1) //Gray scale image
            {
                CvInvoke.EqualizeHist(this, this);
            }
            else //Color image
            {
                //Get an hsv representation of this image
                Image<Hsv, TDepth> hsv = this as Image<Hsv, TDepth> ?? Convert<Hsv, TDepth>();

                //equalize the V (value) channel
                using (Image<Gray, TDepth> v = new Image<Gray, TDepth>(Size))
                {
                    CvInvoke.MixChannels(hsv, v, new int[] { 1, 0 });
                    //CvInvoke.cvSetImageCOI(hsv.Ptr, 3);
                    //CvInvoke.cvCopy(hsv.Ptr, v.Ptr, IntPtr.Zero);
                    v._EqualizeHist();
                    CvInvoke.MixChannels(v, hsv, new int[] { 0, 1 });
                    //CvInvoke.cvCopy(v.Ptr, hsv.Ptr, IntPtr.Zero);
                    //CvInvoke.cvSetImageCOI(hsv.Ptr, 0);
                }

                if (!Object.ReferenceEquals(this, hsv))
                {
                    ConvertFrom(hsv);
                    hsv.Dispose();
                }
            }
        }
#endregion

        /// <summary>
        /// This function load the image data from Mat
        /// </summary>
        /// <param name="mat">The Mat</param>
        private void LoadImageFromMat(Mat mat)
        {
            Size size = mat.Size;

            //Allocate data in managed memory
            AllocateData(size.Height, size.Width, NumberOfChannels);
            switch (mat.NumberOfChannels)
            {
                case 1:
                    //Grayscale image;
                    switch (mat.Depth)
                    {
                        case CvEnum.DepthType.Cv8U:
                            using (Image<Gray, Byte> tmp = new Image<Gray, byte>(size.Width, size.Height, mat.Step, mat.DataPointer))
                                ConvertFrom(tmp);
                            break;
                        case CvEnum.DepthType.Cv16U:
                            using (Image<Gray, UInt16> tmp = new Image<Gray, ushort>(size.Width, size.Height, mat.Step, mat.DataPointer))
                                ConvertFrom(tmp);
                            break;
                        case CvEnum.DepthType.Cv32F:
                            using (Image<Gray, float> tmp = new Image<Gray, float>(size.Width, size.Height, mat.Step, mat.DataPointer))
                                ConvertFrom(tmp);
                            break;
                        case CvEnum.DepthType.Cv64F:
                            using (Image<Gray, double> tmp = new Image<Gray, double>(size.Width, size.Height, mat.Step, mat.DataPointer))
                                ConvertFrom(tmp);
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mat.Depth, mat.NumberOfChannels));
                    }
                    break;
                case 3:
                    //BGR image
                    switch (mat.Depth)
                    {
                        case CvEnum.DepthType.Cv8U:
                            using (Image<Bgr, Byte> tmp = new Image<Bgr, byte>(size.Width, size.Height, mat.Step, mat.DataPointer))
                                ConvertFrom(tmp);
                            break;
                        case CvEnum.DepthType.Cv16U:
                            using (Image<Bgr, UInt16> tmp = new Image<Bgr, ushort>(size.Width, size.Height, mat.Step, mat.DataPointer))
                                ConvertFrom(tmp);
                            break;
                        case CvEnum.DepthType.Cv32F:
                            using (Image<Bgr, float> tmp = new Image<Bgr, float>(size.Width, size.Height, mat.Step, mat.DataPointer))
                                ConvertFrom(tmp);
                            break;
                        case CvEnum.DepthType.Cv64F:
                            using (Image<Bgr, double> tmp = new Image<Bgr, double>(size.Width, size.Height, mat.Step, mat.DataPointer))
                                ConvertFrom(tmp);
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mat.Depth, mat.NumberOfChannels));
                    }
                    break;
                default:
                    throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mat.Depth, mat.NumberOfChannels));
            }
        }

        /// <summary>
        /// This function load the image data from the iplImage pointer
        /// </summary>
        /// <param name="iplImage">The pointer to the iplImage</param>
        private void LoadImageFromIplImagePtr(IntPtr iplImage)
        {
            MIplImage mptr = (MIplImage)Marshal.PtrToStructure(iplImage, typeof(MIplImage));
            Size size = new Size(mptr.Width, mptr.Height);

            //Allocate data in managed memory
            AllocateData(size.Height, size.Width, NumberOfChannels);

            if (mptr.NChannels == 1)
            {  //Grayscale image;
                switch (mptr.Depth)
                {
                    case CvEnum.IplDepth.IplDepth_8U:
                        using (Image<Gray, Byte> tmp = new Image<Gray, byte>(mptr.Width, mptr.Height, mptr.WidthStep, mptr.ImageData))
                            ConvertFrom(tmp);
                        break;
                    case CvEnum.IplDepth.IplDepth16U:
                        using (Image<Gray, UInt16> tmp = new Image<Gray, ushort>(mptr.Width, mptr.Height, mptr.WidthStep, mptr.ImageData))
                            ConvertFrom(tmp);
                        break;
                    case CvEnum.IplDepth.IplDepth32F:
                        using (Image<Gray, float> tmp = new Image<Gray, float>(mptr.Width, mptr.Height, mptr.WidthStep, mptr.ImageData))
                            ConvertFrom(tmp);
                        break;
                    case CvEnum.IplDepth.IplDepth64F:
                        using (Image<Gray, double> tmp = new Image<Gray, double>(mptr.Width, mptr.Height, mptr.WidthStep, mptr.ImageData))
                            ConvertFrom(tmp);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mptr.Depth, mptr.NChannels));
                }
            }
            else if (mptr.NChannels == 3)
            {  //BGR image
                switch (mptr.Depth)
                {
                    case CvEnum.IplDepth.IplDepth_8U:
                        using (Image<Bgr, Byte> tmp = new Image<Bgr, byte>(mptr.Width, mptr.Height, mptr.WidthStep, mptr.ImageData))
                            ConvertFrom(tmp);
                        break;
                    case CvEnum.IplDepth.IplDepth16U:
                        using (Image<Bgr, UInt16> tmp = new Image<Bgr, ushort>(mptr.Width, mptr.Height, mptr.WidthStep, mptr.ImageData))
                            ConvertFrom(tmp);
                        break;
                    case CvEnum.IplDepth.IplDepth32F:
                        using (Image<Bgr, float> tmp = new Image<Bgr, float>(mptr.Width, mptr.Height, mptr.WidthStep, mptr.ImageData))
                            ConvertFrom(tmp);
                        break;
                    case CvEnum.IplDepth.IplDepth64F:
                        using (Image<Bgr, double> tmp = new Image<Bgr, double>(mptr.Width, mptr.Height, mptr.WidthStep, mptr.ImageData))
                            ConvertFrom(tmp);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mptr.Depth, mptr.NChannels));
                }
            }
            else
            {
                throw new NotImplementedException(String.Format("Loading of {0}, {1} channel image is not implemented.", mptr.Depth, mptr.NChannels));
            }
        }

        /// <summary>
        /// Get the managed image from an unmanaged IplImagePointer
        /// </summary>
        /// <param name="iplImage">The pointer to the iplImage</param>
        /// <returns>The managed image from the iplImage pointer</returns>
        public static Image<TColor, TDepth> FromIplImagePtr(IntPtr iplImage)
        {
            Image<TColor, TDepth> result = new Image<TColor, TDepth>();
            result.LoadImageFromIplImagePtr(iplImage);
            return result;
        }

        /// <summary>
        /// Get the jpeg representation of the image
        /// </summary>
        /// <param name="quality">The jpeg quality</param>
        /// <returns>An byte array that contains the image as jpeg data</returns>
        public byte[] ToJpegData(int quality = 95)
        {
            using (VectorOfByte buf = new VectorOfByte())
            {
                CvInvoke.Imencode(".jpg", this, buf, new KeyValuePair<ImwriteFlags, int>(ImwriteFlags.JpegQuality, quality));
                return buf.ToArray();
            }
        }

        /// <summary> 
        /// Get the size of the array
        /// </summary>
        public override System.Drawing.Size Size
        {
            get
            {
                MIplImage iplImage = this.MIplImage;
                if (iplImage.Roi == IntPtr.Zero)
                    return new Size(iplImage.Width, iplImage.Height);
                else
                    return ROI.Size;
                //return CvInvoke.cvGetSize(_ptr);
            }
        }
    }

    /// <summary>
    /// Constants used by the image class
    /// </summary>
    internal static class ImageConstants
    {
        /// <summary>
        /// Offset of roi
        /// </summary>
        public static readonly int RoiOffset = (int)Marshal.OffsetOf(typeof(MIplImage), "Roi");
    }

    /// <summary>
    /// Image data release mode
    /// </summary>
    internal enum ImageDataReleaseMode
    {
        /// <summary>
        /// Release just the header
        /// </summary>
        ReleaseHeaderOnly,

        /// <summary>
        /// Release the IplImage
        /// </summary>
        ReleaseIplImage
    }
}


