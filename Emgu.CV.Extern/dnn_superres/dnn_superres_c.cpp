//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "dnn_superres_c.h"

cv::dnn_superres::DnnSuperResImpl* cveDnnSuperResImplCreate()
{
#ifdef HAVE_OPENCV_DNN_SUPERRES
	return new cv::dnn_superres::DnnSuperResImpl();
#else
	throw_no_dnn_superres();
#endif
}
void cveDnnSuperResImplSetModel(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* algo, int scale)
{
#ifdef HAVE_OPENCV_DNN_SUPERRES
	dnnSuperRes->setModel(*algo, scale);
#else
	throw_no_dnn_superres();
#endif
}
void cveDnnSuperResImplReadModel1(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* path)
{
#ifdef HAVE_OPENCV_DNN_SUPERRES
	dnnSuperRes->readModel(*path);
#else
	throw_no_dnn_superres();
#endif
}
void cveDnnSuperResImplReadModel2(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, const cv::String* weights, cv::String* definition)
{
#ifdef HAVE_OPENCV_DNN_SUPERRES
	dnnSuperRes->readModel(*weights, *definition);
#else
	throw_no_dnn_superres();
#endif
}
void cveDnnSuperResImplUpsample(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, cv::_InputArray* img, cv::_OutputArray* result)
{
#ifdef HAVE_OPENCV_DNN_SUPERRES
	dnnSuperRes->upsample(*img, *result);
#else
	throw_no_dnn_superres();
#endif
}
int cveDnnSuperResImplGetScale(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes)
{
#ifdef HAVE_OPENCV_DNN_SUPERRES
	return dnnSuperRes->getScale();
#else
	throw_no_dnn_superres();
#endif
}
void cveDnnSuperResImplGetAlgorithm(cv::dnn_superres::DnnSuperResImpl* dnnSuperRes, cv::String* algorithm)
{
#ifdef HAVE_OPENCV_DNN_SUPERRES
	std::string s = dnnSuperRes->getAlgorithm();
	*algorithm = s;
#else
	throw_no_dnn_superres();
#endif
}
void cveDnnSuperResImplRelease(cv::dnn_superres::DnnSuperResImpl** dnnSuperRes)
{
#ifdef HAVE_OPENCV_DNN_SUPERRES
	delete* dnnSuperRes;
	*dnnSuperRes = 0;
#else
	throw_no_dnn_superres();
#endif
}