//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace Emgu.Util
{

   public abstract class AndroidFileAsset : DisposableObject
   {
      protected FileInfo _file;
      protected OverwriteMethod _overwriteMethod;

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
      public static FileInfo WritePermanantFileAsset(Context context, String assertName, String dstDir, OverwriteMethod overwriteMethod)
      {
         String fullPath = Path.Combine(context.FilesDir.AbsolutePath, dstDir, assertName);

         //Create the directory if it is not already exist.
         Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

         using (Stream iStream = context.Assets.Open(assertName))
            WriteStream(iStream, fullPath, overwriteMethod);
         return new FileInfo(fullPath);
      }

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

      public String DirectoryName
      {
         get
         {
            return _file.DirectoryName;
         }
      }

      public String FileFullPath
      {
         get
         {
            return _file.FullName;
         }
      }

      protected override void DisposeObject()
      {
      }
   }

   /// <summary>
   /// Copy the Android assets to the cache folder
   /// </summary>
   public class AndroidCacheFileAsset : AndroidFileAsset
   {
      public AndroidCacheFileAsset(Context context, String assertName, String cacheFolderPostfix)
         : this(context, assertName, cacheFolderPostfix, OverwriteMethod.NeverOverwrite)
      {
      }

      public AndroidCacheFileAsset(Context context, String assertName, String cacheFolderPostfix, OverwriteMethod method)
      {
         String fileName = Path.GetFileName(assertName);
         fileName = Path.Combine(context.GetDir(cacheFolderPostfix, FileCreationMode.Private).AbsolutePath, fileName);
         _file = new FileInfo(fileName);
         _overwriteMethod = method;

         using(System.IO.Stream iStream = context.Assets.Open(assertName))
            WriteStream(iStream, FileFullPath, _overwriteMethod);

      }

      public AndroidCacheFileAsset(Context context, String assertName)
         : this(context, assertName, "tmp")
      {
      }

      protected override void DisposeObject()
      {
         if (_file.Exists)
            _file.Delete();
           
         base.DisposeObject();
      }
   }
}

#endif