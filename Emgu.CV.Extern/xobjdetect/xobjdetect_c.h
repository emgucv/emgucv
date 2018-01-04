//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_XOBJDETECT_C_H
#define EMGU_XOBJDETECT_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/xobjdetect.hpp"

CVAPI(cv::xobjdetect::WBDetector*) cveWBDetectorCreate();
CVAPI(void) cveWBDetectorRead(cv::xobjdetect::WBDetector* detector, cv::FileNode* node);
CVAPI(void) cveWBDetectorWrite(cv::xobjdetect::WBDetector* detector, cv::FileStorage* fs);
CVAPI(void) cveWBDetectorTrain(cv::xobjdetect::WBDetector* detector, cv::String* posSamples, cv::String* negImgs);
CVAPI(void) cveWBDetectorDetect(cv::xobjdetect::WBDetector* detector, cv::Mat* img, std::vector<cv::Rect>* bboxes, std::vector<double>* confidences);
CVAPI(void) cveWBDetectorRelease(cv::xobjdetect::WBDetector** detector);

#endif