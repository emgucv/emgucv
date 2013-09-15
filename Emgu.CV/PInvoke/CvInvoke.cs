//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

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
      /// Represent a bool value in C++
      /// </summary>
      public const UnmanagedType BoolMarshalType = UnmanagedType.U1;

      /// <summary>
      /// Represent a int value in C++
      /// </summary>
      public const UnmanagedType BoolToIntMarshalType = UnmanagedType.Bool;

      /// <summary>
      /// Opencv's calling convention
      /// </summary>
      public const CallingConvention CvCallingConvention = CallingConvention.Cdecl;

      /// <summary>
      /// Attemps to load opencv modules from the specific location
      /// </summary>
      /// <param name="loadDirectory">The directory where the unmanaged modules will be loaded. If it is null, the default location will be used.</param>
      /// <param name="unmanagedModules">The names of opencv modules. e.g. "opencv_cxcore.dll" on windows.</param>
      /// <returns>True if all the modules has been loaded sucessfully</returns>
      /// <remarks>If <paramref name="loadDirectory"/> is null, the default location on windows is the dll's path appended by either "x64" or "x86", depends on the applications current mode.</remarks>
      public static bool LoadUnmanagedModules(String loadDirectory, params String[] unmanagedModules)
      {
#if NETFX_CORE
         if (loadDirectory != null)
         {
            throw new NotImplementedException("Loading modules from a specific directory is not implemented in Windows Store App");
         }

         String subfolder = String.Empty;
         if (Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
         {
            if (IntPtr.Size == 8)
            {  //64bit process
               subfolder = "x64";
            }
            else
            {
               subfolder = "x86";
            }
         }

         Windows.Storage.StorageFolder installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
         loadDirectory = Path.Combine(installFolder.Path, subfolder);

         var t = System.Threading.Tasks.Task.Run(async () => 
         {
            Windows.Storage.StorageFolder loadFolder = await installFolder.GetFolderAsync(subfolder);
            List<string> files = new List<string>();
            foreach (var file in await loadFolder.GetFilesAsync())
            {
               files.Add(file.Path);
            }
            return files;
         });
         t.Wait();

         List<String> loadableFiles = t.Result;
#else
         if (loadDirectory == null)
         {
            String subfolder = String.Empty;
            if (Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
            {
               if (IntPtr.Size == 8)
               {  //64bit process
                  subfolder = "x64";
               }
               else
               {
                  subfolder = "x86";
               }
            }  
            /*
            else if (Platform.OperationSystem == Emgu.Util.TypeEnum.OS.MacOSX)
            {
               subfolder = "..";
            }*/

            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo file = new FileInfo(asm.Location);
            //FileInfo file = new FileInfo(asm.CodeBase);
            DirectoryInfo directory = file.Directory;
            loadDirectory = directory.FullName;
            
            if (!String.IsNullOrEmpty(subfolder))
            loadDirectory = Path.Combine(loadDirectory, subfolder);

            if (!Directory.Exists(loadDirectory))
            {
               //try to find an alternative loadDirectory path
               //The following code should handle finding the asp.NET BIN folder 
               String altLoadDirectory = Path.GetDirectoryName(asm.CodeBase);
               if (altLoadDirectory.StartsWith(@"file:\"))
                  altLoadDirectory = altLoadDirectory.Substring(6);

               if (!String.IsNullOrEmpty(subfolder))
                  altLoadDirectory = Path.Combine(altLoadDirectory, subfolder);

               if (!Directory.Exists(altLoadDirectory))
                  return false;
               else
                  loadDirectory = altLoadDirectory;
            }
         }
         
         String oldDir = Environment.CurrentDirectory;
         Environment.CurrentDirectory = loadDirectory;
#endif
         bool success = true;

         string prefix = string.Empty;
         
         foreach (String module in unmanagedModules)
         {
            string mName = module;

            //handle special case for universal build
            if (
               mName.StartsWith("opencv_ffmpeg")  //opencv_ffmpegvvv(_64).dll
               && (IntPtr.Size == 4) //32bit application
               )
            {
               mName = module.Replace("_64", String.Empty);
            }

            String fullPath = Path.Combine(loadDirectory, Path.Combine(prefix, mName));

#if NETFX_CORE
            if (loadableFiles.Exists(sf => sf.Equals(fullPath)))
            {
               IntPtr handle = Toolbox.LoadLibrary(fullPath);
               success &= (!IntPtr.Zero.Equals(handle));
            }
            else
            {
               success = false;
            }
#else
            success &= (File.Exists(fullPath) && !IntPtr.Zero.Equals(Toolbox.LoadLibrary(fullPath)));
#endif
         }

#if !NETFX_CORE
         Environment.CurrentDirectory = oldDir;
#endif
         return success;
      }

      /// <summary>
      /// Get the module format string.
      /// </summary>
      /// <returns>On Windows, "{0}".dll will be returned; On Linux, "lib{0}.so" will be returned; Otherwise {0} is returned.</returns>
      public static String GetModuleFormatString()
      {
         String formatString = "{0}";
         if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
            formatString = "{0}.dll";
         else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Linux)
            formatString = "lib{0}.so";
         else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.MacOSX)
            formatString = "lib{0}.dylib";
         return formatString;
      }

      /// <summary>
      /// Static Constructor to setup opencv environment
      /// </summary>
      static CvInvoke()
      {
         /*
         List<string> modules = new List<String> 
         {
            CvInvoke.OPENCV_FFMPEG_LIBRARY, 

            CvInvoke.OPENCV_CORE_LIBRARY,
            CvInvoke.OPENCV_IMGPROC_LIBRARY,

            CvInvoke.OPENCV_VIDEO_LIBRARY,
            CvInvoke.OPENCV_FLANN_LIBRARY,
            CvInvoke.OPENCV_ML_LIBRARY,

            CvInvoke.OPENCV_HIGHGUI_LIBRARY,
            CvInvoke.OPENCV_OBJDETECT_LIBRARY,
            CvInvoke.OPENCV_FEATURES2D_LIBRARY,
            CvInvoke.OPENCV_CALIB3D_LIBRARY,

            CvInvoke.OPENCV_LEGACY_LIBRARY,

            CvInvoke.OPENCV_CONTRIB_LIBRARY,

            CvInvoke.OPENCV_GPULEGACY_LIBRARY,
            CvInvoke.OPENCV_GPUARITHM_LIBRARY,
            CvInvoke.OPENCV_GPUWARPING_LIBRARY,
            CvInvoke.OPENCV_GPU_LIBRARY, 
            CvInvoke.OPENCV_GPUFILTERS_LIBRARY,
            CvInvoke.OPENCV_GPUIMGPROC_LIBRARY,
            CvInvoke.OPENCV_GPUOPTFLOW_LIBRARY,
            CvInvoke.OPENCV_GPUSTEREO_LIBRARY,
            CvInvoke.OPENCV_GPUWARPING_LIBRARY,

            CvInvoke.OPENCV_OCL_LIBRARY,

            CvInvoke.OPENCV_PHOTO_LIBRARY,
            CvInvoke.OPENCV_BIOINSPIRED_LIBRARY,
            CvInvoke.OPENCV_VIDEOSTAB_LIBRARY,
            CvInvoke.OPENCV_SUPERRES_LIBRARY,
            CvInvoke.OPENCV_NONFREE_LIBRARY,
            CvInvoke.OPENCV_STITCHING_LIBRARY,

            //CvInvoke.EXTERN_GPU_LIBRARY,
            CvInvoke.EXTERN_LIBRARY

         };*/
         List<String> modules = CvInvoke.OpenCVModuleList;
         modules.RemoveAll(String.IsNullOrEmpty);

#if ANDROID
         System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
         FileInfo file = new FileInfo(asm.Location);
         DirectoryInfo directory = file.Directory;

         foreach (String module in modules)
         {
            //IntPtr handle = Emgu.Util.Toolbox.LoadLibrary(module);
            //Debug.WriteLine(string.Format(handle == IntPtr.Zero ? "Failed to load {0}." : "Loaded {0}.", module));
            Java.Lang.JavaSystem.LoadLibrary(module);
            Debug.WriteLine(string.Format("Loaded {0}.", module));
         }
#elif IOS 
#else
         if (Platform.OperationSystem != Emgu.Util.TypeEnum.OS.MacOSX)
         {
            String formatString = GetModuleFormatString();
            for (int i = 0; i < modules.Count; ++i)
               modules[i] = String.Format(formatString, modules[i]);

            LoadUnmanagedModules(null, modules.ToArray());
         }
#endif
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

      /// <summary>
      /// Check if the size of the C structures match those of C#
      /// </summary>
      /// <returns>True if the size matches</returns>
      public static bool SanityCheck()
      {
         bool sane = true;
         CvStructSizes sizes = new CvStructSizes();
         CvInvoke.GetCvStructSizes(ref sizes);

         sane &= (sizes.CvBox2D == Marshal.SizeOf(typeof(MCvBox2D)));
         sane &= (sizes.CvContour == Marshal.SizeOf(typeof(MCvContour)));
         sane &= (sizes.CvHistogram == Marshal.SizeOf(typeof(MCvHistogram)));
         sane &= (sizes.CvMat == Marshal.SizeOf(typeof(MCvMat)));
         sane &= (sizes.CvMatND == Marshal.SizeOf(typeof(MCvMatND)));
         sane &= (sizes.CvPoint == Marshal.SizeOf(typeof(System.Drawing.Point)));
         sane &= (sizes.CvPoint2D32f == Marshal.SizeOf(typeof(System.Drawing.PointF)));
         sane &= (sizes.CvPoint3D32f == Marshal.SizeOf(typeof(MCvPoint3D32f)));
         sane &= (sizes.CvRect == Marshal.SizeOf(typeof(System.Drawing.Rectangle)));
         sane &= (sizes.CvScalar == Marshal.SizeOf(typeof(MCvScalar)));
         sane &= (sizes.CvSeq == Marshal.SizeOf(typeof(MCvSeq)));
         sane &= (sizes.CvSize == Marshal.SizeOf(typeof(System.Drawing.Size)));
         sane &= (sizes.CvSize2D32f == Marshal.SizeOf(typeof(System.Drawing.SizeF)));
         sane &= (sizes.CvTermCriteria == Marshal.SizeOf(typeof(MCvTermCriteria)));


         return sane;
      }
   }
}