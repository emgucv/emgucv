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
#ifdef HAVE_OPENCV_RAPID
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_mcc();
#endif
}
