//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudastereo_c.h"

cv::cuda::StereoBM* cudaStereoBMCreate(int numDisparities, int blockSize)
{
	cv::Ptr<cv::cuda::StereoBM> ptr =  cv::cuda::createStereoBM(numDisparities, blockSize);
	ptr.addref();
   return ptr.get();
}

void cudaStereoBMFindStereoCorrespondence(cv::cuda::StereoBM* stereo, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity, cv::cuda::Stream* stream)
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

void cudaStereoConstantSpaceBPFindStereoCorrespondence(cv::cuda::StereoConstantSpaceBP* stereo, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity, cv::cuda::Stream* stream)
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

void GpuDisparityBilateralFilterApply(cv::cuda::DisparityBilateralFilter* filter, cv::_InputArray* disparity, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
   filter->apply(*disparity, *image, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void GpuDisparityBilateralFilterRelease(cv::cuda::DisparityBilateralFilter** filter)
{
   delete *filter;
   *filter = 0;
}

