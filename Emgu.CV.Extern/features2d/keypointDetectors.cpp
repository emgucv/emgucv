//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "features2d_c.h"

//ORB
cv::ORB* cveOrbCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, int scoreType, int patchSize, int fastThreshold, cv::Feature2D** feature2D, cv::Ptr<cv::ORB>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::ORB> orbPtr = cv::ORB::create(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTA_K, static_cast<cv::ORB::ScoreType>( scoreType ), patchSize, fastThreshold);
	*sharedPtr = new cv::Ptr<cv::ORB>(orbPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(orbPtr.get());
	return orbPtr.get();
#else
	throw_no_features2d();
#endif
}

void cveOrbRelease(cv::Ptr<cv::ORB>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}

//Brisk
cv::BRISK* cveBriskCreate(int thresh, int octaves, float patternScale, cv::Feature2D** feature2D, cv::Ptr<cv::BRISK>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::BRISK> briskPtr = cv::BRISK::create(thresh, octaves, patternScale);
	*sharedPtr = new cv::Ptr<cv::BRISK>(briskPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(briskPtr.get());
	return briskPtr.get();
#else
	throw_no_features2d();
#endif
}

void cveBriskRelease(cv::Ptr<cv::BRISK>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}

// detect corners using FAST algorithm
cv::FastFeatureDetector* cveFASTFeatureDetectorCreate(int threshold, bool nonmax_supression, int type, cv::Feature2D** feature2D, cv::Ptr<cv::FastFeatureDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::FastFeatureDetector> fastPtr = cv::FastFeatureDetector::create(threshold, nonmax_supression, static_cast<cv::FastFeatureDetector::DetectorType>( type ));
	*sharedPtr = new cv::Ptr<cv::FastFeatureDetector>(fastPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(fastPtr.get());
	return fastPtr.get();
#else
	throw_no_features2d();
#endif
}

void cveFASTFeatureDetectorRelease(cv::Ptr<cv::FastFeatureDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}

// MSER detector
cv::MSER* cveMserCreate(
	int delta,
	int minArea,
	int maxArea,
	double maxVariation,
	double minDiversity,
	int maxEvolution,
	double areaThreshold,
	double minMargin,
	int edgeBlurSize,
	cv::Feature2D** feature2D, 
	cv::Ptr<cv::MSER>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
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
	*sharedPtr = new cv::Ptr<cv::MSER>(mserPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(mserPtr.get());
	return mserPtr.get();
#else
	throw_no_features2d();
#endif
}

void cveMserDetectRegions(
	cv::MSER* mserPtr,
	cv::_InputArray* image,
	std::vector< std::vector<cv::Point> >* msers,
	std::vector< cv::Rect >* bboxes)
{
#ifdef HAVE_OPENCV_FEATURES2D
	mserPtr->detectRegions(*image, *msers, *bboxes);
#else
	throw_no_features2d();
#endif
}

void cveMserRelease(cv::Ptr<cv::MSER>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}

// SimpleBlobDetector
cv::SimpleBlobDetector* cveSimpleBlobDetectorCreate(cv::Feature2D** feature2DPtr, cv::Ptr<cv::SimpleBlobDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::SimpleBlobDetector> detectorPtr = cv::SimpleBlobDetector::create();
	*sharedPtr = new cv::Ptr<cv::SimpleBlobDetector>(detectorPtr);
	*feature2DPtr = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
	return detectorPtr.get();
#else
	throw_no_features2d();
#endif
}
void cveSimpleBlobDetectorRelease(cv::Ptr<cv::SimpleBlobDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}
cv::SimpleBlobDetector* cveSimpleBlobDetectorCreateWithParams(cv::Feature2D** feature2DPtr, cv::SimpleBlobDetector::Params* params, cv::Ptr<cv::SimpleBlobDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::SimpleBlobDetector> detectorPtr = cv::SimpleBlobDetector::create(*params);
	*sharedPtr = new cv::Ptr<cv::SimpleBlobDetector>(detectorPtr);
	*feature2DPtr = dynamic_cast<cv::Feature2D*>(detectorPtr.get());
	return detectorPtr.get();
#else
	throw_no_features2d();
#endif
}
cv::SimpleBlobDetector::Params* cveSimpleBlobDetectorParamsCreate()
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::SimpleBlobDetector::Params* p = new cv::SimpleBlobDetector::Params();
	return p;
#else
	throw_no_features2d();
#endif
}
void cveSimpleBlobDetectorParamsRelease(cv::SimpleBlobDetector::Params** params)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *params;
	*params = 0;
#else
	throw_no_features2d();
#endif
}

// Draw keypoints.
void drawKeypoints(
	cv::_InputArray* image,
	const std::vector<cv::KeyPoint>* keypoints,
	cv::_InputOutputArray* outImage,
	const CvScalar* color,
	int flags)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::drawKeypoints(*image, *keypoints, *outImage, *color, static_cast<cv::DrawMatchesFlags>( flags ));
#else
	throw_no_features2d();
#endif
}

