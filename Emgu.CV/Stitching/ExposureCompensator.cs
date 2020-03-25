//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ExposureCompensator : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the native ExposureCompensator object.
        /// </summary>
        protected IntPtr _exposureCompensatorPtr;

        /// <summary>
        /// Pointer to the native ExposureCompensator object.
        /// </summary>
        public IntPtr ExposureCompensatorPtr
        {
            get { return _exposureCompensatorPtr; }
        }

        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_exposureCompensatorPtr != IntPtr.Zero)
                _exposureCompensatorPtr = IntPtr.Zero;
        }
    }


    /// <summary>
    /// Stub exposure compensator which does nothing.
    /// </summary>
    public class NoExposureCompensator : ExposureCompensator
    {
        /// <summary>
        /// Create a new stub exposure compensator which does nothing.
        /// </summary>
        public NoExposureCompensator()
        {
            _ptr = StitchingInvoke.cveNoExposureCompensatorCreate(ref _exposureCompensatorPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this exposure compensator 
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveNoExposureCompensatorRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Exposure compensator which tries to remove exposure related artifacts by adjusting image intensities
    /// </summary>
    public class GainCompensator : ExposureCompensator
    {
        /// <summary>
        /// Create a new gain compensator
        /// </summary>
        /// <param name="nrFeeds"></param>
        public GainCompensator(int nrFeeds)
        {
            _ptr = StitchingInvoke.cveGainCompensatorCreate(nrFeeds, ref _exposureCompensatorPtr);
        }

        /// <summary>
        /// Release all unmanaged resources associated with this exposure compensator 
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveGainCompensatorRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    /// <summary>
    /// Exposure compensator which tries to remove exposure related artifacts by adjusting image intensities on each channel independently. 
    /// </summary>
    public class ChannelsCompensator : ExposureCompensator
    {
        /// <summary>
        /// Create a new exposure compensator
        /// </summary>
        /// <param name="nrFeeds"></param>
        public ChannelsCompensator(int nrFeeds)
        {
            _ptr = StitchingInvoke.cveChannelsCompensatorCreate(nrFeeds, ref _exposureCompensatorPtr);
        }

        /// <summary>
        /// Release all unmanaged resources associated with this exposure compensator 
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveChannelsCompensatorRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    /*
    public class BlocksCompensator : ExposureCompensator
    {
        public BlocksCompensator(int blWidth = 32, int blHeight = 32, int nrFeeds = 1)
        {
            _ptr = StitchingInvoke.cveBlocksCompensatorCreate(blWidth, blHeight, nrFeeds, ref _exposureCompensatorPtr);
        }

        /// <summary>
        /// Release all unmanaged resources associated with this exposure compensator 
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBlocksCompensatorRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }*/

    /// <summary>
    /// Exposure compensator which tries to remove exposure related artifacts by adjusting image block intensities
    /// </summary>
    public class BlocksGainCompensator : ExposureCompensator
    {
        /// <summary>
        /// Create a new exposure compensator
        /// </summary>
        /// <param name="blWidth">block width</param>
        /// <param name="blHeight">block height</param>
        /// <param name="nrFeeds"></param>
        public BlocksGainCompensator(int blWidth = 32, int blHeight = 32, int nrFeeds = 1)
        {
            _ptr = StitchingInvoke.cveBlocksGainCompensatorCreate(blWidth, blHeight, nrFeeds,
                ref _exposureCompensatorPtr);
        }

        /// <summary>
        /// Release all unmanaged resources associated with this exposure compensator 
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBlocksGainCompensatorRelease(ref _ptr);
            }

            base.DisposeObject();
        }
    }

    /// <summary>
    /// Exposure compensator which tries to remove exposure related artifacts by adjusting image block on each channel.
    /// </summary>
    public class BlocksChannelsCompensator : ExposureCompensator
    {
        /// <summary>
        /// Create a new exposure compensator
        /// </summary>
        /// <param name="blWidth">Block width</param>
        /// <param name="blHeight">Block height</param>
        /// <param name="nrFeeds"></param>
        public BlocksChannelsCompensator(int blWidth = 32, int blHeight = 32, int nrFeeds = 1)
        {
            _ptr = StitchingInvoke.cveBlocksChannelsCompensatorCreate(blWidth, blHeight, nrFeeds,
                ref _exposureCompensatorPtr);
        }

        /// <summary>
        /// Release all unmanaged resources associated with this exposure compensator 
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBlocksChannelsCompensatorRelease(ref _ptr);
            }

            base.DisposeObject();
        }
    }

    public static partial class StitchingInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveNoExposureCompensatorCreate(ref IntPtr exposureCompensatorPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveNoExposureCompensatorRelease(ref IntPtr compensator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGainCompensatorCreate(int nrFeeds, ref IntPtr exposureCompensatorPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGainCompensatorRelease(ref IntPtr compensator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveChannelsCompensatorCreate(int nrFeeds, ref IntPtr exposureCompensatorPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveChannelsCompensatorRelease(ref IntPtr compensator);

        /*
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBlocksCompensatorCreate(int blWidth, int blHeight, int nrFeeds,
            ref IntPtr exposureCompensatorPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBlocksCompensatorRelease(ref IntPtr compensator);
        */

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBlocksGainCompensatorCreate(int blWidth, int blHeight, int nrFeeds,
            ref IntPtr exposureCompensatorPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBlocksGainCompensatorRelease(ref IntPtr compensator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBlocksChannelsCompensatorCreate(int blWidth, int blHeight, int nrFeeds,
            ref IntPtr exposureCompensatorPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBlocksChannelsCompensatorRelease(ref IntPtr compensator);
    }

}
