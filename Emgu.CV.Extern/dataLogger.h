#pragma once
#ifndef DATA_LOGGER_H
#define DATA_LOGGER_H

#include "opencv2/core/core.hpp"
#include <stdarg.h>

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

      //Use this function only if you are trying to log messages
      void logMsg(const char* format, int level, ...)
      {
         if (callback && level >=logLevel)
         {
            char buffer[1024];
            va_list args;
            va_start(args, level);
            vsprintf(buffer, format, args);
            va_end(args);
            callback(buffer);
         }
      }
   };
};

/* DataLogger */
CVAPI(emgu::DataLogger*) DataLoggerCreate(int logLevel);

CVAPI(void) DataLoggerRelease(emgu::DataLogger** logger);

CVAPI(void) DataLoggerRegisterCallback(emgu::DataLogger* logger, emgu::DataCallback messageCallback );

CVAPI(void) DataLoggerLog(emgu::DataLogger* logger, void* data, int logLevel);

#endif