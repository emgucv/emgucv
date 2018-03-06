//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudaoptflow_c.h"


//----------------------------------------------------------------------------
//
//  DenseOpticalFlow 
//
//----------------------------------------------------------------------------

void cudaDenseOpticalFlowCalc(
	cv::cuda::DenseOpticalFlow* opticalFlow,
	cv::_InputArray* I0,
	cv::_InputArray* I1,
	cv::_InputOutputArray* flow,
	cv::cuda::Stream* stream)
{
	opticalFlow->calc(*I0, *I1, *flow, stream ? *stream : cv::cuda::Stream::Null());
}

//----------------------------------------------------------------------------
//
//  SparseOpticalFlow 
//
//----------------------------------------------------------------------------

void cudaSparseOpticalFlowCalc(
	cv::cuda::SparseOpticalFlow* opticalFlow,
	cv::_InputArray* prevImg, cv::_InputArray* nextImg,
	cv::_InputArray* prevPts, cv::_InputOutputArray* nextPts,
	cv::_OutputArray* status,
	cv::_OutputArray* err,
	cv::cuda::Stream* stream)
{
	opticalFlow->calc(*prevImg, *nextImg, *prevPts, *nextPts, status ? *status : (cv::_OutputArray) cv::noArray(), err ? *err : (cv::_OutputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
}

//----------------------------------------------------------------------------
//
//  CudaBroxOpticalFlow 
//
//----------------------------------------------------------------------------

cv::cuda::BroxOpticalFlow* cudaBroxOpticalFlowCreate(double alpha, double gamma, double scaleFactor, int innerIterations, int outerIterations, int solverIterations, cv::cuda::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::cuda::BroxOpticalFlow> ptr = cv::cuda::BroxOpticalFlow::create(alpha, gamma, scaleFactor, innerIterations, outerIterations, solverIterations);
	ptr.addref();
	cv::cuda::BroxOpticalFlow* flow = ptr.get();
	*denseFlow = dynamic_cast<cv::cuda::DenseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
}

void cudaBroxOpticalFlowRelease(cv::cuda::BroxOpticalFlow** flow)
{
	delete *flow;
	*flow = 0;
}

//----------------------------------------------------------------------------
//
//  CudaFarnebackOpticalFlow
//
//----------------------------------------------------------------------------
cv::cuda::FarnebackOpticalFlow* cudaFarnebackOpticalFlowCreate(
	int numLevels,
	double pyrScale,
	bool fastPyramids,
	int winSize,
	int numIters,
	int polyN,
	double polySigma,
	int flags,
	cv::cuda::DenseOpticalFlow** denseFlow,
	cv::Algorithm** algorithm)
{
	cv::Ptr<cv::cuda::FarnebackOpticalFlow> ptr = cv::cuda::FarnebackOpticalFlow::create(
		numLevels,
		pyrScale,
		fastPyramids,
		winSize,
		numIters,
		polyN,
		polySigma,
		flags);
	ptr.addref();
	cv::cuda::FarnebackOpticalFlow* flow = ptr.get();
	*denseFlow = dynamic_cast<cv::cuda::DenseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
}


void cudaFarnebackOpticalFlowRelease(cv::cuda::FarnebackOpticalFlow** flow)
{
	delete *flow;
	*flow = 0;
}

//----------------------------------------------------------------------------
//
//  CudaOpticalFlowDualTvl1
//
//----------------------------------------------------------------------------
cv::cuda::OpticalFlowDual_TVL1* cudaOpticalFlowDualTvl1Create(
	double tau, double lambda, double theta, int nscales, int warps,
	double epsilon, int iterations, double scaleStep, double gamma, bool useInitialFlow,
	cv::cuda::DenseOpticalFlow** denseFlow,
	cv::Algorithm** algorithm)
{
	cv::Ptr<cv::cuda::OpticalFlowDual_TVL1> ptr = cv::cuda::OpticalFlowDual_TVL1::create(tau, lambda, theta, nscales, warps, epsilon, iterations, scaleStep, gamma, useInitialFlow);
	ptr.addref();
	cv::cuda::OpticalFlowDual_TVL1* flow = ptr.get();
	*denseFlow = dynamic_cast<cv::cuda::DenseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
}


void cudaOpticalFlowDualTvl1Release(cv::cuda::OpticalFlowDual_TVL1** flow)
{
	delete *flow;
	*flow = 0;
}

//----------------------------------------------------------------------------
//
//  CudaDensePyrLKOpticalFlow
//
//----------------------------------------------------------------------------
cv::cuda::DensePyrLKOpticalFlow* cudaDensePyrLKOpticalFlowCreate(CvSize* winSize, int maxLevel, int iters, bool useInitialFlow, cv::cuda::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::cuda::DensePyrLKOpticalFlow> ptr = cv::cuda::DensePyrLKOpticalFlow::create(*winSize, maxLevel, iters, useInitialFlow);
	ptr.addref();
	cv::cuda::DensePyrLKOpticalFlow* flow = ptr.get();
	*denseFlow = dynamic_cast<cv::cuda::DenseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
}
void cudaDensePyrLKOpticalFlowRelease(cv::cuda::DensePyrLKOpticalFlow** flow)
{
	delete *flow;
}

//----------------------------------------------------------------------------
//
//  CudaSparsePyrLKOpticalFlow
//
//----------------------------------------------------------------------------
cv::cuda::SparsePyrLKOpticalFlow* cudaSparsePyrLKOpticalFlowCreate(CvSize* winSize, int maxLevel, int iters, bool useInitialFlow, cv::cuda::SparseOpticalFlow** sparseFlow, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::cuda::SparsePyrLKOpticalFlow> ptr = cv::cuda::SparsePyrLKOpticalFlow::create(*winSize, maxLevel, iters, useInitialFlow);
	ptr.addref();
	cv::cuda::SparsePyrLKOpticalFlow* flow = ptr.get();
	*sparseFlow = dynamic_cast<cv::cuda::SparseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
}
void cudaDensePyrLKOpticalFlowRelease(cv::cuda::SparsePyrLKOpticalFlow** flow)
{
	delete *flow;
}

/*
//----------------------------------------------------------------------------
//
//  Utilities
//
//----------------------------------------------------------------------------
void cudaCreateOpticalFlowNeedleMap(const cv::cuda::GpuMat* u, const cv::cuda::GpuMat* v, cv::cuda::GpuMat* vertex, cv::cuda::GpuMat* colors)
{
   cv::cuda:: ::createOpticalFlowNeedleMap(*u, *v, *vertex, *colors);
}*/