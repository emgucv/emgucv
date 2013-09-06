//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "gpu_c.h"

cv::gpu::StereoBM* GpuStereoBMCreate(int numDisparities, int blockSize)
{
	cv::Ptr<cv::gpu::StereoBM> ptr =  cv::gpu::createStereoBM(numDisparities, blockSize);
	ptr.addref();
	return ptr.obj;
}

void GpuStereoBMFindStereoCorrespondence(cv::gpu::StereoBM* stereo, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream)
{
   stereo->compute(*left, *right, *disparity, stream ? *stream : cv::gpu::Stream::Null());
}

void GpuStereoBMRelease(cv::gpu::StereoBM** stereoBM)
{
   delete *stereoBM;
   *stereoBM = 0;
}

cv::gpu::StereoConstantSpaceBP* GpuStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane)
{
   cv::Ptr<cv::gpu::StereoConstantSpaceBP> ptr = cv::gpu::createStereoConstantSpaceBP(ndisp, iters, levels, nr_plane, CV_32F);
   ptr.addref();
   return ptr.obj;
}

void GpuStereoConstantSpaceBPFindStereoCorrespondence(cv::gpu::StereoConstantSpaceBP* stereo, const cv::gpu::GpuMat* left, const cv::gpu::GpuMat* right, cv::gpu::GpuMat* disparity, cv::gpu::Stream* stream)
{
   stereo->compute(*left, *right, *disparity, stream ? *stream : cv::gpu::Stream::Null());
}

void GpuStereoConstantSpaceBPRelease(cv::gpu::StereoConstantSpaceBP** stereo)
{
   delete *stereo;
   *stereo = 0;
}

cv::gpu::DisparityBilateralFilter* GpuDisparityBilateralFilterCreate(int ndisp, int radius, int iters)
{
   cv::Ptr<cv::gpu::DisparityBilateralFilter> ptr = cv::gpu::createDisparityBilateralFilter(ndisp, radius, iters);
   ptr.addref();
   return ptr.obj;
}

void GpuDisparityBilateralFilterApply(cv::gpu::DisparityBilateralFilter* filter, const cv::gpu::GpuMat* disparity, const cv::gpu::GpuMat* image, cv::gpu::GpuMat* dst, cv::gpu::Stream* stream)
{
   filter->apply(*disparity, *image, *dst, stream ? *stream : cv::gpu::Stream::Null());
}

void GpuDisparityBilateralFilterRelease(cv::gpu::DisparityBilateralFilter** filter)
{
   delete *filter;
   *filter = 0;
}

