//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cuda_c.h"

cv::cuda::Stream* streamCreate() 
{ 
   return new cv::cuda::Stream(); 
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

