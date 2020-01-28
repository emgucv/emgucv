//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_objdetect_c.h"

cv::dnn_objdetect::InferBbox* cveInferBboxCreate(cv::Mat* deltaBbox, cv::Mat* classScores, cv::Mat* confScores)
{
#ifdef HAVE_OPENCV_DNN_OBJDETECT
	return new cv::dnn_objdetect::InferBbox(*deltaBbox, *classScores, *confScores);
#else
	throw_no_dnn_objdetect();
#endif	
}
void cveInferBboxFilter(cv::dnn_objdetect::InferBbox* inferBbox, double thresh)
{
#ifdef HAVE_OPENCV_DNN_OBJDETECT
	inferBbox->filter(thresh);
#else
	throw_no_dnn_objdetect();
#endif		
	
}
void cveInferBboxRelease(cv::dnn_objdetect::InferBbox** inferBbox)
{
#ifdef HAVE_OPENCV_DNN_OBJDETECT
	delete* inferBbox;
	inferBbox = 0;
#else
	throw_no_dnn_objdetect();
#endif	
}
