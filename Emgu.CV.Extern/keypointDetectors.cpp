//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "opencv2/core/core_c.h"
#include "opencv2/features2d/features2d.hpp"
#include "opencv2/contrib/contrib.hpp"

//FernClassifier
CVAPI(cv::FernClassifier*) CvFernClassifierCreate() { return new cv::FernClassifier; }
CVAPI(void) CvFernClassifierRelease(cv::FernClassifier* classifier) { delete classifier;}

CVAPI(void) CvFernClassifierTrainFromSingleView(
                                  cv::FernClassifier* classifier,
                                  IplImage* image,
                                  cv::KeyPoint* keypoints,
                                  int numberOfKeyPoints,
                                  int _patchSize,
                                  int _signatureSize,
                                  int _nstructs,
                                  int _structSize,
                                  int _nviews,
                                  int _compressionMethod,
                                  cv::PatchGenerator* patchGenerator)
{
   cv::Mat mat = cv::cvarrToMat(image);
   std::vector<cv::KeyPoint> keypointVector = std::vector<cv::KeyPoint>(numberOfKeyPoints); 
   memcpy(&keypointVector[0], keypoints, numberOfKeyPoints * sizeof(cv::KeyPoint));
   classifier->trainFromSingleView(mat, keypointVector, _patchSize, _signatureSize, _nstructs, _structSize, _nviews, _compressionMethod, *patchGenerator);
}

//Patch Genetator
CVAPI(void) CvPatchGeneratorInit(cv::PatchGenerator* pg) 
{ 
   cv::PatchGenerator defaultPG;
   memcpy(pg, &defaultPG, sizeof(cv::PatchGenerator));
}

//LDetector
CVAPI(void) CvLDetectorDetectKeyPoints(cv::LDetector* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, int maxCount, bool scaleCoords)
{
   cv::Mat mat = cv::cvarrToMat(image);
   (*detector)(mat, *keypoints, maxCount, scaleCoords);
}

//SelfSimDescriptor
CVAPI(cv::SelfSimDescriptor*) CvSelfSimDescriptorCreate(int smallSize,int largeSize, int startDistanceBucket, int numberOfDistanceBuckets, int numberOfAngles)
{  return new cv::SelfSimDescriptor(smallSize, largeSize, startDistanceBucket, numberOfDistanceBuckets, numberOfAngles); }
CVAPI(void) CvSelfSimDescriptorRelease(cv::SelfSimDescriptor* descriptor) { delete descriptor; }
CVAPI(void) CvSelfSimDescriptorCompute(cv::SelfSimDescriptor* descriptor, IplImage* image, std::vector<float>* descriptors, cv::Size* winStride, cv::Point* locations, int numberOfLocation)
{
   std::vector<cv::Point> locationVec = std::vector<cv::Point>(numberOfLocation);
   memcpy(&locationVec[0], locations, sizeof(cv::Point) * numberOfLocation);
   //CV_Assert(numberOfLocation == locationVec.size());
   cv::Mat imageMat = cv::cvarrToMat(image);
   descriptor->compute(imageMat, *descriptors, *winStride, locationVec);

   //float sumAbs = 0.0f;
   //for (int i = 0; i < descriptors->data.size(); i++)
   //   sumAbs += descriptors->data[i];
   
   //CV_Assert(sumAbs != 0.0f);
   
}
CVAPI(int) CvSelfSimDescriptorGetDescriptorSize(cv::SelfSimDescriptor* descriptor) { return descriptor->getDescriptorSize(); }

//StarDetector
CVAPI(void) CvStarDetectorDetectKeyPoints(cv::StarDetector* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints)
{
   cv::Mat mat = cv::cvarrToMat(image);
   (*detector)(mat, *keypoints);
}

//SIFTDetector
CVAPI(cv::SIFT*) CvSIFTDetectorCreate(
   int nOctaves, int nOctaveLayers, int firstOctave, int angleMode,//common parameters
   double threshold, double edgeThreshold, //detector parameters
   double magnification, bool isNormalize, bool recalculateAngles) //descriptor parameters
{
   cv::SIFT tmp;
   cv::SIFT::CommonParams p = tmp.getCommonParams();
   p.nOctaves=nOctaves;
   p.nOctaveLayers=nOctaveLayers; 
   p.firstOctave=firstOctave;
   p.angleMode=angleMode;

   cv::SIFT::DetectorParams detectorP = tmp.getDetectorParams();
   detectorP.threshold=threshold;
   detectorP.edgeThreshold=edgeThreshold;

   cv::SIFT::DescriptorParams descriptorP = tmp.getDescriptorParams();
   descriptorP.magnification=magnification;
   descriptorP.isNormalize=isNormalize;
   descriptorP.recalculateAngles=recalculateAngles;
   return new cv::SIFT(p, detectorP, descriptorP);
}

CVAPI(void) CvSIFTDetectorRelease(cv::SIFT** detector)
{
   delete *detector;
   detector = 0;
}

CVAPI(int) CvSIFTDetectorGetDescriptorSize(cv::SIFT* detector)
{
   return detector->descriptorSize();
}

