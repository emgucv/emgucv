//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "saliency_c.h"

cv::saliency::Saliency* cveSaliencyCreate(cv::String* saliencyType)
{
	cv::Ptr<cv::saliency::Saliency> saliency = cv::saliency::Saliency::create(*saliencyType);
	saliency.addref();
	return saliency.get();
}

void cveSaliencyRelease(cv::saliency::Saliency** saliency)
{
	delete *saliency;
	*saliency = 0;
}

bool cveSaliencyComputeSaliency(cv::saliency::Saliency* saliency, cv::_InputArray* image, cv::_OutputArray* saliencyMap)
{
	return saliency->computeSaliency(*image, *saliencyMap);
}

bool cveSaliencyStaticComputeBinaryMap(cv::saliency::Saliency* saliency, cv::Mat* saliencyMap, cv::Mat* binaryMap)
{
	return dynamic_cast<cv::saliency::StaticSaliency*>(saliency)->computeBinaryMap(*saliencyMap, *binaryMap);
}

bool cveSaliencyMotionInit(cv::saliency::Saliency* saliency)
{
	return dynamic_cast<cv::saliency::MotionSaliencyBinWangApr2014*>(saliency)->init();
}

void cveSaliencyMotionSetImageSize(cv::saliency::Saliency* saliency, int width, int height)
{
	dynamic_cast<cv::saliency::MotionSaliencyBinWangApr2014*>(saliency)->setImagesize(width, height);
}

void cveSaliencyGetObjectnessValues(cv::saliency::Saliency* saliency, std::vector<float>* values)
{
	std::vector<float> vals = dynamic_cast<cv::saliency::ObjectnessBING*>(saliency)->getobjectnessValues();

	values->reserve(vals.size());
	for (std::vector<float>::iterator it = vals.begin(); it < vals.end(); ++it)
		values->push_back(*it);
}

void cveSaliencySetTrainingPath(cv::saliency::Saliency* saliency, cv::String* trainingPath)
{
	dynamic_cast<cv::saliency::ObjectnessBING*>(saliency)->setTrainingPath(*trainingPath);
}

cv::Algorithm* cveSaliencyGetAlgorithm(cv::saliency::Saliency* saliency)
{
	return dynamic_cast<cv::Algorithm*>(saliency);
}