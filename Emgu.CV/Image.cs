using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;

namespace Emgu.CV
{
    /// <summary>
    /// A wrapper for IplImage
    /// </summary>
    /// <typeparam name="TColor">Color type of this image</typeparam>
    /// <typeparam name="TDepth">Depth of this image (either Byte, Single or Double)</typeparam>
    [Serializable]
    public class Image<TColor, TDepth> : CvArray<TDepth>, IImage, IEquatable<Image<TColor, TDepth>> where TColor : ColorType, new()
    {
        private TDepth[, ,] _array;

        #region constructors

        ///<summary>
        ///Create an empty Image
        ///</summary>
        protected Image()
        {
        }

        /// <summary>
        /// Create image from the specific multi-dimensional data, where the 1st dimesion is # of rows (height), the 2nd dimension is # cols (cols) and the 3rd dimension is the channel
        /// </summary>
        /// <param name="data">The multi-dimensional data where the 1st dimesion is # of rows (height), the 2nd dimension is # cols (cols) and the 3rd dimension is the channel </param>
        public Image(TDepth[, ,] data)
        {
            Data = data;
        }

        /// <summary>
        /// Read image from a file
        /// </summary>
        /// <param name="fileName">the name of the file that contains the image</param>
        public Image(String fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            String[] cvFormats = new string[] { ".jpg", ".jpeg", ".jpe", ".png", ".bmp", ".dib", ".pbm", ".pgm", ".ppm", ".sr", ".ras", ".tiff", ".tif", ".exr", ".jp2" };
            if (System.Array.Exists(cvFormats, delegate(String s) { return s.Equals(fi.Extension.ToLower()); }))
            {   //if the file can be imported from Open CV

                IntPtr ptr;
                MIplImage mptr;

                #region read the image into ptr ( color type - C, depth - Byte )
                if (typeof(TColor) == typeof(Gray)) //color type is gray
                {
                    ptr = CvInvoke.cvLoadImage(fileName, Emgu.CV.CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_GRAYSCALE);
                    mptr = (MIplImage)Marshal.PtrToStructure(ptr, typeof(MIplImage));
                }
                else //color type is not gray
                {
                    ptr = CvInvoke.cvLoadImage(fileName, CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_COLOR);
                    mptr = (MIplImage)Marshal.PtrToStructure(ptr, typeof(MIplImage));

                    if (typeof(TColor) != typeof(Bgr)) //color type is not Bgr
                    {
                        IntPtr tmp1 = CvInvoke.cvCreateImage(
                            new MCvSize(mptr.width, mptr.height),
                            (CvEnum.IPL_DEPTH)mptr.depth,
                            3);
                        CvInvoke.cvCvtColor(ptr, tmp1, GetColorCvtCode(typeof(TColor), typeof(Bgr)));

                        CvInvoke.cvReleaseImage(ref ptr);
                        ptr = tmp1;
                        mptr = (MIplImage)Marshal.PtrToStructure(ptr, typeof(MIplImage));
                    }
                }
                #endregion

                if (typeof(TDepth) != typeof(Byte)) //depth is not Byte
                {
                    IntPtr tmp1 = CvInvoke.cvCreateImage(
                        new MCvSize(mptr.width, mptr.height),
                        CvDepth,
                        new TColor().Dimension);
                    CvInvoke.cvConvertScale(ptr, tmp1, 1.0, 0.0);
                    CvInvoke.cvReleaseImage(ref ptr);
                    ptr = tmp1;
                    mptr = (MIplImage)Marshal.PtrToStructure(ptr, typeof(MIplImage));
                }

                #region use managed memory instead of unmanaged
                AllocateData(mptr.height, mptr.width);
                //The above line of code might change the widthStep, therefore a re-marshal is necessary
                mptr = (MIplImage)Marshal.PtrToStructure(ptr, typeof(MIplImage));

                //TODO: fix the following to handle the case when input image has non 4-align byte in a row
                Emgu.Utils.memcpy(_dataHandle.AddrOfPinnedObject(), mptr.imageData, mptr.widthStep * mptr.height);
                #endregion
            }
            else
            {   //if the file format cannot be recognized by OpenCV 
                String[] bmpFormats = new string[] { ".gif", ".exig" };
                if (System.Array.Exists(bmpFormats, delegate(String s) { return s.Equals(fi.Extension.ToLower()); }))
                {
                    Bitmap = new Bitmap(fi.FullName);
                }
                else
                    throw new FileLoadException(String.Format("Unable to load file of type {0}", fi.Extension));
            }
        }

        /// <summary>
        /// Obtain the image from the specific Bitmap
        /// </summary>
        /// <param name="bmp">The bitmap which will be converted to the image</param>
        public Image(Bitmap bmp)
        {
            Bitmap = bmp;
        }

        ///<summary>
        ///Create a blank Image of the specified width, height and color.
        ///</summary>
        ///<param name="width">The width of the image</param>
        ///<param name="height">The height of the image</param>
        ///<param name="value">The initial color of the image</param>
        public Image(int width, int height, TColor value)
            : this(width, height)
        {
            SetValue(value);
        }

        ///<summary>
        ///Create a blank Image of the specified width and height. 
        ///</summary>
        ///<param name="width">The width of the image</param>
        ///<param name="height">The height of the image</param>
        public Image(int width, int height)
        {
            AllocateData(height, width);
        }

        /// <summary>
        /// Get or Set the data for this matrix
        /// </summary>
        /// <remarks> 
        /// The Get function has O(1) complexity. 
        /// If the image contains Byte and width is not a multiple of 4. The second dimension of the array might be larger than the Width of this image.  
        /// This is necessary since the length of a row need to be 4 align for OpenCV optimization. 
        /// The Set function always make a copy of the specific value. If the image contains Byte and width is not a multiple of 4. The second dimension of the array created might be larger than the Width of this image.  
        /// </remarks>
        public TDepth[, ,] Data
        {
            get
            {
                return _array;
            }
            set
            {
                Debug.Assert(value != null, "The Array cannot be null");
                AllocateData(value.GetLength(0), value.GetLength(1));
                int rows = value.GetLength(0);
                int valueRowLength = value.GetLength(1) * value.GetLength(2);
                int arrayRowLength = _array.GetLength(1) * _array.GetLength(2);
                for (int i = 0; i < rows; i++)
                    Array.Copy(value, i * valueRowLength, _array, i * arrayRowLength, valueRowLength);
            }
        }

        /// <summary>
        /// Allocate data for the array
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of columns</param>
        protected override void AllocateData(int rows, int cols)
        {
            DisposeObject();
            Debug.Assert(!_dataHandle.IsAllocated, "Handle should be free");

            int channelCount = new TColor().Dimension;

            _ptr = CvInvoke.cvCreateImageHeader(new MCvSize(cols, rows), CvDepth, channelCount);
            MIplImage iplImage = MIplImage;

            Debug.Assert(iplImage.align == 4, "Only 4 align is supported at this moment");

            if (typeof(TDepth) == typeof(Byte) && (cols & 3) != 0)
            {   //if the managed data isn't 4 aligned, make it so
                _array = new TDepth[rows, ((cols >> 2) << 2) + 4, channelCount];
            }
            else
            {
                _array = new TDepth[rows, cols, channelCount];
            }

            _dataHandle = GCHandle.Alloc(_array, GCHandleType.Pinned);
            CvInvoke.cvSetData(_ptr, _dataHandle.AddrOfPinnedObject(), _array.GetLength(1) * _array.GetLength(2) * Marshal.SizeOf(typeof(TDepth)));
        }

