//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using Platform = Emgu.Util.Platform;

namespace Emgu.CV
{
   /// <summary>
   /// Library to invoke OpenCV functions
   /// </summary>
   public static partial class CvInvoke
   {
      private static readonly bool _libraryLoaded;

      /// <summary>
      /// Check to make sure all the unmanaged libraries are loaded
      /// </summary>
      /// <returns>True if library loaded</returns>
      public static bool CheckLibraryLoaded()
      {
            return _libraryLoaded;
      }

      
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
      /// Attempts to load opencv modules from the specific location
      /// </summary>
      /// <param name="loadDirectory">The directory where the unmanaged modules will be loaded. If it is null, the default location will be used.</param>
      /// <param name="unmanagedModules">The names of opencv modules. e.g. "opencv_cxcore.dll" on windows.</param>
      /// <returns>True if all the modules has been loaded successfully</returns>
      /// <remarks>If <paramref name="loadDirectory"/> is null, the default location on windows is the dll's path appended by either "x64" or "x86", depends on the applications current mode.</remarks>
      public static bool LoadUnmanagedModules(String loadDirectory, params String[] unmanagedModules)
      {
#if NETFX_CORE
         if (loadDirectory != null)
         {
            throw new NotImplementedException("Loading modules from a specific directory is not implemented in Windows Store App");
         }

         String subfolder = String.Empty;
         if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows) //|| Platform.OperationSystem == Emgu.Util.TypeEnum.OS.WindowsPhone)
         {
            if (IntPtr.Size == 8)
            {  //64bit process
#if UNITY_METRO
               subfolder = "x86_64";
#else
               subfolder = String.Empty;
#endif
            }
            else
            {
               subfolder = String.Empty;
            }
         }

         Windows.Storage.StorageFolder installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
         
#if UNITY_METRO
         loadDirectory = Path.Combine(
            Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName( installFolder.Path))))
            , "Plugins", "Metro", subfolder);
#else
         loadDirectory = Path.Combine(installFolder.Path, subfolder);
#endif

         var t = System.Threading.Tasks.Task.Run(async () =>
         {
            List<string> files = new List<string>();
            Windows.Storage.StorageFolder loadFolder = installFolder;
            try
            {
               
               if (!String.IsNullOrEmpty(subfolder))
                  loadFolder = await installFolder.GetFolderAsync(subfolder);

               foreach (var file in await loadFolder.GetFilesAsync())
                  files.Add(file.Name);
            }
            catch (Exception e)
            {
               System.Diagnostics.Debug.WriteLine(String.Format("Unable to retrieve files in folder '{0}':{1}", loadFolder.Path, e.StackTrace));
            }

            return files;
         });
         t.Wait();

         List<String> loadableFiles = t.Result;
