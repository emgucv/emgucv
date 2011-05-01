//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "opencv2/core/core_c.h"
#include "opencv2/features2d/features2d.hpp"
#include "opencv2/contrib/contrib.hpp"
#include "vectorOfDMatch.h"

//FernClassifier
CVAPI(cv::FernClassifier*) CvFernClassifierCreate() { return new cv::FernClassifier; }
CVAPI(void) CvFernClassifierRelease(cv::FernClassifier* classifier) { delete classifier;}

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
                                  cv::PatchGenerator* patchGenerator)
{
   cv::Mat mat = cv::cvarrToMat(image);
   classifier->trainFromSingleView(mat, *keypoints, _patchSize, _signatureSize, _nstructs, _structSize, _nviews, _compressionMethod, *patchGenerator);
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
CVAPI(cv::StarFeatureDetector*) CvStarGetFeatureDetector(cv::StarDetector* detector)
{
   return new cv::StarFeatureDetector(*detector);
}

CVAPI(void) CvStarFeatureDetectorRelease(cv::StarFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
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

CVAPI(cv::SiftFeatureDetector*) CvSiftGetFeatureDetector(cv::SIFT* detector)
{
   return new cv::SiftFeatureDetector(detector->getDetectorParams(), detector->getCommonParams());
}

CVAPI(void) CvSiftFeatureDetectorRelease(cv::SiftFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
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

//SIFT with OpponentColorDescriptorExtractor
CVAPI(void) CvSIFTDetectorComputeDescriptorsBGR(cv::SIFT* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   /*
   if (keypoints->size() <= 0) return;
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);

   cv::Ptr<cv::DescriptorExtractor> siftExtractor = new cv::SiftDescriptorExtractor(detector->getDescriptorParams(), detector->getCommonParams());
   cv::OpponentColorDescriptorExtractor colorDetector(siftExtractor);
   colorDetector.compute(mat, *keypoints, descriptorsMat);
*/

   if (keypoints->size() <= 0) return;
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);
   cv::Ptr<cv::DescriptorExtractor> siftExtractor = new cv::SiftDescriptorExtractor(detector->getDescriptorParams(), detector->getCommonParams());
   cv::OpponentColorDescriptorExtractor colorDetector(siftExtractor);
   colorDetector.compute(mat, *keypoints, descriptorsMat);
}

//FeatureDetector
CVAPI(void) CvFeatureDetectorDetectKeyPoints(cv::FeatureDetector* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   detector->detect(mat, *keypoints, maskMat);
}

CVAPI(void) CvFeatureDetectorRelease(cv::FeatureDetector** detector)
{
   delete *detector;
}

//GridAdaptedFeatureDetector
CVAPI(cv::GridAdaptedFeatureDetector*) GridAdaptedFeatureDetectorCreate(   
   cv::FeatureDetector* detector,
   int maxTotalKeypoints,
   int gridRows, int gridCols)
{
   return new cv::GridAdaptedFeatureDetector(detector, maxTotalKeypoints, gridRows, gridCols);
}
/*
CVAPI(void) GridAdaptedFeatureDetectorDetect(
   cv::GridAdaptedFeatureDetector* detector, 
   const cv::Mat* image, std::vector<cv::KeyPoint>* keypoints, const cv::Mat* mask)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat = mask? cv::cvarrToMat(mask) : cv::Mat();
   detector->detect(mat, *keypoints, maskMat);
}*/

CVAPI(void) GridAdaptedFeatureDetectorRelease(cv::GridAdaptedFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//SURFDetector
CVAPI(cv::SurfFeatureDetector*) CvSURFGetFeatureDetector(cv::SURF* detector)
{
   return new cv::SurfFeatureDetector(detector->hessianThreshold, detector->nOctaves, detector->nOctaveLayers);
}

CVAPI(void) CvSURFFeatureDetectorRelease(cv::SurfFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
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

   cv::Mat img = cv::cvarrToMat(image);
   cv::Mat maskMat = mask ? cv::cvarrToMat(mask) : cv::Mat();
   std::vector<float> desc;
   (*detector)(img, maskMat, *keypoints, desc, true);
   CV_Assert(desc.size() == descriptors->width * descriptors->height);
   memcpy(descriptors->data.ptr, &desc[0], desc.size());
}

//SURF with OpponentColorDescriptorExtractor
CVAPI(void) CvSURFDetectorComputeDescriptorsBGR(cv::SURF* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);
   cv::Ptr<cv::DescriptorExtractor> surfExtractor = new cv::SurfDescriptorExtractor(detector->nOctaves, detector->nOctaveLayers, detector->extended != 0);
   cv::OpponentColorDescriptorExtractor colorDetector(surfExtractor);
   colorDetector.compute(mat, *keypoints, descriptorsMat);
}

CVAPI(cv::BriefDescriptorExtractor*) CvBriefDescriptorExtractorCreate(int descriptorSize)
{
   return new cv::BriefDescriptorExtractor(descriptorSize);
}

CVAPI(int) CvBriefDescriptorExtractorGetDescriptorSize(cv::BriefDescriptorExtractor* extractor)
{
   return extractor->descriptorSize();
}

CVAPI(void) CvBriefDescriptorComputeDescriptors(cv::BriefDescriptorExtractor* extractor, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::Mat img = cv::cvarrToMat(image);
   cv::Mat result = cv::cvarrToMat(descriptors);
   extractor->compute(img, *keypoints, result);
}

CVAPI(void) CvBriefDescriptorExtractorRelease(cv::BriefDescriptorExtractor** extractor)
{
   delete *extractor;
}

// detect corners using FAST algorithm
CVAPI(cv::FastFeatureDetector*) CvFASTGetFeatureDetector(int threshold, bool nonmax_supression)
{
   return new cv::FastFeatureDetector(threshold, nonmax_supression);
}

CVAPI(void) CvFASTFeatureDetectorRelease(cv::FastFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}

// MSER detector
CVAPI(cv::MserFeatureDetector*) CvMserGetFeatureDetector(CvMSERParams* detector)
{  
   return new cv::MserFeatureDetector(*detector);
}

CVAPI(void) CvMserFeatureDetectorRelease(cv::MserFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
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

   std::vector<cv::DMatch> matches;
   VectorOfDMatchPushMatrix(&matches, matchIndicies, 0, matchesMask);

   cv::Mat outMat = cv::cvarrToMat(outImg);
   cv::drawMatches(mat1, *keypoints1, mat2, *keypoints2, matches, outMat, 
      matchColor, singlePointColor, std::vector<char>(), flags);
}

//DescriptorMatcher
CVAPI(void) CvDescriptorMatcherAdd(cv::DescriptorMatcher* matcher, CvMat* trainDescriptor)
{
   cv::Mat trainMat = cv::cvarrToMat(trainDescriptor);
   std::vector<cv::Mat> trainVector;
   trainVector.push_back(trainMat);
   matcher->add(trainVector);   
}

CVAPI(void) CvDescriptorMatcherKnnMatch(cv::DescriptorMatcher* matcher, const CvMat* queryDescriptors, 
                   CvMat* trainIdx, CvMat* distance, int k,
                   const CvMat* mask) 
{
   //only implemented for a single trained image for now
   CV_Assert( matcher->getTrainDescriptors().size() == 1);

   cv::Mat queryMat = cv::cvarrToMat(queryDescriptors);
   cv::Mat trainIdxMat = cv::cvarrToMat(trainIdx);
   cv::Mat distanceMat = cv::cvarrToMat(distance);
   cv::Mat maskMat = mask ? cv::cvarrToMat(mask) : cv::Mat();
   std::vector< std::vector< cv::DMatch > > matches; //The first index is the index of the image.
   std::vector<cv::Mat> masks;
   matcher->knnMatch(queryMat, matches, k, masks, false);
   
   float* distance_ptr = distanceMat.ptr<float>();
   int* trainIdx_ptr = trainIdxMat.ptr<int>();
   for(std::vector<cv::DMatch>::iterator m = matches[0].begin(); m != matches[0].end(); ++m, ++trainIdx_ptr, ++distance_ptr)
   {
      cv::DMatch match = *m;
      *distance_ptr = match.distance;
      *trainIdx_ptr = match.trainIdx;
   }
}
CVAPI(cv::DescriptorMatcher*) CvBruteForceMatcherCreate(int distanceType)
{
   switch(distanceType)
   {
   case(0): //L1 float
      return new cv::BruteForceMatcher< cv::L1<float> >();
   case(1): //L2 float
      return new cv::BruteForceMatcher< cv::L2<float> >();
   case(2): //HammingLUT
      return new cv::BruteForceMatcher< cv::HammingLUT >();
   case(3):
      return new cv::BruteForceMatcher< cv::Hamming >();
   default:
      return 0;
   }
}

CVAPI(void) CvBruteForceMatcherRelease(cv::DescriptorMatcher** matcher, int distanceType)
{
   cv::BruteForceMatcher< cv::L1<float> >* m0;
   cv::BruteForceMatcher< cv::L2<float> >* m1;
   cv::BruteForceMatcher< cv::HammingLUT >* m2;
   cv::BruteForceMatcher< cv::Hamming >* m3;
   switch(distanceType)
   {
   case(0): //L1 float
      m0 = (cv::BruteForceMatcher< cv::L1<float> >*) *matcher;
      delete m0;
      break;
   case(1): //L2 float
      m1 = (cv::BruteForceMatcher< cv::L2<float> >*) *matcher;
      delete m1;
      break;
   case(2):
      m2 = (cv::BruteForceMatcher< cv::HammingLUT >*) *matcher;
      delete m2;
      break;
   case(3):
      m3 = (cv::BruteForceMatcher< cv::Hamming >*) *matcher;
      delete m3;
   default:
      CV_Error(-1, "Invalid Distance type");
   }
   *matcher = 0;
}
