//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
        public static bool Init()
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

        private static String FindValidSubfolders(String baseFolder, List<String> subfolderOptions)
        {
            foreach (String sfo in subfolderOptions)
            {
                if (Directory.Exists(Path.Combine(baseFolder, sfo)))
                {
                    return sfo;
                }
            }
            return String.Empty;
        }
        
        /// <summary>
        /// Attempts to load opencv modules from the specific location
        /// </summary>
        /// <param name="loadDirectory">The directory where the unmanaged modules will be loaded. If it is null, the default location will be used.</param>
        /// <param name="unmanagedModules">The names of opencv modules. e.g. "opencv_core.dll" on windows.</param>
        /// <returns>True if all the modules has been loaded successfully</returns>
        /// <remarks>If <paramref name="loadDirectory"/> is null, the default location on windows is the dll's path appended by either "x64" or "x86", depends on the applications current mode.</remarks>
        public static bool LoadUnmanagedModules(String loadDirectory, params String[] unmanagedModules)
        {
#if UNITY_WSA || UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
            if (loadDirectory != null)
            {
                throw new NotImplementedException("Loading modules from a specific directory is not implemented in Windows Store App");
            }
            //Let unity handle the library loading
            return true;
#else
            String oldDir = String.Empty;
            if (loadDirectory == null)
            {
                List<String> subfolderOptions = new List<string>();

                if (Platform.OperationSystem == Emgu.Util.Platform.OS.Windows 
                    || Platform.OperationSystem == Emgu.Util.Platform.OS.Linux)
                {
                    //var fd = RuntimeInformation.FrameworkDescription;
                    if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                    {
                        if (Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
                            subfolderOptions.Add(Path.Combine("runtimes", "win-x86", "native"));
                        subfolderOptions.Add("x86");
                    }
                    else if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    {
                        if (Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
                            subfolderOptions.Add(Path.Combine("runtimes", "win-x64", "native"));
                        subfolderOptions.Add("x64");
                    }
                    else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                    {
                        subfolderOptions.Add("arm");
                    }
                    else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                    {
                        subfolderOptions.Add("arm64");
                    }
                }

                String subfolder = String.Empty;
                
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
                        String baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        DirectoryInfo baseDirectoryInfo = new DirectoryInfo(baseDirectory);
                        String debuggerVisualizerPath = String.Empty;
                        if (baseDirectoryInfo.Parent != null)
                            debuggerVisualizerPath = Path.Combine(baseDirectoryInfo.Parent.FullName, "Packages", "Debugger", "Visualizers");

                        if (!debuggerVisualizerPath.Equals(String.Empty) && Directory.Exists(debuggerVisualizerPath))
                        {
                            loadDirectory = debuggerVisualizerPath;
                        }
                        else
                        {
                            loadDirectory = baseDirectoryInfo.FullName;
                        }
                        subfolder = FindValidSubfolders(loadDirectory, subfolderOptions);
                    }
                }
                else
                {
                    loadDirectory = Path.GetDirectoryName(asm.Location);
                    if (loadDirectory != null) 
                    {
                        subfolder = FindValidSubfolders(loadDirectory, subfolderOptions);
                    }
                }

                if (!String.IsNullOrEmpty(subfolder))
                {
                    if (Directory.Exists(Path.Combine(loadDirectory, subfolder)))
                    {
                        loadDirectory = Path.Combine(loadDirectory, subfolder);
                    }
                    else
                    {
                        loadDirectory = Path.Combine(Path.GetFullPath("."), subfolder);
                    }
                }

                if (!Directory.Exists(loadDirectory))
                {
                    //try to find an alternative loadDirectory path
                    //The following code should handle finding the asp.NET BIN folder 
                    if (String.IsNullOrEmpty(asm.Location) || !File.Exists(asm.Location))
                    {
                        Debug.WriteLine(String.Format("asm.Location is invalid: '{0}'", asm.Location));
                    }
                    else
                    {
                        FileInfo file = new FileInfo(asm.Location);
                        DirectoryInfo directory = file.Directory;
                        if ((directory != null) && (!String.IsNullOrEmpty(subfolder)) && Directory.Exists(Path.Combine(directory.FullName, subfolder)))
                        {
                            loadDirectory = Path.Combine(directory.FullName, subfolder);
                        }
                        else if (directory != null &&  Directory.Exists(directory.FullName))
                        {
                            loadDirectory = directory.FullName;
                        }
                    }
                }
            }

            bool setDllDirectorySuccess = false;
            if (!String.IsNullOrEmpty(loadDirectory) && Directory.Exists(loadDirectory))
            {
                if (Platform.ClrType == Platform.Clr.DotNetNative )
                {
                    //do nothing
                }
                else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows )
                {
                    //addDllDirectorySuccess = Emgu.Util.Toolbox.AddDllDirectory(loadDirectory);
                    setDllDirectorySuccess = Emgu.Util.Toolbox.SetDllDirectory(loadDirectory);
                    if (!setDllDirectorySuccess)
                    {
                        System.Diagnostics.Debug.WriteLine(String.Format("Failed to set dll directory: {0}", loadDirectory));
                    }
                } else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.IOS)
                {
                    //do nothing
                    System.Diagnostics.Debug.WriteLine("iOS required static linking, Setting loadDirectory is not supported");
                }
                else
                {
                    oldDir = Environment.CurrentDirectory;
                    Environment.CurrentDirectory = loadDirectory;
                }
            }

            if (setDllDirectorySuccess)
            {
                System.Diagnostics.Debug.WriteLine(
                    String.Format(
                        "Loading Open CV binary for default locations. Current directory: {0}; Additional load folder: {1}",
                        Environment.CurrentDirectory,
                        loadDirectory));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(
                    String.Format(
                        "Loading Open CV binary for default locations. Current directory: {0}", 
                        Environment.CurrentDirectory));
            }


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

                bool optionalComponent = mName.Contains("ffmpeg");

                String moduleName = Path.Combine(prefix, mName);

                //Use absolute path for Windows Desktop
                String fullPath = Path.Combine(loadDirectory, moduleName);

                bool fileExist = File.Exists(fullPath);
                bool loaded = false;

                if (fileExist)
                {
                    //Try to load using the full path
                    System.Diagnostics.Trace.WriteLine(String.Format("Found full path {0} for {1}. Trying to load it.", fullPath, mName));
                    loaded = !IntPtr.Zero.Equals(Toolbox.LoadLibrary(fullPath));
                    if (loaded)
                        System.Diagnostics.Trace.WriteLine(String.Format("{0} loaded.", mName));
                    else
                        System.Diagnostics.Trace.WriteLine(String.Format("Failed to load {0} from {1}.", mName, fullPath));
                }
                if (!loaded)
                {
                    //Try to load without the full path
                    System.Diagnostics.Trace.WriteLine(String.Format("Trying to load {0} using default path.", mName));
                    loaded = !IntPtr.Zero.Equals(Toolbox.LoadLibrary(mName));
                    if (loaded)
                        System.Diagnostics.Trace.WriteLine(String.Format("{0} loaded using default path", mName));
                    else
                        System.Diagnostics.Trace.WriteLine(String.Format("Failed to load {0} using default path", mName));
                }

                if (!loaded)
                    System.Diagnostics.Trace.WriteLine(String.Format("!!! Failed to load {0}.", mName));

                if (!optionalComponent)
                    success &= loaded;
            }

            if (!oldDir.Equals(String.Empty))
            {
                Environment.CurrentDirectory = oldDir;
            }

            return success;
