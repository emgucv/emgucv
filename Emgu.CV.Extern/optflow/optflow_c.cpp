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

cv::DenseOpticalFlow* cveOptFlowDeepFlowCreate(cv::Algorithm** algorithm)
{
	cv::Ptr<cv::DenseOpticalFlow> ptr = cv::optflow::createOptFlow_DeepFlow();
	ptr.addref();
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}

cv::optflow::DISOpticalFlow* cveDISOpticalFlowCreate(int preset, cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::optflow::DISOpticalFlow> ptr = cv::optflow::createOptFlow_DIS(preset);
	ptr.addref();
	*denseFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}

void cveDISOpticalFlowRelease(cv::optflow::DISOpticalFlow** flow)
{
	delete *flow;
	*flow = 0;
}

cv::DenseOpticalFlow* cveOptFlowPCAFlowCreate(cv::Algorithm** algorithm)
{
	cv::Ptr<cv::DenseOpticalFlow> ptr = cv::optflow::createOptFlow_PCAFlow();
	ptr.addref();
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}


cv::optflow::VariationalRefinement* cveVariationalRefinementCreate(cv::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::optflow::VariationalRefinement> ptr = cv::optflow::createVariationalFlowRefinement();
	ptr.addref();
	*denseFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr.get());
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
}
void cveVariationalRefinementRelease(cv::optflow::VariationalRefinement** flow)
{
	delete *flow;
	*flow = 0;
}