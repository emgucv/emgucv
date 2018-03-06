//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_DATA_LOGGER_H
#define EMGU_DATA_LOGGER_H

#include "opencv2/core/core.hpp"
#include "opencv2/core/types_c.h"

#include <stdarg.h>
#include <stdio.h>

namespace emgu {

   typedef void (CV_CDECL *DataCallback)(void* data, int loggerId);

   class CV_EXPORTS DataLogger
   {
   public:
      int logLevel;
      int loggerId;

      DataCallback callback;

      DataLogger(int level, int id)
         : callback(0), logLevel(level), loggerId(id) {};

      void registerCallback(DataCallback dataCallback)
      {
         callback = dataCallback;
      }

      void log(void* data, int level)
      {
         if (callback && level >=logLevel) callback(data, loggerId);
      }

      void log(void* data)
      {
         if (callback) callback(data, loggerId);
      }

      //Use this function only if you are trying to log messages
      //the total length of the message should be less than 1024 char
      void logMsg(const char* format, int level, ...)
      {
         if (callback && level >=logLevel)
         {
            char buffer[1024];
            va_list args;
            va_start(args, level);
            vsprintf(buffer, format, args);
            va_end(args);
            callback(buffer, loggerId);
         }
      }
   };
};

/* DataLogger */
CVAPI(emgu::DataLogger*) DataLoggerCreate(int logLevel, int loggerId);

CVAPI(void) DataLoggerRelease(emgu::DataLogger** logger);

CVAPI(void) DataLoggerRegisterCallback(emgu::DataLogger* logger, emgu::DataCallback messageCallback );

CVAPI(void) DataLoggerLog(emgu::DataLogger* logger, void* data, int logLevel);

#endif
