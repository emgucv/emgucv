//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "highgui_c_extra.h"

void cveImshow(cv::String* winname, cv::_InputArray* mat)
{
#ifdef HAVE_OPENCV_HIGHGUI
	cv::imshow(*winname, *mat);
#else
	throw_no_highgui();
#endif
}
void cveNamedWindow(cv::String* winname, int flags)
{
#ifdef HAVE_OPENCV_HIGHGUI
	cv::namedWindow(*winname, flags);
#else
	throw_no_highgui();
#endif
}
void cveSetWindowProperty(cv::String* winname, int propId, double propValue)
{
#ifdef HAVE_OPENCV_HIGHGUI
	cv::setWindowProperty(*winname, propId, propValue);
#else
	throw_no_highgui();
#endif
}
void cveSetWindowTitle(cv::String* winname, cv::String* title)
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
double cveGetWindowProperty(cv::String* winname, int propId)
{
#ifdef HAVE_OPENCV_HIGHGUI
	return cv::getWindowProperty(*winname, propId);
#else
	throw_no_highgui();
#endif
}
void cveDestroyWindow(cv::String* winname)
{
#ifdef HAVE_OPENCV_HIGHGUI
	cv::destroyWindow(*winname);
#else
	throw_no_highgui();
#endif
}
void cveDestroyAllWindows()
{
#ifdef HAVE_OPENCV_HIGHGUI
	cv::destroyAllWindows();
#else
	throw_no_highgui();
#endif
}
int cveWaitKey(int delay)
{
#ifdef HAVE_OPENCV_HIGHGUI
	return cv::waitKey(delay);
#else
	throw_no_highgui();
#endif
}

void cveSelectROI(cv::String* windowName, cv::_InputArray* img, bool showCrosshair, bool fromCenter, CvRect* roi)
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

void cveSelectROIs(
	cv::String* windowName,
	cv::_InputArray* img,
	std::vector< cv::Rect >* boundingBoxs,
	bool showCrosshair,
	bool fromCenter)
{
#ifdef HAVE_OPENCV_HIGHGUI
	cv::selectROIs(*windowName, *img, *boundingBoxs, showCrosshair, fromCenter);
#else
	throw_no_highgui();
#endif
}