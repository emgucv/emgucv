//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudacodec_c.h"

//----------------------------------------------------------------------------
//
//  CudaVideoWritter
//
//----------------------------------------------------------------------------

cv::cudacodec::VideoWriter* cudaVideoWriterCreate(cv::String* fileName, CvSize* frameSize, double fps, cv::cudacodec::SurfaceFormat format, cv::Ptr<cv::cudacodec::VideoWriter>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDACODEC
	cv::Ptr<cv::cudacodec::VideoWriter> ptr = cv::cudacodec::createVideoWriter(*fileName, *frameSize, fps, format);
	*sharedPtr = new cv::Ptr<cv::cudacodec::VideoWriter>(ptr);
	return ptr.get();
#else
	throw_no_cudacodec();
#endif
}
void cudaVideoWriterRelease(cv::Ptr<cv::cudacodec::VideoWriter>** writer)
{
#ifdef HAVE_OPENCV_CUDACODEC
	delete* writer;
	*writer = 0;
#else
	throw_no_cudacodec();
#endif
}
void cudaVideoWriterWrite(cv::cudacodec::VideoWriter* writer, cv::_InputArray* frame, bool lastFrame)
{
#ifdef HAVE_OPENCV_CUDACODEC
	writer->write(*frame, lastFrame);
#else
	throw_no_cudacodec();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaVideoReader
//
//----------------------------------------------------------------------------

cv::cudacodec::VideoReader* cudaVideoReaderCreate(cv::String* fileName, cv::Ptr<cv::cudacodec::VideoReader>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDACODEC
	cv::Ptr<cv::cudacodec::VideoReader> ptr = cv::cudacodec::createVideoReader(*fileName);
	*sharedPtr = new cv::Ptr<cv::cudacodec::VideoReader>(ptr);
	return ptr.get();
#else
	throw_no_cudacodec();
#endif
}
void cudaVideoReaderRelease(cv::Ptr<cv::cudacodec::VideoReader>** reader)
{
#ifdef HAVE_OPENCV_CUDACODEC
	delete* reader;
	*reader = 0;
#else
	throw_no_cudacodec();
#endif
}
bool cudaVideoReaderNextFrame(cv::cudacodec::VideoReader* reader, cv::cuda::GpuMat* frame, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDACODEC
	return reader->nextFrame(*frame, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudacodec();
#endif
}
void cudaVideoReaderFormat(cv::cudacodec::VideoReader* reader, cv::cudacodec::FormatInfo* formatInfo)
{
#ifdef HAVE_OPENCV_CUDACODEC
	cv::cudacodec::FormatInfo fi = reader->format();
	memcpy(formatInfo, &fi, sizeof(cv::cudacodec::FormatInfo));
#else
	throw_no_cudacodec();
#endif
}