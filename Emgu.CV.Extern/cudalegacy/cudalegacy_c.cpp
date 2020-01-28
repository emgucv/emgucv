//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudalegacy_c.h"


//----------------------------------------------------------------------------
//
//  Cuda BackgroundSubtractorGMG
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorGMG*) cudaBackgroundSubtractorGMGCreate(
	int initializationFrames,
	double decisionThreshold,
	cv::Ptr<cv::cuda::BackgroundSubtractorGMG>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDALEGACY
	cv::Ptr<cv::cuda::BackgroundSubtractorGMG> ptr = cv::cuda::createBackgroundSubtractorGMG(initializationFrames, decisionThreshold);
	*sharedPtr = new cv::Ptr<cv::cuda::BackgroundSubtractorGMG>(ptr);
	return ptr.get();
#else
	throw_no_cudalegacy();
#endif
}
void cudaBackgroundSubtractorGMGApply(cv::cuda::BackgroundSubtractorGMG* gmg, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream)
{
#ifdef HAVE_OPENCV_CUDALEGACY
	gmg->apply(*frame, *fgMask, learningRate, stream ? *stream : cv::cuda::Stream::Null());
#else
	throw_no_cudalegacy();
#endif	
}
void cudaBackgroundSubtractorGMGRelease(cv::Ptr<cv::cuda::BackgroundSubtractorGMG>** gmg)
{
#ifdef HAVE_OPENCV_CUDALEGACY
	delete *gmg;
	*gmg = 0;
#else
throw_no_cudalegacy();
#endif
}


//----------------------------------------------------------------------------
//
//  Cuda BackgroundSubtractorFGD
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorFGD*) cudaBackgroundSubtractorFGDCreate(
	int Lc,
	int N1c,
	int N2c,
	int Lcc,
	int N1cc,
	int N2cc,
	bool isObjWithoutHoles,
	int performMorphing,
	float alpha1,
	float alpha2,
	float alpha3,
	float delta,
	float T,
	float minArea,
	cv::Ptr<cv::cuda::BackgroundSubtractorFGD>** sharedPtr)
{
#ifdef HAVE_OPENCV_CUDALEGACY
	cv::cuda::FGDParams p;
	p.Lc = Lc;
	p.N1c = N1c;
	p.N2c = N2c;
	p.Lcc = Lcc;
	p.N1cc = N1cc;
	p.N2cc = N2cc;
	p.is_obj_without_holes = isObjWithoutHoles;
	p.perform_morphing = performMorphing;
	p.alpha1 = alpha1;
	p.alpha2 = alpha2;
	p.alpha3 = alpha3;
	p.delta = delta;
	p.T = T;
	p.minArea = minArea;
	cv::Ptr<cv::cuda::BackgroundSubtractorFGD> fgdPtr = cv::cuda::createBackgroundSubtractorFGD(p);
	*sharedPtr = new cv::Ptr<cv::cuda::BackgroundSubtractorFGD>(fgdPtr);
	return fgdPtr.get();
#else
throw_no_cudalegacy();
#endif
}

void cudaBackgroundSubtractorFGDApply(cv::cuda::BackgroundSubtractorFGD* fgd, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate)
{
#ifdef HAVE_OPENCV_CUDALEGACY
	fgd->apply(*frame, *fgMask, learningRate);
#else
throw_no_cudalegacy();
#endif
}
void cudaBackgroundSubtractorFGDRelease(cv::Ptr<cv::cuda::BackgroundSubtractorFGD>** fgd)
{
#ifdef HAVE_OPENCV_CUDALEGACY
	delete *fgd;
	*fgd = 0;
#else
throw_no_cudalegacy();
#endif
}

