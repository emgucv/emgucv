//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "features2d_c.h"



//ORB
cv::ORB* cveOrbDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, int fastThreshold, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::ORB> orbPtr = cv::ORB::create(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTA_K, scoreType, patchSize, fastThreshold);
   orbPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(orbPtr.get());
   return orbPtr.get();
}

void cveOrbDetectorRelease(cv::ORB** detector)
{
   delete *detector;
   *detector = 0;
}

//Brisk
cv::BRISK* cveBriskCreate(int thresh, int octaves, float patternScale, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::BRISK> briskPtr = cv::BRISK::create(thresh, octaves, patternScale);
   briskPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(briskPtr.get());
   return briskPtr.get();
}

void cveBriskRelease(cv::BRISK** detector)
{
   delete *detector;
   *detector = 0;
}

// detect corners using FAST algorithm
cv::FastFeatureDetector* cveFASTGetFeatureDetector(int threshold, bool nonmax_supression, int type, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::FastFeatureDetector> fastPtr = cv::FastFeatureDetector::create(threshold, nonmax_supression, type);
   fastPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(fastPtr.get());
   return fastPtr.get();
}

void cveFASTFeatureDetectorRelease(cv::FastFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}

// MSER detector
cv::MSER* cveMserGetFeatureDetector(
   int delta, 
   int minArea, 
   int maxArea,
   double maxVariation, 
   double minDiversity,
   int maxEvolution, 
   double areaThreshold,
   double minMargin, 
   int edgeBlurSize, 
   cv::Feature2D** feature2D)
{  
   cv::Ptr<cv::MSER> mserPtr = cv::MSER::create(
      delta,
      minArea, 
      maxArea,
      maxVariation,
      minDiversity, 
      maxEvolution,
      areaThreshold,
      minMargin, 
      edgeBlurSize);
   mserPtr.addref();
   *feature2D = dynamic_cast<cv::MSER*>(mserPtr.get());
   return mserPtr.get();
}

void cveMserDetectRegions(
	cv::MSER* mserPtr,
	cv::_InputArray* image,
	std::vector< std::vector<cv::Point> >* msers,
	std::vector< cv::Rect >* bboxes)
{
	mserPtr->detectRegions(*image, *msers, *bboxes);
}

void cveMserFeatureDetectorRelease(cv::MSER** detector)
{
   delete *detector;
   *detector = 0;
}

// SimpleBlobDetector
cv::SimpleBlobDetector* cveSimpleBlobDetectorCreate(cv::Feature2D** feature2DPtr)
{
   cv::Ptr<cv::SimpleBlobDetector> detectorPtr = cv::SimpleBlobDetector::create();
   detectorPtr.addref();
   *feature2DPtr = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
   return detectorPtr.get();
}
void cveSimpleBlobDetectorRelease(cv::SimpleBlobDetector** detector)
{
   delete *detector;
   *detector = 0;
}
cv::SimpleBlobDetector* cveSimpleBlobDetectorCreateWithParams(cv::Feature2D** feature2DPtr, cv::SimpleBlobDetector::Params* params)
{
   cv::Ptr<cv::SimpleBlobDetector> detectorPtr = cv::SimpleBlobDetector::create(*params);
   detectorPtr.addref();
   *feature2DPtr = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
   return detectorPtr.get();
}
cv::SimpleBlobDetector::Params* cveSimpleBlobDetectorParamsCreate()
{
   cv::SimpleBlobDetector::Params* p = new cv::SimpleBlobDetector::Params();
   return p;
}
void cveSimpleBlobDetectorParamsRelease(cv::SimpleBlobDetector::Params** params)
{
   delete *params;
   *params = 0;
}

// Draw keypoints.
void drawKeypoints(
   cv::_InputArray* image, 
   const std::vector<cv::KeyPoint>* keypoints, 
   cv::_InputOutputArray* outImage,
   const CvScalar* color, 
   int flags)
{
   cv::drawKeypoints(*image, *keypoints, *outImage, *color, flags);
}

// Draws matches of keypints from two images on output image.
void drawMatchedFeatures(
   cv::_InputArray* img1, const std::vector<cv::KeyPoint>* keypoints1,
   cv::_InputArray* img2, const std::vector<cv::KeyPoint>* keypoints2,
   std::vector< std::vector< cv::DMatch > >* matches, 
   cv::_InputOutputArray* outImg,
   const CvScalar* matchColor, const CvScalar* singlePointColor,
   cv::_InputArray* matchesMask, 
   int flags)
{
   if (matchesMask)
   {
      int size = matchesMask->rows() * matchesMask->cols() * matchesMask->channels();
      std::vector< std::vector< char > > matchesVec(
         size,
         std::vector< char >(2));
      cv::Mat m = matchesMask->getMat();
     
      cv::MatIterator_<unsigned char> begin, end;
      int i = 0;
      for (
         i = 0, begin = m.begin<unsigned char>(), end = m.end<unsigned char>();
         begin != end;
         ++begin, ++i)
      {
         matchesVec[i][0] = (char) *begin;
         matchesVec[i][1] = 0;
      }
      cv::drawMatches(*img1, *keypoints1, *img2, *keypoints2, *matches, *outImg, 
         *matchColor, *singlePointColor, matchesVec, flags);
   } else
   {
      cv::drawMatches(*img1, *keypoints1, *img2, *keypoints2, *matches, *outImg, 
         *matchColor, *singlePointColor, std::vector< std::vector< char > >(), flags);
   }

}

