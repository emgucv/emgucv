//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_HIGHGUI_C_H
#define EMGU_HIGHGUI_C_H

#include "opencv2/opencv_modules.hpp"
#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_VIDEOIO
#include "opencv2/videoio/videoio_c.h"
#include "opencv2/videoio/videoio.hpp"
#include "opencv2/videoio/registry.hpp"

#if WINAPI_FAMILY
//using namespace System;
//using namespace System::Runtime::InteropServices;
#include "opencv2/videoio/cap_winrt.hpp"
//#include <msclr/gcroot.h>
#endif

#else
static inline CV_NORETURN void throw_no_videoio() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without Videoio support"); }
class CvCapture
{	
};
namespace cv
{
	class VideoCapture
	{
		
	};

	class VideoWriter
	{
		
	};
}

#endif

struct ColorPoint
{
   CvPoint3D32f position;
   unsigned char blue;
   unsigned char green;
   unsigned char red;
};

namespace cv {
	namespace traits {
		template<>
		struct Depth < ColorPoint > { enum { value = Depth<uchar>::value }; };
		template<>
		struct Type< ColorPoint > { enum { value = CV_MAKETYPE(Depth<uchar>::value, sizeof(ColorPoint)) }; };
	}
}

CVAPI(void) OpenniGetColorPoints(
                                 CvCapture* capture, // must be an openni capture
                                 std::vector<ColorPoint>* points, // sequence of ColorPoint
                                 IplImage* mask // CV_8UC1
                                 );

CVAPI(cv::VideoCapture*) cveVideoCaptureCreateFromDevice(int device, int apiPreference, std::vector< int >* params);
CVAPI(cv::VideoCapture*) cveVideoCaptureCreateFromFile(cv::String* fileName, int apiPreference, std::vector< int >* params);

CVAPI(void) cveVideoCaptureRelease(cv::VideoCapture** capture);
CVAPI(bool) cveVideoCaptureSet(cv::VideoCapture* capture, int propId, double value);
CVAPI(double) cveVideoCaptureGet(cv::VideoCapture* capture, int propId);
CVAPI(bool) cveVideoCaptureGrab(cv::VideoCapture* capture);
CVAPI(bool) cveVideoCaptureRetrieve(cv::VideoCapture* capture, cv::_OutputArray* image, int flag);
CVAPI(bool) cveVideoCaptureRead(cv::VideoCapture* capture, cv::_OutputArray* image);
CVAPI(void) cveVideoCaptureReadToMat(cv::VideoCapture* capture, cv::Mat* mat);
CVAPI(void) cveVideoCaptureReadToUMat(cv::VideoCapture* capture, cv::UMat* umat);
CVAPI(void) cveVideoCaptureGetBackendName(cv::VideoCapture* capture, cv::String* name);

CVAPI(bool) cveVideoCaptureWaitAny(std::vector<cv::VideoCapture>* streams, std::vector<int>* readyIndex, int timeoutNs);

#if WINAPI_FAMILY
CVAPI(void) cveWinrtSetFrameContainer(::Windows::UI::Xaml::Controls::Image^ image);
typedef void (CV_CDECL *CvWinrtMessageLoopCallback)();
CVAPI(void) cveWinrtStartMessageLoop(CvWinrtMessageLoopCallback callback);
CVAPI(void) cveWinrtImshow();
CVAPI(void) cveWinrtOnVisibilityChanged(bool visible);
#endif

CVAPI(cv::VideoWriter*) cveVideoWriterCreate(cv::String* filename, int fourcc, double fps, CvSize* frameSize, bool isColor);
CVAPI(cv::VideoWriter*) cveVideoWriterCreate2(cv::String* filename, int apiPreference, int fourcc, double fps, CvSize* frameSize, bool isColor);
CVAPI(cv::VideoWriter*) cveVideoWriterCreate3(cv::String* filename, int apiPreference, int fourcc, double fps, CvSize* frameSize, std::vector< int >* params);
CVAPI(bool) cveVideoWriterIsOpened(cv::VideoWriter* writer);
CVAPI(bool) cveVideoWriterSet(cv::VideoWriter* writer, int propId, double value);
CVAPI(double) cveVideoWriterGet(cv::VideoWriter* writer, int propId);
CVAPI(void) cveVideoWriterRelease(cv::VideoWriter** writer);
CVAPI(void) cveVideoWriterWrite(cv::VideoWriter* writer, cv::Mat* image);
CVAPI(int) cveVideoWriterFourcc(char c1, char c2, char c3, char c4);

CVAPI(void) cveGetBackendName(int api, cv::String* name);
CVAPI(void) cveGetBackends(std::vector<int>* backends);
CVAPI(void) cveGetCameraBackends(std::vector<int>* backends);
CVAPI(void) cveGetStreamBackends(std::vector<int>* backends);
CVAPI(void) cveGetWriterBackends(std::vector<int>* backends);

#endif