//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudacodec_c.h"

//----------------------------------------------------------------------------
//
//  CudaVideoWritter
//
//----------------------------------------------------------------------------

cv::cudacodec::VideoWriter* cudaVideoWriterCreate(cv::String* fileName, cv::Size* frameSize, cv::cudacodec::Codec codec, double fps, cv::cudacodec::ColorFormat colorFormat, cv::cuda::Stream* stream, cv::Ptr<cv::cudacodec::VideoWriter>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDACODEC
		cv::Ptr<cv::cudacodec::VideoWriter> ptr = cv::cudacodec::createVideoWriter(*fileName, *frameSize, codec, fps, colorFormat, 0, stream ? *stream : cv::cuda::Stream::Null());
		*sharedPtr = new cv::Ptr<cv::cudacodec::VideoWriter>(ptr);
		return ptr.get();
	#else
		throw_no_cudacodec();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cudaVideoWriterDelete(cv::Ptr<cv::cudacodec::VideoWriter>** writer)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDACODEC
		delete* writer;
		*writer = 0;
	#else
		throw_no_cudacodec();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cudaVideoWriterRelease(cv::cudacodec::VideoWriter* writer)
{
#ifdef HAVE_OPENCV_CUDACODEC
	writer->release();
#else
	throw_no_cudacodec();
#endif	
}
void cudaVideoWriterWrite(cv::cudacodec::VideoWriter* writer, cv::_InputArray* frame)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDACODEC
		writer->write(*frame);
	#else
		throw_no_cudacodec();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

//----------------------------------------------------------------------------
//
//  CudaVideoReader
//
//----------------------------------------------------------------------------

cv::cudacodec::VideoReader* cudaVideoReaderCreate(cv::String* fileName, cv::Ptr<cv::cudacodec::VideoReader>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDACODEC
		cv::Ptr<cv::cudacodec::VideoReader> ptr = cv::cudacodec::createVideoReader(*fileName);
		*sharedPtr = new cv::Ptr<cv::cudacodec::VideoReader>(ptr);
		return ptr.get();
	#else
		throw_no_cudacodec();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
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
	try
	{
	#ifdef HAVE_OPENCV_CUDACODEC
		return reader->nextFrame(*frame, stream ? *stream : cv::cuda::Stream::Null());
	#else
		throw_no_cudacodec();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
void cudaVideoReaderFormat(cv::cudacodec::VideoReader* reader, cv::cudacodec::FormatInfo* formatInfo)
{
	try
	{
	#ifdef HAVE_OPENCV_CUDACODEC
		cv::cudacodec::FormatInfo fi = reader->format();
		memcpy(formatInfo, &fi, sizeof(cv::cudacodec::FormatInfo));
	#else
		throw_no_cudacodec();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}