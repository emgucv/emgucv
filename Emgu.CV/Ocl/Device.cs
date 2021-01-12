//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Ocl
{
    /// <summary>
    /// This class contains ocl device information
    /// </summary>
    public partial class Device : UnmanagedObject
    {
        private static Device _defaultDevice = new Device(OclInvoke.oclDeviceGetDefault(), false);

        private bool _needDispose;

        /// <summary>
        /// Create a empty OclDevice object
        /// </summary>
        public Device()
           : this(OclInvoke.oclDeviceCreate(), true)
        {
        }

        /// <summary>
        /// Get the default OclDevice. Do not dispose this device.
        /// </summary>
        public static Device Default
        {
            get
            {
                return _defaultDevice;
            }
        }

        internal Device(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this OclDevice
        /// </summary>
        protected override void DisposeObject()
        {
            if (_needDispose)
            {
                if (_ptr != IntPtr.Zero)
                {
                    OclInvoke.oclDeviceRelease(ref _ptr);
                }
            }
        }

        /// <summary>
        /// Get the native device pointer
        /// </summary>
        public IntPtr NativeDevicePointer
        {
            get { return OclInvoke.oclDeviceGetPtr(_ptr); }
        }

        /// <summary>
        /// Set the native device pointer
        /// </summary>
        /// <param name="nativeDevicePointer">The native device pointer</param>
        public void Set(IntPtr nativeDevicePointer)
        {
            OclInvoke.oclDeviceSet(_ptr, nativeDevicePointer);
        }

        /// <summary>
        /// Get the string representation of this oclDevice
        /// </summary>
        /// <returns>A string representation of this oclDevice</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}.{2} ({3}):Version - {4}; Global memory - {5}； Local memory - {6}; Max image size - {7}x{8}; DoubleFpConfig: {9}", Name, DeviceVersionMajor, DeviceVersionMinor, Type, Version, GlobalMemSize, LocalMemSize, Image2DMaxWidth, Image2DMaxHeight, DoubleFPConfig);
        }
    }


    /// <summary>
    /// Ocl Device Type
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = (1 << 0),
        /// <summary>
        /// Cpu
        /// </summary>
        Cpu = (1 << 1),
        /// <summary>
        /// Gpu
        /// </summary>
        Gpu = (1 << 2),
        /// <summary>
        /// Accerlerator
        /// </summary>
        Accelerator = (1 << 3),
        /// <summary>
        /// DGpu
        /// </summary>
        DGpu = Gpu | (1 << 16),
        /// <summary>
        /// IGpu
        /// </summary>
        IGpu = Gpu | (1 << 17),
        /// <summary>
        /// All
        /// </summary>
        All = -1 //0xFFFFFFFF
    }

    /// <summary>
    /// Floating point configuration
    /// </summary>
    [Flags]
    public enum FpConfig
    {
        /// <summary>
        /// Denorm
        /// </summary>
        Denorm = (1 << 0),
        /// <summary>
        /// inf, nan
        /// </summary>
        InfNan = (1 << 1),
        /// <summary>
        /// round to nearest
        /// </summary>
        RoundToNearest = (1 << 2),
        /// <summary>
        /// round to zero
        /// </summary>
        RoundToZero = (1 << 3),
        /// <summary>
        /// round to infinite
        /// </summary>
        RoundToInf = (1 << 4),
        /// <summary>
        /// FMA
        /// </summary>
        Fma = (1 << 5),
        /// <summary>
        /// soft float
        /// </summary>
        SoftFloat = (1 << 6),
        /// <summary>
        /// Correctly rounded divide sqrt
        /// </summary>
        CorrectlyRoundedDivideSqrt = (1 << 7)
    }

    /// <summary>
    /// Class that contains ocl functions
    /// </summary>
    public static partial class OclInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr oclDeviceCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void oclDeviceSet(IntPtr device, IntPtr p);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr oclDeviceGetDefault();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void oclDeviceRelease(ref IntPtr oclDevice);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr oclDeviceGetPtr(IntPtr device);
    }
}
