//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "opencv2/gpu/gpu.hpp"

CVAPI(cv::gpu::Stream*) streamCreate() { return new cv::gpu::Stream(); }

CVAPI(void) streamRelease(cv::gpu::Stream** stream) { delete stream; }

CVAPI(void) streamWaitForCompletion(cv::gpu::Stream* stream) { stream->waitForCompletion(); }

CVAPI(bool) streamQueryIfComplete(cv::gpu::Stream* stream) { return stream->queryIfComplete(); }

CVAPI(void) streamEnqueueCopy(cv::gpu::Stream* stream, cv::gpu::GpuMat* src, cv::gpu::GpuMat* dst) { return stream->enqueueCopy(*src, *dst); }

