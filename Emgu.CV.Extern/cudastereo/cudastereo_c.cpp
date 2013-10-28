//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudastereo_c.h"

cv::cuda::StereoBM* cudaStereoBMCreate(int numDisparities, int blockSize)
{
	cv::Ptr<cv::cuda::StereoBM> ptr =  cv::cuda::createStereoBM(numDisparities, blockSize);
	ptr.addref();
   return ptr.get();
}

void cudaStereoBMFindStereoCorrespondence(cv::cuda::StereoBM* stereo, const cv::cuda::GpuMat* left, const cv::cuda::GpuMat* right, cv::cuda::GpuMat* disparity, cv::cuda::Stream* stream)
{
   stereo->compute(*left, *right, *disparity, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaStereoBMRelease(cv::cuda::StereoBM** stereoBM)
{
   delete *stereoBM;
   *stereoBM = 0;
}

cv::cuda::StereoConstantSpaceBP* cudaStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane)
{
   cv::Ptr<cv::cuda::StereoConstantSpaceBP> ptr = cv::cuda::createStereoConstantSpaceBP(ndisp, iters, levels, nr_plane, CV_32F);
   ptr.addref();
   return ptr.get();
}

void cudaStereoConstantSpaceBPFindStereoCorrespondence(cv::cuda::StereoConstantSpaceBP* stereo, const cv::cuda::GpuMat* left, const cv::cuda::GpuMat* right, cv::cuda::GpuMat* disparity, cv::cuda::Stream* stream)
{
   stereo->compute(*left, *right, *disparity, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaStereoConstantSpaceBPRelease(cv::cuda::StereoConstantSpaceBP** stereo)
{
   delete *stereo;
   *stereo = 0;
}

cv::cuda::DisparityBilateralFilter* GpuDisparityBilateralFilterCreate(int ndisp, int radius, int iters)
{
   cv::Ptr<cv::cuda::DisparityBilateralFilter> ptr = cv::cuda::createDisparityBilateralFilter(ndisp, radius, iters);
   ptr.addref();
   return ptr.get();
}

void GpuDisparityBilateralFilterApply(cv::cuda::DisparityBilateralFilter* filter, const cv::cuda::GpuMat* disparity, const cv::cuda::GpuMat* image, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   filter->apply(*disparity, *image, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void GpuDisparityBilateralFilterRelease(cv::cuda::DisparityBilateralFilter** filter)
{
   delete *filter;
   *filter = 0;
}

