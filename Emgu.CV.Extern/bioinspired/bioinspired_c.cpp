//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "bioinspired_c.h"

//Retina
cv::bioinspired::Retina* CvRetinaCreate(emgu::size* inputSize, const bool colorMode, int colorSamplingMethod, const bool useRetinaLogSampling, const double reductionFactor, const double samplingStrength)
{
   cv::Size sz(inputSize->width, inputSize->height);
   cv::Ptr<cv::bioinspired::Retina> ptr = cv::bioinspired::createRetina(sz, colorMode, colorSamplingMethod, useRetinaLogSampling, reductionFactor, samplingStrength);
   ptr.addref();
   return ptr.get();
}
void CvRetinaRelease(cv::bioinspired::Retina** retina)
{
   delete *retina;
   *retina = 0;
}
void CvRetinaRun(cv::bioinspired::Retina* retina, cv::_InputArray* image)
{
   retina->run(*image);
}
void CvRetinaGetParvo(cv::bioinspired::Retina* retina, cv::_OutputArray* parvo)
{
   retina->getParvo(*parvo);
}
void CvRetinaGetMagno(cv::bioinspired::Retina* retina, cv::_OutputArray* magno)
{
   retina->getMagno(*magno);
}
void CvRetinaClearBuffers(cv::bioinspired::Retina* retina)
{
   retina->clearBuffers();
}
void CvRetinaGetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::Retina::RetinaParameters* p)
{
   cv::bioinspired::Retina::RetinaParameters result = retina->getParameters();
   memcpy(p, &result, sizeof(cv::bioinspired::Retina::RetinaParameters));
}
void CvRetinaSetParameters(cv::bioinspired::Retina* retina, cv::bioinspired::Retina::RetinaParameters* p)
{
   retina->setup(*p);
}
