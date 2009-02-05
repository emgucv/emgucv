#ifndef CVEXTERN_BLOB_H
#define CVEXTERN_BLOB_H

#include "cvaux.h"
//Blob
CVAPI(CvBlobSeq*) CvBlobSeqCreate(int BlobSize = sizeof(CvBlob));
CVAPI(void) CvBlobSeqRelease(CvBlobSeq* blobSeq);
CVAPI(CvBlob*) CvBlobSeqGetBlob(CvBlobSeq* blobSeq, int blobIndex);
CVAPI(int) CvBlobSeqGetBlobNum(CvBlobSeq* blobSeq);

//Forground detector
CVAPI(CvFGDetector*) CvCreateFGDetectorBase(int type, void* param) { return cvCreateFGDetectorBase(type, param); }
CVAPI(void) CvFGDetectorRelease(CvFGDetector* detector) { detector->Release(); }

//Blob Detector
CVAPI(void) CvBlobDetectorRelease(CvBlobDetector* detector);
CVAPI(int) CvBlobDetectorDetectNewBlob(CvBlobDetector* detector, IplImage* pImg, IplImage* pImgFG, CvBlobSeq* pNewBlobList, CvBlobSeq* pOldBlobList);
CVAPI(CvBlobDetector*) CvCreateBlobDetectorSimple();
CVAPI(CvBlobDetector*) CvCreateBlobDetectorCC();

//blob Tracker
/* Simple blob tracker based on connected component tracking: */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerCC() { return  cvCreateBlobTrackerCC(); }
/* Connected component tracking and mean-shift particle filter collion-resolver: */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerCCMSPF() { return  cvCreateBlobTrackerCCMSPF(); }
/* Blob tracker that integrates meanshift and connected components: */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerMSFG() { return  cvCreateBlobTrackerMSFG(); }
CVAPI( CvBlobTracker*) CvCreateBlobTrackerMSFGS() { return  cvCreateBlobTrackerMSFGS(); }
/* Meanshift without connected-components */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerMS() { return  cvCreateBlobTrackerMS(); }
/* Particle filtering via Bhattacharya coefficient, which        */
/* is roughly the dot-product of two probability densities.      */
/* See: Real-Time Tracking of Non-Rigid Objects using Mean Shift */
/*      Comanicius, Ramesh, Meer, 2000, 8p                       */
/*      http://citeseer.ist.psu.edu/321441.html                  */
CVAPI( CvBlobTracker*) CvCreateBlobTrackerMSPF() { return  cvCreateBlobTrackerMSPF(); }
CVAPI(void) CvBlobTrackerRealease(CvBlobTracker* tracker) { tracker->Release(); }
CVAPI(int) CvBlobTrackerGetBlobNum(CvBlobTracker* tracker) { return tracker->GetBlobNum(); }
CVAPI(CvBlob*) CvBlobTrackerGetBlob(CvBlobTracker* tracker, int BlobIndex) { return tracker->GetBlob(BlobIndex); }
CVAPI(void) CvBlobTrackerDelBlob(CvBlobTracker* tracker, int BlobIndex) { tracker->DelBlob(BlobIndex); }
CVAPI(CvBlob*) CvBlobTrackerAddBlob(CvBlobTracker* tracker, CvBlob* pBlob, IplImage* pImg, IplImage* pImgFG = NULL ) { return tracker->AddBlob(pBlob, pImg, pImgFG); }

//blob tracker auto
CVAPI(CvBlobTrackerAuto*) CvCreateBlobTrackerAuto1(CvBlobTrackerAutoParam1* param =NULL) { return cvCreateBlobTrackerAuto1(param); }
CVAPI(void) CvBlobTrackerAutoRelease(CvBlobTrackerAuto* tracker) { tracker->Release(); }
CVAPI(CvBlob*) CvBlobTrackerAutoGetBlob(CvBlobTrackerAuto* tracker, int index) { return tracker->GetBlob(index); }
CVAPI(CvBlob*) CvBlobTrackerAutoGetBlobByID(CvBlobTrackerAuto* tracker, int blobID) { return tracker->GetBlobByID(blobID); }
CVAPI(int) CvBlobTrackerAutoGetBlobNum(CvBlobTrackerAuto* tracker) { return tracker->GetBlobNum(); }
CVAPI(void) CvBlobTrackerAutoProcess(CvBlobTrackerAuto* tracker, IplImage* pImg, IplImage* pMask = NULL) { return tracker->Process(pImg, pMask); }
CVAPI(IplImage*) CvBlobTrackerAutoGetFGMask(CvBlobTrackerAuto* tracker) { return tracker->GetFGMask(); }

//blob tracker post process
CVAPI(CvBlobTrackPostProc*) CvCreateModuleBlobTrackPostProcKalman() { return cvCreateModuleBlobTrackPostProcKalman(); }
CVAPI(CvBlobTrackPostProc*) CvCreateModuleBlobTrackPostProcTimeAverRect() { return cvCreateModuleBlobTrackPostProcTimeAverRect(); }
CVAPI(CvBlobTrackPostProc*) CvCreateModuleBlobTrackPostProcTimeAverExp() { return cvCreateModuleBlobTrackPostProcTimeAverExp(); }
CVAPI(void) CvBlobTrackPostProcRelease(CvBlobTrackPostProc* postProc) { postProc->Release(); };
#endif
