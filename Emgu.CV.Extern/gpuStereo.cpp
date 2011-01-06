#include "opencv2/gpu/gpu.hpp"

CVAPI(cv::gpu::StereoBM_GPU*) GpuStereoBMCreate(int preset, int ndisparities, int winSize)
{
   return new cv::gpu::StereoBM_GPU(preset, ndisparities, winSize);
}

CVAPI(void) GpuStereoBMFindStereoCorrespondence(cv::gpu::StereoBM_GPU* stereoBM, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream)
{
   if (stream)
      (*stereoBM)(*left, *right, *disparity, *stream);
   else
      (*stereoBM)(*left, *right, *disparity);
}

CVAPI(void) GpuStereoBMRelease(cv::gpu::StereoBM_GPU** stereoBM)
{
   delete *stereoBM;
}