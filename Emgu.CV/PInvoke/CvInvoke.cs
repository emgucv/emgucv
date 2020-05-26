//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.CodeDom;
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
    /// Class that provide access to native OpenCV functions
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
                if (Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
                {
                    subfolder = IntPtr.Size == 8 ? "x64" : "x86";
                }
#endif

                /*
                else if (Platform.OperationSystem == Emgu.Util.TypeEnum.OS.MacOS)
                {
                   subfolder = "..";
                }*/

                System.Reflection.Assembly asm = typeof(CvInvoke).Assembly; //System.Reflection.Assembly.GetExecutingAssembly();
                if ((String.IsNullOrEmpty(asm.Location) || !File.Exists(asm.Location)))
                {
                    if (String.IsNullOrEmpty(AppDomain.CurrentDomain.BaseDirectory))
                    {
                        loadDirectory = String.Empty;
                    }
                    else
                    {
                        //we may be running in a debugger visualizer under a unit test in this case
                        String visualStudioDir = AppDomain.CurrentDomain.BaseDirectory;
                        DirectoryInfo visualStudioDirInfo = new DirectoryInfo(visualStudioDir);
                        String debuggerVisualizerPath =
                            Path.Combine(visualStudioDirInfo.Parent.FullName, "Packages", "Debugger", "Visualizers");

                        if (Directory.Exists(debuggerVisualizerPath))
                            loadDirectory = debuggerVisualizerPath;
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
                {
                    var temp = Path.Combine(loadDirectory, subfolder);
                    if (Directory.Exists(temp))
                    {
                        loadDirectory = temp;
                    }
                    else
                    {
                        loadDirectory = Path.Combine(Path.GetFullPath("."), subfolder);
                    }
                }

#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR_WIN)
            if (String.IsNullOrEmpty(asm.Location) || !File.Exists(asm.Location))
            {
                Debug.WriteLine(String.Format("UNITY_STANDALONE_WIN: asm.Location is invalid: '{0}'", asm.Location));
                return false;
            }
                
            
            FileInfo file = new FileInfo(asm.Location);
            DirectoryInfo directory = file.Directory;
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
            
#elif UNITY_ANDROID
#else
                if (!Directory.Exists(loadDirectory))
                {
                    //try to find an alternative loadDirectory path
                    //The following code should handle finding the asp.NET BIN folder 
                    String altLoadDirectory = Path.GetDirectoryName(asm.CodeBase);
                    if (!String.IsNullOrEmpty(altLoadDirectory) && altLoadDirectory.StartsWith(@"file:\"))
                        altLoadDirectory = altLoadDirectory.Substring(6);

                    if (!String.IsNullOrEmpty(subfolder))
                        altLoadDirectory = Path.Combine(altLoadDirectory, subfolder);

                    if (!Directory.Exists(altLoadDirectory))
                    {
                        if (String.IsNullOrEmpty(asm.Location) || !File.Exists(asm.Location))
                        {
                            Debug.WriteLine(String.Format("asm.Location is invalid: '{0}'", asm.Location));
                            return false;
                        }
                        FileInfo file = new FileInfo(asm.Location);
                        DirectoryInfo directory = file.Directory;
#if UNITY_EDITOR_WIN
              if (directory.Parent != null && directory.Parent.Parent != null)
                  {
                     String unityAltFolder =
                        Path.Combine(
                           Path.Combine(Path.Combine(Path.Combine(directory.Parent.Parent.FullName, "Assets"), "Emgu.CV"), "Plugins"),
                           subfolder);
                     
					 Debug.WriteLine("Trying unityAltFolder: " + unityAltFolder);
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
            if (!String.IsNullOrEmpty(loadDirectory) && Directory.Exists(loadDirectory))
            {
                Environment.CurrentDirectory = loadDirectory;
                if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
                {
                    bool setDllDirectorySuccess = Emgu.Util.Toolbox.SetDllDirectory(loadDirectory);
                }
            }
            
#endif

            System.Diagnostics.Debug.WriteLine(String.Format("Loading open cv binary from {0}", loadDirectory));
            bool success = true;

            string prefix = string.Empty;

            foreach (String module in unmanagedModules)
            {
                string mName = module;

                //handle special case for universal build
                if (
                   mName.StartsWith("opencv_videoio_ffmpeg")  //opencv_ffmpegvvv(_64).dll
                   && (IntPtr.Size == 4) //32bit application
                   )
                {
                    mName = module.Replace("_64", String.Empty);
                }

                String fullPath = Path.Combine(prefix, mName);

                //Use absolute path for Windows Desktop
                fullPath = Path.Combine(loadDirectory, fullPath);

                bool fileExist = File.Exists(fullPath);
                if (!fileExist)
                    System.Diagnostics.Trace.WriteLine(String.Format("File {0} do not exist.", fullPath));
                bool fileExistAndLoaded = fileExist && !IntPtr.Zero.Equals(Toolbox.LoadLibrary(fullPath));
                if (fileExist && (!fileExistAndLoaded))
                    System.Diagnostics.Trace.WriteLine(String.Format("File {0} cannot be loaded.", fullPath));
                success &= fileExistAndLoaded;
            }


            Environment.CurrentDirectory = oldDir;

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
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
                formatString = "{0}.dll";
            else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Linux)
                formatString = "lib{0}.so";
            else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.MacOS)
                formatString = "lib{0}.dylib";
            return formatString;
#endif
        }

        /// <summary>
        /// Attempts to load opencv modules from the specific location
        /// </summary>
        /// <param name="modules">The names of opencv modules. e.g. "opencv_core.dll" on windows.</param>
        /// <returns>True if all the modules has been loaded successfully</returns>
        public static bool DefaultLoadUnmanagedModules(String[] modules)
        {
            bool libraryLoaded = true;

#if (UNITY_ANDROID && !UNITY_EDITOR)
            UnityEngine.AndroidJavaObject jo = new UnityEngine.AndroidJavaObject("java.lang.System");
            foreach (String module in modules)
            {
                try
                {
                   Console.WriteLine(string.Format("Trying to load {0}.", module));
                   jo.CallStatic("loadLibrary", module); 
                   Console.WriteLine(string.Format("Loaded {0}.", module));
                }
                catch (Exception e)
                {
                   libraryLoaded = false;
                   Console.WriteLine(String.Format("Failed to load {0}: {1}", module, e.Message));
                }
            }
#elif UNITY_IOS
#else
            if (Emgu.Util.Platform.OperationSystem == Platform.OS.IOS)
                return libraryLoaded;
            else if (Emgu.Util.Platform.OperationSystem == Platform.OS.Android)
            {
                System.Reflection.Assembly monoAndroidAssembly = Emgu.Util.Toolbox.FindAssembly("Mono.Android.dll");

                //Running on Xamarin Android
                Type javaSystemType = monoAndroidAssembly.GetType("Java.Lang.JavaSystem");
                if (javaSystemType != null)
                {
                    System.Reflection.MethodInfo loadLibraryMethodInfo = javaSystemType.GetMethod("LoadLibrary");
                    if (loadLibraryMethodInfo != null)
                    {
                        foreach (String module in modules)
                        {
                            if (module.StartsWith("opencv_videoio_ffmpeg"))
                                continue; //skip the ffmpeg modules.
                            try
                            {
                                System.Diagnostics.Trace.WriteLine(string.Format("Trying to load {0} ({1} bit).", module,
                                    IntPtr.Size * 8));
                                loadLibraryMethodInfo.Invoke(null, new object[] { module });
                                //Java.Lang.JavaSystem.LoadLibrary(module);
                                System.Diagnostics.Trace.WriteLine(string.Format("Loaded {0}.", module));
                            }
                            catch (Exception e)
                            {
                                libraryLoaded = false;
                                System.Diagnostics.Trace.WriteLine(String.Format("Failed to load {0}: {1}", module, e.Message));
                            }
                        }
                        return libraryLoaded;
                    }
                }
            } else if (Emgu.Util.Platform.OperationSystem != Emgu.Util.Platform.OS.MacOS)
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
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.IOS)
            {
                /*
                Assembly assembly = Assembly.GetExecutingAssembly();

                System.Runtime.InteropServices.DllImportResolver resolver =
                    (string libraryName, Assembly asm, DllImportSearchPath? dllImportSearchPath) =>
                    {
                        if (dllImportSearchPath != DllImportSearchPath.System32)
                        {
                            Console.WriteLine($"Unexpected dllImportSearchPath: {dllImportSearchPath.ToString()}");
                            throw new ArgumentException();
                        }

                        return System.Runtime.InteropServices.NativeLibrary.Load("ResolveLib", asm, null);
                    };
                    */
                return;
            }

            List<String> modules = CvInvoke.OpenCVModuleList;
            modules.RemoveAll(String.IsNullOrEmpty);

            _libraryLoaded = DefaultLoadUnmanagedModules(modules.ToArray());

//#if !UNITY_IOS
            if (Emgu.Util.Platform.OperationSystem != Emgu.Util.Platform.OS.IOS)
            {
                //Use the custom error handler
                RedirectError(CvErrorHandlerThrowException, IntPtr.Zero, IntPtr.Zero);
            }
//#endif
        }

        #region CV MACROS

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

            sane &= (sizes.CvBox2D == Toolbox.SizeOf<RotatedRect>());
            sane &= (sizes.CvMat == Toolbox.SizeOf<MCvMat>());
            sane &= (sizes.CvMatND == Toolbox.SizeOf<MCvMatND>());
            sane &= (sizes.CvPoint == Toolbox.SizeOf<System.Drawing.Point>());
            sane &= (sizes.CvPoint2D32f == Toolbox.SizeOf<System.Drawing.PointF>());
            sane &= (sizes.CvPoint3D32f == Toolbox.SizeOf<MCvPoint3D32f>());
            sane &= (sizes.CvRect == Toolbox.SizeOf<System.Drawing.Rectangle>());
            sane &= (sizes.CvScalar == Toolbox.SizeOf<MCvScalar>());
            sane &= (sizes.CvSize == Toolbox.SizeOf<System.Drawing.Size>());
            sane &= (sizes.CvSize2D32f == Toolbox.SizeOf<System.Drawing.SizeF>());
            sane &= (sizes.CvTermCriteria == Toolbox.SizeOf<MCvTermCriteria>());
            sane &= 2 * Toolbox.SizeOf<int>() == Toolbox.SizeOf<Emgu.CV.Structure.Range>();
            return sane;
        }
    }
}