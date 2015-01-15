/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Oct-Tree
   /// </summary>
   public class Octree : UnmanagedObject
   {
      static Octree()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create an empty Oct-Tree
      /// </summary>
      public Octree()
      {
         _ptr = CvOctreeCreate();
      }

      /// <summary>
      /// Create an Oct-Tree from the given points
      /// </summary>
      /// <param name="points">The points to be inserted into the Oct-Tree</param>
      /// <param name="maxLevels">The maximum levels of the Oct-Tree</param>
      /// <param name="minPoints">The minimum number of points in each level</param>
      public Octree(MCvPoint3D32f[] points, int maxLevels, int minPoints)
         : this()
      {
         BuildTree(points, maxLevels, minPoints);
      }

      /// <summary>
      /// Build an Oct-Tree from the given points
      /// </summary>
      /// <param name="points">The points to be inserted into the Oct-Tree</param>
      /// <param name="maxLevels">The maximum levels of the Oct-Tree</param>
      /// <param name="minPoints">The minimum number of points in each level</param>
      public void BuildTree(MCvPoint3D32f[] points, int maxLevels, int minPoints)
      {
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         CvOctreeBuildTree(_ptr, handle.AddrOfPinnedObject(), points.Length, maxLevels, minPoints);
         handle.Free();
      }

      /// <summary>
      /// Get the points within the specific sphere
      /// </summary>
      /// <param name="center">The center of the sphere</param>
      /// <param name="radius">The radius of the sphere</param>
      /// <returns>The points withing the specific sphere</returns>
      public MCvPoint3D32f[] GetPointsWithinSphere(MCvPoint3D32f center, float radius)
      {
         using (Util.VectorOfPoint3D32F pts = new Util.VectorOfPoint3D32F())
         {
            CvOctreeGetPointsWithinSphere(_ptr, ref center, radius, pts);
            return pts.ToArray();
         }
      }

      /// <summary>
      /// Release the Oct Tree
      /// </summary>
      protected override void DisposeObject()
      {
         CvOctreeRelease(_ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvOctreeCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvOctreeRelease(IntPtr tree);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvOctreeBuildTree(IntPtr tree, IntPtr points, int numberOfPoints, int maxLevels, int minPoints);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvOctreeGetPointsWithinSphere(IntPtr tree, ref MCvPoint3D32f center, float radius, IntPtr points);
   }

}
*/