#else
         if (loadDirectory == null)
         {
            String subfolder = String.Empty;
#if UNITY_EDITOR_WIN
            subfolder = IntPtr.Size == 8 ? "x86_64" : "x86";
#elif UNITY_STANDALONE_WIN
#else
            if (Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
            {
               subfolder = IntPtr.Size == 8 ? "x64" : "x86";
            }
#endif

            /*
            else if (Platform.OperationSystem == Emgu.Util.TypeEnum.OS.MacOSX)
            {
               subfolder = "..";
            }*/

            System.Reflection.Assembly asm = typeof (CvInvoke).Assembly; //System.Reflection.Assembly.GetExecutingAssembly();
            if ((String.IsNullOrEmpty(asm.Location) || !File.Exists(asm.Location)) && AppDomain.CurrentDomain.BaseDirectory != null)
            {
               //if may be running in a debugger visualizer under a unit test in this case
               String visualStudioDir = AppDomain.CurrentDomain.BaseDirectory;
               DirectoryInfo visualStudioDirInfo = new DirectoryInfo(visualStudioDir);
               String debuggerVisualzerPath = 
                  Path.Combine(Path.Combine(Path.Combine(
                  visualStudioDirInfo.Parent.FullName, "Packages"), "Debugger"), "Visualizers");

               if (Directory.Exists(debuggerVisualzerPath))
                  loadDirectory = debuggerVisualzerPath;
               else
                  loadDirectory = String.Empty;
/*
               loadDirectory = Path.GetDirectoryName(new UriBuilder(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Path);

               DirectoryInfo dir = new DirectoryInfo(loadDirectory);
               string subdir = String.Join(";", Array.ConvertAll(dir.GetDirectories(), d => d.ToString()));
               
               throw new Exception(String.Format(
                  "The Emgu.CV.dll assembly path (typeof (CvInvoke).Assembly.Location) '{0}' is invalid." +
                  Environment.NewLine
                  + " Other possible path (System.Reflection.Assembly.GetExecutingAssembly().Location): '{1}';" +
                  Environment.NewLine
                  + " Other possible path (Path.GetDirectoryName(new UriBuilder(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Path): '{2}';" +
                  Environment.NewLine
                  + " Other possible path (System.Reflection.Assembly.GetExecutingAssembly().CodeBase): '{3};'" +
                  Environment.NewLine
                  + " Other possible path (typeof(CvInvoke).Assembly.CodeBase): '{4}'" +
                  Environment.NewLine
                  + " Other possible path (AppDomain.CurrentDomain.BaseDirectory): '{5}'" +
                  Environment.NewLine
                  + " subfolder name: '{6}'",
                  asm.Location,
                  Path.GetDirectoryName(new UriBuilder(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Path),
                  loadDirectory + ": subdir '" + subdir +"'",
                  System.Reflection.Assembly.GetExecutingAssembly().CodeBase,
                  typeof(CvInvoke).Assembly.Location,
                  AppDomain.CurrentDomain.BaseDirectory,
                  subfolder
                  ));
 */
            }
            else
            {
               loadDirectory = Path.GetDirectoryName(asm.Location);
            }
            /*
            FileInfo file = new FileInfo(asm.Location);
            //FileInfo file = new FileInfo(asm.CodeBase);
            DirectoryInfo directory = file.Directory;
            loadDirectory = directory.FullName;
            */

            if (!String.IsNullOrEmpty(subfolder))
               loadDirectory = Path.Combine(loadDirectory, subfolder);
            
#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR_WIN)
            if (directory.Parent != null)
            {
               String unityAltFolder = Path.Combine(directory.Parent.FullName, "Plugins");
              
               if (Directory.Exists(unityAltFolder))
                  loadDirectory = unityAltFolder;
               else
               {
                  Debug.WriteLine("No suitable directory found to load unmanaged modules");
                  return false;
               }
            }
#elif ANDROID || UNITY_ANDROID
#else
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
               {
#if UNITY_EDITOR_WIN
              if (directory.Parent != null && directory.Parent.Parent != null)
                  {
                     String unityAltFolder =
                        Path.Combine(
                           Path.Combine(Path.Combine(Path.Combine(directory.Parent.Parent.FullName, "Assets"), "Emgu.CV"), "Plugins"),
                           subfolder);
                     
                     if (Directory.Exists(unityAltFolder))
                        loadDirectory = unityAltFolder;
                     else
                     {
                        Debug.WriteLine("No suitable directory found to load unmanaged modules");
                        return false;
                     }
                    
                  }
                  else
#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
                     if (directory.Parent != null && directory.Parent.Parent != null)
                  {
                     String unityAltFolder =
                        Path.Combine(Path.Combine(Path.Combine(
                           Path.Combine(Path.Combine(directory.Parent.Parent.FullName, "Assets"), "Plugins"),
                           "emgucv.bundle"), "Contents"), "MacOS");
                     
                     if (Directory.Exists(unityAltFolder))
                     {
                        loadDirectory = unityAltFolder;
                     }
                     else
                     {
                        return false;
                     }
                     
                  }
                  else       
#endif
                  {
                     Debug.WriteLine("No suitable directory found to load unmanaged modules");
                     return false;
                  }
               }
               else
                  loadDirectory = altLoadDirectory;
            }
#endif
         }
         
         String oldDir = Environment.CurrentDirectory;
         if (!String.IsNullOrEmpty(loadDirectory))
            Environment.CurrentDirectory = loadDirectory;
#endif

         System.Diagnostics.Debug.WriteLine(String.Format("Loading open cv binary from {0}", loadDirectory));
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
     
            String fullPath = Path.Combine(prefix, mName);

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
            //Use absolute path for Windows Desktop
            fullPath = Path.Combine(loadDirectory, fullPath);

            bool fileExist = File.Exists(fullPath);
            if (!fileExist)
               System.Diagnostics.Debug.WriteLine(String.Format("File {0} do not exist.", fullPath));
            bool fileExistAndLoaded = fileExist && !IntPtr.Zero.Equals(Toolbox.LoadLibrary(fullPath));
            if (fileExist && (!fileExistAndLoaded))
               System.Diagnostics.Debug.WriteLine(String.Format("File {0} cannot be loaded.", fullPath));
            success &= fileExistAndLoaded;
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
#if UNITY_EDITOR_WIN
         return "{0}.dll";
#elif UNITY_EDITOR_OSX
         return "lib{0}.dylib";
#else
         String formatString = "{0}";
         if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows 
            || Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.WindowsPhone)
            formatString = "{0}.dll";
         else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Linux)
            formatString = "lib{0}.so";
         else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.MacOSX)
            formatString = "lib{0}.dylib";
         return formatString;
