//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "videoio_c_extra.h"

cv::VideoCapture* cveVideoCaptureCreateFromDevice(int device, int apiPreference, std::vector< int >* params)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		if (params && !params->empty())
		{
			return new cv::VideoCapture(device, apiPreference, *params);
		}
		else
		{
			return new cv::VideoCapture(device, apiPreference);
		}
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

cv::VideoCapture* cveVideoCaptureCreateFromFile(cv::String* fileName, int apiPreference, std::vector< int >* params)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		if (params && !params->empty())
		{
			return new cv::VideoCapture(*fileName, apiPreference, *params);
		}
		else
		{
			return new cv::VideoCapture(*fileName, apiPreference);
		}
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveVideoCaptureRelease2(cv::VideoCapture** capture)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		delete *capture;
		*capture = 0;
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
bool cveVideoCaptureSet(cv::VideoCapture* capture, int propId, double value)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return capture->set(propId, value);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
double cveVideoCaptureGet(cv::VideoCapture* capture, int propId)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return capture->get(propId);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
bool cveVideoCaptureGrab(cv::VideoCapture* capture)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return capture->grab();
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
bool cveVideoCaptureRetrieve(cv::VideoCapture* capture, cv::_OutputArray* image, int flag)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return capture->retrieve(*image, flag);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
bool cveVideoCaptureRead(cv::VideoCapture* capture, cv::_OutputArray* image)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return capture->read(*image);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}


void cveVideoCaptureReadToMat(cv::VideoCapture* capture, cv::Mat* mat)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		(*capture) >> *mat;
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveVideoCaptureReadToUMat(cv::VideoCapture* capture, cv::UMat* umat)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		(*capture) >> *umat;
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveVideoCaptureGetBackendName(cv::VideoCapture* capture, cv::String* name)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		*name = capture->getBackendName();
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

bool cveVideoCaptureWaitAny(std::vector<cv::VideoCapture>* streams, std::vector<int>* readyIndex, int timeoutNs)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return cv::VideoCapture::waitAny(*streams, *readyIndex, timeoutNs);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

#if WINAPI_FAMILY
void cveWinrtSetFrameContainer(::Windows::UI::Xaml::Controls::Image^ image)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		cv::winrt_setFrameContainer(image);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveWinrtStartMessageLoop(CvWinrtMessageLoopCallback callback)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		cv::winrt_startMessageLoop(callback);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveWinrtImshow()
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		cv::winrt_imshow();
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveWinrtOnVisibilityChanged(bool visible)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		cv::winrt_onVisibilityChanged(visible);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
#endif

cv::VideoWriter* cveVideoWriterCreate(cv::String* filename, int fourcc, double fps, cv::Size* frameSize, bool isColor)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return new cv::VideoWriter(*filename, fourcc, fps, *frameSize, isColor);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::VideoWriter* cveVideoWriterCreate2(cv::String* filename, int apiPreference, int fourcc, double fps, cv::Size* frameSize, bool isColor)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return new cv::VideoWriter(*filename, apiPreference, fourcc, fps, *frameSize, isColor);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
cv::VideoWriter* cveVideoWriterCreate3(cv::String* filename, int apiPreference, int fourcc, double fps, cv::Size* frameSize, std::vector< int >* params)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return new cv::VideoWriter(*filename, apiPreference, fourcc, fps, *frameSize, *params);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveVideoWriterRelease(cv::VideoWriter** writer)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		delete *writer;
		*writer = 0;
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
bool cveVideoWriterIsOpened(cv::VideoWriter* writer)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return writer->isOpened();
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
bool cveVideoWriterSet(cv::VideoWriter* writer, int propId, double value)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return writer->set(propId, value);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}
double cveVideoWriterGet(cv::VideoWriter* writer, int propId)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return writer->get(propId);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveVideoWriterWrite(cv::VideoWriter* writer, cv::_InputArray* image)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		writer->write(*image);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveVideoWriterFourcc(char c1, char c2, char c3, char c4)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		return cv::VideoWriter::fourcc(c1, c2, c3, c4);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveVideoWriterGetBackendName(cv::VideoWriter* writer, cv::String* name)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		* name = writer->getBackendName();
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}


void cveGetBackendName(int api, cv::String* name)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		*name = cv::videoio_registry::getBackendName((cv::VideoCaptureAPIs) api);
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGetBackends(std::vector<int>* backends)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		std::vector<cv::VideoCaptureAPIs> b = cv::videoio_registry::getBackends();
		backends->clear();
		for (std::vector<cv::VideoCaptureAPIs>::iterator it = b.begin(); it != b.end(); ++it)
		{
			backends->push_back(static_cast<int>(*it));
		}
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveGetCameraBackends(std::vector<int>* backends)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		std::vector<cv::VideoCaptureAPIs> b = cv::videoio_registry::getCameraBackends();
		backends->clear();
		for (std::vector<cv::VideoCaptureAPIs>::iterator it = b.begin(); it != b.end(); ++it)
		{
			backends->push_back(static_cast<int>(*it));
		}
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGetStreamBackends(std::vector<int>* backends)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		std::vector<cv::VideoCaptureAPIs> b = cv::videoio_registry::getStreamBackends();
		backends->clear();
		for (std::vector<cv::VideoCaptureAPIs>::iterator it = b.begin(); it != b.end(); ++it)
		{
			backends->push_back(static_cast<int>(*it));
		}
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveGetWriterBackends(std::vector<int>* backends)
{
	try
	{
	#ifdef HAVE_OPENCV_VIDEOIO
		std::vector<cv::VideoCaptureAPIs> b = cv::videoio_registry::getWriterBackends();
		backends->clear();
		for (std::vector<cv::VideoCaptureAPIs>::iterator it = b.begin(); it != b.end(); ++it)
		{
			backends->push_back(static_cast<int>(*it));
		}
	#else
		throw_no_videoio();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

