//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "barcode_c.h"

cv::barcode::BarcodeDetector* cveBarcodeDetectorCreate(
    cv::String* prototxtPath,
    cv::String* modelPath)
{
#ifdef HAVE_OPENCV_BARCODE
    return new cv::barcode::BarcodeDetector(
        *prototxtPath,
        *modelPath);
#else
    throw_no_barcode();
#endif
}

void cveBarcodeDetectorRelease(cv::barcode::BarcodeDetector** detector)
{
#ifdef HAVE_OPENCV_BARCODE
    delete* detector;
    detector = 0;
#else
    throw_no_barcode();
#endif
}

bool cveBarcodeDetectorDetect(
	cv::barcode::BarcodeDetector* detector,
	cv::_InputArray* img,
	cv::_OutputArray* points)
{
#ifdef HAVE_OPENCV_BARCODE
	return detector->detect(*img, *points);
#else
	throw_no_barcode();
#endif
}

bool cveBarcodeDetectorDecode(
	cv::barcode::BarcodeDetector* detector,
	cv::_InputArray* img,
	cv::_InputArray* points,
	std::vector< cv::String >* decodedInfo,
	std::vector< int >* decodedType)
{
#ifdef HAVE_OPENCV_BARCODE
	std::vector< cv::barcode::BarcodeType > decodedTypeVec;
	bool result = detector->decode(*img, *points, *decodedInfo, decodedTypeVec);
	decodedType->clear();
	for (std::vector< cv::barcode::BarcodeType >::iterator it = decodedTypeVec.begin(); it != decodedTypeVec.end(); ++it)
		decodedType->push_back(static_cast<int>(*it));
	return result;
#else
	throw_no_barcode();
#endif
}

bool cveBarcodeDetectorDetectAndDecode(
	cv::barcode::BarcodeDetector* detector,
	cv::_InputArray* img,
	std::vector< cv::String >* decodedInfo,
	std::vector< int >* decodedType,
	cv::_OutputArray* points)
{
#ifdef HAVE_OPENCV_BARCODE
	std::vector< cv::barcode::BarcodeType > decodedTypeVec;
	bool result = detector->detectAndDecode(*img, *decodedInfo, decodedTypeVec, points ? *points : static_cast<cv::OutputArray>(cv::noArray()));

	decodedType->clear();
	for (std::vector< cv::barcode::BarcodeType >::iterator it = decodedTypeVec.begin(); it != decodedTypeVec.end(); ++it)
		decodedType->push_back(static_cast<int>(*it));
	
	return result;
#else
	throw_no_barcode();
#endif
}