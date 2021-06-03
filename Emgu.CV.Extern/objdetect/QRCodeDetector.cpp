//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
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
bool cveQRCodeDetectorDetectMulti(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points)
{
	return detector->detectMulti(*img, *points);
}

void cveQRCodeDetectorDecode(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightQrcode)
{
	std::string s = detector->decode(*img, *points, straightQrcode ? *straightQrcode : static_cast<cv::OutputArray>(cv::noArray()));
	*decodedInfo = s;
}

void cveQRCodeDetectorDecodeCurved(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightQrcode)
{
	std::string s = detector->decodeCurved(*img, *points, straightQrcode ? *straightQrcode : static_cast<cv::OutputArray>(cv::noArray()));
	*decodedInfo = s;
}

bool cveQRCodeDetectorDecodeMulti(
	cv::QRCodeDetector* detector,
	cv::_InputArray* img,
	cv::_InputArray* points,
	std::vector< std::string >* decodedInfo,
	cv::_OutputArray* straightQrcode)
{
	return detector->decodeMulti(
		*img,
		*points,
		*decodedInfo,
		straightQrcode ? *straightQrcode : static_cast<cv::OutputArray>(cv::noArray())
	);
}
