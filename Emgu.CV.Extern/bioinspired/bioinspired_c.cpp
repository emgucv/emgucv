//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "bioinspired_c.h"

//Retina
cv::bioinspired::Retina* cveRetinaCreate(CvSize* inputSize, const bool colorMode, int colorSamplingMethod, const bool useRetinaLogSampling, const double reductionFactor, const double samplingStrength, cv::Ptr<cv::bioinspired::Retina>** sharedPtr)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	cv::Ptr<cv::bioinspired::Retina> ptr = cv::bioinspired::Retina::create(*inputSize, colorMode, colorSamplingMethod, useRetinaLogSampling, reductionFactor, samplingStrength);
	*sharedPtr = new cv::Ptr<cv::bioinspired::Retina>(ptr);
	return ptr.get();
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaRelease(cv::Ptr<cv::bioinspired::Retina>** sharedPtr)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaRun(cv::bioinspired::Retina* retina, cv::_InputArray* image)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	retina->run(*image);
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaGetParvo(cv::bioinspired::Retina* retina, cv::_OutputArray* parvo)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	retina->getParvo(*parvo);
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaGetMagno(cv::bioinspired::Retina* retina, cv::_OutputArray* magno)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	retina->getMagno(*magno);
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaClearBuffers(cv::bioinspired::Retina* retina)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	retina->clearBuffers();
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaGetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::RetinaParameters* p)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	cv::bioinspired::RetinaParameters result = retina->getParameters();
	memcpy(p, &result, sizeof(cv::bioinspired::RetinaParameters));
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaSetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::RetinaParameters* p)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	retina->setup(*p);
#else
	throw_no_bioinspired();
#endif
}


//RetinaFastToneMapping
cv::bioinspired::RetinaFastToneMapping* cveRetinaFastToneMappingCreate(CvSize* inputSize, cv::Ptr<cv::bioinspired::RetinaFastToneMapping>** sharedPtr)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	cv::Ptr<cv::bioinspired::RetinaFastToneMapping> ptr = cv::bioinspired::RetinaFastToneMapping::create(*inputSize);
	*sharedPtr = new cv::Ptr<cv::bioinspired::RetinaFastToneMapping>(ptr);
	return (*sharedPtr)->get();
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaFastToneMappingSetup(cv::bioinspired::RetinaFastToneMapping* toneMapping, float photoreceptorsNeighborhoodRadius, float ganglioncellsNeighborhoodRadius, float meanLuminanceModulatorK)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	toneMapping->setup(photoreceptorsNeighborhoodRadius, ganglioncellsNeighborhoodRadius, meanLuminanceModulatorK);
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaFastToneMappingApplyFastToneMapping(
	cv::bioinspired::RetinaFastToneMapping* toneMapping,
	cv::_InputArray* inputImage,
	cv::_OutputArray* outputToneMappedImage)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	toneMapping->applyFastToneMapping(*inputImage, *outputToneMappedImage);
#else
	throw_no_bioinspired();
#endif
}
void cveRetinaFastToneMappingRelease(cv::Ptr<cv::bioinspired::RetinaFastToneMapping>** sharedPtr)
{
#ifdef HAVE_OPENCV_BIOINSPIRED
	delete* sharedPtr;
	*sharedPtr = 0;
#else
	throw_no_bioinspired();
#endif
}