CVAPI(void) CvSIFTDetectorDetectKeyPoints(cv::SIFT* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   (*detector)(mat, maskMat, *keypoints);
}

/*
CVAPI(void) CvSIFTDetectorDetectFeature(cv::SIFT* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, std::vector<float>* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   cv::Mat descriptorsMat;
   (*detector)(mat, maskMat, *keypoints, descriptorsMat, false);

   descriptors->resize(keypoints->size()*detector->descriptorSize());

   if (keypoints->size() > 0)
      memcpy(&(*descriptors)[0], descriptorsMat.ptr<float>(), sizeof(float)* descriptorsMat.rows * descriptorsMat.cols);
}*/

CVAPI(void) CvSIFTDetectorComputeDescriptors(cv::SIFT* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);

   cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);
   (*detector)(mat, maskMat, *keypoints, descriptorsMat, true);
}

//SURFDetector
CVAPI(void) CvSURFDetectorDetectKeyPoints(cv::SURF* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   (*detector)(mat, maskMat, *keypoints);
}

/*
CVAPI(void) CvSURFDetectorDetectFeature(cv::SURF* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, std::vector<float>* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   (*detector)(mat, maskMat, *keypoints, *descriptors, false);
}*/

CVAPI(void) CvSURFDetectorComputeDescriptors(cv::SURF* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::SurfDescriptorExtractor extractor(detector->nOctaves, detector->nOctaveLayers, detector->extended != 0);

   cv::Mat img = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   cv::Mat result = cv::cvarrToMat(descriptors);
   extractor.compute(img, *keypoints, result);
}

// detect corners using FAST algorithm
CVAPI(void) CvFASTKeyPoints( IplImage* image, std::vector<cv::KeyPoint>* keypoints, int threshold, bool nonmax_supression)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::FAST(mat, *keypoints, threshold, nonmax_supression);
}

// MSER detector
CVAPI(void) CvMSERKeyPoints(IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, CvMSERParams* param)
{
   cv::MserFeatureDetector mser = cv::MserFeatureDetector(param->delta, param->minArea, param->maxArea, param->maxVariation, param->minDiversity, param->maxEvolution, param->areaThreshold, param->minMargin, param->edgeBlurSize);
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat = mask ? cv::cvarrToMat(mask) : cv::Mat();

   mser.detect(mat, *keypoints, maskMat);
}

//Plannar Object Detector
CVAPI(cv::PlanarObjectDetector*) CvPlanarObjectDetectorDefaultCreate() { return new cv::PlanarObjectDetector; }
CVAPI(void) CvPlanarObjectDetectorRelease(cv::PlanarObjectDetector* detector) { delete detector; }
CVAPI(void) CvPlanarObjectDetectorTrain(
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
CVAPI(void) CvPlanarObjectDetectorDetect(cv::PlanarObjectDetector* detector, IplImage* image, CvMat* homography, CvSeq* corners)
{
   std::vector<cv::Point2f> cornerVec;
   cv::Mat imageMat = cv::cvarrToMat(image);
   cv::Mat homographyMat = cv::cvarrToMat(homography);
   (*detector)(imageMat, homographyMat,  cornerVec);
   if (cornerVec.size() > 0)
      cvSeqPushMulti(corners, &cornerVec[0], cornerVec.size());
}
CVAPI(void) CvPlanarObjectDetectorGetModelPoints(cv::PlanarObjectDetector* detector, CvSeq* modelPoints)
{
   std::vector<cv::KeyPoint> modelPtVec = detector->getModelPoints();
   if (modelPtVec.size() > 0)
      cvSeqPushMulti(modelPoints, &modelPtVec[0], modelPtVec.size());
}

// Draws matches of keypints from two images on output image.
CVAPI(void) drawMatchedFeatures(
                                const IplImage* img1, const std::vector<cv::KeyPoint>* keypoints1,
                                const IplImage* img2, const std::vector<cv::KeyPoint>* keypoints2,
                                const CvMat* matchIndicies, 
                                IplImage* outImg,
                                const CvScalar matchColor, const CvScalar singlePointColor,
                                const CvMat* matchesMask, 
                                int flags)
{
   cv::Mat mat1 = cv::cvarrToMat(img1);
   cv::Mat mat2 = cv::cvarrToMat(img2);
   cv::Mat_<int> matchesMat = (cv::Mat_<int>) cv::cvarrToMat(matchIndicies);
   cv::Mat_<unsigned char> matchesMaskMat = 
      matchesMask ? (cv::Mat_<unsigned char>) cv::cvarrToMat(matchesMask) : cv::Mat();
   
   std::vector<cv::DMatch> matches;
   for (int i = 0; i < matchesMat.rows; ++i)
   {
      cv::DMatch m(i, matchesMat.at<int>(i, 0), 0.0f);
      matches.push_back(m);
   }

   cv::Mat outMat = cv::cvarrToMat(outImg);
   cv::drawMatches(mat1, *keypoints1, mat2, *keypoints2, matches, outMat, 
      matchColor, singlePointColor, matchesMaskMat, flags);
}