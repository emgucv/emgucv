#include "opencv2/core/core.hpp"

namespace emgu {

   typedef void (_stdcall *DataCallback)(void* data);

   class CV_EXPORTS DataLogger
   {
   public:
      DataCallback callback;

      DataLogger()
         : callback(0)  {};

      void registerCallback(DataCallback dataCallback)
      {
         callback = dataCallback;
      }

      void log(void* data)
      {
         if (callback) callback(data);
      }
   };
};

/* DataLogger */
CVAPI(emgu::DataLogger*) DataLoggerCreate();

CVAPI(void) DataLoggerRelease(emgu::DataLogger** logger);

CVAPI(void) DataLoggerRegisterCallback(emgu::DataLogger* logger, emgu::DataCallback messageCallback );

CVAPI(void) DataLoggerLog(emgu::DataLogger* logger, void* data);
