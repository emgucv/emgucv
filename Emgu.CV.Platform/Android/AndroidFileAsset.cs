//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Android.Content;

namespace Emgu.Util
{
    /// <summary>
    /// Android file asset.
    /// </summary>
    public abstract class AndroidFileAsset : DisposableObject
    {
        /// <summary>
        /// The file
        /// </summary>
        protected FileInfo _file;

        /// <summary>
        /// The file overwrite method
        /// </summary>
        protected OverwriteMethod _overwriteMethod;

        /// <summary>
        /// The file overwriting method
        /// </summary>
        public enum OverwriteMethod
        {
            /// <summary>
            /// Always overwrite the file
            /// </summary>
            AlwaysOverwrite,
            /*
            /// <summary>
            /// Copy if the current file is newer than the existing one
            /// </summary>
            CopyIfNewer,*/
            /// <summary>
            /// Will never overwrite. Throw exception if the file already exist
            /// </summary>
            NeverOverwrite
        }

        /// <summary>
        /// Copy the Android assets to the app's FilesDir
        /// </summary>
        /// <param name="context">The android context</param>
        /// <param name="assertName">The name of the assert</param>
        /// <param name="dstDir">The subfolder in the app's FilesDir</param>
        /// <param name="overwriteMethod">overwrite method</param>
        /// <returns>The resulting FileInfo</returns>
        public static FileInfo WritePermanentFileAsset(Context context, String assertName, String dstDir, OverwriteMethod overwriteMethod)
        {
            String fullPath = Path.Combine(context.FilesDir.AbsolutePath, dstDir, assertName);

            //Create the directory if it is not already exist.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using (Stream iStream = context.Assets.Open(assertName))
                WriteStream(iStream, fullPath, overwriteMethod);
            return new FileInfo(fullPath);
        }

        /// <summary>
        /// Write the io stream to a file
        /// </summary>
        /// <param name="iStream">The IO stream</param>
        /// <param name="fileFullPath">The file path</param>
        /// <param name="method">The overwriting method</param>
        public static void WriteStream(System.IO.Stream iStream, String fileFullPath, OverwriteMethod method)
        {
            if (method == OverwriteMethod.NeverOverwrite && File.Exists(fileFullPath))
            {
                throw new IOException(String.Format("A file with the name {0} already exist.", fileFullPath));
            }
            using (Stream os = File.OpenWrite(fileFullPath))
            {
                byte[] buffer = new byte[8 * 1024];
                int len;
                while ((len = iStream.Read(buffer, 0, buffer.Length)) > 0)
                    os.Write(buffer, 0, len);
            }
        }

        /// <summary>
        /// The directory of the file 
        /// </summary>
        public String DirectoryName
        {
            get
            {
                return _file.DirectoryName;
            }
        }

        /// <summary>
        /// The full file path
        /// </summary>
        public String FileFullPath
        {
            get
            {
                return _file.FullName;
            }
        }

        /// <summary>
        /// Release resources associate with this file asset.
        /// </summary>
        protected override void DisposeObject()
        {
        }
    }

    /// <summary>
    /// Copy the Android assets to the cache folder
    /// </summary>
    public class AndroidCacheFileAsset : AndroidFileAsset
    {
        /// <summary>
        /// Create an android file from an asset
        /// </summary>
        /// <param name="context">The Android context</param>
        /// <param name="assertName">The name of the asset</param>
        /// <param name="cacheFolderPostfix">The post fix of the cache folder</param>
        /// <param name="method">The file overwriting method</param>
        public AndroidCacheFileAsset(Context context, String assertName, String cacheFolderPostfix = "tmp", OverwriteMethod method = OverwriteMethod.NeverOverwrite)
        {
            String fileName = Path.GetFileName(assertName);
            fileName = Path.Combine(context.GetDir(cacheFolderPostfix, FileCreationMode.Private).AbsolutePath, fileName);
            _file = new FileInfo(fileName);
            _overwriteMethod = method;

            using (System.IO.Stream iStream = context.Assets.Open(assertName))
                WriteStream(iStream, FileFullPath, _overwriteMethod);

        }

        /// <summary>
        /// Release resources associate with this file asset.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_file.Exists)
                _file.Delete();

            base.DisposeObject();
        }
    }
}

#endif