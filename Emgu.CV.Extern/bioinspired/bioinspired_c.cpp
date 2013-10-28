//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "bioinspired_c.h"

//Retina
cv::bioinspired::Retina* CvRetinaCreate(CvSize inputSize, const bool colorMode, int colorSamplingMethod, const bool useRetinaLogSampling, const double reductionFactor, const double samplingStrength)
{
   cv::Ptr<cv::bioinspired::Retina> ptr = cv::bioinspired::createRetina(inputSize, colorMode, colorSamplingMethod, useRetinaLogSampling, reductionFactor, samplingStrength);
   ptr.addref();
   return ptr.get();
}
void CvRetinaRelease(cv::bioinspired::Retina** retina)
{
   delete *retina;
   *retina = 0;
}
void CvRetinaRun(cv::bioinspired::Retina* retina, IplImage* image)
{
   cv::Mat m = cv::cvarrToMat(image);
   retina->run(m);
}
void CvRetinaGetParvo(cv::bioinspired::Retina* retina, IplImage* parvo)
{
   cv::Mat m = cv::cvarrToMat(parvo);
   retina->getParvo(m);
}
void CvRetinaGetMagno(cv::bioinspired::Retina* retina, IplImage* magno)
{
   cv::Mat m = cv::cvarrToMat(magno);
   retina->getMagno(m);
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
