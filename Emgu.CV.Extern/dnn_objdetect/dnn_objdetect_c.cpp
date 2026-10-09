//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_objdetect_c.h"

cv::dnn_objdetect::InferBbox* cveInferBboxCreate(cv::Mat* deltaBbox, cv::Mat* classScores, cv::Mat* confScores)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_OBJDETECT
		return new cv::dnn_objdetect::InferBbox(*deltaBbox, *classScores, *confScores);
#else
		throw_no_dnn_objdetect();
#endif
	}
	CVAPI_CATCH_CV_ERRORS(0)
}
void cveInferBboxFilter(cv::dnn_objdetect::InferBbox* inferBbox, double thresh)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_OBJDETECT
		inferBbox->filter(thresh);
#else
		throw_no_dnn_objdetect();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
void cveInferBboxRelease(cv::dnn_objdetect::InferBbox** inferBbox)
{
	try
	{
#ifdef HAVE_OPENCV_DNN_OBJDETECT
		delete* inferBbox;
		inferBbox = 0;
#else
		throw_no_dnn_objdetect();
#endif
	}
	CVAPI_CATCH_CV_ERRORS_VOID
}