#endif
      }

      /// <summary>
      /// Attempts to load opencv modules from the specific location
      /// </summary>
      /// <param name="modules">The names of opencv modules. e.g. "opencv_cxcore.dll" on windows.</param>
      /// <returns>True if all the modules has been loaded successfully</returns>
      public static bool DefaultLoadUnmanagedModules(String[] modules)
      {
         bool libraryLoaded = true;
#if ANDROID || (UNITY_ANDROID && !UNITY_EDITOR)

         System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
         FileInfo file = new FileInfo(asm.Location);
         DirectoryInfo directory = file.Directory;

#if (UNITY_ANDROID && !UNITY_EDITOR)
         UnityEngine.AndroidJavaObject jo = new UnityEngine.AndroidJavaObject("java.lang.System");
#endif
         foreach (String module in modules)
         {
            //IntPtr handle = Emgu.Util.Toolbox.LoadLibrary(module);
            //Debug.WriteLine(string.Format(handle == IntPtr.Zero ? "Failed to load {0}." : "Loaded {0}.", module));
            try
            {

               Console.WriteLine(string.Format("Trying to load {0}.", module));
#if ANDROID
               Java.Lang.JavaSystem.LoadLibrary(module);
#else //(UNITY_ANDROID && !UNITY_EDITOR)

               jo.CallStatic("loadLibrary", module); 
#endif
               Console.WriteLine(string.Format("Loaded {0}.", module));
            }
            catch (Exception e)
            {
               libraryLoaded = false;
               Console.WriteLine(String.Format("Failed to load {0}: {1}", module, e.Message));
            }
         }
#elif IOS || UNITY_IPHONE || NETFX_CORE
#else
         if (Emgu.Util.Platform.OperationSystem != Emgu.Util.TypeEnum.OS.MacOSX)
         {
            String formatString = GetModuleFormatString();
            for (int i = 0; i < modules.Length; ++i)
               modules[i] = String.Format(formatString, modules[i]);

            libraryLoaded &= LoadUnmanagedModules(null, modules);
         }
#endif
         return libraryLoaded;
      }

      /// <summary>
      /// Static Constructor to setup opencv environment
      /// </summary>
      static CvInvoke()
      {
         List<String> modules = CvInvoke.OpenCVModuleList;
         modules.RemoveAll(String.IsNullOrEmpty);

         _libraryLoaded = DefaultLoadUnmanagedModules(modules.ToArray());
         /*
         _libraryLoaded = true;
#if ANDROID || (UNITY_ANDROID && !UNITY_EDITOR)
		 
         System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
         FileInfo file = new FileInfo(asm.Location);
         DirectoryInfo directory = file.Directory;

#if (UNITY_ANDROID && !UNITY_EDITOR)
         UnityEngine.AndroidJavaObject jo = new UnityEngine.AndroidJavaObject("java.lang.System");
#endif
         foreach (String module in modules)
         {
            //IntPtr handle = Emgu.Util.Toolbox.LoadLibrary(module);
            //Debug.WriteLine(string.Format(handle == IntPtr.Zero ? "Failed to load {0}." : "Loaded {0}.", module));
            try
            {
			
               Console.WriteLine(string.Format("Trying to load {0}.", module));
#if ANDROID
               Java.Lang.JavaSystem.LoadLibrary(module);
#else //(UNITY_ANDROID && !UNITY_EDITOR)

               jo.CallStatic("loadLibrary", module); 
#endif
               Console.WriteLine(string.Format("Loaded {0}.", module));
            }
            catch (Exception e)
            {
               _libraryLoaded = false; 
               Console.WriteLine(String.Format("Failed to load {0}: {1}", module, e.Message));
            }
         }
#elif IOS || UNITY_IPHONE || NETFX_CORE
#else
         if (Emgu.Util.Platform.OperationSystem != Emgu.Util.TypeEnum.OS.MacOSX)
         {
            String formatString = GetModuleFormatString();
            for (int i = 0; i < modules.Count; ++i)
               modules[i] = String.Format(formatString, modules[i]);

            _libraryLoaded &= LoadUnmanagedModules(null, modules.ToArray());
         }
#endif*/

#if !UNITY_IPHONE
         //Use the custom error handler
         RedirectError(CvErrorHandlerThrowException, IntPtr.Zero, IntPtr.Zero);
#endif
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
      /*
      /// <summary>
      /// This function performs the same as MakeType macro
      /// </summary>
      /// <param name="depth">The type of depth</param>
      /// <param name="cn">The number of channels</param>
      /// <returns></returns>
      public static int MakeType(int depth, int cn)
      {
         return ((depth) + (((cn) - 1) << 3));
      }*/

      /// <summary>
      /// Get the corresponding depth type
      /// </summary>
      /// <param name="t">The opencv depth type</param>
      /// <returns>The equivalent depth type</returns>
      public static Type GetDepthType(CvEnum.DepthType t)
      {
         switch (t)
         {
            case CvEnum.DepthType.Cv8U:
               return typeof(byte);
            case CvEnum.DepthType.Cv8S:
               return typeof(SByte);
            case CvEnum.DepthType.Cv16U:
               return typeof(UInt16);
            case CvEnum.DepthType.Cv16S:
               return typeof(Int16);
            case CvEnum.DepthType.Cv32S:
               return typeof(Int32);
            case CvEnum.DepthType.Cv32F:
               return typeof(float);
            case CvEnum.DepthType.Cv64F:
               return typeof(double);
            default:
               throw new ArgumentException(String.Format("Unable to convert type {0} to depth type", t.ToString()));
         }
      }

      /// <summary>
      /// Get the corresponding opencv depth type
      /// </summary>
      /// <param name="t">The element type</param>
      /// <returns>The equivalent opencv depth type</returns>
      public static CvEnum.DepthType GetDepthType(Type t)
      {
         if (t == typeof(byte))
         {
            return CvEnum.DepthType.Cv8U;
         }
         else if (t == typeof(SByte))
         {
            return CvEnum.DepthType.Cv8S;
         }
         else if (t == typeof(UInt16))
         {
            return CvEnum.DepthType.Cv16U;
         }
         else if (t == typeof(Int16))
         {
            return CvEnum.DepthType.Cv16S;
         }
         else if (t == typeof(Int32))
         {
            return CvEnum.DepthType.Cv32S;
         }
         else if (t == typeof(float))
         {
            return CvEnum.DepthType.Cv32F;
         }
         else if (t == typeof(double))
         {
            return CvEnum.DepthType.Cv64F;
         }
         else
         {
            throw new ArgumentException(String.Format("Unable to convert type {0} to depth type", t.ToString()));
         }
      }

      /// <summary>
      /// This function performs the same as MakeType macro
      /// </summary>
      /// <param name="depth">The type of depth</param>
      /// <param name="channels">The number of channels</param>
      /// <returns>An interger tha represent a mat type</returns>
      public static int MakeType(CvEnum.DepthType depth, int channels)
      {
         const int shift = 3;
         return (((int)depth) & ((1 << shift) - 1)) + (((channels) - 1) << shift);
      }

      /*
      private static int _CV_DepthType(int flag)
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
         return (_CV_MAT_CN(type) << ((((4 / 4 + 1) * 16384 | 0x3a50) >> _CV_DepthType(type) * 2) & 3));
      }

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
      }*/
      #endregion

      /// <summary>
      /// Check if the size of the C structures match those of C#
      /// </summary>
      /// <returns>True if the size matches</returns>
      public static bool SanityCheck()
      {
         bool sane = true;

         CvStructSizes sizes = CvInvoke.GetCvStructSizes();

#if NETFX_CORE
         sane &= (sizes.CvBox2D == Marshal.SizeOf<RotatedRect>());
         sane &= (sizes.CvContour == Marshal.SizeOf<MCvContour>());
         //sane &= (sizes.CvHistogram == Marshal.SizeOf(typeof(MCvHistogram)));
         sane &= (sizes.CvMat == Marshal.SizeOf<MCvMat>());
         sane &= (sizes.CvMatND == Marshal.SizeOf<MCvMatND>());
         sane &= (sizes.CvPoint == Marshal.SizeOf<System.Drawing.Point>());
         sane &= (sizes.CvPoint2D32f == Marshal.SizeOf<System.Drawing.PointF>());
         sane &= (sizes.CvPoint3D32f == Marshal.SizeOf<MCvPoint3D32f>());
         sane &= (sizes.CvRect == Marshal.SizeOf<System.Drawing.Rectangle>());
         sane &= (sizes.CvScalar == Marshal.SizeOf<MCvScalar>());
         sane &= (sizes.CvSeq == Marshal.SizeOf<MCvSeq>());
         sane &= (sizes.CvSize == Marshal.SizeOf<System.Drawing.Size>());
         sane &= (sizes.CvSize2D32f == Marshal.SizeOf<System.Drawing.SizeF>());
         sane &= (sizes.CvTermCriteria == Marshal.SizeOf<MCvTermCriteria>());

#else
         sane &= (sizes.CvBox2D == Marshal.SizeOf(typeof(RotatedRect)));
         sane &= (sizes.CvContour == Marshal.SizeOf(typeof(MCvContour)));
         //sane &= (sizes.CvHistogram == Marshal.SizeOf(typeof(MCvHistogram)));
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
#endif

         return sane;
      }
   }
}