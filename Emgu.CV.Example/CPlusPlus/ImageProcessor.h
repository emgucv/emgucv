#include "cv.h"
#include "highgui.h"

namespace CPlusPlus 
{
   using namespace Emgu::CV;
   using namespace System;

   public ref class ImageProcessor
   {
   public:
      static array<Emgu::CV::Image<Bgr^, Byte>^>^ ProcessImage();
   };
}