//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XOBJDETECT_C_H
#define EMGU_XOBJDETECT_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_XOBJDETECT
#include "opencv2/xobjdetect.hpp"
#else
static inline CV_NORETURN void throw_no_xobjdetect() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without xobjdetect support"); }
namespace cv {
	namespace xobjdetect {
		class WBDetector {};
	}
}
#endif
CVAPI(cv::xobjdetect::WBDetector*) cveWBDetectorCreate(cv::Ptr<cv::xobjdetect::WBDetector>** sharedPtr);
CVAPI(void) cveWBDetectorRead(cv::xobjdetect::WBDetector* detector, cv::FileNode* node);
CVAPI(void) cveWBDetectorWrite(cv::xobjdetect::WBDetector* detector, cv::FileStorage* fs);
CVAPI(void) cveWBDetectorTrain(cv::xobjdetect::WBDetector* detector, cv::String* posSamples, cv::String* negImgs);
CVAPI(void) cveWBDetectorDetect(cv::xobjdetect::WBDetector* detector, cv::Mat* img, std::vector<cv::Rect>* bboxes, std::vector<double>* confidences);
CVAPI(void) cveWBDetectorRelease(cv::xobjdetect::WBDetector** detector, cv::Ptr<cv::xobjdetect::WBDetector>** sharedPtr);

#endif