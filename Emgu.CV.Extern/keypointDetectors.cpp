//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "features2d_c.h"

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

//SelfSimDescriptor
cv::SelfSimDescriptor* CvSelfSimDescriptorCreate(int smallSize,int largeSize, int startDistanceBucket, int numberOfDistanceBuckets, int numberOfAngles)
{  return new cv::SelfSimDescriptor(smallSize, largeSize, startDistanceBucket, numberOfDistanceBuckets, numberOfAngles); }
void CvSelfSimDescriptorRelease(cv::SelfSimDescriptor* descriptor) { delete descriptor; }
void CvSelfSimDescriptorCompute(cv::SelfSimDescriptor* descriptor, IplImage* image, std::vector<float>* descriptors, cv::Size* winStride, cv::Point* locations, int numberOfLocation)
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
int CvSelfSimDescriptorGetDescriptorSize(cv::SelfSimDescriptor* descriptor) { return static_cast<int>(descriptor->getDescriptorSize()); }

//StarDetector
cv::StarFeatureDetector* CvStarGetFeatureDetector(cv::StarDetector* detector)
{
   return new cv::StarFeatureDetector(*detector);
}

void CvStarFeatureDetectorRelease(cv::StarFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//SIFTDetector
cv::SIFT* CvSIFTDetectorCreate(
   int nFeatures, int nOctaveLayers, 
   double contrastThreshold, double edgeThreshold, 
   double sigma, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor)
{
   cv::SIFT* sift = new cv::SIFT(nFeatures, nOctaveLayers, contrastThreshold, edgeThreshold, sigma);
   *featureDetector = static_cast<cv::FeatureDetector*>(sift);
   *descriptorExtractor = static_cast<cv::DescriptorExtractor*>(sift);
   return sift;
}

void CvSIFTDetectorRelease(cv::SIFT** detector)
{
   delete *detector;
   *detector = 0;
}

int CvSIFTDetectorGetDescriptorSize(cv::SIFT* detector)
{
   return detector->descriptorSize();
}

/*
void CvSIFTDetectorDetectFeature(cv::SIFT* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, std::vector<float>* descriptors)
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

void CvSIFTDetectorComputeDescriptors(cv::SIFT* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;

   cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);

   if (mat.channels() == 1)
      (*detector)(mat, maskMat, *keypoints, descriptorsMat, true);
   else //opponent color
   {
      cv::Ptr<cv::DescriptorExtractor> siftExtractor(detector);
      siftExtractor.addref(); //add reference such that the detector will not be released when the smart pointer went out of scope.
      cv::OpponentColorDescriptorExtractor colorDetector(siftExtractor);
      colorDetector.compute(mat, *keypoints, descriptorsMat);
   }
}

//ORB
cv::ORB* CvOrbDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor)
{
   cv::ORB* orb = new cv::ORB(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTA_K, scoreType, patchSize);
   *featureDetector = static_cast<cv::FeatureDetector*>(orb);
   *descriptorExtractor = static_cast<cv::DescriptorExtractor*>(orb);
   return orb;
}

int CvOrbDetectorGetDescriptorSize(cv::ORB* detector)
{
   return detector->descriptorSize();
}

void CvOrbDetectorComputeDescriptors(cv::ORB* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;

   cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);
   if (mat.channels() == 1)
   {
      (*detector)(mat, maskMat, *keypoints, descriptorsMat, true);
   } else
   {
      //TODO: create the extractor using the parameters in the ORB detector instead of using the default values
      cv::Ptr<cv::DescriptorExtractor> extractor = new cv::OrbDescriptorExtractor();

      cv::OpponentColorDescriptorExtractor colorExtractor(extractor);
      colorExtractor.compute(mat, *keypoints, descriptorsMat);
   }
}

void CvOrbDetectorRelease(cv::ORB** detector)
{
   delete *detector;
   *detector = 0;
}

//FeatureDetector
void CvFeatureDetectorDetectKeyPoints(cv::FeatureDetector* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   detector->detect(mat, *keypoints, maskMat);
}

void CvFeatureDetectorRelease(cv::FeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//GridAdaptedFeatureDetector
cv::GridAdaptedFeatureDetector* GridAdaptedFeatureDetectorCreate(   
   cv::FeatureDetector* detector,
   int maxTotalKeypoints,
   int gridRows, int gridCols)
{
   cv::Ptr<cv::FeatureDetector> detectorPtr = detector;
   detectorPtr.addref(); //increment the counter such that it should never be release by the grid adapeted feature detector
   return new cv::GridAdaptedFeatureDetector(detectorPtr, maxTotalKeypoints, gridRows, gridCols);
}
/*
void GridAdaptedFeatureDetectorDetect(
   cv::GridAdaptedFeatureDetector* detector, 
   const cv::Mat* image, std::vector<cv::KeyPoint>* keypoints, const cv::Mat* mask)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat = mask? cv::cvarrToMat(mask) : cv::Mat();
   detector->detect(mat, *keypoints, maskMat);
}*/

void GridAdaptedFeatureDetectorRelease(cv::GridAdaptedFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}

//SURFDetector
CVAPI(cv::SURF*) CvSURFDetectorCreate(CvSURFParams* detector, cv::FeatureDetector** featureDetector, cv::DescriptorExtractor** descriptorExtractor)
{
   cv::SURF* surf = new cv::SURF(detector->hessianThreshold, detector->nOctaves, detector->nOctaveLayers, detector->extended != 0, detector->upright != 0);
   *featureDetector = static_cast<cv::FeatureDetector*>(surf);
   *descriptorExtractor = static_cast<cv::DescriptorExtractor*>(surf);
   return surf;
}

CVAPI(void) CvSURFDetectorRelease(cv::SURF** detector)
{
   delete *detector;
   *detector = 0;
}

/*
void CvSURFDetectorDetectFeature(cv::SURF* detector, IplImage* image, IplImage* mask, std::vector<cv::KeyPoint>* keypoints, std::vector<float>* descriptors)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat maskMat;
   if (mask) maskMat = cv::cvarrToMat(mask);
   (*detector)(mat, maskMat, *keypoints, *descriptors, false);
}*/

void CvSURFDetectorComputeDescriptors(cv::SURF* detector, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;

   cv::Mat img = cv::cvarrToMat(image);
   cv::Mat maskMat;

   if (img.channels() == 1)
   {
      //std::vector<float> desc;
      cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);
      (*detector)(img, maskMat, *keypoints, descriptorsMat, true);
      //CV_Assert(desc.size() == descriptors->width * descriptors->height);
      //memcpy(descriptors->data.ptr, &desc[0], desc.size() * sizeof(float));
   } else //opponent SURF
   {
      cv::Mat descriptorsMat = cv::cvarrToMat(descriptors);
      cv::Ptr<cv::DescriptorExtractor> surfExtractor(detector);
      surfExtractor.addref();
      cv::OpponentColorDescriptorExtractor colorDetector(surfExtractor);
      colorDetector.compute(img, *keypoints, descriptorsMat);
   }
}

