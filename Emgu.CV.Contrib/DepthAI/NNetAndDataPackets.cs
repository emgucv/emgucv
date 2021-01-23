//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.DepthAI
{
    public partial class NNetAndDataPackets : UnmanagedObject
    {
        internal NNetAndDataPackets(IntPtr ptr)
        {
            _ptr = ptr;
        }

        public HostDataPacket[] HostDataPackets
        {
            get
            {
                int size = DepthAIInvoke.depthaiNNetAndDataPacketsGetHostDataPacketCount(_ptr);
                IntPtr[] ptrs = new IntPtr[size];
                GCHandle ptrsHandle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
                DepthAIInvoke.depthaiNNetAndDataPacketsGetHostDataPacketArr(_ptr, ptrsHandle.AddrOfPinnedObject());
                ptrsHandle.Free();
                return Array.ConvertAll<IntPtr, HostDataPacket>(ptrs, delegate(IntPtr ptr) { return new HostDataPacket(ptr); });
            }
        }

        public NNetPacket[] NNetPackets
        {
            get
            {
                int size = DepthAIInvoke.depthaiNNetAndDataPacketsGetNNetCount(_ptr);
                IntPtr[] ptrs = new IntPtr[size];
                GCHandle ptrsHandle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
                DepthAIInvoke.depthaiNNetAndDataPacketsGetNNetArr(_ptr, ptrsHandle.AddrOfPinnedObject());
                ptrsHandle.Free();
                return Array.ConvertAll<IntPtr, NNetPacket>(ptrs, delegate (IntPtr ptr) { return new NNetPacket(ptr); });
            }
        }

        protected override void DisposeObject()
        {
            
            if (_ptr != IntPtr.Zero)
            {
                DepthAIInvoke.depthaiNNetAndDataPacketsRelease(ref _ptr);
            }
        }
    }

}