//DescriptorMatcher
void CvDescriptorMatcherAdd(cv::DescriptorMatcher* matcher, cv::_InputArray* trainDescriptors)
{
   matcher->add(*trainDescriptors);   
}

void CvDescriptorMatcherKnnMatch(cv::DescriptorMatcher* matcher, cv::_InputArray* queryDescriptors, 
                   std::vector< std::vector< cv::DMatch > >* matches, int k,
                   cv::_InputArray* mask) 
{
   matcher->knnMatch(*queryDescriptors, *matches, k, mask ? * mask : (cv::InputArray) cv::noArray(), false);
}

cv::Algorithm* CvDescriptorMatcherGetAlgorithm(cv::DescriptorMatcher* matcher)
{
   return dynamic_cast<cv::Algorithm*>( matcher );
}

cv::BFMatcher* cveBFMatcherCreate(int distanceType, bool crossCheck, cv::DescriptorMatcher** m)
{
   cv::BFMatcher* matcher = new cv::BFMatcher(distanceType, crossCheck);
   *m = dynamic_cast< cv::DescriptorMatcher* > (matcher);
   return matcher;
}

void cveBFMatcherRelease(cv::BFMatcher** matcher)
{
   delete *matcher;
   *matcher = 0;
}

//FlannBasedMatcher
cv::FlannBasedMatcher* cveFlannBasedMatcherCreate(cv::flann::IndexParams* indexParams, cv::flann::SearchParams* searchParams, cv::DescriptorMatcher** m)
{
	cv::Ptr<cv::flann::IndexParams> ip = indexParams;
	cv::Ptr<cv::flann::SearchParams> sp = searchParams;
	ip.addref(); //add reference such that the matcher's destructor will not release the indexParams
	sp.addref(); //add reference such that the matcher's destructor will not release the searchParams
	cv::FlannBasedMatcher* matcher = new cv::FlannBasedMatcher(ip, sp);
	*m = dynamic_cast<cv::DescriptorMatcher*>(matcher);
	return matcher;
}
void cveFlannBasedMatcherRelease(cv::FlannBasedMatcher** matcher)
{
	delete *matcher;
	*matcher = 0;
}

