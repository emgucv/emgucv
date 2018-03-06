//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "saliency_c.h"

cv::saliency::StaticSaliencySpectralResidual* cveStaticSaliencySpectralResidualCreate(cv::saliency::StaticSaliency** static_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::saliency::StaticSaliencySpectralResidual> ptr = cv::saliency::StaticSaliencySpectralResidual::create();
	ptr.addref();
	*static_saliency = static_cast<cv::saliency::StaticSaliency*>(ptr);
	*saliency = static_cast<cv::saliency::Saliency*>(ptr);
	*algorithm = static_cast<cv::Algorithm*>(ptr);
	return ptr.get();
}
void cveStaticSaliencySpectralResidualRelease(cv::saliency::StaticSaliencySpectralResidual** saliency)
{
	delete *saliency;
	*saliency = 0;
}

cv::saliency::StaticSaliencyFineGrained* cveStaticSaliencyFineGrainedCreate(cv::saliency::StaticSaliency** static_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::saliency::StaticSaliencyFineGrained> ptr = cv::saliency::StaticSaliencyFineGrained::create();
	ptr.addref();
	*static_saliency = static_cast<cv::saliency::StaticSaliency*>(ptr);
	*saliency = static_cast<cv::saliency::Saliency*>(ptr);
	*algorithm = static_cast<cv::Algorithm*>(ptr);
	return ptr.get();
}
void cveStaticSaliencyFineGrainedRelease(cv::saliency::StaticSaliencyFineGrained** saliency)
{
	delete *saliency;
	*saliency = 0;
}

cv::saliency::MotionSaliencyBinWangApr2014* cveMotionSaliencyBinWangApr2014Create(cv::saliency::MotionSaliency** motion_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::saliency::MotionSaliencyBinWangApr2014> ptr = cv::saliency::MotionSaliencyBinWangApr2014::create();
	ptr.addref();
	*motion_saliency = static_cast<cv::saliency::MotionSaliency*>(ptr);
	*saliency = static_cast<cv::saliency::Saliency*>(ptr);
	*algorithm = static_cast<cv::Algorithm*>(ptr);
	return ptr.get();
}
void cveMotionSaliencyBinWangApr2014Release(cv::saliency::MotionSaliencyBinWangApr2014** saliency)
{
	delete *saliency;
	*saliency = 0;
}

cv::saliency::ObjectnessBING* cveObjectnessBINGCreate(cv::saliency::Objectness** objectness_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm)
{
	cv::Ptr<cv::saliency::ObjectnessBING> ptr = cv::saliency::ObjectnessBING::create();
	ptr.addref();
	*objectness_saliency = static_cast<cv::saliency::Objectness*>(ptr);
	*saliency = static_cast<cv::saliency::Saliency*>(ptr);
	*algorithm = static_cast<cv::Algorithm*>(ptr);
	return ptr.get();
}
void cveObjectnessBINGRelease(cv::saliency::ObjectnessBING** saliency)
{
	delete *saliency;
	*saliency = 0;
}

bool cveSaliencyComputeSaliency(cv::saliency::Saliency* saliency, cv::_InputArray* image, cv::_OutputArray* saliencyMap)
{
	return saliency->computeSaliency(*image, *saliencyMap);
}

bool cveStaticSaliencyComputeBinaryMap(cv::saliency::StaticSaliency* saliency, cv::_InputArray* saliencyMap, cv::_OutputArray* binaryMap)
{
	return saliency->computeBinaryMap(*saliencyMap, *binaryMap);
}

bool cveSaliencyMotionInit(cv::saliency::Saliency* saliency)
{
	return dynamic_cast<cv::saliency::MotionSaliencyBinWangApr2014*>(saliency)->init();
}

void cveSaliencyMotionSetImageSize(cv::saliency::Saliency* saliency, int width, int height)
{
	dynamic_cast<cv::saliency::MotionSaliencyBinWangApr2014*>(saliency)->setImagesize(width, height);
}

void cveObjectnessBINGGetObjectnessValues(cv::saliency::ObjectnessBING* saliency, std::vector<float>* values)
{
	std::vector<float> vals = saliency->getobjectnessValues();

	values->reserve(vals.size());
	for (std::vector<float>::iterator it = vals.begin(); it < vals.end(); ++it)
		values->push_back(*it);
}

void cveObjectnessBINGSetTrainingPath(cv::saliency::ObjectnessBING* saliency, cv::String* trainingPath)
{
	saliency->setTrainingPath(*trainingPath);
}
/*
cv::Algorithm* cveSaliencyGetAlgorithm(cv::saliency::Saliency* saliency)
{
	return dynamic_cast<cv::Algorithm*>(saliency);
}*/