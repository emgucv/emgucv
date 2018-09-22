//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "superres_c.h"

cv::superres::FrameSource* cveSuperresCreateFrameSourceVideo(cv::String* fileName, bool useGpu, cv::Ptr<cv::superres::FrameSource>** sharedPtr)
{
	cv::Ptr<cv::superres::FrameSource> ptr = useGpu ?
		cv::superres::createFrameSource_Video_CUDA(*fileName)
		: cv::superres::createFrameSource_Video(*fileName);
	*sharedPtr = new cv::Ptr<cv::superres::FrameSource>(ptr);
	return ptr.get();
}
cv::superres::FrameSource* cveSuperresCreateFrameSourceCamera(int deviceId, cv::Ptr<cv::superres::FrameSource>** sharedPtr)
{
	cv::Ptr<cv::superres::FrameSource> ptr = cv::superres::createFrameSource_Camera(deviceId);
	*sharedPtr = new cv::Ptr<cv::superres::FrameSource>(ptr);
	return ptr.get();
}
void cveSuperresFrameSourceRelease(cv::superres::FrameSource** frameSource, cv::Ptr<cv::superres::FrameSource>** sharedPtr)
{
	delete *sharedPtr;
	*frameSource = 0;
	*sharedPtr = 0;
}
void cveSuperresFrameSourceNextFrame(cv::superres::FrameSource* frameSource, cv::_OutputArray* frame)
{
	frameSource->nextFrame(*frame);
}

cv::superres::SuperResolution* cveSuperResolutionCreate(int type, cv::superres::FrameSource* frameSource, cv::superres::FrameSource** frameSourceOut, cv::Ptr<cv::superres::SuperResolution>** sharedPtr)
{
	cv::Ptr<cv::superres::SuperResolution> ptr =
		(type == 1) ? cv::superres::createSuperResolution_BTVL1_CUDA() :
		//((type == 2) ? cv::superres::createSuperResolution_BTVL1_OCL() :
		cv::superres::createSuperResolution_BTVL1();

	cv::Ptr<cv::superres::FrameSource> fsPtr(frameSource, [](cv::superres::FrameSource*) {});

	ptr->setInput(fsPtr);
	cv::Mat tmp;
	ptr->nextFrame(tmp);
	*frameSourceOut = dynamic_cast<cv::superres::FrameSource*>(ptr.get());

	*sharedPtr = new cv::Ptr<cv::superres::SuperResolution>(ptr);
	return ptr.get();
}
void cveSuperResolutionRelease(cv::superres::SuperResolution** superres, cv::Ptr<cv::superres::SuperResolution>** sharedPtr)
{
	delete *sharedPtr;
	*superres = 0;
	*sharedPtr = 0;
}
