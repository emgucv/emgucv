using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE

namespace Emgu.CV
{
    public static class WinrtInvoke
    {
        static WinrtInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }
        [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
        public delegate void WinrtMessageLoopCallback();

        [DllImport(CvInvoke.ExternLibrary, EntryPoint = "cveWinrtStartMessageLoop", CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void WinrtStartMessageLoop(WinrtMessageLoopCallback callback);

        [DllImport(CvInvoke.ExternLibrary, EntryPoint = "cveWinrtSetFrameContainer", CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void WinrtSetFrameContainer(Windows.UI.Xaml.Controls.Image image);

        [DllImport(CvInvoke.ExternLibrary, EntryPoint = "cveWinrtImshow", CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void WinrtImshow();

        [DllImport(CvInvoke.ExternLibrary, EntryPoint = "cveWinrtOnVisibilityChanged", CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void WinrtOnVisibilityChanged(
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool visible);
    }
}

#endif