//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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

      protected IntPtr _rotationWarper;

      public IntPtr WarperCreatorPtr
      {
         get { return _warperCreatorPtr; }
      }

      protected override void DisposeObject()
      {
         if (_rotationWarper != IntPtr.Zero)
            _rotationWarper = IntPtr.Zero;

         if (_warperCreatorPtr != IntPtr.Zero)
            _warperCreatorPtr = IntPtr.Zero;
      }

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

   public class PlanWarper : WarperCreator
   {
      public PlanWarper(float scale)
      {
         _ptr = StitchingInvoke.cvePlanWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cvePlanWarperRelease(ref _ptr);
         }

      }
   }

   public class SphericalWarper : WarperCreator
   {
      public SphericalWarper(float scale)
      {
         _ptr = StitchingInvoke.cveSphericalWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveSphericalWarperRelease(ref _ptr);
            
         }
      }
   }

   public class FisheyeWarper : WarperCreator
   {
      public FisheyeWarper(float scale)
      {
         _ptr = StitchingInvoke.cveFisheyeWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveFisheyeWarperRelease(ref _ptr);
            
         }
      }
   }


   public class StereographicWarper : WarperCreator
   {
      public StereographicWarper(float scale)
      {
         _ptr = StitchingInvoke.cveStereographicWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveStereographicWarperRelease(ref _ptr);
            
         }
      }
   }

   public class CompressedRectilinearWarper : WarperCreator
   {
      public CompressedRectilinearWarper(float scale)
      {
         _ptr = StitchingInvoke.cveCompressedRectilinearWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveCompressedRectilinearWarperRelease(ref _ptr);
            
         }
      }
   }

   public class PaniniWarper : WarperCreator
   {
      public PaniniWarper(float scale)
      {
         _ptr = StitchingInvoke.cvePaniniWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cvePaniniWarperRelease(ref _ptr);
            
         }
      }
   }


   public class PaniniPortraitWarper : WarperCreator
   {
      public PaniniPortraitWarper(float scale)
      {
         _ptr = StitchingInvoke.cvePaniniPortraitWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cvePaniniPortraitWarperRelease(ref _ptr);
            
         }
      }
   }

   public class MercatorWarper : WarperCreator
   {
      public MercatorWarper(float scale)
      {
         _ptr = StitchingInvoke.cveMercatorWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

      protected override void DisposeObject()
      {
         base.DisposeObject();
         if (_ptr != IntPtr.Zero)
         {
            StitchingInvoke.cveMercatorWarperRelease(ref _ptr);
            
         }
      }
   }

   public class TransverseMercatorWarper : WarperCreator
   {
      public TransverseMercatorWarper(float scale)
      {
         _ptr = StitchingInvoke.cveTransverseMercatorWarperCreate(scale, ref _warperCreatorPtr, ref _rotationWarper);
      }

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
      internal static extern IntPtr cvePlanWarperCreate(float scale, ref IntPtr creator, ref IntPtr rotationWarper);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvePlanWarperRelease(ref IntPtr warper);


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
