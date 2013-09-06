//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

cv::gpu::Stream* streamCreate() 
{ 
   return new cv::gpu::Stream(); 
}

void streamRelease(cv::gpu::Stream** stream) 
{ 
   delete *stream; 
   *stream = 0;
}

void streamWaitForCompletion(cv::gpu::Stream* stream) 
{ 
   stream->waitForCompletion(); 
}

bool streamQueryIfComplete(cv::gpu::Stream* stream) 
{ 
   return stream->queryIfComplete(); 
}

/*
void streamEnqueueCopy(cv::gpu::Stream* stream, cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst) 
{ 
   return stream->enqueueCopy(*src, *dst); 
}*/

