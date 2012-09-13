using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace AndroidExamples
{
   public class ImageBufferFactory<TColor> : Emgu.Util.DisposableObject
   where TColor : struct, IColor
   {
      public ImageBufferFactory()
      {
         _sizes = new List<Size>();
         _buffers = new List<Image<TColor, byte>>();
      }

      private List<System.Drawing.Size> _sizes;

      private List<Image<TColor, Byte>> _buffers;

      public Image<TColor, Byte> GetBuffer(int index)
      {
         if (index < _buffers.Count)
            return _buffers[index];
         else
            return null;
      }

      public Image<TColor, Byte> GetBuffer(System.Drawing.Size size, int index)
      {
         for (int i = _buffers.Count; i < index + 1; i++)
         {
            _buffers.Add(null);
            _sizes.Add(Size.Empty);
         }

         if (!_sizes[index].Equals(size))
         {
            if (_buffers[index] == null)
               _buffers[index] = new Image<TColor, byte>(size);
            else
            {
               _buffers[index].Dispose();
               _buffers[index] = new Image<TColor, byte>(size);
            }

            _sizes[index] = size;
         }
         return _buffers[index];
      }

      protected override void DisposeObject()
      {
         for (int i = 0; i < _buffers.Count; i++)
         {
            if (_buffers[i] != null)
               _buffers[i].Dispose();
         }
      }
   }
}