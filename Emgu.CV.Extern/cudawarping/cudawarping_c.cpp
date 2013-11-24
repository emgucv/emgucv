//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudawarping_c.h"

void cudaPyrDown(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::pyrDown(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaPyrUp(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, cv::cuda::Stream* stream)
{
   cv::cuda::pyrUp(*src, *dst, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaWarpAffine( const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst,  const CvArr* M, int flags, int borderMode, CvScalar* borderValue, cv::cuda::Stream* stream)
{
   cv::Mat Mat = cv::cvarrToMat(M);
   cv::cuda::warpAffine(*src, *dst, Mat, dst->size(), flags, borderMode, *borderValue, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaWarpPerspective( const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst,  const CvArr* M, int flags,  int borderMode, CvScalar* borderValue, cv::cuda::Stream* stream)
{
   cv::Mat Mat = cv::cvarrToMat(M);
   cv::cuda::warpPerspective(*src, *dst, Mat, dst->size(), flags, borderMode, *borderValue, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaRemap(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, const cv::cuda::GpuMat* xmap, const cv::cuda::GpuMat* ymap, int interpolation, int borderMode, CvScalar* borderValue, cv::cuda::Stream* stream)
{
	cv::cuda::remap(*src, *dst, *xmap, *ymap, interpolation, borderMode, *borderValue, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaResize(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, int interpolation, cv::cuda::Stream* stream)
{  
   if ( !stream && !(src->channels() == 1 || src->channels() == 4) )
   {
      //in synchronous version
      //added support for gpuMat with number of channels other than 1 or 4.
      cv::cuda::Stream ts;
      std::vector<cv::cuda::GpuMat> channels(src->channels());
      std::vector<cv::cuda::GpuMat> resizedChannels(src->channels());
      cv::cuda::split(*src, channels, ts);
      for (unsigned int i = 0; i < channels.size(); ++i)
      {
         //CV_Assert(channels[i].size() == src->size());
         cv::cuda::resize(channels[i], resizedChannels[i], dst->size(), 0, 0, interpolation, ts);
         //CV_Assert(resizedChannels[i].size() == dst->size());
      }

      cv::cuda::merge(resizedChannels, *dst, ts);
      ts.waitForCompletion();
   } else
   {  
      cv::cuda::resize(*src, *dst, dst->size(), 0, 0, interpolation, stream ? *stream : cv::cuda::Stream::Null());
   }
}

void cudaRotate(const cv::cuda::GpuMat* src, cv::cuda::GpuMat* dst, double angle, double xShift, double yShift, int interpolation, cv::cuda::Stream* s)
{
	cv::cuda::rotate(*src, *dst, dst->size(), angle, xShift, yShift, interpolation, s ? *s : cv::cuda::Stream::Null());
}
