//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

cv::gpu::StereoBM_GPU* GpuStereoBMCreate(int preset, int ndisparities, int winSize)
{
   return new cv::gpu::StereoBM_GPU(preset, ndisparities, winSize);
}

void GpuStereoBMFindStereoCorrespondence(cv::gpu::StereoBM_GPU* stereo, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream)
{
   (*stereo)(*left, *right, *disparity, stream ? *stream : cv::gpu::Stream::Null());
}

void GpuStereoBMRelease(cv::gpu::StereoBM_GPU** stereoBM)
{
   delete *stereoBM;
   *stereoBM = 0;
}

cv::gpu::StereoConstantSpaceBP* GpuStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane)
{
   return new cv::gpu::StereoConstantSpaceBP(ndisp, iters, levels, nr_plane, CV_32F);
}

void GpuStereoConstantSpaceBPFindStereoCorrespondence(cv::gpu::StereoConstantSpaceBP* stereo, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream)
{
   (*stereo)(*left, *right, *disparity, stream ? *stream : cv::gpu::Stream::Null());
}

void GpuStereoConstantSpaceBPRelease(cv::gpu::StereoConstantSpaceBP** stereo)
{
   delete *stereo;
   *stereo = 0;
}

cv::gpu::DisparityBilateralFilter* GpuDisparityBilateralFilterCreate(int ndisp, int radius, int iters, float edge_threshold, float max_disc_threshold, float sigma_range)
{
   return new cv::gpu::DisparityBilateralFilter(ndisp, radius, iters, edge_threshold, max_disc_threshold, sigma_range);
}

void GpuDisparityBilateralFilterApply(cv::gpu::DisparityBilateralFilter* filter, const cv::gpu::GpuMat* disparity, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   (*filter)(*disparity, *image, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void GpuDisparityBilateralFilterRelease(cv::gpu::DisparityBilateralFilter** filter)
{
   delete *filter;
   *filter = 0;
}

