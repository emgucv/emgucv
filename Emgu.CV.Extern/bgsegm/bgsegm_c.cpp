//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "bgsegm_c.h"

//BackgroundSubtractorMOG
cv::bgsegm::BackgroundSubtractorMOG* cveBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::bgsegm::BackgroundSubtractorMOG> ptr = cv::bgsegm::createBackgroundSubtractorMOG(history, nmixtures, backgroundRatio, noiseSigma);
   ptr.addref();
   cv::bgsegm::BackgroundSubtractorMOG* bs = ptr.get();
   *bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
   *algorithm = dynamic_cast<cv::Algorithm*>(bs);
   return bs;
}

void cveBackgroundSubtractorMOGRelease(cv::bgsegm::BackgroundSubtractorMOG** bgSubtractor)
{
   delete *bgSubtractor;
   *bgSubtractor = 0;
}

//BackgroundSubtractorGMG
cv::bgsegm::BackgroundSubtractorGMG* cveBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::bgsegm::BackgroundSubtractorGMG> ptr = cv::bgsegm::createBackgroundSubtractorGMG(initializationFrames, decisionThreshold);
   ptr.addref();
   cv::bgsegm::BackgroundSubtractorGMG* bs = ptr.get();
   *bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
   *algorithm = dynamic_cast<cv::Algorithm*>(bs);
   return bs;
}
void cveBackgroundSubtractorGMGRelease(cv::bgsegm::BackgroundSubtractorGMG** bgSubtractor)
{
   delete *bgSubtractor;
   *bgSubtractor = 0;
}

//BackgroundSubtractorCNT
cv::bgsegm::BackgroundSubtractorCNT* cveBackgroundSubtractorCNTCreate(
	int minPixelStability,
	bool useHistory,
	int maxPixelStability,
	bool isParallel,
	cv::BackgroundSubtractor** bgSubtractor,
	cv::Algorithm** algorithm)
{
	cv::Ptr<cv::bgsegm::BackgroundSubtractorCNT> ptr = cv::bgsegm::createBackgroundSubtractorCNT(minPixelStability, useHistory, maxPixelStability, isParallel);
	ptr.addref();
	cv::bgsegm::BackgroundSubtractorCNT* bs = ptr.get();
	*bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
	*algorithm = dynamic_cast<cv::Algorithm*>(bs);
	return bs;
}
void cveBackgroundSubtractorCNTRelease(cv::bgsegm::BackgroundSubtractorCNT** bgSubtractor)
{
	delete *bgSubtractor;
	*bgSubtractor = 0;
}


//BackgroundSubtractorGSOC
cv::bgsegm::BackgroundSubtractorGSOC* cveBackgroundSubtractorGSOCCreate(
	int mc, int nSamples, float replaceRate, float propagationRate, int hitsThreshold, float alpha, float beta, float blinkingSupressionDecay, float blinkingSupressionMultiplier, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG,
	cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::bgsegm::BackgroundSubtractorGSOC> ptr = cv::bgsegm::createBackgroundSubtractorGSOC(mc, nSamples, replaceRate, propagationRate, hitsThreshold, alpha, beta, blinkingSupressionDecay, blinkingSupressionMultiplier, noiseRemovalThresholdFacBG, noiseRemovalThresholdFacFG);
	ptr.addref();
	cv::bgsegm::BackgroundSubtractorGSOC* bs = ptr.get();
	*bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
	*algorithm = dynamic_cast<cv::Algorithm*>(bs);
	return bs;
}
void cveBackgroundSubtractorGSOCRelease(cv::bgsegm::BackgroundSubtractorGSOC** bgSubtractor)
{
	delete *bgSubtractor;
	*bgSubtractor = 0;
}

//BackgroundSubtractorLSBP
cv::bgsegm::BackgroundSubtractorLSBP* cveBackgroundSubtractorLSBPCreate(
	int mc, int nSamples, int LSBPRadius, float tlower, float tupper, float tinc, float tdec, float rscale, float rincdec, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG, int LSBPthreshold, int minCount,
	cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::bgsegm::BackgroundSubtractorLSBP> ptr = cv::bgsegm::createBackgroundSubtractorLSBP(mc, nSamples, LSBPRadius, tlower, tupper, tinc, tdec, rscale, rincdec, noiseRemovalThresholdFacBG, noiseRemovalThresholdFacFG, LSBPthreshold, minCount);
	ptr.addref();
	cv::bgsegm::BackgroundSubtractorLSBP* bs = ptr.get();
	*bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
	*algorithm = dynamic_cast<cv::Algorithm*>(bs);
	return bs;
}
void cveBackgroundSubtractorLSBPRelease(cv::bgsegm::BackgroundSubtractorLSBP** bgSubtractor)
{
	delete *bgSubtractor;
	*bgSubtractor = 0;
}