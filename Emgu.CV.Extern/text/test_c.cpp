//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "text_c.h"

//ERFilter
cv::text::ERFilter* cveERFilterNM1Create(
	cv::String* classifier,
	int thresholdDelta,
	float minArea,
	float maxArea,
	float minProbability,
	bool nonMaxSuppression,
	float minProbabilityDiff,
	cv::Ptr<cv::text::ERFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_TEXT
	cv::Ptr<cv::text::ERFilter> filter = cv::text::createERFilterNM1(cv::text::loadClassifierNM1(*classifier), thresholdDelta, minArea, maxArea, minProbability, nonMaxSuppression, minProbabilityDiff);
	*sharedPtr = new cv::Ptr<cv::text::ERFilter>(filter);
	return filter.get();
#else
	throw_no_text();
#endif
}
cv::text::ERFilter* cveERFilterNM2Create(cv::String* classifier, float minProbability, cv::Ptr<cv::text::ERFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_TEXT
	cv::Ptr<cv::text::ERFilter> filter = cv::text::createERFilterNM2(cv::text::loadClassifierNM2(*classifier), minProbability);
	*sharedPtr = new cv::Ptr<cv::text::ERFilter>(filter);
	return filter.get();
#else
	throw_no_text();
#endif
}
void cveERFilterRelease(cv::Ptr<cv::text::ERFilter>** sharedPtr)
{
#ifdef HAVE_OPENCV_TEXT
	delete *sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_text();
#endif
}
void cveERFilterRun(cv::text::ERFilter* filter, cv::_InputArray* image, std::vector<cv::text::ERStat>* regions)
{
#ifdef HAVE_OPENCV_TEXT
	filter->run(*image, *regions);
#else
	throw_no_text();
#endif
}

void cveERGrouping(
	cv::_InputArray* image, cv::_InputArray* channels,
	std::vector<cv::text::ERStat>** regions, int count,
	std::vector< std::vector<cv::Vec2i> >* groups, std::vector<cv::Rect>* group_rects,
	int method, cv::String* fileName, float minProbability)
{
#ifdef HAVE_OPENCV_TEXT
	std::vector< std::vector< cv::text::ERStat > > statVecs;
	for (int i = 0; i < count; i++)
	{
		statVecs.push_back(*regions[i]);
	}

	cv::text::erGrouping(*image, *channels, statVecs, *groups, *group_rects, method, *fileName, minProbability);
#else
	throw_no_text();
#endif
}

void cveMSERsToERStats(
	cv::_InputArray* image,
	std::vector< std::vector< cv::Point > >* contours,
	std::vector< std::vector< cv::text::ERStat> >* regions)
{
#ifdef HAVE_OPENCV_TEXT
	cv::text::MSERsToERStats(*image, *contours, *regions);
#else
	throw_no_text();
#endif
}

void cveComputeNMChannels(cv::_InputArray* src, cv::_OutputArray* channels, int mode)
{
#ifdef HAVE_OPENCV_TEXT
	cv::text::computeNMChannels(*src, *channels, mode);
#else
	throw_no_text();
#endif
}

cv::text::TextDetectorCNN* cveTextDetectorCNNCreate(cv::String* modelArchFilename, cv::String* modelWeightsFilename, cv::Ptr<cv::text::TextDetectorCNN>** sharedPtr)
{
#ifdef HAVE_OPENCV_TEXT
	cv::Ptr<cv::text::TextDetectorCNN> detector = cv::text::TextDetectorCNN::create(*modelArchFilename, *modelWeightsFilename);
	*sharedPtr = new cv::Ptr<cv::text::TextDetectorCNN>(detector);
	return detector.get();
#else
	throw_no_text();
#endif
}
cv::text::TextDetectorCNN* cveTextDetectorCNNCreate2(cv::String* modelArchFilename, cv::String* modelWeightsFilename, std::vector<cv::Size>* detectionSizes, cv::Ptr<cv::text::TextDetectorCNN>** sharedPtr)
{
#ifdef HAVE_OPENCV_TEXT
	cv::Ptr<cv::text::TextDetectorCNN> detector = cv::text::TextDetectorCNN::create(*modelArchFilename, *modelWeightsFilename, *detectionSizes);
	*sharedPtr = new cv::Ptr<cv::text::TextDetectorCNN>(detector);
	return detector.get();
#else
	throw_no_text();
#endif	
}
void cveTextDetectorCNNDetect(cv::text::TextDetectorCNN* detector, cv::_InputArray* inputImage, std::vector<cv::Rect>* bbox, std::vector<float>* confidence)
{
#ifdef HAVE_OPENCV_TEXT
	detector->detect(*inputImage, *bbox, *confidence);
#else
	throw_no_text();
#endif
}
void cveTextDetectorCNNRelease(cv::Ptr<cv::text::TextDetectorCNN>** sharedPtr)
{
#ifdef HAVE_OPENCV_TEXT
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_text();
#endif
}