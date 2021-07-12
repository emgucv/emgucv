//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_BARCODE_C_H
#define EMGU_BARCODE_C_H

#include "opencv2/core/core_c.h"
#ifdef HAVE_OPENCV_BARCODE
#include "opencv2/barcode.hpp"
#else
static inline CV_NORETURN void throw_no_barcode() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without barcode support"); }

namespace cv
{
	namespace barcode
	{
		class BarcodeDetector {};
		//enum BarcodeType;
	}
}

#endif

CVAPI(cv::barcode::BarcodeDetector*) cveBarcodeDetectorCreate(
    cv::String* prototxtPath,
    cv::String* modelPath);

CVAPI(void) cveBarcodeDetectorRelease(cv::barcode::BarcodeDetector** detector);


CVAPI(bool) cveBarcodeDetectorDetect(
	cv::barcode::BarcodeDetector* detector, 
	cv::_InputArray* img, 
	cv::_OutputArray* points);

CVAPI(bool) cveBarcodeDetectorDecode(
	cv::barcode::BarcodeDetector* detector,
	cv::_InputArray* img, 
	cv::_InputArray* points, 
	std::vector< cv::String >* decoded_info,
    std::vector< int >* decoded_type);

CVAPI(bool) cveBarcodeDetectorDetectAndDecode(
	cv::barcode::BarcodeDetector* detector, 
	cv::_InputArray* img, 
	std::vector< cv::String >* decoded_info, 
    std::vector< int >* decoded_type, 
	cv::_OutputArray* points);

#endif