cv::BriefDescriptorExtractor* CvBriefDescriptorExtractorCreate(int descriptorSize)
{
   return new cv::BriefDescriptorExtractor(descriptorSize);
}

int CvBriefDescriptorExtractorGetDescriptorSize(cv::BriefDescriptorExtractor* extractor)
{
   return extractor->descriptorSize();
}

void CvBriefDescriptorComputeDescriptors(cv::BriefDescriptorExtractor* extractor, IplImage* image, std::vector<cv::KeyPoint>* keypoints, CvMat* descriptors)
{
   if (keypoints->size() <= 0) return;
   cv::Mat img = cv::cvarrToMat(image);
   cv::Mat result = cv::cvarrToMat(descriptors);
   if (img.channels() == 1)
   {
     extractor->compute(img, *keypoints, result);
   } else //opponent brief
   {
      cv::Ptr<cv::DescriptorExtractor> briefExtractor = new cv::BriefDescriptorExtractor(extractor->descriptorSize());
      cv::OpponentColorDescriptorExtractor colorDescriptorExtractor(briefExtractor);
      colorDescriptorExtractor.compute(img, *keypoints, result);
   }
}

void CvBriefDescriptorExtractorRelease(cv::BriefDescriptorExtractor** extractor)
{
   delete *extractor;
   *extractor = 0;
}

