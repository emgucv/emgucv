//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// File Storage Node class.
   /// The node is used to store each and every element of the file storage opened for reading. When
   /// XML/YAML file is read, it is first parsed and stored in the memory as a hierarchical collection of
   /// nodes. Each node can be a “leaf” that is contain a single number or a string, or be a collection of
   /// other nodes. There can be named collections (mappings) where each element has a name and it is
   /// accessed by a name, and ordered collections (sequences) where elements do not have names but rather
   /// accessed by index. Type of the file node can be determined using FileNode::type method.
   /// Note that file nodes are only used for navigating file storages opened for reading. When a file
   /// storage is opened for writing, no data is stored in memory after it is written.
   /// </summary>
   public class FileNode : UnmanagedObject
   {
      /// <summary>
      /// Type of the file storage node
      /// </summary>
      public enum Type
      {
         /// <summary>
         /// Empty node
         /// </summary>
         None = 0,
         ///<summary>
         ///  an integer
         ///</summary>
         Int = 1,
         /// <summary>
         /// Floating-point number
         /// </summary>
         Real = 2,
         /// <summary>
         /// Synonym or Real
         /// </summary>
         Float = Real,
         /// <summary>
         /// Text string in UTF-8 encoding
         /// </summary>
         Str = 3,
         /// <summary>
         /// Synonym for Str
         /// </summary>
         String = Str,
         /// <summary>
         /// Integer of size size_t. Typically used for storing complex dynamic structures where some elements reference the others
         /// </summary>
         Ref = 4,
         /// <summary>
         /// The sequence
         /// </summary>
         Seq = 5,
         /// <summary>
         /// Mapping
         /// </summary>
         Map = 6,
         /// <summary>
         /// The type mask
         /// </summary>
         TypeMask = 7,
         /// <summary>
         /// Compact representation of a sequence or mapping. Used only by YAML writer
         /// </summary>
         Flow = 8,
         /// <summary>
         /// A registered object (e.g. a matrix)
         /// </summary>
         User = 16,
         /// <summary>
         /// Empty structure (sequence or mapping)
         /// </summary>
         Empty = 32,
         /// <summary>
         /// The node has a name (i.e. it is element of a mapping)
         /// </summary>
         Named = 64
      };

      internal FileNode(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Reads a Mat from the node
      /// </summary>
      /// <param name="mat">The Mat where the result is read into</param>
      /// <param name="defaultMat">The default mat.</param>
      public void ReadMat(Mat mat, Mat defaultMat = null)
      {
         if (defaultMat == null)
         {
            using (Mat def = new Mat())
               CvInvoke.cveFileNodeReadMat(_ptr, mat, def);
         }
         else
         {
            CvInvoke.cveFileNodeReadMat(_ptr, mat, defaultMat);
         }
      }

      /// <summary>
      /// Gets a value indicating whether this instance is empty.
      /// </summary>
      /// <value>
      ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
      /// </value>
      public bool IsEmpty
      {
         get { return CvInvoke.cveFileNodeIsEmpty(_ptr); }
      }

      /// <summary>
      /// Gets the type of the node.
      /// </summary>
      /// <value>
      /// The type of the node.
      /// </value>
      public Type NodeType
      {
         get { return (Type)CvInvoke.cveFileNodeGetType(_ptr); }
      }

      /// <summary>
      /// Release the unmanaged resources
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveFileNodeRelease(ref _ptr);
      }

      /// <summary>
      /// Reads the string from the node
      /// </summary>
      /// <returns>The string from the node</returns>
      public String ReadString(String defaultString = null)
      {
         using (CvString s = new CvString())
         using (CvString ds = new CvString(defaultString))
         {
            CvInvoke.cveFileNodeReadString(_ptr, s, ds);
            return s.ToString();
         }
      }

      /// <summary>
      /// Reads the int from the node.
      /// </summary>
      /// <returns>The int from the node.</returns>
      public int ReadInt(int defaultInt = int.MinValue)
      {
         return CvInvoke.cveFileNodeReadInt(_ptr, defaultInt);
      }

      /// <summary>
      /// Reads the float from the node.
      /// </summary>
      /// <returns>The float from the node.</returns>
      public float ReadFloat(float defaultFloat = float.MinValue)
      {
         return CvInvoke.cveFileNodeReadFloat(_ptr, defaultFloat);
      }

      /// <summary>
      /// Reads the double from the node.
      /// </summary>
      /// <returns>The double from the node.</returns>
      public double ReadDouble(double defaultDouble = Double.MinValue)
      {
         return CvInvoke.cveFileNodeReadDouble(_ptr, defaultDouble);
      }

   }

   public static partial class CvInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFileNodeRelease(ref IntPtr node);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFileNodeReadMat(IntPtr node, IntPtr mat, IntPtr defaultMat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveFileNodeGetType(IntPtr node);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool cveFileNodeIsEmpty(IntPtr node);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFileNodeReadString(IntPtr node, IntPtr str, IntPtr defaultStr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveFileNodeReadInt(IntPtr node, int defaultInt);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern double cveFileNodeReadDouble(IntPtr node, double defaultDouble);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern float cveFileNodeReadFloat(IntPtr node, float defaultFloat);
   }
}
