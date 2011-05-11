//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_LEGACY_C_H
#define EMGU_LEGACY_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/legacy/blobtrack.hpp"

//Forground detector
CVAPI(CvFGDetector*) CvCreateFGDetectorBase(int type, void* param);
CVAPI(IplImage*) CvFGDetectorGetMask(CvFGDetector* detector);
CVAPI(void) CvFGDetectorProcess(CvFGDetector* detector, IplImage* image);
CVAPI(void) CvFGDetectorRelease(CvFGDetector* detector);

//Blob
CVAPI(CvBlobSeq*) CvBlobSeqCreate(int BlobSize);
CVAPI(void) CvBlobSeqRelease(CvBlobSeq** blobSeq);
CVAPI(CvBlob*) CvBlobSeqGetBlobByID(CvBlobSeq* blobSeq, int blobID);
CVAPI(CvBlob*) CvBlobSeqGetBlob(CvBlobSeq* blobSeq, int blobIndex);
CVAPI(int) CvBlobSeqGetBlobNum(CvBlobSeq* blobSeq);
CVAPI(void) CvBlobSeqClear(CvBlobSeq* blobSeq);

//Blob Detector
CVAPI(void) CvBlobDetectorRelease(CvBlobDetector** detector);
CVAPI(int) CvBlobDetectorDetectNewBlob(CvBlobDetector* detector, IplImage* pImg, IplImage* pImgFG, CvBlobSeq* pNewBlobList, CvBlobSeq* pOldBlobList);
CVAPI(CvBlobDetector*) CvCreateBlobDetectorSimple();
CVAPI(CvBlobDetector*) CvCreateBlobDetectorCC();

//blob Tracker
/* Simple blob tracker based on connected component tracking: */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerCC();
/* Connected component tracking and mean-shift particle filter collion-resolver: */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerCCMSPF();
/* Blob tracker that integrates meanshift and connected components: */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerMSFG();
CVAPI( CvBlobTracker*) CvCreateBlobTrackerMSFGS();
/* Meanshift without connected-components */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerMS();
/* Particle filtering via Bhattacharya coefficient, which        */
/* is roughly the dot-product of two probability densities.      */
/* See: Real-Time Tracking of Non-Rigid Objects using Mean Shift */
/*      Comanicius, Ramesh, Meer, 2000, 8p                       */
/*      http://citeseer.ist.psu.edu/321441.html                  */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerMSPF();
CVAPI(void) CvBlobTrackerRealease(CvBlobTracker** tracker);
CVAPI(int) CvBlobTrackerGetBlobNum(CvBlobTracker* tracker);
CVAPI(CvBlob*) CvBlobTrackerGetBlob(CvBlobTracker* tracker, int BlobIndex);
CVAPI(CvBlob*) CvBlobTrackerGetBlobByID(CvBlobTracker* tracker, int BlobId);
CVAPI(void) CvBlobTrackerDelBlob(CvBlobTracker* tracker, int BlobIndex);
CVAPI(CvBlob*) CvBlobTrackerAddBlob(CvBlobTracker* tracker, CvBlob* pBlob, IplImage* pImg, IplImage* pImgFG);

//blob tracker auto
CVAPI(CvBlobTrackerAuto*) CvCreateBlobTrackerAuto1(CvBlobTrackerAutoParam1* param);
CVAPI(void) CvBlobTrackerAutoRelease(CvBlobTrackerAuto** tracker);
CVAPI(CvBlob*) CvBlobTrackerAutoGetBlob(CvBlobTrackerAuto* tracker, int index);
CVAPI(CvBlob*) CvBlobTrackerAutoGetBlobByID(CvBlobTrackerAuto* tracker, int blobID);
CVAPI(int) CvBlobTrackerAutoGetBlobNum(CvBlobTrackerAuto* tracker);
CVAPI(void) CvBlobTrackerAutoProcess(CvBlobTrackerAuto* tracker, IplImage* pImg, IplImage* pMask);
CVAPI(IplImage*) CvBlobTrackerAutoGetFGMask(CvBlobTrackerAuto* tracker);

//blob tracker post process
CVAPI(CvBlobTrackPostProc*) CvCreateModuleBlobTrackPostProcKalman();
CVAPI(CvBlobTrackPostProc*) CvCreateModuleBlobTrackPostProcTimeAverRect();
CVAPI(CvBlobTrackPostProc*) CvCreateModuleBlobTrackPostProcTimeAverExp();
CVAPI(void) CvBlobTrackPostProcRelease(CvBlobTrackPostProc** postProc);

#endif