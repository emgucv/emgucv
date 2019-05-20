//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "bioinspired_c.h"

//Retina
cv::bioinspired::Retina* cveRetinaCreate(CvSize* inputSize, const bool colorMode, int colorSamplingMethod, const bool useRetinaLogSampling, const double reductionFactor, const double samplingStrength, cv::Ptr<cv::bioinspired::Retina>** sharedPtr)
{
   
   cv::Ptr<cv::bioinspired::Retina> ptr = cv::bioinspired::Retina::create(*inputSize, colorMode, colorSamplingMethod, useRetinaLogSampling, reductionFactor, samplingStrength);
   *sharedPtr = new cv::Ptr<cv::bioinspired::Retina>(ptr);
   return ptr.get();
}
void cveRetinaRelease( cv::Ptr<cv::bioinspired::Retina>** sharedPtr)
{
   delete *sharedPtr;
   *sharedPtr = 0;
}
void cveRetinaRun(cv::bioinspired::Retina* retina, cv::_InputArray* image)
{
   retina->run(*image);
}
void cveRetinaGetParvo(cv::bioinspired::Retina* retina, cv::_OutputArray* parvo)
{
   retina->getParvo(*parvo);
}
void cveRetinaGetMagno(cv::bioinspired::Retina* retina, cv::_OutputArray* magno)
{
   retina->getMagno(*magno);
}
void cveRetinaClearBuffers(cv::bioinspired::Retina* retina)
{
   retina->clearBuffers();
}
void cveRetinaGetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::RetinaParameters* p)
{
   cv::bioinspired::RetinaParameters result = retina->getParameters();
   memcpy(p, &result, sizeof(cv::bioinspired::RetinaParameters));
}
void cveRetinaSetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::RetinaParameters* p)
{
   retina->setup(*p);
}


//RetinaFastToneMapping
cv::bioinspired::RetinaFastToneMapping* cveRetinaFastToneMappingCreate(CvSize* inputSize, cv::Ptr<cv::bioinspired::RetinaFastToneMapping>** sharedPtr)
{
	cv::Ptr<cv::bioinspired::RetinaFastToneMapping> ptr = cv::bioinspired::RetinaFastToneMapping::create(*inputSize);
	*sharedPtr = new cv::Ptr<cv::bioinspired::RetinaFastToneMapping>(ptr);
	return (*sharedPtr)->get();
}
void cveRetinaFastToneMappingSetup(cv::bioinspired::RetinaFastToneMapping* toneMapping, float photoreceptorsNeighborhoodRadius, float ganglioncellsNeighborhoodRadius, float meanLuminanceModulatorK)
{
	toneMapping->setup(photoreceptorsNeighborhoodRadius, ganglioncellsNeighborhoodRadius, meanLuminanceModulatorK);
}
void cveRetinaFastToneMappingApplyFastToneMapping(
	cv::bioinspired::RetinaFastToneMapping* toneMapping, 
	cv::_InputArray* inputImage, 
	cv::_OutputArray* outputToneMappedImage)
{
	toneMapping->applyFastToneMapping(*inputImage, *outputToneMappedImage);
}
void cveRetinaFastToneMappingRelease(cv::Ptr<cv::bioinspired::RetinaFastToneMapping>** sharedPtr)
{
	delete *sharedPtr;
	*sharedPtr = 0;
}