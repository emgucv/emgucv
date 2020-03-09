//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
    /// <summary>
    /// Utilities class
    /// </summary>
    public static class CvToolbox
    {

        /*
        /// <summary>
        /// Returns information about one of or all of the registered modules
        /// </summary>
        /// <param name="pluginName">The list of names and versions of the optimized plugins that CXCORE was able to find and load</param>
        /// <param name="versionName">Information about the module(s), including version</param>
        public static void GetModuleInfo(out String pluginName, out String versionName)
        {
           IntPtr version = IntPtr.Zero;
           IntPtr pluginInfo = IntPtr.Zero;
           CvInvoke.cvGetModuleInfo(IntPtr.Zero, ref version, ref pluginInfo);

           pluginName = Marshal.PtrToStringAnsi(pluginInfo);
           versionName = Marshal.PtrToStringAnsi(version);
        }

        /// <summary>
        /// Enable or diable IPL optimization for opencv
        /// </summary>
        /// <param name="enable">true to enable optimization, false to disable</param>
        public static void OptimizeCV(bool enable)
        {
           CvInvoke.cvUseOptimized(enable);
        }


        /// <summary>
        /// Get the OpenCV matrix depth enumeration from depth type
        /// </summary>
        /// <param name="typeOfDepth">The depth of type</param>
        /// <returns>OpenCV Matrix depth</returns>
        public static CvEnum.DepthType GetMatrixDepth(Type typeOfDepth)
        {
           if (typeOfDepth == typeof(Single))
              return CvEnum.DepthType.Cv32F;
           if (typeOfDepth == typeof(Int32))
              return Emgu.CV.CvEnum.DepthType.Cv32S;
           if (typeOfDepth == typeof(SByte))
              return Emgu.CV.CvEnum.DepthType.Cv8S;
           if (typeOfDepth == typeof(Byte))
              return CvEnum.DepthType.Cv8U;
           if (typeOfDepth == typeof(Double))
              return CvEnum.DepthType.Cv64F;
           if (typeOfDepth == typeof(UInt16))
              return CvEnum.DepthType.Cv16U;
           if (typeOfDepth == typeof(Int16))
              return CvEnum.DepthType.Cv16S;
           throw new NotImplementedException("Unsupported matrix depth");
        }*/

        /// <summary>
        /// Convert arrays of data to matrix
        /// </summary>
        /// <param name="data">Arrays of data</param>
        /// <returns>A two dimension matrix that represent the array</returns>
        /// <typeparam name="T">The data type of the matrix</typeparam>
        public static Matrix<T> GetMatrixFromArrays<T>(T[][] data)
         where T : struct
        {
            int rows = data.Length;
            int cols = data[0].Length;
            Matrix<T> res = new Matrix<T>(rows, cols);
            MCvMat mat = res.MCvMat;
            long dataPos = mat.Data.ToInt64();
            //int rowSizeInBytes = Marshal.SizeOf(typeof(T)) * cols;
            for (int i = 0; i < rows; i++, dataPos += mat.Step)
            {
                CopyVector(data[i], new IntPtr(dataPos));
            }
            return res;
        }

        /// <summary>
        /// Convert arrays of points to matrix
        /// </summary>
        /// <param name="points">Arrays of points</param>
        /// <returns>A two dimension matrix that represent the points</returns>
        public static Matrix<double> GetMatrixFromPoints(MCvPoint2D64f[][] points)
        {
            int rows = points.Length;
            int cols = points[0].Length;
            Matrix<double> res = new Matrix<double>(rows, cols, 2);

            MCvMat cvMat = res.MCvMat;
            for (int i = 0; i < rows; i++)
            {
                IntPtr dst = new IntPtr(cvMat.Data.ToInt64() + cvMat.Step * i);
                CopyVector(points[i], dst);
            }
            return res;
        }

        /// <summary>
        /// Compute the minimum and maximum value from the points
        /// </summary>
        /// <param name="points">The points</param>
        /// <param name="min">The minimum x,y,z values</param>
        /// <param name="max">The maximum x,y,z values</param>
        public static void GetMinMax(IEnumerable<MCvPoint3D64f> points, out MCvPoint3D64f min, out MCvPoint3D64f max)
        {
            min = new MCvPoint3D64f();
            min.X = min.Y = min.Z = double.MaxValue;
            max = new MCvPoint3D64f();
            max.X = max.Y = max.Z = double.MinValue;

            foreach (MCvPoint3D64f p in points)
            {
                min.X = Math.Min(min.X, p.X);
                min.Y = Math.Min(min.Y, p.Y);
                min.Z = Math.Min(min.Z, p.Z);
                max.X = Math.Max(max.X, p.X);
                max.Y = Math.Max(max.Y, p.Y);
                max.Z = Math.Max(max.Z, p.Z);
            }
        }

        /// <summary>
        /// Copy a generic vector to the unmanaged memory
        /// </summary>
        /// <typeparam name="TData">The data type of the vector</typeparam>
        /// <param name="src">The source vector</param>
        /// <param name="dest">Pointer to the destination unmanaged memory</param>
        /// <param name="bytesToCopy">Specify the number of bytes to copy. If this is -1, the number of bytes equals the number of bytes in the <paramref name="src"> array</paramref></param>
        /// <returns>The number of bytes copied</returns>
        public static int CopyVector<TData>(TData[] src, IntPtr dest, int bytesToCopy = -1)
        {
            int size;
            if (bytesToCopy < 0)
                size = Emgu.Util.Toolbox.SizeOf<TData>() * src.Length;
            else
                size = bytesToCopy;

            GCHandle handle = GCHandle.Alloc(src, GCHandleType.Pinned);
            Memcpy(dest, handle.AddrOfPinnedObject(), size);
            handle.Free();
            return size;
        }

        /// <summary>
        /// Copy a jagged two dimensional array to the unmanaged memory
        /// </summary>
        /// <typeparam name="TData">The data type of the jagged two dimensional</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="dest">Pointer to the destination unmanaged memory</param>
        public static void CopyMatrix<TData>(TData[][] source, IntPtr dest)
        {
            Int64 current = dest.ToInt64();

            for (int i = 0; i < source.Length; i++)
            {
                int step = CopyVector(source[i], new IntPtr(current));
                current += step;
            }
        }

        /// <summary>
        /// Copy a jagged two dimensional array from the unmanaged memory
        /// </summary>
        /// <typeparam name="D">The data type of the jagged two dimensional</typeparam>
        /// <param name="src">The src array</param>
        /// <param name="dest">Pointer to the destination unmanaged memory</param>
        public static void CopyMatrix<D>(IntPtr src, D[][] dest)
        {
            int datasize = Emgu.Util.Toolbox.SizeOf<D>();

            int step = datasize * dest[0].Length;
            long current = src.ToInt64();

            for (int i = 0; i < dest.Length; i++, current += step)
            {
                GCHandle handle = GCHandle.Alloc(dest[i], GCHandleType.Pinned);
                Memcpy(handle.AddrOfPinnedObject(), new IntPtr(current), step);
                handle.Free();
            }
        }


        /// <summary>
        /// memcpy function
        /// </summary>
        /// <param name="dest">the destination of memory copy</param>
        /// <param name="src">the source of memory copy</param>
        /// <param name="len">the number of bytes to be copied</param>
        public static void Memcpy(IntPtr dest, IntPtr src, int len)
        {
            CvInvoke.cveMemcpy(dest, src, len);
        }

        #region color conversion
        private static Dictionary<Type, Dictionary<Type, CvEnum.ColorConversion>> _lookupTable
           = new Dictionary<Type, Dictionary<Type, CvEnum.ColorConversion>>();

        private static CvEnum.ColorConversion GetCode(Type srcType, Type destType)
        {
            String key = String.Format("{0}2{1}", GetConversionCodenameFromType(srcType), GetConversionCodenameFromType(destType));
            return (CvEnum.ColorConversion)Enum.Parse(typeof(CvEnum.ColorConversion), key, true);
        }

        private static String GetConversionCodenameFromType(Type colorType)
        {
            ColorInfoAttribute info = (ColorInfoAttribute)colorType.GetCustomAttributes(typeof(ColorInfoAttribute), true)[0];
            return info.ConversionCodename;
        }

        /// <summary>
        /// Given the source and destination color type, compute the color conversion code for CvInvoke.cvCvtColor function
        /// </summary>
        /// <param name="srcColorType">The source color type. Must be a type inherited from IColor</param>
        /// <param name="destColorType">The dest color type. Must be a type inherited from IColor</param>
        /// <returns>The color conversion code for CvInvoke.cvCvtColor function</returns>
        public static CvEnum.ColorConversion GetColorCvtCode(Type srcColorType, Type destColorType)
        {
            CvEnum.ColorConversion conversion;
            lock (_lookupTable)
            {
                Dictionary<Type, CvEnum.ColorConversion> table = _lookupTable.ContainsKey(srcColorType) ?
                   _lookupTable[srcColorType] : (_lookupTable[srcColorType] = new Dictionary<Type, Emgu.CV.CvEnum.ColorConversion>());
                conversion = table.ContainsKey(destColorType) ?
                              table[destColorType] : (table[destColorType] = GetCode(srcColorType, destColorType));
                ;
            }
            return conversion;
        }
        #endregion
    }
}

