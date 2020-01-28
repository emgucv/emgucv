//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "highgui_c_extra.h"

void cveImshow(cv::String* winname, cv::_InputArray* mat)
{
  cv::imshow(*winname, *mat);
}
void cveNamedWindow(cv::String* winname, int flags)
{
  cv::namedWindow(*winname, flags);
}
void cveDestroyWindow(cv::String* winname)
{
  cv::destroyWindow(*winname);
}
void cveDestroyAllWindows()
{
  cv::destroyAllWindows();
}
int cveWaitKey(int delay)
{
  return cv::waitKey(delay);
}

void cveSelectROI(cv::String* windowName, cv::_InputArray* img, bool showCrosshair, bool fromCenter, CvRect* roi)
{
	cv::Rect r = cv::selectROI(*windowName, *img, showCrosshair, fromCenter);
	roi->x = r.x;
	roi->y = r.y;
	roi->width = r.width;
	roi->height = r.height;
}

void cveSelectROIs(
	cv::String* windowName,
	cv::_InputArray* img,
	std::vector< cv::Rect >* boundingBoxs,
	bool showCrosshair,
	bool fromCenter)
{
	cv::selectROIs(*windowName, *img, *boundingBoxs, showCrosshair, fromCenter);
}