//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "core_cuda_c.h"

cv::cuda::Stream* streamCreate()
{
	try
	{
		return new cv::cuda::Stream();
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::cuda::Stream* streamCreateWithFlag(int flag)
{
	try
	{
		return new cv::cuda::Stream(flag);
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void streamRelease(cv::cuda::Stream** stream)
{
   delete *stream;
   *stream = 0;
}

void streamWaitForCompletion(cv::cuda::Stream* stream)
{
	try
	{
		stream->waitForCompletion();
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool streamQueryIfComplete(cv::cuda::Stream* stream)
{
	try
	{
		return stream->queryIfComplete();
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

/*
void streamEnqueueCopy(cv::cuda::Stream* stream, cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst)
{
   return stream->enqueueCopy(*src, *dst);
}*/
