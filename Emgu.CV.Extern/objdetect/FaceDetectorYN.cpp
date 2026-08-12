//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

cv::FaceDetectorYN* cveFaceDetectorYNCreate(
    cv::String* model,
    cv::String* config,
    cv::Size* inputSize,
    float scoreThreshold,
    float nmsThreshold,
    int topK,
    int backendId,
    int targetId,
    cv::Ptr<cv::FaceDetectorYN>** sharedPtr)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
	    cv::Ptr<cv::FaceDetectorYN> ptr = cv::FaceDetectorYN::create(
	        *model, 
	        *config,
	        *inputSize,
	        scoreThreshold,
	        nmsThreshold,
	        topK,
	        backendId,
	        targetId);
	    *sharedPtr = new cv::Ptr<cv::FaceDetectorYN>(ptr);
	    return (*sharedPtr)->get();
	#else
	        throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveFaceDetectorYNRelease(cv::Ptr<cv::FaceDetectorYN>** faceDetector)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
		delete* faceDetector;
		*faceDetector = 0;
	#else 
		throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
int cveFaceDetectorYNDetect(cv::FaceDetectorYN* faceDetetor, cv::_InputArray* image, cv::_OutputArray* faces)
{
	try
	{
	#ifdef HAVE_OPENCV_OBJDETECT
	    return faceDetetor->detect(*image, *faces);
	#else 
	    throw_no_objdetect();
	#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}



