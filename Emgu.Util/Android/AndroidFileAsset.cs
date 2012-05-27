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

namespace Emgu.Util
{
   public class AndroidFileAsset : DisposableObject
   {
      private Java.IO.File _dir;
      private Java.IO.File _file;

      public AndroidFileAsset(Context context, String assertName)
      {
         System.IO.Stream iStream = context.Assets.Open("haarcascade_frontalface_default.xml"); ;
         _dir = context.GetDir("tmp", FileCreationMode.Private);
         _file = new Java.IO.File(_dir, assertName);
         using (System.IO.Stream os = System.IO.File.OpenWrite(_file.AbsolutePath))
         {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = iStream.Read(buffer, 0, buffer.Length)) > 0)
               os.Write(buffer, 0, len);
         }
      }

      public String FileName
      {
         get
         {
            return _file.AbsolutePath;
         }
      }

      protected override void DisposeObject()
      {
         _file.Delete();
         _dir.Delete();
      }
   }
}
