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

cv::optflow::DISOpticalFlow* cveDISOpticalFlowCreate(int preset, cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm, cv::Ptr<cv::optflow::DISOpticalFlow>** sharedPtr)
{
	cv::Ptr<cv::optflow::DISOpticalFlow> ptr = cv::optflow::createOptFlow_DIS(preset);
	*sharedPtr = new cv::Ptr<cv::optflow::DISOpticalFlow>(ptr);
	*denseFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}

void cveDISOpticalFlowRelease(cv::optflow::DISOpticalFlow** flow, cv::Ptr<cv::optflow::DISOpticalFlow>** sharedPtr)
{
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
}

cv::DenseOpticalFlow* cveOptFlowPCAFlowCreate(cv::Algorithm** algorithm, cv::Ptr<cv::DenseOpticalFlow>** sharedPtr)
{
	cv::Ptr<cv::DenseOpticalFlow> ptr = cv::optflow::createOptFlow_PCAFlow();
	*sharedPtr = new cv::Ptr<cv::DenseOpticalFlow>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}

cv::optflow::VariationalRefinement* cveVariationalRefinementCreate(cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm, cv::Ptr<cv::optflow::VariationalRefinement>** sharedPtr)
{
	cv::Ptr<cv::optflow::VariationalRefinement> ptr = cv::optflow::createVariationalFlowRefinement();
	*sharedPtr = new cv::Ptr<cv::optflow::VariationalRefinement>(ptr);
	*denseFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}
void cveVariationalRefinementRelease(cv::optflow::VariationalRefinement** flow, cv::Ptr<cv::optflow::VariationalRefinement>** sharedPtr)
{
	delete *sharedPtr;
	*flow = 0;
	*sharedPtr = 0;
}