// detect corners using FAST algorithm
cv::FastFeatureDetector* CvFASTGetFeatureDetector(int threshold, bool nonmax_supression)
{
   return new cv::FastFeatureDetector(threshold, nonmax_supression);
}

void CvFASTFeatureDetectorRelease(cv::FastFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}

// MSER detector
cv::MSER* CvMserGetFeatureDetector(CvMSERParams* detector)
{  
   return new cv::MSER(
      detector->delta,
      detector->minArea, 
      detector->maxArea,
      detector->maxVariation,
      detector->minDiversity, 
      detector->maxEvolution,
      detector->areaThreshold,
      detector->minMargin, 
      detector->edgeBlurSize);
}

void CvMserFeatureDetectorRelease(cv::MSER** detector)
{
   delete *detector;
   *detector = 0;
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

// Draw keypoints.
void drawKeypoints(
                          const IplImage* image, 
                          const std::vector<cv::KeyPoint>* keypoints, 
                          IplImage* outImage,
                          const CvScalar color, 
                          int flags)
{
   cv::Mat mat = cv::cvarrToMat(image);
   cv::Mat outMat = cv::cvarrToMat(outImage);
   cv::drawKeypoints(mat, *keypoints, outMat, color, flags);
}

// Draws matches of keypints from two images on output image.
void drawMatchedFeatures(
                                const IplImage* img1, const std::vector<cv::KeyPoint>* keypoints1,
                                const IplImage* img2, const std::vector<cv::KeyPoint>* keypoints2,
                                const CvMat* matchIndices, 
                                IplImage* outImg,
                                const CvScalar matchColor, const CvScalar singlePointColor,
                                const CvMat* matchesMask, 
                                int flags)
{
   cv::Mat mat1 = cv::cvarrToMat(img1);
   cv::Mat mat2 = cv::cvarrToMat(img2);

   std::vector<cv::DMatch> matches;
   VectorOfDMatchPushMatrix(&matches, matchIndices, 0, matchesMask);

   cv::Mat outMat = cv::cvarrToMat(outImg);
   cv::drawMatches(mat1, *keypoints1, mat2, *keypoints2, matches, outMat, 
      matchColor, singlePointColor, std::vector<char>(), flags);
}

//DescriptorMatcher
void CvDescriptorMatcherAdd(cv::DescriptorMatcher* matcher, CvMat* trainDescriptor)
{
   cv::Mat trainMat = cv::cvarrToMat(trainDescriptor);
   std::vector<cv::Mat> trainVector;
   trainVector.push_back(trainMat);
   matcher->add(trainVector);   
}

void CvDescriptorMatcherKnnMatch(cv::DescriptorMatcher* matcher, const CvMat* queryDescriptors, 
                   CvMat* trainIdx, CvMat* distance, int k,
                   const CvMat* mask) 
{
   std::vector< std::vector< cv::DMatch > > matches; //The first index is the index of the query

   //only implemented for a single trained image for now
   CV_Assert( matcher->getTrainDescriptors().size() == 1);

   cv::Mat queryMat = cv::cvarrToMat(queryDescriptors);
   cv::Mat maskMat = mask ? cv::cvarrToMat(mask) : cv::Mat();
   std::vector<cv::Mat> masks;
   if (!maskMat.empty()) 
      masks.push_back(maskMat);

   matcher->knnMatch(queryMat, matches, k, masks, false);
   
   VectorOfDMatchToMat(&matches, trainIdx, distance);
}

cv::DescriptorMatcher* CvBruteForceMatcherCreate(int distanceType, bool crossCheck)
{
   return new cv::BFMatcher(distanceType, crossCheck);
}

void CvBruteForceMatcherRelease(cv::DescriptorMatcher** matcher)
{
   delete *matcher;
   *matcher = 0;
}

//2D tracker
bool getHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, CvArr* indices, CvArr* mask, double randsacThreshold, CvMat* homography)
{
   cv::Mat_<int> indMat = (cv::Mat_<int>) cv::cvarrToMat(indices);

   cv::Mat_<uchar> maskMat = mask ? (cv::Mat_<uchar>) cv::cvarrToMat(mask) : cv::Mat_<uchar>(indMat.rows, 1, 255);
   int nonZero = mask? cv::countNonZero(maskMat): indMat.rows;
   if (nonZero < 4) return false;

   std::vector<cv::Point2f> srcPtVec;
   std::vector<cv::Point2f> dstPtVec;

   for(int i = 0; i < maskMat.rows; i++)
   {
      if ( maskMat.at<uchar>(i) )
      {  
         int modelIdx = indMat(i, 0); 
         srcPtVec.push_back((*model)[modelIdx].pt);
         dstPtVec.push_back((*observed)[i].pt);
      }
   }
   
   //cv::Mat_<uchar> ransacMask(srcPtVec.size(), 1);
   std::vector<uchar> ransacMask;
   cv::Mat result = cv::findHomography(cv::Mat(srcPtVec), cv::Mat(dstPtVec), cv::RANSAC, randsacThreshold, ransacMask);
   cv::Mat hMat = cv::cvarrToMat(homography);
   result.copyTo(hMat);

   int idx = 0;
   for (int i = 0; i < maskMat.rows; i++)
   {
      uchar* val = maskMat.ptr<uchar>(i);
      if (*val)
         *val = ransacMask[idx++];
   }
   return true;

}

