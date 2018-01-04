//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "legacy_c.h"

//Blob
CvBlobSeq* CvBlobSeqCreate(int BlobSize) { return new CvBlobSeq(BlobSize); }
void CvBlobSeqRelease(CvBlobSeq** blobSeq) { delete *blobSeq; }
CvBlob* CvBlobSeqGetBlobByID(CvBlobSeq* blobSeq, int blobID) { return blobSeq->GetBlobByID(blobID); }
CvBlob* CvBlobSeqGetBlob(CvBlobSeq* blobSeq, int blobIndex) { return blobSeq->GetBlob(blobIndex); }
int CvBlobSeqGetBlobNum(CvBlobSeq* blobSeq) { return blobSeq->GetBlobNum(); }
void CvBlobSeqAddBlob(CvBlobSeq* blobSeq, CvBlob* blob) { blobSeq->AddBlob(blob); }
void CvBlobSeqClear(CvBlobSeq* blobSeq) { blobSeq->Clear(); }

//Blob Detector
void CvBlobDetectorRelease(CvBlobDetector** detector) { delete *detector; }
int CvBlobDetectorDetectNewBlob(CvBlobDetector* detector, IplImage* pImg, IplImage* pImgFG, CvBlobSeq* pNewBlobList, CvBlobSeq* pOldBlobList)
   { return detector->DetectNewBlob(pImg, pImgFG, pNewBlobList, pOldBlobList); }
CvBlobDetector* CvCreateBlobDetectorSimple() { return cvCreateBlobDetectorSimple(); }
CvBlobDetector* CvCreateBlobDetectorCC() { return cvCreateBlobDetectorCC(); }

//blob Tracker
/* Simple blob tracker based on connected component tracking: */
 CvBlobTracker* CvCreateBlobTrackerCC() { return  cvCreateBlobTrackerCC(); }
/* Connected component tracking and mean-shift particle filter collion-resolver: */
 CvBlobTracker* CvCreateBlobTrackerCCMSPF() { return  cvCreateBlobTrackerCCMSPF(); }
/* Blob tracker that integrates meanshift and connected components: */
 CvBlobTracker* CvCreateBlobTrackerMSFG() { return  cvCreateBlobTrackerMSFG(); }
 CvBlobTracker* CvCreateBlobTrackerMSFGS() { return  cvCreateBlobTrackerMSFGS(); }
/* Meanshift without connected-components */
 CvBlobTracker* CvCreateBlobTrackerMS() { return  cvCreateBlobTrackerMS(); }
/* Particle filtering via Bhattacharya coefficient, which        */
/* is roughly the dot-product of two probability densities.      */
/* See: Real-Time Tracking of Non-Rigid Objects using Mean Shift */
/*      Comanicius, Ramesh, Meer, 2000, 8p                       */
/*      http://citeseer.ist.psu.edu/321441.html                  */
 CvBlobTracker* CvCreateBlobTrackerMSPF() { return  cvCreateBlobTrackerMSPF(); }
void CvBlobTrackerRealease(CvBlobTracker** tracker) { delete *tracker; }
int CvBlobTrackerGetBlobNum(CvBlobTracker* tracker) { return tracker->GetBlobNum(); }
CvBlob* CvBlobTrackerGetBlob(CvBlobTracker* tracker, int BlobIndex) { return tracker->GetBlob(BlobIndex); }
CvBlob* CvBlobTrackerGetBlobByID(CvBlobTracker* tracker, int BlobId) { return tracker->GetBlobByID(BlobId); }
void CvBlobTrackerDelBlob(CvBlobTracker* tracker, int BlobIndex) { tracker->DelBlob(BlobIndex); }
CvBlob* CvBlobTrackerAddBlob(CvBlobTracker* tracker, CvBlob* pBlob, IplImage* pImg, IplImage* pImgFG) { return tracker->AddBlob(pBlob, pImg, pImgFG); }

//blob tracker auto
CvBlobTrackerAuto* CvCreateBlobTrackerAuto1(CvBlobTrackerAutoParam1* param) { return cvCreateBlobTrackerAuto1(param); }
void CvBlobTrackerAutoRelease(CvBlobTrackerAuto** tracker) { delete *tracker; }
CvBlob* CvBlobTrackerAutoGetBlob(CvBlobTrackerAuto* tracker, int index) { return tracker->GetBlob(index); }
CvBlob* CvBlobTrackerAutoGetBlobByID(CvBlobTrackerAuto* tracker, int blobID) { return tracker->GetBlobByID(blobID); }
int CvBlobTrackerAutoGetBlobNum(CvBlobTrackerAuto* tracker) { return tracker->GetBlobNum(); }
void CvBlobTrackerAutoProcess(CvBlobTrackerAuto* tracker, IplImage* pImg, IplImage* pMask) { return tracker->Process(pImg, pMask); }
IplImage* CvBlobTrackerAutoGetFGMask(CvBlobTrackerAuto* tracker) { return tracker->GetFGMask(); }

//blob tracker post process
CvBlobTrackPostProc* CvCreateModuleBlobTrackPostProcKalman() { return cvCreateModuleBlobTrackPostProcKalman(); }
CvBlobTrackPostProc* CvCreateModuleBlobTrackPostProcTimeAverRect() { return cvCreateModuleBlobTrackPostProcTimeAverRect(); }
CvBlobTrackPostProc* CvCreateModuleBlobTrackPostProcTimeAverExp() { return cvCreateModuleBlobTrackPostProcTimeAverExp(); }
void CvBlobTrackPostProcRelease(CvBlobTrackPostProc** postProc) { delete *postProc; };
