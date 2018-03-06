//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "cudabgsegm_c.h"

//----------------------------------------------------------------------------
//
//  Cuda BackgroundSubtractorMOG
//
//----------------------------------------------------------------------------
cv::cuda::BackgroundSubtractorMOG* cudaBackgroundSubtractorMOGCreate(int history, int nMixtures, double backgroundRatio, double noiseSigma, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::cuda::BackgroundSubtractorMOG> ptr = cv::cuda::createBackgroundSubtractorMOG(history, nMixtures, backgroundRatio, noiseSigma);
   ptr.addref();
   cv::cuda::BackgroundSubtractorMOG* bs = ptr.get();
   *bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
   *algorithm = dynamic_cast<cv::Algorithm*>(bs);
   return bs;
}
void cudaBackgroundSubtractorMOGApply(cv::cuda::BackgroundSubtractorMOG* mog, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream)
{
   mog->apply(*frame, *fgMask, learningRate, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaBackgroundSubtractorMOGRelease(cv::cuda::BackgroundSubtractorMOG** mog)
{
   delete (*mog);
   *mog = 0;
}

//----------------------------------------------------------------------------
//
//  Cuda BackgroundSubtractorMOG2 
//
//----------------------------------------------------------------------------
cv::cuda::BackgroundSubtractorMOG2* cudaBackgroundSubtractorMOG2Create(int history, double varThreshold, bool detectShadows, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::cuda::BackgroundSubtractorMOG2> ptr = cv::cuda::createBackgroundSubtractorMOG2(history, varThreshold, detectShadows);
   ptr.addref();
   cv::cuda::BackgroundSubtractorMOG2* bs = ptr.get();
   *bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
   *algorithm = dynamic_cast<cv::Algorithm*>(bs);
   return bs;
}

void cudaBackgroundSubtractorMOG2Apply(cv::cuda::BackgroundSubtractorMOG2* mog, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream)
{
   mog->apply(*frame, *fgMask, learningRate, stream ? *stream : cv::cuda::Stream::Null());
}

void cudaBackgroundSubtractorMOG2Release(cv::cuda::BackgroundSubtractorMOG2** mog)
{
   delete (*mog);
   *mog = 0;
}

/*
//----------------------------------------------------------------------------
//
//  Cuda BackgroundSubtractorGMG
//
//----------------------------------------------------------------------------
CVAPI(cv::cuda::BackgroundSubtractorGMG*) cudaBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold)
{
   cv::Ptr<cv::cuda::BackgroundSubtractorGMG> ptr = cv::cuda::createBackgroundSubtractorGMG(initializationFrames, decisionThreshold);
   ptr.addref();
   return ptr.get();
}
void cudaBackgroundSubtractorGMGApply(cv::cuda::BackgroundSubtractorGMG* gmg, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate, cv::cuda::Stream* stream)
{
   gmg->apply(*frame, *fgMask, learningRate, stream ? *stream : cv::cuda::Stream::Null());
}
void cudaBackgroundSubtractorGMGRelease(cv::cuda::BackgroundSubtractorGMG** gmg)
{
   delete *gmg;
   *gmg = 0;
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
   float minArea)
{
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
   fgdPtr.addref();
   return fgdPtr.get();
}

void cudaBackgroundSubtractorFGDApply(cv::cuda::BackgroundSubtractorFGD* fgd, cv::_InputArray* frame, cv::_OutputArray* fgMask, double learningRate)
{
   fgd->apply(*frame, *fgMask, learningRate);
}
void cudaBackgroundSubtractorFGDRelease(cv::cuda::BackgroundSubtractorFGD** fgd)
{
   delete *fgd;
   *fgd=0;
}*/