//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
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
bool cveQRCodeDetectorDetect(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points)
{
	return detector->detect(*img, *points);
}

void cveQRCodeDetectorDecode(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightQrcode)
{
	std::string s = detector->decode(*img, *points, *straightQrcode);
	*decodedInfo = s;
}

/*
bool cveDetectQRCode(cv::_InputArray* in, std::vector< cv::Point >* points, double epsX, double epsY)
{
	return cv::detectQRCode(*in, *points, epsX, epsY);
}

bool cveDecodeQRCode(cv::_InputArray* in, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightQrcode)
{
	return cv::decodeQRCode(*in, *points, *decodedInfo, *straightQrcode);
}*/