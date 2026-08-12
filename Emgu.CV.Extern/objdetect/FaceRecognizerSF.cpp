 //----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::FaceRecognizerSF* cveFaceRecognizerSFCreate(
    cv::String* model,
    cv::String* config,
    int backendId,
    int targetId,
    cv::Ptr<cv::FaceRecognizerSF>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
	    cv::Ptr<cv::FaceRecognizerSF> ptr = cv::FaceRecognizerSF::create(
	        *model,
	        *config,
	        backendId,
	        targetId);
	    *sharedPtr = new cv::Ptr<cv::FaceRecognizerSF>(ptr);
	    return (*sharedPtr)->get();
	#else
	    throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}

void cveFaceRecognizerSFRelease(cv::Ptr<cv::FaceRecognizerSF>** faceRecognizer)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
	    delete* faceRecognizer;
	    *faceRecognizer = 0;
	#else 
	    throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFaceRecognizerSFAlignCrop(cv::FaceRecognizerSF* faceRecognizer, cv::_InputArray* srcImg, cv::_InputArray* faceBox, cv::_OutputArray* alignedImg)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
	    faceRecognizer->alignCrop(*srcImg, *faceBox, *alignedImg);
	#else 
	    throw_no_objdetect();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

void cveFaceRecognizerSFFeature(cv::FaceRecognizerSF* faceRecognizer, cv::_InputArray* alignedImg, cv::_OutputArray* faceFeature)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
	    faceRecognizer->feature(*alignedImg, *faceFeature);
	#else 
	    throw_no_objdetect();
	#endif		
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}

double cveFaceRecognizerSFMatch(cv::FaceRecognizerSF* faceRecognizer, cv::_InputArray* faceFeature1, cv::_InputArray* faceFeature2, int disType)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
	    return faceRecognizer->match(*faceFeature1, *faceFeature2, disType);
	#else 
	    throw_no_objdetect();
	#endif	
	}
	CVAPI_CATCH_CV_ERRORS(0)
}





