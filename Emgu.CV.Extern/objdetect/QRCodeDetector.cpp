//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"
cv::QRCodeDetector* cveQRCodeDetectorCreate()
{
	return new cv::QRCodeDetector();
}
void cveQRCodeDetectorRelease(cv::QRCodeDetector** detector)
{
	delete *detector;
	*detector = 0;
}
bool cveQRCodeDetectorDetect(cv::QRCodeDetector* detector, cv::_InputArray* in, cv::_OutputArray* points)
{
	return detector->detect(*in, *points);
}