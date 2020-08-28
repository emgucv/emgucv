//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "hfs_c.h"

cv::hfs::HfsSegment* cveHfsSegmentCreate(
	int height,
	int width,
	float segEgbThresholdI,
	int minRegionSizeI,
	float segEgbThresholdII,
	int minRegionSizeII,
	float spatialWeight,
	int slicSpixelSize,
	int numSlicIter,
	cv::Algorithm** algorithmPtr,
	cv::Ptr<cv::hfs::HfsSegment>** sharedPtr)
{
#ifdef HAVE_OPENCV_HFS
	cv::Ptr<cv::hfs::HfsSegment> ptr = cv::hfs::HfsSegment::create(height, width, segEgbThresholdI, minRegionSizeI, segEgbThresholdII, minRegionSizeII, spatialWeight, slicSpixelSize, numSlicIter);
	*sharedPtr = new cv::Ptr<cv::hfs::HfsSegment>(ptr);
	cv::hfs::HfsSegment* r = (*sharedPtr)->get();
	*algorithmPtr = dynamic_cast<cv::hfs::HfsSegment*>(r);
	return r;
#else
	throw_no_hfs();
#endif
}

void cveHfsSegmentRelease(cv::Ptr<cv::hfs::HfsSegment>** hfsSegmentPtr)
{
#ifdef HAVE_OPENCV_HFS
	delete *hfsSegmentPtr;
	*hfsSegmentPtr = 0;
#else
	throw_no_hfs();
#endif
}

void cveHfsPerformSegment(cv::hfs::HfsSegment* hfsSegment, cv::_InputArray* src, cv::Mat* dst, bool ifDraw, bool useGpu)
{
#ifdef HAVE_OPENCV_HFS
	if (useGpu)
	{
		cv::Mat m = hfsSegment->performSegmentGpu(*src, ifDraw);
		cv::swap(m, *dst);
	} else
	{
		cv::Mat m = hfsSegment->performSegmentCpu(*src, ifDraw);
		cv::swap(m, *dst);
	}
#else
	throw_no_hfs();
#endif
}