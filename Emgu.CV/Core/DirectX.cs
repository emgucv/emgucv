//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;


namespace Emgu.CV
{

    /// <summary>
    /// DirectX interoperability
    /// </summary>
    public static class DirectX
    {
        /// <summary>
        /// Converts InputArray to ID3D11Texture2D. If destination texture format is DXGI_FORMAT_NV12 then input UMat expected to be in BGR format and data will be downsampled and color-converted to NV12.
        /// </summary>
        /// <param name="src">Source InputArray</param>
        /// <param name="pD3D11Texture2D">Destination D3D11 texture</param>
        public static void ConvertToD3D11Texture2D(IInputArray src, IntPtr pD3D11Texture2D)
        {
            using (InputArray iaSrc = src.GetInputArray())
            {
                CvInvoke.cveDirectxConvertToD3D11Texture2D(iaSrc, pD3D11Texture2D);
            }
        }

        /// <summary>
        /// Converts ID3D11Texture2D to OutputArray. If input texture format is DXGI_FORMAT_NV12 then data will be upsampled and color-converted to BGR format.
        /// </summary>
        /// <param name="pD3D11Texture2D">Source D3D11 texture</param>
        /// <param name="dst">Destination OutputArray</param>
        public static void ConvertFromD3D11Texture2D(IntPtr pD3D11Texture2D, IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                CvInvoke.cveDirectxConvertFromD3D11Texture2D(pD3D11Texture2D, oaDst);
            }
        }

        /// <summary>
        /// Converts InputArray to ID3D10Texture2D.
        /// </summary>
        /// <param name="src">Source InputArray</param>
        /// <param name="pD3D10Texture2D">Destination D3D10 texture</param>
        public static void ConvertToD3D10Texture2D(IInputArray src, IntPtr pD3D10Texture2D)
        {
            using (InputArray iaSrc = src.GetInputArray())
            {
                CvInvoke.cveDirectxConvertToD3D10Texture2D(iaSrc, pD3D10Texture2D);
            }
        }

        /// <summary>
        /// Converts ID3D10Texture2D to OutputArray.
        /// </summary>
        /// <param name="pD3D10Texture2D">Source D3D10 texture</param>
        /// <param name="dst">Destination OutputArray</param>
        public static void ConvertFromD3D10Texture2D(IntPtr pD3D10Texture2D, IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                CvInvoke.cveDirectxConvertFromD3D10Texture2D(pD3D10Texture2D, oaDst);
            }
        }

        /// <summary>
        /// Converts InputArray to IDirect3DSurface9
        /// </summary>
        /// <param name="src">Source InputArray</param>
        /// <param name="pDirect3DSurface9">Destination D3D10 texture</param>
        /// <param name="surfaceSharedHandle">Shared handle</param>
        public static void ConvertToDirect3DSurface9(
            IInputArray src, 
            IntPtr pDirect3DSurface9,
            IntPtr surfaceSharedHandle)
        {
            using (InputArray iaSrc = src.GetInputArray())
            {
                CvInvoke.cveDirectxConvertToDirect3DSurface9(iaSrc, pDirect3DSurface9, surfaceSharedHandle);
            }
        }

        /// <summary>
        /// Converts IDirect3DSurface9 to OutputArray.
        /// </summary>
        /// <param name="pDirect3DSurface9">Source texture</param>
        /// <param name="dst">Destination OutputArray</param>
        /// <param name="surfaceSharedHandle">Shared handle</param>
        public static void ConvertFromDirect3DSurface9(IntPtr pDirect3DSurface9, IOutputArray dst, IntPtr surfaceSharedHandle)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                CvInvoke.cveDirectxConvertFromDirect3DSurface9(pDirect3DSurface9, oaDst, surfaceSharedHandle);
            }
        }
    }

    public static partial class CvInvoke
    {


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDirectxConvertToD3D11Texture2D(IntPtr src, IntPtr pD3D11Texture2D);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDirectxConvertFromD3D11Texture2D(IntPtr pD3D11Texture2D, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDirectxConvertToD3D10Texture2D(IntPtr src, IntPtr pD3D10Texture2D);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDirectxConvertFromD3D10Texture2D(IntPtr pD3D10Texture2D, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDirectxConvertToDirect3DSurface9(IntPtr src, IntPtr pDirect3DSurface9, IntPtr surfaceSharedHandle);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDirectxConvertFromDirect3DSurface9(IntPtr pDirect3DSurface9, IntPtr dst, IntPtr surfaceSharedHandle);

    }
}
