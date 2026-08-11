//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "saliency_c.h"

cv::saliency::StaticSaliencySpectralResidual* cveStaticSaliencySpectralResidualCreate(cv::saliency::StaticSaliency** static_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm, cv::Ptr<cv::saliency::StaticSaliencySpectralResidual>** sharedPtr)
{
	try
	{
#ifdef HAVE_OPENCV_SALIENCY
		cv::Ptr<cv::saliency::StaticSaliencySpectralResidual> ptr = cv::saliency::StaticSaliencySpectralResidual::create();
		*static_saliency = static_cast<cv::saliency::StaticSaliency*>(ptr);
		*saliency = static_cast<cv::saliency::Saliency*>(ptr);
		*algorithm = static_cast<cv::Algorithm*>(ptr);
		*sharedPtr = new cv::Ptr<cv::saliency::StaticSaliencySpectralResidual>(ptr);
		return ptr.get();
#else
		throw_no_saliency();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveStaticSaliencySpectralResidualRelease(cv::saliency::StaticSaliencySpectralResidual** saliency, cv::Ptr<cv::saliency::StaticSaliencySpectralResidual>** sharedPtr)
{
#ifdef HAVE_OPENCV_SALIENCY
	delete *sharedPtr;
	*saliency = 0;
	*sharedPtr = 0;
#else
	throw_no_saliency();
#endif
}

cv::saliency::StaticSaliencyFineGrained* cveStaticSaliencyFineGrainedCreate(cv::saliency::StaticSaliency** static_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm, cv::Ptr<cv::saliency::StaticSaliencyFineGrained>** sharedPtr)
{
	try
	{
#ifdef HAVE_OPENCV_SALIENCY
		cv::Ptr<cv::saliency::StaticSaliencyFineGrained> ptr = cv::saliency::StaticSaliencyFineGrained::create();
		*static_saliency = static_cast<cv::saliency::StaticSaliency*>(ptr);
		*saliency = static_cast<cv::saliency::Saliency*>(ptr);
		*algorithm = static_cast<cv::Algorithm*>(ptr);
		*sharedPtr = new cv::Ptr<cv::saliency::StaticSaliencyFineGrained>(ptr);
		return ptr.get();
#else
		throw_no_saliency();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveStaticSaliencyFineGrainedRelease(cv::saliency::StaticSaliencyFineGrained** saliency, cv::Ptr<cv::saliency::StaticSaliencyFineGrained>** sharedPtr)
{
#ifdef HAVE_OPENCV_SALIENCY
	delete *sharedPtr;
	*saliency = 0;
	*sharedPtr = 0;
#else
	throw_no_saliency();
#endif
}

cv::saliency::MotionSaliencyBinWangApr2014* cveMotionSaliencyBinWangApr2014Create(cv::saliency::MotionSaliency** motion_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm, cv::Ptr<cv::saliency::MotionSaliencyBinWangApr2014>** sharedPtr)
{
	try
	{
#ifdef HAVE_OPENCV_SALIENCY
		cv::Ptr<cv::saliency::MotionSaliencyBinWangApr2014> ptr = cv::saliency::MotionSaliencyBinWangApr2014::create();
		*motion_saliency = static_cast<cv::saliency::MotionSaliency*>(ptr);
		*saliency = static_cast<cv::saliency::Saliency*>(ptr);
		*algorithm = static_cast<cv::Algorithm*>(ptr);
		*sharedPtr = new cv::Ptr<cv::saliency::MotionSaliencyBinWangApr2014>(ptr);
		return ptr.get();
#else
		throw_no_saliency();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveMotionSaliencyBinWangApr2014Release(cv::saliency::MotionSaliencyBinWangApr2014** saliency, cv::Ptr<cv::saliency::MotionSaliencyBinWangApr2014>** sharedPtr)
{
#ifdef HAVE_OPENCV_SALIENCY
	delete *sharedPtr;
	*saliency = 0;
	*sharedPtr = 0;
#else
	throw_no_saliency();
#endif
}

cv::saliency::ObjectnessBING* cveObjectnessBINGCreate(cv::saliency::Objectness** objectness_saliency, cv::saliency::Saliency** saliency, cv::Algorithm** algorithm, cv::Ptr<cv::saliency::ObjectnessBING>** sharedPtr)
{
	try
	{
#ifdef HAVE_OPENCV_SALIENCY
		cv::Ptr<cv::saliency::ObjectnessBING> ptr = cv::saliency::ObjectnessBING::create();
		*objectness_saliency = static_cast<cv::saliency::Objectness*>(ptr);
		*saliency = static_cast<cv::saliency::Saliency*>(ptr);
		*algorithm = static_cast<cv::Algorithm*>(ptr);
		*sharedPtr = new cv::Ptr<cv::saliency::ObjectnessBING>(ptr);
		return ptr.get();
#else
		throw_no_saliency();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveObjectnessBINGRelease(cv::saliency::ObjectnessBING** saliency, cv::Ptr<cv::saliency::ObjectnessBING>** sharedPtr)
{
#ifdef HAVE_OPENCV_SALIENCY
	delete *sharedPtr;
	*saliency = 0;
	*sharedPtr = 0;
#else
	throw_no_saliency();
#endif
}

bool cveSaliencyComputeSaliency(cv::saliency::Saliency* saliency, cv::_InputArray* image, cv::_OutputArray* saliencyMap)
{
	try
	{
	#ifdef HAVE_OPENCV_SALIENCY
		return saliency->computeSaliency(*image, *saliencyMap);
	#else
		throw_no_saliency();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveStaticSaliencyComputeBinaryMap(cv::saliency::StaticSaliency* saliency, cv::_InputArray* saliencyMap, cv::_OutputArray* binaryMap)
{
	try
	{
	#ifdef HAVE_OPENCV_SALIENCY
		return saliency->computeBinaryMap(*saliencyMap, *binaryMap);
	#else
		throw_no_saliency();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

bool cveSaliencyMotionInit(cv::saliency::Saliency* saliency)
{
	try
	{
#ifdef HAVE_OPENCV_SALIENCY
		return dynamic_cast<cv::saliency::MotionSaliencyBinWangApr2014*>(saliency)->init();
#else
		throw_no_saliency();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(false)
}

void cveSaliencyMotionSetImageSize(cv::saliency::Saliency* saliency, int width, int height)
{
	try
	{
#ifdef HAVE_OPENCV_SALIENCY
		dynamic_cast<cv::saliency::MotionSaliencyBinWangApr2014*>(saliency)->setImagesize(width, height);
#else
		throw_no_saliency();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveObjectnessBINGGetObjectnessValues(cv::saliency::ObjectnessBING* saliency, std::vector<float>* values)
{
	try
	{
#ifdef HAVE_OPENCV_SALIENCY
		std::vector<float> vals = saliency->getobjectnessValues();

		values->reserve(vals.size());
		for (std::vector<float>::iterator it = vals.begin(); it < vals.end(); ++it)
			values->push_back(*it);
#else
		throw_no_saliency();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveObjectnessBINGSetTrainingPath(cv::saliency::ObjectnessBING* saliency, cv::String* trainingPath)
{
	try
	{
#ifdef HAVE_OPENCV_SALIENCY
		saliency->setTrainingPath(*trainingPath);
#else
		throw_no_saliency();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

/*
cv::Algorithm* cveSaliencyGetAlgorithm(cv::saliency::Saliency* saliency)
{
	return dynamic_cast<cv::Algorithm*>(saliency);
}*/