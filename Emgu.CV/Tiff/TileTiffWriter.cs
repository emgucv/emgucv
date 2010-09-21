using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Tiff
{
   public class TileTiffWriter : TiffWriter
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void tiffWriteTileInfo(IntPtr pTiff, Size tileSize);
      #endregion

      public TileTiffWriter(String fileName, Size tileSize)
         : base(fileName)
      {
         tiffWriteTileInfo(_ptr, tileSize);
      }

      public override void WriteImage<TColor, TDepth>(Image<TColor, TDepth> image)
      {
         throw new NotImplementedException("TODO:// implement this");
      }
   }
}
