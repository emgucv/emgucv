//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "core_cuda_c.h"

cv::cuda::Stream* streamCreate() 
{ 
   return new cv::cuda::Stream(); 
}

cv::cuda::Stream* streamCreateWithFlag(int flag)
{
	return new cv::cuda::Stream(flag);
}

void streamRelease(cv::cuda::Stream** stream) 
{ 
   delete *stream; 
   *stream = 0;
}

void streamWaitForCompletion(cv::cuda::Stream* stream) 
{ 
   stream->waitForCompletion(); 
}

bool streamQueryIfComplete(cv::cuda::Stream* stream) 
{ 
   return stream->queryIfComplete(); 
}

/*
void streamEnqueueCopy(cv::cuda::Stream* stream, cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst) 
{ 
   return stream->enqueueCopy(*src, *dst); 
}*/

