//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "optflow_c.h"
#include <memory>

void cveUpdateMotionHistory(cv::_InputArray* silhouette, cv::_InputOutputArray* mhi, double timestamp, double duration)
{
   cv::motempl::updateMotionHistory(*silhouette, *mhi, timestamp, duration);
}
void cveCalcMotionGradient(cv::_InputArray* mhi, cv::_OutputArray* mask, cv::_OutputArray* orientation, double delta1, double delta2, int apertureSize)
{
   cv::motempl::calcMotionGradient(*mhi, *mask, *orientation, delta1, delta2, apertureSize);
}
void cveCalcGlobalOrientation(cv::_InputArray* orientation, cv::_InputArray* mask, cv::_InputArray* mhi, double timestamp, double duration)
{
   cv::motempl::calcGlobalOrientation(*orientation, *mask, *mhi, timestamp, duration);
}
void cveSegmentMotion(cv::_InputArray* mhi, cv::_OutputArray* segmask, std::vector< cv::Rect >* boundingRects, double timestamp, double segThresh)
{
   cv::motempl::segmentMotion(*mhi, *segmask, *boundingRects, timestamp, segThresh);
}

cv::DenseOpticalFlow* cveOptFlowDeepFlowCreate(cv::Algorithm** algorithm, cv::Ptr<cv::DenseOpticalFlow>** sharedPtr)
{
	cv::Ptr<cv::DenseOpticalFlow> ptr = cv::optflow::createOptFlow_DeepFlow();
	*sharedPtr = new cv::Ptr<cv::DenseOpticalFlow>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}


cv::DenseOpticalFlow* cveOptFlowPCAFlowCreate(cv::Algorithm** algorithm, cv::Ptr<cv::DenseOpticalFlow>** sharedPtr)
{
	cv::Ptr<cv::DenseOpticalFlow> ptr = cv::optflow::createOptFlow_PCAFlow();
	*sharedPtr = new cv::Ptr<cv::DenseOpticalFlow>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}


cv::optflow::DualTVL1OpticalFlow* cveDenseOpticalFlowCreateDualTVL1(cv::DenseOpticalFlow** denseOpticalFlow, cv::Algorithm** algorithm, cv::Ptr<cv::optflow::DualTVL1OpticalFlow>** sharedPtr)
{
	cv::Ptr<cv::DualTVL1OpticalFlow> dof = cv::createOptFlow_DualTVL1();
	*sharedPtr = new cv::Ptr<cv::DualTVL1OpticalFlow>(dof);
	cv::DualTVL1OpticalFlow* ptr = dof.get();
	*denseOpticalFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
}
void cveDualTVL1OpticalFlowRelease(cv::optflow::DualTVL1OpticalFlow** flow, cv::Ptr<cv::optflow::DualTVL1OpticalFlow>** sharedPtr)
{
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
}

