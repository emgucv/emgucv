//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
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
   cv::Ptr<cv::cudacodec::VideoWriter> ptr = cv::cudacodec::createVideoWriter(*fileName, *frameSize, fps, format);
   *sharedPtr = new cv::Ptr<cv::cudacodec::VideoWriter>(ptr);
   return ptr.get();
}
void cudaVideoWriterRelease(cv::Ptr<cv::cudacodec::VideoWriter>** writer)
{
   delete *writer;
   *writer = 0;
}
void cudaVideoWriterWrite(cv::cudacodec::VideoWriter* writer, cv::_InputArray* frame, bool lastFrame)
{
   writer->write(*frame, lastFrame);
}

//----------------------------------------------------------------------------
//
//  CudaVideoReader
//
//----------------------------------------------------------------------------

cv::cudacodec::VideoReader* cudaVideoReaderCreate(cv::String* fileName, cv::Ptr<cv::cudacodec::VideoReader>** sharedPtr)
{
   cv::Ptr<cv::cudacodec::VideoReader> ptr = cv::cudacodec::createVideoReader(*fileName);
   *sharedPtr = new cv::Ptr<cv::cudacodec::VideoReader>(ptr);
   return ptr.get();
}
void cudaVideoReaderRelease(cv::Ptr<cv::cudacodec::VideoReader>** reader)
{
   delete *reader;
   *reader = 0;
}
bool cudaVideoReaderNextFrame(cv::cudacodec::VideoReader* reader, cv::_OutputArray* frame)
{
   return reader->nextFrame(*frame);
}
