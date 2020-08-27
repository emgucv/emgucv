//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_VIDEO_C_H
#define EMGU_VIDEO_C_H

#include "opencv2/core/core_c.h"

#ifdef HAVE_OPENCV_OPTFLOW
#include "opencv2/optflow.hpp"
#else
static inline CV_NORETURN void throw_no_optflow() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without optflow."); }
namespace cv {
class DenseOpticalFlow{};
class SparseOpticalFlow{};
namespace optflow {
class DualTVL1OpticalFlow {};
class RLOFOpticalFlowParameter {};
class DenseRLOFOpticalFlow {};
class SparseRLOFOpticalFlow {};
enum SolverType {};
enum SupportRegionType {};
}
}
#endif

CVAPI(void) cveUpdateMotionHistory(cv::_InputArray* silhouette, cv::_InputOutputArray* mhi, double timestamp, double duration);
CVAPI(void) cveCalcMotionGradient(cv::_InputArray* mhi, cv::_OutputArray* mask, cv::_OutputArray* orientation, double delta1, double delta2, int apertureSize);
CVAPI(void) cveCalcGlobalOrientation(cv::_InputArray* orientation, cv::_InputArray* mask, cv::_InputArray* mhi, double timestamp, double duration);
CVAPI(void) cveSegmentMotion(cv::_InputArray* mhi, cv::_OutputArray* segmask, std::vector< cv::Rect >* boundingRects, double timestamp, double segThresh);

CVAPI(cv::DenseOpticalFlow*) cveOptFlowDeepFlowCreate(cv::Algorithm** algorithm, cv::Ptr<cv::DenseOpticalFlow>** sharedPtr);
CVAPI(cv::DenseOpticalFlow*) cveOptFlowPCAFlowCreate(cv::Algorithm** algorithm, cv::Ptr<cv::DenseOpticalFlow>** sharedPtr);

CVAPI(cv::optflow::DualTVL1OpticalFlow*) cveDenseOpticalFlowCreateDualTVL1(cv::DenseOpticalFlow** denseOpticalFlow, cv::Algorithm** algorithm, cv::Ptr<cv::optflow::DualTVL1OpticalFlow>** sharedPtr);
CVAPI(void) cveDualTVL1OpticalFlowRelease(cv::Ptr<cv::optflow::DualTVL1OpticalFlow>** sharedPtr);


CVAPI(cv::optflow::RLOFOpticalFlowParameter*) cveRLOFOpticalFlowParameterCreate();
CVAPI(void) cveRLOFOpticalFlowParameterRelease(cv::optflow::RLOFOpticalFlowParameter** p);

CVAPI(cv::optflow::DenseRLOFOpticalFlow*) cveDenseRLOFOpticalFlowCreate(
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
	cv::Ptr<cv::optflow::DenseRLOFOpticalFlow>** sharedPtr);
CVAPI(void) cveDenseRLOFOpticalFlowRelease(cv::Ptr<cv::optflow::DenseRLOFOpticalFlow>** sharedPtr);


CVAPI(cv::optflow::SparseRLOFOpticalFlow*) cveSparseRLOFOpticalFlowCreate(
	cv::optflow::RLOFOpticalFlowParameter* rlofParameter,
	float forwardBackwardThreshold,
	cv::SparseOpticalFlow** sparseOpticalFlow,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::optflow::SparseRLOFOpticalFlow>** sharedPtr);

CVAPI(void) cveSparseRLOFOpticalFlowRelease(cv::Ptr<cv::optflow::SparseRLOFOpticalFlow>** sharedPtr);
#endif