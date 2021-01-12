//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------



#pragma once
#ifndef EMGU_LINE_DESCRIPTOR_C_H
#define EMGU_LINE_DESCRIPTOR_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_LINE_DESCRIPTOR

#include "opencv2/line_descriptor.hpp"

#else

static inline CV_NORETURN void throw_no_line_descriptor() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without line descriptor support"); }

namespace cv
{
	namespace line_descriptor
	{
		class BinaryDescriptor
		{

		};

		class KeyLine
		{

		};

		class LSDDetector
		{

		};
	}
}

#endif



CVAPI(cv::line_descriptor::BinaryDescriptor*) cveLineDescriptorBinaryDescriptorCreate(cv::Ptr<cv::line_descriptor::BinaryDescriptor>** sharedPtr);
CVAPI(void) cveLineDescriptorBinaryDescriptorDetect(cv::line_descriptor::BinaryDescriptor* descriptor, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keypoints, cv::Mat* mask);
CVAPI(void) cveLineDescriptorBinaryDescriptorCompute(cv::line_descriptor::BinaryDescriptor* descriptor, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keylines, cv::Mat* descriptors, bool returnFloatDescr);
CVAPI(void) cveLineDescriptorBinaryDescriptorRelease(cv::Ptr<cv::line_descriptor::BinaryDescriptor>** sharedPtr);

CVAPI(cv::line_descriptor::LSDDetector*) cveLineDescriptorLSDDetectorCreate(cv::Ptr<cv::line_descriptor::LSDDetector>** sharedPtr);
CVAPI(void) cveLineDescriptorLSDDetectorDetect(cv::line_descriptor::LSDDetector* detector, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keypoints, int scale, int numOctaves, cv::Mat* mask);
CVAPI(void) cveLineDescriptorLSDDetectorRelease(cv::Ptr<cv::line_descriptor::LSDDetector>** sharedPtr);

#endif

