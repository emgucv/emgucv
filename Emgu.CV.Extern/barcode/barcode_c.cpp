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
	std::vector< cv::String >* decoded_info,
	std::vector< cv::barcode::BarcodeType >* decoded_type)
{
#ifdef HAVE_OPENCV_BARCODE
	return detector->decode(*img, *points, *decoded_info, *decoded_type);
#else
	throw_no_barcode();
#endif
}

bool cveBarcodeDetectorDetectAndDecode(
	cv::barcode::BarcodeDetector* detector,
	cv::_InputArray* img,
	std::vector< cv::String >* decoded_info,
	std::vector< cv::barcode::BarcodeType >* decoded_type,
	cv::_OutputArray* points)
{
#ifdef HAVE_OPENCV_BARCODE
	return detector->detectAndDecode(*img, *decoded_info, *decoded_type, points ? *points : static_cast<cv::OutputArray>(cv::noArray()));
#else
	throw_no_barcode();
#endif
}