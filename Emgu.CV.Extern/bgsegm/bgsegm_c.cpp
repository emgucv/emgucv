//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "bgsegm_c.h"

//BackgroundSubtractorMOG
cv::bgsegm::BackgroundSubtractorMOG* CvBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::bgsegm::BackgroundSubtractorMOG> ptr = cv::bgsegm::createBackgroundSubtractorMOG(history, nmixtures, backgroundRatio, noiseSigma);
   ptr.addref();
   cv::bgsegm::BackgroundSubtractorMOG* bs = ptr.get();
   *bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
   *algorithm = dynamic_cast<cv::Algorithm*>(bs);
   return bs;
}

void CvBackgroundSubtractorMOGRelease(cv::bgsegm::BackgroundSubtractorMOG** bgSubtractor)
{
   delete *bgSubtractor;
   *bgSubtractor = 0;
}

//BackgroundSubtractorGMG
cv::bgsegm::BackgroundSubtractorGMG* CvBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold, cv::BackgroundSubtractor** bgSubtractor, cv::Algorithm** algorithm)
{
   cv::Ptr<cv::bgsegm::BackgroundSubtractorGMG> ptr = cv::bgsegm::createBackgroundSubtractorGMG(initializationFrames, decisionThreshold);
   ptr.addref();
   cv::bgsegm::BackgroundSubtractorGMG* bs = ptr.get();
   *bgSubtractor = dynamic_cast<cv::BackgroundSubtractor*>(bs);
   *algorithm = dynamic_cast<cv::Algorithm*>(bs);
   return bs;
}
void CvBackgroundSubtractorGMGRelease(cv::bgsegm::BackgroundSubtractorGMG** bgSubtractor)
{
   delete *bgSubtractor;
   *bgSubtractor = 0;
}