// Draws matches of keypoints from two images on output image.
void drawMatchedFeatures1(
	cv::_InputArray* img1,
	const std::vector<cv::KeyPoint>* keypoints1,
	cv::_InputArray* img2,
	const std::vector<cv::KeyPoint>* keypoints2,
	std::vector< cv::DMatch >* matches,
	cv::_InputOutputArray* outImg,
	const CvScalar* matchColor,
	const CvScalar* singlePointColor,
	std::vector< unsigned char >* matchesMask,
	int flags)
{
#ifdef HAVE_OPENCV_FEATURES2D
	if (matchesMask)
	{
		std::vector< char >  matchesVec;	
		for (std::vector< unsigned char >::iterator it = matchesMask->begin(); it != matchesMask->end(); ++it)
		{
			matchesVec.push_back(static_cast<char>(*it));
		}

		cv::drawMatches(*img1, *keypoints1, *img2, *keypoints2, *matches, *outImg,
			*matchColor, *singlePointColor, matchesVec, static_cast<cv::DrawMatchesFlags>(flags));
	}
	else
	{
		cv::drawMatches(*img1, *keypoints1, *img2, *keypoints2, *matches, *outImg,
			*matchColor, *singlePointColor, std::vector< char >(), static_cast<cv::DrawMatchesFlags>(flags));
	}
#else
	throw_no_features2d();
#endif
}

void drawMatchedFeatures2(
	cv::_InputArray* img1,
	const std::vector<cv::KeyPoint>* keypoints1,
	cv::_InputArray* img2,
	const std::vector<cv::KeyPoint>* keypoints2,
	std::vector< std::vector< cv::DMatch > >* matches,
	cv::_InputOutputArray* outImg,
	const CvScalar* matchColor, 
	const CvScalar* singlePointColor,
	std::vector< std::vector< unsigned char > >* matchesMask,
	int flags)
{
#ifdef HAVE_OPENCV_FEATURES2D
	if (matchesMask)
	{
		std::vector< std::vector< char > > matchesVec;
		
		for (std::vector< std::vector< unsigned char > >::iterator it = matchesMask->begin();
			it != matchesMask->begin();
			++it)
		{
			std::vector< char > charVec;
			for (std::vector< unsigned char >::iterator it2 = it->begin(); it2 != it->end(); ++it2)
			{
				charVec.push_back(static_cast<char>(*it2));
			}
			matchesVec.push_back(charVec);
		}
		cv::drawMatches(*img1, *keypoints1, *img2, *keypoints2, *matches, *outImg,
			*matchColor, *singlePointColor, matchesVec, static_cast<cv::DrawMatchesFlags>( flags ));
	}
	else
	{
		cv::drawMatches(*img1, *keypoints1, *img2, *keypoints2, *matches, *outImg,
			*matchColor, *singlePointColor, std::vector< std::vector< char > >(), static_cast<cv::DrawMatchesFlags>( flags ));
	}
#else
	throw_no_features2d();
#endif
}

