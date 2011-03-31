//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Library to invoke OpenCV functions
   /// </summary>
   public static partial class CvInvoke
   {
      /// <summary>
      /// string marshaling type
      /// </summary>
      public const UnmanagedType StringMarshalType = UnmanagedType.LPStr;

      /// <summary>
      /// bool marshaling type
      /// </summary>
      public const UnmanagedType BoolMarshalType = UnmanagedType.U1;

      /// <summary>
      /// int marshaling type
      /// </summary>
      public const UnmanagedType BoolToIntMarshalType = UnmanagedType.Bool;

      /// <summary>
      /// Opencv's calling convention
      /// </summary>
      public const CallingConvention CvCallingConvention = CallingConvention.Cdecl;

      /// <summary>
      /// Static Constructor to setup opencv environment
      /// </summary>
      static CvInvoke()
      {
         /*
         if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
         {
            
            //System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            //System.IO.FileInfo file = new System.IO.FileInfo(asm.Location);
            //System.IO.DirectoryInfo directory = file.Directory;
            //System.Security.AccessControl.DirectorySecurity security = directory.GetAccessControl();
            //Emgu.Util.Toolbox.SetDllDirectory(directory.FullName);
           
            String loadLibraryErrorMessage =
               "Unable to load {0}. Please check the following: 1. {0} is located in the same folder as Emgu.CV.dll; 2. MSVCRT 8.0 SP1 is installed.";
            LoadLibrary(CXCORE_LIBRARY, loadLibraryErrorMessage);
            LoadLibrary(CV_LIBRARY, loadLibraryErrorMessage);
            LoadLibrary(HIGHGUI_LIBRARY, loadLibraryErrorMessage);
            LoadLibrary(CVAUX_LIBRARY, loadLibraryErrorMessage);
         }*/

         //Use the custom error handler
         cvRedirectError(CvErrorHandlerThrowException, IntPtr.Zero, IntPtr.Zero);
      }

      /*
      private static void LoadLibrary(string libraryName, string errorMessage)
      {
         errorMessage = String.Format(errorMessage, libraryName);
         try
         {
            IntPtr handle = Emgu.Util.Toolbox.LoadLibrary(libraryName);
            if (handle == IntPtr.Zero)
               throw new DllNotFoundException(errorMessage);
         }
         catch (Exception e)
         {
            throw new DllNotFoundException(errorMessage, e);
         }
      }*/

      #region CV MACROS

      /// <summary>
      /// This function performs the same as CV_MAKETYPE macro
      /// </summary>
      /// <param name="depth">The type of depth</param>
      /// <param name="cn">The number of channels</param>
      /// <returns></returns>
      public static int CV_MAKETYPE(int depth, int cn)
      {
         return ((depth) + (((cn) - 1) << 3));
      }

      /*
      private static int _CV_MAT_DEPTH(int flag)
      {
         return flag & ((1 << 3) - 1);
      }
      private static int _CV_MAT_TYPE(int type)
      {
         return type & ((1 << 3) * 64 - 1);
      }

      private static int _CV_MAT_CN(int flag)
      {
         return ((((flag) & ((64 - 1) << 3)) >> 3) + 1);
      }
      private static int _CV_ELEM_SIZE(int type)
      {
         return (_CV_MAT_CN(type) << ((((4 / 4 + 1) * 16384 | 0x3a50) >> _CV_MAT_DEPTH(type) * 2) & 3));
      }*/

      /// <summary>
      /// Generate 4-character code of codec used to compress the frames. For example, CV_FOURCC('P','I','M','1') is MPEG-1 codec, CV_FOURCC('M','J','P','G') is motion-jpeg codec etc.
      /// </summary>
      /// <param name="c1"></param>
      /// <param name="c2"></param>
      /// <param name="c3"></param>
      /// <param name="c4"></param>
      /// <returns></returns>
      public static int CV_FOURCC(char c1, char c2, char c3, char c4)
      {
         return (((c1) & 255) + (((c2) & 255) << 8) + (((c3) & 255) << 16) + (((c4) & 255) << 24));
      }
      #endregion
   }
}