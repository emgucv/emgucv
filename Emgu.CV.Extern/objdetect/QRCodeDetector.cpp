//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"
cv::QRCodeDetector* cveQRCodeDetectorCreate()
{
#ifdef HAVE_OPENCV_OBJDETECT
	return new cv::QRCodeDetector();
#else 
	throw_no_objdetect();
#endif
}
void cveQRCodeDetectorRelease(cv::QRCodeDetector** detector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete *detector;
	*detector = 0;
#else 
	throw_no_objdetect();
#endif
}
bool cveQRCodeDetectorDetect(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return detector->detect(*img, *points);
#else 
	throw_no_objdetect();
#endif
}
bool cveQRCodeDetectorDetectMulti(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return detector->detectMulti(*img, *points);
#else 
	throw_no_objdetect();
#endif
}

void cveQRCodeDetectorDecode(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightQrcode)
{
#ifdef HAVE_OPENCV_OBJDETECT
	std::string s = detector->decode(*img, *points, straightQrcode ? *straightQrcode : static_cast<cv::OutputArray>(cv::noArray()));
	*decodedInfo = s;
#else 
	throw_no_objdetect();
#endif
}

void cveQRCodeDetectorDecodeCurved(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightQrcode)
{
#ifdef HAVE_OPENCV_OBJDETECT
	std::string s = detector->decodeCurved(*img, *points, straightQrcode ? *straightQrcode : static_cast<cv::OutputArray>(cv::noArray()));
	*decodedInfo = s;
#else 
	throw_no_objdetect();
#endif
}

bool cveQRCodeDetectorDecodeMulti(
	cv::QRCodeDetector* detector,
	cv::_InputArray* img,
	cv::_InputArray* points,
	std::vector< std::string >* decodedInfo,
	cv::_OutputArray* straightQrcode)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return detector->decodeMulti(
		*img,
		*points,
		*decodedInfo,
		straightQrcode ? *straightQrcode : static_cast<cv::OutputArray>(cv::noArray())
	);
#else 
	throw_no_objdetect();
#endif
}