void drawMatchedFeatures3(
	cv::_InputArray* img1, const std::vector<cv::KeyPoint>* keypoints1,
	cv::_InputArray* img2, const std::vector<cv::KeyPoint>* keypoints2,
	std::vector< std::vector< cv::DMatch > >* matches,
	cv::_InputOutputArray* outImg,
	const CvScalar* matchColor, const CvScalar* singlePointColor,
	cv::_InputArray* matchesMask,
	int flags)
{
#ifdef HAVE_OPENCV_FEATURES2D
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
			matchesVec[i][0] = (char)*begin;
			matchesVec[i][1] = 0;
		}
		cv::drawMatches(*img1, *keypoints1, *img2, *keypoints2, *matches, *outImg,
			*matchColor, *singlePointColor, matchesVec, static_cast<cv::DrawMatchesFlags>(flags));
	}
	else
	{
		cv::drawMatches(*img1, *keypoints1, *img2, *keypoints2, *matches, *outImg,
			*matchColor, *singlePointColor, std::vector< std::vector< char > >(), static_cast<cv::DrawMatchesFlags>(flags));
	}
#else
	throw_no_features2d();
#endif
}

//DescriptorMatcher
void cveDescriptorMatcherAdd(cv::DescriptorMatcher* matcher, cv::_InputArray* trainDescriptors)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->add(*trainDescriptors);
#else
	throw_no_features2d();
#endif
}

void cveDescriptorMatcherKnnMatch1(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k,
	cv::_InputArray* mask,
	bool compactResult)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->knnMatch(*queryDescriptors, *trainDescriptors, *matches, k, mask ? *mask : (cv::InputArray) cv::noArray(), compactResult);
#else
	throw_no_features2d();
#endif
}

void cveDescriptorMatcherKnnMatch2(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector< cv::DMatch > >* matches,
	int k,
	cv::_InputArray* mask,
	bool compactResult)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->knnMatch(*queryDescriptors, *matches, k, mask ? *mask : (cv::InputArrayOfArrays) cv::noArray(), compactResult);
#else
	throw_no_features2d();
#endif
}

cv::Algorithm* cveDescriptorMatcherGetAlgorithm(cv::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_FEATURES2D
	return dynamic_cast<cv::Algorithm*>(matcher);
#else
	throw_no_features2d();
#endif
}

cv::BFMatcher* cveBFMatcherCreate(int distanceType, bool crossCheck, cv::DescriptorMatcher** m)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::BFMatcher* matcher = new cv::BFMatcher(distanceType, crossCheck);
	*m = dynamic_cast<cv::DescriptorMatcher*> (matcher);
	return matcher;
#else
	throw_no_features2d();
#endif
}

void cveBFMatcherRelease(cv::BFMatcher** matcher)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *matcher;
	*matcher = 0;
#else
	throw_no_features2d();
#endif
}

void cveDescriptorMatcherClear(cv::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->clear();
#else
	throw_no_features2d();
#endif
}
bool cveDescriptorMatcherEmpty(cv::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_FEATURES2D
	return matcher->empty();
#else
	throw_no_features2d();
#endif
}
bool cveDescriptorMatcherIsMaskSupported(cv::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_FEATURES2D
	return matcher->isMaskSupported();
#else
	throw_no_features2d();
#endif
}
void cveDescriptorMatcherTrain(cv::DescriptorMatcher* matcher)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->train();
#else
	throw_no_features2d();
#endif
}
void cveDescriptorMatcherMatch1(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector<cv::DMatch>* matches,
	cv::_InputArray* mask)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->match(*queryDescriptors, *trainDescriptors, *matches, mask ? *mask : (cv::InputArray) cv::noArray());
#else
	throw_no_features2d();
#endif
}
void cveDescriptorMatcherMatch2(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector<cv::DMatch>* matches,
	cv::_InputArray* masks)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->match(*queryDescriptors, *matches, masks ? *masks : (cv::InputArrayOfArrays) cv::noArray());
#else
	throw_no_features2d();
#endif
}

void cveDescriptorMatcherRadiusMatch1(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	cv::_InputArray* trainDescriptors,
	std::vector< std::vector<cv::DMatch> >* matches,
	float maxDistance,
	cv::_InputArray* mask,
	bool compactResult)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->radiusMatch(*queryDescriptors, *matches, maxDistance, mask ? *mask : (cv::InputArray) cv::noArray(), compactResult);
