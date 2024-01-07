//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::QRCodeDetector* cveQRCodeDetectorCreate(cv::GraphicalCodeDetector** graphicalCodeDetector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::QRCodeDetector* result =  new cv::QRCodeDetector();
	*graphicalCodeDetector = static_cast<cv::GraphicalCodeDetector*>(result);
	return result;
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

void cveQRCodeDetectorDecodeCurved(cv::QRCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::String* decodedInfo, cv::_OutputArray* straightQrcode)
{
#ifdef HAVE_OPENCV_OBJDETECT
	std::string s = detector->decodeCurved(*img, *points, straightQrcode ? *straightQrcode : static_cast<cv::OutputArray>(cv::noArray()));
	*decodedInfo = s;
#else 
	throw_no_objdetect();
#endif
}

cv::QRCodeDetectorAruco* cveQRCodeDetectorArucoCreate(cv::GraphicalCodeDetector** graphicalCodeDetector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::QRCodeDetectorAruco* result = new cv::QRCodeDetectorAruco();
	*graphicalCodeDetector = static_cast<cv::GraphicalCodeDetector*>(result);
	return result;
#else 
	throw_no_objdetect();
#endif
}
void cveQRCodeDetectorArucoRelease(cv::QRCodeDetectorAruco** detector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* detector;
	*detector = 0;
#else 
	throw_no_objdetect();
#endif
}

cv::barcode::BarcodeDetector* cveBarcodeDetectorCreate(
	cv::String* prototxtPath,
	cv::String* modelPath,
	cv::GraphicalCodeDetector** graphicalCodeDetector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	cv::barcode::BarcodeDetector* result = 	new cv::barcode::BarcodeDetector(
		*prototxtPath,
		*modelPath);
	*graphicalCodeDetector = static_cast<cv::GraphicalCodeDetector*>(result);
	return result;
#else
	throw_no_objdetect();
#endif
}

void cveBarcodeDetectorRelease(cv::barcode::BarcodeDetector** detector)
{
#ifdef HAVE_OPENCV_OBJDETECT
	delete* detector;
	detector = 0;
#else
	throw_no_objdetect();
#endif
}



bool cveGraphicalCodeDetectorDetect(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return detector->detect(*img, *points);
#else 
	throw_no_objdetect();
#endif
}
bool cveGraphicalCodeDetectorDetectMulti(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_OutputArray* points)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return detector->detectMulti(*img, *points);
#else 
	throw_no_objdetect();
#endif
}

void cveGraphicalCodeDetectorDecode(cv::GraphicalCodeDetector* detector, cv::_InputArray* img, cv::_InputArray* points, cv::_OutputArray* straightCode, cv::String* output)
{
#ifdef HAVE_OPENCV_OBJDETECT
	*output = detector->decode(*img, *points, straightCode ? *straightCode : static_cast<cv::OutputArray>(cv::noArray()));
#else 
	throw_no_objdetect();
#endif
}

bool cveGraphicalCodeDetectorDecodeMulti(
	cv::GraphicalCodeDetector* detector,
	cv::_InputArray* img,
	cv::_InputArray* points,
	std::vector< std::string >* decodedInfo,
	cv::_OutputArray* straightCode)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return detector->decodeMulti(
		*img,
		*points,
		*decodedInfo,
		straightCode ? *straightCode : static_cast<cv::OutputArray>(cv::noArray())
	);
#else 
	throw_no_objdetect();
#endif
}

bool cveGraphicalCodeDetectorDetectAndDecodeMulti(
	cv::GraphicalCodeDetector* detector,
	cv::_InputArray* img,
	std::vector< std::string >* decodedInfo,
	cv::_OutputArray* points,
	cv::_OutputArray* straightCode)
{
#ifdef HAVE_OPENCV_OBJDETECT
	return detector->detectAndDecodeMulti(
		*img,
		*decodedInfo,
		*points,	
		straightCode ? *straightCode : static_cast<cv::OutputArray>(cv::noArray())
	);
#else 
	throw_no_objdetect();
#endif	
}
