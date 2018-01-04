//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "legacy_c.h"

//FernClassifier
cv::FernClassifier* CvFernClassifierCreate() { return new cv::FernClassifier; }
void CvFernClassifierRelease(cv::FernClassifier* classifier) { delete classifier;}

void CvFernClassifierTrainFromSingleView(
                                  cv::FernClassifier* classifier,
                                  IplImage* image,
                                  std::vector<cv::KeyPoint>* keypoints,
                                  int _patchSize,
                                  int _signatureSize,
                                  int _nstructs,
                                  int _structSize,
                                  int _nviews,
                                  int _compressionMethod,
                                  cv::PatchGenerator* patchGenerator)
{
   cv::Mat mat = cv::cvarrToMat(image);
   classifier->trainFromSingleView(mat, *keypoints, _patchSize, _signatureSize, _nstructs, _structSize, _nviews, _compressionMethod, *patchGenerator);
}

//Patch Genetator
void CvPatchGeneratorInit(cv::PatchGenerator* pg) 
{ 
   cv::PatchGenerator defaultPG;
   memcpy(pg, &defaultPG, sizeof(cv::PatchGenerator));
}

//LDetector
void CvLDetectorDetectKeyPoints(cv::LDetector* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, int maxCount, bool scaleCoords)
{
   cv::Mat mat = cv::cvarrToMat(image);
   (*detector)(mat, *keypoints, maxCount, scaleCoords);
}

//Plannar Object Detector
cv::PlanarObjectDetector* CvPlanarObjectDetectorDefaultCreate() { return new cv::PlanarObjectDetector; }
void CvPlanarObjectDetectorRelease(cv::PlanarObjectDetector* detector) { delete detector; }
void CvPlanarObjectDetectorTrain(
   cv::PlanarObjectDetector* objectDetector, 
   IplImage* image, 
   int _npoints,
   int _patchSize,
   int _nstructs,
   int _structSize,
   int _nviews,
   cv::LDetector* detector,
   cv::PatchGenerator* patchGenerator)
{
   std::vector<cv::Mat> pyr;
   cv::Mat imageMat = cv::cvarrToMat(image);
   pyr.push_back(imageMat);
   objectDetector->train(pyr, _npoints, _patchSize, _nstructs, _structSize, _nviews, *detector, *patchGenerator);
}
void CvPlanarObjectDetectorDetect(cv::PlanarObjectDetector* detector, IplImage* image, CvMat* homography, CvSeq* corners)
{
   std::vector<cv::Point2f> cornerVec;
   cv::Mat imageMat = cv::cvarrToMat(image);
   cv::Mat homographyMat = cv::cvarrToMat(homography);
   (*detector)(imageMat, homographyMat,  cornerVec);
   if (!cornerVec.empty())
      cvSeqPushMulti(corners, &cornerVec[0], static_cast<int>(cornerVec.size()));
}
void CvPlanarObjectDetectorGetModelPoints(cv::PlanarObjectDetector* detector, CvSeq* modelPoints)
{
   std::vector<cv::KeyPoint> modelPtVec = detector->getModelPoints();
   if (!modelPtVec.empty())
      cvSeqPushMulti(modelPoints, &modelPtVec[0], static_cast<int>(modelPtVec.size()));
}