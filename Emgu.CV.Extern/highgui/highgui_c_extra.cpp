//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "highgui_c_extra.h"

void cveImshow(cv::String* winname, cv::_InputArray* mat)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		cv::imshow(*winname, *mat);
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveNamedWindow(cv::String* winname, int flags)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		cv::namedWindow(*winname, flags);
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSetWindowProperty(cv::String* winname, int propId, double propValue)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		cv::setWindowProperty(*winname, propId, propValue);
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveSetWindowTitle(cv::String* winname, cv::String* title)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI

#if (defined WINAPI_FAMILY) && (WINAPI_FAMILY != WINAPI_FAMILY_DESKTOP_APP)
		CV_Error(cv::Error::StsBadFunc, "The library is compiled without SetWindowTitle support");
#else
		cv::setWindowTitle(*winname, *title);
#endif
	
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
double cveGetWindowProperty(cv::String* winname, int propId)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		return cv::getWindowProperty(*winname, propId);
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0.0)
}
void cveDestroyWindow(cv::String* winname)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		cv::destroyWindow(*winname);
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveDestroyAllWindows()
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		cv::destroyAllWindows();
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveWaitKey(int delay)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		return cv::waitKey(delay);
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

int cvePollKey()
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		return cv::pollKey();
#else
		throw_no_highgui();
#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveSelectROI(cv::String* windowName, cv::_InputArray* img, bool showCrosshair, bool fromCenter, cv::Rect* roi)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		cv::Rect r = cv::selectROI(*windowName, *img, showCrosshair, fromCenter);
		roi->x = r.x;
		roi->y = r.y;
		roi->width = r.width;
		roi->height = r.height;
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveSelectROIs(
	cv::String* windowName,
	cv::_InputArray* img,
	std::vector< cv::Rect >* boundingBoxes,
	bool showCrosshair,
	bool fromCenter)
{
	try
	{
#ifdef HAVE_OPENCV_HIGHGUI
		cv::selectROIs(*windowName, *img, *boundingBoxes, showCrosshair, fromCenter);
#else
		throw_no_highgui();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}