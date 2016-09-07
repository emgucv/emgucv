//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   public partial class Viz3d : UnmanagedObject
   {
      public Viz3d(String windowName)
      {
         using (CvString cvs = new CvString(windowName))
         {
            _ptr = CvInvoke.cveViz3dCreate(cvs);
         }
      }

      public void ShowWidget(String id, IWidget widget)
      {
         using (CvString cvsId = new CvString(id))
            CvInvoke.cveViz3dShowWidget(_ptr, cvsId, widget.GetWidget);
      }

      public void Spin()
      {
         CvInvoke.cveViz3dSpin(_ptr);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this Viz3d object
      /// </summary>
      protected override void DisposeObject()
      {
         if (!IntPtr.Zero.Equals(_ptr))
            CvInvoke.cveViz3dRelease(ref _ptr);
      }
   }

   public interface IWidget
   {
      IntPtr GetWidget { get; }
   }

   public interface IWidget3D : IWidget
   {
      IntPtr GetWidget3D { get; }
   }

   public interface IWidget2D : IWidget
   {
      IntPtr GetWidget2D { get; }
   }

   public class WCloud : UnmanagedObject, IWidget3D
   {
      private IntPtr _widgetPtr;
      private IntPtr _widget3dPtr;

      public WCloud(IInputArray cloud, IInputArray color)
      {
         using (InputArray iaCloud = cloud.GetInputArray())
         using (InputArray iaColor = color.GetInputArray())
            CvInvoke.cveWCloudCreateWithColorArray(iaCloud, iaColor, ref _widget3dPtr, ref _widgetPtr);
      }

      public WCloud(IInputArray cloud, MCvScalar color)
      {
         using (InputArray iaCloud = cloud.GetInputArray())
            CvInvoke.cveWCloudCreateWithColor(iaCloud, ref color, ref _widget3dPtr, ref _widgetPtr);
      }

      public IntPtr GetWidget3D
      {
         get { return _widget3dPtr; }
      }

      public IntPtr GetWidget
      {
         get { return _widgetPtr; }
      }

      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveWCloudRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
         }
      }
   }

   public class WText :  UnmanagedObject, IWidget2D
   {
      private IntPtr _widgetPtr;
      private IntPtr _widget2dPtr;

      public WText(String text, Point pos, int fontSize, MCvScalar color)
      {
         using (CvString cvs = new CvString(text))
            _ptr = CvInvoke.cveWTextCreate(cvs, ref pos, fontSize, ref color, ref _widget2dPtr, ref _widgetPtr);

      }

      public IntPtr GetWidget2D
      {
         get { return _widget2dPtr; }
      }

      public IntPtr GetWidget
      {
         get { return _widgetPtr; }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this Viz3d object
      /// </summary>
      protected override void DisposeObject()
      {
         if (!IntPtr.Zero.Equals(_ptr))
            CvInvoke.cveViz3dRelease(ref _ptr);
         _widgetPtr = IntPtr.Zero;
         _widget2dPtr = IntPtr.Zero;
      }
   }
   
      public class WCoordinateSystem :  UnmanagedObject, IWidget3D
   {
      private IntPtr _widgetPtr;
      private IntPtr _widget3dPtr;

      public WCoordinateSystem(double scale)
      {
         
            _ptr = CvInvoke.cveWCoordinateSystemCreate(scale,ref _widget3dPtr, ref _widgetPtr);

      }

      public IntPtr GetWidget3D
      {
         get { return _widget3dPtr; }
      }

      public IntPtr GetWidget
      {
         get { return _widgetPtr; }
      }

      public void SetBackgroundMeshLab()
      {
         CvInvoke.cveViz3dSetBackgroundMeshLab(_ptr);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this Viz3d object
      /// </summary>
      protected override void DisposeObject()
      {
         if (!IntPtr.Zero.Equals(_ptr))
            CvInvoke.cveWCoordinateSystemRelease(ref _ptr);
         _widgetPtr = IntPtr.Zero;
         _widget3dPtr = IntPtr.Zero;
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveViz3dCreate(IntPtr s);

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveViz3dShowWidget(IntPtr viz, IntPtr id, IntPtr widget);

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveViz3dSpin(IntPtr viz);

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveViz3dSetBackgroundMeshLab(IntPtr viz);

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveViz3dRelease(ref IntPtr viz);


      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveWTextCreate(IntPtr text, ref Point pos, int fontSize, ref MCvScalar color, ref IntPtr widget2D, ref IntPtr widget);

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveWTextRelease(ref IntPtr text);
      
            [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveWCoordinateSystemCreate(double scale, ref IntPtr widget3d, ref IntPtr widget);

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveWCoordinateSystemRelease(ref IntPtr system);


      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveWCloudCreateWithColorArray(IntPtr cloud, IntPtr color, ref IntPtr widget3d, ref IntPtr widget);
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveWCloudCreateWithColor(IntPtr cloud, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveWCloudRelease(ref IntPtr cloud);
   }
}