int voteForSizeAndOrientation(std::vector<cv::KeyPoint>* modelKeyPoints, std::vector<cv::KeyPoint>* observedKeyPoints, CvArr* indices, CvArr* mask, double scaleIncrement, int rotationBins)
{
   CV_Assert(!modelKeyPoints->empty());
   CV_Assert(!observedKeyPoints->empty());
   cv::Mat_<int> indicesMat = (cv::Mat_<int>) cv::cvarrToMat(indices);
   cv::Mat_<uchar> maskMat = (cv::Mat_<uchar>) cv::cvarrToMat(mask);
   std::vector<float> logScale;
   std::vector<float> rotations;
   float s, maxS, minS, r;
   maxS = -1.0e-10f; minS = 1.0e10f;

   for (int i = 0; i < maskMat.rows; i++)
   {
      if ( maskMat(i, 0)) 
      {
         cv::KeyPoint observedKeyPoint = observedKeyPoints->at(i);
         cv::KeyPoint modelKeyPoint = modelKeyPoints->at( indicesMat(i, 0));
         s = log10( observedKeyPoint.size / modelKeyPoint.size );
         logScale.push_back(s);
         maxS = s > maxS ? s : maxS;
         minS = s < minS ? s : minS;

         r = observedKeyPoint.angle - modelKeyPoint.angle;
         r = r < 0.0f? r + 360.0f : r;
         rotations.push_back(r);
      }    
   }

   int scaleBinSize = cvCeil((maxS - minS) / log10(scaleIncrement));
   if (scaleBinSize < 2) scaleBinSize = 2;
   float scaleRanges[] = {minS, (float) (minS + scaleBinSize * log10(scaleIncrement))};

   cv::Mat_<float> scalesMat(logScale);
   cv::Mat_<float> rotationsMat(rotations);
   std::vector<float> flags(logScale.size());
   cv::Mat flagsMat(flags);

   {  //Perform voting for both scale and orientation
      int histSize[] = {scaleBinSize, rotationBins};
      float rotationRanges[] = {0, 360};
      int channels[] = {0, 1};
      const float* ranges[] = {scaleRanges, rotationRanges};
      double minVal, maxVal;

      const cv::Mat_<float> arrs[] = {scalesMat, rotationsMat}; 

      cv::MatND hist; //CV_32S
      cv::calcHist(arrs, 2, channels, cv::Mat(), hist, 2, histSize, ranges, true);
      cv::minMaxLoc(hist, &minVal, &maxVal);

      cv::threshold(hist, hist, maxVal * 0.5, 0, cv::THRESH_TOZERO);
      cv::calcBackProject(arrs, 2, channels, hist, flagsMat, ranges);
   }

   int idx =0;
   int nonZeroCount = 0;
   for (int i = 0; i < maskMat.rows; i++)
   {
      if (maskMat(i, 0))
      {
         if (flags[idx++] != 0.0f)
            nonZeroCount++;
         else 
            maskMat(i, 0) = 0;
      }
   }
   return nonZeroCount;
}