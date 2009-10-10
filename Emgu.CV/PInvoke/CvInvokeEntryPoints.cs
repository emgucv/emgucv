using System;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      // The following name are .Net Windows specific
      // When run under Mono, change the mapping in Emgu.CV.dll.config accordingly

      /// <summary>
      /// The file name of the cxcore library
      /// </summary>
      public const string CXCORE_LIBRARY = "cxcore200.dll";
      /// <summary>
      /// The file name of the cv library
      /// </summary>
      public const string CV_LIBRARY = "cv200.dll";
      /// <summary>
      /// The file name of the highgui library
      /// </summary>
      public const string HIGHGUI_LIBRARY = "highgui200.dll";
      /// <summary>
      /// The file name of the cvaux library
      /// </summary>
      public const string CVAUX_LIBRARY = "cvaux200.dll";
      /// <summary>
      /// The file name of the cvextern library
      /// </summary>
      public const string EXTERN_LIBRARY = "cvextern.dll";
   }
}
