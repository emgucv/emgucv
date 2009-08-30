using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV 
{
   public class PatchGenerator :UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static IntPtr CvPatchGeneratorDefaultCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvPatchGeneratorRelease(IntPtr pg);
      #endregion

      public PatchGenerator()
      {
         _ptr = CvPatchGeneratorDefaultCreate();
      }

      protected override void DisposeObject()
      {
         CvPatchGeneratorRelease(_ptr);
      }
   }
}
