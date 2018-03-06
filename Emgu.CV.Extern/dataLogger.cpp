//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dataLogger.h"

using namespace emgu;

DataLogger* DataLoggerCreate(int logLevel, int loggerId) { return new DataLogger(logLevel, loggerId); }

void DataLoggerRelease(DataLogger** logger) { if (*logger) { delete *logger; *logger = 0; } }

void DataLoggerRegisterCallback(DataLogger* logger, DataCallback dataCallback )
{
   logger->callback = dataCallback;
}

void DataLoggerLog(DataLogger* logger, void* data, int logLevel)
{
   logger->log(data, logLevel);
}


