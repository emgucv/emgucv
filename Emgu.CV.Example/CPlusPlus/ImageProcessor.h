#include "cv.h"
#include "highgui.h"

namespace CPlusPlus 
{
   using namespace Emgu::CV;
   using namespace Emgu::CV::Structure;
   using namespace System;

   public ref class ImageProcessor
   {
   public:
      static array<Emgu::CV::Image<Bgr, Byte>^>^ ProcessImage();
   };
}