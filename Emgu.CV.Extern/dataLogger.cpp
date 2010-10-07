#include "dataLogger.h"

using namespace emgu;

DataLogger* DataLoggerCreate(int logLevel) { return new DataLogger(logLevel); }

void DataLoggerRelease(DataLogger** logger) { if (*logger) { delete *logger; *logger = 0; } }

void DataLoggerRegisterCallback(DataLogger* logger, DataCallback dataCallback )
{
   logger->callback = dataCallback;
}

void DataLoggerLog(DataLogger* logger, void* data, int logLevel)
{
   logger->log(data, logLevel);
}


