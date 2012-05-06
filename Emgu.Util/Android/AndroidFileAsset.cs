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

namespace Emgu.Util.Android
{
/*   
   public class AndroidFileAsset
   {
      public AndroidFileAsset(String assertName)
      {
         //Context context = Application.Context;
         AssetManager manager = new AssetManager();
         System.IO.Stream iStream = context.Assets.Open("haarcascade_frontalface_default.xml"); ;
         Java.IO.File dir = context.GetDir("cascade", FileCreationMode.Private);
         Java.IO.File file = new Java.IO.File(dir, "cascade.xml");
         using (System.IO.Stream os = System.IO.File.OpenWrite(file.AbsolutePath))
         {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = iStream.Read(buffer, 0, buffer.Length)) > 0)
               os.Write(buffer, 0, len);
         }
         _faceDetector = new HaarCascade(file.AbsolutePath);
         file.Delete();
         dir.Delete();
      }
   }*/
}
