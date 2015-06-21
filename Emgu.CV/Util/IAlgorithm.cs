//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// This is the algorithm class
   /// </summary>
   public interface IAlgorithm
   {
      /// <summary>
      /// Return the pointer to the algorithm object
      /// </summary>
      /// <returns>The pointer to the algorithm object</returns>
      IntPtr AlgorithmPtr { get; }
   }

   /// <summary>
   /// Extension methods to the IAlgorithm interface
   /// </summary>
   public static class AlgorithmExtensions
   {
      /// <summary>
      /// Reads algorithm parameters from a file storage.
      /// </summary>
      /// <param name="algorithm">The algorithm.</param>
      /// <param name="node">The node from file storage.</param>
      public static void Read(this IAlgorithm algorithm, FileNode node)
      {
         CvInvoke.cveAlgorithmRead(algorithm.AlgorithmPtr, node);
      }

      /// <summary>
      /// Stores algorithm parameters in a file storage
      /// </summary>
      /// <param name="algorithm">The algorithm.</param>
      /// <param name="storage">The storage.</param>
      public static void Write(this IAlgorithm algorithm, FileStorage storage)
      {
         CvInvoke.cveAlgorithmWrite(algorithm.AlgorithmPtr, storage);
      }
   }

   public partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAlgorithmRead(IntPtr algorithm, IntPtr node);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAlgorithmWrite(IntPtr algorithm, IntPtr storage);

   }
}
