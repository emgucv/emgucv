//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   public partial class CvInvoke
   {

      /// <summary>
      /// Creates a window which can be used as a placeholder for images and trackbars. Created windows are reffered by their names. 
      /// If the window with such a name already exists, the function does nothing.
      /// </summary>
      /// <param name="name">Name of the window which is used as window identifier and appears in the window caption</param>
      /// <param name="flags">Flags of the window.</param>
      public static void NamedWindow(String name, CvEnum.NamedWindowType flags = NamedWindowType.AutoSize)
      {
         using (CvString s = new CvString(name))
            cveNamedWindow(s, flags);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveNamedWindow(IntPtr name, CvEnum.NamedWindowType flags);

      /// <summary>
      /// Waits for key event infinitely (delay &lt;= 0) or for "delay" milliseconds. 
      /// </summary>
      /// <param name="delay">Delay in milliseconds.</param>
      /// <returns>The code of the pressed key or -1 if no key were pressed until the specified timeout has elapsed</returns>
      public static int WaitKey(int delay = 0)
      {
         return cveWaitKey(delay);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int cveWaitKey(int delay);

      /// <summary>
      /// Shows the image in the specified window
      /// </summary>
      /// <param name="name">Name of the window</param>
      /// <param name="image">Image to be shown</param>
      public static void Imshow(String name, IInputArray image)
      {
         using (CvString s = new CvString(name))
         using (InputArray iaImage = image.GetInputArray())
         {
            cveImshow(s, iaImage);
         }
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveImshow(IntPtr name, IntPtr image);

      /// <summary>
      /// Destroys the window with a given name
      /// </summary>
      /// <param name="name">Name of the window to be destroyed</param>
      public static void DestroyWindow(String name)
      {
         using (CvString s = new CvString(name))
            cveDestroyWindow(s);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDestroyWindow(IntPtr name);

      /// <summary>
      /// Destroys all of the HighGUI windows.
      /// </summary>
      public static void DestroyAllWindows()
      {
         cveDestroyAllWindows();
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDestroyAllWindows();
   }
}