#endif
        }

        /// <summary>
        /// Get the module format string.
        /// </summary>
        /// <returns>On Windows, "{0}".dll will be returned; On Linux, "lib{0}.so" will be returned; Otherwise {0} is returned.</returns>
        public static String GetModuleFormatString()
        {
            String formatString = "{0}";
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
                formatString = "{0}.dll";
            else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Linux)
                formatString = "lib{0}.so";
            else if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.MacOS)
                formatString = "lib{0}.dylib";
            return formatString;
        }

        /// <summary>
        /// Attempts to load opencv modules from the specific location
        /// </summary>
        /// <param name="modules">The names of opencv modules. e.g. "opencv_core.dll" on windows.</param>
        /// <returns>True if all the modules has been loaded successfully</returns>
        public static bool DefaultLoadUnmanagedModules(String[] modules)
        {
            bool libraryLoaded = true;

#if !(UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE || UNITY_WSA)
            if (Emgu.Util.Platform.OperationSystem == Platform.OS.IOS)
                return libraryLoaded;
            else if (Emgu.Util.Platform.OperationSystem == Platform.OS.Android && (Emgu.Util.Platform.ClrType != Platform.Clr.Unity))
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
            }
            else if (Emgu.Util.Platform.OperationSystem != Emgu.Util.Platform.OS.MacOS)
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

                //For iOS, library are static linked, assume correct loading by default.
                _libraryLoaded = true;
                return;
            }

            List<String> modules = CvInvoke.OpenCVModuleList;
            modules.RemoveAll(String.IsNullOrEmpty);

            _libraryLoaded = DefaultLoadUnmanagedModules(modules.ToArray());

            if (Emgu.Util.Platform.OperationSystem != Emgu.Util.Platform.OS.IOS)
            {
                try
                {
                    //Use the custom error handler
                    RedirectError(CvErrorHandlerThrowException, IntPtr.Zero, IntPtr.Zero);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(String.Format("Failed to register error handler using RedirectError : {0}", e.StackTrace));
                    throw;
                }
            }
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
        /// <returns>An integer that represent a mat type</returns>
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