#else
	throw_no_features2d();
#endif
}
void cveDescriptorMatcherRadiusMatch2(
	cv::DescriptorMatcher* matcher,
	cv::_InputArray* queryDescriptors,
	std::vector< std::vector<cv::DMatch> >* matches,
	float maxDistance,
	cv::_InputArray* masks,
	bool compactResult)
{
#ifdef HAVE_OPENCV_FEATURES2D
	matcher->radiusMatch(*queryDescriptors, *matches, maxDistance, masks ? *masks : (cv::InputArrayOfArrays) cv::noArray(), compactResult);
#else
	throw_no_features2d();
#endif
}

//FlannBasedMatcher
cv::FlannBasedMatcher* cveFlannBasedMatcherCreate(cv::flann::IndexParams* indexParams, cv::flann::SearchParams* searchParams, cv::DescriptorMatcher** m)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::flann::IndexParams> ip(indexParams, [](cv::flann::IndexParams*){});
	cv::Ptr<cv::flann::SearchParams> sp(searchParams, [](cv::flann::SearchParams*) {});
	cv::FlannBasedMatcher* matcher = new cv::FlannBasedMatcher(ip, sp);
	*m = dynamic_cast<cv::DescriptorMatcher*>(matcher);
	return matcher;
#else
	throw_no_features2d();
#endif
}
void cveFlannBasedMatcherRelease(cv::FlannBasedMatcher** matcher)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *matcher;
	*matcher = 0;
#else
	throw_no_features2d();
#endif
}

//2D tracker
int voteForSizeAndOrientation(std::vector<cv::KeyPoint>* modelKeyPoints, std::vector<cv::KeyPoint>* observedKeyPoints, std::vector< std::vector< cv::DMatch > >* matches, cv::Mat* mask, double scaleIncrement, int rotationBins)
{
#ifdef HAVE_OPENCV_FEATURES2D
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
		if (maskMat(i, 0))
		{
			cv::KeyPoint observedKeyPoint = observedKeyPoints->at(i);
			cv::KeyPoint modelKeyPoint = modelKeyPoints->at(matches->at(i).at(0).trainIdx);
			s = log10(observedKeyPoint.size / modelKeyPoint.size);
			logScale.push_back(s);
			maxS = s > maxS ? s : maxS;
			minS = s < minS ? s : minS;

			r = observedKeyPoint.angle - modelKeyPoint.angle;
			r = r < 0.0f ? r + 360.0f : r;
			rotations.push_back(r);
		}
	}

	int scaleBinSize = cvCeil((maxS - minS) / log10(scaleIncrement));
	if (scaleBinSize < 2) scaleBinSize = 2;
	float scaleRanges[] = { minS, (float)(minS + scaleBinSize * log10(scaleIncrement)) };

	cv::Mat_<float> scalesMat(logScale);
	cv::Mat_<float> rotationsMat(rotations);
	std::vector<float> flags(logScale.size());
	cv::Mat flagsMat(flags);

	{  //Perform voting for both scale and orientation
		int histSize[] = { scaleBinSize, rotationBins };
		float rotationRanges[] = { 0, 360 };
		int channels[] = { 0, 1 };
		const float* ranges[] = { scaleRanges, rotationRanges };
		double minVal, maxVal;

		const cv::Mat_<float> arrs[] = { scalesMat, rotationsMat };

		cv::MatND hist; //CV_32S
		cv::calcHist(arrs, 2, channels, cv::Mat(), hist, 2, histSize, ranges, true);
		cv::minMaxLoc(hist, &minVal, &maxVal);

		cv::threshold(hist, hist, maxVal * 0.5, 0, cv::THRESH_TOZERO);
		cv::calcBackProject(arrs, 2, channels, hist, flagsMat, ranges);
	}

	int idx = 0;
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
#else
	throw_no_features2d();
#endif
}

