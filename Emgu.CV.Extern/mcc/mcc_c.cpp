//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "mcc_c.h"

cv::mcc::CChecker* cveCCheckerCreate(cv::Ptr<cv::mcc::CChecker>** sharedPtr)
{
#ifdef HAVE_OPENCV_MCC
	cv::Ptr<cv::mcc::CChecker> checker = cv::mcc::CChecker::create();
	*sharedPtr = new cv::Ptr<cv::mcc::CChecker>(checker);
	return (*sharedPtr)->get();
#else
	throw_no_mcc();
#endif
}
void cveCCheckerRelease(cv::Ptr<cv::mcc::CChecker>** sharedPtr)
{
#ifdef HAVE_OPENCV_MCC
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_mcc();
#endif
}

cv::mcc::CCheckerDraw* cveCCheckerDrawCreate(
	cv::mcc::CChecker* pChecker,
	CvScalar* color,
	int thickness,
	cv::Ptr<cv::mcc::CCheckerDraw>** sharedPtr)
{
#ifdef HAVE_OPENCV_MCC
	cv::Ptr<cv::mcc::CChecker> cCheckerPtr(pChecker, [](cv::mcc::CChecker* p) {});
	cv::Ptr<cv::mcc::CCheckerDraw> checkerDraw = cv::mcc::CCheckerDraw::create(
		cCheckerPtr,
		*color,
		thickness);
	*sharedPtr = new cv::Ptr<cv::mcc::CCheckerDraw>(checkerDraw);
	return (*sharedPtr)->get();
#else
	throw_no_mcc();
#endif
}

void cveCCheckerDrawDraw(cv::mcc::CCheckerDraw* ccheckerDraw, cv::_InputOutputArray* img)
{
#ifdef HAVE_OPENCV_MCC
	ccheckerDraw->draw(*img);
#else
	throw_no_mcc();
#endif
}
void cveCCheckerDrawRelease(cv::Ptr<cv::mcc::CCheckerDraw>** sharedPtr)
{
#ifdef HAVE_OPENCV_MCC
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_mcc();
#endif
  
}

