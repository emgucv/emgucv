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

CVAPI(cv::gpu::DisparityBilateralFilter*) GpuDisparityBilateralFilterCreate(int ndisp, int radius, int iters, float edge_threshold, float max_disc_threshold, float sigma_range)
{
   return new cv::gpu::DisparityBilateralFilter(ndisp, radius, iters, edge_threshold, max_disc_threshold, sigma_range);
}

CVAPI(void) GpuDisparityBilateralFilterApply(cv::gpu::DisparityBilateralFilter* filter, const cv::gpu::GpuMat* disparity, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   if (stream)
      (*filter)(*disparity, *image, *dst, *stream);
   else
      (*filter)(*disparity, *image, *dst);
}

CVAPI(void) GpuDisparityBilateralFilterRelease(cv::gpu::DisparityBilateralFilter** filter)
{
   delete *filter;
}
