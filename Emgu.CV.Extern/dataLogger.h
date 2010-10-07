#pragma once
#ifndef DATA_LOGGER_H
#define DATA_LOGGER_H

#include "opencv2/core/core.hpp"

namespace emgu {

   typedef void (CV_CDECL *DataCallback)(void* data);

   class CV_EXPORTS DataLogger
   {
   public:
      int logLevel;

      DataCallback callback;

      DataLogger(int level)
         : callback(0), logLevel(level)  {};

      void registerCallback(DataCallback dataCallback)
      {
         callback = dataCallback;
      }

      void log(void* data, int level)
      {
         if (callback && level >=logLevel) callback(data);
      }

      void log(void* data)
      {
         if (callback) callback(data);
      }
   };
};

/* DataLogger */
CVAPI(emgu::DataLogger*) DataLoggerCreate(int logLevel);

CVAPI(void) DataLoggerRelease(emgu::DataLogger** logger);

CVAPI(void) DataLoggerRegisterCallback(emgu::DataLogger* logger, emgu::DataCallback messageCallback );

CVAPI(void) DataLoggerLog(emgu::DataLogger* logger, void* data, int logLevel);

#endif