//2D tracker
int voteForSizeAndOrientation(std::vector<cv::KeyPoint>* modelKeyPoints, std::vector<cv::KeyPoint>* observedKeyPoints, std::vector< std::vector< cv::DMatch > >* matches, cv::Mat* mask, double scaleIncrement, int rotationBins)
{
   CV_Assert(!modelKeyPoints->empty());
   CV_Assert(!observedKeyPoints->empty());
   CV_Assert(mask->depth() == CV_8U && mask->channels() == 1);
   //cv::Mat_<int> indicesMat = (cv::Mat_<int>) cv::cvarrToMat(indices);
   cv::Mat_<uchar> maskMat = (cv::Mat_<uchar>) *mask;
   std::vector<float> logScale;
   std::vector<float> rotations;
   float s, maxS, minS, r;
   maxS = -1.0e-10f; minS = 1.0e10f;

   for (int i = 0; i < maskMat.rows; i++)
   {
      if ( maskMat(i, 0)) 
      {
         cv::KeyPoint observedKeyPoint = observedKeyPoints->at(i);
         cv::KeyPoint modelKeyPoint = modelKeyPoints->at( matches->at(i).at(0).trainIdx);
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

//Feature2D
void CvFeature2DDetectAndCompute(cv::Feature2D* feature2D, cv::_InputArray* image, cv::_InputArray* mask, std::vector<cv::KeyPoint>* keypoints, cv::_OutputArray* descriptors, bool useProvidedKeyPoints)
{
   feature2D->detectAndCompute(*image, mask ? *mask : (cv::InputArray) cv::noArray(), *keypoints, *descriptors, useProvidedKeyPoints);
}
void CvFeature2DDetect(cv::Feature2D* feature2D, cv::_InputArray* image, std::vector<cv::KeyPoint>* keypoints, cv::_InputArray* mask)
{
   feature2D->detect(*image, *keypoints, mask ? *mask : (cv::InputArray) cv::noArray());
}
void CvFeature2DCompute(cv::Feature2D* feature2D, cv::_InputArray* image,  std::vector<cv::KeyPoint>* keypoints, cv::_OutputArray* descriptors)
{
   feature2D->compute(*image, *keypoints, *descriptors);
}
int CvFeature2DGetDescriptorSize(cv::Feature2D* feature2D)
{
   return feature2D->descriptorSize();
}
cv::Algorithm* CvFeature2DGetAlgorithm(cv::Feature2D* feature2D)
{
   return dynamic_cast<cv::Algorithm*>( feature2D );
}


/*
//OpponentColorDescriptorExtractor
cv::OpponentColorDescriptorExtractor* CvOpponentColorDescriptorExtractorCreate(cv::DescriptorExtractor* extractor)
{
   cv::Ptr<cv::DescriptorExtractor> ptr(extractor);
   ptr.addref();
   return new cv::OpponentColorDescriptorExtractor(ptr);
}
void CvOpponentColorDescriptorExtractorRelease(cv::OpponentColorDescriptorExtractor** extractor)
{
   delete *extractor;
   *extractor = 0;
}*/


//GFTT
cv::GFTTDetector* cveGFTTDetectorCreate( int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double k, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::GFTTDetector> gfttPtr = cv::GFTTDetector::create(maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, k);
   gfttPtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(gfttPtr.get());
   return gfttPtr.get();
}
void cveGFTTDetectorRelease(cv::GFTTDetector** detector)
{
   delete *detector;
   *detector = 0;
}


//BowKMeansTrainer
cv::BOWKMeansTrainer* CvBOWKMeansTrainerCreate(int clusterCount, const CvTermCriteria* termcrit, int attempts, int flags)
{
   return new cv::BOWKMeansTrainer(clusterCount, *termcrit, attempts, flags);
}
void CvBOWKMeansTrainerRelease(cv::BOWKMeansTrainer** trainer)
{
   delete * trainer;
   *trainer = 0;
}
int CvBOWKMeansTrainerGetDescriptorCount(cv::BOWKMeansTrainer* trainer)
{
   return trainer->descriptorsCount();
}
void CvBOWKMeansTrainerAdd(cv::BOWKMeansTrainer* trainer, cv::Mat* descriptors)
{
   trainer->add(*descriptors);
}
void CvBOWKMeansTrainerCluster(cv::BOWKMeansTrainer* trainer, cv::_OutputArray* cluster)
{
   cv::Mat m = trainer->cluster();
   m.copyTo(*cluster);
}

//BOWImgDescriptorExtractor
cv::BOWImgDescriptorExtractor* CvBOWImgDescriptorExtractorCreate(cv::Feature2D* descriptorExtractor, cv::DescriptorMatcher* descriptorMatcher)
{
   cv::Ptr<cv::Feature2D> extractorPtr(descriptorExtractor);
   extractorPtr.addref();
   cv::Ptr<cv::DescriptorMatcher> matcherPtr(descriptorMatcher);
   matcherPtr.addref();
   return new cv::BOWImgDescriptorExtractor(extractorPtr, matcherPtr);
}
void CvBOWImgDescriptorExtractorRelease(cv::BOWImgDescriptorExtractor** descriptorExtractor)
{
   delete *descriptorExtractor;
   *descriptorExtractor = 0;
}
void CvBOWImgDescriptorExtractorSetVocabulary(cv::BOWImgDescriptorExtractor* bowImgDescriptorExtractor, cv::Mat* vocabulary)
{
   bowImgDescriptorExtractor->setVocabulary(*vocabulary);
}
void CvBOWImgDescriptorExtractorCompute(cv::BOWImgDescriptorExtractor* bowImgDescriptorExtractor, cv::_InputArray* image, std::vector<cv::KeyPoint>* keypoints, cv::Mat* imgDescriptor)
{
   bowImgDescriptorExtractor->compute(*image, *keypoints, *imgDescriptor);
}

//KAZEDetector
cv::KAZE* cveKAZEDetectorCreate(
  bool extended, bool upright, float threshold,
  int octaves, int sublevels, int diffusivity, 
  cv::Feature2D** feature2D)
{
   cv::Ptr<cv::KAZE> kazePtr = cv::KAZE::create(extended, upright, threshold, octaves, sublevels, diffusivity);
   kazePtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(kazePtr.get());
   
   return kazePtr.get();
}
void cveKAZEDetectorRelease(cv::KAZE** detector)
{
   delete *detector;
   *detector = 0;
}


//AKAZEDetector
cv::AKAZE* cveAKAZEDetectorCreate(
  int descriptorType, int descriptorSize, int descriptorChannels,
  float threshold, int octaves, int sublevels, int diffusivity,
  cv::Feature2D** feature2D)
{
   cv::Ptr<cv::AKAZE> akazePtr = cv::AKAZE::create(descriptorType, descriptorSize, descriptorChannels, threshold, octaves, sublevels, diffusivity);
   akazePtr.addref();
   *feature2D = dynamic_cast<cv::Feature2D*>(akazePtr.get());
   return akazePtr.get();
}
void cveAKAZEDetectorRelease(cv::AKAZE** detector)
{
   delete *detector;
   *detector = 0;
}


//Agast
cv::AgastFeatureDetector* cveAgastFeatureDetectorCreate(int threshold, bool nonmaxSuppression, int type, cv::Feature2D** feature2D)
{
   cv::Ptr<cv::AgastFeatureDetector> agastPtr = cv::AgastFeatureDetector::create(threshold, nonmaxSuppression, type);
   agastPtr.addref();
   return agastPtr.get();
}
void cveAgastFeatureDetectorRelease(cv::AgastFeatureDetector** detector)
{
   delete *detector;
   *detector = 0;
}
