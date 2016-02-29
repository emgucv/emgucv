//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "line_descriptor_c.h"

cv::line_descriptor::BinaryDescriptor* cveLineDescriptorBinaryDescriptorCreate()
{
   cv::Ptr<cv::line_descriptor::BinaryDescriptor> ptr = cv::line_descriptor::BinaryDescriptor::createBinaryDescriptor();
   ptr.addref();
   return ptr.get();
}
void cveBinaryDescriptorDetect(cv::line_descriptor::BinaryDescriptor* descriptor, cv::Mat* image, std::vector<cv::line_descriptor::KeyLine>* keypoints, cv::Mat* mask)
{
   descriptor->detect(*image, *keypoints, mask ? *mask : cv::Mat());
}
void cveLineDescriptorBinaryDescriptoyRelease(cv::line_descriptor::BinaryDescriptor** descriptor)
{
   delete * descriptor;
   *descriptor = 0;
}