        ///<summary>
        ///Create a multi-channel image from multiple gray scale images
        ///</summary>
        ///<param name="channels">The image channels to be merged into a single image</param>
        public Image(Image<Gray, TDepth>[] channels)
        {
            int channelCount = new TColor().Dimension;

            Debug.Assert(channelCount == channels.Length);
            AllocateData(channels[0].Height, channels[0].Width);

            if (channelCount == 1)
            {
                //if this image only have a single channel
                CvInvoke.cvCopy(channels[0].Ptr, Ptr, IntPtr.Zero);
            }
            else
            {
                for (int i = 0; i < channelCount; i++)
                {
                    Image<Gray, TDepth> c = channels[i];

                    Debug.Assert(EqualSize(c), String.Format("The size of the {0}th channel is different from the 1st channel", i + 1));

                    CvInvoke.cvSetImageCOI(Ptr, i + 1);
                    CvInvoke.cvCopy(c.Ptr, Ptr, IntPtr.Zero);
                }
                CvInvoke.cvSetImageCOI(Ptr, 0);
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
            ROI = (Rectangle<double>)info.GetValue("Roi", typeof(Rectangle<double>));
        }

        /// <summary>
        /// A function used for runtime serilization of the object
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Roi", ROI);
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

        ///<summary> 
        /// The region of interest for this image 
        ///</summary>
        public Rectangle<double> ROI
        {
            set
            {
                if (value == null)
                {
                    //reset the image ROI
                    CvInvoke.cvResetImageROI(Ptr);
                }
                else
                {   //set the image ROI to the specific value
                    CvInvoke.cvSetImageROI(Ptr, value.MCvRect);
                }
            }
            get
            {
                //return the image ROI
                return new Rectangle<double>(CvInvoke.cvGetImageROI(Ptr));
            }
        }

        ///<summary> 
        ///The width of the image ( number of pixels in the x direction),
        ///if ROI is set, the width of the ROI 
        ///</summary>
        public override int Width { get { return isROISet ? (int)ROI.Width : Marshal.ReadInt32(Ptr, IplImageOffset.width); } }

        ///<summary> 
        ///The height of the image ( number of pixels in the y direction ),
        ///if ROI is set, the height of the ROI 
        ///</summary> 
        public override int Height { get { return isROISet ? (int)ROI.Height : Marshal.ReadInt32(Ptr, IplImageOffset.height); } }

        /// <summary>
        /// Get the underneath managed array
        /// </summary>
        public override System.Array ManagedArray
        {
            get { return _array; }
        }

        ///<summary> 
        /// The size of the internal iplImage structure, regardness of the ROI of this image: X -- Width; Y -- Height.
        /// When a new size is assigned to this property, the original image is resized (the ROI is resized as well when 
        /// available)
        ///</summary>
        public Point2D<int> Size
        {
            get
            {
                return new Point2D<int>(
                Marshal.ReadInt32(Ptr, IplImageOffset.width),
                Marshal.ReadInt32(Ptr, IplImageOffset.height));
            }
        }

        /// <summary>
        /// The equivalent depth type in opencv for this image
        /// </summary>
        public CvEnum.IPL_DEPTH CvDepth
        {
            get
            {
                if (typeof(TDepth) == typeof(Single))
                    return CvEnum.IPL_DEPTH.IPL_DEPTH_32F;
                else if (typeof(TDepth) == typeof(Byte))
                    return CvEnum.IPL_DEPTH.IPL_DEPTH_8U;
                else if (typeof(TDepth) == typeof(Double))
                    return CvEnum.IPL_DEPTH.IPL_DEPTH_64F;
                else if (typeof(TDepth) == typeof(SByte))
                    return Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8S;
                else
                    throw new NotImplementedException("Unsupported image depth");
            }
        }

        ///<summary> 
        ///Indicates if the region of interest has been set
        ///</summary> 
        public bool isROISet
        {
            get
            {
                return Marshal.ReadIntPtr(Ptr, IplImageOffset.roi) != IntPtr.Zero;
            }
        }

        ///<summary> The average color of this image </summary>
        public TColor Average
        {
            get
            {
                TColor res = new TColor();
                res.MCvScalar = CvInvoke.cvAvg(Ptr, IntPtr.Zero);
                return res;
            }
        }

        ///<summary> The sum for each color channel </summary>
        public TColor Sum
        {
            get
            {
                TColor res = new TColor();
                res.MCvScalar = CvInvoke.cvSum(Ptr);
                return res;
            }
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

        ///<summary> 
        /// Make a clone of the image using a mask, if ROI is set, only copy the ROI 
        /// </summary> 
        /// <param name="mask">the mask for cloning</param>
        ///<returns> A clone of the image</returns>
        public Image<TColor, TDepth> Clone(Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvCopy(Ptr, res.Ptr, mask.Ptr);
            return res;
        }

        ///<summary> Make a clone of the image, if ROI is set, only copy the ROI</summary>
        ///<returns> A clone of the image</returns>
        public Image<TColor, TDepth> Clone()
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvCopy(Ptr, res.Ptr, IntPtr.Zero);
            return res;
        }

        /// <summary>
        /// Copy the masked area of this image to destination
        /// </summary>
        /// <param name="dest">the destination to copy to</param>
        /// <param name="mask">the mask for copy</param>
        public void Copy(Image<TColor, TDepth> dest, Image<Gray, Byte> mask)
        {
            CvInvoke.cvCopy(Ptr, dest.Ptr, mask.Ptr);
        }

        /// <summary> 
        /// Create an image of the same size
        /// </summary>
        /// <remarks>The initial pixel in the image equals zero</remarks>
        /// <returns> The image of the same size</returns>
        public Image<TColor, TDepth> BlankClone()
        {
            return new Image<TColor, TDepth>(Width, Height);
        }
        #endregion

        #region Drawing functions
        ///<summary> Draw an Rectangle of the specific color and thickness </summary>
        ///<param name="rect"> The rectangle to be draw</param>
        ///<param name="color"> The color of the rectangle </param>
        ///<param name="thickness"> If thickness is less than 1, the rectangle is filled up </param>
        public virtual void Draw<T>(Rectangle<T> rect, TColor color, int thickness) where T : IComparable, new()
        {
            CvInvoke.cvRectangle(
                Ptr,
                rect.TopLeft,
                rect.BottomRight,
                color.MCvScalar,
                (thickness <= 0) ? -1 : thickness,
                CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                0);
        }

        ///<summary> Draw a 2D Cross of the specific color and thickness </summary>
        ///<param name="cross"> The 2D Cross to be draw</param>
        ///<param name="color"> The color of the cross </param>
        ///<param name="thickness"> Must be &gt; 0 </param>
        public void Draw<T>(Cross2D<T> cross, TColor color, int thickness) where T : IComparable, new()
        {
            Debug.Assert(thickness > 0, "Thickness should be > 0");
            if (thickness > 0)
            {
                Draw(cross.Horizontal, color, thickness);
                Draw(cross.Vertical, color, thickness);
            }
        }

        ///<summary> Draw a line segment of the specific color and thickness </summary>
        ///<param name="line"> The line segment to be draw</param>
        ///<param name="color"> The color of the line segment </param>
        ///<param name="thickness"> The thickness of the line segment </param>
        public virtual void Draw<T>(LineSegment2D<T> line, TColor color, int thickness) where T : IComparable, new()
        {
            Debug.Assert(thickness > 0, "Thickness should be > 0");
            if (thickness > 0)
                CvInvoke.cvLine(
                    Ptr,
                    line.P1,
                    line.P2,
                    color.MCvScalar,
                    thickness,
                    CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                    0);
        }

        ///<summary> Draw a convex polygon of the specific color and thickness </summary>
        ///<param name="polygon"> The convex polygon to be draw</param>
        ///<param name="color"> The color of the triangle </param>
        ///<param name="thickness"> If thickness is less than 1, the triangle is filled up </param>
        public virtual void Draw<T>(IConvexPolygon<T> polygon, TColor color, int thickness) where T : IComparable, new()
        {
            if (thickness > 0)
            {
                DrawPolyline(polygon.Vertices, true, color, thickness);
            }
            else
            {
                MCvPoint[] pts = Array.ConvertAll<Point2D<T>, MCvPoint>(polygon.Vertices,
                    delegate(Point2D<T> p) { return p.MCvPoint; });

                FillConvexPoly(pts, color);
            }
        }

        /// <summary>
        /// Fill the convex polygon with the specific color
        /// </summary>
        /// <param name="pts">the array of points that define the convex polygon</param>
        /// <param name="color">the color to fill the polygon with</param>
        public void FillConvexPoly(MCvPoint[] pts, TColor color)
        {
            CvInvoke.cvFillConvexPoly(Ptr, pts, pts.Length, color.MCvScalar, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, 0);
        }

        /// <summary>
        /// Draw the polyline defined by the array of 2D points
        /// </summary>
        /// <param name="pts">the points that defines the poly line</param>
        /// <param name="isClosed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
        /// <param name="color">the color used for drawing</param>
        /// <param name="thickness">the thinkness of the line</param>
        public virtual void DrawPolyline<T>(Point2D<T>[] pts, bool isClosed, TColor color, int thickness) where T : IComparable, new()
        {
            DrawPolyline(
                Array.ConvertAll<Point2D<T>, MCvPoint>(pts, delegate(Point2D<T> p) { return p.MCvPoint; }),
                isClosed,
                color,
                thickness);
        }

        /// <summary>
        /// Draw the polyline defined by the array of 2D points
        /// </summary>
        /// <param name="pts">the points that defines the poly line</param>
        /// <param name="isClosed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
        /// <param name="color">the color used for drawing</param>
        /// <param name="thickness">the thinkness of the line</param>
        public void DrawPolyline(MCvPoint[] pts, bool isClosed, TColor color, int thickness)
        {
            DrawPolyline(new MCvPoint[][] { pts }, isClosed, color, thickness);
        }

        /// <summary>
        /// Draw the polylines defined by the array of array of 2D points
        /// </summary>
        /// <param name="pts">An array of array of points that defines the poly lines</param>
        /// <param name="isClosed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
        /// <param name="color">the color used for drawing</param>
        /// <param name="thickness">the thinkness of the line</param>
        public void DrawPolyline(MCvPoint[][] pts, bool isClosed, TColor color, int thickness)
        {
            if (thickness > 0)
            {
                GCHandle[] handles = Array.ConvertAll<MCvPoint[], GCHandle>(pts, delegate(MCvPoint[] polyline) { return GCHandle.Alloc(polyline, GCHandleType.Pinned); });

                CvInvoke.cvPolyLine(
                    Ptr,
                    Array.ConvertAll<GCHandle, IntPtr>(handles, delegate(GCHandle h) { return h.AddrOfPinnedObject(); }),
                    Array.ConvertAll<MCvPoint[], int>(pts, delegate(MCvPoint[] polyline) { return polyline.Length; }),
                    pts.Length,
                    isClosed,
                    color.MCvScalar,
                    thickness,
                    Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                    0);

                foreach (GCHandle h in handles)
                    h.Free();
            }
        }

        ///<summary> Draw a Circle of the specific color and thickness </summary>
        ///<param name="circle"> The circle to be drawn</param>
        ///<param name="color"> The color of the circle </param>
        ///<param name="thickness"> If thickness is less than 1, the circle is filled up </param>
        public virtual void Draw<T>(Circle<T> circle, TColor color, int thickness) where T : IComparable, new()
        {
            CvInvoke.cvCircle(
                Ptr,
                circle.Center,
                System.Convert.ToInt32(circle.Radius),
                color,
                (thickness <= 0) ? -1 : thickness,
                CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                0);
        }

        ///<summary> Draw a Ellipse of the specific color and thickness </summary>
        ///<param name="ellipse"> The ellipse to be draw</param>
        ///<param name="color"> The color of the ellipse </param>
        ///<param name="thickness"> If thickness is less than 1, the ellipse is filled up </param>
        public void Draw(Ellipse<float> ellipse, TColor color, int thickness)
        {
            CvInvoke.cvEllipse(
                Ptr,
                ellipse.Center,
                new MCvSize(System.Convert.ToInt32(ellipse.Width) >> 1, System.Convert.ToInt32(ellipse.Height) >> 1),
                ellipse.RadianAngle,
                0.0,
                360.0,
                color,
                (thickness <= 0) ? -1 : thickness,
                CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                0);
        }

        /// <summary>
        /// Draw the text using the specific font on the image
        /// </summary>
        /// <param name="message">The text message to be draw</param>
        /// <param name="font">The font used for drawing</param>
        /// <param name="bottomLeft">The location of the bottom left corner of the font</param>
        /// <param name="color">The color of the text</param>
        public virtual void Draw<T>(String message, ref MCvFont font, Point2D<T> bottomLeft, TColor color) where T : IComparable, new()
        {
            CvInvoke.cvPutText(
                Ptr,
                message,
                bottomLeft.MCvPoint,
                ref font,
                color.MCvScalar);
        }

        ///<summary> Draw the contour with the specific color and thickness </summary>
        public void Draw(Seq<MCvPoint> c, TColor external_color, TColor hole_color, int thickness)
        {
            CvInvoke.cvDrawContours(
                Ptr,
                c.Ptr,
                external_color,
                hole_color,
                0,
                thickness,
                CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                new MCvPoint(0, 0));
        }
        #endregion

        #region Object Detection
        #region Haar detection
        /// <summary>
        /// Detect HaarCascade object in the current image, using predifined parameters
        /// </summary>
        /// <param name="haarObj">The object to be detected</param>
        /// <returns>The objects detected, one array per channel</returns>
        public Rectangle<double>[][] DetectHaarCascade(HaarCascade haarObj)
        {
            return DetectHaarCascade(haarObj, 1.1, 3, 1, new MCvSize(0, 0));
        }

        /// <summary>
        /// The function cvHaarDetectObjects finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles. The function scans the image several times at different scales (see cvSetImagesForHaarClassifierCascade). Each time it considers overlapping regions in the image and applies the classifiers to the regions using cvRunHaarClassifierCascade. It may also apply some heuristics to reduce number of analyzed regions, such as Canny prunning. After it has proceeded and collected the candidate rectangles (regions that passed the classifier cascade), it groups them and returns a sequence of average rectangles for each large enough group. The default parameters (scale_factor=1.1, min_neighbors=3, flags=0) are tuned for accurate yet slow object detection. For a faster operation on real video images the settings are: scale_factor=1.2, min_neighbors=2, flags=CV_HAAR_DO_CANNY_PRUNING, min_size=&lt;minimum possible face size&gt; (for example, ~1/4 to 1/16 of the image area in case of video conferencing). 
        /// </summary>
        /// <param name="haarObj">Haar classifier cascade in internal representation</param>
        /// <param name="scaleFactor">The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%</param>
        /// <param name="minNeighbors">Minimum number (minus 1) of neighbor rectangles that makes up an object. All the groups of a smaller number of rectangles than min_neighbors-1 are rejected. If min_neighbors is 0, the function does not any grouping at all and returns all the detected candidate rectangles, which may be useful if the user wants to apply a customized grouping procedure</param>
        /// <param name="flag">Mode of operation. Currently the only flag that may be specified is CV_HAAR_DO_CANNY_PRUNING. If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing.</param>
        /// <param name="minSize">Minimum window size. By default, it is set to the size of samples the classifier has been trained on (~20x20 for face detection)</param>
        /// <returns>The objects detected, one array per channel</returns>
        public Rectangle<double>[][] DetectHaarCascade(HaarCascade haarObj, double scaleFactor, int minNeighbors, int flag, MCvSize minSize)
        {
            using (MemStorage stor = new MemStorage())
            {
                Emgu.Utils.Func<IImage, int, Rectangle<double>[]> detector =
                    delegate(IImage img, int channel)
                    {
                        IntPtr objects = CvInvoke.cvHaarDetectObjects(
                        img.Ptr,
                        haarObj.Ptr,
                        stor.Ptr,
                        scaleFactor,
                        minNeighbors,
                        flag,
                        minSize);

                        int count = 0;
                        if (objects != IntPtr.Zero)
                        {
                            MCvSeq seq = (MCvSeq)Marshal.PtrToStructure(objects, typeof(MCvSeq));
                            count = seq.total;
                        }

                        Rectangle<double>[] recs = new Rectangle<double>[count];

                        if (count != 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                recs[i] = new Rectangle<double>(
                                    (MCvRect)Marshal.PtrToStructure(
                                        CvInvoke.cvGetSeqElem(objects, i),
                                        typeof(MCvRect)));
                            }
                            CvInvoke.cvClearSeq((IntPtr)objects);
                        }
                        return recs;
                    };

                Rectangle<double>[][] res = ForEachDuplicateChannel(detector);
                return res;
            }
        }
        #endregion

        #region Hugh line and circles
        ///<summary> 
        ///Apply Hugh transform to find line segments. 
        ///The current image must be a binary image (eg. the edges as a result of the Canny edge detector) 
        ///</summary> 
        public LineSegment2D<int>[][] HughLinesBinary(double rhoResolution, double thetaResolution, int threshold, double minLineWidth, double gapBetweenLines)
        {
            using (MemStorage stor = new MemStorage())
            {
                Emgu.Utils.Func<IImage, int, LineSegment2D<int>[]> detector =
                    delegate(IImage img, int channel)
                    {
                        IntPtr lines = CvInvoke.cvHoughLines2(img.Ptr, stor.Ptr, CvEnum.HOUGH_TYPE.CV_HOUGH_PROBABILISTIC, rhoResolution, thetaResolution, threshold, minLineWidth, gapBetweenLines);
                        MCvSeq lineSeq = (MCvSeq)Marshal.PtrToStructure(lines, typeof(MCvSeq));
                        LineSegment2D<int>[] linesegs = new LineSegment2D<int>[lineSeq.total];
                        for (int i = 0; i < lineSeq.total; i++)
                        {
                            int[] val = new int[4];
                            Marshal.Copy(CvInvoke.cvGetSeqElem(lines, i), val, 0, 4);
                            linesegs[i] = new LineSegment2D<int>(
                                new Point2D<int>(val[0], val[1]),
                                new Point2D<int>(val[2], val[3]));
                        }
                        CvInvoke.cvClearSeq(lines);
                        return linesegs;
                    };
                LineSegment2D<int>[][] res = ForEachDuplicateChannel(detector);
                return res;
            }
        }

        ///<summary> 
        ///First apply Canny Edge Detector on the current image, 
        ///then apply Hugh transform to find line segments 
        ///</summary>
        public LineSegment2D<int>[][] HughLines(TColor cannyThreshold, TColor cannyThresholdLinking, double rhoResolution, double thetaResolution, int threshold, double minLineWidth, double gapBetweenLines)
        {
            using (Image<TColor, TDepth> canny = Canny(cannyThreshold, cannyThresholdLinking))
            {
                return canny.HughLinesBinary(rhoResolution, thetaResolution, threshold, minLineWidth, gapBetweenLines);
            }
        }

        ///<summary> 
        ///First apply Canny Edge Detector on the current image, 
        ///then apply Hugh transform to find circles 
        ///</summary>
        public Circle<float>[][] HughCircles(TColor cannyThreshold, TColor cannyThresholdLinking, double dp, double minDist, int minRadius, int maxRadius)
        {
            using (MemStorage stor = new MemStorage())
            {
                double[] c1 = cannyThreshold.Resize(4).Coordinate;
                double[] c2 = cannyThresholdLinking.Resize(4).Coordinate;
                Emgu.Utils.Func<IImage, int, Circle<float>[]> detector =
                    delegate(IImage img, int channel)
                    {
                        IntPtr circlesSeqPtr = CvInvoke.cvHoughCircles(
                            img.Ptr,
                            stor.Ptr,
                            CvEnum.HOUGH_TYPE.CV_HOUGH_GRADIENT,
                            dp,
                            minDist,
                            c1[channel],
                            c2[channel],
                            minRadius,
                            maxRadius);

                        Seq<MCvPoint3D32f> cirSeq = new Seq<MCvPoint3D32f>(circlesSeqPtr, stor);

                        return System.Array.ConvertAll<MCvPoint3D32f, Circle<float>>(cirSeq.ToArray(),
                            delegate(MCvPoint3D32f p)
                            {
                                return new Circle<float>(new Point2D<float>(p.x, p.y), p.z);
                            });
                    };
                Circle<float>[][] res = ForEachDuplicateChannel(detector);

                return res;
            }
        }
        #endregion

        #region Contour detection
        /// <summary>
        /// Find contours
        /// </summary>
        /// <returns>Contours</returns>
        public Contour FindContours()
        {
            return FindContours(CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, CvEnum.RETR_TYPE.CV_RETR_LIST);
        }

        /// <summary>
        /// Find contours
        /// </summary>
        /// <param name="method">The type of approximation method</param>
        /// <param name="type">The retrival type</param>
        /// <returns>Contours</returns>
        public Contour FindContours(CvEnum.CHAIN_APPROX_METHOD method, CvEnum.RETR_TYPE type)
        {
            return FindContours(method, type, new MemStorage());
        }