//Feature2D
void CvFeature2DDetectAndCompute(cv::Feature2D* feature2D, cv::_InputArray* image, cv::_InputArray* mask, std::vector<cv::KeyPoint>* keypoints, cv::_OutputArray* descriptors, bool useProvidedKeyPoints)
{
#ifdef HAVE_OPENCV_FEATURES2D
	feature2D->detectAndCompute(*image, mask ? *mask : (cv::InputArray) cv::noArray(), *keypoints, *descriptors, useProvidedKeyPoints);
#else
	throw_no_features2d();
#endif
}
void CvFeature2DDetect(cv::Feature2D* feature2D, cv::_InputArray* image, std::vector<cv::KeyPoint>* keypoints, cv::_InputArray* mask)
{
#ifdef HAVE_OPENCV_FEATURES2D
	feature2D->detect(*image, *keypoints, mask ? *mask : (cv::InputArray) cv::noArray());
#else
	throw_no_features2d();
#endif
}
void CvFeature2DCompute(cv::Feature2D* feature2D, cv::_InputArray* image, std::vector<cv::KeyPoint>* keypoints, cv::_OutputArray* descriptors)
{
#ifdef HAVE_OPENCV_FEATURES2D
	feature2D->compute(*image, *keypoints, *descriptors);
#else
	throw_no_features2d();
#endif
}
int CvFeature2DGetDescriptorSize(cv::Feature2D* feature2D)
{
#ifdef HAVE_OPENCV_FEATURES2D
	return feature2D->descriptorSize();
#else
	throw_no_features2d();
#endif
}
cv::Algorithm* CvFeature2DGetAlgorithm(cv::Feature2D* feature2D)
{
#ifdef HAVE_OPENCV_FEATURES2D
	return dynamic_cast<cv::Algorithm*>(feature2D);
#else
	throw_no_features2d();
#endif
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
cv::GFTTDetector* cveGFTTDetectorCreate(int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double k, cv::Feature2D** feature2D, cv::Ptr<cv::GFTTDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::GFTTDetector> gfttPtr = cv::GFTTDetector::create(maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, k);
	*sharedPtr = new cv::Ptr<cv::GFTTDetector>(gfttPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(gfttPtr.get());
	return gfttPtr.get();
#else
	throw_no_features2d();
#endif
}
void cveGFTTDetectorRelease(cv::Ptr<cv::GFTTDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}


//BowKMeansTrainer
cv::BOWKMeansTrainer* cveBOWKMeansTrainerCreate(int clusterCount, const CvTermCriteria* termcrit, int attempts, int flags)
{
#ifdef HAVE_OPENCV_FEATURES2D
	return new cv::BOWKMeansTrainer(clusterCount, *termcrit, attempts, flags);
#else
	throw_no_features2d();
#endif
}
void cveBOWKMeansTrainerRelease(cv::BOWKMeansTrainer** trainer)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete * trainer;
	*trainer = 0;
#else
	throw_no_features2d();
#endif
}
int cveBOWKMeansTrainerGetDescriptorCount(cv::BOWKMeansTrainer* trainer)
{
#ifdef HAVE_OPENCV_FEATURES2D
	return trainer->descriptorsCount();
#else
	throw_no_features2d();
#endif
}
void cveBOWKMeansTrainerAdd(cv::BOWKMeansTrainer* trainer, cv::Mat* descriptors)
{
#ifdef HAVE_OPENCV_FEATURES2D
	trainer->add(*descriptors);
#else
	throw_no_features2d();
#endif
}
void cveBOWKMeansTrainerCluster(cv::BOWKMeansTrainer* trainer, cv::_OutputArray* cluster)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Mat m = trainer->cluster();
	m.copyTo(*cluster);
#else
	throw_no_features2d();
#endif
}

//BOWImgDescriptorExtractor
cv::BOWImgDescriptorExtractor* cveBOWImgDescriptorExtractorCreate(cv::Feature2D* descriptorExtractor, cv::DescriptorMatcher* descriptorMatcher)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::Feature2D> extractorPtr(descriptorExtractor, [] (cv::Feature2D*) {});
	
	cv::Ptr<cv::DescriptorMatcher> matcherPtr(descriptorMatcher, [] (cv::DescriptorMatcher*){});
	
	return new cv::BOWImgDescriptorExtractor(extractorPtr, matcherPtr);
#else
	throw_no_features2d();
