//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dataLogger.h"

using namespace emgu;

DataLogger* cveDataLoggerCreate(int logLevel, int loggerId)
{
	return new DataLogger(logLevel, loggerId);
}

void cveDataLoggerRelease(DataLogger** logger)
{
	try
	{
		if (*logger)
		{
			delete *logger;
			*logger = 0;
		}
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveDataLoggerRegisterCallback(DataLogger* logger, DataCallback dataCallback )
{
   logger->callback = dataCallback;
}

void cveDataLoggerLog(DataLogger* logger, void* data, int logLevel)
{
   logger->log(data, logLevel);
}


