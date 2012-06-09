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
   /// <summary>
   /// Copy the Android assets to the cache folder
   /// </summary>
   public class AndroidCacheFileAsset : DisposableObject
   {
      private Java.IO.File _dir;
      private Java.IO.File _file;

      public AndroidCacheFileAsset(Context context, String assertName, String cacheFolderPostfix)
      {
         System.IO.Stream iStream = context.Assets.Open(assertName);
         String fileName = Path.GetFileName(assertName);
         String fullPath = Path.Combine(cacheFolderPostfix, fileName);
         _dir = context.GetDir(cacheFolderPostfix, FileCreationMode.Private);
         _file = new Java.IO.File(_dir, fileName);
         if (_file.Exists())
            throw new IOException(String.Format("A file with the name {0} already exist.", _file.ToString()));

         using (System.IO.Stream os = System.IO.File.OpenWrite(_file.AbsolutePath))
         {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = iStream.Read(buffer, 0, buffer.Length)) > 0)
               os.Write(buffer, 0, len);
         }
      }

      public AndroidCacheFileAsset(Context context, String assertName)
         : this(context, assertName, "tmp")
      {
      }

      public String DirectoryName
      {
         get
         {
            return _dir.ToString();
         }
      }

      public String FileFullPath
      {
         get
         {
            return _file.AbsolutePath;
         }
      }

      protected override void DisposeObject()
      {
         if (_file.Exists())
            _file.Delete();
         //_dir.Delete();
      }
   }

   /// <summary>
   /// Copy the Android assets to the app's FilesDir
   /// </summary>
   public class AndroidPermanantFileAsset : DisposableObject
   {
      private Java.IO.File _dir;
      private Java.IO.File _file;

      public AndroidPermanantFileAsset(Context context, String assertName, String dstDir, bool alwaysOverwrite)
      {
         System.IO.Stream iStream = context.Assets.Open(assertName);
         String fullPath = Path.Combine( context.FilesDir.AbsolutePath, dstDir, assertName);
         _dir = new Java.IO.File( Path.GetDirectoryName(fullPath));
         _dir.Mkdirs();
         string fileName = Path.GetFileName(assertName);
         _file = new Java.IO.File(_dir, fileName);

         if (!alwaysOverwrite && _file.Exists())
            throw new IOException(String.Format("A file with the name {0} already exist.", _file.ToString()));

         using (System.IO.Stream os = System.IO.File.OpenWrite(_file.AbsolutePath))
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
            return _dir.ToString();
         }
      }

      public String FileFullPath
      {
         get
         {
            return _file.AbsolutePath;
         }
      }

      protected override void DisposeObject()
      {
      }
   }
}