        /// <summary>
        /// Find contours
        /// </summary>
        /// <param name="method">The type of approximation method</param>
        /// <param name="type">The retrival type</param>
        /// <param name="stor">The storage used by the sequences</param>
        /// <returns>Contours</returns>
        public Contour FindContours(CvEnum.CHAIN_APPROX_METHOD method, CvEnum.RETR_TYPE type, MemStorage stor)
        {
            IntPtr seq = IntPtr.Zero;

            int sequenceHeaderSize;
            if (method == Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_CODE)
            {
                //TODO: wrap CvChain and add code here
                throw new NotImplementedException("Not implmented");
            }
            else
            {
                sequenceHeaderSize = Marshal.SizeOf(typeof(MCvContour));
            }

            using (Image<TColor, TDepth> imagecopy = Clone()) //since cvFindContours modifies the content of the source, we need to make a clone
            {
                CvInvoke.cvFindContours(
                    imagecopy.Ptr,
                    stor.Ptr,
                    ref seq,
                    sequenceHeaderSize,
                    type,
                    method,
                    new MCvPoint(0, 0));
            }
            return new Contour(seq, stor);
        }
        #endregion
        #endregion

        #region Indexer pixel access
        /// <summary>
        /// Get or Set the color in the <paramref name="row"/>th row (y direction) and <paramref name="column"/>th column (x direction)
        /// </summary>
        /// <param name="row">the row (y direction) of the pixel </param>
        /// <param name="col">the column (x direction) of the pixel</param>
        /// <returns>the color in the <paramref name="row"/>th row and <paramref name="column"/>th column</returns>
        public TColor this[int row, int col]
        {
            get
            {
                TColor res = new TColor();
                res.MCvScalar = CvInvoke.cvGet2D(Ptr, row, col);
                return res;
            }
            set
            {
                CvInvoke.cvSet2D(Ptr, row, col, value.MCvScalar);
            }
        }

        /// <summary>
        /// Get or Set the color in the <paramref name="location"/>
        /// </summary>
        /// <param name="location">the location of the pixel </param>
        /// <returns>the color in the <paramref name="location"/></returns>
        public TColor this[Point2D<int> location]
        {
            get
            {
                TColor res = new TColor();
                res.MCvScalar = CvInvoke.cvGet2D(Ptr, location.Y, location.X);
                return res;
            }
            set
            {
                CvInvoke.cvSet2D(Ptr, location.Y, location.X, value.MCvScalar);
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
            start = ipl.imageData.ToInt64();
            widthStep = ipl.widthStep;

            if (ipl.roi != IntPtr.Zero)
            {
                MCvRect rec = CvInvoke.cvGetImageROI(ptr);
                elementCount = (int)rec.width * ipl.nChannels;
                byteWidth = ((int) ipl.depth >> 3) * elementCount;

                start += (int)rec.y * widthStep
                        + ((int) ipl.depth >> 3) * (int)rec.x;
                rows = (int)rec.height;
            }
            else
            {
                byteWidth = widthStep;
                elementCount = ipl.width * ipl.nChannels;
                rows = ipl.height;
            }
        }

        /// <summary>
        /// Apply convertor and compute result for each channel of the image.
        /// </summary>
        /// <remarks>
        /// For single channel image, apply converter directly.
        /// For multiple channel image, set the COI for the specific channel before appling the convertor
        /// </remarks>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="conv">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
        /// <returns>An array which contains result for each channel</returns>
        private R[] ForEachChannel<R>(Emgu.Utils.Func<IntPtr, int, R> conv)
        {
            int channelCount = new TColor().Dimension;
            R[] res = new R[channelCount];
            if (channelCount == 1)
                res[0] = conv(Ptr, 0);
            else
            {
                for (int i = 0; i < channelCount; i++)
                {
                    CvInvoke.cvSetImageCOI(Ptr, i + 1);
                    res[i] = conv(Ptr, i);
                }
                CvInvoke.cvSetImageCOI(Ptr, 0);
            }
            return res;
        }

        /// <summary>
        /// Apply convertor and compute result for each channel of the image, for single channel image, apply converter directly, for multiple channel image, make a copy of each channel to a temperary image and apply the convertor
        /// </summary>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="conv">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
        /// <returns>An array which contains result for each channel</returns>
        private R[] ForEachDuplicateChannel<R>(Emgu.Utils.Func<IImage, int, R> conv)
        {
            int channelCount = new TColor().Dimension;
            R[] res = new R[channelCount];
            if (channelCount == 1)
                res[0] = conv(this, 0);
            else
            {
                using (Image<Gray, TDepth> tmp = new Image<Gray, TDepth>(Width, Height))
                    for (int i = 0; i < channelCount; i++)
                    {
                        CvInvoke.cvSetImageCOI(Ptr, i + 1);
                        CvInvoke.cvCopy(Ptr, tmp.Ptr, IntPtr.Zero);
                        res[i] = conv(tmp, i);
                    }

                CvInvoke.cvSetImageCOI(Ptr, 0);
            }
            return res;
        }

        /// <summary>
        /// If the image has only one channel, apply the action directly on the IntPtr of this image and <paramref name="image2"/>,
        /// otherwise, make copy each channel of this image to a temperary one, apply action on it and another temperory image and copy the resulting image back to image2
        /// </summary>
        /// <typeparam name="TOtherDepth">The type of the depth of the <paramref name="dest"/> image</typeparam>
        /// <param name="act">The function which acepts the src IntPtr, dest IntPtr and index of the channel as input</param>
        /// <param name="dest">The destination image</param>
        private void ForEachDuplicateChannel<TOtherDepth>(Emgu.Utils.Action<IntPtr, IntPtr, int> act, Image<TColor, TOtherDepth> dest)
        {
            int channelCount = new TColor().Dimension;
            if (channelCount == 1)
                act(Ptr, dest.Ptr, 0);
            else
            {
                using (Image<Gray, TDepth> tmp1 = new Image<Gray, TDepth>(Width, Height))
                using (Image<Gray, TOtherDepth> tmp2 = new Image<Gray, TOtherDepth>(dest.Width, dest.Height))
                {
                    for (int i = 0; i < channelCount; i++)
                    {
                        CvInvoke.cvSetImageCOI(Ptr, i + 1);
                        CvInvoke.cvSetImageCOI(dest.Ptr, i + 1);
                        CvInvoke.cvCopy(Ptr, tmp1.Ptr, IntPtr.Zero);
                        act(tmp1.Ptr, tmp2.Ptr, i);
                        CvInvoke.cvCopy(tmp2.Ptr, dest.Ptr, IntPtr.Zero);
                    }
                }
                CvInvoke.cvSetImageCOI(Ptr, 0);
                CvInvoke.cvSetImageCOI(dest.Ptr, 0);
            }
        }
        #endregion

        #region Gradient, Edges and Features
        /// <summary>
        /// The function cvSobel calculates the image derivative by convolving the image with the appropriate kernel:
        /// dst(x,y) = dxorder+yodersrc/dxxorder?dyyorder |(x,y)
        /// The Sobel operators combine Gaussian smoothing and differentiation so the result is more or less robust to the noise. Most often, the function is called with (xorder=1, yorder=0, aperture_size=3) or (xorder=0, yorder=1, aperture_size=3) to calculate first x- or y- image derivative.
        /// </summary>
        /// <param name="xorder">Order of the derivative x</param>
        /// <param name="yorder">Order of the derivative y</param>
        /// <param name="apertureSize">Size of the extended Sobel kernel, must be 1, 3, 5 or 7. In all cases except 1, aperture_size xaperture_size separable kernel will be used to calculate the derivative.</param>
        /// <returns>The result of the sobel edge detector</returns>
        public Image<TColor, TDepth> Sobel(int xorder, int yorder, int apertureSize)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvSobel(Ptr, res.Ptr, xorder, yorder, apertureSize);
            return res;
        }

        /// <summary>
        /// The function cvLaplace calculates Laplacian of the source image by summing second x- and y- derivatives calculated using Sobel operator:
        /// dst(x,y) = d2src/dx2 + d2src/dy2
        /// Specifying aperture_size=1 gives the fastest variant that is equal to convolving the image with the following kernel:
        ///
        /// |0  1  0|
        /// |1 -4  1|
        /// |0  1  0|
        /// </summary>
        /// <param name="apertureSize">Aperture size </param>
        /// <returns>The Laplacian of the image</returns>
        public Image<TColor, Single> Laplace(int apertureSize)
        {
            Image<TColor, Single> res = new Image<TColor, float>(Width, Height);
            CvInvoke.cvLaplace(Ptr, res.Ptr, apertureSize);
            return res;
        }

        ///<summary> Find the edges on this image and marked them in the returned image.</summary>
        ///<param name="thresh"> The threshhold to find initial segments of strong edges</param>
        ///<param name="threshLinking"> The threshold used for edge Linking</param>
        ///<returns> The edges found by the Canny edge detector</returns>
        public Image<TColor, TDepth> Canny(TColor thresh, TColor threshLinking)
        {
            Image<TColor, TDepth> res = BlankClone();
            double[] t1 = thresh.Coordinate;
            double[] t2 = threshLinking.Coordinate;
            Emgu.Utils.Action<IntPtr, IntPtr, int> act =
                delegate(IntPtr src, IntPtr dest, int channel)
                {
                    CvInvoke.cvCanny(src, dest, t1[channel], t2[channel], 3);
                };
            ForEachDuplicateChannel<TDepth>(act, res);

            return res;
        }

        /// <summary>
        /// The function cvGoodFeaturesToTrack finds corners with big eigenvalues in the image. The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). The next step is rejecting the corners with the minimal eigenvalue less than quality_level?max(eig_image(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features
        /// </summary>
        /// <param name="maxFeaturesPerChannel">The maximum features to be detected per channel</param>
        /// <param name="quality_level">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
        /// <param name="min_distance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used. </param>
        /// <param name="block_size">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
        /// <param name="use_harris">If nonzero, Harris operator (cvCornerHarris) is used instead of default cvCornerMinEigenVal</param>
        /// <param name="k">Free parameter of Harris detector; used only if use_harris = true </param>
        /// <returns></returns>
        public Point2D<float>[][] GoodFeaturesToTrack(int maxFeaturesPerChannel, double quality_level, double min_distance, int block_size, bool use_harris, double k)
        {
            int channelCount = new TColor().Dimension;
            Point2D<float>[][] res = new Point2D<float>[channelCount][];

            float[,] coors = new float[maxFeaturesPerChannel, 2];

            using (Image<Gray, Single> eig_image = new Image<Gray, float>(Width, Height))
            using (Image<Gray, Single> tmp_image = new Image<Gray, float>(Width, Height))
            {
                Emgu.Utils.Func<IImage, int, Point2D<float>[]> detector =
                    delegate(IImage img, int channel)
                    {
                        int corner_count = maxFeaturesPerChannel;
                        GCHandle handle = GCHandle.Alloc(coors, GCHandleType.Pinned);
                        CvInvoke.cvGoodFeaturesToTrack(
                            img.Ptr,
                            eig_image.Ptr,
                            tmp_image.Ptr,
                            handle.AddrOfPinnedObject(),
                            ref corner_count,
                            quality_level,
                            min_distance,
                            IntPtr.Zero,
                            block_size,
                            use_harris ? 1 : 0,
                            k);
                        handle.Free();

                        Point2D<float>[] pts = new Point2D<float>[corner_count];
                        for (int i = 0; i < corner_count; i++)
                            pts[i] = new Point2D<float>(coors[i, 0], coors[i, 1]);
                        return pts;
                    };

                res = ForEachDuplicateChannel(detector);
            }

            return res;
        }
        #endregion

