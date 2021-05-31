//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "videoio_c_extra.h"


void OpenniGetColorPoints(CvCapture* capture, std::vector<ColorPoint>* points, IplImage* maskImg)
{
#ifdef HAVE_OPENCV_VIDEOIO
	IplImage* pcm = cvRetrieveFrame(capture, CV_CAP_OPENNI_POINT_CLOUD_MAP); //XYZ in meters (CV_32FC3)
	IplImage* bgr = cvRetrieveFrame(capture, CV_CAP_OPENNI_BGR_IMAGE); //CV_8UC3

	int ptCount = pcm->width * pcm->height;
	CvPoint3D32f* position = (CvPoint3D32f*)pcm->imageData;
	unsigned char* color = (unsigned char*)bgr->imageData;

	//int colorIdx = 0;
	ColorPoint cp;
	if (maskImg)
	{
		unsigned char* mask = (unsigned char*)maskImg->imageData;
		for (int i = 0; i < ptCount; i++, mask++, position++, color += 3)
			if (*mask)
			{
				memcpy(&cp.position, position, sizeof(CvPoint3D32f));
				cp.blue = *color;
				cp.green = *(color + 1);
				cp.red = *(color + 2);
				//memcpy(&cp.red, color, 3);
				points->push_back(cp);

			}
	}
	else
	{
		for (int i = 0; i < ptCount; i++, position++, color += 3)
		{
			memcpy(&cp.position, position, sizeof(CvPoint3D32f));
			cp.blue = *color;
			cp.green = *(color + 1);
			cp.red = *(color + 2);
			//memcpy(&cp.red, color, 3);
			points->push_back(cp);

		}
	}
#else
	throw_no_videoio();
#endif
}

cv::VideoCapture* cveVideoCaptureCreateFromDevice(int device, int apiPreference, std::vector< int >* params)
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

cv::VideoCapture* cveVideoCaptureCreateFromFile(cv::String* fileName, int apiPreference, std::vector< int >* params)
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

void cveVideoCaptureRelease(cv::VideoCapture** capture)
{
#ifdef HAVE_OPENCV_VIDEOIO
	delete *capture;
	*capture = 0;
#else
	throw_no_videoio();
#endif
}
bool cveVideoCaptureSet(cv::VideoCapture* capture, int propId, double value)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return capture->set(propId, value);
#else
	throw_no_videoio();
#endif
}
double cveVideoCaptureGet(cv::VideoCapture* capture, int propId)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return capture->get(propId);
#else
	throw_no_videoio();
#endif
}
bool cveVideoCaptureGrab(cv::VideoCapture* capture)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return capture->grab();
#else
	throw_no_videoio();
#endif
}
bool cveVideoCaptureRetrieve(cv::VideoCapture* capture, cv::_OutputArray* image, int flag)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return capture->retrieve(*image, flag);
#else
	throw_no_videoio();
#endif
}
bool cveVideoCaptureRead(cv::VideoCapture* capture, cv::_OutputArray* image)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return capture->read(*image);
#else
	throw_no_videoio();
#endif
}


void cveVideoCaptureReadToMat(cv::VideoCapture* capture, cv::Mat* mat)
{
#ifdef HAVE_OPENCV_VIDEOIO
	(*capture) >> *mat;
#else
	throw_no_videoio();
#endif
}

void cveVideoCaptureReadToUMat(cv::VideoCapture* capture, cv::UMat* umat)
{
#ifdef HAVE_OPENCV_VIDEOIO
	(*capture) >> *umat;
#else
	throw_no_videoio();
#endif
}

void cveVideoCaptureGetBackendName(cv::VideoCapture* capture, cv::String* name)
{
#ifdef HAVE_OPENCV_VIDEOIO
	*name = capture->getBackendName();
#else
	throw_no_videoio();
#endif
}

bool cveVideoCaptureWaitAny(std::vector<cv::VideoCapture>* streams, std::vector<int>* readyIndex, int timeoutNs)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return cv::VideoCapture::waitAny(*streams, *readyIndex, timeoutNs);
#else
	throw_no_videoio();
#endif
}

#if WINAPI_FAMILY
void cveWinrtSetFrameContainer(::Windows::UI::Xaml::Controls::Image^ image)
{
#ifdef HAVE_OPENCV_VIDEOIO
	cv::winrt_setFrameContainer(image);
#else
	throw_no_videoio();
#endif
}
void cveWinrtStartMessageLoop(CvWinrtMessageLoopCallback callback)
{
#ifdef HAVE_OPENCV_VIDEOIO
	cv::winrt_startMessageLoop(callback);
#else
	throw_no_videoio();
#endif
}
void cveWinrtImshow()
{
#ifdef HAVE_OPENCV_VIDEOIO
	cv::winrt_imshow();
#else
	throw_no_videoio();
#endif
}
void cveWinrtOnVisibilityChanged(bool visible)
{
#ifdef HAVE_OPENCV_VIDEOIO
	cv::winrt_onVisibilityChanged(visible);
#else
	throw_no_videoio();
#endif
}
#endif

cv::VideoWriter* cveVideoWriterCreate(cv::String* filename, int fourcc, double fps, CvSize* frameSize, bool isColor)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return new cv::VideoWriter(*filename, fourcc, fps, *frameSize, isColor);
#else
	throw_no_videoio();
#endif
}
cv::VideoWriter* cveVideoWriterCreate2(cv::String* filename, int apiPreference, int fourcc, double fps, CvSize* frameSize, bool isColor)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return new cv::VideoWriter(*filename, apiPreference, fourcc, fps, *frameSize, isColor);
#else
	throw_no_videoio();
#endif
}
cv::VideoWriter* cveVideoWriterCreate3(cv::String* filename, int apiPreference, int fourcc, double fps, CvSize* frameSize, std::vector< int >* params)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return new cv::VideoWriter(*filename, apiPreference, fourcc, fps, *frameSize, *params);
#else
	throw_no_videoio();
#endif
}
void cveVideoWriterRelease(cv::VideoWriter** writer)
{
#ifdef HAVE_OPENCV_VIDEOIO
	delete *writer;
	*writer = 0;
#else
	throw_no_videoio();
#endif
}
bool cveVideoWriterIsOpened(cv::VideoWriter* writer)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return writer->isOpened();
#else
	throw_no_videoio();
#endif
}
bool cveVideoWriterSet(cv::VideoWriter* writer, int propId, double value)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return writer->set(propId, value);
#else
	throw_no_videoio();
#endif
}
double cveVideoWriterGet(cv::VideoWriter* writer, int propId)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return writer->get(propId);
#else
	throw_no_videoio();
#endif
}

void cveVideoWriterWrite(cv::VideoWriter* writer, cv::Mat* image)
{
#ifdef HAVE_OPENCV_VIDEOIO
	writer->write(*image);
#else
	throw_no_videoio();
#endif
}
int cveVideoWriterFourcc(char c1, char c2, char c3, char c4)
{
#ifdef HAVE_OPENCV_VIDEOIO
	return cv::VideoWriter::fourcc(c1, c2, c3, c4);
#else
	throw_no_videoio();
#endif
}

void cveGetBackendName(int api, cv::String* name)
{
#ifdef HAVE_OPENCV_VIDEOIO
	*name = cv::videoio_registry::getBackendName((cv::VideoCaptureAPIs) api);
#else
	throw_no_videoio();
#endif
}
void cveGetBackends(std::vector<int>* backends)
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

void cveGetCameraBackends(std::vector<int>* backends)
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
void cveGetStreamBackends(std::vector<int>* backends)
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
void cveGetWriterBackends(std::vector<int>* backends)
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
