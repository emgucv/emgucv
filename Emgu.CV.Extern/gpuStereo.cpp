#include "opencv2/gpu/gpu.hpp"

CVAPI(cv::gpu::StereoBM_GPU*) GpuStereoBMCreate(int preset, int ndisparities, int winSize)
{
   return new cv::gpu::StereoBM_GPU(preset, ndisparities, winSize);
}

CVAPI(void) GpuStereoBMFindStereoCorrespondence(cv::gpu::StereoBM_GPU* stereo, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream)
{
   if (stream)
      (*stereo)(*left, *right, *disparity, *stream);
   else
      (*stereo)(*left, *right, *disparity);
}

CVAPI(void) GpuStereoBMRelease(cv::gpu::StereoBM_GPU** stereoBM)
{
   delete *stereoBM;
}

CVAPI(cv::gpu::StereoConstantSpaceBP*) GpuStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane)
{
   return new cv::gpu::StereoConstantSpaceBP(ndisp, iters, levels, nr_plane, CV_32F);
}

CVAPI(void) GpuStereoConstantSpaceBPFindStereoCorrespondence(cv::gpu::StereoConstantSpaceBP* stereo, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream)
{
   if (stream)
      (*stereo)(*left, *right, *disparity, *stream);
   else
      (*stereo)(*left, *right, *disparity);
}

CVAPI(void) GpuStereoConstantSpaceBPRelease(cv::gpu::StereoConstantSpaceBP** stereoBM)
{
   delete *stereoBM;
}