        #region Matching
        /// <summary>
        /// The function cvMatchTemplate is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
        /// </summary>
        /// <param name="template">Searched template; must be not greater than the source image and the same data type as the image</param>
        /// <param name="method">Specifies the way the template must be compared with image regions </param>
        /// <returns>The comparison result</returns>
        public Image<TColor, TDepth> MatchTemplate(Image<TColor, TDepth> template, CvEnum.TM_TYPE method)
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width - template.Width + 1, Height - template.Height + 1);
            CvInvoke.cvMatchTemplate(Ptr, template.Ptr, res.Ptr, method);
            return res;
        }
        #endregion

        #region Object tracking
        /// <summary>
        /// The function cvSnakeImage updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
        ///The parameter criteria.epsilon is used to define the minimal number of points that must be moved during any iteration to keep the iteration process running. 
        ///If at some iteration the number of moved points is less than criteria.epsilon or the function performed criteria.max_iter iterations, the function terminates. 
        /// </summary>
        /// <param name="c">Some existing contour</param>
        /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
        /// <param name="beta">Weight[s] of curvature energy, similar to alpha.</param>
        /// <param name="gamma">Weight[s] of image energy, similar to alpha.</param>
        /// <param name="windowSize">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
        /// <param name="tc">Termination criteria</param>
        /// <param name="storage"> the memory storage used by the resulting sequence</param>
        /// <returns>The snake[d] contour</returns>
        public Seq<MCvPoint> Snake(Seq<MCvPoint> c, float alpha, float beta, float gamma, Point2D<int> windowSize, MCvTermCriteria tc, MemStorage storage)
        {
            int count = c.Total;

            IntPtr points = Marshal.AllocHGlobal(count * 2 * sizeof(int));

            CvInvoke.cvCvtSeqToArray(c.Ptr, points, new MCvSlice(0, 0x3fffffff));
            CvInvoke.cvSnakeImage(
                Ptr,
                points,
                count,
                new float[1] { alpha },
                new float[1] { beta },
                new float[1] { gamma },
                1,
                new MCvSize(windowSize.X, windowSize.Y),
                tc,
                true);
            IntPtr rSeq = CvInvoke.cvCreateSeq(
                (int)CvEnum.SEQ_TYPE.CV_SEQ_POLYGON,
                Marshal.SizeOf(typeof(MCvContour)),
                Marshal.SizeOf(typeof(MCvPoint)),
                storage.Ptr);

            CvInvoke.cvSeqPushMulti(rSeq, points, count, false);
            Marshal.FreeHGlobal(points);

            return new Seq<MCvPoint>(rSeq, storage);

        }
        #endregion

        #region Logic
        #region And Methods
        ///<summary> Perform an elementwise AND operation with another image and return the result</summary>
        ///<param name="img2">The second image for the AND operation</param>
        ///<returns> The result of the AND operation</returns>
        public Image<TColor, TDepth> And(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAnd(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
            return res;
        }

        ///<summary> 
        ///Perform an elementwise AND operation with another image, using a mask, and return the result
        ///</summary>
        ///<param name="img2">The second image for the AND operation</param>
        ///<param name="mask">The mask for the AND operation</param>
        ///<returns> The result of the AND operation</returns>
        public Image<TColor, TDepth> And(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAnd(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
            return res;
        }

        ///<summary> Perform an binary AND operation with some color</summary>
        ///<param name="val">The color for the AND operation</param>
        ///<returns> The result of the AND operation</returns>
        public Image<TColor, TDepth> And(TColor val)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAndS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
            return res;
        }

        ///<summary> Perform an binary AND operation with some color using a mask</summary>
        ///<param name="val">The color for the AND operation</param>
        ///<param name="mask">The mask for the AND operation</param>
        ///<returns> The result of the AND operation</returns>
        public Image<TColor, TDepth> And(TColor val, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAndS(Ptr, val.MCvScalar, res.Ptr, mask.Ptr);
            return res;
        }
        #endregion

        #region Or Methods
        ///<summary> Perform an elementwise OR operation with another image and return the result</summary>
        ///<param name="img2">The second image for the OR operation</param>
        ///<returns> The result of the OR operation</returns>
        public Image<TColor, TDepth> Or(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvOr(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
            return res;
        }
        ///<summary> Perform an elementwise OR operation with another image, using a mask, and return the result</summary>
        ///<param name="img2">The second image for the OR operation</param>
        ///<param name="mask">The mask for the OR operation</param>
        ///<returns> The result of the OR operation</returns>
        public Image<TColor, TDepth> Or(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvOr(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
            return res;
        }

        ///<summary> Perform an elementwise OR operation with some color</summary>
        ///<param name="val">The value for the OR operation</param>
        ///<returns> The result of the OR operation</returns>
        public Image<TColor, TDepth> Or(TColor val)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvOrS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
            return res;
        }
        ///<summary> Perform an elementwise OR operation with some color using a mask</summary>
        ///<param name="val">The color for the OR operation</param>
        ///<param name="mask">The mask for the OR operation</param>
        ///<returns> The result of the OR operation</returns>
        public Image<TColor, TDepth> Or(TColor val, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvOrS(Ptr, val.MCvScalar, res.Ptr, mask.Ptr);
            return res;
        }
        #endregion

        #region Xor Methods
        ///<summary> Perform an elementwise XOR operation with another image and return the result</summary>
        ///<param name="img2">The second image for the XOR operation</param>
        ///<returns> The result of the XOR operation</returns>
        public Image<TColor, TDepth> Xor(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvXor(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
            return res;
        }

        /// <summary>
        /// Perform an elementwise XOR operation with another image, using a mask, and return the result
        /// </summary>
        /// <param name="img2">The second image for the XOR operation</param>
        /// <param name="mask">The mask for the XOR operation</param>
        /// <returns>The result of the XOR operation</returns>
        public Image<TColor, TDepth> Xor(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvXor(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
            return res;
        }

        /// <summary> 
        /// Perform an binary XOR operation with some color
        /// </summary>
        /// <param name="val">The value for the XOR operation</param>
        /// <returns> The result of the XOR operation</returns>
        public Image<TColor, TDepth> Xor(TColor val)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvXorS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
            return res;
        }

        /// <summary>
        /// Perform an binary XOR operation with some color using a mask
        /// </summary>
        /// <param name="val">The color for the XOR operation</param>
        /// <param name="mask">The mask for the XOR operation</param>
        /// <returns> The result of the XOR operation</returns>
        public Image<TColor, TDepth> Xor(TColor val, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvXorS(Ptr, val.MCvScalar, res.Ptr, mask.Ptr);
            return res;
        }
        #endregion

        ///<summary> 
        ///Compute the complement image
        ///</summary>
        ///<returns> The complement image</returns>
        public Image<TColor, TDepth> Not()
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvNot(Ptr, res.Ptr);
            return res;
        }
        #endregion

        #region Comparison
        ///<summary> Find the elementwise maximum value </summary>
        ///<param name="img2">The second image for the Max operation</param>
        ///<returns> An image where each pixel is the maximum of <i>this</i> image and the parameter image</returns>
        public Image<TColor, TDepth> Max(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvMax(Ptr, img2.Ptr, res.Ptr);
            return res;
        }

        ///<summary> Find the elementwise maximum value </summary>
        ///<param name="value">The value to compare with</param>
        ///<returns> An image where each pixel is the maximum of <i>this</i> image and <paramref name="value"/></returns>
        public Image<TColor, TDepth> Max(double value)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvMaxS(Ptr, value, res.Ptr);
            return res;
        }

        ///<summary> Find the elementwise minimum value </summary>
        ///<param name="img2">The second image for the Min operation</param>
        ///<returns> An image where each pixel is the minimum of <i>this</i> image and the parameter image</returns>
        public Image<TColor, TDepth> Min(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvMin(Ptr, img2.Ptr, res.Ptr);
            return res;
        }

        ///<summary> Find the elementwise minimum value </summary>
        ///<param name="value">The value to compare with</param>
        ///<returns> An image where each pixel is the minimum of <i>this</i> image and <paramref name="value"/></returns>
        public Image<TColor, TDepth> Min(double value)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvMinS(Ptr, value, res.Ptr);
            return res;
        }

        ///<summary>Checks that image elements lie between two scalars</summary>
        ///<param name="lower"> The lower limit of color value</param>
        ///<param name="higher"> The upper limit of color value</param>
        ///<returns> res[i,j] = 255 if inrange, 0 otherwise</returns>
        public Image<TColor, Byte> InRange(TColor lower, TColor higher)
        {
            Image<TColor, Byte> res = new Image<TColor, Byte>(Width, Height);
            CvInvoke.cvInRangeS(Ptr, lower.MCvScalar, higher.MCvScalar, res.Ptr);
            return res;
        }

        /// <summary>
        /// This function compare the current image with <paramref name="img2"/> and returns the comparison mask
        /// </summary>
        /// <param name="img2">The other image to compare with</param>
        /// <param name="cmp_type">The comparison type</param>
        /// <returns>The result of the comparison as a mask</returns>
        public Image<TColor, Byte> Cmp(Image<TColor, TDepth> img2, CvEnum.CMP_TYPE cmp_type)
        {
            Image<TColor, Byte> res = new Image<TColor, byte>(Width, Height);

            /*
            Emgu.Utils.Action<IntPtr, IntPtr, int> comparator = 
                delegate(IntPtr src, IntPtr dest, int channelIndex)
                {

                };

            ForEachDuplicateChannel<Byte>(
            */

            int dimension = new TColor().Dimension;
            if (dimension == 1)
            {
                CvInvoke.cvCmp(Ptr, img2.Ptr, res.Ptr, cmp_type);
            }
            else
            {
                using (Image<Gray, TDepth> src1 = new Image<Gray, TDepth>(Width, Height))
                using (Image<Gray, TDepth> src2 = new Image<Gray, TDepth>(Width, Height))
                using (Image<Gray, TDepth> dest = new Image<Gray, TDepth>(Width, Height))
                    for (int i = 0; i < dimension; i++)
                    {
                        CvInvoke.cvSetImageCOI(Ptr, i + 1);
                        CvInvoke.cvSetImageCOI(img2.Ptr, i + 1);
                        CvInvoke.cvCopy(Ptr, src1.Ptr, IntPtr.Zero);
                        CvInvoke.cvCopy(img2.Ptr, src2.Ptr, IntPtr.Zero);

                        CvInvoke.cvCmp(src1.Ptr, src2.Ptr, dest.Ptr, cmp_type);

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
        /// This function compare the current image with <paramref name="value"/> and returns the comparison mask
        /// </summary>
        /// <param name="value">The value to compare with</param>
        /// <param name="cmp_type">The comparison type</param>
        /// <returns>The result of the comparison as a mask</returns>
        public Image<TColor, Byte> Cmp(double value, CvEnum.CMP_TYPE cmp_type)
        {
            Image<TColor, Byte> res = new Image<TColor, byte>(Width, Height);

            int dimension = new TColor().Dimension;
            if (dimension == 1)
            {
                CvInvoke.cvCmpS(Ptr, value, res.Ptr, cmp_type);
            }
            else
            {
                using (Image<Gray, TDepth> src1 = new Image<Gray, TDepth>(Width, Height))
                using (Image<Gray, TDepth> dest = new Image<Gray, TDepth>(Width, Height))
                    for (int i = 0; i < dimension; i++)
                    {
                        CvInvoke.cvSetImageCOI(Ptr, i + 1);
                        CvInvoke.cvCopy(Ptr, src1.Ptr, IntPtr.Zero);

                        CvInvoke.cvCmpS(src1.Ptr, value, dest.Ptr, cmp_type);

                        CvInvoke.cvSetImageCOI(res.Ptr, i + 1);
                        CvInvoke.cvCopy(dest.Ptr, res.Ptr, IntPtr.Zero);
                    }
                CvInvoke.cvSetImageCOI(Ptr, 0);
                CvInvoke.cvSetImageCOI(res.Ptr, 0);
            }

            return res;
        }

        /// <summary>
        /// Count the non Zero elements for each channel
        /// </summary>
        /// <returns>Count the non Zero elements for each channel</returns>
        public int[] CountNonzero()
        {
            return
                ForEachChannel<int>(delegate(IntPtr channel, int channelNumber)
                {
                    return CvInvoke.cvCountNonZero(channel);
                });
        }

        /// <summary>
        /// Compare two images, returns true if the each of the pixels are equal, false otherwise
        /// </summary>
        /// <param name="img2">The other image to compare with</param>
        /// <returns>true if the each of the pixels for the two images are equal, false otherwise</returns>
        public bool Equals(Image<TColor, TDepth> img2)
        {
            //true if the references are equal
            if (System.Object.ReferenceEquals(this, img2)) return true;

            //false if size are not equal
            if (!EqualSize(img2)) return false;

            using (Image<TColor, Byte> neqMask = Cmp(img2, Emgu.CV.CvEnum.CMP_TYPE.CV_CMP_NE))
            {
                foreach (int c in neqMask.CountNonzero())
                    if (c != 0) return false;
                return true;
            }
        }
        #endregion

        #region Arithmatic
        #region Substraction methods
        ///<summary> Elementwise subtract another image from the current image </summary>
        ///<param name="img2">The second image to be subtraced from the current image</param>
        ///<returns> The result of elementwise subtracting img2 from the current image</returns>
        public Image<TColor, TDepth> Sub(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvSub(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
            return res;
        }

        ///<summary> Elementwise subtrace another image from the current image, using a mask</summary>
        ///<param name="img2">The image to be subtraced from the current image</param>
        ///<param name="mask">The mask for the subtract operation</param>
        ///<returns> The result of elementwise subtrating img2 from the current image, using the specific mask</returns>
        public Image<TColor, TDepth> Sub(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvSub(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
            return res;
        }

        ///<summary> Elementwise subtrace a color from the current image</summary>
        ///<param name="val">The color value to be subtraced from the current image</param>
        ///<returns> The result of elementwise subtracting color 'val' from the current image</returns>
        public Image<TColor, TDepth> Sub(TColor val)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvSubS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
            return res;
        }

        /// <summary>
        /// result = val - this
        /// </summary>
        /// <param name="val">the value which subtract this image</param>
        /// <returns>val - this</returns>
        public Image<TColor, TDepth> SubR(TColor val)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvSubRS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
            return res;
        }

        /// <summary>
        /// result = val - this, using a mask
        /// </summary>
        /// <param name="val">the value which subtract this image</param>
        /// <param name="mask"> The mask for substraction</param>
        /// <returns>val - this, with mask</returns>
        public Image<TColor, TDepth> SubR(TColor val, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvSubRS(Ptr, val.MCvScalar, res.Ptr, mask.Ptr);
            return res;
        }
        #endregion

        #region Addition methods
        ///<summary> Elementwise add another image with the current image </summary>
        ///<param name="img2">The image to be added to the current image</param>
        ///<returns> The result of elementwise adding img2 to the current image</returns>
        public Image<TColor, TDepth> Add(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAdd(Ptr, img2.Ptr, res.Ptr, IntPtr.Zero);
            return res;
        }
        ///<summary> Elementwise add <paramref name="img2"/> with the current image, using a mask</summary>
        ///<param name="img2">The image to be added to the current image</param>
        ///<param name="mask">The mask for the add operation</param>
        ///<returns> The result of elementwise adding img2 to the current image, using the specific mask</returns>
        public Image<TColor, TDepth> Add(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAdd(Ptr, img2.Ptr, res.Ptr, mask.Ptr);
            return res;
        }
        ///<summary> Elementwise add a color <paramref name="val"/> to the current image</summary>
        ///<param name="val">The color value to be added to the current image</param>
        ///<returns> The result of elementwise adding color <paramref name="val"/> from the current image</returns>
        public Image<TColor, TDepth> Add(TColor val)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAddS(Ptr, val.MCvScalar, res.Ptr, IntPtr.Zero);
            return res;
        }
        #endregion

        #region Multiplication methods
        ///<summary> Elementwise multiply another image with the current image and the <paramref name="scale"/></summary>
        ///<param name="img2">The image to be elementwise multiplied to the current image</param>
        ///<param name="scale">The scale to be multiplied</param>
        ///<returns> this .* img2 * scale </returns>
        public Image<TColor, TDepth> Mul(Image<TColor, TDepth> img2, double scale)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvMul(Ptr, img2.Ptr, res.Ptr, scale);
            return res;
        }
        ///<summary> Elementwise multiply <paramref name="img2"/> with the current image</summary>
        ///<param name="img2">The image to be elementwise multiplied to the current image</param>
        ///<returns> this .* img2 </returns>
        public Image<TColor, TDepth> Mul(Image<TColor, TDepth> img2)
        {
            return Mul(img2, 1.0);
        }

        ///<summary> Elementwise multiply the current image with <paramref name="scale"/></summary>
        ///<param name="scale">The scale to be multiplied</param>
        ///<returns> The scaled image </returns>
        public Image<TColor, TDepth> Mul(double scale)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvConvertScale(Ptr, res.Ptr, scale, 0.0);
            return res;
        }
        #endregion

        /// <summary>
        /// Accumulate <paramref name="img2"/> to the current image using the specific mask
        /// </summary>
        /// <param name="img2">The image to be added to the current image</param>
        /// <param name="mask">the mask</param>
        public void Acc(Image<TColor, TDepth> img2, Image<Gray, Byte> mask)
        {
            CvInvoke.cvAcc(img2.Ptr, Ptr, mask.Ptr);
        }

        /// <summary>
        /// Accumulate <paramref name="img2"/> to the current image using the specific mask
        /// </summary>
        /// <param name="img2">The image to be added to the current image</param>
        public void Acc(Image<TColor, TDepth> img2)
        {
            CvInvoke.cvAcc(img2.Ptr, Ptr, IntPtr.Zero);
        }

        ///<summary> 
        ///Return the weighted sum such that: res = this * alpha + img2 * beta + gamma
        ///</summary>
        public Image<TColor, TDepth> AddWeighted(Image<TColor, TDepth> img2, double alpha, double beta, double gamma)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAddWeighted(Ptr, alpha, img2.Ptr, beta, gamma, res.Ptr);
            return res;
        }

        ///<summary> 
        /// Update Running Average. <i>this</i> = (1-alpha)*<i>this</i> + alpha*img
        ///</summary>
        ///<param name="img">Input image, 1- or 3-channel, Byte or Single (each channel of multi-channel image is processed independently). </param>
        ///<param name="alpha">the weight of <paramref name="img"/></param>
        public void RunningAvg(Image<TColor, TDepth> img, double alpha)
        {
            CvInvoke.cvRunningAvg(img.Ptr, Ptr, alpha, IntPtr.Zero);
        }

        /// <summary>
        /// Raises every element of input array to p
        /// dst(I)=src(I)^p, if p is integer
        /// dst(I)=abs(src(I))^p, otherwise
        /// </summary>
        /// <param name="power">The exponent of power</param>
        /// <returns>The power image</returns>
        public Image<TColor, TDepth> Pow(double power)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvPow(Ptr, res.Ptr, power);
            return res;
        }

        /// <summary>
        /// calculates exponent of every element of input array:
        /// dst(I)=exp(src(I))
        /// Maximum relative error is ?7e-6. Currently, the function converts denormalized values to zeros on output.
        /// </summary>
        /// <returns>The exponent image</returns>
        public Image<TColor, TDepth> Exp()
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvExp(Ptr, res.Ptr);
            return res;
        }

        /// <summary>
        /// performs forward or inverse transform of 1D or 2D floating-point array
        /// </summary>
        /// <param name="type">Transformation flags</param>
        /// <param name="nonzeroRows">Number of nonzero rows to in the source array (in case of forward 2d transform), or a number of rows of interest in the destination array (in case of inverse 2d transform). If the value is negative, zero, or greater than the total number of rows, it is ignored. The parameter can be used to speed up 2d convolution/correlation when computing them via DFT</param>
        /// <returns>The result of DFT</returns>
        public Image<TColor, Single> DFT(CvEnum.CV_DXT type, int nonzeroRows)
        {
            Image<TColor, Single> res = new Image<TColor, float>(Width, Height);
            CvInvoke.cvDFT(Ptr, res.Ptr, type, nonzeroRows);
            return res;
        }

        /// <summary>
        /// performs forward or inverse transform of 2D floating-point image
        /// </summary>
        /// <param name="type">Transformation flags</param>
        /// <returns>The result of DFT</returns>
        public Image<TColor, Single> DFT(CvEnum.CV_DXT type)
        {
            return DFT(type, 0);
        }

        /// <summary>
        /// performs forward or inverse transform of 2D floating-point image
        /// </summary>
        /// <param name="type">Transformation flags</param>
        /// <returns>The result of DCT</returns>
        public Image<TColor, Single> DCT(CvEnum.CV_DCT_TYPE type)
        {
            Image<TColor, Single> res = new Image<TColor, float>(Width, Height);
            CvInvoke.cvDCT(Ptr, res.Ptr, type);
            return res;
        }

        /// <summary>
        /// Calculates natural logarithm of absolute value of every element of input array
        /// </summary>
        /// <returns>Natural logarithm of absolute value of every element of input array</returns>
        public Image<TColor, TDepth> Log()
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvLog(Ptr, res.Ptr);
            return res;
        }

        ///<summary> 
        ///Computes absolute different between <i>this</i> image and the other image
        ///</summary>
        ///<param name="img2">The other image to compute absolute different with</param>
        ///<returns> The image that contains the absolute different value</returns>
        public Image<TColor, TDepth> AbsDiff(Image<TColor, TDepth> img2)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvAbsDiff(Ptr, img2.Ptr, res.Ptr);
            return res;
        }
        #endregion

        #region Sampling, Interpolation and Geometrical Transforms
        ///<summary> Sample the pixel values on the specific line segment </summary>
        ///<param name="line"> The line to obtain samples</param>
        public TDepth[,] Sample(LineSegment2D<int> line)
        {
            int size = Math.Max(Math.Abs(line.P2.X - line.P1.X), Math.Abs(line.P2.Y - line.P1.Y));
            TDepth[,] data = new TDepth[size, new TColor().Dimension];
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            CvInvoke.cvSampleLine(
                Ptr,
                line.P1,
                line.P2,
                handle.AddrOfPinnedObject(),
                8);
            handle.Free();
            return data;
        }

        ///<summary>
        /// Scale the image to the specific size 
        /// </summary>
        ///<param name="width">The width of the returned image.</param>
        ///<param name="height">The height of the returned image.</param>
        ///<returns>The resized image</returns>
        public Image<TColor, TDepth> Resize(int width, int height)
        {
            Image<TColor, TDepth> imgScale = new Image<TColor, TDepth>(width, height);
            CvInvoke.cvResize(Ptr, imgScale.Ptr, CvEnum.INTER.CV_INTER_LINEAR);
            return imgScale;
        }

        /// <summary>
        /// Scale the image to the specific size
        /// </summary>
        /// <param name="width">The width of the returned image.</param>
        /// <param name="height">The height of the returned image.</param>
        /// <param name="preserverScale">if true, the scale is preservered and the resulting image has maximum width(height) possible that is &lt;= <paramref name="width"/> (<paramref name="height"/>), if false, this function is equaivalent to Resize(int width, int height)</param>
        /// <returns></returns>
        public Image<TColor, TDepth> Resize(int width, int height, bool preserverScale)
        {
            if (preserverScale)
            {
                return Resize(Math.Min((double)width / Width, (double)height / Height));
            }
            else
            {
                return Resize(width, height);
            }
        }

        ///<summary>
        /// Scale the image to the specific size: width *= scale; height *= scale  
        /// </summary>
        /// <returns>The scaled image</returns>
        public Image<TColor, TDepth> Resize(double scale)
        {
            return Resize(
                (int)(Width * scale),
                (int)(Height * scale));
        }

        /// <summary>
        /// Rotate the image the specified angle cropping the result to the original size
        /// </summary>
        /// <param name="angle">The angle of rotation in degrees.</param>
        /// <param name="background">The color with wich to fill the background</param>        
        public Image<TColor, TDepth> Rotate(double angle, TColor background)
        {
            return Rotate(angle, background, true);
        }

        /// <summary>
        /// Rotate the image the specified angle
        /// </summary>
        /// <param name="angle">The angle of rotation in degrees.</param>
        /// <param name="background">The color with wich to fill the background</param>
        /// <param name="crop">If set to true the image is cropped to its original size, possibly losing corners information. If set to false the result image has different size than original and all rotation information is preserved</param>
        /// <returns>The rotated image</returns>
        public Image<TColor, TDepth> Rotate(double angle, TColor background, bool crop)
        {
            Image<TColor, TDepth> resultImage;
            if (crop)
            {
                resultImage = new Image<TColor, TDepth>(Width, Height);
                Point2D<float> center = new Point2D<float>(Width * 0.5f, Height * 0.5f);
                using (RotationMatrix2D rotationMatrix = new RotationMatrix2D(center, -angle, 1))
                {
                    CvInvoke.cvWarpAffine(Ptr, resultImage.Ptr, rotationMatrix.Ptr, (int)CvEnum.INTER.CV_INTER_CUBIC | (int)CvEnum.WARP.CV_WARP_FILL_OUTLIERS, background.MCvScalar);
                }
            }
            else
            {
                //Maximum possible size is equal to the diagonal length of the image
                int maxSize = (int)Math.Round(Math.Sqrt(Width * Width + Height * Height));
                float offsetX = (maxSize - Width) * 0.5f;
                float offsetY = (maxSize - Height) * 0.5f;

                Point2D<float> center = new Point2D<float>(maxSize * .5f, maxSize * .5f);

                using (RotationMatrix2D rotationMatrix = new RotationMatrix2D(center, -angle, 1))
                using (Matrix<float> corners = new Matrix<float>(new float[4, 3]{
                 {offsetX,offsetY,1},
                 {offsetX,offsetY+Height,1},
                 {offsetX+Width,offsetY,1},
                 {offsetX+Width,offsetY+Height,1}}))
                using (Matrix<float> rotatedCorners = new Matrix<float>(4, 2))
                using (Image<TColor, TDepth> tempImage1 = new Image<TColor, TDepth>(maxSize, maxSize, background))
                using (Image<TColor, TDepth> tempImage2 = new Image<TColor, TDepth>(maxSize, maxSize))
                {
                    /*
                     * Frame the original image into a bigger one of side maxSize
                     * Rotating the framed image will always keep the original image without losing corners information
                     */
                    MCvRect CvR = new MCvRect((maxSize - Width) / 2, (maxSize - Height) / 2, Width, Height);
                    CvInvoke.cvSetImageROI(tempImage1.Ptr, CvR);
                    CvInvoke.cvCopy(Ptr, tempImage1.Ptr, IntPtr.Zero);
                    CvInvoke.cvResetImageROI(tempImage1.Ptr);

                    /*
                     * Rotate
                     */
                    CvInvoke.cvWarpAffine(tempImage1.Ptr, tempImage2.Ptr, rotationMatrix.Ptr, (int)CvEnum.INTER.CV_INTER_CUBIC | (int)CvEnum.WARP.CV_WARP_FILL_OUTLIERS, background.MCvScalar);

                    /*
                     * Calculate the position of the original corners in the rotated image
                     */
                    CvInvoke.cvGEMM(corners.Ptr, rotationMatrix.Ptr, 1, IntPtr.Zero, 1, rotatedCorners.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);
                    float[,] data = rotatedCorners.Data;
                    int minX = (int)Math.Round(Math.Min(Math.Min(data[0, 0], data[1, 0]), Math.Min(data[2, 0], data[3, 0])));
                    int maxX = (int)Math.Round(Math.Max(Math.Max(data[0, 0], data[1, 0]), Math.Max(data[2, 0], data[3, 0])));
                    int minY = (int)Math.Round(Math.Min(Math.Min(data[0, 1], data[1, 1]), Math.Min(data[2, 1], data[3, 1])));
                    int maxY = (int)Math.Round(Math.Max(Math.Max(data[0, 1], data[1, 1]), Math.Max(data[2, 1], data[3, 1])));
                    MCvRect toCrop = new MCvRect(minX, maxSize - maxY, maxX - minX, maxY - minY);

                    /*
                     * Crop the region of interest
                     */
                    resultImage = new Image<TColor, TDepth>(maxX - minX, maxY - minY);
                    CvInvoke.cvSetImageROI(tempImage2.Ptr, toCrop);
                    CvInvoke.cvCopy(tempImage2.Ptr, resultImage.Ptr, IntPtr.Zero);
                    CvInvoke.cvResetImageROI(tempImage2.Ptr);
                }
            }
            return resultImage;
        }


        #endregion

        #region Image color and depth conversion
        private static CvEnum.COLOR_CONVERSION GetColorCvtCode(Type srcType, Type destType)
        {
            ColorInfoAttribute srcInfo = (ColorInfoAttribute)srcType.GetType().GetCustomAttributes(typeof(ColorInfoAttribute), true)[0];
            ColorInfoAttribute destInfo = (ColorInfoAttribute)destType.GetType().GetCustomAttributes(typeof(ColorInfoAttribute), true)[0];

            String key = String.Format("CV_{0}2{1}", srcInfo.ConversionCodeName, destInfo.ConversionCodeName);
            return (CvEnum.COLOR_CONVERSION)Enum.Parse(typeof(CvEnum.COLOR_CONVERSION), key, true);
        }

        ///<summary> Convert the current image to the specific color and depth </summary>
        ///<typeparam name="TOtherColor"> The type of color to be converted to </typeparam>
        ///<typeparam name="TOtherDepth"> The type of pixel depth to be converted to </typeparam>
        ///<returns> Image of the specific color and depth </returns>
        public Image<TOtherColor, TOtherDepth> Convert<TOtherColor, TOtherDepth>() where TOtherColor : Emgu.CV.ColorType, new()
        {
            Image<TOtherColor, TOtherDepth> res = new Image<TOtherColor, TOtherDepth>(Width, Height);

            if (typeof(TColor) == typeof(TOtherColor))
            {   //same color
                if (typeof(TDepth) == typeof(TOtherDepth))
                {   //same depth
                    CvInvoke.cvCopy(Ptr, res.Ptr, IntPtr.Zero);
                }
                else
                {
                    //different depth
                    int channelCount = new TColor().Dimension;
                    IntPtr src = Ptr;
                    IntPtr dest = res.Ptr;
                    Type t1 = typeof(TDepth);
                    Type t2 = typeof(TOtherDepth);
                    {
                        if (t1 == typeof(Single) && t2 == typeof(Byte))
                        {
                            double min = 0.0, max = 0.0, scale, shift;
                            MCvPoint p1 = new MCvPoint();
                            MCvPoint p2 = new MCvPoint();
                            if (channelCount == 1)
                            {
                                CvInvoke.cvMinMaxLoc(src, ref min, ref max, ref p1, ref p2, IntPtr.Zero);
                            }
                            else
                            {
                                for (int i = 0; i < channelCount; i++)
                                {
                                    double minForChannel = 0.0, maxForChannel = 0.0;
                                    CvInvoke.cvSetImageCOI(src, i + 1);
                                    CvInvoke.cvMinMaxLoc(src, ref minForChannel, ref maxForChannel, ref p1, ref p2, IntPtr.Zero);
                                    min = Math.Min(min, minForChannel);
                                    max = Math.Max(max, maxForChannel);
                                }
                                CvInvoke.cvSetImageCOI(src, 0);
                            }
                            scale = (max == min) ? 0.0 : 256.0 / (max - min);
                            shift = (scale == 0) ? min : -min * scale;
                            CvInvoke.cvConvertScaleAbs(src, dest, scale, shift);
                        }
                        else
                        {
                            CvInvoke.cvConvertScale(src, dest, 1.0, 0.0);
                        }
                    }
                }
            }
            else
            {   //different color
                Emgu.Utils.Action<IntPtr, IntPtr, Type, Type> convertColor =
                    delegate(IntPtr src, IntPtr dest, Type c1, Type c2)
                    {
                        try
                        {
                            // if the direct conversion exist, apply the conversion
                            CvInvoke.cvCvtColor(src, dest, GetColorCvtCode(c1, c2));
                        }
                        catch (Exception)
                        {
                            //if a direct conversion doesn't exist, apply a two step conversion
                            using (Image<Bgr, TDepth> tmp = new Image<Bgr, TDepth>(Width, Height))
                            {
                                CvInvoke.cvCvtColor(src, tmp.Ptr, GetColorCvtCode(c1, typeof(Bgr)));
                                CvInvoke.cvCvtColor(tmp.Ptr, dest, GetColorCvtCode(typeof(Bgr), c2));
                            }
                        }
                    };

                if (typeof(TDepth) == typeof(TOtherDepth))
                {   //same depth
                    convertColor(Ptr, res.Ptr, typeof(TColor), typeof(TOtherColor));
                }
                else
                {   //different depth
                    using (Image<TColor, TOtherDepth> tmp = Convert<TColor, TOtherDepth>())
                        convertColor(tmp.Ptr, res.Ptr, typeof(TColor), typeof(TOtherColor));
                }
            }

            return res;
        }

        ///<summary> Convert the current image to the specific depth, at the same time scale and shift the values of the pixel</summary>
        ///<param name="scale"> The value to be multipled with the pixel </param>
        ///<param name="shift"> The value to be added to the pixel</param>
        /// <typeparam name="TOtherDepth"> The type of depth to convert to</typeparam>
        ///<returns> Image of the specific depth, val = val * scale + shift </returns>
        public Image<TColor, TOtherDepth> ConvertScale<TOtherDepth>(double scale, double shift)
        {
            Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Width, Height);

            if (typeof(TOtherDepth) == typeof(System.Byte))
                CvInvoke.cvConvertScaleAbs(Ptr, res.Ptr, scale, shift);
            else
                CvInvoke.cvConvertScale(Ptr, res.Ptr, scale, shift);
            return res;
        }
        #endregion

        #region Conversion with Bitmap
        /// <summary>
        /// The Get property provide a more efficient way to convert Image&lt;Gray, Byte&gt; and Image&lt;Bgr, Byte&gt; into Bitmap
        /// such that the image data is <b>shared</b> with Bitmap. 
        /// If you change the pixel value on the Bitmap, you change the pixel values on the Image object as well!
        /// For other types of image this property has the same effect as ToBitmap()
        /// <b>Take extra caution not to use the Bitmap after the Image object is disposed</b>
        /// The Set property convert the bitmap to this Image type.
        /// </summary>
        public Bitmap Bitmap
        {
            get
            {
                bool grayByte = (typeof(TColor) == typeof(Gray) && typeof(TDepth) == typeof(Byte));
                bool bgrByte = (typeof(TColor) == typeof(Bgr) && typeof(TDepth) == typeof(Byte));

#if LINUX
                // Mono doesn't support scan0 constructure with Format24bppRgb, use ToBitmap instead
                // TODO: check mono buzilla Bug 363431 to see when it will be fixed 
                if (grayByte)
                    return ToBitmap();
                else
                {
                    Image<Bgra, Byte> res = Convert<Bgra, Byte>();
                    CvInvoke.cvSetImageCOI(res.Ptr, 4);
                    CvInvoke.cvSet(res.Ptr, new MCvScalar(255.0, 255.0, 255.0, 255.0), IntPtr.Zero);
                    CvInvoke.cvSetImageCOI(res.Ptr, 0);
                    return res.ToBitmap();
                }
#else
                
                if (!grayByte && !bgrByte) return ToBitmap();

                IntPtr scan0;
                int step;
                MCvSize size;
                CvInvoke.cvGetRawData(Ptr, out scan0, out step, out size);

                if (grayByte)
                {   //Grayscale of Bytes
                    Bitmap bmp = new Bitmap(
                        size.width,
                        size.height,
                        step,
                        System.Drawing.Imaging.PixelFormat.Format8bppIndexed,
                        scan0
                        );
                    bmp.Palette = Utils.GrayscalePalette;
                    return bmp;
                }
                else 
                {   //Bgr byte    
                    return new Bitmap(
                        size.width,
                        size.height,
                        step,
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                        scan0);
                }
#endif
            }
            set
            {
                if (typeof(TColor) == typeof(Bgr) && typeof(TDepth) == typeof(Byte))
                {
                    #region Handling <Bgr, Byte> image
                    DisposeObject();

                    if (value.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                    {
                        AllocateData(value.Height, value.Width);
                        int rows = value.Height;

                        System.Drawing.Imaging.BitmapData data = value.LockBits(
                            new System.Drawing.Rectangle(0, 0, value.Width, value.Height),
                            System.Drawing.Imaging.ImageLockMode.ReadOnly,
                            System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                        int arrayWidthStep = Marshal.SizeOf(typeof(TDepth)) * new TColor().Dimension * _array.GetLength(1);
                        for (int i = 0; i < rows; i++)
                        {
                            Emgu.Utils.memcpy(
                                (IntPtr)(_dataHandle.AddrOfPinnedObject().ToInt64() + i * arrayWidthStep),
                                (IntPtr)(data.Scan0.ToInt64() + i * data.Stride),
                                data.Stride);
                        }
                        value.UnlockBits(data);
                    }
                    else
                    {   //Other pixel format
                        AllocateData(value.Height, value.Width);
                        for (int i = 0; i < value.Width; i++)
                            for (int j = 0; j < value.Height; j++)
                            {
                                System.Drawing.Color color = value.GetPixel(i, j);
                                CvInvoke.cvSet2D(_ptr, j, i, new MCvScalar(color.B, color.G, color.R));
                            }
                    }
                    #endregion
                } else if (typeof(TColor) == typeof(Bgra) && typeof(TDepth) == typeof(Byte))
                {
                    #region Handling <Bgra, Byte> image
                    AllocateData(value.Height, value.Width);
                    for (int i = 0; i < value.Width; i++)
                        for (int j = 0; j < value.Height; j++)
                        {
                            System.Drawing.Color color = value.GetPixel(i, j);
                            CvInvoke.cvSet2D(_ptr, j, i, new MCvScalar(color.B, color.G, color.R, color.A));
                        }
                    #endregion
                }
                else
                {
                    #region Handling other image types
                    using (Image<Bgr, Byte> tmp1 = new Image<Bgr, Byte>(value))
                    using (Image<TColor, TDepth> tmp2 = tmp1.Convert<TColor, TDepth>())
                    {
                        DisposeObject();
                        AllocateData(tmp2.Rows, tmp2.Cols);
                        tmp2.Copy(this);
                    }
                    #endregion
                }
            }
        }

        /// <summary> 
        /// Convert this image into Bitmap, the pixel values are copied over to the Bitmap
        /// </summary>
        /// <remarks> For better performance on Image&lt;Gray, Byte&gt; and Image&lt;Bgr, Byte&gt;, consider using the Bitmap property </remarks>
        /// <returns> This image in Bitmap format, the pixel data are copied over to the Bitmap</returns>
        public Bitmap ToBitmap()
        {
            if (typeof(TColor) == typeof(Gray)) // if this is a gray scale image
            {
                if (typeof(TDepth) == typeof(Byte))
                {
                    Bitmap bmp = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    System.Drawing.Imaging.BitmapData data = bmp.LockBits(
                        new System.Drawing.Rectangle(0, 0, Width, Height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                        System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    Int64 dataPtr = data.Scan0.ToInt64();

                    Int64 start;
                    int elementCount, byteWidth, rows, widthStep;
                    RoiParam(Ptr, out start, out rows, out elementCount, out byteWidth, out widthStep);
                    for (int row = 0; row < data.Height; row++, start += widthStep, dataPtr += data.Stride)
                        Emgu.Utils.memcpy((IntPtr)dataPtr, (IntPtr)start, data.Stride);

                    bmp.UnlockBits(data);
                    bmp.Palette = Utils.GrayscalePalette;

                    return bmp;
                }
                else
                {
                    using (Image<Gray, Byte> temp = Convert<Gray, Byte>())
                    {
                        return temp.ToBitmap();
                    }
                }
            }
            else if (typeof(TColor) == typeof(Bgra)) //if this is Bgra image
            {
                if (typeof(TDepth) == typeof(byte))
                {
                    Image<Bgra, Byte> img = this as Image<Bgra, Byte>;
                    Image<Gray, Byte>[] channels = img.Split();
                    channels = new Image<Gray, byte>[] { channels[3], channels[0], channels[1], channels[2] };
                    Image<Bgra, Byte> img2 = new Image<Bgra, byte>(channels);

                    Bitmap bmp = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    System.Drawing.Imaging.BitmapData data = bmp.LockBits(
                         new System.Drawing.Rectangle(0, 0, Width, Height),
                         System.Drawing.Imaging.ImageLockMode.WriteOnly,
                         System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    Int64 dataPtr = data.Scan0.ToInt64();

                    Int64 start;
                    int elementCount, byteWidth, rows, widthStep;
                    RoiParam(Ptr, out start, out rows, out elementCount, out byteWidth, out widthStep);
                    for (int row = 0; row < data.Height; row++, start += widthStep, dataPtr += data.Stride)
                        Emgu.Utils.memcpy((IntPtr)dataPtr, (IntPtr)start, data.Stride);

                    bmp.UnlockBits(data);
                    return bmp;
                }
                else
                {
                    using (Image<Bgra, Byte> tmp = Convert<Bgra, Byte>())
                    {
                        return tmp.ToBitmap();
                    }
                }
            }
            else //if this is a multiple channel image
            {
                if (typeof(TColor) == typeof(Bgr) && typeof(TDepth) == typeof(Byte))
                {
                    //create the bitmap and get the pointer to the data
                    Bitmap bmp = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    System.Drawing.Imaging.BitmapData data = bmp.LockBits(
                        new System.Drawing.Rectangle(0, 0, Width, Height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    Int64 dataPtr = data.Scan0.ToInt64();

                    Int64 start;
                    int elementCount, byteWidth, rows, widthStep;
                    RoiParam(Ptr, out start, out rows, out elementCount, out byteWidth, out widthStep);
                    for (int row = 0; row < data.Height; row++, start += widthStep, dataPtr += data.Stride)
                        Emgu.Utils.memcpy((IntPtr)dataPtr, (IntPtr)start, data.Stride);

                    bmp.UnlockBits(data);

                    return bmp;
                }
                else
                {
                    using (Image<Bgr, Byte> temp = Convert<Bgr, Byte>())
                    {
                        return temp.ToBitmap();
                    }
                }
            }
        }

        ///<summary> Create a Bitmap image of certain size</summary>
        ///<param name="width">The width of the bitmap</param>
        ///<param name="height"> The height of the bitmap</param>
        ///<returns> This image in Bitmap format of the specific size</returns>
        public Bitmap ToBitmap(int width, int height)
        {
            using (Image<TColor, TDepth> scaledImage = Resize(width, height))
                return scaledImage.ToBitmap();
        }
        #endregion

        #region Pyramids
        ///<summary>
        ///The function PyrDown performs downsampling step of Gaussian pyramid decomposition. 
        ///First it convolves <i>this</i> image with the specified filter and then downsamples the image 
        ///by rejecting even rows and columns.
        ///</summary>
        ///<returns> The downsampled image</returns>
        public Image<TColor, TDepth> PyrDown()
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width >> 1, Height >> 1);
            CvInvoke.cvPyrDown(Ptr, res.Ptr, CvEnum.FILTER_TYPE.CV_GAUSSIAN_5x5);
            return res;
        }

        ///<summary>
        ///The function cvPyrUp performs up-sampling step of Gaussian pyramid decomposition. 
        ///First it upsamples <i>this</i> image by injecting even zero rows and columns and then convolves 
        ///result with the specified filter multiplied by 4 for interpolation. 
        ///So the resulting image is four times larger than the source image.
        ///</summary>
        ///<returns> The upsampled image</returns>
        public Image<TColor, TDepth> PyrUp()
        {
            Image<TColor, TDepth> res = new Image<TColor, TDepth>(Width << 1, Height << 1);
            CvInvoke.cvPyrUp(Ptr, res.Ptr, CvEnum.FILTER_TYPE.CV_GAUSSIAN_5x5);
            return res;
        }
        #endregion

        #region Special Image Transforms
        ///<summary> Use impaint to recover the intensity of the pixels which location defined by <paramref>mask</paramref> on <i>this</i> image </summary>
        ///<returns> The inpainted image </returns>
        public Image<TColor, TDepth> InPaint(Image<Gray, Byte> mask, double radius)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvInpaint(Ptr, mask.Ptr, res.Ptr, CvEnum.INPAINT_TYPE.CV_INPAINT_TELEA, radius);
            return res;
        }
        #endregion

        #region Morphological Operations
        ///<summary>
        ///Erodes <i>this</i> image using a 3x3 rectangular structuring element.
        ///Erosion are applied serveral (iterations) times
        ///</summary>
        ///<returns> The eroded image</returns>
        public Image<TColor, TDepth> Erode(int iterations)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvErode(Ptr, res.Ptr, IntPtr.Zero, iterations);
            return res;
        }

        ///<summary>
        ///Dilates <i>this</i> image using a 3x3 rectangular structuring element.
        ///Dilation are applied serveral (iterations) times
        ///</summary>
        ///<returns> The dialated image</returns>
        public Image<TColor, TDepth> Dilate(int iterations)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvDilate(Ptr, res.Ptr, IntPtr.Zero, iterations);
            return res;
        }

        ///<summary>
        ///Erodes <i>this</i> image inplace using a 3x3 rectangular structuring element.
        ///Erosion are applied serveral (iterations) times
        ///</summary>
        public void _Erode(int iterations)
        {
            CvInvoke.cvErode(Ptr, Ptr, IntPtr.Zero, iterations);
        }

        ///<summary>
        ///Dilates <i>this</i> image inplace using a 3x3 rectangular structuring element.
        ///Dilation are applied serveral (iterations) times
        ///</summary>
        public void _Dilate(int iterations)
        {
            CvInvoke.cvDilate(Ptr, Ptr, IntPtr.Zero, iterations);
        }
        #endregion

        #region generic operations
        ///<summary> perform an generic action based on each element of the Image</summary>
        public void Action(System.Action<TDepth> action)
        {
            MIplImage image1 = MIplImage;
            Int64 data1 = image1.imageData.ToInt64();
            int step1 = image1.widthStep;
            int cols1 = image1.width * image1.nChannels;

            int sizeOfD = Marshal.SizeOf(typeof(TDepth));
            int width1 = sizeOfD * cols1;
            if (image1.roi != IntPtr.Zero)
            {
                Rectangle<double> rec = ROI;
                data1 += (int)rec.Bottom * step1
                        + sizeOfD * (int)rec.Left * image1.nChannels;
            }

            TDepth[] row1 = new TDepth[cols1];
            GCHandle handle1 = GCHandle.Alloc(row1, GCHandleType.Pinned);
            for (int row = 0; row < Height; row++, data1 += step1)
            {
                Emgu.Utils.memcpy(handle1.AddrOfPinnedObject(), new IntPtr(data1), width1);
                System.Array.ForEach(row1, action);
            }
            handle1.Free();
        }

        /// <summary>
        /// perform an generic operation based on the elements of the two images
        /// </summary>
        /// <typeparam name="TOtherDepth">The depth of the second image</typeparam>
        /// <param name="img2">The second image to perform action on</param>
        /// <param name="action">An action such that the first parameter is the a single channel of a pixel from the first image, the second parameter is the corresponding channel of the correspondind pixel from the second image </param>
        public void Action<TOtherDepth>(Image<TColor, TOtherDepth> img2, Emgu.Utils.Action<TDepth, TOtherDepth> action)
        {
            Debug.Assert(EqualSize(img2));

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
                Emgu.Utils.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                Emgu.Utils.memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
                for (int col = 0; col < cols1; action(row1[col], row2[col]), col++) ;
            }
            handle1.Free();
            handle2.Free();
        }

        ///<summary> Compute the element of a new image based on the value as well as the x and y positions of each pixel on the image</summary> 
        public Image<TColor, TOtherDepth> Convert<TOtherDepth>(Emgu.Utils.Func<TDepth, int, int, TOtherDepth> converter)
        {
            Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Width, Height);

            int nchannel = MIplImage.nChannels;

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
                Emgu.Utils.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                for (int col = 0; col < cols1; row2[col] = converter(row1[col], row, col / nchannel), col++) ;
                Emgu.Utils.memcpy((IntPtr)data2, handle2.AddrOfPinnedObject(), width2);
            }
            handle1.Free();
            handle2.Free();
            return res;
        }

        ///<summary> Compute the element of the new image based on element of this image</summary> 
        public Image<TColor, TOtherDepth> Convert<TOtherDepth>(System.Converter<TDepth, TOtherDepth> converter)
        {
            Image<TColor, TOtherDepth> res = new Image<TColor, TOtherDepth>(Width, Height);

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
                Emgu.Utils.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                for (int col = 0; col < cols1; row2[col] = converter(row1[col]), col++) ;
                Emgu.Utils.memcpy((IntPtr)data2, handle2.AddrOfPinnedObject(), width2);
            }
            handle1.Free();
            handle2.Free();
            return res;
        }

        ///<summary> Compute the element of the new image based on the elements of the two image</summary>
        public Image<TColor, TDepth3> Convert<TDepth2, TDepth3>(Image<TColor, TDepth2> img2, Emgu.Utils.Func<TDepth, TDepth2, TDepth3> converter)
        {
            Debug.Assert(EqualSize(img2), "Image size do not match");

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
                Emgu.Utils.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                Emgu.Utils.memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
                for (int col = 0; col < cols1; row3[col] = converter(row1[col], row2[col]), col++) ;
                Emgu.Utils.memcpy((IntPtr)data3, handle3.AddrOfPinnedObject(), width3);
            }

            handle1.Free();
            handle2.Free();
            handle3.Free();

            return res;
        }

        ///<summary> Compute the element of the new image based on the elements of the three image</summary>
        public Image<TColor, TDepth4> Convert<TDepth2, TDepth3, TDepth4>(Image<TColor, TDepth2> img2, Image<TColor, TDepth3> img3, Emgu.Utils.Func<TDepth, TDepth2, TDepth3, TDepth4> converter)
        {
            Debug.Assert(EqualSize(img2) && EqualSize(img3), "Image size do not match");

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
                Emgu.Utils.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                Emgu.Utils.memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
                Emgu.Utils.memcpy(handle3.AddrOfPinnedObject(), (IntPtr)data3, width3);

                for (int col = 0; col < cols1; row4[col] = converter(row1[col], row2[col], row3[col]), col++) ;

                Emgu.Utils.memcpy((IntPtr)data4, handle4.AddrOfPinnedObject(), width4);
            }
            handle1.Free();
            handle2.Free();
            handle3.Free();
            handle4.Free();

            return res;
        }

        ///<summary> Compute the element of the new image based on the elements of the four image</summary>
        public Image<TColor, TDepth5> Convert<TDepth2, TDepth3, TDepth4, TDepth5>(Image<TColor, TDepth2> img2, Image<TColor, TDepth3> img3, Image<TColor, TDepth4> img4, Emgu.Utils.Func<TDepth, TDepth2, TDepth3, TDepth4, TDepth5> converter)
        {
            Debug.Assert(EqualSize(img2) && EqualSize(img3) && EqualSize(img4), "Image size do not match");

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
                Emgu.Utils.memcpy(handle1.AddrOfPinnedObject(), (IntPtr)data1, width1);
                Emgu.Utils.memcpy(handle2.AddrOfPinnedObject(), (IntPtr)data2, width2);
                Emgu.Utils.memcpy(handle3.AddrOfPinnedObject(), (IntPtr)data3, width3);
                Emgu.Utils.memcpy(handle4.AddrOfPinnedObject(), (IntPtr)data4, width4);

                for (int col = 0; col < cols1; row5[col] = converter(row1[col], row2[col], row3[col], row4[col]), col++) ;
                Emgu.Utils.memcpy((IntPtr)data5, handle5.AddrOfPinnedObject(), width5);
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
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cvReleaseImageHeader(ref _ptr);
                _ptr = IntPtr.Zero;
            }

            base.DisposeObject();
        }
        #endregion

        #region Operator overload

        /// <summary>
        /// Perform an elementwise AND operation on the two images
        /// </summary>
        /// <param name="img1">The first image to AND</param>
        /// <param name="img2">The second image to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Image<TColor, TDepth> operator &(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
        {
            return img1.And(img2);
        }

        /// <summary>
        /// Perform an elementwise AND operation using an images and a color
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
        /// Perform an elementwise AND operation using an images and a color
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
        /// Perform an elementwise AND operation using an images and a color
        /// </summary>
        /// <param name="img1">The first image to AND</param>
        /// <param name="val">The color to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Image<TColor, TDepth> operator &(Image<TColor, TDepth> img1, TColor val)
        {
            return img1.And(val);
        }

        /// <summary>
        /// Perform an elementwise AND operation using an images and a color
        /// </summary>
        /// <param name="img1">The first image to AND</param>
        /// <param name="val">The color to AND</param>
        /// <returns>The result of the AND operation</returns>
        public static Image<TColor, TDepth> operator &(TColor val, Image<TColor, TDepth> img1)
        {
            return img1.And(val);
        }

        ///<summary> Perform an elementwise OR operation with another image and return the result</summary>
        ///<returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
        {
            return img1.Or(img2);
        }

        ///<summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        ///<param name="img1">The image to OR</param>
        ///<param name="val"> The color to OR</param>
        ///<returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, double val)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(val, val, val, val);
            return img1.Or(color);
        }

        ///<summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        ///<param name="img1">The image to OR</param>
        ///<param name="val"> The color to OR</param>
        ///<returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(double val, Image<TColor, TDepth> img1)
        {
            return img1 | val;
        }

        ///<summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        ///<param name="img1">The image to OR</param>
        ///<param name="val"> The color to OR</param>
        ///<returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(Image<TColor, TDepth> img1, TColor val)
        {
            return img1.Or(val);
        }

        ///<summary> 
        /// Perform an binary OR operation with some color
        /// </summary>
        ///<param name="img1">The image to OR</param>
        ///<param name="val"> The color to OR</param>
        ///<returns> The result of the OR operation</returns>
        public static Image<TColor, TDepth> operator |(TColor val, Image<TColor, TDepth> img1)
        {
            return img1.Or(val);
        }

        ///<summary> Compute the complement image</summary>
        public static Image<TColor, TDepth> operator ~(Image<TColor, TDepth> img1)
        {
            return img1.Not();
        }

        /// <summary>
        /// Elementwise add <paramref name="img1"/> with <paramref name="img2"/>
        /// </summary>
        /// <param name="img1">The first image to be added</param>
        /// <param name="img2">The second image to be added</param>
        /// <returns>The sum of the two images</returns>
        public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
        {
            return img1.Add(img2);
        }

        /// <summary>
        /// Elementwise add <paramref name="img1"/> with <paramref name="val"/>
        /// </summary>
        /// <param name="img1">The image to be added</param>
        /// <param name="val">The value to be added</param>
        /// <returns>The images plus the color</returns>
        public static Image<TColor, TDepth> operator +(double val, Image<TColor, TDepth> img1)
        {
            return img1 + val;
        }

        /// <summary>
        /// Elementwise add <paramref name="img1"/> with <paramref name="val"/>
        /// </summary>
        /// <param name="img1">The image to be added</param>
        /// <param name="val">The value to be added</param>
        /// <returns>The images plus the color</returns>
        public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> img1, double val)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(val, val, val, val);
            return img1.Add(color);
        }

        /// <summary>
        /// Elementwise add <paramref name="img1"/> with <paramref name="val"/>
        /// </summary>
        /// <param name="img1">The image to be added</param>
        /// <param name="val">The color to be added</param>
        /// <returns>The images plus the color</returns>
        public static Image<TColor, TDepth> operator +(Image<TColor, TDepth> img1, TColor val)
        {
            return img1.Add(val);
        }

        /// <summary>
        /// Elementwise add <paramref name="img1"/> with <paramref name="val"/>
        /// </summary>
        /// <param name="img1">The image to be added</param>
        /// <param name="val">The color to be added</param>
        /// <returns>The images plus the color</returns>
        public static Image<TColor, TDepth> operator +(TColor val, Image<TColor, TDepth> img1)
        {
            return img1.Add(val);
        }

        /// <summary>
        /// Elementwise subtract another image from the current image
        /// </summary>
        /// <param name="img1">The image to be substracted</param>
        /// <param name="img2">The second image to be subtraced from <paramref name="img1"/></param>
        /// <returns> The result of elementwise subtracting img2 from <paramref name="img1"/> </returns>
        public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> img1, Image<TColor, TDepth> img2)
        {
            return img1.Sub(img2);
        }

        /// <summary>
        /// Elementwise subtract another image from the current image
        /// </summary>
        /// <param name="img1">The image to be substracted</param>
        /// <param name="val">The color to be subtracted</param>
        /// <returns> The result of elementwise subtracting <paramred name="val"/> from <paramref name="img1"/> </returns>
        public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> img1, TColor val)
        {
            return img1.Sub(val);
        }

        /// <summary>
        /// Elementwise subtract another image from the current image
        /// </summary>
        /// <param name="img1">The image to be substracted</param>
        /// <param name="val">The color to be subtracted</param>
        /// <returns> <paramred name="val"/> - <paramref name="img1"/> </returns>
        public static Image<TColor, TDepth> operator -(TColor val, Image<TColor, TDepth> img1)
        {
            return img1.SubR(val);
        }

        /// <summary>
        /// <paramred name="val"/> - <paramref name="img1"/>
        /// </summary>
        /// <param name="img1">The image to be substracted</param>
        /// <param name="val">The value to be subtracted</param>
        /// <returns> <paramred name="val"/> - <paramref name="img1"/> </returns>
        public static Image<TColor, TDepth> operator -(double val, Image<TColor, TDepth> img1)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(val, val, val, val);
            return img1.SubR(color);
        }

        /// <summary>
        /// Elementwise subtract another image from the current image
        /// </summary>
        /// <param name="img1">The image to be substracted</param>
        /// <param name="val">The value to be subtracted</param>
        /// <returns> <paramref name="img1"/> - <paramred name="val"/>   </returns>
        public static Image<TColor, TDepth> operator -(Image<TColor, TDepth> img1, double val)
        {
            TColor color = new TColor();
            color.MCvScalar = new MCvScalar(val, val, val, val);
            return img1.Sub(color);
        }

        /// <summary>
        ///  <paramref name="img1"/> * <paramref name="scale"/>
        /// </summary>
        /// <param name="img1">The image</param>
        /// <param name="scale">The multiplication scale</param>
        /// <returns><paramref name="img1"/> * <paramref name="scale"/></returns>
        public static Image<TColor, TDepth> operator *(Image<TColor, TDepth> img1, double scale)
        {
            return img1.Mul(scale);
        }

        /// <summary>
        ///   <paramref name="scale"/>*<paramref name="img1"/>
        /// </summary>
        /// <param name="img1">The image</param>
        /// <param name="scale">The multiplication scale</param>
        /// <returns><paramref name="scale"/>*<paramref name="img1"/></returns>
        public static Image<TColor, TDepth> operator *(double scale, Image<TColor, TDepth> img1)
        {
            return img1.Mul(scale);
        }

        /// <summary>
        /// Perform the convolution with <paramref name="kernel"/> on <paramref name="img1"/>
        /// </summary>
        /// <param name="img1">The image</param>
        /// <param name="kernel">The kernel</param>
        /// <returns>Result of the convolution</returns>
        public static Image<TColor, Single> operator *(Image<TColor, TDepth> img1, ConvolutionKernelF kernel)
        {
            return img1.Convolution(kernel);
        }

        /// <summary>
        ///  <paramref name="img1"/> / <paramref name="scale"/>
        /// </summary>
        /// <param name="img1">The image</param>
        /// <param name="scale">The division scale</param>
        /// <returns><paramref name="img1"/> / <paramref name="scale"/></returns>
        public static Image<TColor, TDepth> operator /(Image<TColor, TDepth> img1, double scale)
        {
            return img1.Mul(1.0 / scale);
        }

        /// <summary>
        ///   <paramref name="scale"/> / <paramref name="img1"/>
        /// </summary>
        /// <param name="img1">The image</param>
        /// <param name="scale">The scale</param>
        /// <returns><paramref name="scale"/> / <paramref name="img1"/></returns>
        public static Image<TColor, TDepth> operator /(double scale, Image<TColor, TDepth> img1)
        {
            Image<TColor, TDepth> res = img1.BlankClone();
            CvInvoke.cvDiv(IntPtr.Zero, img1.Ptr, res.Ptr, scale);
            return res;
        }

        #endregion

        #region Filters
        ///<summary> performs a convolution using the provided kernel </summary>
        public Image<TColor, Single> Convolution(ConvolutionKernelF kernel)
        {
            bool isFloat = (typeof(TDepth) == typeof(Single));

            Emgu.Utils.Action<IntPtr, IntPtr, int> act =
                delegate(IntPtr src, IntPtr dest, int channel)
                {
                    IntPtr srcFloat = src;
                    if (!isFloat)
                    {
                        srcFloat = CvInvoke.cvCreateImage(new MCvSize(Width, Height), CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 1);
                        CvInvoke.cvConvertScale(src, srcFloat, 1.0, 0.0);
                    }

                    //perform the convolution operation
                    CvInvoke.cvFilter2D(
                        srcFloat,
                        dest,
                        kernel.Ptr,
                        kernel.Center);

                    if (!isFloat)
                    {
                        CvInvoke.cvReleaseImage(ref srcFloat);
                    }
                };

            Image<TColor, Single> res = new Image<TColor, Single>(Width, Height);
            ForEachDuplicateChannel(act, res);

            return res;
        }

        #region Gaussian Smooth
        ///<summary> Perform Gaussian Smoothing in the current image and return the result </summary>
        ///<param name="kernelSize"> The size of the Gaussian kernel (<paramref>kernelSize</paramref> x <paramref>kernelSize</paramref>)</param>
        ///<returns> The smoothed image</returns>
        public Image<TColor, TDepth> GaussianSmooth(int kernelSize) { return GaussianSmooth(kernelSize, 0, 0); }

        ///<summary> Perform Gaussian Smoothing in the current image and return the result </summary>
        ///<param name="kernelWidth"> The width of the Gaussian kernel</param>
        ///<param name="kernelHeight"> The height of the Gaussian kernel</param>
        ///<param name="sigma"> The standard deviation of the Gaussian kernel</param>
        ///<returns> The smoothed image</returns>
        public Image<TColor, TDepth> GaussianSmooth(int kernelWidth, int kernelHeight, double sigma)
        {
            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvSmooth(Ptr, res.Ptr, CvEnum.SMOOTH_TYPE.CV_GAUSSIAN, kernelWidth, kernelHeight, sigma, 0);
            return res;
        }

        ///<summary> Perform Gaussian Smoothing inplace for the current image </summary>
        ///<param name="kernelSize"> The size of the Gaussian kernel (<paramref>kernelSize</paramref> x <paramref>kernelSize</paramref>)</param>
        public void _GaussianSmooth(int kernelSize)
        {
            _GaussianSmooth(kernelSize, 0, 0);
        }

        ///<summary> Perform Gaussian Smoothing inplace for the current image </summary>
        ///<param name="kernelWidth"> The width of the Gaussian kernel</param>
        ///<param name="kernelHeight"> The height of the Gaussian kernel</param>
        ///<param name="sigma"> The standard deviation of the Gaussian kernel</param>
        public void _GaussianSmooth(int kernelWidth, int kernelHeight, double sigma)
        {
            CvInvoke.cvSmooth(Ptr, Ptr, CvEnum.SMOOTH_TYPE.CV_GAUSSIAN, kernelWidth, kernelHeight, sigma, 0);
        }
        #endregion

        #region Threshold methods
        ///<summary> 
        ///the base threshold method shared by public threshold functions 
        ///</summary>
        private void ThresholdBase(Image<TColor, TDepth> dest, TColor threshold, TColor max_value, CvEnum.THRESH thresh_type)
        {
            double[] t = threshold.Resize(4).Coordinate;
            double[] m = max_value.Resize(4).Coordinate;
            Emgu.Utils.Action<IntPtr, IntPtr, int> act =
                delegate(IntPtr src, IntPtr dst, int channel)
                {
                    CvInvoke.cvThreshold(src, dst, t[channel], m[channel], thresh_type);
                };
            ForEachDuplicateChannel<TDepth>(act, dest);
        }

        ///<summary> Threshold the image such that: dst(x,y) = src(x,y), if src(x,y)>threshold;  0, otherwise </summary>
        ///<returns> dst(x,y) = src(x,y), if src(x,y)>threshold;  0, otherwise </returns>
        public Image<TColor, TDepth> ThresholdToZero(TColor threshold)
        {
            Image<TColor, TDepth> res = BlankClone();
            ThresholdBase(res, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TOZERO);
            return res;
        }

        ///<summary> Threshold the image such that: dst(x,y) = 0, if src(x,y)>threshold;  src(x,y), otherwise </summary>
        public Image<TColor, TDepth> ThresholdToZeroInv(TColor threshold)
        {
            Image<TColor, TDepth> res = BlankClone();
            ThresholdBase(res, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TOZERO_INV);
            return res;
        }
        ///<summary> Threshold the image such that: dst(x,y) = threshold, if src(x,y)>threshold; src(x,y), otherwise </summary>
        public Image<TColor, TDepth> ThresholdTrunc(TColor threshold)
        {
            Image<TColor, TDepth> res = BlankClone();
            ThresholdBase(res, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TRUNC);
            return res;
        }

        ///<summary> Threshold the image such that: dst(x,y) = max_value, if src(x,y)>threshold; 0, otherwise </summary>
        public Image<TColor, TDepth> ThresholdBinary(TColor threshold, TColor maxValue)
        {
            Image<TColor, TDepth> res = BlankClone();
            ThresholdBase(res, threshold, maxValue, CvEnum.THRESH.CV_THRESH_BINARY);
            return res;
        }

        ///<summary> Threshold the image such that: dst(x,y) = 0, if src(x,y)>threshold;  max_value, otherwise </summary>
        public Image<TColor, TDepth> ThresholdBinaryInv(TColor threshold, TColor maxValue)
        {
            Image<TColor, TDepth> res = BlankClone();
            ThresholdBase(res, threshold, maxValue, CvEnum.THRESH.CV_THRESH_BINARY_INV);
            return res;
        }

        ///<summary> Threshold the image inplace such that: dst(x,y) = src(x,y), if src(x,y)>threshold;  0, otherwise </summary>
        public void _ThresholdToZero(TColor threshold)
        {
            ThresholdBase(this, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TOZERO);
        }
        ///<summary> Threshold the image inplace such that: dst(x,y) = 0, if src(x,y)>threshold;  src(x,y), otherwise </summary>
        public void _ThresholdToZeroInv(TColor threshold)
        {
            ThresholdBase(this, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TOZERO_INV);
        }
        ///<summary> Threshold the image inplace such that: dst(x,y) = threshold, if src(x,y)>threshold; src(x,y), otherwise </summary>
        public void _ThresholdTrunc(TColor threshold)
        {
            ThresholdBase(this, threshold, new TColor(), CvEnum.THRESH.CV_THRESH_TRUNC);
        }
        ///<summary> Threshold the image inplace such that: dst(x,y) = max_value, if src(x,y)>threshold; 0, otherwise </summary>
        public void _ThresholdBinary(TColor threshold, TColor max_value)
        {
            ThresholdBase(this, threshold, max_value, CvEnum.THRESH.CV_THRESH_BINARY);
        }
        ///<summary> Threshold the image inplace such that: dst(x,y) = 0, if src(x,y)>threshold;  max_value, otherwise </summary>
        public void _ThresholdBinaryInv(TColor threshold, TColor max_value)
        {
            ThresholdBase(this, threshold, max_value, CvEnum.THRESH.CV_THRESH_BINARY_INV);
        }
        #endregion
        #endregion

        #region various
        ///<summary> Return a filpped copy of the current image</summary>
        ///<param name="flipType">The type of the flipping</param>
        ///<returns> The flipped copy of <i>this</i> image </returns>
        public Image<TColor, TDepth> Flip(CvEnum.FLIP flipType)
        {
            if (flipType == Emgu.CV.CvEnum.FLIP.NONE) return Clone();

            //code = 0 indicates vertical flip only
            int code = 0;
            //code = -1 indicates vertical and horizontal flip
            if (flipType == (Emgu.CV.CvEnum.FLIP.HORIZONTAL | Emgu.CV.CvEnum.FLIP.VERTICAL)) code = -1;
            //code = 1 indicates horizontal flip only
            else if (flipType == Emgu.CV.CvEnum.FLIP.HORIZONTAL) code = 1;

            Image<TColor, TDepth> res = BlankClone();
            CvInvoke.cvFlip(Ptr, res.Ptr, code);
            return res;
        }

        /// <summary>
        /// Calculates spatial and central moments up to the third order and writes them to moments. The moments may be used then to calculate gravity center of the shape, its area, main axises and various shape characeteristics including 7 Hu invariants.
        /// </summary>
        /// <param name="binary">If the flag is true, all the zero pixel values are treated as zeroes, all the others are treated as 1?s</param>
        /// <returns>spatial and central moments up to the third order</returns>
        public MCvMoments Moments(bool binary)
        {
            MCvMoments m = new MCvMoments();
            CvInvoke.cvMoments(Ptr, ref m, binary ? 1 : 0);
            return m;
        }

        ///<summary> 
        ///Split current Image into an array of gray scale images where each element 
        ///in the array represent a single color channel of the original image
        ///</summary>
        ///<returns> 
        ///An array of gray scale images where each element 
        ///in the array represent a single color channel of the original image 
        ///</returns>
        public Image<Gray, TDepth>[] Split()
        {
            int channelCount = new TColor().Dimension;
            if (channelCount == 1) return new Image<Gray, TDepth>[] { Clone() as Image<Gray, TDepth> };

            Image<Gray, TDepth>[] res = new Image<Gray, TDepth>[channelCount];
            IntPtr[] a = new IntPtr[4];
            a.Initialize();
            for (int i = 0; i < channelCount; i++)
            {
                res[i] = new Image<Gray, TDepth>(Width, Height);
                a[i] = res[i].Ptr;
            }

            CvInvoke.cvSplit(Ptr, a[0], a[1], a[2], a[3]);

            return res;
        }

        /// <summary>
        /// Returns the min / max location and values for the image
        /// </summary>
        /// <returns>
        /// Returns the min / max location and values for the image
        /// </returns>
        public void MinMax(out double[] minValues, out double[] maxValues, out MCvPoint[] minLocations, out MCvPoint[] maxLocations)
        {
            int channelCount = new TColor().Dimension;
            minValues = new double[channelCount];
            maxValues = new double[channelCount];
            minLocations = new MCvPoint[channelCount];
            maxLocations = new MCvPoint[channelCount];

            if (channelCount == 1)
            {
                CvInvoke.cvMinMaxLoc(Ptr, ref minValues[0], ref maxValues[0], ref minLocations[0], ref maxLocations[0], IntPtr.Zero);
            }
            else
            {
                for (int i = 0; i < channelCount; i++)
                {
                    CvInvoke.cvSetImageCOI(Ptr, i + 1);
                    CvInvoke.cvMinMaxLoc(Ptr, ref minValues[i], ref maxValues[i], ref minLocations[i], ref maxLocations[i], IntPtr.Zero);
                }
                CvInvoke.cvSetImageCOI(Ptr, 0);
            }
        }

        /// <summary>
        /// Save this image to the specific file
        /// </summary>
        /// <param name="fileName">The name of the file to be saved to</param>
        public void Save(String fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            String[] cvFormats = new string[] { ".jpg", ".jpeg", ".png", ".bmp" };
            if (System.Array.Exists(cvFormats, delegate(String s) { return s.Equals(fi.Extension.ToLower()); }))
            {   //if the file can be imported from Open CV
                CvInvoke.cvSaveImage(fileName, Ptr);
            }
            else
            {
                String[] bmpFormats = new String[] { ".gif" };
                if (System.Array.Exists(bmpFormats, delegate(String s) { return s.Equals(fi.Extension.ToLower()); }))
                {
                    Bitmap.Save(fileName);
                }
                else
                {
                    throw new NotImplementedException(String.Format("Saving to {0} Format is not implemented", fi.Extension));
                }
            }
        }

        /// <summary>
        /// The algorithm normalizes brightness and increases contrast of the image
        /// </summary>
        public void _EqualizeHist()
        {
            //TODO: handle multiple channel as well
            CvInvoke.cvEqualizeHist(Ptr, Ptr);
        }
        #endregion

        #region IImage
        Type IImage.TypeOfDepth
        {
            get
            {
                return typeof(TDepth);
            }
        }

        Type IImage.TypeOfColor
        {
            get
            {
                return typeof(TColor);
            }
        }

        IImage IImage.PyrUp()
        {
            return (IImage)PyrUp();
        }

        IImage IImage.PyrDown()
        {
            return (IImage)PyrDown();
        }

        IImage IImage.Laplace(int apertureSize)
        {
            return (IImage)Laplace(apertureSize);
        }

        IImage IImage.ToGray()
        {
            return (IImage)Convert<Gray, TDepth>();
        }

        IImage IImage.Resize(int width, int height)
        {
            return (IImage)Resize(width, height);
        }

        IImage IImage.Resize(double scale)
        {
            return (IImage)Resize(scale);
        }

        ColorType IImage.GetColor(Point2D<int> location)
        {
            return (ColorType)this[location];
        }

        IImage IImage.Canny(MCvScalar thresh, MCvScalar threshLinking)
        {
            TColor threshColor = new TColor();
            threshColor.MCvScalar = thresh;
            TColor threshLinkingColor = new TColor();
            threshLinkingColor.MCvScalar = threshLinking;

            return (IImage)Canny(threshColor, threshLinkingColor);
        }

        IImage IImage.Sobel(int xorder, int yorder, int apertureSize)
        {
            return (IImage)Sobel(xorder, yorder, apertureSize);
        }

        IImage IImage.Rotate(double angle, MCvScalar background, bool crop)
        {
            TColor backgroundColor = new TColor();
            backgroundColor.MCvScalar = background;
            return (IImage)Rotate(angle, backgroundColor, crop);
        }

        IImage IImage.Flip(CvEnum.FLIP flipType)
        {
            return (IImage)Flip(flipType);
        }

        IImage IImage.DFT(CvEnum.CV_DXT type, int nonzeroRows)
        {
            return (IImage)DFT(type, nonzeroRows);
        }

        IImage IImage.DCT(CvEnum.CV_DCT_TYPE type)
        {
            return (IImage)DCT(type);
        }

        IImage IImage.ToSingle()
        {
            return (IImage)Convert<TColor, Single>();
        }

        IImage IImage.ToByte()
        {
            return (IImage)Convert<TColor, Byte>();
        }

        IImage[] IImage.Split()
        {
            return Array.ConvertAll<Image<Gray, TDepth>, IImage>(Split(), delegate(Image<Gray, TDepth> img) { return (IImage)img; });
        }

        int IImage.NumberOfChannel
        {
            get
            {
                return new TColor().Dimension;
            }
        }
        #endregion

    }
}
