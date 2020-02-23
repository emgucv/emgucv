//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "superres_c.h"

cv::superres::FrameSource* cveSuperresCreateFrameSourceVideo(cv::String* fileName, bool useGpu, cv::Ptr<cv::superres::FrameSource>** sharedPtr)
{
#ifdef HAVE_OPENCV_SUPERRES
	cv::Ptr<cv::superres::FrameSource> ptr = useGpu ?
		cv::superres::createFrameSource_Video_CUDA(*fileName)
		: cv::superres::createFrameSource_Video(*fileName);
	*sharedPtr = new cv::Ptr<cv::superres::FrameSource>(ptr);
	return ptr.get();
#else
	throw_no_superres();
#endif
}
cv::superres::FrameSource* cveSuperresCreateFrameSourceCamera(int deviceId, cv::Ptr<cv::superres::FrameSource>** sharedPtr)
{
#ifdef HAVE_OPENCV_SUPERRES
	cv::Ptr<cv::superres::FrameSource> ptr = cv::superres::createFrameSource_Camera(deviceId);
	*sharedPtr = new cv::Ptr<cv::superres::FrameSource>(ptr);
	return ptr.get();
#else
	throw_no_superres();
#endif
}
void cveSuperresFrameSourceRelease(cv::Ptr<cv::superres::FrameSource>** sharedPtr)
{
#ifdef HAVE_OPENCV_SUPERRES
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_superres();
#endif
}
void cveSuperresFrameSourceNextFrame(cv::superres::FrameSource* frameSource, cv::_OutputArray* frame)
{
#ifdef HAVE_OPENCV_SUPERRES
	frameSource->nextFrame(*frame);
#else
	throw_no_superres();
#endif
}

cv::superres::SuperResolution* cveSuperResolutionCreate(int type, cv::superres::FrameSource* frameSource, cv::superres::FrameSource** frameSourceOut, cv::Ptr<cv::superres::SuperResolution>** sharedPtr)
{
#ifdef HAVE_OPENCV_SUPERRES
	cv::Ptr<cv::superres::SuperResolution> ptr =
		(type == 1) ? cv::superres::createSuperResolution_BTVL1_CUDA() :
		//((type == 2) ? cv::superres::createSuperResolution_BTVL1_OCL() :
		cv::superres::createSuperResolution_BTVL1();

	cv::Ptr<cv::superres::FrameSource> fsPtr(frameSource, [](cv::superres::FrameSource*) {});

	ptr->setInput(fsPtr);
	//cv::Mat tmp;
	//ptr->nextFrame(tmp);
	*frameSourceOut = dynamic_cast<cv::superres::FrameSource*>(ptr.get());

	*sharedPtr = new cv::Ptr<cv::superres::SuperResolution>(ptr);
	return ptr.get();
#else
	throw_no_superres();
#endif
}
void cveSuperResolutionRelease(cv::Ptr<cv::superres::SuperResolution>** sharedPtr)
{
#ifdef HAVE_OPENCV_SUPERRES
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_superres();
#endif
}