#endif
}
void cveBOWImgDescriptorExtractorRelease(cv::BOWImgDescriptorExtractor** descriptorExtractor)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *descriptorExtractor;
	*descriptorExtractor = 0;
#else
	throw_no_features2d();
#endif
}
void cveBOWImgDescriptorExtractorSetVocabulary(cv::BOWImgDescriptorExtractor* bowImgDescriptorExtractor, cv::Mat* vocabulary)
{
#ifdef HAVE_OPENCV_FEATURES2D
	bowImgDescriptorExtractor->setVocabulary(*vocabulary);
#else
	throw_no_features2d();
#endif
}
void cveBOWImgDescriptorExtractorCompute(cv::BOWImgDescriptorExtractor* bowImgDescriptorExtractor, cv::_InputArray* image, std::vector<cv::KeyPoint>* keypoints, cv::Mat* imgDescriptor)
{
#ifdef HAVE_OPENCV_FEATURES2D
	bowImgDescriptorExtractor->compute(*image, *keypoints, *imgDescriptor);
#else
	throw_no_features2d();
#endif
}

//KAZEDetector
cv::KAZE* cveKAZEDetectorCreate(
	bool extended, bool upright, float threshold,
	int octaves, int sublevels, int diffusivity,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::KAZE>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::KAZE> kazePtr = cv::KAZE::create(extended, upright, threshold, octaves, sublevels, static_cast<cv::KAZE::DiffusivityType>( diffusivity ));
	*sharedPtr = new cv::Ptr<cv::KAZE>(kazePtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(kazePtr.get());

	return kazePtr.get();
#else
	throw_no_features2d();
#endif
}
void cveKAZEDetectorRelease(cv::Ptr<cv::KAZE>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}


//AKAZEDetector
cv::AKAZE* cveAKAZEDetectorCreate(
	int descriptorType, int descriptorSize, int descriptorChannels,
	float threshold, int octaves, int sublevels, int diffusivity,
	cv::Feature2D** feature2D,
	cv::Ptr<cv::AKAZE>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::AKAZE> akazePtr = cv::AKAZE::create( static_cast<cv::AKAZE::DescriptorType>( descriptorType ), descriptorSize, descriptorChannels, threshold, octaves, sublevels, static_cast<cv::KAZE::DiffusivityType>( diffusivity ));
	*sharedPtr = new cv::Ptr<cv::AKAZE>(akazePtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(akazePtr.get());
	return akazePtr.get();
#else
	throw_no_features2d();
#endif
}
void cveAKAZEDetectorRelease(cv::Ptr<cv::AKAZE>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}


//Agast
cv::AgastFeatureDetector* cveAgastFeatureDetectorCreate(int threshold, bool nonmaxSuppression, int type, cv::Feature2D** feature2D, cv::Ptr<cv::AgastFeatureDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::AgastFeatureDetector> agastPtr = cv::AgastFeatureDetector::create(threshold, nonmaxSuppression, static_cast<cv::AgastFeatureDetector::DetectorType>( type ));
	*sharedPtr = new cv::Ptr<cv::AgastFeatureDetector>(agastPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(agastPtr.get());
	return agastPtr.get();
#else
	throw_no_features2d();
#endif
}
void cveAgastFeatureDetectorRelease(cv::Ptr<cv::AgastFeatureDetector>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}

//SIFTDetector
cv::SIFT* cveSIFTCreate(
	int nFeatures, int nOctaveLayers,
	double contrastThreshold, double edgeThreshold,
	double sigma, cv::Feature2D** feature2D,
	cv::Ptr<cv::SIFT>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	cv::Ptr<cv::SIFT> siftPtr = cv::SIFT::create(nFeatures, nOctaveLayers, contrastThreshold, edgeThreshold, sigma);
	*sharedPtr = new cv::Ptr<cv::SIFT>(siftPtr);
	*feature2D = dynamic_cast<cv::Feature2D*>(siftPtr.get());

	return siftPtr.get();
#else
	throw_no_features2d();
#endif
}

void cveSIFTRelease(cv::Ptr<cv::SIFT>** sharedPtr)
{
#ifdef HAVE_OPENCV_FEATURES2D
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_features2d();
#endif
}