//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_LINE_DESCRIPTOR_C_H
#define EMGU_LINE_DESCRIPTOR_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/line_descriptor.hpp"

CVAPI(cv::line_descriptor::BinaryDescriptor*) cveLineDescriptorBinaryDescriptorCreate();
CVAPI(void) cveLineDescriptorBinaryDescriptorDetect(cv::line_descriptor::BinaryDescriptor* descriptor, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keypoints, cv::Mat* mask);
CVAPI(void) cveLineDescriptorBinaryDescriptorCompute(cv::line_descriptor::BinaryDescriptor* descriptor, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keylines, cv::Mat* descriptors, bool returnFloatDescr);
CVAPI(void) cveLineDescriptorBinaryDescriptorRelease(cv::line_descriptor::BinaryDescriptor** descriptor);

CVAPI(cv::line_descriptor::LSDDetector*) cveLineDescriptorLSDDetectorCreate();
CVAPI(void) cveLineDescriptorLSDDetectorDetect(cv::line_descriptor::LSDDetector* detector, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keypoints, int scale, int numOctaves, cv::Mat* mask);
CVAPI(void) cveLineDescriptorLSDDetectorRelease(cv::line_descriptor::LSDDetector** detector);
#endif