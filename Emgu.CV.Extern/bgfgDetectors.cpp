#include "cvaux.h"

//Forground detector
CVAPI(CvFGDetector*) CvCreateFGDetectorBase(int type, void* param) { return cvCreateFGDetectorBase(type, param); }
CVAPI(IplImage*) CvFGDetectorGetMask(CvFGDetector* detector) { return detector->GetMask(); } 
CVAPI(void) CvFGDetectorProcess(CvFGDetector* detector, IplImage* image) { detector->Process(image); }
CVAPI(void) CvFGDetectorRelease(CvFGDetector* detector) { detector->Release(); }