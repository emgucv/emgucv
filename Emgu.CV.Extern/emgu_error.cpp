//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "emgu_error.h"
#include <cstring>

EMGU_THREAD_LOCAL EmguPendingError emguPendingError = { false, 0, { 0 }, { 0 }, { 0 }, 0 };

static void copyTruncated(char* dst, size_t dstSize, const char* src)
{
	if (!src)
	{
		dst[0] = 0;
		return;
	}
#if defined(_MSC_VER)
	strncpy_s(dst, dstSize, src, dstSize - 1);
#else
	std::strncpy(dst, src, dstSize - 1);
	dst[dstSize - 1] = 0;
#endif
}

void emguRecordError(int status, const char* funcName, const char* errMsg, const char* fileName, int line)
{
	emguPendingError.hasError = true;
	emguPendingError.status = status;
	copyTruncated(emguPendingError.funcName, sizeof(emguPendingError.funcName), funcName);
	copyTruncated(emguPendingError.errMsg, sizeof(emguPendingError.errMsg), errMsg);
	copyTruncated(emguPendingError.fileName, sizeof(emguPendingError.fileName), fileName);
	emguPendingError.line = line;
}

bool cveCheckPendingError(int* status, cv::String* funcName, cv::String* errMsg, cv::String* fileName, int* line)
{
	if (!emguPendingError.hasError)
		return false;

	*status = emguPendingError.status;
	*funcName = cv::String(emguPendingError.funcName);
	*errMsg = cv::String(emguPendingError.errMsg);
	*fileName = cv::String(emguPendingError.fileName);
	*line = emguPendingError.line;

	emguPendingError.hasError = false;
	return true;
}
