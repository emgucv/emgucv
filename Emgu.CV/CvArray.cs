//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;
using Emgu.CV.Reflection;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   ///<summary>
   ///Wrapped CvArr 
   ///</summary>
   ///<typeparam name="TDepth">The type of elements in this CvArray</typeparam>
   public abstract class CvArray<TDepth> : UnmanagedObject, IXmlSerializable, ISerializable where TDepth : new()
   {
      /// <summary>
      /// The size of the elements in the CvArray, it is the cached value of Marshal.SizeOf(typeof(TDepth)).
      /// </summary>
      private static readonly int _sizeOfElement = Marshal.SizeOf(typeof(TDepth));

      /// <summary>
      /// The pinned GCHandle to _array;
      /// </summary>
      protected GCHandle _dataHandle;

      private int _serializationCompressionRatio;

      /// <summary>
      /// Get or set the Compression Ratio for serialization. A number between 0 - 9. 
      /// 0 means no compression at all, while 9 means best compression
      /// </summary>
      public int SerializationCompressionRatio
      {
         get { return _serializationCompressionRatio; }
         set
         {
            Debug.Assert(0 <= value && value <= 9, "Compression ratio must >=0 and <=9");
            _serializationCompressionRatio = value;
         }
      }

      /// <summary>
      /// Get the size of element in bytes
      /// </summary>
      public static int SizeOfElement
      {
         get
         {
            return _sizeOfElement;
         }
      }

      #region properties
      ///<summary> The pointer to the internal structure </summary>
      public new IntPtr Ptr
      {
         get { return _ptr; }
         set { _ptr = value; }
      }

      ///<summary> 
      /// Get the size of the array
      ///</summary>
      public System.Drawing.Size Size
      {
         get
         {
            return CvInvoke.cvGetSize(_ptr);
         }
      }

      ///<summary> 
      ///Get the width (#Cols) of the cvArray.
      ///If ROI is set, the width of the ROI 
      ///</summary>
      public int Width { get { return Size.Width; } }

      ///<summary> 
      ///Get the height (#Rows) of the cvArray.
      ///If ROI is set, the height of the ROI 
      ///</summary> 
      public int Height { get { return Size.Height; } }

      /// <summary>
      /// Get the number of channels of the array
      /// </summary>
      public abstract int NumberOfChannels { get;}

      /// <summary>
      /// The number of rows for this array
      /// </summary>
      public int Rows { get { return Height; } }

      /// <summary>
      /// The number of cols for this array
      /// </summary>
      public int Cols { get { return Width; } }

      /// <summary>
      /// Get or Set an Array of bytes that represent the data in this array
      /// </summary>
      /// <remarks> Should only be used for serialization &amp; deserialization</remarks>
      [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
      public Byte[] Bytes
      {
         get
         {
            int size;
            IntPtr dataStart;

            if (_dataHandle.IsAllocated)
            {
               size = _sizeOfElement * ManagedArray.Length;
               dataStart = _dataHandle.AddrOfPinnedObject();
            }
            else if (this is Matrix<TDepth>)
            {
               Matrix<TDepth> matrix = (Matrix<TDepth>)this;
               MCvMat mat = matrix.MCvMat;
               if (mat.step == 0)
               {  //The matrix only have one row
                  size = mat.cols * NumberOfChannels * Marshal.SizeOf(typeof(TDepth));
               }
               else
                  size = mat.rows * mat.step;
               dataStart = mat.data;
            }
            else if (this is MatND<TDepth>)
            {
               throw new NotImplementedException("Getting Bytes from Pinned MatND is not implemented");
            }
            else
            {  //this is Image<TColor, TDepth>
               MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(Ptr, typeof(MIplImage));
               size = iplImage.height * iplImage.widthStep;
               dataStart = iplImage.imageData;
            }
            Byte[] data = new Byte[size];
            Marshal.Copy(dataStart, data, 0, size);

            if (SerializationCompressionRatio == 0)
            {
               return data;
            }
            else
            {
               return ZlibCompression.Compress(data, SerializationCompressionRatio);
        
               /*
               using (MemoryStream ms = new MemoryStream())
               {

                  //using (GZipStream compressedStream = new GZipStream(ms, CompressionMode.Compress))
                  using (ZOutputStream compressedStream = new ZOutputStream(ms, SerializationCompressionRatio))
                  {
                     compressedStream.Write(data, 0, data.Length);
                     compressedStream.Flush();
                  }
                  return ms.ToArray();
               }*/
            }

         }
         set
         {
            Byte[] bytes;
            int size = _sizeOfElement * ManagedArray.Length;

            if (SerializationCompressionRatio == 0)
            {
               bytes = value;
            }
            else
            {
               try
               {  //try to use zlib to decompressed the data
                  bytes = ZlibCompression.Uncompress(value, size);
                  /*
                  using (MemoryStream ms = new MemoryStream())
                  {
                     using (ZOutputStream stream = new ZOutputStream(ms))
                     {
                        stream.Write(value, 0, value.Length);
                        stream.Flush();
                     }
                     bytes = ms.ToArray();
                  }*/
               }
               catch
               {  //if using zlib decompression fails, try to use .NET GZipStream to decompress

                  using (MemoryStream ms = new MemoryStream(value))
                  {
                     //ms.Position = 0;
                     using (GZipStream stream = new GZipStream(ms, CompressionMode.Decompress))
                     {
                        bytes = new Byte[size];
                        stream.Read(bytes, 0, size);
                     }
                  }
               }
            }

            Marshal.Copy(bytes, 0, _dataHandle.AddrOfPinnedObject(), size);
         }
      }

      /// <summary>
      /// Get the underneath managed array
      /// </summary>
      public abstract Array ManagedArray
      {
         get;
         set;
      }

      /// <summary>
      /// Allocate data for the array
      /// </summary>
      /// <param name="rows">The number of rows</param>
      /// <param name="cols">The number of columns</param>
      /// <param name="numberOfChannels">The number of channels of this cvArray</param>
      protected abstract void AllocateData(int rows, int cols, int numberOfChannels);

      /// <summary>
      /// Sum of diagonal elements of the matrix 
      /// </summary>
      [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
      public MCvScalar Trace
      {
         get
         {
            return CvInvoke.cvTrace(Ptr);
         }
      }

      ///<summary> 
      ///The norm of this Array 
      ///</summary>
      [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
      public double Norm
      {
         get
         {
            return CvInvoke.cvNorm(Ptr, IntPtr.Zero, CvEnum.NORM_TYPE.CV_L2, IntPtr.Zero);
         }
      }
      #endregion

      /// <summary>
      /// Calculates and returns the Euclidean dot product of two arrays.
      /// src1 dot src2 = sumI(src1(I)*src2(I))
      /// </summary>
      /// <remarks>In case of multiple channel arrays the results for all channels are accumulated. In particular, cvDotProduct(a,a), where a is a complex vector, will return ||a||^2. The function can process multi-dimensional arrays, row by row, layer by layer and so on.</remarks>
      /// <param name="src2">The other Array to apply dot product with</param>
      /// <returns>src1 dot src2</returns>
      public double DotProduct(CvArray<TDepth> src2)
      {
         return CvInvoke.cvDotProduct(Ptr, src2.Ptr);
      }

      /// <summary>
      /// Check that every array element is neither NaN nor +- inf. The functions also check that each value
      /// is between <paramref name="min"/> and <paramref name="max"/>. in the case of multi-channel arrays each channel is processed
      /// independently. If some values are out of range, position of the first outlier is stored in pos, 
      /// and then the functions return false.
      /// </summary>
      /// <param name="min">The inclusive lower boundary of valid values range</param>
      /// <param name="max">The exclusive upper boundary of valid values range</param>
      /// <param name="position">This will be filled with the position of the first outlier</param>
      /// <returns>True if all values are in range</returns>
      public bool CheckRange(double min, double max, ref System.Drawing.Point position)
      {
         return CvInvoke.cvCheckRange(_ptr, true, ref position, min, max);
      }

      #region statistic
      /// <summary>
      /// Reduces matrix to a vector by treating the matrix rows/columns as a set of 1D vectors and performing the specified operation on the vectors until a single row/column is obtained. 
      /// </summary>
      /// <remarks>
      /// The function can be used to compute horizontal and vertical projections of an raster image. 
      /// In case of CV_REDUCE_SUM and CV_REDUCE_AVG the output may have a larger element bit-depth to preserve accuracy. 
      /// And multi-channel arrays are also supported in these two reduction modes
      /// </remarks>
      /// <param name="array1D">The destination single-row/single-column vector that accumulates somehow all the matrix rows/columns</param>
      /// <param name="dim">The dimension index along which the matrix is reduce.</param>
      /// <param name="type">The reduction operation type</param>
      /// <typeparam name="TOtherDepth">The type of depth of the reduced array</typeparam>
      public void Reduce<TOtherDepth>(CvArray<TOtherDepth> array1D, CvEnum.REDUCE_DIMENSION dim, CvEnum.REDUCE_TYPE type)
         where TOtherDepth : new ()
      {
         CvInvoke.cvReduce(Ptr, array1D.Ptr, dim, type);
      }
      #endregion

      #region Coping and filling
      ///<summary>
      /// Copy the current array to <paramref name="dest"/>
      /// </summary>
      /// <param name="dest"> The destination Array</param>
      public void CopyTo(CvArray<TDepth> dest)
      {
         CvInvoke.cvCopy(Ptr, dest.Ptr, IntPtr.Zero);
      }

      ///<summary> 
      ///Set the element of the Array to <paramref name="val"/>
      ///</summary>
      ///<param name="val"> The value to be set for each element of the Array </param>
      public void SetValue(MCvScalar val)
      {
         CvInvoke.cvSet(_ptr, val, IntPtr.Zero);
      }

      ///<summary> 
      ///Set the element of the Array to <paramref name="val"/>
      ///</summary>
      ///<param name="val"> The value to be set for each element of the Array </param>
      public void SetValue(double val)
      {
         SetValue(new MCvScalar(val, val, val, val));
      }

      ///<summary>
      ///Set the element of the Array to <paramref name="val"/>, using the specific <paramref name="mask"/>
      ///</summary>
      ///<param name="val">The value to be set</param>
      ///<param name="mask">The mask for the operation</param>
      public void SetValue(MCvScalar val, CvArray<Byte> mask)
      {
         CvInvoke.cvSet(_ptr, val, mask == null ? IntPtr.Zero : mask.Ptr);
      }

      ///<summary>
      ///Set the element of the Array to <paramref name="val"/>, using the specific <paramref name="mask"/>
      ///</summary>
      ///<param name="val">The value to be set</param>
      ///<param name="mask">The mask for the operation</param>
      public void SetValue(double val, CvArray<Byte> mask)
      {
         SetValue(new MCvScalar(val, val, val, val), mask);
      }

      private readonly static Random _randomGenerator = new Random();

      /// <summary>
      /// Inplace fills Array with uniformly distributed random numbers
      /// </summary>
      /// <param name="seed">Seed for the random number generator</param>
      /// <param name="floorValue">the inclusive lower boundary of random numbers range</param>
      /// <param name="ceilingValue">the exclusive upper boundary of random numbers range</param>
      public void SetRandUniform(UInt64 seed, MCvScalar floorValue, MCvScalar ceilingValue)
      {
         CvInvoke.cvRandArr(ref seed, Ptr, CvEnum.RAND_TYPE.CV_RAND_UNI, floorValue, ceilingValue);
      }

      /// <summary>
      /// Inplace fills Array with uniformly distributed random numbers
      /// </summary>
      /// <param name="floorValue">the inclusive lower boundary of random numbers range</param>
      /// <param name="ceilingValue">the exclusive upper boundary of random numbers range</param>
      [ExposableMethod(Exposable = true)]
      public void SetRandUniform(MCvScalar floorValue, MCvScalar ceilingValue)
      {
         SetRandUniform((UInt64)_randomGenerator.Next(), floorValue, ceilingValue);
      }

      /// <summary>
      /// Inplace fills Array with normally distributed random numbers
      /// </summary>
      /// <param name="seed">Seed for the random number generator</param>
      /// <param name="mean">the mean value of random numbers</param>
      /// <param name="std"> the standard deviation of random numbers</param>
      public void SetRandNormal(UInt64 seed, MCvScalar mean, MCvScalar std)
      {
         CvInvoke.cvRandArr(ref seed, Ptr, CvEnum.RAND_TYPE.CV_RAND_NORMAL, mean, std);
      }

      /// <summary>
      /// Inplace fills Array with normally distributed random numbers
      /// </summary>
      /// <param name="mean">the mean value of random numbers</param>
      /// <param name="std"> the standard deviation of random numbers</param>
      [ExposableMethod(Exposable = true)]
      public void SetRandNormal(MCvScalar mean, MCvScalar std)
      {
         SetRandNormal((UInt64)_randomGenerator.Next(), mean, std);
      }

      /// <summary>
      /// Initializs scaled identity matrix
      /// </summary>
      /// <param name="value">The value on the diagonal</param>
      public void SetIdentity(MCvScalar value)
      {
         CvInvoke.cvSetIdentity(Ptr, value);
      }

      /// <summary>
      /// Set the values to zero
      /// </summary>
      public void SetZero()
      {
         CvInvoke.cvSetZero(Ptr);
      }

      /// <summary>
      /// Initialize the identity matrix
      /// </summary>
      public void SetIdentity()
      {
         SetIdentity(new MCvScalar(1.0, 1.0, 1.0, 1.0));
      }

      #endregion

      #region Inplace Arithmatic
      /// <summary>
      /// Inplace multiply elements of the Array by <paramref name="scale"/>
      /// </summary>
      /// <param name="scale">The scale to be multiplyed</param>
      public void _Mul(double scale)
      {
         CvInvoke.cvConvertScale(Ptr, Ptr, scale, 0.0);
      }

      /// <summary>
      /// Inplace elementwise multiply the current Array with <paramref name="src2"/>
      /// </summary>
      /// <param name="src2">The other array to be elementwise multiplied with</param>
      public void _Mul(CvArray<TDepth> src2)
      {
         CvInvoke.cvMul(Ptr, src2.Ptr, Ptr, 1.0);
      }
      #endregion

      #region UnmanagedObject
      /// <summary>
      /// Free the _dataHandle if it is set
      /// </summary>
      protected override void DisposeObject()
      {
         if (_dataHandle.IsAllocated)
            _dataHandle.Free();
      }
      #endregion

      #region Inplace Comparison
      ///<summary>
      ///Inplace compute the elementwise minimum value 
      ///</summary>
      ///<param name="value">The value to compare with</param>
      [ExposableMethod(Exposable = true, Category = "Logic")]
      public void _Min(double value)
      {
         CvInvoke.cvMinS(Ptr, value, Ptr);
      }

      /// <summary>
      /// Inplace elementwise minimize the current Array with <paramref name="other"/>
      /// </summary>
      /// <param name="other">The other array to be elementwise minimized with this array</param>
      public void _Min(CvArray<TDepth> other)
      {
         CvInvoke.cvMin(Ptr, other.Ptr, Ptr);
      }

      /// <summary>
      /// Inplace compute the elementwise maximum value with <paramref name="value"/>
      /// </summary>
      /// <param name="value">The value to be compare with</param>
      [ExposableMethod(Exposable = true, Category = "Logic")]
      public void _Max(double value)
      {
         CvInvoke.cvMaxS(Ptr, value, Ptr);
      }

      /// <summary>
      /// Inplace elementwise maximize the current Array with <paramref name="other"/>
      /// </summary>
      /// <param name="other">The other array to be elementwise maximized with this array</param>
      public void _Max(CvArray<TDepth> other)
      {
         CvInvoke.cvMax(Ptr, other.Ptr, Ptr);
      }
      #endregion

      #region Inplace Logic Operators
      /// <summary>
      /// Inplace And operation with <paramref name="src2"/>
      /// </summary>
      /// <param name="src2">The other array to perform AND operation</param>
      public void _And(CvArray<TDepth> src2)
      {
         CvInvoke.cvAnd(Ptr, src2.Ptr, Ptr, IntPtr.Zero);
      }

      /// <summary>
      /// Inplace Or operation with <paramref name="src2"/>
      /// </summary>
      /// <param name="src2">The other array to perform OR operation</param>
      public void _Or(CvArray<TDepth> src2)
      {
         CvInvoke.cvOr(Ptr, src2.Ptr, Ptr, IntPtr.Zero);
      }

      ///<summary> 
      ///Inplace compute the complement for all array elements
      ///</summary>
      [ExposableMethod(Exposable = true, Category = "Logic")]
      public void _Not()
      {
         CvInvoke.cvNot(Ptr, Ptr);
      }

      #endregion

      #region File IO
      /// <summary>
      /// Save the CvArray as image
      /// </summary>
      /// <param name="fileName">The name of the image to save</param>
      public virtual void Save(String fileName)
      {
         //FileInfo fi = new FileInfo(fileName);
         CvInvoke.cvSaveImage(fileName, Ptr, IntPtr.Zero);
      }
      #endregion

      #region IXmlSerializable Members

      /// <summary>
      /// Get the xml schema
      /// </summary>
      /// <returns>the xml schema</returns>
      public System.Xml.Schema.XmlSchema GetSchema()
      {
         throw new NotImplementedException("The method or operation is not implemented.");
      }

      /// <summary>
      /// Function to call when deserializing this object from XML
      /// </summary>
      /// <param name="reader">The xml reader</param>
      public virtual void ReadXml(System.Xml.XmlReader reader)
      {
         #region read the size of the matrix and assign storage
         int rows = Int32.Parse( reader.GetAttribute("Rows") );
         int cols = Int32.Parse( reader.GetAttribute("Cols"));
         int numberOfChannels = Int32.Parse( reader.GetAttribute("NumberOfChannels"));
         AllocateData(rows, cols, numberOfChannels);
         #endregion
         
         SerializationCompressionRatio = Int32.Parse (reader.GetAttribute ("CompressionRatio"));

         #region decode the data from Xml and assign the value to the matrix
         reader.MoveToContent();
         reader.ReadToFollowing("Bytes");
         int size = _sizeOfElement * ManagedArray.Length;
         if (SerializationCompressionRatio == 0)
         {
            Byte[] bytes = new Byte[size];
            reader.ReadElementContentAsBase64(bytes, 0, bytes.Length);
            Bytes = bytes;
         }
         else
         {
            int extraHeaderBytes = 20000;
            Byte[] bytes = new Byte[size + extraHeaderBytes];
            int countOfBytesRead = reader.ReadElementContentAsBase64(bytes, 0, bytes.Length);
            Array.Resize<Byte>(ref bytes, countOfBytesRead);
            Bytes = bytes;
         }
         //reader.MoveToElement();
         #endregion
      }

      /// <summary>
      /// Function to call when serializing this object to XML 
      /// </summary>
      /// <param name="writer">The xml writer</param>
      public virtual void WriteXml(System.Xml.XmlWriter writer)
      {
         writer.WriteAttributeString("Rows", Rows.ToString());
         writer.WriteAttributeString("Cols", Cols.ToString());
         writer.WriteAttributeString("NumberOfChannels", NumberOfChannels.ToString());
         writer.WriteAttributeString("CompressionRatio", SerializationCompressionRatio.ToString());

         writer.WriteStartElement("Bytes");
         Byte[] bytes = Bytes;
         writer.WriteBase64(bytes, 0, bytes.Length);
         writer.WriteEndElement();
      }
      #endregion

      #region ISerializable Members
      /// <summary>
      /// A function used for runtime serialization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
      public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("Rows", Rows);
         info.AddValue("Cols", Cols);
         info.AddValue("NumberOfChannels", NumberOfChannels);
         info.AddValue("CompressionRatio", SerializationCompressionRatio);
         info.AddValue("Bytes", Bytes);
      }

      /// <summary>
      /// A function used for runtime deserailization of the object
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      protected virtual void DeserializeObjectData(SerializationInfo info, StreamingContext context)
      {
         int rows = (int)info.GetValue("Rows", typeof(int));
         int cols = (int)info.GetValue("Cols", typeof(int));
         int numberOfChannels = (int)info.GetValue("NumberOfChannels", typeof(int));
         AllocateData(rows, cols, numberOfChannels);
         SerializationCompressionRatio = (int)info.GetValue("CompressionRatio", typeof(int));
         Bytes = (Byte[])info.GetValue("Bytes", typeof(Byte[]));
      }
      #endregion
   }
}
