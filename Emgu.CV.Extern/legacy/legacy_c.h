//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_LEGACY_C_H
#define EMGU_LEGACY_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/legacy/blobtrack.hpp"
#include "opencv2/legacy/legacy.hpp"
#include "opencv2/ml/ml.hpp"

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
CVAPI(void) CvBlobSeqAddBlob(CvBlobSeq* blobSeq, CvBlob* blob);
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

//EMLegacy
CVAPI(CvEM*) CvEMLegacyDefaultCreate();
CVAPI(void) CvEMLegacyRelease(CvEM** model);
CVAPI(bool) CvEMLegacyTrain(CvEM* model, CvMat* samples, CvMat* sample_idx,
                      CvEMParams* params, CvMat* labels );
CVAPI(float) CvEMLegacyPredict(CvEM* model, CvMat* sample, CvMat* probs );
CVAPI(int) CvEMLegacyGetNclusters(CvEM* model);
CVAPI(CvMat*) CvEMLegacyGetMeans(CvEM* model);
CVAPI(CvMat**) CvEMLegacyGetCovs(CvEM* model);
CVAPI(CvMat*) CvEMLegacyGetWeights(CvEM* model);
CVAPI(CvMat*) CvEMLegacyGetProbs(CvEM* model);

//FernClassifier
CVAPI(cv::FernClassifier*) CvFernClassifierCreate();
CVAPI(void) CvFernClassifierRelease(cv::FernClassifier* classifier);

CVAPI(void) CvFernClassifierTrainFromSingleView(
                                  cv::FernClassifier* classifier,
                                  IplImage* image,
                                  std::vector<cv::KeyPoint>* keypoints,
                                  int _patchSize,
                                  int _signatureSize,
                                  int _nstructs,
                                  int _structSize,
                                  int _nviews,
                                  int _compressionMethod,
                                  cv::PatchGenerator* patchGenerator);

//Patch Genetator
CVAPI(void) CvPatchGeneratorInit(cv::PatchGenerator* pg);

//LDetector
CVAPI(void) CvLDetectorDetectKeyPoints(cv::LDetector* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, int maxCount, bool scaleCoords);

//Plannar Object Detector
CVAPI(cv::PlanarObjectDetector*) CvPlanarObjectDetectorDefaultCreate();
CVAPI(void) CvPlanarObjectDetectorRelease(cv::PlanarObjectDetector* detector);
CVAPI(void) CvPlanarObjectDetectorTrain(
   cv::PlanarObjectDetector* objectDetector, 
   IplImage* image, 
   int _npoints,
   int _patchSize,
   int _nstructs,
   int _structSize,
   int _nviews,
   cv::LDetector* detector,
   cv::PatchGenerator* patchGenerator);
CVAPI(void) CvPlanarObjectDetectorDetect(cv::PlanarObjectDetector* detector, IplImage* image, CvMat* homography, CvSeq* corners);

CVAPI(void) CvPlanarObjectDetectorGetModelPoints(cv::PlanarObjectDetector* detector, CvSeq* modelPoints);

//RTreeClassifier
CVAPI(cv::RTreeClassifier*) CvRTreeClassifierCreate();
CVAPI(void) CvRTreeClassifierRelease(cv::RTreeClassifier* classifier);
CVAPI(void) CvRTreeClassifierTrain(
      cv::RTreeClassifier* classifier, 
      IplImage* train_image,
      CvPoint* train_points,
      int numberOfPoints,
		cv::RNG* rng, 
      int num_trees, int depth,
		int views, size_t reduced_num_dim,
		int num_quant_bits);

CVAPI(int) CvRTreeClassifierGetOriginalNumClasses(cv::RTreeClassifier* classifier);
CVAPI(int) CvRTreeClassifierGetNumClasses(cv::RTreeClassifier* classifier);

CVAPI(int) CvRTreeClassifierGetSigniture(
   cv::RTreeClassifier* classifier, 
   IplImage* image, 
   CvPoint* point,
   int patchSize,
   float* signiture);
#endif