#include "opencv2/gpu/gpu.hpp"

CVAPI(cv::gpu::Stream*) streamCreate() { return new cv::gpu::Stream(); }

CVAPI(void) streamRelease(cv::gpu::Stream** stream) { delete stream; }

CVAPI(void) streamWaitForCompletion(cv::gpu::Stream* stream) { stream->waitForCompletion(); }

CVAPI(bool) streamQueryIfComplete(cv::gpu::Stream* stream) { return stream->queryIfComplete(); }

