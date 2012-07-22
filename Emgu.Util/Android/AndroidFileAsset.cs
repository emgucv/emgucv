//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Runtime;
using Android.OS;
using Android.Content.Res;
using System.IO;

namespace Emgu.Util
{

   public abstract class AndroidFileAsset : DisposableObject
   {
      protected FileInfo _file;
      protected OverwriteMethod _overwriteMethod;

      public enum OverwriteMethod
      {
         AlwaysOverwrite,
         NeverOverwrite
      }

      protected void WriteStream(System.IO.Stream iStream)
      {
         if (_overwriteMethod == OverwriteMethod.NeverOverwrite && File.Exists(FileFullPath))
         {
            throw new IOException(String.Format("A file with the name {0} already exist.", FileFullPath));
         }
         using (System.IO.Stream os = System.IO.File.OpenWrite(FileFullPath))
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

      public AndroidCacheFileAsset(Context context, String assertName, String cacheFolderPostfix, OverwriteMethod overwrite)
      {
         String fileName = Path.GetFileName(assertName);
         fileName = Path.Combine(context.GetDir(cacheFolderPostfix, FileCreationMode.Private).AbsolutePath, fileName);
         _file = new FileInfo(fileName);
         _overwriteMethod = overwrite;

         using(System.IO.Stream iStream = context.Assets.Open(assertName))
            WriteStream(iStream);

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

   /// <summary>
   /// Copy the Android assets to the app's FilesDir
   /// </summary>
   public class AndroidPermanantFileAsset : AndroidFileAsset
   {
      public AndroidPermanantFileAsset(Context context, String assertName, String dstDir, OverwriteMethod overwrite)
      {
         String fullPath = Path.Combine(context.FilesDir.AbsolutePath, dstDir, assertName);
         Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
         _file = new FileInfo(fullPath);

         _overwriteMethod = overwrite;

         using (System.IO.Stream iStream = context.Assets.Open(assertName))
            WriteStream(iStream);
      }
   }
}
