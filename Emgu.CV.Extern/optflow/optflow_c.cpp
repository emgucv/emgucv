//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "optflow_c.h"
#include <memory>

void cveUpdateMotionHistory(cv::_InputArray* silhouette, cv::_InputOutputArray* mhi, double timestamp, double duration)
{
#ifdef HAVE_OPENCV_OPTFLOW
   cv::motempl::updateMotionHistory(*silhouette, *mhi, timestamp, duration);
#else
	throw_no_optflow();
#endif
}
void cveCalcMotionGradient(cv::_InputArray* mhi, cv::_OutputArray* mask, cv::_OutputArray* orientation, double delta1, double delta2, int apertureSize)
{
#ifdef HAVE_OPENCV_OPTFLOW
   cv::motempl::calcMotionGradient(*mhi, *mask, *orientation, delta1, delta2, apertureSize);
#else
	throw_no_optflow();
#endif
}
void cveCalcGlobalOrientation(cv::_InputArray* orientation, cv::_InputArray* mask, cv::_InputArray* mhi, double timestamp, double duration)
{
#ifdef HAVE_OPENCV_OPTFLOW
   cv::motempl::calcGlobalOrientation(*orientation, *mask, *mhi, timestamp, duration);
#else
	throw_no_optflow();
#endif
}
void cveSegmentMotion(cv::_InputArray* mhi, cv::_OutputArray* segmask, std::vector< cv::Rect >* boundingRects, double timestamp, double segThresh)
{
#ifdef HAVE_OPENCV_OPTFLOW
   cv::motempl::segmentMotion(*mhi, *segmask, *boundingRects, timestamp, segThresh);
#else
	throw_no_optflow();
#endif
}

cv::DenseOpticalFlow* cveOptFlowDeepFlowCreate(cv::Algorithm** algorithm, cv::Ptr<cv::DenseOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_OPTFLOW
	cv::Ptr<cv::DenseOpticalFlow> ptr = cv::optflow::createOptFlow_DeepFlow();
	*sharedPtr = new cv::Ptr<cv::DenseOpticalFlow>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
#else
	throw_no_optflow();
#endif
}


cv::DenseOpticalFlow* cveOptFlowPCAFlowCreate(cv::Algorithm** algorithm, cv::Ptr<cv::DenseOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_OPTFLOW
	cv::Ptr<cv::DenseOpticalFlow> ptr = cv::optflow::createOptFlow_PCAFlow();
	*sharedPtr = new cv::Ptr<cv::DenseOpticalFlow>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr.get());
	return ptr.get();
#else
	throw_no_optflow();
#endif
}


cv::optflow::DualTVL1OpticalFlow* cveDenseOpticalFlowCreateDualTVL1(cv::DenseOpticalFlow** denseOpticalFlow, cv::Algorithm** algorithm, cv::Ptr<cv::optflow::DualTVL1OpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_OPTFLOW
	cv::Ptr<cv::optflow::DualTVL1OpticalFlow> dof = cv::optflow::createOptFlow_DualTVL1();
	*sharedPtr = new cv::Ptr<cv::optflow::DualTVL1OpticalFlow>(dof);
	cv::optflow::DualTVL1OpticalFlow* ptr = dof.get();
	*denseOpticalFlow = dynamic_cast<cv::DenseOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_optflow();
#endif
}
void cveDualTVL1OpticalFlowRelease(cv::Ptr<cv::optflow::DualTVL1OpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_OPTFLOW
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_optflow();
#endif
}

cv::optflow::RLOFOpticalFlowParameter* cveRLOFOpticalFlowParameterCreate()
{
#ifdef HAVE_OPENCV_OPTFLOW
	return new cv::optflow::RLOFOpticalFlowParameter();
#else
	throw_no_optflow();
#endif
}
void cveRLOFOpticalFlowParameterRelease(cv::optflow::RLOFOpticalFlowParameter** p)
{
#ifdef HAVE_OPENCV_OPTFLOW
	delete *p;
	*p = 0;
#else
	throw_no_optflow();
#endif
}

cv::optflow::DenseRLOFOpticalFlow* cveDenseRLOFOpticalFlowCreate(
	cv::optflow::RLOFOpticalFlowParameter* rlofParameter,
	float forwardBackwardThreshold,
	CvSize* gridStep,
	int interpType,
	int epicK,
	float epicSigma,
	float epicLambda,
	bool usePostProc,
	float fgsLambda,
	float fgsSigma,
	cv::DenseOpticalFlow** denseOpticalFlow,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::optflow::DenseRLOFOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_OPTFLOW
	cv::Ptr<cv::optflow::RLOFOpticalFlowParameter> pPtr(rlofParameter, [](cv::optflow::RLOFOpticalFlowParameter* p) {});
	cv::Ptr<cv::optflow::DenseRLOFOpticalFlow> rlof = cv::optflow::DenseRLOFOpticalFlow::create(
		pPtr,
		forwardBackwardThreshold, 
		*gridStep,
	 	static_cast<cv::optflow::InterpolationType>(interpType),
		epicK,
		epicSigma, 
		epicLambda, 
		usePostProc, 
		fgsLambda,
		fgsSigma);
	*sharedPtr = new cv::Ptr<cv::optflow::DenseRLOFOpticalFlow>(rlof);
	cv::optflow::DenseRLOFOpticalFlow* ptr = (*sharedPtr)->get();
	*denseOpticalFlow = dynamic_cast<cv::optflow::DenseRLOFOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_optflow();
#endif
}
void cveDenseRLOFOpticalFlowRelease(cv::Ptr<cv::optflow::DenseRLOFOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_OPTFLOW
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_optflow();
#endif
}


cv::optflow::SparseRLOFOpticalFlow* cveSparseRLOFOpticalFlowCreate(
	cv::optflow::RLOFOpticalFlowParameter* rlofParameter,
	float forwardBackwardThreshold,
	cv::SparseOpticalFlow** sparseOpticalFlow,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::optflow::SparseRLOFOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_OPTFLOW
	cv::Ptr<cv::optflow::RLOFOpticalFlowParameter> pPtr(rlofParameter, [](cv::optflow::RLOFOpticalFlowParameter* p) {});
	cv::Ptr<cv::optflow::SparseRLOFOpticalFlow> rlof = cv::optflow::SparseRLOFOpticalFlow::create(
		pPtr,
		forwardBackwardThreshold);
	*sharedPtr = new cv::Ptr<cv::optflow::SparseRLOFOpticalFlow>(rlof);
	cv::optflow::SparseRLOFOpticalFlow* ptr = (*sharedPtr)->get();
	*sparseOpticalFlow = dynamic_cast<cv::optflow::SparseRLOFOpticalFlow*>(ptr);
	*algorithm = dynamic_cast<cv::Algorithm*>(ptr);
	return ptr;
#else
	throw_no_optflow();
#endif
}

void cveSparseRLOFOpticalFlowRelease(cv::Ptr<cv::optflow::SparseRLOFOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_OPTFLOW
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_optflow();
#endif
}