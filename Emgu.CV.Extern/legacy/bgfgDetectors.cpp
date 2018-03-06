//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "legacy_c.h"

//Forground detector
CvFGDetector* CvCreateFGDetectorBase(int type, void* param) { return cvCreateFGDetectorBase(type, param); }
IplImage* CvFGDetectorGetMask(CvFGDetector* detector) { return detector->GetMask(); } 
void CvFGDetectorProcess(CvFGDetector* detector, IplImage* image) { detector->Process(image); }
void CvFGDetectorRelease(CvFGDetector* detector) { detector->Release(); }
