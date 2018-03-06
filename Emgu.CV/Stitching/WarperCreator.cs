//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
   /// Finds features in the given image.
   /// </summary>
   public abstract class WarperCreator : UnmanagedObject
   {
      /// <summary>
      /// Pointer to the unmanaged WarperCreator object
      /// </summary>
      protected IntPtr _warperCreatorPtr;

      /// <summary>
      /// Pointer to the unmanaged RotationWarper object
      /// </summary>
      protected IntPtr _rotationWarper;

      /// <summary>
      /// Get a pointer to the unmanaged WarperCreator object
      /// </summary>
      public IntPtr WarperCreatorPtr
      {
         get { return _warperCreatorPtr; }
      }

      /// <summary>
      /// Reset the unmanaged pointer associated to this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_rotationWarper != IntPtr.Zero)
            _rotationWarper = IntPtr.Zero;

         if (_warperCreatorPtr != IntPtr.Zero)
            _warperCreatorPtr = IntPtr.Zero;
      }

      /// <summary>
      /// Builds the projection maps according to the given camera data.
      /// </summary>
      /// <param name="srcSize">Source image size</param>
      /// <param name="K">Camera intrinsic parameters</param>
      /// <param name="R">Camera rotation matrix</param>
      /// <param name="xmap">Projection map for the x axis</param>
      /// <param name="ymap">Projection map for the y axis</param>
      /// <returns>Projected image minimum bounding box</returns>
      public Rectangle BuildMaps(Size srcSize, IInputArray K, IInputArray R, IOutputArray xmap, IOutputArray ymap)
      {
         Rectangle result = new Rectangle();
         using (InputArray iaK = K.GetInputArray())
         using (InputArray iaR = R.GetInputArray())
         using (OutputArray oaXmap = xmap.GetOutputArray())
         using (OutputArray oaYmap = ymap.GetOutputArray())
         {
            StitchingInvoke.cveRotationWarperBuildMaps(_rotationWarper, ref srcSize, iaK, iaR, oaXmap, oaYmap, ref result);
            return result;
         }
      }

      /// <summary>
      /// Projects the image.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="K">Camera intrinsic parameters</param>
      /// <param name="R">Camera rotation matrix</param>
      /// <param name="interpMode">Interpolation mode</param>
      /// <param name="borderMode">Border extrapolation mode</param>
      /// <param name="dst">Projected image</param>
      /// <returns>Project image top-left corner</returns>
      public Point Warp(IInputArray src, IInputArray K, IInputArray R, CvEnum.Inter interpMode, CvEnum.BorderType borderMode, IOutputArray dst)
      {
         Point corner = new Point();
         using (InputArray iaSrc = src.GetInputArray())
         using (InputArray iaK = K.GetInputArray())
         using (InputArray iaR = R.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
         {
            StitchingInvoke.cveRotationWarperWarp(_rotationWarper, iaSrc, iaK, iaR, interpMode, borderMode, oaDst, ref corner);
            return corner;
         }
      }
   }

   /// <summary>
   /// Warper that maps an image onto the z = 1 plane.
   /// </summary>
   public class PlaneWarper : WarperCreator
   {
      /// <summary>
      /// Construct an instance of the plane warper class.
      /// </summary>
      /// <param name="scale">Projected image scale multiplier</param>
      public PlaneWarper(float scale)
      {
         _ptr = StitchingInvoke.cvePlaneWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cvePlaneWarperRelease(ref _ptr);
         }

      }
   }

   /// <summary>
   /// Warper that maps an image onto the unit sphere located at the origin.
   /// </summary>
   public class SphericalWarper : WarperCreator
   {
      /// <summary>
      /// Construct an instance of the spherical warper class.
      /// </summary>
      /// <param name="scale">Radius of the projected sphere, in pixels. An image spanning the whole sphere will have a width of 2 * scale * PI pixels.</param>
      public SphericalWarper(float scale)
      {
         _ptr = StitchingInvoke.cveSphericalWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveSphericalWarperRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Fisheye Warper
   /// </summary>
   public class FisheyeWarper : WarperCreator
   {
      /// <summary>
      /// Create a fisheye warper
      /// </summary>
      /// <param name="scale">Projected image scale multiplier</param>
      public FisheyeWarper(float scale)
      {
         _ptr = StitchingInvoke.cveFisheyeWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveFisheyeWarperRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Stereographic Warper
   /// </summary>
   public class StereographicWarper : WarperCreator
   {
      /// <summary>
      /// Create a stereographic warper
      /// </summary>
      /// <param name="scale">Projected image scale multiplier</param>
      public StereographicWarper(float scale)
      {
         _ptr = StitchingInvoke.cveStereographicWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveStereographicWarperRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Compressed rectilinear warper
   /// </summary>
   public class CompressedRectilinearWarper : WarperCreator
   {
      /// <summary>
      /// Create a compressed rectilinear warper
      /// </summary>
      /// <param name="scale">Projected image scale multiplier</param>
      public CompressedRectilinearWarper(float scale)
      {
         _ptr = StitchingInvoke.cveCompressedRectilinearWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveCompressedRectilinearWarperRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Panini warper
   /// </summary>
   public class PaniniWarper : WarperCreator
   {
      /// <summary>
      /// Create a Panini warper
      /// </summary>
      /// <param name="scale">Projected image scale multiplier</param>
      public PaniniWarper(float scale)
      {
         _ptr = StitchingInvoke.cvePaniniWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cvePaniniWarperRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Panini portrait warper
   /// </summary>
   public class PaniniPortraitWarper : WarperCreator
   {
      /// <summary>
      /// Create a panini portrait warper
      /// </summary>
      /// <param name="scale">Projected image scale multiplier</param>
      public PaniniPortraitWarper(float scale)
      {
         _ptr = StitchingInvoke.cvePaniniPortraitWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cvePaniniPortraitWarperRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Mercator warper
   /// </summary>
   public class MercatorWarper : WarperCreator
   {
      /// <summary>
      /// Create a Mercator Warper
      /// </summary>
      /// <param name="scale">Projected image scale multiplier</param>
      public MercatorWarper(float scale)
      {
         _ptr = StitchingInvoke.cveMercatorWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveMercatorWarperRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Transverse mercator warper
   /// </summary>
   public class TransverseMercatorWarper : WarperCreator
   {
      /// <summary>
      /// Create a transverse mercator warper
      /// </summary>
      /// <param name="scale">Projected image scale multiplier</param>
      public TransverseMercatorWarper(float scale)
      {
         _ptr = StitchingInvoke.cveTransverseMercatorWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this wraper
      /// </summary>
      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveTransverseMercatorWarperRelease(ref _ptr);
         }
      }
   }

   public static partial class StitchingInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvePlaneWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlaneWarperRelease(ref IntPtr warper);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSphericalWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSphericalWarperRelease(ref IntPtr warper);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveFisheyeWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFisheyeWarperRelease(ref IntPtr warper);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveStereographicWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveStereographicWarperRelease(ref IntPtr warper);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveCompressedRectilinearWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveCompressedRectilinearWarperRelease(ref IntPtr warper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvePaniniWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePaniniWarperRelease(ref IntPtr warper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvePaniniPortraitWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePaniniPortraitWarperRelease(ref IntPtr warper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveMercatorWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveMercatorWarperRelease(ref IntPtr warper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveTransverseMercatorWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveTransverseMercatorWarperRelease(ref IntPtr warper);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveRotationWarperBuildMaps(IntPtr warper, ref Size srcSize, IntPtr K, IntPtr R, IntPtr xmap, IntPtr ymap, ref Rectangle boundingBox);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveRotationWarperWarp(IntPtr warper, IntPtr src, IntPtr K, IntPtr R, CvEnum.Inter interpMode, CvEnum.BorderType borderMode, IntPtr dst, ref Point corner);

   }
}
