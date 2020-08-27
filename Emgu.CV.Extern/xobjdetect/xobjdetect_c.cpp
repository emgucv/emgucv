//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "xobjdetect_c.h"

cv::xobjdetect::WBDetector* cveWBDetectorCreate(cv::Ptr<cv::xobjdetect::WBDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	cv::Ptr<cv::xobjdetect::WBDetector> ptr = cv::xobjdetect::WBDetector::create();
	*sharedPtr = new cv::Ptr<cv::xobjdetect::WBDetector>(ptr);
	return ptr.get();
#else
	throw_no_xobjdetect();
#endif
}
void cveWBDetectorRead(cv::xobjdetect::WBDetector* detector, cv::FileNode* node)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	detector->read(*node);
#else
	throw_no_xobjdetect();
#endif
}
void cveWBDetectorWrite(cv::xobjdetect::WBDetector* detector, cv::FileStorage* fs)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	detector->write(*fs);
#else
	throw_no_xobjdetect();
#endif
}
void cveWBDetectorTrain(cv::xobjdetect::WBDetector* detector, cv::String* posSamples, cv::String* negImgs)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	detector->train(*posSamples, *negImgs);
#else
	throw_no_xobjdetect();
#endif
}
void cveWBDetectorDetect(cv::xobjdetect::WBDetector* detector, cv::Mat* img, std::vector<cv::Rect>* bboxes, std::vector<double>* confidences)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	detector->detect(*img, *bboxes, *confidences);
#else
	throw_no_xobjdetect();
#endif
}
void cveWBDetectorRelease(cv::xobjdetect::WBDetector** detector, cv::Ptr<cv::xobjdetect::WBDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_XOBJDETECT
	delete *sharedPtr;
	*detector = 0;
	*sharedPtr = 0;
#else
	throw_no_xobjdetect();
#endif
}
