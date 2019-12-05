//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.
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
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	opticalFlow->calc(*I0, *I1, *flow, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaoptflow();
#endif
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
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	opticalFlow->calc(*prevImg, *nextImg, *prevPts, *nextPts, status ? *status : (cv::_OutputArray) cv::noArray(), err ? *err : (cv::_OutputArray) cv::noArray(), stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudaoptflow();
#endif
}

void cudaSparsePyrLKOpticalFlowRelease(cv::Ptr<cv::cuda::SparsePyrLKOpticalFlow>** flow)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	delete *flow;
	*flow = 0;
#else
	throw_no_cudaoptflow();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaBroxOpticalFlow 
//
//----------------------------------------------------------------------------

cv::cuda::BroxOpticalFlow* cudaBroxOpticalFlowCreate(
	double alpha, double gamma, double scaleFactor, 
	int innerIterations, int outerIterations, int solverIterations, 
	cv::cuda::DenseOpticalFlow** denseFlow, cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::BroxOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	cv::Ptr<cv::cuda::BroxOpticalFlow> ptr = cv::cuda::BroxOpticalFlow::create(alpha, gamma, scaleFactor, innerIterations, outerIterations, solverIterations);
	*sharedPtr = new cv::Ptr<cv::cuda::BroxOpticalFlow>(ptr);
	cv::cuda::BroxOpticalFlow* flow = ptr.get();
	*denseFlow = dynamic_cast<cv::cuda::DenseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
#else
	throw_no_cudaoptflow();
#endif
}

void cudaBroxOpticalFlowRelease(cv::Ptr<cv::cuda::BroxOpticalFlow>** flow)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	delete *flow;
	*flow = 0;
#else
	throw_no_cudaoptflow();
#endif
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
	cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::FarnebackOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	cv::Ptr<cv::cuda::FarnebackOpticalFlow> ptr = cv::cuda::FarnebackOpticalFlow::create(
		numLevels,
		pyrScale,
		fastPyramids,
		winSize,
		numIters,
		polyN,
		polySigma,
		flags);
	*sharedPtr = new cv::Ptr<cv::cuda::FarnebackOpticalFlow>(ptr);
	cv::cuda::FarnebackOpticalFlow* flow = ptr.get();
	*denseFlow = dynamic_cast<cv::cuda::DenseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
#else
	throw_no_cudaoptflow();
#endif
}


void cudaFarnebackOpticalFlowRelease(cv::Ptr<cv::cuda::FarnebackOpticalFlow>** flow)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	delete *flow;
	*flow = 0;
#else
	throw_no_cudaoptflow();
#endif
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
	cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::OpticalFlowDual_TVL1>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	cv::Ptr<cv::cuda::OpticalFlowDual_TVL1> ptr = cv::cuda::OpticalFlowDual_TVL1::create(tau, lambda, theta, nscales, warps, epsilon, iterations, scaleStep, gamma, useInitialFlow);
	*sharedPtr = new cv::Ptr<cv::cuda::OpticalFlowDual_TVL1>(ptr);
	cv::cuda::OpticalFlowDual_TVL1* flow = ptr.get();
	*denseFlow = dynamic_cast<cv::cuda::DenseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
#else
	throw_no_cudaoptflow();
#endif
}


void cudaOpticalFlowDualTvl1Release(cv::Ptr<cv::cuda::OpticalFlowDual_TVL1>** flow)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	delete *flow;
	*flow = 0;
#else
	throw_no_cudaoptflow();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaDensePyrLKOpticalFlow
//
//----------------------------------------------------------------------------
cv::cuda::DensePyrLKOpticalFlow* cudaDensePyrLKOpticalFlowCreate(
	CvSize* winSize, 
	int maxLevel, 
	int iters, 
	bool useInitialFlow, 
	cv::cuda::DenseOpticalFlow** denseFlow, 
	cv::Algorithm** algorithm, 
	cv::Ptr<cv::cuda::DensePyrLKOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	cv::Ptr<cv::cuda::DensePyrLKOpticalFlow> ptr = cv::cuda::DensePyrLKOpticalFlow::create(*winSize, maxLevel, iters, useInitialFlow);
	*sharedPtr = new cv::Ptr<cv::cuda::DensePyrLKOpticalFlow>(ptr);
	cv::cuda::DensePyrLKOpticalFlow* flow = ptr.get();
	*denseFlow = dynamic_cast<cv::cuda::DenseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
#else
	throw_no_cudaoptflow();
#endif
}
void cudaDensePyrLKOpticalFlowRelease(cv::Ptr<cv::cuda::DensePyrLKOpticalFlow>** flow)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	delete *flow;
	*flow = 0;
#else
	throw_no_cudaoptflow();
#endif
}

//----------------------------------------------------------------------------
//
//  CudaSparsePyrLKOpticalFlow
//
//----------------------------------------------------------------------------
cv::cuda::SparsePyrLKOpticalFlow* cudaSparsePyrLKOpticalFlowCreate(
	CvSize* winSize, 
	int maxLevel, 
	int iters, 
	bool useInitialFlow, 
	cv::cuda::SparseOpticalFlow** sparseFlow, 
	cv::Algorithm** algorithm,
	cv::Ptr<cv::cuda::SparsePyrLKOpticalFlow>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	cv::Ptr<cv::cuda::SparsePyrLKOpticalFlow> ptr = cv::cuda::SparsePyrLKOpticalFlow::create(*winSize, maxLevel, iters, useInitialFlow);
	*sharedPtr = new cv::Ptr<cv::cuda::SparsePyrLKOpticalFlow>(ptr);
	cv::cuda::SparsePyrLKOpticalFlow* flow = ptr.get();
	*sparseFlow = dynamic_cast<cv::cuda::SparseOpticalFlow*>(flow);
	*algorithm = dynamic_cast<cv::Algorithm*>(flow);
	return flow;
#else
	throw_no_cudaoptflow();
#endif
}

void cudaDensePyrLKOpticalFlowRelease(cv::Ptr<cv::cuda::SparsePyrLKOpticalFlow>** flow)
{
#ifdef HAVE_OPENCV_CUDAOPTFLOW
	delete *flow;
	*flow = 0;
#else
	throw_no_cudaoptflow();
#endif
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