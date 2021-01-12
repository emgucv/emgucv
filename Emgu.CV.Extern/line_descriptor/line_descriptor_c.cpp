//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "line_descriptor_c.h"

cv::line_descriptor::BinaryDescriptor* cveLineDescriptorBinaryDescriptorCreate(cv::Ptr<cv::line_descriptor::BinaryDescriptor>** sharedPtr)
{
#ifdef HAVE_OPENCV_LINE_DESCRIPTOR
	cv::Ptr<cv::line_descriptor::BinaryDescriptor> ptr = cv::line_descriptor::BinaryDescriptor::createBinaryDescriptor();
	*sharedPtr = new cv::Ptr<cv::line_descriptor::BinaryDescriptor>(ptr);
	return ptr.get();
#else
	throw_no_line_descriptor();
#endif
}
void cveLineDescriptorBinaryDescriptorDetect(cv::line_descriptor::BinaryDescriptor* descriptor, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keypoints, cv::Mat* mask)
{
#ifdef HAVE_OPENCV_LINE_DESCRIPTOR
	descriptor->detect(*image, *keypoints, mask ? *mask : cv::Mat());
#else
	throw_no_line_descriptor();
#endif
}
void cveLineDescriptorBinaryDescriptorCompute(cv::line_descriptor::BinaryDescriptor* descriptor, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keylines, cv::Mat* descriptors, bool returnFloatDescr)
{
#ifdef HAVE_OPENCV_LINE_DESCRIPTOR
	descriptor->compute(*image, *keylines, *descriptors, returnFloatDescr);
#else
	throw_no_line_descriptor();
#endif
}

void cveLineDescriptorBinaryDescriptorRelease(cv::Ptr<cv::line_descriptor::BinaryDescriptor>** sharedPtr)
{
#ifdef HAVE_OPENCV_LINE_DESCRIPTOR
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_line_descriptor();
#endif
}

cv::line_descriptor::LSDDetector* cveLineDescriptorLSDDetectorCreate(cv::Ptr<cv::line_descriptor::LSDDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_LINE_DESCRIPTOR
	cv::Ptr<cv::line_descriptor::LSDDetector> ptr = cv::line_descriptor::LSDDetector::createLSDDetector();
	*sharedPtr = new cv::Ptr<cv::line_descriptor::LSDDetector>(ptr);
	return ptr.get();
#else
	throw_no_line_descriptor();
#endif
}
void cveLineDescriptorLSDDetectorDetect(cv::line_descriptor::LSDDetector* detector, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keypoints, int scale, int numOctaves, cv::Mat* mask)
{
#ifdef HAVE_OPENCV_LINE_DESCRIPTOR
	detector->detect(*image, *keypoints, scale, numOctaves, mask ? *mask : cv::Mat());
#else
	throw_no_line_descriptor();
#endif
}
void cveLineDescriptorLSDDetectorRelease(cv::Ptr<cv::line_descriptor::LSDDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_LINE_DESCRIPTOR
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_line_descriptor();
#endif
}

