using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV.UI.GLView
{
   public class ImageBufferFactory<T> : Emgu.Util.DisposableObject
   where T : class, IDisposable
   {
      private Func<System.Drawing.Size, T> _createBufferFunc;

      public ImageBufferFactory(Func<System.Drawing.Size, T> createBufferFunc)
      {
         _sizes = new List<Size>();
         _buffers = new List<T>();
         _createBufferFunc = createBufferFunc;
      }

      private List<System.Drawing.Size> _sizes;

      private List<T> _buffers;

      public T GetBuffer(int index)
      {
         if (index < _buffers.Count)
            return _buffers[index];
         else
            return null;
      }

      public void ReleaseBuffer(int index)
      {
         if (index < _buffers.Count && _buffers[index] != null)
         {
            _buffers[index].Dispose();
            _buffers[index] = null;
            _sizes[index] = Size.Empty;
         }
      }

      public T GetBuffer(System.Drawing.Size size, int index)
      {
         for (int i = _buffers.Count; i < index + 1; i++)
         {
            _buffers.Add(null);
            _sizes.Add(Size.Empty);
         }

         if (!_sizes[index].Equals(size))
         {
            if (_buffers[index] != null)
               _buffers[index].Dispose();

            _buffers[index] = _createBufferFunc(size);
            _sizes[index] = size;
         }
         return _buffers[index];
      }

      protected override void DisposeObject()
      {
         for (int i = 0; i < _buffers.Count; i++)
         {
            if (_buffers[i] != null)
            {
               _buffers[i].Dispose();
               _buffers[i] = null;
               _sizes[i] = Size.Empty;
            }
         }
         _createBufferFunc = null;
      }
   }
}