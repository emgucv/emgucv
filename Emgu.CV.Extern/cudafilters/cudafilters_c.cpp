//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudafilters_c.h"

cv::cuda::Filter* cudaCreateSobelFilter(int srcType, int dstType, int dx, int dy, int ksize, double scale, int rowBorderType, int columnBorderType, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createSobelFilter(srcType, dstType, dx, dy, ksize, scale, rowBorderType, columnBorderType);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateGaussianFilter(int srcType, int dstType, CvSize* ksize, double sigma1, double sigma2, int rowBorderType, int columnBorderType, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Size s(ksize->width, ksize->height);
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createGaussianFilter(srcType, dstType, s, sigma1, sigma2, rowBorderType, columnBorderType);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateLaplacianFilter(int srcType, int dstType, int ksize, double scale, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createLaplacianFilter(srcType, dstType, ksize, scale, borderMode, *borderValue);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateLinearFilter(int srcType, int dstType, cv::_InputArray* kernel, CvPoint* anchor, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createLinearFilter(srcType, dstType, *kernel, *anchor, borderMode, *borderValue);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateBoxFilter(int srcType, int dstType, CvSize* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createBoxFilter(srcType, dstType, *ksize, *anchor, borderMode, *borderValue);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateBoxMaxFilter(int srcType, CvSize* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Size s(ksize->width, ksize->height);
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createBoxMaxFilter(srcType, s, *anchor, borderMode, *borderValue);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateBoxMinFilter(int srcType, CvSize* ksize, CvPoint* anchor, int borderMode, CvScalar* borderValue, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Size s(ksize->width, ksize->height);
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createBoxMinFilter(srcType, s, *anchor, borderMode, *borderValue);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateMorphologyFilter(int op, int srcType, cv::_InputArray* kernel, CvPoint* anchor, int iterations, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createMorphologyFilter(op, srcType, *kernel, *anchor, iterations);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateSeparableLinearFilter(
	int srcType, int dstType, cv::_InputArray* rowKernel, cv::_InputArray* columnKernel,
	CvPoint* anchor, int rowBorderMode, int columnBorderMode, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createSeparableLinearFilter(srcType, dstType, *rowKernel, *columnKernel, *anchor, rowBorderMode, columnBorderMode);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateDerivFilter(int srcType, int dstType, int dx, int dy,
	int ksize, bool normalize, double scale,
	int rowBorderMode, int columnBorderMode, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createDerivFilter(srcType, dstType, dx, dy, ksize, normalize, scale, rowBorderMode, columnBorderMode);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateScharrFilter(int srcType, int dstType, int dx, int dy,
	double scale, int rowBorderMode, int columnBorderMode, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createScharrFilter(srcType, dstType, dx, dy, scale, rowBorderMode, columnBorderMode);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateRowSumFilter(int srcType, int dstType, int ksize, int anchor, int borderMode, CvScalar* borderVal, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createRowSumFilter(srcType, dstType, ksize, anchor, borderMode, *borderVal);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateColumnSumFilter(int srcType, int dstType, int ksize, int anchor, int borderMode, CvScalar* borderVal, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createColumnSumFilter(srcType, dstType, ksize, anchor, borderMode, *borderVal);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

cv::cuda::Filter* cudaCreateMedianFilter(int srcType, int windowSize, int partition, cv::Ptr<cv::cuda::Filter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	cv::Ptr<cv::cuda::Filter> ptr = cv::cuda::createMedianFilter(srcType, windowSize, partition);
	*sharedPtr = new cv::Ptr<cv::cuda::Filter>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_cudafilters();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaFilter
//
//----------------------------------------------------------------------------
void cudaFilterApply(cv::cuda::Filter* filter, cv::_InputArray* image, cv::_OutputArray* dst, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	filter->apply(*image, *dst, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudafilters();
#endif
}
void cudaFilterRelease(cv::Ptr<cv::cuda::Filter>** filter)
{
#ifdef HAVE_OPENCV_CUDAFILTERS
	delete* filter;
	*filter = 0;
#else
	throw_no_cudafilters();
#endif
}